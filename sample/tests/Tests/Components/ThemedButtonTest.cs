using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Egil.RazorComponents.Testing.Asserting;
using Egil.RazorComponents.Testing.EventDispatchExtensions;
using Egil.RazorComponents.Testing.SampleApp.Components;
using Xunit;
using Shouldly;
using Microsoft.AspNetCore.Components.Web;

namespace Egil.RazorComponents.Testing.SampleApp.CodeOnlyTests.Components
{
    public class ThemedButtonTest : ComponentTestFixture
    {
        [Fact(DisplayName = "When button is clicked, the OnClick event callback is triggered")]
        public void Test001()
        {
            var wasCalled = false;
            // Arrange - pass a lambda in as parameter to the OnClick parameter.
            //
            // This is equivalent to the follow Razor code:
            //
            // <ThemedButton OnClick="(_) => wasCalled = true"></ThemedButton>
            var cut = RenderComponent<ThemedButton>(
                EventCallback(nameof(ThemedButton.OnClick), (MouseEventArgs _) => wasCalled = true)
            );

            // Act - click the button in CUT
            cut.Find("button").Click();

            // Assert - check if callback was triggered
            wasCalled.ShouldBeTrue();
        }

        [Fact(DisplayName = "Themed button uses provided theme info to set class attribute")]
        public void Test002()
        {
            // Arrange - create an instance of the ThemeInfo class to passs to the ThemedButton
            var theme = new ThemeInfo() { Value = "BUTTON" };

            // Act - Render the ThemedButton component, passing in the instance of ThemeInfo
            // as an _unnamed_ cascading value.
            //
            // This is equivalent to the follow Razor code:
            //
            // <CascadingValue Value="theme">
            //     <ThemedButton></ThemedButton>
            // </CascadingValue>
            var cut = RenderComponent<ThemedButton>(
                CascadingValue(theme)
            );

            // Assert - check that the class specified in the cascading value was indeed used.
            cut.Find("button").ClassList.ShouldContain(theme.Value);
        }

        [Fact(DisplayName = "Named cascading values are passed to components")]
        public void Test003()
        {
            // Arrange - create two instances of the ThemeInfo class to passs to the ThemedButton
            var theme = new ThemeInfo() { Value = "BUTTON" };
            var titleTheme = new ThemeInfo() { Value = "BAR" };

            // Act - Render the ThemedButton component, passing in the instances of ThemeInfo
            // as an _unnamed_ and a _named_ cascading value.
            //
            // This is equivalent to the follow Razor code:
            //
            // <CascadingValue Value="theme">
            //     <CascadingValue Name="Title" Value="titleTheme">
            //         <ThemedButton></ThemedButton>
            //     </CascadingValue>
            // </CascadingValue>
            var cut = RenderComponent<ThemedButton>(
                CascadingValue(theme),
                CascadingValue(nameof(ThemedButton.Title), titleTheme)
            );

            // Assert - check that the class and title specified in the cascading values was indeed used.
            var elm = cut.Find("button");
            elm.ClassList.ShouldContain(theme.Value);
            elm.GetAttribute("title").ShouldContain(titleTheme.Value);
        }
    }
}
