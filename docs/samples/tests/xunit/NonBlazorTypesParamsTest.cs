using Xunit;
using Bunit;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Bunit.Rendering;

namespace Bunit.Docs.Samples;

public class NonBlazorTypesParamsTest : TestContext
{
  [Fact]
  public void Test()
  {
    var lines = new List<string> { "Hello", "World" };

    var cut = Render<NonBlazorTypesParams>(parameters => parameters
      .Add(p => p.Numbers, 42)
      .Add(p => p.Lines, lines)
    );
  }
}