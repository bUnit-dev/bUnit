using System;
using Xunit;
using Bunit;

namespace Company.RazorTests1
{
  public class Component1Test : ComponentTestFixture
  {
    [Fact]
    public void Component1RendersCorrectly()
    {
      var cut = RenderComponent<Component1>();

      cut.MarkupMatches(@"<div class=""my-component"">
                            This Blazor component is defined in the <strong>razorclasslib1</strong> package.
                          </div>");
    }
  }
}