---
uid: semantic-html-comparison
title: Customizing the semantic HTML comparison
---

# Semantic HTML markup comparison

This library includes comparison and assert helpers that uses the [AngleSharp Diffing](https://github.com/AngleSharp/AngleSharp.Diffing/) library to perform semantic HTML comparison.

On this page we will go through how the comparison works, and what options you have to affect the comparison process.

> **NOTE:** The semantic HTML comparison is available in both C# and Razor tests with the <xref:Bunit.Fixture> component, and is always used in Razor tests with the <xref:Bunit.SnapshotTest> component.

## Why semantic comparison is needed for stable tests

Just performing string comparison of two strings containing HTML markup can break quite easily, _even_ if the two markup strings are semantically equivalent. Some changes that can cause a regular string comparison to fail are:

- Formatting of markup, e.g. with extra line breaks or indentions, changes to insignificant whitespace.
- Reordering of attributes. The order of attributes does not matter.
- Reordering of classes defined in the `class="..."` attribute. The order of classes does not matter.
- Change of boolean attribute to be implicit or explicit, e.g. from `required="required"` to `required`.
- Change to insignificant whitespace inside `<style>` tags.
- Changes to HTML comments and comments inside `<style>` tags.

The [AngleSharp Diffing](https://github.com/AngleSharp/AngleSharp.Diffing/) library handles all those cases, so your tests are more stable.

## Customizing the comparison process

The [AngleSharp Diffing](https://github.com/AngleSharp/AngleSharp.Diffing/) library also allows us to customize the comparison process, by added special attributes to the _"control" markup_, i.e. the expected markup we want to use in verification.

There are the customization options you have available to you:

- **Ignore comments (enabled by default):** Comments in markup and inside `<style>` tags are automatically ignored and not part of the comparison process.

- **Ignore element:** Use the `diff:ignore` attribute to ignore an element, all it's attributes and child nodes. For example, to ignore the `h1` element:

  ```html
  <header>
    <h1 class="heading-1" diff:ignore>Hello world</h1>
  </header>
  ```

- **Ignore attribute:** To ignore an attribute during comparison, add the `:ignore` modifier to the attribute (no value is needed). For example, to ignore the `class` attribute:

  ```html
  <header>
    <h1 class:ignore="heading-1">Hello world</h1>
  </header>
  ```

- **Configure whitespace handling:** By default all nodes and elements are compared using the `Normalize` whitespace handling option. The `Normalize` option will trim all text nodes and replace two or more whitespace characters with a single space character. The other options are `Preserve`, which will leave all whitespace unchanged, and `RemoveWhitespaceNodes`, which will only remove empty text nodes.

  To override the default option, use the `diff:whitespace` attribute, and pass one of the three options to it, for example:

  ```html
  <header>
    <h1 diff:whitespace="preserve">Hello <em> woooorld</em></h1>
  </header>
  ```

  **NOTE:** The default for `<pre>` and `<script>` elements is the `Preserve` option. To change that, use the `diff:whitespace` attribute, for example:

  ```html
  <pre diff:whitespace="RemoveWhitespaceNodes">...</pre>
  ```

- **Perform case insensitive comparison:** By default, all text comparison is case sensitive, but if you want to perform a case insensitive comparison of text inside elements or attributes, use the `diff:ignoreCase` attributes on elements and `:ignoreCase` modifier on attributes. For example, to do case insensitive comparison of the text in the `h1` element:

  ```html
  <h1 diff:ignoreCase>HellO WoRlD</h1>
  ```

  To do case insensitive comparison of the text inside the `title` attribute:

  ```html
  <h1 title:ignoreCase="HeaDinG">...</h1>
  ```

- **Use RegEx during comparison:** To use a regular expression when comparing the text inside an element or inside an attribute, use the `diff:regex` on elements and `:regex` modifier on attributes.

  For example, to use a regular expression during comparison of the text in the `h1` element, add the `diff:regex` attribute to the element and place the regular expression in the body of the element:

  ```html
  <h1 diff:regex diff:ignoreCase>Hello World \d{4}</h1>
  ```

  To use a regular expression during comparison of the text inside the `title` attribute, add the `:regex` modifier to attribute and add the regular expression in the attributes value:

  ```html
  <h1 title:regex="Heading-\d{4}">...</h1>
  ```

  **NOTE:** The attribute modifiers `:ignoreCase` and `:regex` can be combined, for example as: `attr:ignoreCase:regex="FOO-\d{4}"`

## Verifying output from components

To verify the rendered output of a component (i.e. in the from of a `IRenderedFragment`), we have the various `MarkupMatches()` methods we can use.

If for example we have a component, `<Heading>`, that renders the following markup:

```html
<h3 id="heading-1337" required>
  Heading text
  <small class="text-muted mark">Secondary text</small>
</h3>
```

If we want to verify the markup is rendered correctly, and for example use RegEx to verify the `id` attribute (it might be generated) and ignore the `<small>` element, we can do it like this in C# based tests:

```csharp
[Fact]
public void InitialHtmlIsCorrect()
{
    // Arrange - renders the Heading component
    var cut = RenderComponent<Heading>();

    // Assert
    // Here we specify expected HTML from CUT.
    var expectedHtml = @"<h3 id:regex=""heading-\d{4}"" required>
                            Heading text
                            <small diff:ignore></small>
                         </h3>";

    // Here we use the HTML diffing library to assert that the rendered HTML
    // from CUT is semantically the same as the expected HTML string above.
    cut.MarkupMatches(expectedHtml);
}
```

In a Razor based test, the example looks like this:

```cshtml
<Fixture Test="Test1">
  <ComponentUnderTest>
    <Heading />
  </ComponentUnderTest>
</Fixture>
@code {
  void Test1(IRazorTestContext context)
  {
    // Arrange - Gets the Heading component
    var cut = context.GetComponentUnderTest<Heading>();

    // Assert
    // Here we specify expected HTML from CUT.
    var expectedHtml = @"<h3 id:regex=""heading-\d{4}"" required>
                            Heading text
                            <small diff:ignore></small>
                         </h3>";

    // Here we use the HTML diffing library to assert that the rendered HTML
    // from CUT is semantically the same as the expected HTML string above.
    cut.MarkupMatches(expectedHtml);
  }
}
```

In a Snapshot test, the example looks like this:

```html
<SnapshotTest Description="Helpful description of the test case">
  <TestInput>
    <Heading />
  </TestInput>
  <ExpectedOutput>
    <h3 id:regex="heading-\d{4}" required>
      Heading text
      <small diff:ignore></small>
    </h3>
  </ExpectedOutput>
</SnapshotTest>
```

## Different ways of getting the differences

This section is coming soon. For now, see examples on the [C# test examples](/docs/CSharp-test-examples.html) page where the methods are demonstrated. Look for examples using these methods:

- `CompareTo`
- `MarkupMatches`
- `GetChangesSinceFirstRender`
- `SaveSnapshot` and `GetChangesSinceSnapshot`
- `ShouldHaveSingleTextChange`
- `ShouldHaveSingleChange`
- `ShouldBeAddition`
- `ShouldBeRemoval`
