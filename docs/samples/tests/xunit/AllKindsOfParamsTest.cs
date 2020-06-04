using Xunit;
using Bunit;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components.Web;

using static Bunit.ComponentParameterFactory;

namespace Bunit.Docs.Samples
{
  public class AllKindsOfParamsTest
  {
    [Fact]
    public void NonBlazorParamTypes()
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
    public void EventCallbackTypes()
    {
      using var ctx = new TestContext();

      // Using factory method with hardcoded name
      var cut1 = ctx.RenderComponent<AllKindsOfParams>(
        EventCallback("OnClick", (MouseEventArgs args) => { /* handle callback */ }),
        EventCallback("OnSomething", () => { /* handle callback */ })
      );

      // Using factory method refactor safe name
      var cut2 = ctx.RenderComponent<AllKindsOfParams>(
        EventCallback(nameof(AllKindsOfParams.OnClick), (MouseEventArgs args) => { /* handle callback */ }),
        EventCallback(nameof(AllKindsOfParams.OnSomething), () => { /* handle callback */ })
      );

      // Using parameter builder
      var cut3 = ctx.RenderComponent<AllKindsOfParams>(parameters => parameters
        .Add(p => p.OnClick, args => { /* handle callback */ })
        .Add(p => p.OnSomething, () => { /* handle callback */ })
      );
    }

    [Fact]
    public void HtmlAsChildContent()
    {
      using var ctx = new TestContext();

      // Using factory method
      var cut1 = ctx.RenderComponent<AllKindsOfParams>(
        ChildContent("<h1>Hello World</h1>")
      );

      // Using parameter builder
      var cut2 = ctx.RenderComponent<AllKindsOfParams>(parameters => parameters
        .AddChildContent("<h1>Hello World</h1>")
      );
    }

    [Fact]
    public void ComponentAsChildContent()
    {
      using var ctx = new TestContext();

      // Using factory method
      var cut1 = ctx.RenderComponent<AllKindsOfParams>(
        ChildContent<Counter>()
      );

      // Using parameter builder
      var cut2 = ctx.RenderComponent<AllKindsOfParams>(parameters => parameters
        .AddChildContent<Counter>()
      );
    }

    [Fact]
    public void ComponentWithParamsAsChildContent()
    {
      using var ctx = new TestContext();

      // Using factory method
      var cut1 = ctx.RenderComponent<AllKindsOfParams>(
        ChildContent<Alert>(
          ("Heading", "Alert heading"),
          ("Type", AlertType.Warning),
          ChildContent("<p>Hello World</p>")
        )
      );

      // Using parameter builder
      var cut2 = ctx.RenderComponent<AllKindsOfParams>(parameters => parameters
        .AddChildContent<Alert>(alertParameters => alertParameters
          .Add(p => p.Heading, "Alert heading")
          .Add(p => p.Type, AlertType.Warning)
          .AddChildContent("<p>Hello World</p>")
        )
      );
    }
  }
}
