---
uid: emulating-ijsruntime
title: Emulating Blazor's IJSRuntime
---

# Emulating Blazor's `IJSRuntime`

It is common for Blazor components to use `IJSRuntime` to call JavaScript, and since bUnit does not run JavaScript, emulating `IJSRuntime` is needed for components that use it. In that regard, `IJSRuntime` is no different than other services that a component might depend on.

bUnit comes with it's own JSInterop, a tailor-made implementation of `IJSRuntime` that is _active by default_, allowing you to specify how JavaScript interop calls should be handled and what values the calls should return, and also allowing you to verify that they the calls have happened. The implementation runs in "strict mode", which means means it will throw an exception if it receives an invocation it has not been configured to handle. See more about strict vs. loose mode in the following section.

If you prefer to use the same mocking framework for all mocking in your tests to keep things consistent, general-purpose mocking frameworks like [Moq](https://github.com/Moq), [JustMock Lite](https://github.com/telerik/JustMockLite), or [NSubstitute](https://nsubstitute.github.io/) all work nicely with bUnit and can be used to mock `IJSRuntime`. In general, registering an implementation of `IJSRuntime` with bUnit's `Services` collection replaces bUnit's implementation.

The following sections show how to use the built-in implementation of `IJSRuntime`.

> [!NOTE] 
> In the beta versions of bUnit you had to explicitly add the mock JSRuntime by calling `Services.AddMockJSRuntime()`. That is no longer needed, and indeed doesn't work any more.

## Strict vs. loose mode

bUnit's JSInterop can run in two modes, **strict** or **loose**:

- **Loose** mode configures the implementation to just return the default value when it receives an invocation that has not been explicitly set up, e.g. if a component calls `InvokeAsync<int>(...)` the mock will simply return `default(int)` back to it immediately.
- **Strict** mode configures the implementation to throw an exception if it is invoked with a method call it has _not_ been set up to handle explicitly. This is useful if you want to ensure that a component only performs a specific set of `IJSRuntime` invocations.

By default, the bUnit's JSInterop runs in **Strict** mode. To change the mode, do the following:

```csharp
using var ctx = new TestContext();
ctx.JSInterop.Mode = JSRuntimeMockMode.Loose;
```

## Setting up invocations

Use the `Setup<TResult>(...)` and `SetupVoid(...)` methods to configure the implementation to handle calls from the **matching** `InvokeAsync<TResult>(...)` and `InvokeVoidAsync(...)` methods on `IJSRuntime`.

Use the parameterless `Setup<TResult>()` method to emulate any call to `InvokeAsync<TResult>(...)` with a given return type `TResult` and use the parameterless `SetupVoid()` to emulate any call to `InvokeVoidAsync(...)`.

When an invocation is set up through of the `Setup<TResult>(...)` and `SetupVoid(...)` methods, a `JSRuntimePlannedInvocation<TResult>` object is returned. This can be used to set a result or an exception, to emulate what can happen during a JavaScript interop call in Blazor.

Similarly, when the parameterless `Setup<TResult>()` and `SetupVoid()` methods are used a `JSRuntimeCatchAllPlannedInvocation<TResult>` object is returned which can be used to set the result of invocation.

Here are two examples:

```csharp
using var ctx = new TestContext();

// Set up an invocation and specify the result value immediately
ctx.JSInterop.Setup<string>("getPageTitle").SetResult("bUnit is awesome");

// Set up an invocation without specifying the result
var plannedInvocation = ctx.JSInterop.SetupVoid("startAnimation");

// ... other test code

// Later in the test, mark the invocation as completed.
// SetResult() is not used in this case since InvokeVoidAsync
// only completes or throws, it doesn’t return a value.
// Any calls to InvokeVoidAsync(...) up till this point will
// have received an incomplete Task which the component 
// is likely waiting until the call to SetCompleted() below.
plannedInvocation.SetCompleted();
```
[__AP: In the above comment, last line,  I feel there should be something after 'waiting', but not quite sure what. (The  __]

## Verifying invocations

All calls to the `InvokeAsync<TResult>(...)` and `InvokeVoidAsync(...)` methods in bUnit's JSInterop are stored in its `Invocations` list, which can be inspected and asserted against. In addition to this, all planned invocations have their own `Invocations` list which only contain their invocations.

Invocations are represented by the `JSRuntimeInvocation` type which has three properties of interest when verifying an invocation happened as expected: 

- `Identifier` - the name of the function name/identifier passed to the invoke method.
- `Arguments` - a list of arguments passed to the invoke method.
- `CancellationToken` - the cancellation token passed to the invoke method (if any). 

To verify these, just use the assertion methods you normally use.

### Support for `IJSInProcessRuntime` and `IJSUnmarshalledRuntime`

bUnit's `IJSRuntime` supports being cast to the `IJSInProcessRuntime` and `IJSUnmarshalledRuntime` types, just like Blazors `IJSRuntime`. 

To set up a handler for a `Invoke` and `InvokeUnmarshalled` call, just use the regular `Setup` and `SetupVoid` methods on bUnit's JSInterop.

## Support for importing JavaScript Modules

Since the .NET 5 release of Blazor, it has been possible to import JavaScript modules directly from components. This is supported by bUnit's JSInterop through the `SetupModule` methods, that setup calls to `InvokeAsync<IJSObjectReference>`.

The `SetupModule` methods return a module JSInterop, that can be configured to handle the any JavaScript calls using the `Setup` and `SetupVoid` methods. For example, to configure bUnit's JSInterop to handle an import of the JavaScript module `hello.js`, and a call to the function `world()` in that model, do the following:

```csharp
using var ctx = new TestContext();

var moduleInterop = ctx.JSInterop.SetupModule("hello.js");
moduleInterop.SetupVoid("world");
```

### Module Interop Mode

By default, a module Interop inherits the `Mode` setting from the root JSInterop in bUnit. However, you can override it explicitly and have it in a different mode from other module Interop or the root JSInterop. Just set the `Mode` property, e.g.:

```csharp
var moduleInterop = ctx.JSInterop.SetupModule("hello.js");
moduleInterop.Mode = JSRuntimeMockMode.Loose;
```

### Support for `IJSInProcessObjectReference` and `IJSUnmarshalledObjectReference`

bUnit's `IJSObjectReference` supports being cast to the `IJSInProcessObjectReference` and `IJSUnmarshalledObjectReference` types, just like Blazors `IJSObjectReference`. 

To set up a handler for a `Invoke` and `InvokeUnmarshalled` call, just use the regular `Setup` and `SetupVoid` methods on bUnit's JSInterop.

## First Party JSInterop Component Emulation

Blazor comes out of the box with a few components that requires a working JSInterop. bUnit's JSInterop is setup to emulate the JavaScript interactions of those components. The following sections describes how the interaction is emulated for the supported components.

### <Virtualize> JSInterop Emulation

The `<Virtualize>` component require JavaScript to notify it about the available screen space it is being rendered to, and when the users scrolls the viewport, to trigger the loading of new data. bUnit emulates this interaction by telling the `<Virtualize>` component that the viewport is `1,000,000,000` pixels large. That should ensure that all items is loaded, which makes sense in a testing scenario.

To test the `<Placeholder>` template of the `<Virtualize>` component, create a items provider that doesn't return all items when queried.

### FocusAsync JSInterop Emulation

Support for the [`FocusAsync`](https://docs.microsoft.com/en-us/aspnet/core/blazor/components/event-handling?view=aspnetcore-5.0#focus-an-element) method on `ElementReference` in Blazor's .NET 5 release works by simply registering the invocations, which can then be verified to have happened.

To verify that the `FocusAsync` has been called in the `<ClickToFocus>` component:

```cshtml
<input @ref="exampleInput" />

<button @onclick="ChangeFocus">Focus the Input Element</button>

@code {
    private ElementReference exampleInput;
    private async Task ChangeFocus()
    {
        await exampleInput.FocusAsync();
    }
}
```

Do the following:

```csharp
using var ctx = new TestContext();
var cut = RenderComponent<ClickToFocus>();
var inputElement = cut.Find("input");

cut.Find("button").Click(); // Triggers onclick handler that sets focus of input element

ctx.JSInterop.VerifyFocusAsyncInvoke() // Verifies that a FocusAsync call has happenend
   .Arguments[0] // gets the first argument passed to the FocusAsync method
   .ShouldBeElementReferenceTo(inputElement); // verify that it is an element reference to the input element.
```

## Support for `IJSInProcessRuntime` and `IJSUnmarshalledRuntime`

bUnit's `IJSRuntime` supports being cast to the `IJSInProcessRuntime` and `IJSUnmarshalledRuntime` types, just like Blazors `IJSRuntime`. 

To set up a handler for a `Invoke` and `InvokeUnmarshalled` call, just use the regular `Setup` and `SetupVoid` methods on bUnit's JSInterop.
<!--stackedit_data:
eyJoaXN0b3J5IjpbLTkyNjUxOTA4NiwtOTI3MjM0MzNdfQ==
-->