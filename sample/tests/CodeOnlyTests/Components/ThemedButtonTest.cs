using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Egil.RazorComponents.Testing.Library.SampleApp.Components;
using Xunit;
using Shouldly;

namespace Egil.RazorComponents.Testing.Library.SampleApp.CodeOnlyTests.Components
{
    public class ThemedButtonTest : TestContext
    {
        [Fact(DisplayName = "Themed button uses provided theme info to set class attribute")]
        public void Test001()
        {
            // arrange
            var theme = new ThemeInfo() { Class = "BUTTON" };

            var cut = RenderComponent<ThemedButton>(
                (nameof(ThemedButton.Theme), theme, true),
                ("id", "testid")
            );

            cut.Find("button").ClassList.ShouldContain(theme.Class);
        }
    }
}
