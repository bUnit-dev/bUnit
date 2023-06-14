using Xunit;
using Bunit;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Bunit.Rendering;
using static Bunit.ComponentParameterFactory;

namespace Bunit.Docs.Samples;

public class AllKindsOfParamsTest : TestContext
{
  [Fact]
  public void NonBlazorParamTypes()
  {
    // Using C# tuple with hardcoded name
    var cut1 = Render<NonBlazorTypesParams>(
      ("Numbers", 42),
      ("Lines", new List<string> { "Hello", "World" })
    );

    // Using C# tuple with refactor safe name
    var cut2 = Render<NonBlazorTypesParams>(
      (nameof(NonBlazorTypesParams.Numbers), 42),
      (nameof(NonBlazorTypesParams.Lines), new List<string> { "Hello", "World" })
    );

    // Using factory method
    var cut3 = Render<NonBlazorTypesParams>(
      Parameter("Numbers", 42),
      Parameter("Lines", new List<string> { "Hello", "World" })
    );

    // Using parameter builder
    var cut4 = Render<NonBlazorTypesParams>(parameters => parameters
      .Add(p => p.Numbers, 42)
      .Add(p => p.Lines, new List<string> { "Hello", "World" })
    );
  }

  [Fact]
  public void EventCallbackTypes()
  {

    // Using factory method with hardcoded name
    var cut1 = Render<EventCallbackParams>(
      EventCallback("OnClick", (MouseEventArgs args) =>
      {
        /* handle callback */
      }),
      EventCallback("OnSomething", () =>
      {
        /* handle callback */
      })
    );

    // Using factory method refactor safe name
    var cut2 = Render<EventCallbackParams>(
      EventCallback(nameof(EventCallbackParams.OnClick), (MouseEventArgs args) =>
      {
        /* handle callback */
      }),
      EventCallback(nameof(EventCallbackParams.OnSomething), () =>
      {
        /* handle callback */
      })
    );

    // Using parameter builder
    var cut3 = Render<EventCallbackParams>(parameters => parameters
      .Add(p => p.OnClick, args =>
      {
        /* handle callback */
      })
      .Add(p => p.OnSomething, () =>
      {
        /* handle callback */
      })
    );
  }

  [Fact]
  public void HtmlAsChildContent()
  {
    // Using factory method
    var cut1 = Render<ChildContentParams>(
      ChildContent("<h1>Hello World</h1>")
    );

    // Using parameter builder
    var cut2 = Render<ChildContentParams>(parameters => parameters
      .AddChildContent("<h1>Hello World</h1>")
    );
  }

  [Fact]
  public void ComponentAsChildContent()
  {
    // Using factory method
    var cut1 = Render<ChildContentParams>(
      ChildContent<Counter>()
    );

    // Using parameter builder
    var cut2 = Render<ChildContentParams>(parameters => parameters
      .AddChildContent<Counter>()
    );
  }

  [Fact]
  public void ComponentWithParamsAsChildContent()
  {
    // Using factory method
    var cut1 = Render<ChildContentParams>(
      ChildContent<Alert>(
        ("Heading", "Alert heading"),
        ("Type", AlertType.Warning),
        ChildContent("<p>Hello World</p>")
      )
    );

    // Using parameter builder
    var cut2 = Render<ChildContentParams>(parameters => parameters
      .AddChildContent<Alert>(alertParameters => alertParameters
        .Add(p => p.Heading, "Alert heading")
        .Add(p => p.Type, AlertType.Warning)
        .AddChildContent("<p>Hello World</p>")
      )
    );
  }

  [Fact]
  public void ComponentAndMarkupAsChildContent()
  {
    // Using factory method
    var cut1 = Render<ChildContentParams>(
      ChildContent("<h1>Below you will find a most interesting alert!</h1>"),
      ChildContent<Alert>(
        ("Heading", "Alert heading"),
        ("Type", AlertType.Warning),
        ChildContent("<p>Hello World</p>")
      )
    );

    // Using parameter builder
    var cut2 = Render<ChildContentParams>(parameters => parameters
      .AddChildContent("<h1>Below you will find a most interesting alert!</h1>")
      .AddChildContent<Alert>(childParams => childParams
        .Add(p => p.Heading, "Alert heading")
        .Add(p => p.Type, AlertType.Warning)
        .AddChildContent("<p>Hello World</p>")
      )
    );
  }

  [Fact]
  public void HtmlAsRenderFragment()
  {
    // Using factory method
    var cut1 = Render<RenderFragmentParams>(
      RenderFragment("Content", "<h1>Hello World</h1>")
    );

    // Using parameter builder
    var cut2 = Render<RenderFragmentParams>(parameters => parameters
      .Add(p => p.Content, "<h1>Hello World</h1>")
    );
  }

  [Fact]
  public void ComponentAsRenderFragment()
  {
    // Using factory method
    var cut1 = Render<RenderFragmentParams>(
      RenderFragment<Counter>("Content")
    );

    // Using parameter builder
    var cut2 = Render<RenderFragmentParams>(parameters => parameters
      .Add<Counter>(p => p.Content)
    );
  }

  [Fact]
  public void ComponentWithParamsAsRenderFragment()
  {
    // Using factory method
    var cut1 = Render<RenderFragmentParams>(
      RenderFragment<Alert>("Content",
        ("Heading", "Alert heading"),
        ("Type", AlertType.Warning),
        ChildContent("<p>Hello World</p>")
      )
    );

    // Using parameter builder
    var cut2 = Render<RenderFragmentParams>(parameters => parameters
      .Add<Alert>(p => p.Content, alertParameters => alertParameters
        .Add(p => p.Heading, "Alert heading")
        .Add(p => p.Type, AlertType.Warning)
        .AddChildContent("<p>Hello World</p>")
      )
    );
  }

  [Fact]
  public void ComponentAndMarkupAsRenderFragment()
  {
    // Using factory method
    var cut1 = Render<RenderFragmentParams>(
      RenderFragment("Content", "<h1>Below you will find a most interesting alert!</h1>"),
      RenderFragment<Alert>("Content",
        ("Heading", "Alert heading"),
        ("Type", AlertType.Warning),
        ChildContent("<p>Hello World</p>")
      )
    );

    // Using parameter builder
    var cut2 = Render<RenderFragmentParams>(parameters => parameters
      .Add(p => p.Content, "<h1>Below you will find a most interesting alert!</h1>")
      .Add<Alert>(p => p.Content, childParams => childParams
        .Add(p => p.Heading, "Alert heading")
        .Add(p => p.Type, AlertType.Warning)
        .AddChildContent("<p>Hello World</p>")
      )
    );
  }

  [Fact]
  public void HtmlTemplateParams()
  {
    // Using factory method
    var cut1 = Render<TemplateParams<string>>(
      ("Items", new string[] { "Foo", "Bar", "Baz" }),
      Template<string>("Template", item => $"<span>{item}</span>")
    );

    // Using parameter builder
    var cut2 = Render<TemplateParams<string>>(parameters => parameters
      .Add(p => p.Items, new[] { "Foo", "Bar", "Baz" })
      .Add(p => p.Template, item => $"<span>{item}</span>")
    );
  }

  [Fact]
  public void HtmlAndComponentTemplateParams()
  {
    // Using factory method
    var cut1 = Render<TemplateParams<string>>(
      ("Items", new string[] { "Foo", "Bar", "Baz" }),
      Template<Item, string>("Template", value => new ComponentParameter[]
      {
        ("Value", value)
      })
    );

    // Using parameter builder
    var cut2 = Render<TemplateParams<string>>(parameters => parameters
      .Add(p => p.Items, new[] { "Foo", "Bar", "Baz" })
      .Add<Item, string>(p => p.Template, value => itemParams => itemParams
        .Add(p => p.Value, value)
      )
    );
  }

  [Fact]
  public void UnmatchedParamsTest()
  {
    // Using factory method
    var cut1 = Render<UnmatchedParams>(
      ("some-unknown-param", "a value")
    );

    // Using parameter builder
    var cut2 = Render<UnmatchedParams>(parameters => parameters
      .AddUnmatched("some-unknown-param", "a value")
    );
  }

  [Fact]
  public void UnnamedCascadingParamsTest()
  {
    var isDarkTheme = true;

    // Using factory method
    var cut1 = Render<CascadingParams>(
      CascadingValue(isDarkTheme)
    );

    // Using parameter builder
    var cut2 = Render<CascadingParams>(parameters => parameters
      .AddCascadingValue(isDarkTheme)
    );

    // Using parameter builder and selecting unnamed cascading parameter
    var cut3 = Render<CascadingParams>(parameters => parameters
      .Add(p => p.IsDarkTheme, isDarkTheme)
    );
  }

  [Fact]
  public void NamedCascadingParamsTest()
  {
    // Using factory method
    var cut1 = Render<CascadingParams>(
      CascadingValue("LoggedInUser", "Egil Hansen")
    );

    // Using parameter builder
    var cut2 = Render<CascadingParams>(parameters => parameters
      .Add(p => p.UserName, "Egil Hansen")
    );
  }

  [Fact]
  public void UnnamedAndNamedCascadingParamsTest()
  {
    var isDarkTheme = true;

    // Using factory method
    var cut1 = Render<CascadingParams>(
      CascadingValue(isDarkTheme),
      CascadingValue("LoggedInUser", "Egil Hansen"),
      CascadingValue("LoggedInEmail", "egil@example.com")
    );

    // Using parameter builder
    var cut2 = Render<CascadingParams>(parameters => parameters
      .AddCascadingValue(isDarkTheme)
      .Add(p => p.UserName, "Egil Hansen")
      .Add(p => p.Email, "egil@example.com")
    );

    // Using parameter builder and selecting unnamed cascading parameter
    var cut3 = Render<CascadingParams>(parameters => parameters
      .Add(p => p.IsDarkTheme, isDarkTheme)
      .Add(p => p.UserName, "Egil Hansen")
      .Add(p => p.Email, "egil@example.com")
    );
  }
}