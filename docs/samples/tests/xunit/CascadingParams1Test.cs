using Xunit;
using Bunit;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Bunit.Rendering;

namespace Bunit.Docs.Samples
{
  public class CascadingParams1Test
  {
    [Fact]
    public void Test()
    {
      using var ctx = new TestContext();

      var isDarkTheme = true;

      var cut = ctx.RenderComponent<CascadingParams>(parameters => parameters
        .Add(p => p.IsDarkTheme, isDarkTheme)
      );
    }
  }
}