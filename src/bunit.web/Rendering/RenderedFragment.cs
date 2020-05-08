using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using AngleSharp.Diffing.Core;
using AngleSharp.Dom;

using Bunit.Diffing;
using Bunit.Extensions;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Bunit.Rendering
{
	/// <summary>
	/// Represents an abstract <see cref="IRenderedFragment"/> with base functionality.
	/// </summary>
	public class RenderedFragment : IRenderedFragment, IRenderEventHandler
	{
		private readonly object _lock = new object();
		private readonly ILogger<RenderedFragment> _logger;
		private string? _snapshotMarkup;
		private INodeList? _firstRenderNodes;
		private INodeList? _latestRenderNodes;
		private INodeList? _snapshotNodes;
		private string _markup;

		private HtmlParser HtmlParser { get; }

		/// <summary>
		/// Gets the renderer used to render the <see cref="IRenderedFragmentBase"/>.
		/// </summary>
		protected ITestRenderer Renderer { get; }

		/// <summary>
		/// Gets the first rendered markup.
		/// </summary>
		protected string FirstRenderMarkup { get; }

		/// <inheritdoc/>
		public IServiceProvider Services { get; }

		/// <inheritdoc/>
		public int ComponentId { get; }

		/// <inheritdoc/>
		public string Markup
		{
			get
			{
				// The lock ensures that we cannot read the _markup and _latestRenderNodes
				// field while it is being updated
				lock (_lock)
				{
					return Volatile.Read(ref _markup);
				}
			}
		}

		/// <inheritdoc/>
		public INodeList Nodes
		{
			get
			{
				// The lock ensures that latest nodes is always based on the latest rendered markup.
				lock (_lock)
				{
					var result = Volatile.Read(ref _latestRenderNodes);
					if (result is null)
					{
						result = HtmlParser.Parse(Markup);
						Volatile.Write(ref _latestRenderNodes, result);
					}
					return result;
				}
			}
		}

		/// <inheritdoc/>
		public event Action? OnMarkupUpdated;

		/// <inheritdoc/>
		public event Action? OnAfterRender;

		/// <inheritdoc/>
		public int RenderCount { get; private set; }

		/// <summary>
		/// Creates an instance of the <see cref="RenderedFragment"/> class.
		/// </summary>
		public RenderedFragment(IServiceProvider services, int componentId)
		{
			if (services is null)
				throw new ArgumentNullException(nameof(services));

			_logger = services.CreateLogger<RenderedFragment>();
			HtmlParser = services.GetRequiredService<HtmlParser>();
			Renderer = services.GetRequiredService<ITestRenderer>();
			Services = services;
			ComponentId = componentId;
			_markup = RetrieveLatestMarkupFromRenderer();
			FirstRenderMarkup = _markup;
			Renderer.AddRenderEventHandler(this);
			RenderCount = 1;
		}

		/// <inheritdoc/>
		public void SaveSnapshot()
		{
			_snapshotNodes = null;
			_snapshotMarkup = Markup;
		}

		/// <inheritdoc/>
		public IReadOnlyList<IDiff> GetChangesSinceSnapshot()
		{
			if (_snapshotMarkup is null)
				throw new InvalidOperationException($"No snapshot exists to compare with. Call {nameof(SaveSnapshot)} to create one.");

			if (_snapshotNodes is null)
				_snapshotNodes = HtmlParser.Parse(_snapshotMarkup);

			return Nodes.CompareTo(_snapshotNodes);
		}

		/// <inheritdoc/>
		public IReadOnlyList<IDiff> GetChangesSinceFirstRender()
		{
			if (_firstRenderNodes is null)
				_firstRenderNodes = HtmlParser.Parse(FirstRenderMarkup);
			return Nodes.CompareTo(_firstRenderNodes);
		}

		private string RetrieveLatestMarkupFromRenderer() => Htmlizer.GetHtml(Renderer, ComponentId);

		Task IRenderEventHandler.Handle(RenderEvent renderEvent)
		{
			HandleComponentRender(renderEvent);
			return Task.CompletedTask;
		}

		private void HandleComponentRender(RenderEvent renderEvent)
		{
			if (renderEvent.DidComponentRender(ComponentId))
			{
				_logger.LogDebug(new EventId(1, nameof(HandleComponentRender)), $"Received a new render where component {ComponentId} did render.");

				RenderCount++;

				// First notify derived types, e.g. queried AngleSharp collections or elements
				// that the markup has changed and they should rerun their queries.
				HandleChangesToMarkup(renderEvent);

				// Then it is safe to tell anybody waiting on updates or changes to the rendered fragment
				// that they can redo their assertions or continue processing.
				OnAfterRender?.Invoke();
			}
		}

		private void HandleChangesToMarkup(RenderEvent renderEvent)
		{
			if (renderEvent.HasMarkupChanges(ComponentId))
			{
				_logger.LogDebug(new EventId(1, nameof(HandleChangesToMarkup)), $"Received a new render where the markup of component {ComponentId} changed.");

				// The lock ensures that latest nodes is always based on the latest rendered markup.
				lock (_lock)
				{
					Volatile.Write(ref _latestRenderNodes, null);
					Volatile.Write(ref _markup, RetrieveLatestMarkupFromRenderer());
				}

				OnMarkupUpdated?.Invoke();
			}
			else if (renderEvent.DidComponentDispose(ComponentId))
			{
				_logger.LogDebug(new EventId(1, nameof(HandleChangesToMarkup)), $"Received a new render where the component {ComponentId} was disposed.");
				Renderer.RemoveRenderEventHandler(this);
			}
		}
	}
}
