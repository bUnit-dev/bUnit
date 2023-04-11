using Microsoft.VisualStudio.TestTools.UnitTesting;
using Bunit;

namespace Bunit.Docs.Samples;

[TestClass]
public class HelloWorldExplicitContext
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