using Bunit;
using Xunit;

namespace Bunit.Docs.Samples;

public class HelloWorldExplicitContext
{
  [Fact]
  public void HelloWorldComponentRendersCorrectly()
  {
    // Arrange
    using var ctx = new TestContext();

    // Act
    var cut = ctx.RenderComponent<HelloWorld>();

    // Assert
    cut.MarkupMatches("<h1>Hello world from Blazor</h1>");
  }
}