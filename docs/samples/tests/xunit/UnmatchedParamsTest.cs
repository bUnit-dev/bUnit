using Xunit;
using Bunit;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Bunit.Rendering;

namespace Bunit.Docs.Samples;

public class UnmatchedParamsTest : BunitContext
{
  [Fact]
  public void Test()
  {
    var cut = Render<UnmatchedParams>(parameters => parameters
      .AddUnmatched("some-unknown-param", "a value")
    );
  }
}