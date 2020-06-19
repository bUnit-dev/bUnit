using Xunit;
using Bunit;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
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
      var cut1 = ctx.RenderComponent<NonBlazorTypesParams>(
        ("Numbers", 42),
        ("Lines", new List<string> { "Hello", "World" })
      );

      // Using C# tuple with refactor safe name
      var cut2 = ctx.RenderComponent<NonBlazorTypesParams>(
        (nameof(NonBlazorTypesParams.Numbers), 42),
        (nameof(NonBlazorTypesParams.Lines), new List<string> { "Hello", "World" })
      );

      // Using factory method
      var cut3 = ctx.RenderComponent<NonBlazorTypesParams>(
        Parameter("Numbers", 42),
        Parameter("Lines", new List<string> { "Hello", "World" })
      );

      // Using parameter builder
      var cut4 = ctx.RenderComponent<NonBlazorTypesParams>(parameters => parameters
        .Add(p => p.Numbers, 42)
        .Add(p => p.Lines, new List<string> { "Hello", "World" })
      );
    }

    [Fact]
    public void EventCallbackTypes()
    {
      using var ctx = new TestContext();

      // Using factory method with hardcoded name
      var cut1 = ctx.RenderComponent<EventCallbackParams>(
        EventCallback("OnClick", (MouseEventArgs args) => { /* handle callback */ }),
        EventCallback("OnSomething", () => { /* handle callback */ })
      );

      // Using factory method refactor safe name
      var cut2 = ctx.RenderComponent<EventCallbackParams>(
        EventCallback(nameof(EventCallbackParams.OnClick), (MouseEventArgs args) => { /* handle callback */ }),
        EventCallback(nameof(EventCallbackParams.OnSomething), () => { /* handle callback */ })
      );

      // Using parameter builder
      var cut3 = ctx.RenderComponent<EventCallbackParams>(parameters => parameters
        .Add(p => p.OnClick, args => { /* handle callback */ })
        .Add(p => p.OnSomething, () => { /* handle callback */ })
      );
    }

    [Fact]
    public void HtmlAsChildContent()
    {
      using var ctx = new TestContext();

      // Using factory method
      var cut1 = ctx.RenderComponent<ChildContentParams>(
        ChildContent("<h1>Hello World</h1>")
      );

      // Using parameter builder
      var cut2 = ctx.RenderComponent<ChildContentParams>(parameters => parameters
        .AddChildContent("<h1>Hello World</h1>")
      );
    }

    [Fact]
    public void ComponentAsChildContent()
    {
      using var ctx = new TestContext();

      // Using factory method
      var cut1 = ctx.RenderComponent<ChildContentParams>(
        ChildContent<Counter>()
      );

      // Using parameter builder
      var cut2 = ctx.RenderComponent<ChildContentParams>(parameters => parameters
        .AddChildContent<Counter>()
      );
    }

    [Fact]
    public void ComponentWithParamsAsChildContent()
    {
      using var ctx = new TestContext();

      // Using factory method
      var cut1 = ctx.RenderComponent<ChildContentParams>(
        ChildContent<Alert>(
          ("Heading", "Alert heading"),
          ("Type", AlertType.Warning),
          ChildContent("<p>Hello World</p>")
        )
      );

      // Using parameter builder
      var cut2 = ctx.RenderComponent<ChildContentParams>(parameters => parameters
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
      using var ctx = new TestContext();

      // Using parameter builder
      var cut = ctx.RenderComponent<ChildContentParams>(parameters => parameters
        .Add(p => p.ChildContent, (RenderFragment)(builder =>
        {
          builder.AddMarkupContent(1, "<h1>Below you will find a most interesting alert!</h1>");
          builder.OpenComponent<Alert>(2);
          builder.AddAttribute(3, "Heading", "Alert heading");
          builder.AddAttribute(4, "Type", AlertType.Warning);
          builder.AddAttribute(5, "ChildContent", (RenderFragment)(alertBuilder => alertBuilder.AddMarkupContent(1, "<p>Hello World</p>")));
          builder.CloseComponent();
        }))
      );
    }

    [Fact]
    public void HtmlAsRenderFragment()
    {
      using var ctx = new TestContext();

      // Using factory method
      var cut1 = ctx.RenderComponent<RenderFragmentParams>(
        RenderFragment("Content", "<h1>Hello World</h1>")
      );

      // Using parameter builder
      var cut2 = ctx.RenderComponent<RenderFragmentParams>(parameters => parameters
        .Add(p => p.Content, "<h1>Hello World</h1>")
      );
    }

    [Fact]
    public void ComponentAsRenderFragment()
    {
      using var ctx = new TestContext();

      // Using factory method
      var cut1 = ctx.RenderComponent<RenderFragmentParams>(
        RenderFragment<Counter>("Content")
      );

      // Using parameter builder
      var cut2 = ctx.RenderComponent<RenderFragmentParams>(parameters => parameters
        .Add<Counter>(p => p.Content)
      );
    }

    [Fact]
    public void ComponentWithParamsAsRenderFragment()
    {
      using var ctx = new TestContext();

      // Using factory method
      var cut1 = ctx.RenderComponent<RenderFragmentParams>(
        RenderFragment<Alert>("Content",
          ("Heading", "Alert heading"),
          ("Type", AlertType.Warning),
          ChildContent("<p>Hello World</p>")
        )
      );

      // Using parameter builder
      var cut2 = ctx.RenderComponent<RenderFragmentParams>(parameters => parameters
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
      using var ctx = new TestContext();

      // Using parameter builder
      var cut = ctx.RenderComponent<RenderFragmentParams>(parameters => parameters
        .Add(p => p.Content, (RenderFragment)(builder =>
        {
          builder.AddMarkupContent(1, "<h1>Below you will find a most interesting alert!</h1>");
          builder.OpenComponent<Alert>(2);
          builder.AddAttribute(3, "Heading", "Alert heading");
          builder.AddAttribute(4, "Type", AlertType.Warning);
          builder.AddAttribute(5, "ChildContent", (RenderFragment)(alertBuilder => alertBuilder.AddMarkupContent(1, "<p>Hello World</p>")));
          builder.CloseComponent();
        }))
      );
    }

    [Fact]
    public void HtmlTemplateParams()
    {
      using var ctx = new TestContext();

      // Using factory method
      var cut1 = ctx.RenderComponent<TemplateParams<string>>(
        ("Items", new string[] { "Foo", "Bar", "Baz" }),
        Template<string>("Template", item => $"<span>{item}</span>")
      );

      // Using parameter builder
      var cut2 = ctx.RenderComponent<TemplateParams<string>>(parameters => parameters
        .Add(p => p.Items, new[] { "Foo", "Bar", "Baz" })
        .Add(p => p.Template, item => $"<span>{item}</span>")
      );
    }

    [Fact]
    public void HtmlAndComponentTemplateParams()
    {
      using var ctx = new TestContext();

      // Using factory method
      var cut1 = ctx.RenderComponent<TemplateParams<string>>(
        ("Items", new string[] { "Foo", "Bar", "Baz" }),
        Template<string>("Template", item => builder =>
        {
          builder.OpenElement(1, "div");
          builder.AddAttribute(2, "class", "item");
          builder.OpenComponent<Item>(3);
          builder.AddAttribute(4, "Value", item);
          builder.CloseComponent();
          builder.CloseElement();
        })
      );

      // Using parameter builder
      var cut2 = ctx.RenderComponent<TemplateParams<string>>(parameters => parameters
        .Add(p => p.Items, new[] { "Foo", "Bar", "Baz" })
        .Add(p => p.Template, item => builder =>
        {
          builder.OpenElement(1, "div");
          builder.AddAttribute(2, "class", "item");
          builder.OpenComponent<Item>(3);
          builder.AddAttribute(4, "Value", item);
          builder.CloseComponent();
          builder.CloseElement();
        })
      );
    }

    [Fact]
    public void UnmatchedParamsTest()
    {
      using var ctx = new TestContext();

      // Using factory method
      var cut1 = ctx.RenderComponent<UnmatchedParams>(
        ("some-unknown-param", "a value")
      );

      // Using parameter builder
      var cut2 = ctx.RenderComponent<UnmatchedParams>(parameters => parameters
        .AddUnmatched("some-unknown-param", "a value")
      );
    }

    [Fact]
    public void UnnamedCascadingParamsTest()
    {
      using var ctx = new TestContext();
      var isDarkTheme = true;

      // Using factory method
      var cut1 = ctx.RenderComponent<CascadingParams>(
        CascadingValue(isDarkTheme)
      );

      // Using parameter builder
      var cut2 = ctx.RenderComponent<CascadingParams>(parameters => parameters
        .Add(isDarkTheme)
      );

      // Using parameter builder and selecting unnamed cascading parameter
      var cut3 = ctx.RenderComponent<CascadingParams>(parameters => parameters
        .Add(p => p.IsDarkTheme, isDarkTheme)
      );
    }

    [Fact]
    public void NamedCascadingParamsTest()
    {
      using var ctx = new TestContext();

      // Using factory method
      var cut1 = ctx.RenderComponent<CascadingParams>(
        CascadingValue("LoggedInUser", "Egil Hansen")
      );

      // Using parameter builder
      var cut2 = ctx.RenderComponent<CascadingParams>(parameters => parameters
        .Add(p => p.UserName, "Egil Hansen")
      );
    }

    [Fact]
    public void UnnamedAndNamedCascadingParamsTest()
    {
      using var ctx = new TestContext();
      var isDarkTheme = true;

      // Using factory method
      var cut1 = ctx.RenderComponent<CascadingParams>(
        CascadingValue(isDarkTheme),
        CascadingValue("LoggedInUser", "Egil Hansen"),
        CascadingValue("LoggedInEmail", "egil@example.com")
      );

      // Using parameter builder
      var cut2 = ctx.RenderComponent<CascadingParams>(parameters => parameters
        .Add(isDarkTheme)
        .Add(p => p.UserName, "Egil Hansen")
        .Add(p => p.Email, "egil@example.com")
      );

      // Using parameter builder and selecting unnamed cascading parameter
      var cut3 = ctx.RenderComponent<CascadingParams>(parameters => parameters
        .Add(p => p.IsDarkTheme, isDarkTheme)
        .Add(p => p.UserName, "Egil Hansen")
        .Add(p => p.Email, "egil@example.com")
      );
    }
  }
}
