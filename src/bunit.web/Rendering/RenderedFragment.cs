using System;
using System.Collections.Generic;
using AngleSharp.Diffing.Core;
using AngleSharp.Dom;
using Bunit.Diffing;
using Bunit.Rendering;
using Bunit.Rendering.RenderEvents;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;

namespace Bunit
{
	/// <summary>
	/// Represents an abstract <see cref="IRenderedFragment"/> with base functionality.
	/// </summary>
	public class RenderedFragment : IWebRenderedFragment
	{
		private readonly ConcurrentRenderEventSubscriber _renderEventSubscriber;
		private string? _snapshotMarkup;
		private string? _latestRenderMarkup;
		private INodeList? _firstRenderNodes;
		private INodeList? _latestRenderNodes;
		private INodeList? _snapshotNodes;

		private TestHtmlParser HtmlParser { get; }

		/// <summary>
		/// Gets the renderer used to render the <see cref="IRenderedFragment"/>.
		/// </summary>
		protected TestRenderer Renderer { get; }

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
				if (_latestRenderMarkup is null)
					_latestRenderMarkup = Htmlizer.GetHtml(Renderer, ComponentId);
				return _latestRenderMarkup;
			}
		}

		/// <inheritdoc/>
		public INodeList Nodes
		{
			get
			{
				if (_latestRenderNodes is null)
					_latestRenderNodes = HtmlParser.Parse(Markup);
				return _latestRenderNodes;
			}
		}

		/// <inheritdoc/>
		public IObservable<RenderEvent> RenderEvents { get; }

		/// <summary>
		/// Creates an instance of the <see cref="RenderedFragment"/> class.
		/// </summary>
		public RenderedFragment(IServiceProvider services, int componentId)
		{
			if (services is null)
				throw new ArgumentNullException(nameof(services));

			Services = services;
			HtmlParser = services.GetRequiredService<TestHtmlParser>();
			Renderer = services.GetRequiredService<TestRenderer>();
			ComponentId = componentId;
			RenderEvents = new RenderEventFilter(Renderer.RenderEvents, RenderFilter);
			_renderEventSubscriber = new ConcurrentRenderEventSubscriber(Renderer.RenderEvents, ComponentRendered);
			FirstRenderMarkup = Markup;
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

		private bool RenderFilter(RenderEvent renderEvent)
			=> renderEvent.DidComponentRender(this.ComponentId);

		private void ComponentRendered(RenderEvent renderEvent)
		{
			if (renderEvent.HasChangesTo(this.ComponentId))
			{
				ResetLatestRenderCache();
			}
		}

		private void ResetLatestRenderCache()
		{
			_latestRenderMarkup = null;
			_latestRenderNodes = null;
		}
	}
}
