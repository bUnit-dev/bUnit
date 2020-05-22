using Bunit;
using NUnit.Framework;

namespace Docs.Components.NUnit
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