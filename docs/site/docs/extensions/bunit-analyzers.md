---
uid: bunit-analyzers
title: bUnit Analyzers
---

# bUnit Analyzers

The `bunit.analyzers` package contains a set of Roslyn analyzers that help identify common mistakes and anti-patterns when writing bUnit tests. The analyzers are designed to provide early feedback during development, catching issues before tests are run. To use the analyzers, install the `bunit.analyzers` NuGet package in your test project.

This page describes the available analyzers and their usage.

## Installation

Install the package via NuGet:

```bash
dotnet add package bunit.analyzers
```

The analyzers will automatically run during compilation and provide warnings or suggestions in your IDE and build output.

## Available Analyzers

### BUNIT0002: Prefer Find&lt;T&gt; over casting

**Severity**: Info  
**Category**: Usage

This analyzer detects when a cast is applied to the result of `Find(selector)` and suggests using the generic `Find<T>(selector)` method instead. Using the generic method is more concise, type-safe, and expresses intent more clearly.

#### Examples

**Incorrect** - triggers BUNIT0002:
```csharp
using AngleSharp.Dom;

var cut = Render<MyComponent>();
IHtmlAnchorElement link = (IHtmlAnchorElement)cut.Find("a");
```

**Correct**:
```csharp
using AngleSharp.Dom;

var cut = Render<MyComponent>();
var link = cut.Find<IHtmlAnchorElement>("a");
```

#### When to Use

`Find<T>()` should be used whenever a specific element type is needed:
- When working with AngleSharp element interfaces (`IHtmlAnchorElement`, `IHtmlButtonElement`, etc.)
- When type-specific properties or methods need to be accessed
- When clearer, more maintainable test code is desired

### BUNIT0001: Razor test files should inherit from BunitContext

**Status**: Planned for future release

This analyzer will detect when Razor test files (`.razor` files) use variables or event callbacks from the test code without inheriting from `BunitContext`. Without the proper inheritance, the error "The render handle is not yet assigned" may be encountered.

#### Planned Examples

**Incorrect** - will trigger BUNIT0001 in the future:
```razor
@code
{
    [Fact]
    public void Test()
    {
        using var ctx = new BunitContext();
        
        Action<MouseEventArgs> onClickHandler = _ => { Assert.True(true); };
        
        var cut = ctx.Render(@<MyComponent OnClick="onClickHandler" />);
        cut.Find("button").Click();
    }
}
```

**Correct**:
```razor
@inherits BunitContext
@code
{
    [Fact]
    public async Task Test()
    {
        var wasInvoked = false;
        
        Action<MouseEventArgs> onClick = _ => { wasInvoked = true; };
        
        var cut = Render(@<MyComponent OnClick="onClick" />);
        var button = cut.Find("button");
        await button.ClickAsync(new MouseEventArgs());
        
        cut.WaitForAssertion(() => Assert.True(wasInvoked));
    }
}
```

## Configuration

The analyzers can be configured in a project's `.editorconfig` file or using ruleset files. For example, to change the severity of BUNIT0002:

```ini
# .editorconfig
[*.cs]
# Change BUNIT0002 from Info to Warning
dotnet_diagnostic.BUNIT0002.severity = warning

# Or disable it entirely
dotnet_diagnostic.BUNIT0002.severity = none
```

## Contributing

We welcome contributions! If you have ideas for additional analyzers that could help bUnit users, please:

1. Check existing [issues](https://github.com/bunit-dev/bUnit/issues) for similar suggestions
2. Open a new issue describing the problem the analyzer would solve
3. Submit a pull request with your implementation

Common areas for improvement include:
- Detecting incorrect usage of `WaitFor` methods
- Identifying missing or incorrect component parameter bindings
- Catching improper service injection patterns
- Finding opportunities to use helper methods

## Feedback

If you encounter issues with the analyzers or have suggestions for improvements, please [open an issue](https://github.com/bunit-dev/bUnit/issues/new) on GitHub.
