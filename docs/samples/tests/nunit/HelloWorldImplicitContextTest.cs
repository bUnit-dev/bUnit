using Bunit;
using NUnit.Framework;

namespace Bunit.Docs.Samples
{
  public class HelloHelloWorldImplicitContextTest : BunitTestContext
  {
    [Test]
    public void HelloWorldComponentRendersCorrectly()
    {
      // Act
      var cut = RenderComponent<HelloWorld>();

      // Assert
      cut.MarkupMatches("<h1>Hello world from Blazor</h1>");
    }
  }
}