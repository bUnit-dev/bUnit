---
uid: verify-markup
title: Verifying Markup from a Component
---

# Verifying Markup from a Component

When a component is rendered in a test, the result is a <xref:Bunit.IRenderedFragment> or a <xref:Bunit.IRenderedComponent`1>. Through these it is possible to access the rendered markup (HTML) of the component, and in the case of <xref:Bunit.IRenderedComponent`1>, the instance of the component. 

> [!NOTE]
> An <xref:Bunit.IRenderedComponent`1> inherits from <xref:Bunit.IRenderedFragment>. This page will only show how a <xref:Bunit.IRenderedFragment> is used. <xref:Bunit.IRenderedComponent`1> is covered on the <xref:verify-component-state> page.

This page cover the following **verification approaches**:

- Basic verification of raw markup.
- Semantic comparison of markup.
- Inspecting the individual DOM nodes in the DOM tree.
- Diffing of markup between renders.

The following sections will cover each of these.

## Basic Verification of Raw Markup

To access the rendered markup of a component, just use the <xref:Bunit.IRenderedFragment.Markup> property on <xref:Bunit.IRenderedFragment>. It holds the *raw* HTML from the component as a `string`. 

To get the markup as a string, do the following:

[!code-csharp[](../../../samples/tests/xunit/VerifyMarkupExamples.cs?start=16&end=21&highlight=5)]

You can perform standard string assertions against the markup string, e.g. like checking if it contains a value or is empty.

> [!WARNING]
> Be aware that all indentions and whitespace in your components (`.razor` files) are included in the rendered markup, so it is often wise to normalize the markup string a little, e.g. via the string `Trim()` method, to make the tests more stable. Otherwise a change to the formatting in your components might break the tests when it does not need to.

## Semantic Comparison of Markup

Working with raw markup only works well with very simple output, and even then, you have to sanitize it to get stable tests. A much better approach is to use the semantic HTML comparer that comes with bUnit.

### How does the Semantic HTML Comparer Work?

The comparer takes two HTML fragments (e.g. in the form of a C# string) as input, and returns `true` if both HTML fragments will result in the same visual rendered output in a web browser, otherwise it returns `false`.

For example, a web browser will render this HTML:

```html
<span>Foo Bar</span>
```

Exactly the same as this HTML:

```html
<span>
  Foo Bar
</span>
```

That is why it makes sense to allow tests to pass, _even_ when the rendered HTML markup is not entire identical to the expected HTML, from a normal string comparer perspective.

bUnit's semantic HTML comparer safely ignores things like insignificant whitespace and the order of attributes on elements, and many more things. **This leads to much more stable tests, as e.g. a reformatted component doesn't break it's tests because insignificant whitespace changes.**

### The MarkupMatches() Method

The HTML comparer can be easily accessed through `MarkupMatches()` extension methods, available in places that represents HTML fragments in bUnit, i.e. on <xref:Bunit.IRenderedFragment> and the `INode` and `INodeList` types.

In the following examples, the `<Heading>` component listed below will be used as the component under test.

[!code-razor[Heading.razor](../../../samples/components/Heading.razor)]

To use the `MarkupMatches()` method to perform a semantic comparison of the output of the `<Heading>` component, through its <xref:Bunit.IRenderedFragment>, do the following:

[!code-csharp[](../../../samples/tests/xunit/VerifyMarkupExamples.cs?start=27&end=34&highlight=5-9)]

The highlighted line shows the call to the `MarkupMatches()` method. This test passes even though the insignificant whitespace is not exactly the same between the expected HTML string and the raw markup produced by the `<Heading>` component. It even works when the CSS class-list is not in the same order on the `<small>` element.

The `MarkupMatches()` method is also available on `INode` and `INodeList` types, for example:

[!code-csharp[](../../../samples/tests/xunit/VerifyMarkupExamples.cs?start=40&end=45&highlight=5-6)]

Here we use the `Find(string cssSelector)` method to find the `<small>` element, and only verify it and it's content and attributes.

> [!TIP]
> Working with `Find()`, `FindAll()`, `INode` and `INodeList` is covered later on this page.

Text content can also be verified with the `MarkupMatches()` method, e.g. the text inside the `<small>` element. It has the advantage over regular string comparison that it removes insignificant whitespace in the text automatically, even between words, where a normal string `Trim()` method isn't enough. For example:

[!code-csharp[](../../../samples/tests/xunit/VerifyMarkupExamples.cs?start=51&end=56&highlight=5)]

The semantic HTML comparer can be customized to make a test case even more stable and easier to maintain. It is e.g. possible to ignore an element or attribute during comparison, or provide an regular expression to the comparer when comparing a specific element or attribute, to make the comparer work with generated data. Learn more about the customizations options on the <xref:semantic-html-comparison> page.

## Inspecting DOM Nodes

The rendered markup from a component is available as a DOM nodes through the <xref:Bunit.IRenderedFragment.Nodes> property on <xref:Bunit.IRenderedFragment>, as well as the `Find(string cssSelector)` and `FindAll(string cssSelector)` extension methods on <xref:Bunit.IRenderedFragment>.

The <xref:Bunit.IRenderedFragment.Nodes> property and the `FindAll()` method returns an [AngleSharp](https://anglesharp.github.io/) `INodeList` type, and the `Find()` method returns an [AngleSharp](https://anglesharp.github.io/) `IElement` type. 

The DOM API in AngleSharp follows the W3C DOM API specifications and gives you the same results as state of the art browsers implementation of the DOM API in JavaScript does. Besides the official DOM API, AngleSharp and bUnit adds some useful extension methods on top. This makes working with DOM nodes convenient.

### Finding Nodes with the Find() and FindAll() methods

Users of the famous JavaScript framework [jQuery](https://jquery.com/) will recognize the two methods [`Find(string cssSelector)`](xref:Bunit.RenderedFragmentExtensions.Find(Bunit.IRenderedFragment,System.String)) and [`FindAll(string cssSelector)`](xref:Bunit.RenderedFragmentExtensions.FindAll(Bunit.IRenderedFragment,System.String,System.Boolean)). 

- [`Find(string cssSelector)`](xref:Bunit.RenderedFragmentExtensions.Find(Bunit.IRenderedFragment,System.String)) takes a "CSS selector" as input and returns an `IElement` as output, or throws an exception if non is found.
- [`FindAll(string cssSelector)`](xref:Bunit.RenderedFragmentExtensions.FindAll(Bunit.IRenderedFragment,System.String,System.Boolean)) takes a "CSS selector" as input a list of `IElement` elements.

Let's see some examples of using the [`Find(string cssSelector)`](xref:Bunit.RenderedFragmentExtensions.Find(Bunit.IRenderedFragment,System.String)) and [`FindAll(string cssSelector)`](xref:Bunit.RenderedFragmentExtensions.FindAll(Bunit.IRenderedFragment,System.String,System.Boolean)) methods to query the `<FanyTable>` component listed below.

[!code-razor[FanyTable.razor](../../../samples/components/FanyTable.razor)]

To find the `<caption>` element and the first `<td>` elements in each row, do the following:

[!code-csharp[](../../../samples/tests/xunit/VerifyMarkupExamples.cs?start=62&end=67&highlight=5-6)]

Once you have one or more elements, you verify against them by e.g. inspecting their properties through the DOM API. For example:

[!code-csharp[](../../../samples/tests/xunit/VerifyMarkupExamples.cs?start=69&end=71)]

#### Refreshable FindAll() Queries

### Useful DOM Properties and Methods for Asserting 

## Diffing DOM Nodes

- Since first render
- since snapshot
- Assertion helpers for List of IDiff