---
uid: writing-csharp-tests
title: Writing Tests in C# for Blazor Components
---

# Writing Tests in C# for Blazor Components

Testing Blazor components is a little different from testing regular C# classes: Blazor components are *rendered*, they have the *Blazor component life cycle* during which we can *provide input* to them, and they can *produce output*.

Use **bUnit** to render the component under test, pass in its parameters, inject required services, and access the rendered component instance and the markup it has produced.

Rendering a component happens through bUnit's <xref:Bunit.TestContext>. The result of the rendering - a <xref:Bunit.IRenderedComponent`1> - provides access to the component instance and the markup produced by the component.

## Creating a Basic Test

This is a simple example that tests the following `<HelloWorld>` component:

[!code-cshtml[HelloWorld.razor](../../../samples/components/HelloWorld.razor)]

# [xUnit](#tab/xunit)

[!code-csharp[HelloWorldTest.cs](../../../samples/tests/xunit/HelloWorldTest.cs)]

# [NUnit](#tab/nunit)

[!code-csharp[HelloWorldTest.cs](../../../samples/tests/nunit/HelloWorldTest.cs)]

> [!NOTE]
> `TestContext` is an ambiguous reference - it could mean `TestContext` or `NUnit.Framework.TestContext` - so you have to specify the `Bunit` namespace when referencing `TestContext` to resolve the ambiguity for the compiler. Alternatively, you can give bUnit's `TestContext` a different name during import, e.g.: `using BunitTestContext = Bunit.TestContext;` 

# [MSTest](#tab/mstest)

[!code-csharp[HelloWorldTest.cs](../../../samples/tests/mstest/HelloWorldTest.cs)]

> [!NOTE]
> `TestContext` is an ambiguous reference - it could mean `TestContext` or `Microsoft.VisualStudio.TestTools.UnitTesting.TestContext` - so you have to specify the `Bunit` namespace when referencing `TestContext` to resolve the ambiguity for the compiler. Alternatively, you can give bUnit's `TestContext` a different name during import, e.g.:   
> `using BunitTestContext = Bunit.TestContext;` 

***

The test above does the following:

1. Creates a new instance of the disposable bUnit <xref:Bunit.TestContext>, and assigns it to `ctx` variable using the `using var` syntax to avoid unnecessary source code indention.
2. Renders the `<HelloWorld>` component using <xref:Bunit.TestContext>, which is done through the <xref:Bunit.TestContext.RenderComponent``1(Bunit.Rendering.ComponentParameter[])> method. We cover passing parameters to components on the <xref:passing-parameters-to-components> page.
3. Verifies the rendered markup from the `<HelloWorld>` component using the `MarkupMatches` method. The `MarkupMatches` method performs a semantic comparison of the expected markup with the rendered markup.

> [!TIP]
> Learn more about how the semantic HTML/markup comparison in bUnit works, and how to customize it, on the <xref:semantic-html-comparison> page.

> [!TIP]
> In bUnit tests, we like to use the abbreviation `CUT`, short for "component under test", to indicate the component that is being tested. This is inspired by the common testing abbreviation `SUT`, short for "system under test".

## Remove Boilerplate Code from Tests

We can remove some boilerplate code from each test by making the <xref:Bunit.TestContext> implicitly available to the test class, so we don't have to have `using var ctx = new Bunit.TestContext();` in every test. This can be done like this:

# [xUnit](#tab/xunit)

[!code-csharp[HelloWorldImplicitContextTest.cs](../../../samples/tests/xunit/HelloWorldImplicitContextTest.cs)]

Since xUnit instantiates test classes for each execution of the test methods inside them, and disposes of them after each test method has run, we simply inherit from <xref:Bunit.TestContext>, and methods like <xref:Bunit.TestContext.RenderComponent``1(Bunit.Rendering.ComponentParameter[])> can now be called directly from each test. This is seen in the listing above. 

# [NUnit](#tab/nunit)

[!code-csharp[HelloWorldImplicitContextTest.cs](../../../samples/tests/nunit/HelloWorldImplicitContextTest.cs)]

[!code-csharp[BunitTestContext.cs](../../../samples/tests/nunit/BunitTestContext.cs)]

Since NUnit instantiates a test class only once for all tests inside it, we cannot simply inherit directly from <xref:Bunit.TestContext> as we want a fresh instance of <xref:Bunit.TestContext> for each test. Instead, we create a helper class, `BunitTestContext`, which is listed above, and use that to hook into NUnit's `[SetUp]` and `[TearDown]` methods, which runs before and after each test.

Methods like <xref:Bunit.TestContext.RenderComponent``1(Bunit.Rendering.ComponentParameter[])> can then be called directly from each test, as seen in the listing above.

# [MSTest](#tab/mstest)

[!code-csharp[HelloWorldImplicitContextTest.cs](../../../samples/tests/mstest/HelloWorldImplicitContextTest.cs)]

[!code-csharp[BunitTestContext.cs](../../../samples/tests/mstest/BunitTestContext.cs)]

Since MSTest instantiates a test class only once for all tests inside it, we cannot simply inherit directly from <xref:Bunit.TestContext> as we want a fresh instance of <xref:Bunit.TestContext> for each test. Instead, we create a helper class, `BunitTestContext`, which is listed above, and use that to hook into MSTest's `[TestInitialize]` and `[TestCleanup]` methods. This runs before and after each test.

Then methods like <xref:Bunit.TestContext.RenderComponent``1(Bunit.Rendering.ComponentParameter[])> can now be called directly from each test, as seen in the listing above.

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
<!--stackedit_data:
eyJoaXN0b3J5IjpbLTYxOTcxMjY5Nyw3Nzc4NTcxODZdfQ==
-->