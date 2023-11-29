# bUnit Generators

This package contains source generators for bUnit, to make it easier and more convenient to write tests.

## `AddStub` Generator
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

This limits the usage to .NET 8 and above.

## `StubAttribute`
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

[Stub(typeof(ThirdPartyText))]
internal partial class ThidPartyStub { }
```

Current limitations of this approach:
 * The stubbed type is not allowed to be nested inside the test class.

## Developer notes

### Tips for developing with the generator

When changing the source generator, to see the effect, clearing the build cache may be necessary:

```
dotnet build-server shutdown
```

A good way to quickly see if the generate is producing output:

```
dotnet build-server shutdown && dotnet clean && dotnet test -p:TargetFramework=net8.0
```
