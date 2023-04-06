using Xunit;
using Bunit;

namespace Bunit.Docs.Samples;

public class HelloWorldImplicitContextTest : TestContext
{
  [Fact]
  public void HelloWorldComponentRendersCorrectly()
  {
    // Act
    var cut = RenderComponent<HelloWorld>();

    // Assert
    cut.MarkupMatches("<h1>Hello world from Blazor</h1>");
  }
}