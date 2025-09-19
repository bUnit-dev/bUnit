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
    buttonElement.Click(new MouseEventArgs { Detail = 3, CtrlKey = true });
    buttonElement.Click(new MouseEventArgs());

    // Assert
    // ...
  }
}