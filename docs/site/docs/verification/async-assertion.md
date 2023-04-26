---
uid: async-assertion
title: Assertion of asynchronous changes
---

# Assertion of asynchronous changes

A test can fail if a component performs asynchronous renders. This may be due to a reason such as waiting for an asynchronous operation to complete before continuing its render life-cycle . For example, if a component is waiting for an async web service to return data to it in the `OnInitializedAsync()` life-cycle method before rendering it to the render tree.

You need to handle this specifically in your tests because tests execute in the test framework's synchronization context and the test renderer executes renders in its own synchronization context. If you do not, you will likely experience tests that sometimes pass, and sometimes fail.

bUnit comes with two methods that help to deal with this issue: the [`WaitForAssertion()`](xref:Bunit.RenderedFragmentWaitForHelperExtensions.WaitForAssertion(Bunit.IRenderedFragmentBase,Action,System.Nullable{TimeSpan})) method covered on this page, and the [`WaitForState()`](xref:Bunit.RenderedFragmentWaitForHelperExtensions.WaitForState(Bunit.IRenderedFragmentBase,Func{System.Boolean},System.Nullable{TimeSpan})) method covered on the <xref:awaiting-async-state> page.

Let's start by taking a look at the ` WaitForAssertion` method in more detail.

## Waiting for assertion to pass using `WaitForAssertion`

The [`WaitForAssertion(Action, TimeSpan?)`](xref:Bunit.RenderedFragmentWaitForHelperExtensions.WaitForAssertion(Bunit.IRenderedFragmentBase,Action,System.Nullable{TimeSpan})) method can be used to block and wait in a test method until the provided assert action does not throw an exception, or until the timeout is reached (the default timeout is one second).

> [!NOTE]
> The `WaitForAssertion()` method will try the assert action passed to it when the `WaitForAssertion()` method is called and every time the component under test renders.

Let's look at an example. Consider the following `<AsyncData>` component, which awaits an async `TextService` in its `OnInitializedAsync()` life-cycle method. When the service returns the data, the component will automatically re-render to update its rendered markup. 

[!code-cshtml[AsyncData.razor](../../../samples/components/AsyncData.razor)]

To test the `<AsyncData>` component, do the following:

[!code-csharp[AsyncDataTest.cs](../../../samples/tests/xunit/AsyncDataTest.cs?start=52&end=62&highlight=2,8,11)]

This is what happens in the test:

1. The test uses a `TaskCompletionSource<string>` to simulate an async web service.
2. In the second highlighted line, the result is provided to the component through the `textService`. This causes the component to re-render.
3. Finally, in the third highlighted line, the `WaitForAssertion()` method is used to block the test until the predicate assertion action runs without throwing an exception.
 
### Controlling wait timeout

The timeout, which defaults to one second, can be controlled by passing a `TimeSpan` as the second argument to the `WaitForAssertion()` method, e.g.:

[!code-csharp[](../../../samples/tests/xunit/AsyncDataTest.cs?start=63&end=63)]

If the timeout is reached, a <xref:Bunit.Extensions.WaitForHelpers.WaitForFailedException> exception is thrown with the following error message:

> The assertion did not pass within the timeout period.

Setting the timeout to something less than one second does _not_ make tests run faster. The `WaitForAssertion()` method returns as soon as it observes the predicate assertion running without throwing. So, it is generally only useful to set a different timeout than the default if the asynchronous operation takes longer than one second to complete, which should only be an issue in end-2-end or integration-testing scenarios.
