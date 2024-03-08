---
uid: writing-tests
title: Writing tests for Blazor components
---

# Writing tests for Blazor components

Testing Blazor components is a little different from testing regular C# classes: Blazor components are *rendered*, they have the *Blazor component life cycle* during which we can *provide input* to them, and they can *produce output*.

Use **bUnit** to render the component under test, pass in its parameters, inject required services, and access the rendered component instance and the markup it has produced.

Rendering a component happens through bUnit's <xref:Bunit.BunitContext>. The result of the rendering is an `RenderedComponent`, referred to as a "rendered component", that provides access to the component instance and the markup produced by the component.

## Write tests in `.cs` or `.razor` files

bUnit works with MSTest, NUnit, and xUnit, and it allows you to write the unit tests in either `.cs` or `.razor` files.

The latter, writing tests in `.razor` files, provides an easier way to declare component markup and HTML markup in the tests, so it will most likely be the go-to for many people in the future.

*However*, the current Razor editor in Visual Studio 2022 does not offer all the code editing features available in the C# editor, so that is something to consider if you choose to write tests in `.razor` files.

The following sections show how to get started writing tests in either `.cs` or `.razor` files.

## Creating basic tests in `.razor` files

Before writing tests in `.razor` files, a few things needs to be in place:

1. Make sure the test project has the SDK type set to `Microsoft.NET.Sdk.Razor`. Otherwise the Blazor compiler will not translate your `.razor` files into runnable code.
2. Add an `_Imports.razor` file to the test project. It serves the same purpose as `_Imports.razor` files in regular Blazor projects. These using statements are useful to add right away:

   ```cshtml
   @using Microsoft.AspNetCore.Components.Forms
   @using Microsoft.AspNetCore.Components.Web
   @using Microsoft.JSInterop
   @using Microsoft.Extensions.DependencyInjection
   @using AngleSharp.Dom
   @using Bunit
   @using Bunit.TestDoubles
   ```

Also add an using statement for your general purpose testing framework, e.g. `@using Xunit` for xUnit.

With that in place, lets look at a simple example that tests the following `<HelloWorld>` component:

[!code-cshtml[HelloWorld.razor](../../../samples/components/HelloWorld.razor)]

# [xUnit](#tab/xunit)

[!code-cshtml[HelloWorldRazorTest.razor](../../../samples/tests/xunit/HelloWorldRazorTest.razor)]

The test above does the following:

1. Inherits from the bUnit <xref:Bunit.BunitContext>. This base class offers the majority of functions.
2. Renders the `<HelloWorld>` component using <xref:Bunit.BunitContext>, which is done through the `Render(RenderFragment)` method. We cover passing parameters to components on the <xref:passing-parameters-to-components> page.
3. Verifies the rendered markup from the `<HelloWorld>` component using the `MarkupMatches` method. The `MarkupMatches` method performs a semantic comparison of the expected markup with the rendered markup.

# [NUnit](#tab/nunit)

[!code-cshtml[HelloWorldRazorTest.razor](../../../samples/tests/nunit/HelloWorldRazorTest.razor)]

Use the [LifeCycle.InstancePerTestCase](https://docs.nunit.org/articles/nunit/writing-tests/attributes/fixturelifecycle.html) attribute (introduced in NUnit 3.13) so that a new instance of the test class is created for each test removing the need for the wrapper.

[!code-csharp[HelloWorldInstancePerTestCase.cs](../../../samples/tests/nunit/HelloWorldInstancePerTestCase.cs#L5-L17)]

# [MSTest](#tab/mstest)

[!code-cshtml[HelloWorldRazorTest.razor](../../../samples/tests/mstest/HelloWorldRazorTest.razor)]


***

> [!TIP]
> Learn more about how the semantic HTML/markup comparison in bUnit works, and how to customize it, on the <xref:semantic-html-comparison> page.

> [!TIP]
> In bUnit tests, we like to use the abbreviation `CUT`, short for "component under test", to indicate the component that is being tested. This is inspired by the common testing abbreviation `SUT`, short for "system under test".

### Secret sauce of '.razor' files tests

The trick employed in these tests is the "[inline Razor templates syntax](https://docs.microsoft.com/en-us/aspnet/core/blazor/components/?view=aspnetcore-5.0#razor-templates)", i.e. where a render fragment is simply created using the `@<{HTML tag}>...</{HTML tag}>` notation. In that notation there is no need to do any escaping of e.g. the quotation mark (`"`), that is usually associated with working with markup in C# code.

One small caveat to be aware of is that the inline Razor templates syntax only supports one outer element, e.g. this is OK:

```cshtml
@<Foo>
   <Bar>
     <Baz>...</Baz>
   </Bar>
 </Foo>
```

However, this will **not** work:

```cshtml
@<Foo></Foo>
 <Bar></Bar>
```

There is a simple workaround though: wrap all elements in the special Blazor element `<text>`. The `<text>` element will not be part of the rendered output, but it provides a simple way to group multiple root elements into a single inline Razor template. E.g.:

```cshtml
@<text>
   <Foo></Foo>
   <Bar></Bar>
 </text>
```

## Creating basic tests in `.cs` files

This is a simple example of writing tests in `.cs` files which tests the following `<HelloWorld>` component:

[!code-cshtml[HelloWorld.razor](../../../samples/components/HelloWorld.razor)]

# [xUnit](#tab/xunit)

[!code-csharp[HelloWorldTest.cs](../../../samples/tests/xunit/HelloWorldTest.cs#L3-L17)]

The test above does the following:

1. Inherits from the bUnit's `BunitContext`. This base class offers the majority of functions.
2. Renders the `<HelloWorld>` component using <xref:Bunit.BunitContext>, which is done through the <xref:Bunit.BunitContext.Render``1(System.Action{Bunit.ComponentParameterCollectionBuilder{``0}})> method. We cover passing parameters to components on the <xref:passing-parameters-to-components> page.
3. Verifies the rendered markup from the `<HelloWorld>` component using the `MarkupMatches` method. The `MarkupMatches` method performs a semantic comparison of the expected markup with the rendered markup.

# [NUnit](#tab/nunit)

[!code-csharp[HelloWorldTest.cs](../../../samples/tests/nunit/HelloWorldTest.cs#L3-L20)]

The test above does the following:

1. Inherits from the `BunitContext` listed above. This base class offers the majority of functions.
2. Renders the `<HelloWorld>` component using <xref:Bunit.BunitContext>, which is done through the <xref:Bunit.BunitContext.Render``1(System.Action{Bunit.ComponentParameterCollectionBuilder{``0}})> method. We cover passing parameters to components on the <xref:passing-parameters-to-components> page.
3. Verifies the rendered markup from the `<HelloWorld>` component using the `MarkupMatches` method. The `MarkupMatches` method performs a semantic comparison of the expected markup with the rendered markup.

Alternatively, use the [LifeCycle.InstancePerTestCase](https://docs.nunit.org/articles/nunit/writing-tests/attributes/fixturelifecycle.html) attribute (introduced in NUnit 3.13) so that a new instance of the test class is created for each test removing the need for the wrapper.

[!code-csharp[HelloWorldRazorInstancePerTestCase.razor](../../../samples/tests/nunit/HelloWorldInstancePerTestCase.cs)]

# [MSTest](#tab/mstest)

[!code-csharp[HelloWorldTest.cs](../../../samples/tests/mstest/HelloWorldTest.cs#L3-L20)]

The test above does the following:

1. Inherits from the `BunitContext` listed above. This base class offers the majority of functions.
2. Renders the `<HelloWorld>` component using <xref:Bunit.BunitContext>, which is done through the <xref:Bunit.BunitContext.Render``1(System.Action{Bunit.ComponentParameterCollectionBuilder{``0}})> method. We cover passing parameters to components on the <xref:passing-parameters-to-components> page.
3. Verifies the rendered markup from the `<HelloWorld>` component using the `MarkupMatches` method. The `MarkupMatches` method performs a semantic comparison of the expected markup with the rendered markup.

***

> [!TIP]
> Learn more about how the semantic HTML/markup comparison in bUnit works, and how to customize it, on the <xref:semantic-html-comparison> page.

> [!TIP]
> In bUnit tests, we like to use the abbreviation `CUT`, short for "component under test", to indicate the component that is being tested. This is inspired by the common testing abbreviation `SUT`, short for "system under test".

### Instantiate BunitContext in each test

If you prefer to instantiate <xref:Bunit.BunitContext> in each test, instead of inheriting from it, you can do so. This can be useful if you have your own base class that you want to inherit from, or if you want to use a different test framework than the ones listed here.

Just be aware that all examples in the rest of the documentation assumes that you are inheriting from <xref:Bunit.BunitContext>, so adjust accordingly.

# [xUnit](#tab/xunit)

[!code-csharp[HelloWorldExplicitContextTest.cs](../../../samples/tests/xunit/HelloWorldExplicitContextTest.cs#L6-L20)]

# [NUnit](#tab/nunit)

[!code-csharp[HelloWorldExplicitContextTest.cs](../../../samples/tests/nunit/HelloWorldExplicitContextTest.cs#L6-L20)]

# [MSTest](#tab/mstest)

[!code-csharp[HelloWorldImplicitContextTest.cs](../../../samples/tests/mstest/HelloWorldExplicitContextTest.cs#L6-L21)]

## Further reading

With the basics out of the way, next we will look at how to pass parameters and inject services into our component under test. After that, we will cover the ways in which we can verify the outcome of a rendering in more detail

- <xref:passing-parameters-to-components>
- <xref:inject-services>
- <xref:verify-markup>
- <xref:verify-component-state>
- <xref:trigger-event-handlers>

