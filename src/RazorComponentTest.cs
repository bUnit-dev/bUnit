using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Egil.RazorComponents.Testing.Rendering;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.RenderTree;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Egil.RazorComponents.Testing
{
    [SuppressMessage("Design", "CA1063:Implement IDisposable Correctly", Justification = "<Pending>")]
    public abstract class RazorComponentTest : IDisposable // : IClassFixture<RazorComponentFixture>
    {
        private readonly RazerComponentTestRenderer _renderer;

        public IReadOnlyList<TestRenderResult> RenderResults { get; private set; }

        public RazorComponentTest()
        {
            // TODO: Get this injected by xunit once for all tests in class. 
            // Doesnt currently work as there have to be a parameter less constructor
            // for razor file generator to work, and if there are multiple, xunit
            // picks the first without parameters...
            _renderer = new RazerComponentTestRenderer();
            Render();
            RenderResults = _renderer.RenderResults;
        }

        public void Render()
        {
            var services = new ServiceCollection();
            AddServices(services);
            _renderer.Render(BuildRenderTree, services);
            RenderResults = _renderer.RenderResults;
        }

        [SuppressMessage("Usage", "CA1816:Dispose methods should call SuppressFinalize", Justification = "<Pending>")]
        public void Dispose()
        {
            _renderer.Dispose();
        }

        [Fact(DisplayName = "Rendered HTML should be the same as expected HTML")]
        public virtual void DefaultTestRenderedHtmlShouldBeExpctedHtml()
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