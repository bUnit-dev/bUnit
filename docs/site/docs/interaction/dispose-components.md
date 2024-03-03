---
uid: dispose-components
title: Disposing components and their children
---

# Disposing components
To dispose all components rendered with a `TestContext`, use the [`DisposeComponents`](xref:Bunit.TestContext.DisposeComponents) method.  Calling this method will dispose all rendered components, calling any `IDisposable.Dispose` or/and `IAsyncDisposable.DisposeAsync` methods they might have, and remove the components from the render tree, starting with the root components and then walking down the render tree to all the child components.

Disposing rendered components enables testing of logic in `Dispose` methods, e.g., event handlers, that should be detached to avoid memory leaks.

The following example of this:

[!code-csharp[](../../../samples/tests/xunit/DisposeComponentsTest.cs#L13-L22)]

> [!WARNING]
> For `IAsyncDisposable` (since .net5) relying on [`WaitForState()`](xref:Bunit.RenderedFragmentWaitForHelperExtensions.WaitForState(Bunit.IRenderedFragment,Func{System.Boolean},System.Nullable{TimeSpan})) or [`WaitForAssertion()`](xref:Bunit.RenderedFragmentWaitForHelperExtensions.WaitForAssertion(Bunit.IRenderedFragment,Action,System.Nullable{TimeSpan})) will not work as a disposed component will not trigger a new render cycle.

## Checking for exceptions
`Dispose` as well as `DisposeAsync` can throw exceptions which can be asserted as well. If a component under test throws an exception in `Dispose` the [`DisposeComponents`](xref:Bunit.TestContext.DisposeComponents) will throw the exception to the user code:

[!code-csharp[](../../../samples/tests/xunit/DisposeComponentsTest.cs#L28-L32)]

`DisposeAsync` behaves a bit different. The following example will demonstrate how to assert an exception in `DisposeAsync`:

[!code-csharp[](../../../samples/tests/xunit/DisposeComponentsTest.cs#L39-L43)]