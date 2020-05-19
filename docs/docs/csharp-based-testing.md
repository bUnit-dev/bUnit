# C# based testing

This pages documents how to do Blazor/Razor component testing using just C#.

## Creating an new test class

When using xUnit as the general test framework
All test classes are expected to inherit from `TestContext`, which implements the `ITestContext` interface. It is also recommended to include the `ComponentParameterFactory` through `using static`, to have easy access to component parameter factory methods.

The example below includes the needed using statements as well:

```csharp
using System;
using Bunit;
using Bunit.Mocking.JSInterop;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using static Bunit.ComponentParameterFactory;

public class MyComponentTest : TestContext
{
  [Fact]
  public void MyFirstTest()
  {
    // ...
  }
}
```

The `TestContext` contains all the logic for rendering components and correctly dispose of renderers, components, and HTML parsers after each test.

## Executing test cases

Since Blazor component tests are just regular xUnit test/facts, you execute them in exactly the same way as you would normal tests, i.e. by running `dotnet test` from the console or running the tests through the Test Explorer in Visual Studio.

## Rendering components during tests

To render a component, we use the `RenderComponent<TComponent>(params ComponentParameter[] parameters)` method. It will take the component (`TComponent`) through its usual life-cycle from `OnInitialized` to `OnAfterRender`. For example:

```csharp
public class ComponentTest : TestContext
{
  [Fact]
  public void Test1()
  {
    // Renders a MyComponent component and assigns the result to
    // a cut variable. CUT is short for Component Under Test.
    IRenderedComponent<MyComponent> cut = RenderComponent<MyComponent>();
  }
}
```

The `RenderComponent<TComponent>(params ComponentParameter[] parameters) : IRenderedComponent<MyComponent>` method has these parts:

- `TComponent` is the type of component you want to render.
- `ComponentParameter[] parameters` represents parameters that will be passed to the component during render.
- `IRenderedComponent<TComponent>` is the representation of the rendered component.

### Passing parameters to components during render

There are four types of parameters you can pass to a component being rendered through the `RenderComponent()` method:

- Cascading values (normally provided by the `<CascadingValue>` component in `.razor` files).
- Event callbacks (of type `EventCallback<T>` or `EventCallback`).
- Child content, render fragments, or templates (of type `RenderFragment` and `RenderFragment<T>`).
- All other normal parameters, including unmatched parameters.

In addition to parameters, services can also be registered in the `TestContext` and injected during component render.

To show how, let us look at a few examples that correctly pass parameters and services to the following `AllTypesOfParams<TItem>` component:

```cshtml
@typeparam TItem
@inject IJSRuntime jsRuntime
@code {
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object> Attributes { get; set; }

    [Parameter]
    public string RegularParam { get; set; }

    [CascadingParameter]
    public int UnnamedCascadingValue { get; set; }

    [CascadingParameter(Name = "Named")]
    public int NamedCascadingValue { get; set; }

    [Parameter]
    public EventCallback NonGenericCallback { get; set; }

    [Parameter]
    public EventCallback<EventArgs> GenericCallback { get; set; }

    [Parameter]
    public RenderFragment ChildContent { get; set; }

    [Parameter]
    public RenderFragment OtherContent { get; set; }

    [Parameter]
    public RenderFragment<TItem> ItemTemplate { get; set; }
}
```

And to render the `AllTypesOfParams<TItem>` component with all possible parameters set, use the following code:

```csharp
var cut = RenderComponent<AllTypesOfParams<string>>(
    // pass name-value attribute to be captured by the Attributes parameter
    ("some-unmatched-attribute", "unmatched value"),
    // pass value to the RegularParam parameter
    ("RegularParam", "some value"),
    // pass value to the UnnamedCascadingValue cascading parameter
    CascadingValue(42),
    // pass value to the NamedCascadingValue cascading parameter
    CascadingValue("Named", 1337),
     // pass action callback to the NonGenericCallback parameter
    EventCallback("NonGenericCallback", () => { /* logic here */ }),
     // pass action callback to the GenericCallback parameter
    EventCallback("GenericCallback", (EventArgs args) => { /* logic here */ }),
    // pass render fragment to the ChildContent parameter
    ChildContent("<h1>hello world</h1>"),
    // paas render fragment to the OtherContent parameter
    RenderFragment("OtherContent", "<h1>hello world</h1>"),
    // pass an template render fragment to the ItemTemplate parameter
    Template<string>("ItemTemplate", (item) => (builder) => { })
);
```

- **Regular parameters** can easily be passed as `(string name, object? value)` pairs (they are automatically converted to a ComponentParameter). We see two examples of that with the _"RegularParam"_ and the unmatched attribute _"some-unmatched-attribute"_.
- **Cascading values** can be passed both as named and unnamed via the `CascadingValue` helper method, as we see in the example above with _"UnnamedCascadingValue"_ and _"NamedCascadingValue"_.
- **Event callbacks** can be passed as `Func` and `Action` types with and without input and return types, using the `EventCallback` helper method. The example above shows two examples in _"NonGenericCallback"_ and _"GenericCallback"_
- **Child content** and general **Render fragments** is passed to a component using the `ChildContent` or `RenderFragment` helper methods. The `ChildContent` and `RenderFragment` methods has two overloads, one that takes a (markup) string and a generic version, e.g. for child content, `ChildContent<TComponent>(params ComponentParameter[] parameters)`, which will generate the necessary render fragment to render a component as the child content. Note that the methods takes the same input arguments as the `RenderComponent` method, which means it too can be passed all the types of parameters shown in the example above.
- **Templates** render fragments can be passed via the `Template<TValue>` method, which takes the name of the parameter and a `RenderFragment<TValue>` as input. Unfortunately, you will have to turn to the `RenderTreeBuilder` API to create templates at the moment.

> _**TIP:**_ Use the `nameof(Component.Parameter)` method to get parameter names in a refactor-safe way. For example, if we have a component `MyComponent` with a parameter named `RegularParam`, then use this when rendering:

```csharp
var cut = RenderComponent<MyComponent>(
  (nameof(MyComponent.RegularParam), "some value")
);
```

### Registering and injecting services into components during render

When testing components that require services to be injected into them, i.e. `@inject IJsRuntime jsRuntime`, you must register the services or a mock thereof before you render your component.

This is done via the `Services` property available on the `TestContext`. Once a component has been rendered, no more services can be added to the service collection.

If for example we want to render the with a dependency on an `IMyService`, we first have to call one of the `AddSingleton` methods on the service collection and register it. All the normal `AddSingleton` `ServiceCollection` overloads are available.

In the case if a `IJsRuntime` dependency, we can however use the built-in [Mocking JsRuntime](/docs/mocking-jsruntime.html). For example:

```csharp
public class ComponentTest : TestContext // implements the ITestContext interface
{
  [Fact]
  public void Test1()
  {
    // Add an custom service to the services collection
    Services.AddSingleton<IMyService>(new MyService());

    // Add the Mock JsRuntime service
    Services.AddMockJsRuntime();

    // Renders a MyComponent component and assigns the result to
    // a cut variable. CUT is short for Component Under Test.
    IRenderedComponent<MyComponent> cut = RenderComponent<MyComponent>();
  }
}
```

See the page [Mocking JsRuntime](/docs/mocking-jsruntime.html) for more details mock.

**Further reading:**

- [Semantic HTML markup comparison](/docs/semantic-html-markup-comparison.html)
- [Mocking JsRuntime](/docs/mocking-jsruntime.html)
- [C# test examples](/docs/csharp-test-examples.html)