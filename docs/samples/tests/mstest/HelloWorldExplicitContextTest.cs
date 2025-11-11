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
    using var ctx = new Bunit.BunitContext();

    // Act
    var cut = ctx.Render<HelloWorld>();

    // Assert
    cut.MarkupMatches("<h1>Hello world from Blazor</h1>");
  }
}