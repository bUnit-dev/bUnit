---
uid: semantic-html-comparison
title: Customizing the semantic HTML comparison
---

# Customizing the semantic HTML comparison

This library includes comparison and assert helpers that use the [AngleSharp Diffing](https://github.com/AngleSharp/AngleSharp.Diffing/) library to perform semantic HTML comparison.

On this page we will go through how the comparison works, and what options you have to affect the comparison process.

## Why semantic comparison is needed for stable tests

Just performing string comparison of two strings containing HTML markup can break quite easily, _even_ if the two markup strings are semantically equivalent. Some changes that can cause a regular string comparison to fail are as follows:

- Formatting of markup, e.g. with extra line breaks or indentions and changes to insignificant whitespace.
- Reordering of attributes. The order of attributes does not matter.
- Reordering of classes defined in the `class="..."` attribute. The order of classes does not matter.
- Change of boolean attributes  to be implicit or explicit, e.g. from `required="required"` to `required`.
- Changes to insignificant whitespace inside `<style>` tags.
- Changes to HTML comments and comments inside `<style>` tags.

The [AngleSharp Diffing](https://github.com/AngleSharp/AngleSharp.Diffing/) library handles all those cases, so your tests are more stable.

## Customizing Options

The [AngleSharp Diffing](https://github.com/AngleSharp/AngleSharp.Diffing/) library also allows us to customize the comparison process by adding special attributes to the _"control" markup_, i.e. the expected markup we want to use in verification.

All the customization options below will match with the following markup:

```html
<header>
  <h1 id="head-1">
    Hello    <em>world</em>
  </h1>
</header>
```

Here are the customization options you have available to you:

- **Ignore comments (enabled by default):** Comments in markup and inside `<style>` tags are automatically ignored and not part of the comparison process.

- **Ignore element:** Use the `diff:ignore` attribute to ignore an element, all its attributes and its child nodes. For example, to ignore the `h1` element, do the following:

  ```html
  <header>
    <h1 diff:ignore></h1>
  </header>
  ```

- **Ignore attribute:** To ignore an attribute during comparison, add the `:ignore` modifier to the attribute (no value is needed). For example, to ignore the `id` attribute:

  ```html
  <header>
    <h1 id:ignore>Hello <em>world</em></h1>
  </header>
  ```

- **Ignore children:** Use the `diff:ignoreChildren` attribute (no value is needed) to ignore all child nodes/elements of an element. This does not include attributes. For example, to ignore all child nodes of the `h1` element, do the following:

  ```html
  <header>
    <h1 id="head-1" diff:ignoreChildren></h1>
  </header>
  ```

- **Ignore all attributes:** Use the `diff:ignoreAttributes` attribute (no value is needed) to ignore all attributes of an element. For example:

  ```html
  <header>
    <h1 diff:ignoreAttributes>Hello <em>world</em></h1>
  </header>
  ```

  > [!NOTE]
  > The `diff:ignoreChildren` and `diff:ignoreAttributes` attributes can be combined to ignore all child nodes/element *and* attributes of an element, but still verify that the element itself exists. For example:
  ```html
  <header>
    <h1 diff:ignoreChildren diff:ignoreAttributes></h1>
  </header>

- **Configure whitespace handling:** By default, all nodes and elements are compared using the `Normalize` whitespace handling option. The `Normalize` option will trim all text nodes and replace two or more whitespace characters with a single space character. The other options are `Preserve`, which will leave all whitespace unchanged, and `RemoveWhitespaceNodes`, which will only remove empty text nodes.

  To override the default option, use the `diff:whitespace` attribute, and pass one of the three options to it. For example:

  ```html
  <header>
    <h1 id="head-1" diff:whitespace="preserve">
      Hello    <em>world</em>
    </h1>
  </header>
  ```

  > [!NOTE]
  > The default for `<pre>` and `<script>` elements is the `Preserve` option. To change that, use the `diff:whitespace` attribute. For example:

  ```html
  <header>
    <h1 id="head-1" diff:whitespace="RemoveWhitespaceNodes">Hello<em>world</em></pre>
  </header>
  ```

- **Perform case-insensitive comparison:** By default, all text comparison is case sensitive, but if you want to perform a case-insensitive comparison of text inside elements or attributes, use the `diff:ignoreCase` attribute on elements and `:ignoreCase` modifier on attributes. For example, to perform a case insensitive comparison of the text in the following `h1` element , do the following:

  ```html
  <header>
    <h1 id="head-1" diff:ignoreCase>HeLLo <em>world</em></h1>
  </header
  ```

  To perform case insensitive comparison of the text inside the `id` attribute, do the following:

  ```html
  <header>
    <h1 id:ignoreCase="HeAD-1">Hello <em>world</em></h1>
  </header>
  ```

- **Use RegEx during comparison:** To use a regular expression when comparing the text inside an element or inside an attribute, use the `diff:regex` attribute on elements and the `:regex` modifier on attributes.

  For example, to use a regular expression during comparison of the text in the `em` element, add the `diff:regex` attribute to the element and place the regular expression in the body of the element:

  ```html
  <header>
    <h1 id="head-1">Hello <em diff:regex>\w</em></h1>
  </header>  
  ```

  To use a regular expression during comparison of the text inside the `id` attribute, add the `:regex` modifier to the attribute and add the regular expression in the attribute's value:

  ```html
  <h1 id:regex="head-\d{1}">...</h1>
  ```

  > [!NOTE] 
  > The attribute modifiers `:ignoreCase` and `:regex` can be combined, for example, as: `attr:ignoreCase:regex="FOO-\d{4}"`

## Examples

Let’s look at a few examples where we use the semantic comparison options listed above to modify the comparison. In tests, we have the `MarkupMatches()` methods we can use to perform semantic comparison of the output from a rendered component. For example, we may have a component, `<Heading>`, that renders the following markup:

[!code-razor[Heading.razor](../../../samples/components/Heading.razor)]   

In this case, we want to verify that the markup is rendered correctly, using something such as RegEx to verify the `id` attribute (it might be generated) and ignoring the `<small>` element.  In tests we can do this like so with the `MarkupMatches()` method:

[!code-csharp[SemanticHtmlTest.cs](../../../samples/tests/xunit/SemanticHtmlTest.cs#L15-L27)]
