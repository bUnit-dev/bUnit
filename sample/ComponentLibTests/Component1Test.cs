using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Egil.RazorComponents.Testing;
using Xunit;

namespace ComponentLib
{
    public class Component1Test : RazorComponentFixture
    {
        [Fact]
        public void Component1Test_1()
        {
            var expectedHtml = @"<div class=""my-component"">
                                     This Blazor component is defined in the <strong>ComponentLib</strong> package.
                                 </div>
                                 ";

            var result = Component<Component1>().Render();

            result.ShouldBe(expectedHtml);
        }

        [Fact]
        public void Component1Test_1_FAILS()
        {
            var expectedHtml = @"<hr />";

            var result = Component<Component1>().Render();

            result.ShouldBe(expectedHtml);
        }
    }
}
