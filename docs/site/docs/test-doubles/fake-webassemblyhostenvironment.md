---
uid: bunit-webassemblyhostenvironment
title: Adding IWebAssemblyHostEnvironment
---

# Adding `IWebAssemblyHostEnvironment`

bUnit has itws own implementation of Blazor's `IWebAssemblyHostEnvironment` built-in, which is added by default to bUnit's `BunitContext.Services` service provider. That means nothing special is needed to test components that depend on `IWebAssemblyHostEnvironment`, as it is already available by default.

Out of the box, the implementation has its `Environment` property set to `production`, and its `BaseAddress` set to `/`.

## Setting `Environment` and `BaseAddress`

Lets look at a few examples of how to set the two `IWebAssemblyHostEnvironment` properties `Environment` and `BaseAddress` via the built-in object.

In the examples, we'll use the following `<HelloWorld>` component:

```cshtml
@inject IWebAssemblyHostEnvironment HostEnvironment

<p id="message">
  Hello @(HostEnvironment.IsDevelopment() ? "Developers" : "World"). 
</p>
<p id="address">
  The base URL is: @HostEnvironment.BaseAddress
</p>
```

To verify that the `<HelloWorld>` component correctly says hello to the developers, do the following:

```csharp
// Arrange
var hostEnvironment = Services.GetRequiredService<BunitWebAssemblyHostEnvironment>();

// Sets the environment to "Development". There are two other helper 
// methods available as well, SetEnvironmentToProduction() and 
// set SetEnvironmentToStaging(), or environment can also be changed
// directly through the hostEnvironment.Environment property.
hostEnvironment.SetEnvironmentToDevelopment();

var cut = Render<SimpleUsingWebAssemblyHostEnvironment>();

// Assert - inspects markup to verify the message
cut.Find("#message").MarkupMatches($"<p>Hello Developers.</p>");
```

To verify that the `<HelloWorld>` component correctly uses the current `BaseAddress`, do the following:

```csharp
// Arrange
var hostEnvironment = Services.GetRequiredService<BunitWebAssemblyHostEnvironment>();

// Sets a new base address directly on the BaseAddress property.
hostEnvironment.BaseAddress = "myBaseUrl/";

// Act
var cut = Render<SimpleUsingWebAssemblyHostEnvironment>();

// Assert - inspect markup to verify that the BaseAddress is used correctly.
cut.Find("#address").MarkupMatches($"<p>The base URL is: myBaseUrl/</p>");
```
