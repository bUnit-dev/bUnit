---
uid: awaiting-async-state
title: Awaiting an asynchronous state change in a component under test
---

# Awaiting an asynchronous state change

A test can fail if a component performs asynchronous renders. This may be due to a reason such as waiting for an asynchronous operation to complete before continuing its render life-cycle. For example, if a component is waiting for an async web service to return data to it in the `OnInitializedAsync()` life-cycle method before rendering it to the render tree.

You need to handle this specifically in your tests because tests execute in the test framework's synchronization context and the test renderer executes renders in its own synchronization context. If you do not, you will likely experience tests that sometimes pass and sometimes fail.

bUnit comes with two methods that help to deal with this issue: the [`WaitForState()`](xref:Bunit.RenderedFragmentWaitForHelperExtensions.WaitForState(Bunit.RenderedFragment,System.Func{System.Boolean},System.Nullable{System.TimeSpan})) method covered on this page, and the [`WaitForAssertion()`](xref:Bunit.RenderedFragmentWaitForHelperExtensions.WaitForAssertion(Bunit.RenderedFragment,System.Action,System.Nullable{System.TimeSpan})) method covered on the <xref:async-assertion> page.

Let's start by taking a look at the `WaitForState` method in more detail.

## Waiting for state using `WaitForState`

The [`WaitForState(Func<Boolean>, TimeSpan?)`](xref:Bunit.RenderedFragmentWaitForHelperExtensions.WaitForState(Bunit.RenderedFragment,System.Func{System.Boolean},System.Nullable{System.TimeSpan})) method can be used to block and wait in a test method, until the provided predicate returns true or the timeout is reached. (The default timeout is one second.)

> [!NOTE]
> The `WaitForState()` method will try the predicate passed to it when the `WaitForState()` method is called, and every time the component under test renders.

Let us look at an example. Consider the following `<AsyncData>` component which awaits an async `TextService` in its `OnInitializedAsync()` life-cycle method. When the service returns the data, the component will automatically re-render to update its rendered markup:

[!code-cshtml[AsyncData.razor](../../../samples/components/AsyncData.razor)]

To test the `<AsyncData>` component, do the following:

[!code-csharp[AsyncDataTest.cs](../../../samples/tests/xunit/AsyncDataTest.cs?start=15&end=27&highlight=1,7,10,13)]

This is what happens in the test:

1. The test uses a `TaskCompletionSource<string>` to simulate an async web service.
2. In the second highlighted line, the result is provided to the component through the `textService`. This causes the component to re-render.
3. In the third highlighted line, the `WaitForState()` method is used to block the test until the predicate provided to it returns true.
4. Finally, the tests assertion step can execute, knowing that the desired state has been reached.

> [!WARNING]
> The wait predicate and an assertion should not verify the same thing. Instead, use the `WaitForAssertion(...)` method covered on the <xref:async-assertion> page instead.
 
### Controlling wait timeout

The timeout, which defaults to one second, can be controlled by passing a `TimeSpan` as the second argument to the `WaitForState()` method, e.g.:

[!code-csharp[](../../../samples/tests/xunit/AsyncDataTest.cs?start=43&end=43)]

If the timeout is reached, a <xref:Bunit.Extensions.WaitForHelpers.WaitForFailedException> exception is thrown with the following error message:

> The state predicate did not pass before the timeout period passed.

## Debugging code that uses `WaitForState`, `WaitForAssertion`, or `WaitForElement`

When `bUnit` detects that a debugger is attached (`Debugger.IsAttached`), it will automatically disable the timeout functionality of the "wait for" methods.