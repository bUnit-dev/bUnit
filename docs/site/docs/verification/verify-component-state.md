---
uid: verify-component-state
title: Verifying the State of a Component Under Test
---

# Verifying the State of a Component

Calling [`RenderComponent<TComponent>()`](xref:Bunit.TestContext.RenderComponent``1(System.Action{Bunit.ComponentParameterBuilder{``0}})) on a <xref:Bunit.TestContext> or calling <xref:Bunit.Fixture.GetComponentUnderTest``1> on a <xref:Bunit.Fixture> returns an instance of the <xref:Bunit.IRenderedComponent`1> type. 

The <xref:Bunit.IRenderedComponent`1> type makes it possible to inspect the instance of the component under test (`TComponent`), and trigger re-renders explicitly.

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
> The correct approach is to set parameters through the [`SetParametersAndRender()`](xref:Bunit.RenderedComponentRenderExtensions.SetParametersAndRender``1(Bunit.IRenderedComponentBase{``0},Bunit.Rendering.ComponentParameter[])) methods. See the <xref:trigger-renders> page for more on this.

## Finding Components in the Render Tree

To get the instances of components nested inside the component under test, use the 
[`FindComponent<TComponent>()`](xref:Bunit.RenderedFragmentExtensions.FindComponent``1(Bunit.IRenderedFragment)) and [`FindComponents<TComponent>()`](xref:Bunit.RenderedFragmentExtensions.FindComponents``1(Bunit.IRenderedFragment)) methods on the <xref:Bunit.IRenderedComponent`1> type. Suppose we have a `<TodoList>` component with `<Task>` components nested inside for each task in the todo list. In this case, the `<Task>` components can be found like this:

```csharp
using var ctx = new TestContext();
var cut = ctx.RenderComponent<TodoList>(parameter => parameter
  .Add(p => p.Tasks, new [] { "Task 1", "Task 2" })
);

var tasks = cut.FindComponents<Task>();

Assert.Equal(2, tasks.Count);
```

Both the [`FindComponent<TComponent>()`](xref:Bunit.RenderedFragmentExtensions.FindComponent``1(Bunit.IRenderedFragment)) and [`FindComponents<TComponent>()`](xref:Bunit.RenderedFragmentExtensions.FindComponents``1(Bunit.IRenderedFragment)) methods perform a **depth-first search** of the render tree, with the first method returning only the first found matching component, and the latter returning all matching components in the render tree.
Both the [`FindComponent<TComponent>()`](xref:Bunit.RenderedFragmentExtensions.FindComponent``1(Bunit.IRenderedFragment)) and [`FindComponents<TComponent>()`](xref:Bunit.RenderedFragmentExtensions.FindComponents``1(Bunit.IRenderedFragment)) methods performs a **depth-first search** of the render tree, with the first method returning only the first found matching component, and the latter returning all matching components in the render tree.
<!--stackedit_data:
eyJoaXN0b3J5IjpbLTg4NjUyMTk0NV19
-->