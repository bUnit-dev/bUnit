using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Egil.RazorComponents.Testing.SampleApp.Components;
using Xunit;
using Shouldly;

namespace Egil.RazorComponents.Testing.SampleApp.CodeOnlyTests.Components
{
    public class CascadingValueTest : ComponentTestFixture
    {
        [Fact(DisplayName = "Themed button uses provided theme info to set class attribute")]
        public void Test001()
        {
            // arrange
            var theme = new ThemeInfo() { Value = "BUTTON" };

            // act
            var cut = RenderComponent<ThemedButton>(
                CascadingValue(theme)
            );

            // assert
            cut.Find("button").ClassList.ShouldContain(theme.Value);
        }

        [Fact(DisplayName = "Named cascading values are passed to components")]
        public void Test002()
        {
            // arrange
            var classTheme = new ThemeInfo() { Value = "FOO" };
            var titleTheme = new ThemeInfo() { Value = "BAR" };

            // act
            var cut = RenderComponent<ThemedElement>(
                CascadingValue(nameof(ThemedElement.Class), classTheme),
                CascadingValue(nameof(ThemedElement.Title), titleTheme)
            );

            // assert
            var elm = cut.Find("div");
            elm.ClassList.ShouldContain(classTheme.Value);
            elm.GetAttribute("title").ShouldContain(titleTheme.Value);
        }
    }
}
