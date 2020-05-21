using Bunit;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Docs.Components.MSTest
{
  [TestClass]
  public class HelloWorldTest
  {
    [TestMethod]
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