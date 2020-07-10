---
uid: async-assertion
title: Assertion of Asynchronous Changes
---

# Assertion of Asynchronous Changes

A test can fail if a component performs asynchronous renders, e.g. because it was awaiting a task to complete before continuing its render life-cycle. For example, if a component is waiting for an async web service to return data to it in the `OnInitializedAsync()` life-cycle method, before rendering it to the render tree.

This happens because tests execute in the test framework's synchronization context and the test renderer executes renders in its own synchronization context.

bUnit comes with two methods that helps deal with this issue, the [`WaitForAssertion(Action, TimeSpan?)`](xref:Bunit.RenderedFragmentWaitForHelperExtensions.WaitForAssertion(Bunit.IRenderedFragmentBase,System.Action,System.Nullable{System.TimeSpan})) method covered on this page, and the [`WaitForState(Func<Boolean>, TimeSpan?)`](xref:Bunit.RenderedFragmentWaitForHelperExtensions.WaitForState(Bunit.IRenderedFragmentBase,System.Func{System.Boolean},System.Nullable{System.TimeSpan})) method covered on the <xref:awaiting-async-state> page.

## Waiting for Assertion to Pass Using `WaitForAssertion`

The [`WaitForAssertion(Action, TimeSpan?)`](xref:Bunit.RenderedFragmentWaitForHelperExtensions.WaitForAssertion(Bunit.IRenderedFragmentBase,System.Action,System.Nullable{System.TimeSpan})) method can be used to block and wait in a test method, until the provided assert action does not throw an exception, or the timeout is reached (the default timeout is one second).

> [!NOTE]
> The `WaitForAssertion()` method will try the assert action pass to it when the `WaitForAssertion()` method is called, and every time the component under test renders.

Let us look at an example. Consider the following `<AsyncData>` component, who awaits an async `TextService` in its `OnInitializedAsync()` life-cycle method. When the service returns the data, the component will automatically re-render, to update its rendered markup. 

[!code-html[AsyncData.razor](../../../samples/components/AsyncData.razor)]

To test the `<AsyncData>` component, do the following:

[!code-csharp[AsyncDataTest.cs](../../../samples/tests/xunit/AsyncDataTest.cs?start=54&end=65&highlight=3,9,12)]

This is what happens in the test:

1. The test uses a `TaskCompletionSource<string>` to simulate an async web service.
2. In the second highlighted line, the result is provided to the component through the `textService`. This causes the component to re-render.
3. Finally, in the third highlighted line, the `WaitForAssertion()` method is used to block the test until the predicate assertion action runs without throwing an exception.
 
### Controlling Wait Timeout

The timeout, which defaults to one second, can be controlled by passing a `TimeSpan` as the second argument to the `WaitForAssertion()` method, e.g.:

[!code-csharp[](../../../samples/tests/xunit/AsyncDataTest.cs?start=66&end=66)]

If the timeout is reached, a <xref:Bunit.Extensions.WaitForHelpers.WaitForFailedException> exception is thrown with the following error message:

> The assertion did not pass within the timeout period.