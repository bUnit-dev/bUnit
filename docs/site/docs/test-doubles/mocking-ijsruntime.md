---
uid: mocking-ijsruntime
title: Mocking Blazor's IJSRuntime
---

# Mocking Blazor's `IJSRuntime`

It is common for Blazor components to use `IJSRuntime` to call JavaScript, and since bUnit does not run JavaScript, mocking `IJSRuntime` is needed for components that uses it. In that regard, `IJSRuntime` is no different than other services that a component might depend on.

bUnit comes with a tailor built mock of `IJSRuntime`, that allows you to specify how JavaScript interop calls should be handled, what values they should return, and to verify that they have happened.

If you have more complex mocking needs, or you prefer to use the same mocking framework for all mocking in your tests to keep things consistent, general purpose mocking frameworks like [Moq](https://github.com/Moq), [JustMock Lite](https://github.com/telerik/JustMockLite), or [NSubstitute](https://nsubstitute.github.io/) all works nicely with bUnit.

The following sections shows how to use the built-in mock of `IJSRuntime`.

## Registering the mock `IJSRuntime`

A mock of `IJSRuntime` must be added to the `Services` collection, just like other services that a component under test requires. This is done like so:

```csharp
using var ctx = new TestContext();
var mockJS = ctx.Services.AddMockJSRuntime();
```

Calling `AddMockJSRuntime()` returns a <xref:Bunit.TestDoubles.JSInterop.MockJSRuntimeInvokeHandler>, which is used to set up expected calls and verify invocations.

### Strict vs loose mode

The `AddMockJSRuntime()` method takes an optional <xref:Bunit.TestDoubles.JSInterop.JSRuntimeMockMode> parameter as input, which defaults to `Loose`, if not provided.

- **Loose** mode configures the mock to just return the default value when it receives an invocation that has not been explicitly set up, e.g. if a component calls `InvokeAsync<int>(...)` the mock will simply return `default(int)` back to it immediately.
- **Strict** mode configures the mock to throw an exception if it is invoked with a method call it has _not_ been set up to handle explicitly. This is useful if you want to ensure that a component only performs a specific set of `IJSRuntime` invocations.

To set the mock to strict mode, do the following:

```csharp
using var ctx = new TestContext();
var mockJS = ctx.Services.AddMockJSRuntime(JSRuntimeMockMode.Strict);
```

## Setting up invocations

Use the `Setup<TResult>(...)` and `SetupVoid(...)` methods to configure the mock to handle calls from the matching `InvokeAsync<TResult>(...)` and `InvokeVoidAsync(...)` methods on `IJSRuntime`.

When an invocation is set up through the `Setup<TResult>(...)` and `SetupVoid(...)` methods, a `JSRuntimePlannedInvocation<TResult>` object is returned. This can be used to set a result or an exception, to emulate what can happen during a JavaScript interop call in Blazor.

Here are two examples:

```csharp
using var ctx = new TestContext();
var mockJS = ctx.Services.AddMockJSRuntime();

// Set up an invocation and specify the result value immidiately
mockJS.Setup<string>("getPageTitle").SetResult("bUnit is awesome");

// Set up an invocation without specifying the result
var plannedInvocation = mockJS.SetupVoid("startAnimation");

// ... other test code

// Later in the test, mark the invocation as completed.
// SetResult() is not used in this case since InvokeVoidAsync
// only completes or throws, it doesnt return a value.
// Any calls to InvokeVoidAsync(...) up till this point will
// have received an incompleted Task which the component 
// is likely awaiting until the call to SetCompleted() below.
plannedInvocation.SetCompleted();
```

## Verifying invocations

All calls to the `InvokeAsync<TResult>(...)` and `InvokeVoidAsync(...)` methods on the mock are stored in its `Invocations` list, which can be inspected and asserted against. In addition to this, all planned invocations has their own `Invocations` list, which only contains their invocations.

Invocations are represented by the `JSRuntimeInvocation` type, which has three properties of interest when verifying an invocation happened as expected: 

- `Identifier` - the name of the function name/identifier passed to the invoke method.
- `Arguments` - a list of arguments passed to the invoke method.
- `CancellationToken` - the cancellation token passed to the invoke method (if any).