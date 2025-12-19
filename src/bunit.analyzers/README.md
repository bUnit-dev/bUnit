# bUnit Analyzers

This package contains Roslyn analyzers for bUnit that help identify common mistakes and anti-patterns when writing bUnit tests.

## Analyzers

### BUNIT0001: Razor test files should inherit from BunitContext

When writing tests in `.razor` files that use variables or event callbacks from the test code, the file must inherit from `BunitContext` using `@inherits BunitContext`. Otherwise, you may encounter the error "The render handle is not yet assigned."

**Bad:**
```razor
@code
{
    [Fact]
    public void Test()
    {
        using var ctx = new BunitContext();
        Action<MouseEventArgs> onClickHandler = _ => { Assert.True(true); };
        var cut = ctx.Render(@<MyComponent OnClick="onClickHandler" />);
    }
}
```

**Good:**
```razor
@inherits BunitContext
@code
{
    [Fact]
    public void Test()
    {
        Action<MouseEventArgs> onClickHandler = _ => { Assert.True(true); };
        var cut = Render(@<MyComponent OnClick="onClickHandler" />);
    }
}
```

### BUNIT0002: Prefer Find<T> over casting

When finding elements with a specific type, use `Find<T>(selector)` instead of casting the result of `Find(selector)`.

**Bad:**
```csharp
IHtmlAnchorElement elem = (IHtmlAnchorElement)cut.Find("a");
```

**Good:**
```csharp
var elem = cut.Find<IHtmlAnchorElement>("a");
```

## Installation

Install the package via NuGet:

```bash
dotnet add package bunit.analyzers
```

The analyzers will automatically run during compilation and provide warnings and code fixes in your IDE.
