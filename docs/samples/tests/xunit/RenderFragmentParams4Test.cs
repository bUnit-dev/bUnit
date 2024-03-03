using Xunit;
using Bunit;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Bunit.Rendering;

namespace Bunit.Docs.Samples;

public class RenderFragmentParams4Test : TestContext
{
  [Fact]
  public void Test()
  {
    var cut = Render<RenderFragmentParams>(parameters => parameters
      .Add(p => p.Content, "<h1>Below you will find a most interesting alert!</h1>")
      .Add<Alert>(p => p.Content, childParams => childParams
        .Add(p => p.Heading, "Alert heading")
        .Add(p => p.Type, AlertType.Warning)
        .AddChildContent("<p>Hello World</p>")
      )
    );
  }
}