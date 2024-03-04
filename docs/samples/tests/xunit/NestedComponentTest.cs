using Xunit;
using Bunit;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Bunit.Docs.Samples;

public class NestedComponentTest : TestContext
{
  [Fact]
  public void Test()
  {
    var wrapper = Render<Wrapper>(parameters => parameters
      .AddChildContent<HelloWorld>()
    );
    var cut = wrapper.FindComponent<HelloWorld>();
  }
}