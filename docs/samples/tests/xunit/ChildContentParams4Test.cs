using Xunit;
using Bunit;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Bunit.Rendering;

namespace Bunit.Docs.Samples;

public class ChildContentParams4Test : TestContext
{
  [Fact]
  public void Test()
  {
    var cut = RenderComponent<ChildContentParams>(parameters => parameters
      .AddChildContent("<h1>Below you will find a most interesting alert!</h1>")
      .AddChildContent<Alert>(childParams => childParams
        .Add(p => p.Heading, "Alert heading")
        .Add(p => p.Type, AlertType.Warning)
        .AddChildContent("<p>Hello World</p>")
      )
    );
  }
}