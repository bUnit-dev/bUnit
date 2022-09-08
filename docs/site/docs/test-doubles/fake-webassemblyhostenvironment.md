---
uid: fake-webassemblyhostenvironment
title: Faking IWebAssemblyHostEnvironment
---

# Faking `IWebAssemblyHostEnvironment`

bUnit has a fake implementation of Blazor's `IWebAssemblyHostEnvironment` built-in, which is added by default to bUnit's `TestContext.Services` service provider. That means nothing special is needed to test components that depend on `IWebAssemblyHostEnvironment`, as it is already available by default.

## Verify `IWebAssemblyHostEnvironment` interactions

Lets look at a few examples that show how to verify that a component correctly uses the `IWebAssemblyHostEnvironment` in various ways.

In the examples, we'll use the following `<HelloWorld>` component:

```cshtml
@inject IWebAssemblyHostEnvironment HostEnvironment

<p>Hello @(HostEnvironment.IsDevelopment() ? "Developers" : "World"). The base URL is: @HostEnvironment.BaseAddress</p>
```

To verify that the `<HelloWorld>` component correctly says hello to the developers, do the following:

```csharp
// Arrange
using var ctx = new TestContext();
var hostEnvironment = ctx.Services.GetRequiredService<FakeWebAssemblyHostEnvironment>();
hostEnvironment.SetEnvironmentToDevelopment();

var cut = ctx.RenderComponent<SimpleUsingWebAssemblyHostEnvironment>();

// Assert - inspects markup to verify the message
cut.Find("p").MarkupMatches($"<p>Hello Developers. The base URL is: /</p>");
```

To verify that the `<HelloWorld>` component correctly uses the current `BaseAddress`, do the following:

```csharp
// Arrange
using var ctx = new TestContext();
var hostEnvironment = ctx.Services.GetRequiredService<FakeWebAssemblyHostEnvironment>();
hostEnvironment.BaseAddress = "myBaseUrl/";
var cut = ctx.RenderComponent<SimpleUsingWebAssemblyHostEnvironment>();

// Assert - inspect markup to verify that the BaseAddress is used correctly.
cut.Find("p").MarkupMatches($"<p>Hello World. The base URL is: myBaseUrl/</p>");
```
