---
uid: root-render-tree
title: Controlling the Root Render Tree
---

# Controlling the Root Render Tree

The root render tree, the render tree in which components under test are rendered, can be added to before components are rendered with bUnit's test context. This is mostly useful when a component under test, or a component it depends on, must be rendered inside another component, that provides a cascading value to it. 

For example, when using Blazor’s authentication, it is common to add the `CascadingAuthenticationState` component higher up the render tree, such that it can provide authentication state to those components below it that need it. Adding this through the <xref:Bunit.TestContext.RenderTree> property on the <xref:Bunit.TestContext> type makes it possible to add it once in a shared setup method, and not have to do so in every test method during the call to [`RenderComponent()`](xref:Bunit.TestContext.RenderComponent``1(System.Action{Bunit.ComponentParameterCollectionBuilder{``0}})).

This can also be useful when writing tests that use a third-party component library, that require a special root component to be added to the render tree, but which otherwise doesn’t change between tests.

## Adding a Component to the Root Render Tree

The following example verifies that the `PrintCascadingValue` component correctly prints out the cascading value passed to it. This value is passed to it by adding the `CascadingValue<string>` component to the render tree and then rendering the `PrintCascadingValue` component. The `PrintCascadingValue` component looks like this:

[!code-cshtml[PrintCascadingValue.razor](../../../samples/components/PrintCascadingValue.razor)]

Here is the test that adds the `CascadingValue<string>` component to the render tree and then renders the `PrintCascadingValue` component.

[!code-csharp[PrintCascadingValueTest.cs](../../../samples/tests/xunit/RenderTreeTest.cs#L15-L27)]

> [!NOTE]
> The call to [`Add`](xref:Bunit.Rendering.RootRenderTree.Add``1(System.Action{Bunit.ComponentParameterCollectionBuilder{``0}})) can be done in a common setup method, outside the context of the test method listed here, for easy reuse between tests.

## Add Only if Not Already in Root Render Tree

Sometimes common test setup logic exists outside the test class, perhaps abstracted away in other libraries. In those cases, the [`TryAdd`](xref:Bunit.Rendering.RootRenderTree.TryAdd``1(System.Action{Bunit.ComponentParameterCollectionBuilder{``0}})) can be used add the component to the render tree, _only if_ it has not already been added. [`TryAdd`](xref:Bunit.Rendering.RootRenderTree.TryAdd``1(System.Action{Bunit.ComponentParameterCollectionBuilder{``0}})) returns true if the component was added, false otherwise.

[!code-csharp[](../../../samples/tests/xunit/RenderTreeTest.cs#L36-L38)]

In the listing above, the cascading value `BAR?` is only added if there is not another `CascadingValue<string>` component added to the render tree already.
<!--stackedit_data:
eyJoaXN0b3J5IjpbLTk1ODk0NTIwMywxNzUyMTU0MzkyXX0=
-->