using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Egil.RazorComponents.Testing.SampleApp.Components;
using Shouldly;
using Xunit;

namespace Egil.RazorComponents.Testing.SampleApp.CodeOnlyTests.Components
{
    public class WikiSearchTest : ComponentTestFixture
    {
        [Fact(DisplayName = "On first render WikiSearch uses JSInterop to query wiki and display the result")]
        public void Test001()
        {
            var expectedSearchResult = "SEARCH RESULT";
            var jsMock = Services.AddMockJsRuntime(JsRuntimeMockMode.Strict);
            var plannedInvocation = jsMock.Setup<string>("queryWiki", "blazor");
            
            var cut = RenderComponent<WikiSearch>();
            cut.Find("p").InnerHtml.ShouldBeEmpty();

            WaitForNextRender(() => plannedInvocation.SetResult(expectedSearchResult));

            cut.Find("p").InnerHtml.ShouldBe(expectedSearchResult);
        }
    }
}
