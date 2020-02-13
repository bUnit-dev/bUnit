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
        /// Gets the id of the rendered component or fragment.
        /// </summary>
        protected abstract int ComponentId { get; }

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

        /// <summary>
        /// Creates an instance of the <see cref="RenderedFragmentBase"/> class.
        /// </summary>
        public RenderedFragmentBase(ITestContext testContext, RenderFragment renderFragment)
        {
            if (testContext is null) throw new ArgumentNullException(nameof(testContext));

            TestContext = testContext;
            Container = new ContainerComponent(testContext.Renderer);
            Container.Render(renderFragment);
            _renderEventSubscriber = new RenderEventSubscriber(testContext.Renderer.RenderEvents);
            _renderEventSubscriber.OnRender = ComponentMarkupChanged;
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

        private void ComponentMarkupChanged(RenderEvent renderBatch)
        {
            if (renderBatch.RenderBatch.HasUpdatesTo(ComponentId) || HasChildComponentUpdated(renderBatch.RenderBatch, ComponentId))
            {
                ResetLatestRenderCache();
            }
        }

        private bool HasChildComponentUpdated(in RenderBatch renderBatch, int componentId)
        {
            var frames = TestContext.Renderer.GetCurrentRenderTreeFrames(componentId);

            for (int i = 0; i < frames.Count; i++)
            {
                var frame = frames.Array[i];
                if (frame.FrameType == RenderTreeFrameType.Component)
                {
                    if (renderBatch.HasUpdatesTo(frame.ComponentId))
                    {
                        return true;
                    }
                    if (HasChildComponentUpdated(in renderBatch, frame.ComponentId))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private void ResetLatestRenderCache()
        {
            _latestRenderMarkup = null;
            _latestRenderNodes = null;
        }
    }
}
