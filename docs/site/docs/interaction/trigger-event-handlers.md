---
uid: trigger-event-handlers
title: Triggering event handlers in components
---

# Triggering event handlers in components

Blazor makes it possible to bind many event handlers to elements in a Blazor component using the `@onXXXX` syntax, e.g. `@onclick="MyClickHandler"`. 

bUnit comes with event dispatch helper methods that makes it possible to invoke event handlers for all event types supported by Blazor.

**The built-in dispatch event helpers are:** [here](xref:Bunit.EventHandlerDispatchExtensions).

To use these, first find the element in the component under test where the event handler is bound. This is usually done with the [`Find(string cssSelector)`](xref:Bunit.RenderedComponentExtensions.Find``1(Bunit.IRenderedComponent{``0},System.String)) method. Next, invoke the event dispatch helper method of choice. 

The following section demonstrates how to do this...

## Invoking an event handler on an element

To invoke an event handler on an element, first find the element in the component under test, and then call the desired event dispatch helper method.

Let's look at a common example where an `@onclick` event handler is invoked. The example will use the `<ClickMe>` component listed here:

[!code-cshtml[ClickMe.razor](../../../samples/components/ClickMe.razor)]

To trigger the `@onclick` `ClickHandler` event handler method in the `<ClickMe>` component, do the following:

# [C# test code](#tab/csharp)

[!code-csharp[ClickMeTest.cs](../../../samples/tests/xunit/ClickMeTest.cs?range=7-25&highlight=9-11)]

# [Razor test code](#tab/razor)

[!code-cshtml[ClickMeTest.razor](../../../samples/tests/razor/ClickMeTest.razor?highlight=13-15)]

***

This is what happens in the test:

1. In the arrange step of the test, the `<ClickMe>` component is rendered and the `<button>` element is found using the [`Find(string cssSelector)`](xref:Bunit.RenderedComponentExtensions.Find``1(Bunit.IRenderedComponent{``0},System.String)) method.
2. The act step of the test is the `<button>`'s click event handler. In this case, the `ClickHandler` event handler method is invoked by calling the [`Click`](xref:Bunit.EventHandlerDispatchExtensions.Click(AngleSharp.Dom.IElement,Microsoft.AspNetCore.Components.Web.MouseEventArgs)) extension method on the found `<button>` element. The method takes an optional `MouseEventArgs` argument, which, if not supplied, will be initialized with default values.

All the event dispatch helper methods have the same two overloads: one that takes a number of optional arguments, and one that takes one of the `EventArgs` types provided by Blazor.

## Triggering custom events

bUnit support triggering custom events through the `TriggerEvent` method. 

Lets try to test the `<CustomPasteSample>` component below:

```cshtml
<p>Try pasting into the following text box:</p>
<input @oncustompaste="HandleCustomPaste" />
<p>@message</p>

@code {
  string message = string.Empty;
  void HandleCustomPaste(CustomPasteEventArgs eventArgs)
  {
    message = $"You pasted: {eventArgs.PastedData}";
  }
}
```

Here are the custom event types:

```csharp
[EventHandler("oncustompaste", typeof(CustomPasteEventArgs), enableStopPropagation: true, enablePreventDefault: true)]
public static class EventHandlers
{
  // This static class doesn't need to contain any members. It's just a place where we can put
  // [EventHandler] attributes to configure event types on the Razor compiler. This affects the
  // compiler output as well as code completions in the editor.
}

public class CustomPasteEventArgs : EventArgs
{
  // Data for these properties will be supplied by custom JavaScript logic
  public DateTime EventTimestamp { get; set; }
  public string PastedData { get; set; }
}
```

To trigger the `@oncustompaste` event callback, do the following:

```csharp
// Arrange
var cut = Render<CustomPasteSample>();

// Atc - find the input element and trigger the oncustompaste event
cut.Find("input").TriggerEvent("oncustompaste", new CustomPasteEventArgs
{
  EventTimestamp = DateTime.Now,
  PastedData = "FOO"
});

// Assert that the custom event data was passed correctly
cut.Find("p:last-child").MarkupMatches("<p>You pasted: FOO</p>");
```

## Using the `Async` version
All event dispatch helper methods have an `Async` version that returns a `Task`. Important to note is that the `Async` version will await the event handler callback **but not** the rendercycle that may be triggered by the event handler.

Example:

```csharp
await cut.Find("button").ClickAsync();
```