---
uid: trigger-renders
title: Triggering a render life cycle on a component
---

# Triggering a render life cycle on a component

To trigger a re-render of a component under test, a reference to it through a <xref:Bunit.IRenderedComponent`1> type is needed. When using the <xref:Bunit.TestContext>'s `Render<TComponent>()` method, this is the type returned.

In `.razor` based tests, using the <xref:Bunit.TestContext>'s <xref:Bunit.TestContext.Render``1(RenderFragment)> method also returns an <xref:Bunit.IRenderedComponent`1> (as opposed to the <xref:Bunit.TestContext.Render(RenderFragment)> method which returns the more simple <xref:Bunit.IRenderedFragment>).

If you have a <xref:Bunit.IRenderedFragment> or a <xref:Bunit.IRenderedComponent`1> in a test, but need a child component's <xref:Bunit.IRenderedComponent`1>, then use the `FindComponent<TComponent>()` or the `FindComponents<TComponent>()` methods, which traverse down the render tree and finds rendered components.

With a <xref:Bunit.IRenderedComponent`1>, it is possible to cause the component to render again directly through the <xref:Bunit.RenderedComponentRenderExtensions.Render``1(Bunit.IRenderedComponent{``0},System.Nullable{Action{Bunit.ComponentParameterCollectionBuilder{``0}}})> method.

Let's look at how to use each of these methods to cause a re-render.

## Render

The [`Render()`](xref:Bunit.RenderedComponentRenderExtensions.Render``1(Bunit.IRenderedComponent{``0},System.Nullable{Action{Bunit.ComponentParameterCollectionBuilder{``0}}})) method tells the renderer to re-render the component, i.e. go through its life-cycle methods (except for `OnInitialized()` and `OnInitializedAsync()` methods). To use it, do the following:

[!code-csharp[](../../../samples/tests/xunit/ReRenderTest.cs?start=17&end=23&highlight=5)]

The highlighted line shows the call to [`Render()`](xref:Bunit.RenderedComponentRenderExtensions.Render``1(Bunit.IRenderedComponent{``0},System.Nullable{Action{Bunit.ComponentParameterCollectionBuilder{``0}}})).

> [!TIP]
> The number of renders a component has been through can be inspected and verified using the <xref:Bunit.IRenderedFragment.RenderCount> property.

## Passing new or updated parameters

The <xref:Bunit.RenderedComponentRenderExtensions.Render``1(Bunit.IRenderedComponent{``0},System.Nullable{Action{Bunit.ComponentParameterCollectionBuilder{``0}}})> method tells the renderer to re-render the component with new parameters, i.e. go through its life-cycle methods (except for `OnInitialized()` and `OnInitializedAsync()` methods), passing the new parameters &mdash; _but only the new parameters_ &mdash; to the `SetParametersAsync()` method. To use it, do the following:

[!code-csharp[](../../../samples/tests/xunit/ReRenderTest.cs?start=30&end=40&highlight=7-9)]

The highlighted line shows the call to [`Render()`](xref:Bunit.RenderedComponentRenderExtensions.Render``1(Bunit.IRenderedComponent{``0},System.Nullable{Action{Bunit.ComponentParameterCollectionBuilder{``0}}})), which is also available as a version that takes the zero or more component parameters, e.g. created through the component parameter factory helper methods, if you prefer that method of passing parameters.

> [!NOTE]
> Passing parameters to components through the [`Render(...)`](xref:Bunit.RenderedComponentRenderExtensions.Render``1(Bunit.IRenderedComponent{``0},System.Nullable{Action{Bunit.ComponentParameterCollectionBuilder{``0}}})) methods is identical to doing it with the `Render<TComponent>(...)` methods, described in detail on the <xref:passing-parameters-to-components> page.