using System;
using System.Collections.Generic;
using System.Linq;
using AngleSharp.Diffing.Core;
using AngleSharp.Dom;
using Egil.RazorComponents.Testing.Asserting;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Egil.RazorComponents.Testing
{
    /// <summary>
    /// Represents a rendered fragment.
    /// </summary>
    public class RenderedFragment : IRenderedFragment
    {
        private readonly RenderFragment _renderFragment;
        private readonly string _firstRenderMarkup;
        private string? _lastSnapshotMarkup;

        /// <summary>
        /// Gets and sets the id of the fragment being rendered.
        /// </summary>
        protected int ComponentId { get; set; }
        
        /// <summary>
        /// Gets the container that handles the (re)rendering of the fragment.
        /// </summary>
        protected ContainerComponent Container { get; }

        /// <inheritdoc/>
        public ITestContext TestContext { get; }

        internal RenderedFragment(TestContext testContext, RenderFragment renderFragment)
        {
            TestContext = testContext;
            _renderFragment = renderFragment;
            Container = new ContainerComponent(testContext.Renderer);
            ComponentId = Container.ComponentId;
            Container.RenderComponentUnderTest(_renderFragment);
            _firstRenderMarkup = GetMarkup();
        }

        /// <inheritdoc/>
        public void TakeSnapshot() => _lastSnapshotMarkup = GetMarkup();

        /// <inheritdoc/>
        public IReadOnlyList<IDiff> GetChangesSinceFirstRender() => this.CompareTo(_firstRenderMarkup);

        /// <inheritdoc/>
        public IReadOnlyList<IDiff> GetChangesSinceSnapshot()
        {
            if (_lastSnapshotMarkup is null)
                throw new InvalidOperationException($"No snapshot exists to compare with. Call {nameof(TakeSnapshot)} to create one.");
            return this.CompareTo(_lastSnapshotMarkup);
        }

        /// <inheritdoc/>
        public string GetMarkup() => Htmlizer.GetHtml(TestContext.Renderer, ComponentId);

        /// <inheritdoc/>
        public INodeList GetNodes() => TestContext.HtmlParser.Parse(GetMarkup());
    }
}
