---
uid: bunit-generators
title: bUnit Generators
---

# bUnit Generators

The `bunit.generators` package contains a set of source generators that can be used to generate code likes stubs for Blazor components. The generators are designed to be used with the [bUnit](https://github.com/bunit-dev/bunit) testing framework for Blazor components. To use the generators, you must install the `bunit.generators` NuGet package in test project.

This page will describe the generators and their usage.

## Component stub generator via `AddStub`

This generator adds the ability to automatically generate stubs for a given type with no setup involved. The generator sits on top of the already
present `AddStub` method.
This comes in handy, when dealing with 3rd party components that might need an extensive setup. Here a small example:

Given the following component
```razor
<ThirdPartyText Text="@count" />
<button @onclick="IncrementCount">Increase by one</button>
@code {
 private int count;
 
 private void IncrementCount()
 {
	 count++;
 }
}
```

If `ThirdPartyText` is a 3rd party component, that needs a lot of setup, it might be easier to just stub it out:

```csharp
[Fact]
public void Text_button_gets_initial_count()
{
    // This call will automatically generate a stub for the ThirdPartyButton component
    // with the name "ThirdPartyButtonStub"
    ComponentFactories.AddStub<ThirdPartyText>();
    var cut = Render<Counter>(@<Counter />);
    
    cut.Find("button").Click();
    
    // Retrieves the stub from the render tree and checks if the text is "1"
    cut.FindComponent<ThirdPartyTextStub>().Instance.Text.Should().Be("1");
}
```

### Setup
To use the generator, the **Interceptor** feature has to be used inside the csproj file:

```xml
<Project Sdk="Microsoft.NET.Sdk">
	<PropertyGroup>
		<TargetFramework>net8.0</TargetFramework>
		<!-- This line is required to enable the generator and interceptor -->
		<InterceptorsPreviewNamespaces>$(InterceptorsPreviewNamespaces);Bunit</InterceptorsPreviewNamespaces>
```

Due to the usage of **Interceptors** the generator is only available for .NET 8.0 and above. The generator does create a `partial` class, so it can be extended with custom logic if needed.

## Component stub generator via `ComponentStubAttribute`

This generator adds the ability to automatically generate stubs for a given type via an attribute.
The general setup for the given component above looks like this:
```csharp
namespace MyTest;

public class FeatureTests : TestContext
{
    [Fact]
    public void Test()
    {
        ComponentFactories.Add<ThirdPartyText, ThirdPartyStub>();
        ...
    }    
}

[ComponentStub<ThirdPartyText>]
internal partial class ThirdPartyStub { }
```

Current limitations of this approach is that he stubbed type is not allowed to be nested inside the test class.
