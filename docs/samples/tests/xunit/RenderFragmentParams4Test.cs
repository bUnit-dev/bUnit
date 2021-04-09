using Xunit;
using Bunit;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Bunit.Rendering;

namespace Bunit.Docs.Samples
{
  public class RenderFragmentParams4Test
  {
    [Fact]
    public void Test()
    {
      using var ctx = new TestContext();

      var cut = ctx.RenderComponent<RenderFragmentParams>(parameters => parameters
        .Add(p => p.Content, "<h1>Below you will find a most interesting alert!</h1>")
        .Add<Alert>(p => p.Content, childParams => childParams
          .Add(p => p.Heading, "Alert heading")
          .Add(p => p.Type, AlertType.Warning)
          .AddChildContent("<p>Hello World</p>")
        )
      );
    }
  }
}