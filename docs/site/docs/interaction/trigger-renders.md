---
uid: trigger-renders
title: Triggering a render life cycle on a component
---

# Triggering a render life cycle on a component

To trigger a re-render of a component under test, a reference to it through a <xref:Bunit.IRenderedComponent`1> type is needed. When using the <xref:Bunit.TestContext>'s `RenderComponent<TComponent>()` method, this is the type returned.

In `.razor` based tests, using the <xref:Bunit.TestContext>'s <xref:Bunit.TestContext.Render(Microsoft.AspNetCore.Components.RenderFragment)> method also returns an <xref:Bunit.IRenderedComponent`1> (as opposed to the <xref:Bunit.TestContext.Render(Microsoft.AspNetCore.Components.RenderFragment)> method which returns the more simple <xref:Bunit.IRenderedFragment>).

If you have a <xref:Bunit.IRenderedFragment> or a <xref:Bunit.IRenderedComponent`1> in a test, but need a child component's <xref:Bunit.IRenderedComponent`1>, then use the `FindComponent<TComponent>()` or the `FindComponents<TComponent>()` methods, which traverse down the render tree and finds rendered components.

With a <xref:Bunit.IRenderedComponent`1>, it is possible to cause the component to render again directly through the [`Render()`](xref:Bunit.TestContext.Render``1(Microsoft.AspNetCore.Components.RenderFragment)) method or one of the [`SetParametersAndRender()`](xref:Bunit.RenderedComponentRenderExtensions.SetParametersAndRender``1(Bunit.IRenderedComponentBase{``0},System.Action{Bunit.ComponentParameterCollectionBuilder{``0}})) methods, or indirectly through the [`InvokeAsync()`](xref:Bunit.IRenderedFragmentBase.Bunit.RenderedFragmentInvokeAsyncExtensions.InvokeAsync(System.Action)) method.

Let's look at how to use each of these methods to cause a re-render.

## Render

The [`Render()`](xref:Bunit.RenderedComponentRenderExtensions.Render``1(Bunit.IRenderedComponentBase{``0})) method tells the renderer to re-render the component, i.e. go through its life-cycle methods (except for `OnInitialized()` and `OnInitializedAsync()` methods). To use it, do the following:

[!code-csharp[](../../../samples/tests/xunit/ReRenderTest.cs?start=17&end=23&highlight=5)]

The highlighted line shows the call to [`Render()`](xref:Bunit.RenderedComponentRenderExtensions.Render``1(Bunit.IRenderedComponentBase{``0})).

> [!TIP]
> The number of renders a component has been through can be inspected and verified using the <xref:Bunit.IRenderedFragmentBase.RenderCount> property.

## SetParametersAndRender

The [`SetParametersAndRender(...)`](xref:Bunit.RenderedComponentRenderExtensions.SetParametersAndRender``1(Bunit.IRenderedComponentBase{``0},System.Action{Bunit.ComponentParameterCollectionBuilder{``0}})) methods tells the renderer to re-render the component with new parameters, i.e. go through its life-cycle methods (except for `OnInitialized()` and `OnInitializedAsync()` methods), passing the new parameters &mdash; _but only the new parameters_ &mdash; to the `SetParametersAsync()` method. To use it, do the following:

[!code-csharp[](../../../samples/tests/xunit/ReRenderTest.cs?start=30&end=40&highlight=7-9)]

The highlighted line shows the call to [`SetParametersAndRender()`](xref:Bunit.RenderedComponentRenderExtensions.SetParametersAndRender``1(Bunit.IRenderedComponentBase{``0},System.Action{Bunit.ComponentParameterCollectionBuilder{``0}})), which is also available as a version that takes the zero or more component parameters, e.g. created through the component parameter factory helper methods, if you prefer that method of passing parameters.

> [!NOTE]
> Passing parameters to components through the [`SetParametersAndRender(...)`](xref:Bunit.RenderedComponentRenderExtensions.SetParametersAndRender``1(Bunit.IRenderedComponentBase{``0},System.Action{Bunit.ComponentParameterCollectionBuilder{``0}})) methods is identical to doing it with the `RenderComponent<TComponent>(...)` methods, described in detail on the <xref:passing-parameters-to-components> page.

## InvokeAsync

Invoking methods on a component under test, which causes a render, e.g. by calling `StateHasChanged`, can result in the following error, if the caller is running on another thread than the renderer's thread:

> The current thread is not associated with the Dispatcher. Use `InvokeAsync()` to switch execution to the Dispatcher when triggering rendering or component state.

If you receive this error, you need to invoke your method inside an `Action` delegate passed to the `InvokeAsync()` method.

Let’s look at an example of this, using the `<Calc>` component listed below:

[!code-cshtml[Calc.razor](../../../samples/components/Calc.razor)]

To invoke the `Calculate()` method on the component instance, do the following:

[!code-csharp[](../../../samples/tests/xunit/ReRenderTest.cs?start=47&end=53&highlight=5)]

The highlighted line shows the call to `InvokeAsync()`, which is passed an `Action` delegate that calls the `Calculate` method.

> [!TIP]
> The instance of a component under test is available through the <xref:Bunit.IRenderedComponentBase`1.Instance> property.
