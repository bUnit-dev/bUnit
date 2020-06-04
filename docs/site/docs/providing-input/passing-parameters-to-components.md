---
uid: passing-parameters-to-components
title: Passing Parameters to Components
---

# Passing Parameters to Components

bUnit comes with a bunch of ways to pass parameters to components. 

In Razor-based tests, those written in `.razor` files, passing parameters is exactly the same as in your normal Blazor pages and components.

For C#-based test code, help is needed. This comes as:

- Loosely typed factory methods
- Simple tuple-based syntax, i.e. `(name, value)`
- Strongly typed builder (still experimental)

There are two methods in bUnit that allows passing parameters in C#-based test code:

- `RenderComponent` on the test context
- `SetParametersAndRender` on a rendered component

In the following sub sections we will show both C# and Razor-based test code, just click between them using the tabs.

> [!TIP]
> In all examples below, the <xref:Bunit.ComponentParameterFactory> is imported into the test class using `using static Bunit.ComponentParameterFactory;`. This results in a lot less boilerplate code, which improves test readability. 
> 
> With `using static` import, we can use the factory methods like this:
>   
> ```csharp
> using static Bunit.ComponentParameterFactory;
> ...
> var componentParameter = Parameter("paramName", someValue);
> ```
>   
> With a regular `using` import, we have to prefix the static factory methods like this:
>   
> ```csharp
> using Bunit.ComponentParameterFactory;
> ...
> var componentParameter = ComponentParameterFactory.Parameter("paramName", someValue);
> ```

> [!NOTE]
> The examples below are written using xUnit, but the code is the same with NUnit and MSTest.

## Regular Parameters

A regular parameters is one that is declared using the `[Parameter]` attribute. The following subsections will
cover both _non_ Blazor types parameters, e.g. `int` and `List<string>`, and the special Blazor types like `EventCallback` and `RenderFragment`.

### Non-Blazor Type Parameters

Let's look at an example of passing parameter that takes types which or _not_ special to Blazor, i.e.:

[!code-csharp[AllKindsOfParams.razor](../../../samples/components/AllKindsOfParams.razor#L3-L7)]

Using either C# or Razor test code, this can be done like this:

# [C# test code](#tab/csharp)

[!code-csharp[](../../../samples/tests/xunit/AllKindsOfParamsTest.cs#L17-L39)]

All of these examples does the same thing, here is what is going on:

1. The first example, passes parameters using C# tuples, `(string name, object? value)`.
2. The second example also uses C# tuples to pass the parameters, but the name is retrieved in a refactor safe manner using the `nameof` keyword in C#.
3. The third example uses the <xref:Bunit.ComponentParameterFactory.Parameter(System.String,System.Object)> factory method.
4. The last example uses the <xref:Bunit.ComponentParameterBuilder`1>'s `Add` method, which takes a parameter selector expression that selects the parameter using a lambda, and forces you to provide the correct type for the value. This makes the builders methods strongly typed and refactor safe.

# [Razor test code](#tab/razor)

[!code-html[](../../../samples/tests/razor/AllKindsOfParamsTest.razor#L3-L8)]

This is just regular Blazor parameter passing, which is the same for both `Fixture` and `SnapshotTest` razor tests.

***

### EventCallback Parameters

This example will pass parameters to the follow two `EventCallback` parameters:

[!code-csharp[AllKindsOfParams.razor](../../../samples/components/AllKindsOfParams.razor#L9-L13)]

Using either C# or Razor test code, this can be done like this:

# [C# test code](#tab/csharp)

[!code-csharp[](../../../samples/tests/xunit/AllKindsOfParamsTest.cs#L47-L63)]

All of these examples does the same thing, here is what is going on:

1. The first and second example uses the `EventCallback` factory method in <xref:Bunit.ComponentParameterFactory> (there are many overloads that take different kinds of `Action` and `Func` delegates), to pass a lambda as the event callback to the specified parameter.
2. The last example uses the <xref:Bunit.ComponentParameterBuilder`1>'s `Add` method, which takes a parameter selector expression that selects the parameter using a lambda, and forces you to provide the correct type of callback method. This makes the builders methods strongly typed and refactor safe.

# [Razor test code](#tab/razor)

[!code-html[](../../../samples/tests/razor/AllKindsOfParamsTest.razor#L10-L16)]

This is just regular Blazor parameter passing, which is the same for both `Fixture` and `SnapshotTest` razor tests.

***

### ChildContent Parameters

The `ChildContent` parameter in Blazor is represented by a `RenderFragment`. In Blazor, this can be both contain regular HTML markup, it can be Razor markup, e.g. other component declarations, or a mix of the two. If it is another component, then that component can also receive child content, and so on and so forth.

The following subsections has different examples of child content being passed to the following component:

[!code-csharp[AllKindsOfParams.razor](../../../samples/components/AllKindsOfParams.razor#L15-L16)]

#### Passing HTML to the ChildContent Parameter

# [C# test code](#tab/csharp)

[!code-csharp[](../../../samples/tests/xunit/AllKindsOfParamsTest.cs#L71-L79)]

All of these examples does the same thing, here is what is going on:

1. The first example uses the `ChildContent` factory method in <xref:Bunit.ComponentParameterFactory>, to pass a HTML markup string as the input to the `ChildContent` parameter.
2. The second example uses the <xref:Bunit.ComponentParameterBuilder`1>'s `AddChildContent` method to pass a HTML markup string as the input to the `ChildContent` parameter.

# [Razor test code](#tab/razor)

[!code-html[](../../../samples/tests/razor/AllKindsOfParamsTest.razor#L18-L24)]

This is just regular Blazor child content parameter passing, e.g. as child content to the component under test, which is the same for both `Fixture` and `SnapshotTest` razor tests.

***

#### Passing a Component to the ChildContent Parameter

To pass a component, e.g. the classic `<Counter>` component, which does not take any parameters, to a component under test, do the following:

# [C# test code](#tab/csharp)

[!code-csharp[](../../../samples/tests/xunit/AllKindsOfParamsTest.cs#L87-L95)]

All of these examples does the same thing, here is what is going on:

1. The first example uses the `ChildContent<TChildComponent>` factory method in <xref:Bunit.ComponentParameterFactory>, where `TChildComponent` is the (child) component that should be passed to the component under test.
2. The second example uses the <xref:Bunit.ComponentParameterBuilder`1>'s `AddChildContent<TChildComponent>` method, where `TChildComponent` is the (child) component that should be passed to the component under test.

# [Razor test code](#tab/razor)

[!code-html[](../../../samples/tests/razor/AllKindsOfParamsTest.razor#L26-L32)]

This is just regular Blazor child content parameter passing, where the `<Counter />` component is declared inside the component under test.  This is the same for both `Fixture` and `SnapshotTest` razor tests.

***

#### Passing a Component with Parameters to the ChildContent Parameter

To pass a component with parameters to a component under test, e.g. the `<Alert>` component with the following parameters,, e.g. the classic `<Counter>` component, do the following:

[!code-csharp[Alert.razor](../../../samples/components/Alert.razor#L2-L4)]

# [C# test code](#tab/csharp)

[!code-csharp[](../../../samples/tests/xunit/AllKindsOfParamsTest.cs#L103-L119)]

All of these examples does the same thing, here is what is going on:

1. The first example uses the `ChildContent<TChildComponent>` factory method in <xref:Bunit.ComponentParameterFactory>, where `TChildComponent` is the (child) component that should be passed to the component under test. `ChildContent<TChildComponent>` factory method can take zero or more component parameters as input itself, which will be passed to the `TChildComponent` component, in this case, the `<Alert>` component.
2. The second example uses the <xref:Bunit.ComponentParameterBuilder`1>'s `AddChildContent<TChildComponent>` method, where `TChildComponent` is the (child) component that should be passed to the component under test. The `AddChildContent<TChildComponent>` method takes an optional <xref:Bunit.ComponentParameterBuilder`1> as input, which can be used to pass parameters to the `TChildComponent` component, in this case, the `<Alert>` component.

# [Razor test code](#tab/razor)

[!code-html[](../../../samples/tests/razor/AllKindsOfParamsTest.razor#L34-L42)]

This is just regular Blazor child content parameter passing, where the `<Alert>` component is declared inside the component under test, and any parameters is passed to it like normal in Blazor. This is the same for both `Fixture` and `SnapshotTest` razor tests.

***

#### Passing a mix of Razor and HTML to ChildContent Parameter

It is currently only possible to pass a mix of HTML markup and Razor markup to a ChildContent parameter using Razor based tests. This is illustrated in the example below:

[!code-html[](../../../samples/tests/razor/AllKindsOfParamsTest.razor#L44-L53)]

### RenderFragment Parameters

### Templates Parameters

### Unmatched Parameters

## Cascading Value Parameters

## Render Component Test inside other Components

## Further Reading

- <xref:inject-services-into-components>