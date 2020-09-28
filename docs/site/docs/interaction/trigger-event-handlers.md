---
uid: trigger-event-handlers
title: Triggering Event Handlers in Components
---

# Triggering Event Handlers in Components

Blazor makes it possible to bind many event handlers to elements in a Blazor component, using the `@onXXXX` syntax, e.g. `@onclick="MyClickHandler"`. 

bUnit comes with event dispatch helper methods that makes it possible to invoke event handlers for all event types supported by Blazor.

**The built-in dispatch event helpers are:**

- [Clipboard events](xref:Bunit.ClipboardEventDispatchExtensions)
- [Drag events](xref:Bunit.DragEventDispatchExtensions)
- [Focus events](xref:Bunit.FocusEventDispatchExtensions)
- [General events](xref:Bunit.GeneralEventDispatchExtensions)
- [Input events](xref:Bunit.InputEventDispatchExtensions)
- [Keyboard events](xref:Bunit.KeyboardEventDispatchExtensions)
- [Media events](xref:Bunit.MediaEventDispatchExtensions)
- [Mouse events](xref:Bunit.MouseEventDispatchExtensions)
- [Pointer events](xref:Bunit.PointerEventDispatchExtensions)
- [Progress events](xref:Bunit.ProgressEventDispatchExtensions)
- [Touch event](xref:Bunit.TouchEventDispatchExtensions)

To use these, first find the element in the component under test where the event handler is bound to, this is usually done with the [`Find(string cssSelector)`](xref:Bunit.RenderedFragmentExtensions.Find(Bunit.IRenderedFragment,System.String)) method, and then invoke the event dispatch helper method of choice. 

The following section demonstrates how.

## Invoking an Event Handler on an Element

To invoke an event handler on an element, first find the element in the component under test, and then call the desired event dispatch helper method.

Let's look at a common example, where a `@onclick` event handler is invoked. The example will use the `<ClickMe>` component listed here:

[!code-cshtml[ClickMe.razor](../../../samples/components/ClickMe.razor)]

To trigger the `@onclick` `ClickHandler` event handler method in the `<ClickMe>` component, do the following:

# [C# test code](#tab/csharp)

[!code-csharp[ClickMeTest.cs](../../../samples/tests/xunit/ClickMeTest.cs?range=12-23&highlight=7-9)]

# [Razor test code](#tab/razor)

[!code-cshtml[ClickMeTest.razor](../../../samples/tests/razor/ClickMeTest.razor?highlight=17-19)]

***

This is what happens in the test:

1. In the arrange step of the test, the `<ClickMe>` component is rendered and the `<button>` element is found using the [`Find(string cssSelector)`](xref:Bunit.RenderedFragmentExtensions.Find(Bunit.IRenderedFragment,System.String)) method.
2. In the act step of the test, the `<button>`'s click event handler, in this case, the `ClickHandler` event handler method, is invoked in three different ways:
   - The first and second invocation uses the same [`Click`](xref:Bunit.MouseEventDispatchExtensions.Click(AngleSharp.Dom.IElement,System.Int64,System.Double,System.Double,System.Double,System.Double,System.Int64,System.Int64,System.Boolean,System.Boolean,System.Boolean,System.Boolean,System.String)) method. It has a number of optional arguments, some of which are passed in the second invocation. If any arguments are provided, they are added to an instance of the `MouseEventArgs` type, which is passed to the event handler, if it has it as an argument. 
   - The last invocation uses the [`Click`](xref:Bunit.MouseEventDispatchExtensions.Click(AngleSharp.Dom.IElement,Microsoft.AspNetCore.Components.Web.MouseEventArgs)) method that takes an instance of the `MouseEventArgs` type, which is passed to the event handler, if it has it as an argument.

All the event dispatch helper methods has the same two overloads, one that takes a number of optional arguments, and one that takes the event types `EventArgs` type.