using Bunit;
using NUnit.Framework;

namespace Bunit.Docs.Samples;

public class HelloWorldExplicitContext
{
  [Test]
  public void HelloWorldComponentRendersCorrectly()
  {
    // Arrange
    using var ctx = new Bunit.TestContext();

    // Act
    var cut = ctx.Render<HelloWorld>();

    // Assert
    cut.MarkupMatches("<h1>Hello world from Blazor</h1>");
  }
}