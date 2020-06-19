---
uid: writing-csharp-tests
title: Writing Tests in C# for Blazor Components
---

# Writing Tests in C# for Blazor Components

Testing Blazor components is a different from testing regular C# classes: Blazor components are *rendered*, they have the *Blazor component life cycle*, during which we can *provide input* to them and where they *produce output*.

Use **bUnit** to render the component you want to test, pass in parameters to it, inject services into it, and access the rendered component instance and the markup it has produced.

Rendering a component happens through bUnit's <xref:Bunit.TestContext>, and the result of the rendering, a <xref:Bunit.IRenderedComponent`1>, provides access to the component instance, and the markup produced by the component.

## Creating a Basic Test

This is a simple example, that tests the following `<HelloWorld>` component:

[!code-html[HelloWorld.razor](../../../samples/components/HelloWorld.razor)]

# [xUnit](#tab/xunit)

[!code-csharp[HelloWorldTest.cs](../../../samples/tests/xunit/HelloWorldTest.cs)]

# [NUnit](#tab/nunit)

[!code-csharp[HelloWorldTest.cs](../../../samples/tests/nunit/HelloWorldTest.cs)]

> [!NOTE]
> `TestContext` is an ambiguous reference between `TestContext` and `NUnit.Framework.TestContext`, so you have to specify the `Bunit` namespace when referencing `TestContext` to resolve the ambiguity for the compiler. Alternatively, you can give bUnit's `TestContext` a different name during import, e.g.: `using BunitTestContext = Bunit.TestContext;` 

# [MSTest](#tab/mstest)

[!code-csharp[HelloWorldTest.cs](../../../samples/tests/mstest/HelloWorldTest.cs)]

> [!NOTE]
> `TestContext` is an ambiguous reference between `TestContext` and `Microsoft.VisualStudio.TestTools.UnitTesting.TestContext`, so you have to specify the `Bunit` namespace when referencing `TestContext` to resolve the ambiguity for the compiler. Alternatively, you can give bUnit's `TestContext` a different name during import, e.g.:   
> `using BunitTestContext = Bunit.TestContext;` 

***

The following happens in the test above:

1. New up the disposable <xref:Bunit.TestContext>, and assign it using the `using var` syntax, to avoid unnecessary indention.
2. Render the `<HelloWorld>` component using <xref:Bunit.TestContext>, which we do through the <xref:Bunit.TestContext.RenderComponent``1(Bunit.Rendering.ComponentParameter[])> method. We will cover passing parameters to components elsewhere.
3. Verify the rendered markup from the `<HelloWorld>` component using the `MarkupMatches` method, which performs a semantic comparison of the expected markup with the rendered markup.

> [!TIP]
> Learn more about how the semantic HTML/markup comparison in bUnit work, and how to customize it on the <xref:semantic-html-comparison> page.

> [!TIP]
> In bUnit tests, we like to use the abbreviation `CUT`, short for "component under test", to indicate the component that is being tested. This is inspired by the common testing abbreviation `SUT`, short for "system under test".

## Remove Boilerplate Code from Tests

We can remove some boilerplate code from each test by making the <xref:Bunit.TestContext> implicitly available to the test class, so we do not have to have `using var ctx = new Bunit.TestContext();` in every test. This can be done like this:

# [xUnit](#tab/xunit)

[!code-csharp[HelloWorldImplicitContextTest.cs](../../../samples/tests/xunit/HelloWorldImplicitContextTest.cs)]

Since xUnit instantiates test classes for each execution of test methods inside them and disposes them after each test method has run, we simply inherit from <xref:Bunit.TestContext>, and methods like <xref:Bunit.TestContext.RenderComponent``1(Bunit.Rendering.ComponentParameter[])> can now be called directly from each test, as seen in the listing above. 

# [NUnit](#tab/nunit)

[!code-csharp[HelloWorldImplicitContextTest.cs](../../../samples/tests/nunit/HelloWorldImplicitContextTest.cs)]

[!code-csharp[BunitTestContext.cs](../../../samples/tests/nunit/BunitTestContext.cs)]

Since NUnit instantiates the test class is once, we cannot simply inherit directly from <xref:Bunit.TestContext>, as we want a fresh instance of <xref:Bunit.TestContext> for each test. Instead, we create a helper class, `BunitTestContext`, which is listed above, and use that to hook into NUnit's `[SetUp]` and `[TearDown]` methods, which runs before and after each test.

Then methods like <xref:Bunit.TestContext.RenderComponent``1(Bunit.Rendering.ComponentParameter[])> can now be called directly from each test, as seen in the listing above.

# [MSTest](#tab/mstest)

[!code-csharp[HelloWorldImplicitContextTest.cs](../../../samples/tests/mstest/HelloWorldImplicitContextTest.cs)]

[!code-csharp[BunitTestContext.cs](../../../samples/tests/mstest/BunitTestContext.cs)]

Since MSTest instantiates the test class is once, we cannot simply inherit directly from <xref:Bunit.TestContext>, as we want a fresh instance of <xref:Bunit.TestContext> for each test. Instead, we create a helper class, `BunitTestContext`, which is listed above, and use that to hook into MSTest's `[TestInitialize]` and `[TestCleanup]` methods, which runs before and after each test.

Then methods like <xref:Bunit.TestContext.RenderComponent``1(Bunit.Rendering.ComponentParameter[])> can now be called directly from each test, as seen in the listing above.

***

## Further Reading

With the basics out of the way, we will next look at how to pass parameters and inject services into our components under test, and after that, cover in more details the various ways we can verify the outcome of a rendering.

- <xref:passing-parameters-to-components>
- <xref:inject-services>
- <xref:verify-markup>
- <xref:verify-component-state>
- <xref:trigger-event-handlers>