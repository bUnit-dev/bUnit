using Bunit;
using NUnit.Framework;

namespace Docs.Components.NUnit
{
  public class HelloWorldTest
  {
    [Test]
    public void HelloWorldComponentRendersCorrectly()
    {
      // Arrange
      using var ctx = new Bunit.TestContext();

      // Act
      var cut = ctx.RenderComponent<HelloWorld>();

      // Assert
      cut.MarkupMatches("<h1>Hello world from Blazor</h1>");
    }
  }
}