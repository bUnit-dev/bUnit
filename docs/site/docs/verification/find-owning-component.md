---
uid: find-owning-component
title: Finding the component that rendered an element
---

# Finding the component that rendered an element

The DOM query API is often the quickest way to reach an interesting part of the rendered
output. <xref:Bunit.ElementOwningComponentExtensions.GetOwningComponent(AngleSharp.Dom.INode)>
takes you from a node found that way back to the component that rendered it, so you can
assert on that component's state:

```csharp
var cut = Render<TodoList>();

IRenderedComponent<IComponent> owner = cut.Find("li.done").GetOwningComponent();

owner.Instance.ShouldBeOfType<TodoItem>();
```

Use the generic overload,
<xref:Bunit.ElementOwningComponentExtensions.GetOwningComponent``1(AngleSharp.Dom.INode)>,
when you know which component type you are after. It returns a strongly typed
<xref:Bunit.IRenderedComponent`1>, so the component instance and all the usual verification
methods are available:

```csharp
var cut = Render<TodoList>();

IRenderedComponent<TodoItem> item = cut.Find("li.done").GetOwningComponent<TodoItem>();

item.Instance.IsDone.ShouldBeTrue();
item.MarkupMatches("<li class=\"done\">Buy milk</li>");
```

## Which component is returned

The **innermost** component that rendered the node. Given a `<TodoList>` that renders a
`<ul>` and a `<TodoItem>` per entry, `cut.Find("ul").GetOwningComponent()` returns the
`<TodoList>` and `cut.Find("li").GetOwningComponent()` returns the `<TodoItem>`.

A few details worth knowing:

- **Nodes that are not elements resolve through their closest element.** Calling it on a text
  node returns the component that rendered the element containing that text.
- **Content passed as a `RenderFragment` belongs to the receiving component.** A
  `ChildContent` written in the parent is rendered into the child's render tree, so
  `GetOwningComponent()` names the child. This matches what the child's
  <xref:Bunit.IRenderedComponent`1.Markup> already showed.
- **bUnit's own root component is never returned.** Wrapper components with no markup of their
  own, such as `CascadingValue<T>`, lose to the component inside them.

The generic overload widens the search outwards: if the innermost component is not a
`TComponent`, it walks up the component tree until it finds one, and throws
<xref:Bunit.Rendering.ComponentNotFoundException> if there is none. That makes it a concise
way to reach a specific ancestor:

```csharp
// The <button> is rendered by <TodoItem>, but the assertion is about the list around it.
IRenderedComponent<TodoList> list = cut.Find("li button").GetOwningComponent<TodoList>();
```

## Nodes that cannot be traced

Both methods throw an `InvalidOperationException` for a node that bUnit did not render, for
example one parsed from a markup string in a test. Only nodes from a rendered component tree
carry the information needed to name their component.

## How it works

All components in a render tree share one markup string and one parsed document. While
generating that markup, bUnit records the character range each component occupies in it, and
AngleSharp records the position each element was parsed from. Looking up an element's position
in those ranges identifies the innermost component that produced it - no markers are added to
the markup, so nothing about the rendered output changes.

The same shared document is what lets events bubble across component boundaries; see
<xref:trigger-event-handlers>.
