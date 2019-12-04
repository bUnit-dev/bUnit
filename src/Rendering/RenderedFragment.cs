using System;
using System.Collections.Generic;
using System.Linq;
using AngleSharp.Diffing.Core;
using AngleSharp.Dom;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Egil.RazorComponents.Testing
{
    public class RenderedFragment : IRenderedFragment
    {
        private readonly RenderFragment _renderFragment;
        private readonly string _firstRenderMarkup;
        private string? _lastSnapshotMarkup;

        protected int ComponentId { get; }
        protected ContainerComponent Container { get; }

        public TestHost TestContext { get; }

        internal RenderedFragment(TestHost testContext, RenderFragment renderFragment)
        {
            TestContext = testContext;
            _renderFragment = renderFragment;
            Container = new ContainerComponent(testContext.Renderer);
            ComponentId = Container.ComponentId;
            Container.RenderComponentUnderTest(_renderFragment);
            _firstRenderMarkup = GetMarkup();
        }

        public void TakeSnapshot() => _lastSnapshotMarkup = GetMarkup();

        public IReadOnlyList<IDiff> GetChangesSinceFirstRender() => this.CompareTo(_firstRenderMarkup);

        public IReadOnlyList<IDiff> GetChangesSinceSnapshot()
        {
            if (_lastSnapshotMarkup is null) 
                throw new InvalidOperationException($"No snapshot exists to compare with. Call {nameof(TakeSnapshot)} to create one.");
            return this.CompareTo(_lastSnapshotMarkup);
        }

        public string GetMarkup() => Htmlizer.GetHtml(TestContext.Renderer, ComponentId);

        public INodeList GetNodes() => TestContext.HtmlParser.Parse(GetMarkup());
    }
}
