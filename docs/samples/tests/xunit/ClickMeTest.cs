using Microsoft.AspNetCore.Components.Web;
using Xunit;
using Bunit;

namespace Bunit.Docs.Samples;

public class ClickMeTest : BunitContext
{
  [Fact]
  public void Test()
  {
    // Arrange
    var cut = Render<ClickMe>();
    var buttonElement = cut.Find("button");

    // Act
    buttonElement.Click();
    buttonElement.Click(detail: 3, ctrlKey: true);
    buttonElement.Click(new MouseEventArgs());

    // Assert
    // ...
  }
}