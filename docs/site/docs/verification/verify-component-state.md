---
uid: verify-component-state
title: Verifying the state of a component under test
---

# Verifying the state of a component

The instance of a component under test is available through the <xref:Bunit.IRenderedComponentBase`1.Instance> property on the <xref:Bunit.IRenderedComponent`1> type. When using the <xref:Bunit.TestContext>'s `RenderComponent<TComponent>()` method, this is the type returned.

In `.razor` based tests, using the <xref:Bunit.TestContext>'s <xref:Bunit.TestContext.Render``1(RenderFragment)> method also returns an <xref:Bunit.IRenderedComponent`1> (as opposed to the <xref:Bunit.TestContext.Render(RenderFragment)> method which returns the more simple <xref:Bunit.IRenderedFragment>).

> [!NOTE]
> Since <xref:Bunit.IRenderedComponent`1> inherits from <xref:Bunit.IRenderedFragment>, all the markup verification techniques covered on the <xref:verify-markup> page also apply to the <xref:Bunit.IRenderedComponent`1> type.

## Inspecting the Component Under Test

The <xref:Bunit.IRenderedComponentBase`1.Instance> property on the <xref:Bunit.IRenderedComponent`1> type provides access to the component under test. For example:

```csharp
using var ctx = new TestContext();

IRenderedComponent<Alert> cut = ctx.RenderComponent<Alert>();

Alert alert = cut.Instance;

// Assert against <Alert /> instance
```

> [!WARNING]
> While it is possible to set `[Parameter]` and `[CascadingParameter]` properties directly through the <xref:Bunit.IRenderedComponentBase`1.Instance> property on the <xref:Bunit.IRenderedComponent`1> type, doing so does not implicitly trigger a render and the component life-cycle methods are not called. 
> 
> The correct approach is to set parameters through the [`SetParametersAndRender()`](xref:Bunit.RenderedComponentRenderExtensions.SetParametersAndRender``1(Bunit.IRenderedComponentBase{``0},System.Action{Bunit.ComponentParameterCollectionBuilder{``0}})) methods. See the <xref:trigger-renders> page for more on this.

## Finding Components in the Render Tree

To get the instances of components nested inside the component under test, use the 
[`FindComponent<TComponent>()`](xref:Bunit.RenderedFragmentExtensions.FindComponent``1(Bunit.IRenderedFragment)) and [`FindComponents<TComponent>()`](xref:Bunit.RenderedFragmentExtensions.FindComponents``1(Bunit.IRenderedFragment)) methods on the <xref:Bunit.IRenderedComponent`1> type. Suppose, for each task in the todo list, we have a `<TodoList>` component with `<Task>` components nested inside. In this case, the `<Task>` components can be found like this:

```csharp
using var ctx = new TestContext();
var cut = ctx.RenderComponent<TodoList>(parameter => parameter
  .Add(p => p.Tasks, new [] { "Task 1", "Task 2" })
);

var tasks = cut.FindComponents<Task>();

Assert.Equal(2, tasks.Count);
```

Both the [`FindComponent<TComponent>()`](xref:Bunit.RenderedFragmentExtensions.FindComponent``1(Bunit.IRenderedFragment)) and [`FindComponents<TComponent>()`](xref:Bunit.RenderedFragmentExtensions.FindComponents``1(Bunit.IRenderedFragment)) methods perform a **depth-first search** of the render tree, with the first method returning only the first matching component found, and the latter returning all matching components in the render tree.
