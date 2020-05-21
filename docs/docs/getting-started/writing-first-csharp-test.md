---
uid: writing-first-csharp-test
title: Writing the first C# test
---

# Writing the first C# test

Testing Blazor components is a different from testing regular C# classes: Blazor components are *rendered*, they have the *Blazor component life-cycle*, during which we can *provide input* to them and where they *produce output*.

**bUnit** enables you to render the component you want to test, pass in parameters to it, inject services into it, and access the rendered component instance and the markup it has produced.

Rendering a component happens through bUnit's <xref:Bunit.TestContext>, and the result of the rendering, a <xref:Bunit.IRenderedComponent`1>, provides access to the component instance, and the markup produced by the component.

## Rendering a component 

Lets see a simple example, where we test the following `HelloWorld` component:

[!code-cshtml[HelloWorld.razor](../../samples/Components/HelloWorld.razor)]

# [xUnit](#tab/xunit)

[!code-csharp[HelloWorldTest.cs](../../samples/tests/xunit/HelloWorldTest.cs)]

# [NUnit](#tab/nunit)

[!code-csharp[HelloWorldTest.cs](../../samples/tests/nunit/HelloWorldTest.cs)]

> [!NOTE]
> `TestContext` is an ambiguous reference between `<xref:Bunit.TestContext>` and `NUnit.Framework.TestContext`, so you have to specify the `Bunit` namespace when referencing `<xref:Bunit.TestContext>` to resolve the ambiguity for the compiler. Alternatively, you can give bUnit's <xref:Bunit.TestContext> a different name during import, e.g.: `using BunitTestContext = Bunit.TestContext;` 

# [MSTest](#tab/mstest)

[!code-csharp[HelloWorldTest.cs](../../samples/tests/mstest/HelloWorldTest.cs)]

> [!NOTE]
> `TestContext` is an ambiguous reference between `<xref:Bunit.TestContext>` and `Microsoft.VisualStudio.TestTools.UnitTesting.TestContext`, so you have to specify the `Bunit` namespace when referencing `<xref:Bunit.TestContext>` to resolve the ambiguity for the compiler. Alternatively, you can give bUnit's <xref:Bunit.TestContext> a different name during import, e.g.:   
> `using BunitTestContext = Bunit.TestContext;` 

***

In this test, we do the following:

1. New up the disposable <xref:Bunit.TestContext>, and assign it using  `using var` syntax, to avoid unnecessary indention.
2. Render the `HelloWorld` component using <xref:Bunit.TestContext>, which we do through the <xref:Bunit.TestContext.RenderComponent``1(Bunit.Rendering.ComponentParameter[])> method. 
3. Verify the rendered markup from the `HelloWorld` component using the <xref:Bunit.MarkupMatchesAssertExtensions.MarkupMatches(Bunit.IRenderedFragment,System.String,System.String)> method, which performs a semantic comparison of the expected markup with the rendered markup.

> [!TIP]
> Learn more about how the semantic HTML/markup comparison in bUnit work, and how to customize it on the <xref:semantic-html-comparison> page.

