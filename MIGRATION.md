# Migration Guide `v1` to `v2`
This document describes the changes that need to be made to migrate from bUnit 1.x to 2.x.

## Removal of `GetChangesSinceFirstRender` and `GetChangesSinceLastRender` methods
The `GetChangesSinceFirstRender` and `GetChangesSinceLastRender` methods have been removed from `RenderedComponent<TComponent>`. There is no one-to-one replacement for these methods, but the general idea is to select the HTML in question via `Find` and assert against that.

Alternatively, the `IRenderedFragment` still offers the `OnMarkupUpdated` event, which can be used to assert against the markup after a render.

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

### `IRenderedComponentBase<TComponent>` and `RenderedFragmentBase`
`IRenderedComponentBase<TComponent>`, `IRenderedComponent<TComponent>`, `IRenderedFragmentBase`, `IRenderedFragment` and `RenderedFragmentBase` have been removed.
If you used either of these types, you should replace them with `RenderedComponent<TComponent>` or `RenderedFragment` respectively.

### `WebTestRender` merged into `BunitTestRender`
The `WebTestRender` class has been merged into the `TestRender` class. If you used `WebTestRender`, you should replace it with `BunitTestRender`.

## Renamed `Fake` to `Bunit` in many test doubles
The `Fake` prefix has been replaced with `Bunit` in many test doubles. For example, `FakeNavigationManager` is now `BunitNavigationManager`. If you reference any of these types explicitly, you need to update your code.

### Renamed `AddTestAuthorization` to `AddAuthorization`
The `AddTestAuthorization` method on `BunitContext` has been renamed to `AddAuthorization`. If you used `AddTestAuthorization`, you should replace it with `AddAuthorization`.

## Merged `BunitContext` and `BunitContextBase`
The `BunitContext` and `BunitContextBase` classes have been merged into a single `BunitContext` class. All references to `BunitContextBase` should replace them with `BunitContext` to migrate.

## Renamed all `RenderComponent` and `SetParametersAndRender` to `Render`
To make the API more consistent, `RenderComponent` and `SetParametersAndRender` methods have been renamed to `Render`.

## Removal of `ComponentParameter` and method using them
Using `ComponentParameter` and factory methods to create them is not recommend in V1 and have now been removed in V2. Instead, use the strongly typed builder pattern that enables you to pass parameters to components you render.

## `BunitContext` implements `IDisposable` and `IAsyncDisposable`
The `BunitContext` now implements `IDisposable` and `IAsyncDisposable`. In version 1.x, `BunitContext` only implemented `IDisposable` and cleaned up asynchronous objects in the synchronous `Dispose` method. This is no longer the case, and asynchronous objects are now cleaned up in the `DisposeAsync` method.
If you register services into the container that implement `IAsyncDisposable` make sure that the test framework calls the right method.

## `TestContext` was renamed to `BunitContext`
The `TestContext` class has been renamed to `BunitContext`. If you used `TestContext`, you should replace it with `BunitContext`.

## `TestContextWrapper` was removed
The `TestContextWrapper` class has been removed. Either use lifecycle events of the testing framework (like `LifeCycle.InstancePerTestCase` in NUnit).
```csharp
[FixtureLifeCycle(LifeCycle.InstancePerTestCase)]
public class HelloWorldInstancePerTestCase : Bunit.TestContext
{
  [Test]
  public void HelloWorldComponentRendersCorrectly()
  {
    // Act
    var cut = RenderComponent<HelloWorld>();

    // Assert
    cut.MarkupMatches("<h1>Hello world from Blazor</h1>");
  }
}
```

Or use the `BunitContext` directly and manage the lifecycle yourself.

## `TestServiceProvider` renamed to `BunitTestServiceProvider`
The `TestServiceProvider` class has been renamed to `BunitTestServiceProvider`. If you used `TestServiceProvider`, you should replace it with `BunitTestServiceProvider`.
