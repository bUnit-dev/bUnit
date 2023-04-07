namespace Bunit.Docs.Samples;

using Xunit;
using Bunit;

public class HelloWorldTest : TestContext
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