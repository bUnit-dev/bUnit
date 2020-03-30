using System;
using System.Collections.Generic;
using AngleSharp.Diffing.Core;
using AngleSharp.Dom;
using Bunit.Diffing;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;

namespace Bunit
{
	/// <summary>
	/// Represents an abstract <see cref="IRenderedFragment"/> with base functionality.
	/// </summary>
	public abstract class RenderedFragmentBase : IRenderedFragment
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
		protected abstract string FirstRenderMarkup { get; }

		/// <summary>
		/// Gets the container that handles the (re)rendering of the fragment.
		/// </summary>
		protected ContainerComponent Container { get; }

		/// <inheritdoc/>
		public IServiceProvider Services { get; }

		/// <inheritdoc/>
		public abstract int ComponentId { get; }

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
		/// Creates an instance of the <see cref="RenderedFragmentBase"/> class.
		/// </summary>
		protected RenderedFragmentBase(IServiceProvider services, RenderFragment renderFragment) : this(services, container: null)
		{
			Container.Render(renderFragment);
		}

		/// <summary>
		/// Creates an instance of the <see cref="RenderedFragmentBase"/> class.
		/// </summary>
		protected RenderedFragmentBase(IServiceProvider services, ContainerComponent? container = null)
		{
			if (services is null)
				throw new ArgumentNullException(nameof(services));

			HtmlParser = services.GetRequiredService<TestHtmlParser>();
			Renderer = services.GetRequiredService<TestRenderer>();
			Services = services;
			Container = container ?? new ContainerComponent(Renderer);
			RenderEvents = new RenderEventFilter(Renderer.RenderEvents, RenderFilter);
			_renderEventSubscriber = new ConcurrentRenderEventSubscriber(Renderer.RenderEvents, ComponentRendered);
		}

		/// <inheritdoc/>
		public IRenderedComponent<T> FindComponent<T>() where T : class, IComponent
		{
			var (id, component) = Container.GetComponent<T>();
			return new RenderedComponent<T>(Services, Container, id, component);
		}

		/// <inheritdoc/>
		public IReadOnlyList<IRenderedComponent<T>> FindComponents<T>() where T : class, IComponent
		{
			var result = new List<IRenderedComponent<T>>();
			foreach (var (id, component) in Container.GetComponents<T>())
			{
				result.Add(new RenderedComponent<T>(Services, Container, id, component));
			}
			return result;
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
			=> renderEvent.DidComponentRender(this);

		private void ComponentRendered(RenderEvent renderEvent)
		{
			if (renderEvent.HasChangesTo(this))
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
