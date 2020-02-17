using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AngleSharp.Diffing.Core;
using AngleSharp.Dom;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.RenderTree;

namespace Bunit
{
    /// <summary>
    /// Represents an abstract <see cref="IRenderedFragment"/> with base functionality.
    /// </summary>
    public abstract class RenderedFragmentBase : IRenderedFragment
    {
        private readonly RenderEventSubscriber _renderEventSubscriber;
        private string? _snapshotMarkup;
        private string? _latestRenderMarkup;
        private INodeList? _firstRenderNodes;
        private INodeList? _latestRenderNodes;
        private INodeList? _snapshotNodes;

        /// <summary>
        /// Gets the first rendered markup.
        /// </summary>
        protected abstract string FirstRenderMarkup { get; }

        /// <summary>
        /// Gets the container that handles the (re)rendering of the fragment.
        /// </summary>
        protected ContainerComponent Container { get; }

        /// <inheritdoc/>
        public ITestContext TestContext { get; }

        /// <inheritdoc/>
        public abstract int ComponentId { get; }

        /// <inheritdoc/>
        public string Markup
        {
            get
            {
                if (_latestRenderMarkup is null)
                    _latestRenderMarkup = Htmlizer.GetHtml(TestContext.Renderer, ComponentId);
                return _latestRenderMarkup;
            }
        }

        /// <inheritdoc/>
        public INodeList Nodes
        {
            get
            {
                if (_latestRenderNodes is null)
                    _latestRenderNodes = TestContext.CreateNodes(Markup);
                return _latestRenderNodes;
            }
        }

        /// <inheritdoc/>
        public IObservable<RenderEvent> RenderEvents { get; }

        /// <summary>
        /// Creates an instance of the <see cref="RenderedFragmentBase"/> class.
        /// </summary>
        protected RenderedFragmentBase(ITestContext testContext, RenderFragment renderFragment)
            : this(testContext, testContext is { } ctx ? new ContainerComponent(ctx.Renderer) : throw new ArgumentNullException(nameof(testContext)))
        {
            Container.Render(renderFragment);
        }

        /// <summary>
        /// Creates an instance of the <see cref="RenderedFragmentBase"/> class.
        /// </summary>
        protected RenderedFragmentBase(ITestContext testContext, ContainerComponent container)
        {
            if (testContext is null) throw new ArgumentNullException(nameof(testContext));
            if (container is null) throw new ArgumentNullException(nameof(container));

            TestContext = testContext;
            Container = container;
            RenderEvents = new RenderEventFilter(testContext.Renderer.RenderEvents, RenderFilter);
            _renderEventSubscriber = new RenderEventSubscriber(testContext.Renderer.RenderEvents, ComponentRendered);
        }

        /// <inheritdoc/>
        public IRenderedComponent<T> FindComponent<T>() where T : class, IComponent
        {
            var (id, component) = Container.GetComponent<T>();
            return new RenderedComponent<T>(TestContext, Container, id, component);
        }

        /// <inheritdoc/>
        public IReadOnlyList<IRenderedComponent<T>> FindComponents<T>() where T : class, IComponent
        {
            var result = new List<IRenderedComponent<T>>();
            foreach (var (id, component) in Container.GetComponents<T>())
            {
                result.Add(new RenderedComponent<T>(TestContext, Container, id, component));
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
                _snapshotNodes = TestContext.CreateNodes(_snapshotMarkup);

            return Nodes.CompareTo(_snapshotNodes);
        }

        /// <inheritdoc/>
        public IReadOnlyList<IDiff> GetChangesSinceFirstRender()
        {
            if (_firstRenderNodes is null)
                _firstRenderNodes = TestContext.CreateNodes(FirstRenderMarkup);
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
