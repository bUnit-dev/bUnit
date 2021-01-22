---
uid: trigger-renders
title: Triggering a Render Life Cycle on a Component
---

# Triggering a Render Life Cycle on a Component

When a component under test is rendered, an instance of the <xref:Bunit.IRenderedComponent`1> type is returned. Through that, it is possible to cause the component under test to render again directly through the [`Render()`](xref:Bunit.RenderedComponentRenderExtensions.Render``1(Bunit.IRenderedComponentBase{``0})) method or one of the [`SetParametersAndRender()`](xref:Bunit.RenderedComponentRenderExtensions.SetParametersAndRender``1(Bunit.IRenderedComponentBase{``0},System.Action{Bunit.ComponentParameterCollectionBuilder{``0}})) methods, or indirectly through the [`InvokeAsync()`](xref:Bunit.RenderedComponentInvokeAsyncExtensions.InvokeAsync``1(Bunit.IRenderedComponentBase{``0},System.Action)) method.

> [!WARNING]
> The `Render()` and `SetParametersAndRender()` methods are not available in the <xref:Bunit.IRenderedFragment> type that is returned when calling the _non-generic_ version of `GetComponentUnderTest()` in `<Fixture>`-based Razor tests. Call the generic version of `GetComponentUnderTest<TComponent>()` to get a <xref:Bunit.IRenderedComponent`1>.

> [!NOTE]
> These methods are available and work the same in both C#- and Razor-based tests. The examples below are from C#-based tests only.

Let's look at how to use each of these methods to cause a re-render.

## Render

The [`Render()`](xref:Bunit.RenderedComponentRenderExtensions.Render``1(Bunit.IRenderedComponentBase{``0})) method tells the renderer to re-render the component, i.e. go through its life-cycle methods (except for `OnInitialized()` and `OnInitializedAsync()` methods). To use it, do the following:

[!code-csharp[](../../../samples/tests/xunit/ReRenderTest.cs?start=17&end=24&highlight=6)]

The highlighted line shows the call to [`Render()`](xref:Bunit.RenderedComponentRenderExtensions.Render``1(Bunit.IRenderedComponentBase{``0})). 

> [!TIP]
> The number of renders a component has been through can be inspected and verified using the <xref:Bunit.IRenderedFragmentBase.RenderCount> property.

## SetParametersAndRender

The [`SetParametersAndRender(...)`](xref:Bunit.RenderedComponentRenderExtensions.SetParametersAndRender``1(Bunit.IRenderedComponentBase{``0},System.Action{Bunit.ComponentParameterCollectionBuilder{``0}})) methods tells the renderer to re-render the component with new parameters, i.e. go through its life-cycle methods (except for `OnInitialized()` and `OnInitializedAsync()` methods), passing the new parameters &mdash; _but only the new parameters_ &mdash; to the `SetParametersAsync()` method. To use it, do the following:

[!code-csharp[](../../../samples/tests/xunit/ReRenderTest.cs?start=31&end=42&highlight=8-10)]

The highlighted line shows the call to [`SetParametersAndRender()`](xref:Bunit.RenderedComponentRenderExtensions.SetParametersAndRender``1(Bunit.IRenderedComponentBase{``0},System.Action{Bunit.ComponentParameterCollectionBuilder{``0}})), which is also available as a version that takes the zero or more component parameters, e.g. created through the component parameter factory helper methods, if you prefer that method of passing parameters.

> [!NOTE]
> Passing parameters to components through the [`SetParametersAndRender(...)`](xref:Bunit.RenderedComponentRenderExtensions.SetParametersAndRender``1(Bunit.IRenderedComponentBase{``0},System.Action{Bunit.ComponentParameterCollectionBuilder{``0}})) methods is identical to doing it with the `RenderComponent<TComponent>(...)` methods, described in detail on the <xref:passing-parameters-to-components> page.

## InvokeAsync

Invoking methods on a component under test which cause [__AP: I'm assuming it's the methods that cause the rend] a render - e.g. by calling `StateHasChanged` - can result in the following error:

> The current thread is not associated with the Dispatcher. Use InvokeAsync() to switch execution to the Dispatcher when triggering rendering or component state.

If you receive this error, you need to invoke your method inside an `Action` delegate passed to the [`InvokeAsync(...)`](xref:Bunit.RenderedComponentInvokeAsyncExtensions.InvokeAsync``1(Bunit.IRenderedComponentBase{``0},System.Action)) method.

Let’s look at an example of this, using the `<ImparativeCalc>` component listed below:

[!code-cshtml[ImparativeCalc.razor](../../../samples/components/ImparativeCalc.razor)]

To invoke the `Calculate()` method on the component instance, do the following:

[!code-csharp[](../../../samples/tests/xunit/ReRenderTest.cs?start=49&end=56&highlight=6)]

The highlighted line shows the call to [`InvokeAsync(...)`](xref:Bunit.RenderedComponentInvokeAsyncExtensions.InvokeAsync``1(Bunit.IRenderedComponentBase{``0},System.Action)), which is passed an `Action` delegate that calls the `Calculate` method.

> [!TIP]
> The instance of a component under test is available through the <xref:Bunit.IRenderedComponentBase`1.Instance> property.
<!--stackedit_data:
eyJoaXN0b3J5IjpbMTIwMDAxOTcwMl19
-->