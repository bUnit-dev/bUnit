using System;
using Xunit;
using Egil.RazorComponents.Testing;
using Egil.RazorComponents.Testing.Diffing;
using Egil.RazorComponents.Testing.Asserting;
using Egil.RazorComponents.Testing.EventDispatchExtensions;

namespace Company.RazorTests1
{
  public class Component1Test : ComponentTestFixture
  {
    [Fact]
    public void Component1RendersCorrectly()
    {
      var cut = RenderComponent<Component1>();

      cut.ShouldBe(@"<div class=""my-component"">
                      This Blazor component is defined in the <strong>razorclasslib1</strong> package.
                    </div>");
    }
  }
}