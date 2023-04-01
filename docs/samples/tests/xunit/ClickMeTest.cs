using Microsoft.AspNetCore.Components.Web;
using Xunit;
using Bunit;

namespace Bunit.Docs.Samples;

public class ClickMeTest : TestContext
{
    [Fact]
    public void Test()
    {
        // Arrange
        var cut = RenderComponent<ClickMe>();
        var buttonElement = cut.Find("button");

        // Act
        buttonElement.Click();
        buttonElement.Click(detail: 3, ctrlKey: true);
        buttonElement.Click(new MouseEventArgs());

        // Assert
        // ...
    }
}