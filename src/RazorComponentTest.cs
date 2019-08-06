﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Microsoft.AspNetCore.Components.RenderTree;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit;

namespace Egil.RazorComponents.Testing
{
    public abstract class RazorComponentTest : IDisposable // : IClassFixture<RazorComponentFixture>
    {
        private RazerComponentTestRenderer _renderer;

        public IReadOnlyList<TestRenderResult> RenderResults { get; private set; }

        public RazorComponentTest()
        {
            // TODO: Get this injected by xunit once for all tests in class. 
            // Doesnt currently work as there have to be a parameter less constructor
            // for razor file generator to work, and if there are multiple, xunit
            // picks the first without parameters...
            _renderer = new RazerComponentTestRenderer();
            Render();
        }

        public void Render()
        {
            var services = new ServiceCollection();
            AddServices(services);
            _renderer.Render(BuildRenderTree, services);
            RenderResults = _renderer.RenderResults;
        }

        public void Dispose()
        {
            _renderer.Dispose();
        }

        [Fact(DisplayName = "Rendered HTML should be the same as expected HTML")]
        public virtual void DefaultTest_RenderedHtml_Should_Be_ExpctedHtml()
        {
            foreach (var result in RenderResults)
            {
                if (result.ExpectedHtml is null) continue;
                result.RenderedHtml.ShouldBe(result.ExpectedHtml);
            }
        }

        protected virtual void AddServices(IServiceCollection services) { }

        protected virtual void BuildRenderTree(RenderTreeBuilder builder) { }
    }
}