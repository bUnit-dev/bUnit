using Xunit;
using Bunit;
using System.Collections.Generic;

using static Bunit.ComponentParameterFactory;

namespace Docs.Components.Xunit
{

  public class AllKindsOfParamsTest
  {
    [Fact]
    public void RegularParametersLooselyTyped()
    {
      using var ctx = new TestContext();

      // Using C# tuple with hardcoded name
      var cut1 = ctx.RenderComponent<AllKindsOfParams>(
        ("Numbers", 42),
        ("Lines", new List<string> { "Hello", "World" })
      );

      // Using C# tuple with refactor safe name
      var cut2 = ctx.RenderComponent<AllKindsOfParams>(
        (nameof(AllKindsOfParams.Numbers), 42),
        (nameof(AllKindsOfParams.Lines), new List<string> { "Hello", "World" })
      );

      // Using factory method
      var cut3 = ctx.RenderComponent<AllKindsOfParams>(
        Parameter("Numbers", 42),
        Parameter("Lines", new List<string> { "Hello", "World" })
      );

      // Using parameter builder
      var cut4 = ctx.RenderComponent<AllKindsOfParams>(parameters => parameters
        .Add(p => p.Numbers, 42)
        .Add(p => p.Lines, new List<string> { "Hello", "World" })
      );
    }

    [Fact]
    public void RegularParameters2()
    {
      using var ctx = new TestContext();


    }
  }
}
