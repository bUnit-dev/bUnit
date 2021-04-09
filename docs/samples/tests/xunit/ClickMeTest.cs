using Microsoft.AspNetCore.Components.Web;
using Xunit;
using Bunit;

namespace Bunit.Docs.Samples
{
  public class ClickMeTest
  {
    [Fact]
    public void Test()
    {
      // Arrange
      using var ctx = new TestContext();
      var cut = ctx.RenderComponent<ClickMe>();
      var buttonElement = cut.Find("button");

      // Act
      buttonElement.Click();
      buttonElement.Click(detail: 3, ctrlKey: true);
      buttonElement.Click(new MouseEventArgs());

      // Assert
      // ...
    }
  }
}
