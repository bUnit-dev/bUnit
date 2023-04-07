using Xunit;
using Bunit;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Bunit.Rendering;

namespace Bunit.Docs.Samples;

public class RenderFragmentParams1Test : TestContext
{
  [Fact]
  public void Test()
  {
    var cut = RenderComponent<RenderFragmentParams>(parameters => parameters
      .Add(p => p.Content, "<h1>Hello World</h1>")
    );
  }
}