namespace Bunit.Docs.Samples;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Bunit;

[TestClass]
public class HelloWorldTest : BunitTestContext
{
  [TestMethod]
  public void HelloWorldComponentRendersCorrectly()
  {
    // Act
    var cut = RenderComponent<HelloWorld>();

    // Assert
    cut.MarkupMatches("<h1>Hello world from Blazor</h1>");
  }
}