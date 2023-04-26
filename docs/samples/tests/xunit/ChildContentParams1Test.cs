using Xunit;
using Bunit;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Bunit.Rendering;

namespace Bunit.Docs.Samples;

public class ChildContentParams1Test : TestContext
{
  [Fact]
  public void Test()
  {
    var cut = RenderComponent<ChildContentParams>(parameters => parameters
      .AddChildContent("<h1>Hello World</h1>")
    );
  }
}