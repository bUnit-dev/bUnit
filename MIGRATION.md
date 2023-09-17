# Migration Guide `v1` to `v2`
This document describes the changes that need to be made to migrate from bUnit 1.x to 2.x.

## Renamed all `RenderComponent` and `SetParametersAndRender` to `Render`

To make the API more consistent, `RenderComponent` and `SetParametersAndRender` methods have been renamed to `Render`.

## Removal of `ComponentParameter` and method using them

Using the `ComponentParameter` type and related factory methods is not recommended in V1 and have now been removed in V2. Instead, use the strongly typed builder pattern that enables you to pass parameters to components you render or write your tests in .razor files. Go to https://bunit.dev/docs/providing-input/passing-parameters-to-components to learn more.

## Change default "WaitFor" timeout

When running many tests in parallel on slow machines, e.g. CI runners like GitHub Actions or Azure DevOps, the default "WaitFor" timeout of 1 second may not be enough for the renderer to get processor time and be able to complete the expected renders. Thus, in v2, the default timeout is being increased to 30 seconds.

## Removal of `GetChangesSinceFirstRender` and `GetChangesSinceLastRender` methods
The `GetChangesSinceFirstRender` and `GetChangesSinceLastRender` methods have been removed from `IRenderedComponent<TComponent>`. There is no one-to-one replacement for these methods, but the general idea is to select the HTML in question via `Find` and assert against that.

Alternatively, the `IRenderFragment` still offers the `OnMarkupUpdated` event, which can be used to assert against the markup after a render.

## Removal of `IsNullOrEmpty` extension method on `IEnumerable<T>` and `CreateLogger` on `IServiceProvider`
The `IsNullOrEmpty` extension method on `IEnumerable<T>` has been removed, as well as the `CreateLogger` extension method on `IServiceProvider`. These extension methods are pretty common and conflict with other libraries. These methods can be recreated like this:

```csharp
public static class Extensions
{
    public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable)
        => enumerable == null || !enumerable.Any();
    
    public static ILogger<T> CreateLogger<T>(this IServiceProvider serviceProvider)
    {
        var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>() ?? NullLoggerFactory.Instance;
        return loggerFactory.CreateLogger<T>();
    }
}
```

## Merge of `bunit.core` and `bunit.web`
The `bunit.core` and `bunit.web` packages have been merged into a single `bunit` package. If you used either of these packages, you should remove them and install the `bunit` package instead.

## Removal of unneeded abstraction

### `IRenderedComponentBase<TComponent>`, `IRenderedFragmentBase`, `IRenderedFragment` and `IRenderedComponent<TComponent>` removed
`IRenderedComponentBase<TComponent>`, `IRenderedFragmentBase`, `IRenderedFragment` and `IRenderedComponent<TComponent>` have been removed. They were used as an abstraction and extension point. If you used either of these interfaces, you should replace them with `RenderedComponent<TComponent>` and `RenderedFragment` respectively.

### `WebTestRender` merged into `TestRender`
The `WebTestRender` class has been merged into the `TestRender` class. If you used `WebTestRender`, you should replace it with `TestRender`.

## `WaitFor` methods are asynchronous
`WaitForState`, `WaitForAssertion`, `WaitForElement` and `WaitForElements` are now asynchronous methods. Therefore they should be awaited and all of them have the `Async` suffix in the method name.

## `AddFallbackServiceProvider` renamed to `SetFallbackServiceProvider`
The `AddFallbackServiceProvider` on the `TestServiceProvider` has been renamed to `SetFallbackServiceProvider`. If you used `AddFallbackServiceProvider`, you should replace it with `SetFallbackServiceProvider`.

## Exceptions remove obsolete `SerializableAttribute` and `ISerializable` interface
With .NET 8 the `SerializableAttribute` and `ISerializable` interface have been marked as obsolete. Therefore, exceptions in bUnit no longer implement `ISerializable` and are no longer marked with the `SerializableAttribute`. More information can be found here: https://github.com/dotnet/docs/issues/34893

## Renamed `Fake` to `Bunit` in many test doubles
The `Fake` prefix has been replaced with `Bunit` in many test doubles. For example, `FakeNavigationManager` is now `BunitNavigationManager`. If you reference any of these types explicitly, you need to update your code.

### Renamed `AddTestAuthorization` to `AddAuthorization`
The `AddTestAuthorization` method on `TestContext` has been renamed to `AddAuthorization`. If you used `AddTestAuthorization`, you should replace it with `AddAuthorization`.

## Merged `TestContext` and `TestContextBase`
The `TestContext` and `TestContextBase` classes have been merged into a single `TestContext` class. All references to `TestContextBase` should replace them with `TestContext` to migrate.

## `TestContext` implements `IDisposable` and `IAsyncDisposable`
The `TestContext` now implements `IDisposable` and `IAsyncDisposable`. In version 1.x, `TestContext` only implemented `IDisposable` and cleaned up asynchronous objects in the synchronous `Dispose` method. This is no longer the case, and asynchronous objects are now cleaned up in the `DisposeAsync` method.
If you register services into the container that implement `IAsyncDisposable` make sure that the test framework calls the right method.
