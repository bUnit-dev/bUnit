---
uid: passing-parameters-to-components
title: Passing Parameters to Components
---

# Passing Parameters to Components

bUnit comes with a number of ways to pass parameters to components under test:

1. In Razor-based tests - those written in `.razor` files - passing parameters is exactly the same as in your normal Blazor pages and components. Just declare the component and assign values to its parameters.

2. For C#-based test code, bUnit provides a few ways to make it easy to pass parameters:   
  - Loosely typed factory methods and simple tuple-based syntax, i.e. `(name, value)`
  - Strongly typed builder (preferred in most cases)

There are two methods in bUnit that allow passing parameters in C#-based test code:

- `RenderComponent` method on the test context, which is used to render a component initially.
- `SetParametersAndRender` method on a rendered component, which is used to pass new parameters to an already rendered component.

In the following sub sections, we will show both C#- and Razor-based test code; just click between them using the tabs.

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

A regular parameter is one that is declared using the `[Parameter]` attribute. The following subsections will cover both _non_-Blazor type parameters, e.g. `int` and `List<string>`, and the special Blazor types like `EventCallback` and `RenderFragment`.

### Non-Blazor Type Parameters

Let's look at an example of passing parameters that takes types which or [__AP: should 'or' be 'are'?__] _not_ special to Blazor, i.e.:

[!code-csharp[NonBlazorTypesParams](../../../samples/components/NonBlazorTypesParams.cs#L10-L17)]

Using either C# or Razor test code, this can be done like this:

# [C# test code](#tab/csharp)

[!code-csharp[](../../../samples/tests/xunit/AllKindsOfParamsTest.cs#L16-L40)]

All of these examples do the same thing. Here's what's going on:

1. The first example passes parameters using C# tuples, `(string name, object? value)`.
2. The second example also uses C# tuples to pass the parameters, but the name is retrieved in a refactor-safe manner using the `nameof` keyword in C#.
3. The third example uses the <xref:Bunit.ComponentParameterFactory.Parameter(System.String,System.Object)> factory method.
4. The last example uses the <xref:Bunit.ComponentParameterCollectionBuilder`1>'s `Add` method, which takes a parameter selector expression that selects the parameter using a lambda, and forces you to provide the correct type for the value. This makes the builder's methods strongly typed and refactor-safe.

# [Razor test code](#tab/razor)

[!code-cshtml[](../../../samples/tests/razor/AllKindsOfParamsTest.razor#L3-L7)]

This is just regular Blazor parameter passing, which is the same for both `Fixture` and `SnapshotTest` razor tests.

***

### EventCallback Parameters

This example will pass parameters to the following two `EventCallback` parameters:

[!code-csharp[EventCallbackParams](../../../samples/components/EventCallbackParams.cs#L10-L17)]

Using either C# or Razor test code, this can be done like this:

# [C# test code](#tab/csharp)

[!code-csharp[](../../../samples/tests/xunit/AllKindsOfParamsTest.cs#L46-L64)]

These examples do the same thing.

The first and second examples use the `EventCallback` factory method in <xref:Bunit.ComponentParameterFactory> (there are many overloads that take different kinds of `Action` and `Func` delegates) to pass a lambda as the event callback to the specified parameter.

The second example uses the <xref:Bunit.ComponentParameterCollectionBuilder`1>'s `Add` method, which takes a parameter selector expression that selects the parameter using a lambda, and forces you to provide the correct type of callback method. This makes the builder's methods strongly typed and refactor-safe.

# [Razor test code](#tab/razor)

[!code-cshtml[](../../../samples/tests/razor/AllKindsOfParamsTest.razor#L9-L14)]

This is just regular Blazor parameter passing, which is the same for both `Fixture` and `SnapshotTest` razor tests.

***

### ChildContent Parameters

The `ChildContent` parameter in Blazor is represented by a `RenderFragment`. In Blazor, this can be regular HTML markup, it can be Razor markup, e.g. other component declarations, or a mix of the two. If it is another component, then that component can also receive child content, and so forth.

The following subsections have different examples of child content being passed to the following component:

[!code-csharp[ChildContentParams.razor](../../../samples/components/ChildContentParams.cs#L10-L14)]

#### Passing HTML to the ChildContent Parameter

# [C# test code](#tab/csharp)

[!code-csharp[](../../../samples/tests/xunit/AllKindsOfParamsTest.cs#L70-L80)]

These examples do the same thing.

The first example uses the `ChildContent` factory method in <xref:Bunit.ComponentParameterFactory> to pass an HTML markup string as the input to the `ChildContent` parameter.

The second example uses the <xref:Bunit.ComponentParameterCollectionBuilder`1>'s `AddChildContent` method to pass an HTML markup string as the input to the `ChildContent` parameter.

# [Razor test code](#tab/razor)

[!code-cshtml[](../../../samples/tests/razor/AllKindsOfParamsTest.razor#L16-L22)]

This is just regular Blazor child content parameter passing, e.g. as child content to the component under test, which is the same for both `Fixture` and `SnapshotTest` razor tests.

***

#### Passing a Component without Parameters to the ChildContent Parameter

To pass a component, e.g. the classic `<Counter>` component, which does not take any parameters itself, to a `ChildContent` parameter, do the following:

# [C# test code](#tab/csharp)

[!code-csharp[](../../../samples/tests/xunit/AllKindsOfParamsTest.cs#L86-L96)]

These examples do the same thing.

The first example uses the `ChildContent<TChildComponent>` factory method in <xref:Bunit.ComponentParameterFactory>, where `TChildComponent` is the (child) component that should be passed to the component under test's `ChildContent` parameter.

The second example uses the <xref:Bunit.ComponentParameterCollectionBuilder`1>'s `AddChildContent<TChildComponent>` method, where `TChildComponent` is the (child) component that should be passed to the component under test's `ChildContent` parameter.

# [Razor test code](#tab/razor)

[!code-cshtml[](../../../samples/tests/razor/AllKindsOfParamsTest.razor#L24-L30)]

This is just regular Blazor child content parameter passing, where the `<Counter />` component is declared inside the component under test.  This is the same for both `Fixture` and `SnapshotTest` razor tests.

***

#### Passing a Component with Parameters to the ChildContent Parameter

To pass a component with parameters to a component under test, e.g. the `<Alert>` component with the following parameters, do the following:

[!code-csharp[Alert.razor](../../../samples/components/Alert.razor#L2-L4)]

# [C# test code](#tab/csharp)

[!code-csharp[](../../../samples/tests/xunit/AllKindsOfParamsTest.cs#L102-L120)]

These examples do the same thing.

The first example uses the `ChildContent<TChildComponent>` factory method in <xref:Bunit.ComponentParameterFactory>, where `TChildComponent` is the (child) component that should be passed to the component under test. `ChildContent<TChildComponent>` factory method can take zero or more component parameters as input itself, which will be passed to the `TChildComponent` component, which in this case is the `<Alert>` component.

The second example uses the <xref:Bunit.ComponentParameterCollectionBuilder`1>'s `AddChildContent<TChildComponent>` method, where `TChildComponent` is the (child) component that should be passed to the component under test. The `AddChildContent<TChildComponent>` method takes an optional <xref:Bunit.ComponentParameterCollectionBuilder`1> as input, which can be used to pass parameters to the `TChildComponent` component, which in this case is the `<Alert>` component.

# [Razor test code](#tab/razor)

[!code-cshtml[](../../../samples/tests/razor/AllKindsOfParamsTest.razor#L32-L40)]

This is just regular Blazor child content parameter passing, where the `<Alert>` component is declared inside the component under test, and any parameters are passed to it as normal in Blazor. This is the same for both `Fixture` and `SnapshotTest` razor tests.

***

#### Passing a mix of Razor and HTML to a ChildContent Parameter

Some times you need to pass multiple different types of content to a ChildContent parameter, e.g. both some markup and a component. This can be done in the following way:

# [C# test code](#tab/csharp)

[!code-csharp[](../../../samples/tests/xunit/AllKindsOfParamsTest.cs#L126-L146)]

Passing a mix of markup and components to a `ChildContent` parameter is done by simply calling the <xref:Bunit.ComponentParameterCollectionBuilder`1>'s `AddChildContent()` methods or using the `ChildContent()` factory methods in <xref:Bunit.ComponentParameterFactory>, as seen here.

# [Razor test code](#tab/razor)

[!code-cshtml[](../../../samples/tests/razor/AllKindsOfParamsTest.razor#L42-L51)]

This is just regular Blazor child content parameter passing, where regular HTML markup and an `<Alert>` component is declared inside the component under test, and any parameters are passed to it as normal in Blazor. This is the same for both `Fixture` and `SnapshotTest` razor tests.

***

### RenderFragment Parameters

A `RenderFragment` parameter is very similar to the special `ChildContent` parameter described in the previous section, since a `ChildContent` parameter _is_ of type `RenderFragment`. The only difference is the name, which must be anything other than `ChildContent`. 

In Blazor, a `RenderFragment` parameter can be regular HTML markup, it can be Razor markup, e.g. other component declarations, or it can be a mix of the two. If it is another component, then that component can also receive child content, and so forth.

The following subsections have different examples of content being passed to the following component's `RenderFragment` parameter:

[!code-csharp[RenderFragmentParams.razor](../../../samples/components/RenderFragmentParams.cs#L9-L13)]

#### Passing HTML to a RenderFragment Parameter

# [C# test code](#tab/csharp)

[!code-csharp[](../../../samples/tests/xunit/AllKindsOfParamsTest.cs#L152-L162)]

These examples do the same thing.

The first example uses the `RenderFragment` factory method in <xref:Bunit.ComponentParameterFactory> to pass an HTML markup string as the input to the `RenderFragment` parameter.

The second example uses the <xref:Bunit.ComponentParameterCollectionBuilder`1>'s `Add` method to pass an HTML markup string as the input to the `RenderFragment` parameter.

# [Razor test code](#tab/razor)

[!code-cshtml[](../../../samples/tests/razor/AllKindsOfParamsTest.razor#L53-L61)]

This is just regular Blazor `RenderFragment` parameter passing as markup, for example in the component under test's `<Content>` element. This is the same for both `Fixture` and `SnapshotTest` razor tests.

***

#### Passing a Component without Parameters to a RenderFragment Parameter

To pass a component such as the classic `<Counter>` component, which does not take any parameters, to a `RenderFragment` parameter, do the following:

# [C# test code](#tab/csharp)

[!code-csharp[](../../../samples/tests/xunit/AllKindsOfParamsTest.cs#L168-L178)]

These examples do the same thing.

The first example uses the `RenderFragment<TChildComponent>` factory method in <xref:Bunit.ComponentParameterFactory>, where `TChildComponent` is the (child) component that should be passed to the `RenderFragment` parameter.

The second example uses the <xref:Bunit.ComponentParameterCollectionBuilder`1>'s `Add<TChildComponent>` method, where `TChildComponent` is the (child) component that should be passed to the `RenderFragment` parameter.

# [Razor test code](#tab/razor)

[!code-cshtml[](../../../samples/tests/razor/AllKindsOfParamsTest.razor#L63-L71)]

This is just regular Blazor child content parameter passing, where the `<Counter />` component is declared inside the component under test's `<Content>` element.  This is the same for both `Fixture` and `SnapshotTest` razor tests.

***

#### Passing a Component with Parameters to a RenderFragment Parameter

To pass a component with parameters to a `RenderFragment` parameter, e.g. the `<Alert>` component with the following parameters, do the following:

[!code-csharp[Alert.razor](../../../samples/components/Alert.razor#L2-L4)]

# [C# test code](#tab/csharp)

[!code-csharp[](../../../samples/tests/xunit/AllKindsOfParamsTest.cs#L184-L202)]

These examples do the same thing.

The first example uses the `RenderFragment<TChildComponent>` factory method in <xref:Bunit.ComponentParameterFactory>, where `TChildComponent` is the (child) component that should be passed to the  `RenderFragment` parameter.  The `RenderFragment<TChildComponent>` factory method takes the name of the parameter and zero or more component parameters as input, which will be passed to the `TChildComponent` component, which in this case is the `<Alert>` component.

The second example uses the <xref:Bunit.ComponentParameterCollectionBuilder`1>'s `Add<TChildComponent>` method, where `TChildComponent` is the (child) component that should be passed to the `RenderFragment` parameter. The `Add<TChildComponent>` method takes an optional <xref:Bunit.ComponentParameterCollectionBuilder`1> as input, which can be used to pass parameters to the `TChildComponent` component, which in this case is the `<Alert>` component.

# [Razor test code](#tab/razor)

[!code-cshtml[](../../../samples/tests/razor/AllKindsOfParamsTest.razor#L73-L83)]

This is just regular Blazor `RenderFragment` parameter passing, where the `<Alert>` component is declared inside the component under test's `<Content>` element, and any parameters are passed to it as normal in Blazor. This is the same for both `Fixture` and `SnapshotTest` razor tests.

***

#### Passing a mix of Razor and HTML to a RenderFragment Parameter  

Some times you need to pass multiple different types of content to a `RenderFragment` parameter, e.g. both markup and and a component. This can be done in the following way:

# [C# test code](#tab/csharp)

[!code-csharp[](../../../samples/tests/xunit/AllKindsOfParamsTest.cs#L208-L228)]

Passing a mix of markup and components to a `RenderFragment` parameter is simply done by calling the <xref:Bunit.ComponentParameterCollectionBuilder`1>'s `Add()` methods or using the `ChildContent()` factory methods in <xref:Bunit.ComponentParameterFactory>, as seen here.

# [Razor test code](#tab/razor)

[!code-cshtml[](../../../samples/tests/razor/AllKindsOfParamsTest.razor#L85-L96)]

This is just regular Blazor `RenderFragment` parameter passing, where regular HTML markup and an `<Alert>` component is declared inside the component under test's `<Content>` element, and any parameters are passed to it as normal in Blazor. This is the same for both `Fixture` and `SnapshotTest` razor tests.

***

### Templates Parameters

Template parameters are closely related to the `RenderFragment` parameters described in the previous section. The difference is that a template parameter is of type `RenderFragment<TValue>`. As with a regular `RenderFragment`, a `RenderFragment<TValue>` template parameter can consist of regular HTML markup, it can be Razor markup, e.g. other component declarations, or it can be a mix of the two. If it is another component, then that component can also receive child content, and so forth.

The following examples renders a template component which has a `RenderFragment<TValue>` template parameter:

[!code-csharp[TemplateParams.razor](../../../samples/components/TemplateParams.razor)]

#### Passing HTML-based templates

To pass a template into a `RenderFragment<TValue>` parameter that just consists of regular HTML markup, do the following:

# [C# test code](#tab/csharp)

[!code-csharp[](../../../samples/tests/xunit/AllKindsOfParamsTest.cs#L234-L246)]

These examples do the same thing, i.e. pass a HTML markup template into the component under test. This is done with the help of a `Func<TValue, string>` delegate which takes whatever the template value is as input, and returns a (markup) string. The delegate is automatically turned into a `RenderFragment<TValue>` type and passed to the template parameter.

The first example passes data to the `Items` parameter, and then it uses the `Template<TValue>` factory method in <xref:Bunit.ComponentParameterFactory>, which takes the name of the `RenderFragment<TValue>` template parameter and the `Func<TValue, string>` delegate as input.

The second example uses the <xref:Bunit.ComponentParameterCollectionBuilder`1>'s `Add` method to first add the data to the `Items` parameter and then to a `Func<TValue, string>` delegate.

The delegate creates a simple markup string in both examples.

# [Razor test code](#tab/razor)

[!code-cshtml[](../../../samples/tests/razor/AllKindsOfParamsTest.razor#L98-L106)]

This is just regular Blazor `RenderFragment<TValue>` parameter passing, in this case to the `Template` parameter. This is the same for both `Fixture` and `SnapshotTest` razor tests.

***

#### Passing a Component-Based Template

To pass a template into a `RenderFragment<TValue>` parameter, which is based on a component that receives the template value as input (in this case, the `<Item>` component listed below), do the following:

[!code-csharp[Item.razor](../../../samples/components/Item.razor)]

# [C# test code](#tab/csharp)

[!code-csharp[](../../../samples/tests/xunit/AllKindsOfParamsTest.cs#L252-L268)]

These examples do the same thing, i.e. create a template with the `<Item>` component listed above. 

# [Razor test code](#tab/razor)

[!code-cshtml[](../../../samples/tests/razor/AllKindsOfParamsTest.razor#L108-L116)]

This is just regular Blazor `RenderFragment<TValue>` parameter passing, in this case to the `Template` parameter. This is the same for both `Fixture` and `SnapshotTest` razor tests.

***

### Unmatched Parameters

An unmatched parameter is a parameter passed to a component under test, which does not have an explicit `[Parameter]` parameter, but instead is captured by a `[Parameter(CaptureUnmatchedValues = true)]` parameter.

In the follow examples, we will pass an unmatched parameter to the following component:

[!code-csharp[UnmatchedParams](../../../samples/components/UnmatchedParams.cs#L10-L14)]

# [C# test code](#tab/csharp)

[!code-csharp[](../../../samples/tests/xunit/AllKindsOfParamsTest.cs#L274-L284)]

These examples do the same thing, i.e. pass in the parameter `some-unknown-param` with the value `a value` to the component under test.

# [Razor test code](#tab/razor)

[!code-cshtml[](../../../samples/tests/razor/AllKindsOfParamsTest.razor#L119-L123)]

This is just regular Blazor parameter passing, which is the same for both `Fixture` and `SnapshotTest` razor tests. In this case, the parameter `some-unknown-param` with the value `a value` is passed to the component under test.

***

## Cascading Parameters and Cascading Values

Cascading parameters are properties with the `[CascadingParameter]` attribute. There are two variants: **named** and **unnamed** cascading parameters. In Blazor, the `<CascadingValue>` component is used to provide values to cascading parameters, which we also do in Razor based tests. However, in C# based tests, we need to do it a little differently.

The following examples will pass cascading values to the `<CascadingParams>` component listed below:

[!code-csharp[CascadingParams.razor](../../../samples/components/CascadingParams.razor)]

### Passing Unnamed Cascading Values

To pass the unnamed `IsDarkTheme` cascading parameter to the `<CascadingParams>` component, do the following:

# [C# test code](#tab/csharp)

[!code-csharp[](../../../samples/tests/xunit/AllKindsOfParamsTest.cs#L290-L306)]

These examples do the same thing, i.e. passing the variable `isDarkTheme` to the cascading parameter `IsDarkTheme`.

1. The first example uses the `CascadingValue` factory method in <xref:Bunit.ComponentParameterFactory> to pass the unnamed parameter value.
2. The second example uses the `Add` method on the <xref:Bunit.ComponentParameterCollectionBuilder`1> to pass the unnamed parameter value.
3. The last example uses the `Add` method on the <xref:Bunit.ComponentParameterCollectionBuilder`1> with the parameter selector to explicitly select the desired cascading parameter and pass the unnamed parameter value that way.

# [Razor test code](#tab/razor)

[!code-cshtml[](../../../samples/tests/razor/AllKindsOfParamsTest.razor#L126-L132)]

This is just regular Blazor cascading value parameter passing, which is the same for both `Fixture` and `SnapshotTest` razor tests. In this case, the `<CascadingValue>` component is used to pass the unnamed parameter value.

***

### Passing Named Cascading Values

To pass a named cascading parameter to the `<CascadingParams>` component, do the following:

# [C# test code](#tab/csharp)

[!code-csharp[](../../../samples/tests/xunit/AllKindsOfParamsTest.cs#L312-L322)]

These examples do the same thing, i.e. pass in value `Egil Hansen` to the cascading parameter with the name `LoggedInUser`. Note that the name of the parameter is not the same as the property of the parameter, e.g. `LoggedInUser` vs. `UserName`.

1. The first example uses the `CascadingValue` factory method in <xref:Bunit.ComponentParameterFactory> to pass the named parameter value, specifying the cascading parameters name and a value (not the property name).
2. The second example uses the `Add` method on the <xref:Bunit.ComponentParameterCollectionBuilder`1> with the parameter selector to select the cascading parameter property and pass the parameter value that way. 

# [Razor test code](#tab/razor)

[!code-cshtml[](../../../samples/tests/razor/AllKindsOfParamsTest.razor#L134-L140)]

This is just regular Blazor cascading value parameter passing, which is the same for both `Fixture` and `SnapshotTest` razor tests. In this case, the `<CascadingValue>` component is used to pass a named parameter value, since both the `Name` and `Value` parameters are specified.

***

### Passing Multiple, Named and Unnamed, Cascading Values

To pass all cascading parameters to the `<CascadingParams>` component, do the following:

# [C# test code](#tab/csharp)

[!code-csharp[](../../../samples/tests/xunit/AllKindsOfParamsTest.cs#L328-L350)]

These examples do the same thing, i.e. passing both the unnamed `IsDarkTheme` cascading parameter and the two named cascading parameters (`LoggedInUser`, `LoggedInEmail`).

1. The first example uses the `CascadingValue` factory method in <xref:Bunit.ComponentParameterFactory> to pass the unnamed and named parameter values.
2. The second example uses the `Add` method on the <xref:Bunit.ComponentParameterCollectionBuilder`1> without a parameter to pass the unnamed parameter value, and `Add` method with the parameter selector to select each of the named parameters to pass the named parameter values.
3. The last example uses the `Add` method on the <xref:Bunit.ComponentParameterCollectionBuilder`1> with the parameter selector to select both the named and unnamed cascading parameters and pass values to them that way.

# [Razor test code](#tab/razor)

[!code-cshtml[](../../../samples/tests/razor/AllKindsOfParamsTest.razor#L142-L152)]

This is just regular Blazor cascading value parameter passing, which is the same for both `Fixture` and `SnapshotTest` razor tests. In this case, multiple `<CascadingValue>` components are used to pass the unnamed and named cascading parameter values to the component.

***

## Rendering a Component Under Test Inside Other Components

It is possible to nest a component under tests inside other components, if that is required to test it. For example, to nest the `<HelloWorld>` component inside the `<Wrapper>` component, do the following:

# [C# test code](#tab/csharp)

[!code-csharp[](../../../samples/tests/xunit/NestedComponentTest.cs#L16-L28)]

These examples do the same thing, i.e. rendering the `<HelloWorld>` component inside the `<Wrapper>` component. What is special in both cases is the use of the `FindComponent<HelloWorld>()`, which returns a `IRenderedComponent<HelloWorld>`, which gives access to only the `<HelloWorld>` components part of the render tree, and the `<HelloWorld>` components instance.

# [Razor test code](#tab/razor)

[!code-cshtml[](../../../samples/tests/razor/NestedComponentTest.razor#L3-)]

This is just regular Blazor child content parameter passing, where one component is rendered inside another, i.e. the `<HelloWorld>` component inside the `<Wrapper>` component. 

The special thing in this case is that the `GetComponentUnderTest<HelloWorld>()` method specifies the `<HelloWorld>` component as its target instead of the outer `<Wrapper>` component. This returns a `IRenderedComponent<HelloWorld>`, which gives access to only the `<HelloWorld>` components part of the render tree, and the `<HelloWorld>` components instance.

***

## Further Reading

- <xref:inject-services>
<!--stackedit_data:
eyJoaXN0b3J5IjpbNjQ1Mjc2OTI1LDQxODk4MDY4NCwyMTEzOT
kxOTQsLTE5NDc3MTQ3ODYsMjIxMzgyNDgxXX0=
-->