using Xunit;
using Bunit;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;

namespace Bunit.Docs.Samples
{
  public class RenderTreeTest
  {
    [Fact]
    public void PrintCascadingValueTest()
    {
      using var ctx = new TestContext();

      // Add a cascading value to the test contexts root render tree.
      ctx.RenderTree.Add<CascadingValue<string>>(parameters => parameters
        .Add(p => p.Value, "FOO")
      );

      // The component will be rendered as a child of last
      // component added to the RenderTree property.
      var cut = ctx.RenderComponent<PrintCascadingValue>();

      // Verify that the cascading value was passed correctly.
      cut.MarkupMatches($"Cascading value: FOO");
    }

    [Fact]
    public void PrintCascadingValue2Test()
    {
      using var ctx = new TestContext();

      // Add a cascading value to the test contexts root render tree.
      ctx.RenderTree.TryAdd<CascadingValue<string>>(parameters => parameters
        .Add(p => p.Value, "BAR?")
      );

      // The component will be rendered as a child of last
      // component added to the RenderTree property.
      var cut = ctx.RenderComponent<PrintCascadingValue>();

      // Verify that the cascading value was passed correctly.
      cut.MarkupMatches($"Cascading value: BAR?");
    }
  }
}