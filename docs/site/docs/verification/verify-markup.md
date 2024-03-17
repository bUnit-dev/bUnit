---
uid: verify-markup
title: Verifying markup from a component
---

# Verifying markup from a component

Generally, the strategy for verifying markup produced by components depends on whether you are creating reusable component library or a single-use Blazor app component.

With a **reusable component library**, the markup produced may be considered part of the externally observable behavior of the component, and that should thus be verified, since users of the component may depend on the markup having a specific structure. Consider using `MarkupMatches` and semantic comparison described below to get the best protection against regressions and good maintainability.

When **building components for a Blazor app**, the externally observable behavior of components are how they visibly look and behave from an end-users point of view, e.g. what the user sees and interact with in a browser. In this scenario, consider use `FindByLabelText` and related methods described below to inspect and assert against individual elements look and feel, for a good balance between protection against regressions and maintainability. Learn more about this testing approach at https://testing-library.com.

This page covers the following **verification approaches:**

- Inspecting the individual DOM nodes in the DOM tree
- Semantic comparison of markup
- Finding expected differences in markup between renders
- Verification of raw markup

The following sections will cover each of these.

## Result of rendering components

When a component is rendered in a test, the result is a <xref:Bunit.IRenderedFragment> or a <xref:Bunit.IRenderedComponent`1>. Through these, it is possible to access the rendered markup (HTML) of the component and, in the case of <xref:Bunit.IRenderedComponent`1>, the instance of the component. 

> [!NOTE]
> An <xref:Bunit.IRenderedComponent`1> inherits from <xref:Bunit.IRenderedFragment>. This page will only cover features of the <xref:Bunit.IRenderedFragment> type. <xref:Bunit.IRenderedComponent`1> is covered on the <xref:verify-component-state> page.

## Inspecting DOM nodes

The rendered markup from a component is available as a DOM node through the <xref:Bunit.IRenderedFragment.Nodes> property on <xref:Bunit.IRenderedFragment>. The nodes and element types comes from [AngleSharp](https://anglesharp.github.io/) that follows the W3C DOM API specifications and gives you the same results as a state-of-the-art browser’s implementation of the DOM API in JavaScript. Besides the official DOM API, AngleSharp and bUnit add some useful extension methods on top. This makes working with DOM nodes convenient.

### Finding DOM elements

bUnit supports multiple different ways of searching and querying the rendered HTML elements:

- `FindByLabelText(string labelText)` that takes a text string used to label an input element and returns an `IElement` as output, or throws an exception if none are found (this is included in the experimental library [bunit.web.query](https://www.nuget.org/packages/bunit.web.query)). Use this method when possible compared to the generic `Find` and `FindAll` methods.
- [`Find(string cssSelector)`](xref:Bunit.RenderedFragmentExtensions.Find(Bunit.IRenderedFragment,System.String)) takes a "CSS selector" as input and returns an `IElement` as output, or throws an exception if none are found.
- [`FindAll(string cssSelector)`](xref:Bunit.RenderedFragmentExtensions.FindAll(Bunit.IRenderedFragment,System.String,System.Boolean)) takes a "CSS selector" as input and returns a list of `IElement` elements.

Let's see some examples of using the [`Find(string cssSelector)`](xref:Bunit.RenderedFragmentExtensions.Find(Bunit.IRenderedFragment,System.String)) and [`FindAll(string cssSelector)`](xref:Bunit.RenderedFragmentExtensions.FindAll(Bunit.IRenderedFragment,System.String,System.Boolean)) methods to query the `<FancyTable>` component listed below.

[!code-razor[FancyTable.razor](../../../samples/components/FancyTable.razor)]

To find the `<caption>` element and the first `<td>` elements in each row, do the following:

[!code-csharp[](../../../samples/tests/xunit/VerifyMarkupExamples.cs?start=54&end=57&highlight=3-4)]

Once you have one or more elements, you verify against them,  such as by  inspecting their properties through the DOM API. For example:

[!code-csharp[](../../../samples/tests/xunit/VerifyMarkupExamples.cs?start=59&end=61)]

#### Auto-refreshing Find() queries

An element found with the [`Find(string cssSelector)`](xref:Bunit.RenderedFragmentExtensions.Find(Bunit.IRenderedFragment,System.String)) method will be updated if the component it came from is re-rendered. 

However, that does not apply to elements that are found by traversing the DOM tree via the <xref:Bunit.IRenderedFragment.Nodes> property on <xref:Bunit.IRenderedFragment>, for example, as those nodes do not know when their root component is re-rendered. Consequently, they don’t know when they should be updated.

As a result of this, it is always recommended to use the [`Find(string cssSelector)`](xref:Bunit.RenderedFragmentExtensions.Find(Bunit.IRenderedFragment,System.String)) method when searching for a single element. Alternatively, always reissue the query whenever you need the element.

#### Auto-refreshable FindAll() queries

The [`FindAll(string cssSelector, bool enableAutoRefresh = false)`](xref:Bunit.RenderedFragmentExtensions.FindAll(Bunit.IRenderedFragment,System.String,System.Boolean)) method has an optional parameter, `enableAutoRefresh`, which when set to `true` will return a collection of `IElement`. This automatically refreshes itself when the component the elements came from is re-rendered.

## Semantic comparison of markup

Working with raw markup only works well with very simple output, but even then you have to sanitize it to get stable tests. A much better approach is to use the semantic HTML comparer that comes with bUnit.

### How does the semantic HTML comparer work?

The comparer takes two HTML fragments (e.g. in the form of a C# string) as input, and returns `true` if both HTML fragments result in the same visual rendered output in a web browser. If not, it returns `false`.

For example, a web browser will render this HTML:

```html
<span>Foo Bar</span>
```

This will be done in exactly the same way as this HTML:

```html
<span>
  Foo       Bar
</span>
```

This is why it makes sense to allow tests to pass, _even_ when the rendered HTML markup is not entirely identical to the expected HTML from a normal string comparer's perspective.

bUnit's semantic HTML comparer safely ignores things like insignificant whitespace and the order of attributes on elements, as well as many more things. **This leads to much more stable tests, as - for example - a reformatted component doesn't break its tests because of insignificant whitespace changes.** More details of the semantic comparer can be found on the <xref:semantic-html-comparison> page.

### The MarkupMatches() method

The HTML comparer can be easily accessed through `MarkupMatches()` extension methods, available in places that represent HTML fragments in bUnit, i.e. on <xref:Bunit.IRenderedFragment> and the `INode` and `INodeList` types.

In the following examples, the `<Heading>` component listed below will be used as the component under test.

[!code-razor[Heading.razor](../../../samples/components/Heading.razor)]

To use the `MarkupMatches()` method to perform a semantic comparison of the output of the `<Heading>` component through its <xref:Bunit.IRenderedFragment>, do the following:

[!code-csharp[](../../../samples/tests/xunit/VerifyMarkupExamples.cs?start=25&end=30&highlight=3-6)]

The highlighted line shows the call to the `MarkupMatches()` method. This test passes even though the insignificant whitespace is not exactly the same between the expected HTML string and the raw markup produced by the `<Heading>` component. It even works when the CSS class list is not in the same order on the `<small>` element.

The `MarkupMatches()` method is also available on `INode` and `INodeList` types, for example:

[!code-csharp[](../../../samples/tests/xunit/VerifyMarkupExamples.cs?start=36&end=39&highlight=3-4)]

Here we use the `Find(string cssSelector)` method to find the `<small>` element, and only verify it and its content and attributes.

> [!TIP]
> Working with `Find()`, `FindAll()`, `INode` and `INodeList` is covered later on this page.

Text content can also be verified with the `MarkupMatches()` method, e.g. the text inside the `<small>` element. It has the advantage over regular string comparison in that it removes insignificant whitespace in the text automatically - even between words - where a normal string `Trim()` method isn't enough. For example:

[!code-csharp[](../../../samples/tests/xunit/VerifyMarkupExamples.cs?start=45&end=48&highlight=3)]

The semantic HTML comparer can be customized to make a test case even more stable and easier to maintain. For example, it is possible to ignore an element or attribute during comparison, or provide a regular expression to the comparer when comparing a specific element or attribute to make the comparer work with generated data.

Learn more about the customization options on the <xref:semantic-html-comparison> page.

## Finding expected differences

It can sometimes be easier to verify that an expected change, and only that change, has occurred in the rendered markup than it can be to specify how all the rendered markup should look after re-rendering.

bUnit comes with a number of ways for finding lists of `IDiff`; the representation of a difference between two HTML fragments. All of these are direct methods or extension methods on the <xref:Bunit.IRenderedFragment> type or on the `INode` or `INodeList` types:

- <xref:Bunit.IRenderedFragment.GetChangesSinceFirstRender> method on <xref:Bunit.IRenderedFragment>. This method returns a list of differences since the initial first render of a component.
- <xref:Bunit.IRenderedFragment.GetChangesSinceSnapshot> and <xref:Bunit.IRenderedFragment.SaveSnapshot> methods on <xref:Bunit.IRenderedFragment>. These two methods combined make it possible to get a list of differences between the last time the <xref:Bunit.IRenderedFragment.SaveSnapshot> method was called and the time a call to the <xref:Bunit.IRenderedFragment.GetChangesSinceSnapshot> method is placed.
- `CompareTo()` methods from <xref:Bunit.CompareToExtensions> for the <xref:Bunit.IRenderedFragment>, `INode`, and `INodeList` types. These methods return a list of differences between the two input HTML fragments.

In addition to this, there are a number of experimental assertion helpers for `IDiff` and `IEnumerable<IDiff>`, making it easier and more concise to declare your assertions.

Let's look at a few examples  of using the assertion helpers. In the first one, we will use the `<Counter>` component listed below:

[!code-razor[Counter.razor](../../../samples/components/Counter.razor)]

Here is an example of using the <xref:Bunit.IRenderedFragment.GetChangesSinceFirstRender> method:

[!code-csharp[](../../../samples/tests/xunit/VerifyMarkupExamples.cs?start=67&end=79&highlight=7,10,13)]

This is what happens in the test:

- On line 8, <xref:Bunit.IRenderedFragment.GetChangesSinceFirstRender> is used to get a list of differences.
- On line 11, the `ShouldHaveSingleChange()` method is used to verify that there is only one change found.
- On line 14, the `ShouldBeTextChange()` method is used to verify that the single `IDiff` is a text change.

Testing a more **complex life cycle of a component** can be done more easily using the <xref:Bunit.IRenderedFragment.GetChangesSinceSnapshot> and <xref:Bunit.IRenderedFragment.SaveSnapshot> methods along with a host of other assert helpers. 

This example tests the `<CheckList>` component listed below. The component allows you to add new items to the checklist by typing into the input field and hitting the `enter` key. Items can be removed from the list again by clicking on them.

[!code-razor[CheckList.razor](../../../samples/components/CheckList.razor)]

To test the end-to-end life cycle of adding and removing items from the `<CheckList>` component, do the following:

[!code-csharp[](../../../samples/tests/xunit/VerifyMarkupExamples.cs?start=85&end=126)]

This is what happens in the test:

1. First the component is rendered and the input field is found.
2. The first item is added through the input field.
3. The <xref:Bunit.IRenderedFragment.GetChangesSinceFirstRender>, `ShouldHaveSingleChange()` and `ShouldBeAddition()` methods are used to verify that the item was correctly added.
4. The <xref:Bunit.IRenderedFragment.SaveSnapshot> is used to save a snapshot of current DOM nodes internally in the `cut`. This reduces the number of diffs found in the following steps, simplifying verification.
5. A second item is added to the check list.
6. Two verifications are performed at this point, one using the <xref:Bunit.IRenderedFragment.GetChangesSinceFirstRender> method which finds two changes, and one using the <xref:Bunit.IRenderedFragment.GetChangesSinceSnapshot> method, which finds a single change. The first is only done for illustrative purposes.
7. A new snapshot is saved, replacing the previous one with another call to the <xref:Bunit.IRenderedFragment.SaveSnapshot> method.
8. Finally the last item in the list is found and clicked, and the <xref:Bunit.IRenderedFragment.GetChangesSinceSnapshot> method is used to find the changes, a single diff, which is verified as a removal of the second item.

As mentioned earlier, the `IDiff` assertion helpers are still experimental. Any feedback and suggestions for improvements should be directed to the [related issue](https://github.com/egil/bUnit/issues/84) on GitHub.

## Verification of raw markup

To access the rendered markup of a component, just use the <xref:Bunit.IRenderedFragment.Markup> property on <xref:Bunit.IRenderedFragment>. This holds the *raw* HTML from the component as a `string`. 

> [!WARNING]
> Be aware that all indentions and whitespace in your components (`.razor` files) are included in the raw rendered markup, so it is often wise to normalize the markup string a little. For example, via the string `Trim()` method to make the tests more stable. Otherwise, a change to the formatting in your components might break the tests unnecessarily when it does not need to.
> 
> To avoid these issues and others related to asserting against raw markup, use the semantic HTML comparer that comes with bUnit, described in the next section.

To get the markup as a string, do the following:

[!code-csharp[](../../../samples/tests/xunit/VerifyMarkupExamples.cs?start=16&end=19&highlight=3)]

Standard string assertions can be performed against the markup string, such as checking whether it contains a value or is empty.