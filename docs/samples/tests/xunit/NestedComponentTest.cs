using Xunit;
using Bunit;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

using static Bunit.ComponentParameterFactory;

namespace Bunit.Docs.Samples
{
  public class NestedComponentTest
  {
    [Fact]
    public void NestedComponent()
    {
      using var ctx = new TestContext();

      // Using C# factory methods
      var wrapper1 = ctx.RenderComponent<Wrapper>(
        ChildContent<HelloWorld>()
      );
      var cut1 = wrapper1.FindComponent<HelloWorld>();

      // Using parameter builder
      var wrapper2 = ctx.RenderComponent<Wrapper>(parameters => parameters
        .AddChildContent<HelloWorld>()
      );
      var cut2 = wrapper2.FindComponent<HelloWorld>();
    }
  }
}
