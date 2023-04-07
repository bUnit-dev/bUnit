using Xunit;
using Bunit;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using static Bunit.ComponentParameterFactory;

namespace Bunit.Docs.Samples;

public class NestedComponentTest : TestContext
{
  [Fact]
  public void Test()
  {
    var wrapper = RenderComponent<Wrapper>(parameters => parameters
      .AddChildContent<HelloWorld>()
    );
    var cut = wrapper.FindComponent<HelloWorld>();
  }
}