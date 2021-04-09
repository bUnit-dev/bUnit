---
uid: writing-tests
title: Writing tests for Blazor components
---

# Writing tests for Blazor components

Testing Blazor components is a little different from testing regular C# classes: Blazor components are *rendered*, they have the *Blazor component life cycle* during which we can *provide input* to them, and they can *produce output*.

Use **bUnit** to render the component under test, pass in its parameters, inject required services, and access the rendered component instance and the markup it has produced.

Rendering a component happens through bUnit's <xref:Bunit.TestContext>. The result of the rendering is an `IRenderedComponent`, referred to as a "rendered component", that provides access to the component instance and the markup produced by the component.

> [!NOTE]
> The preview and beta versions of bUnit included an experimental feature for writing tests using two test components, `<Fixture>` and `<SnapshotTest>`. Since there is now a better way to write tests in `.razor` files, the old experimental feature has been moved into a separate project named `bunit.web.testcomponents`. It is kept around to not break early adopters, but _no additional features or improvements_ is planned to it.
> 
> To learn more, head over to the [bunit.web.testcomponents project](https://github.com/egil/bUnit/tree/main/src/bunit.web.testcomponents) on GitHub.

## Write tests in `.cs` or `.razor` files

bUnit works with MSTest, NUnit, and xUnit, and it allows you to write the unit tests in either `.cs` or `.razor` files. 

The latter, writing tests in `.razor` files, provides an easier way to declare component- and HTML-markup in the tests, so it will most likely be the go to for many people in the future.

*However*, the current Razor editor in Visual Studio 2019 does not offer all the code editing features available in the C# editor, and has some formatting bugs on top of that, so that is something to consider if you choose to write tests in `.razor` files.

The following sections will show how to get started writing tests in either `.cs` or `.razor` files.

## Creating basic tests in `.razor` files

Before writing tests in `.razor` files, a few things needs to be in place:

1. Make sure the test project has the SDK type set to `Microsoft.NET.Sdk.Razor`. Otherwise the Blazor compiler will not translate your `.razor` files into runnable code.
2. Add a `_Imports.razor` file to the test project. It serves the same purpose as `_Imports.razor` files in regular Blazor projects. These using statements are useful to add right away:   
   
```cshtml
@using Microsoft.AspNetCore.Components.Forms
@using Microsoft.AspNetCore.Components.Web
@using Microsoft.JSInterop
@using Microsoft.Extensions.DependencyInjection
@using AngleSharp.Dom
@using Bunit
@using Bunit.TestDoubles
```
    
Also add a using statement for your general purpose testing framework, e.g. `@using Xunit` for xUnit.

With that in place, lets look at a simple example that tests the following `<HelloWorld>` component:

[!code-cshtml[HelloWorld.razor](../../../samples/components/HelloWorld.razor)]

# [xUnit](#tab/xunit)

[!code-cshtml[HelloWorldRazorTest.razor](../../../samples/tests/xunit/HelloWorldRazorTest.razor)]

# [NUnit](#tab/nunit)

[!code-cshtml[HelloWorldRazorTest.razor](../../../samples/tests/nunit/HelloWorldRazorTest.razor)]

> [!NOTE]
> `TestContext` is an ambiguous reference - it could mean `Bunit.TestContext` or `NUnit.Framework.TestContext` - so you have to specify the `Bunit` namespace when referencing `TestContext` to resolve the ambiguity for the compiler.

# [MSTest](#tab/mstest)

[!code-cshtml[HelloWorldRazorTest.razor](../../../samples/tests/mstest/HelloWorldRazorTest.razor)]

> [!NOTE]
> `TestContext` is an ambiguous reference - it could mean `Bunit.TestContext` or `Microsoft.VisualStudio.TestTools.UnitTesting.TestContext` - so you have to specify the `Bunit` namespace when referencing `TestContext` to resolve the ambiguity for the compiler.

***

The test above does the following:

1. Creates a new instance of the disposable bUnit <xref:Bunit.TestContext>, and assigns it to `ctx` variable using the `using var` syntax to avoid unnecessary source code indention.
2. Renders the `<HelloWorld>` component using <xref:Bunit.TestContext>, which is done through the <xref:Bunit.TestContext.Render(Microsoft.AspNetCore.Components.RenderFragment)> method. We cover passing parameters to components on the <xref:passing-parameters-to-components> page.
3. Verifies the rendered markup from the `<HelloWorld>` component using the `MarkupMatches` method. The `MarkupMatches` method performs a semantic comparison of the expected markup with the rendered markup.

> [!TIP]
> Learn more about how the semantic HTML/markup comparison in bUnit works, and how to customize it, on the <xref:semantic-html-comparison> page.

> [!TIP]
> In bUnit tests, we like to use the abbreviation `CUT`, short for "component under test", to indicate the component that is being tested. This is inspired by the common testing abbreviation `SUT`, short for "system under test".

### Secret sauce of '.razor' files tests

The trick employed in these tests is the "[inline Razor templates syntax](https://docs.microsoft.com/en-us/aspnet/core/blazor/components/?view=aspnetcore-5.0#razor-templates)", i.e. where a render fragment is simply created using the `@<{HTML tag}>...</{HTML tag}>` notation. In that notation there is no need to do any escaping of e.g. the quatation mark (`"`), that is usually associated with working with markup in C# code.

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

There is a simple workaround though. Wrap all elements in the special Blazor element `<text>`. The `<text>` element will not be part of the rendered output, but is provides a simple way to group multiple root elements into a single inline Razor templates. E.g.:

```cshtml
@<text>
   <Foo></Foo>
   <Bar></Bar>
 </text>
```

### Remove boilerplate code from tests

We can remove some boilerplate code from each test by making the <xref:Bunit.TestContext> implicitly available to the test class, so we don't have to have `using var ctx = new Bunit.TestContext();` in every test. This can be done like this:

# [xUnit](#tab/xunit)

[!code-cshtml[HelloWorldImplicitContextRazorTest.razor](../../../samples/tests/xunit/HelloWorldImplicitContextRazorTest.razor)]

Since xUnit instantiates test classes for each execution of the test methods inside them, and disposes of them after each test method has run, we simply inherit from <xref:Bunit.TestContext>, and methods like <xref:Bunit.TestContext.Render(Microsoft.AspNetCore.Components.RenderFragment)> can then be called directly from each test. This is seen in the listing above. 

# [NUnit](#tab/nunit)

[!code-cshtml[HelloWorldImplicitContextRazorTest.razor](../../../samples/tests/nunit/HelloWorldImplicitContextRazorTest.razor)]

[!code-csharp[BunitTestContext.cs](../../../samples/tests/nunit/BunitTestContext.cs)]

Since NUnit instantiates a test class only once for all tests inside it, we cannot simply inherit directly from <xref:Bunit.TestContext> as we want a fresh instance of <xref:Bunit.TestContext> for each test. Instead, we create a helper class, `BunitTestContext`, listed above, and use that to hook into NUnit's `[SetUp]` and `[TearDown]` methods, which runs before and after each test. The `BunitTestContext` class inherits from the <xref:Bunit.TestContextWrapper> type, which is included specifically to make this scenario easier.

Then methods like <xref:Bunit.TestContext.Render(Microsoft.AspNetCore.Components.RenderFragment)> can be called directly from each test, as seen in the listing above.

# [MSTest](#tab/mstest)

[!code-cshtml[HelloWorldImplicitContextRazorTest.razor](../../../samples/tests/mstest/HelloWorldImplicitContextRazorTest.razor)]

[!code-csharp[BunitTestContext.cs](../../../samples/tests/mstest/BunitTestContext.cs)]

Since MSTest instantiates a test class only once for all tests inside it, we cannot simply inherit directly from <xref:Bunit.TestContext> as we want a fresh instance of <xref:Bunit.TestContext> for each test. Instead, we create a helper class, `BunitTestContext`, listed above, and use that to hook into MSTest's `[TestInitialize]` and `[TestCleanup]` methods. This runs before and after each test. The `BunitTestContext` class inherits from the <xref:Bunit.TestContextWrapper> type, which is included specifically to make this scenario easier.

Then methods like <xref:Bunit.TestContext.Render(Microsoft.AspNetCore.Components.RenderFragment)> can be called directly from each test, as seen in the listing above.

***

> [!IMPORTANT]
> All the examples in the documentation explicitly new up a `TestContext`, i.e. `using var ctx = new TestContext()`. If you are using the trick above and have your test class inherit from `TestContext`, you should **NOT** new up a `TestContext` in test methods also. 
> 
> Simply call the test contest's methods directly, as they are available in your test class. 
> 
> For example, `var cut = ctx.Render(@<HelloWorld/>);`  
> becomes `var cut = Render(@<HelloWorld/>);`.

## Creating basic tests in `.cs` files

This is a simple example of writing tests in `.cs` files which tests the following `<HelloWorld>` component:

[!code-cshtml[HelloWorld.razor](../../../samples/components/HelloWorld.razor)]

# [xUnit](#tab/xunit)

[!code-csharp[HelloWorldTest.cs](../../../samples/tests/xunit/HelloWorldTest.cs)]

# [NUnit](#tab/nunit)

[!code-csharp[HelloWorldTest.cs](../../../samples/tests/nunit/HelloWorldTest.cs)]

> [!NOTE]
> `TestContext` is an ambiguous reference - it could mean `Bunit.TestContext` or `NUnit.Framework.TestContext` - so you have to specify the `Bunit` namespace when referencing `TestContext` to resolve the ambiguity for the compiler. Alternatively, you can give bUnit's `TestContext` a different name during import, e.g.: `using BunitTestContext = Bunit.TestContext;` 

# [MSTest](#tab/mstest)

[!code-csharp[HelloWorldTest.cs](../../../samples/tests/mstest/HelloWorldTest.cs)]

> [!NOTE]
> `TestContext` is an ambiguous reference - it could mean `Bunit.TestContext` or `Microsoft.VisualStudio.TestTools.UnitTesting.TestContext` - so you have to specify the `Bunit` namespace when referencing `TestContext` to resolve the ambiguity for the compiler. Alternatively, you can give bUnit's `TestContext` a different name during import, e.g.:   
> `using BunitTestContext = Bunit.TestContext;` 

***

The test above does the following:

1. Creates a new instance of the disposable bUnit <xref:Bunit.TestContext>, and assigns it to `ctx` variable using the `using var` syntax to avoid unnecessary source code indention.
2. Renders the `<HelloWorld>` component using <xref:Bunit.TestContext>, which is done through the <xref:Bunit.TestContext.RenderComponent``1(System.Action{Bunit.ComponentParameterCollectionBuilder{``0}})> method. We cover passing parameters to components on the <xref:passing-parameters-to-components> page.
3. Verifies the rendered markup from the `<HelloWorld>` component using the `MarkupMatches` method. The `MarkupMatches` method performs a semantic comparison of the expected markup with the rendered markup.

> [!TIP]
> Learn more about how the semantic HTML/markup comparison in bUnit works, and how to customize it, on the <xref:semantic-html-comparison> page.

> [!TIP]
> In bUnit tests, we like to use the abbreviation `CUT`, short for "component under test", to indicate the component that is being tested. This is inspired by the common testing abbreviation `SUT`, short for "system under test".

### Remove boilerplate code from tests

We can remove some boilerplate code from each test by making the <xref:Bunit.TestContext> implicitly available to the test class, so we don't have to have `using var ctx = new Bunit.TestContext();` in every test. This can be done like this:

# [xUnit](#tab/xunit)

[!code-csharp[HelloWorldImplicitContextTest.cs](../../../samples/tests/xunit/HelloWorldImplicitContextTest.cs)]

Since xUnit instantiates test classes for each execution of the test methods inside them, and disposes of them after each test method has run, we simply inherit from <xref:Bunit.TestContext>, and methods like <xref:Bunit.TestContext.RenderComponent``1(System.Action{Bunit.ComponentParameterCollectionBuilder{``0}})> can then be called directly from each test. This is seen in the listing above. 

# [NUnit](#tab/nunit)

[!code-csharp[HelloWorldImplicitContextTest.cs](../../../samples/tests/nunit/HelloWorldImplicitContextTest.cs)]

[!code-csharp[BunitTestContext.cs](../../../samples/tests/nunit/BunitTestContext.cs)]

Since NUnit instantiates a test class only once for all tests inside it, we cannot simply inherit directly from <xref:Bunit.TestContext> as we want a fresh instance of <xref:Bunit.TestContext> for each test. Instead, we create a helper class, `BunitTestContext`, listed above, and use that to hook into NUnit's `[SetUp]` and `[TearDown]` methods, which runs before and after each test.

Then methods like <xref:Bunit.TestContext.RenderComponent``1(System.Action{Bunit.ComponentParameterCollectionBuilder{``0}})> can be called directly from each test, as seen in the listing above.

# [MSTest](#tab/mstest)

[!code-csharp[HelloWorldImplicitContextTest.cs](../../../samples/tests/mstest/HelloWorldImplicitContextTest.cs)]

[!code-csharp[BunitTestContext.cs](../../../samples/tests/mstest/BunitTestContext.cs)]

Since MSTest instantiates a test class only once for all tests inside it, we cannot simply inherit directly from <xref:Bunit.TestContext> as we want a fresh instance of <xref:Bunit.TestContext> for each test. Instead, we create a helper class, `BunitTestContext`, listed above, and use that to hook into MSTest's `[TestInitialize]` and `[TestCleanup]` methods. This runs before and after each test.

Then methods like <xref:Bunit.TestContext.RenderComponent``1(System.Action{Bunit.ComponentParameterCollectionBuilder{``0}})> can be called directly from each test, as seen in the listing above.

***

> [!IMPORTANT]
> All the examples in the documentation explicitly new up a `TestContext`, i.e. `using var ctx = new TestContext()`. If you are using the trick above and have your test class inherit from `TestContext`, you should **NOT** new up a `TestContext` in test methods also. 
> 
> Simply call the test contest's methods directly, as they are available in your test class. 
> 
> For example, `var cut = ctx.RenderComponent<HelloWorld>();`  
> becomes `var cut = RenderComponent<HelloWorld>();`.

## Further Reading

With the basics out of the way, next we will look at how to pass parameters and inject services into our component under test. After that, we will cover ways we can verify the outcome of a rendering in more detail

- <xref:passing-parameters-to-components>
- <xref:inject-services>
- <xref:verify-markup>
- <xref:verify-component-state>
- <xref:trigger-event-handlers>

