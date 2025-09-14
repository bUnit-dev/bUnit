---
uid: dispose-components
title: Disposing components and their children
---

# Disposing components
To dispose all components rendered with a `BunitContext`, use the [`DisposeComponents`](xref:Bunit.BunitContext.DisposeComponentsAsync) method.  Calling this method will dispose all rendered components, calling any `IDisposable.Dispose` or/and `IAsyncDisposable.DisposeAsync` methods they might have, and remove the components from the render tree, starting with the root components and then walking down the render tree to all the child components.

Disposing rendered components enables testing of logic in `Dispose` methods, e.g., event handlers, that should be detached to avoid memory leaks.

The following example of this:

[!code-csharp[DisposeComponentsTest.cs](../../../samples/tests/xunit/DisposeComponentsTest.cs#L13-L22)]

> [!WARNING]
> For `IAsyncDisposable` (since .net5) relying on [`WaitForState()`](xref:Bunit.RenderedComponentWaitForHelperExtensions.WaitForState``1(Bunit.IRenderedComponent{``0},System.Func{System.Boolean},System.Nullable{System.TimeSpan})) or [`WaitForAssertion()`](xref:Bunit.RenderedComponentWaitForHelperExtensions.WaitForAssertion``1(Bunit.IRenderedComponent{``0},System.Action,System.Nullable{System.TimeSpan})) will not work as a disposed component will not trigger a new render cycle.

## Disposing components asynchronously
If a component implements `IAsyncDisposable`, `DisposeComponentsAsync` can be awaited to wait for all asynchronous `DisposeAsync` methods. Sometimes interacting with JavaScript in Blazor WebAssembly requires disposing or resetting state in `DisposeAsync`.

[!code-csharp[DisposeComponentsTest.cs](../../../samples/tests/xunit/DisposeComponentsTest.cs#L48-L53)]

To omit this behavior, discard the returned task

```csharp
_ = DisposeComponentsAsync();
```

## Checking for exceptions
`Dispose` as well as `DisposeAsync` can throw exceptions which can be asserted as well. If a component under test throws an exception in `Dispose` the [`DisposeComponents`](xref:Bunit.BunitContext.DisposeComponentsAsync) will throw the exception to the user code:

[!code-csharp[DisposeComponentsTest.cs](../../../samples/tests/xunit/DisposeComponentsTest.cs#L28-L32)]

`DisposeAsync` behaves a bit different. The following example will demonstrate how to assert an exception in `DisposeAsync`:

[!code-csharp[DisposeComponentsTest.cs](../../../samples/tests/xunit/DisposeComponentsTest.cs#L38-L42)]