---
uid: inject-services
title: Injecting services into components under test
---

# Injecting services into components under test

It is common for components under test to have a dependency on services, injected into them through the `@inject IMyService MyService` syntax in .razor files, or the `[Inject] private IMyService MyService { get; set; }` syntax in .cs files.

This is supported in bUnit through the `Services` collection available through the test context. The `Services` collection is just an `IServiceCollection`, which means services can be registered in the same manner as done for production code in `Startup.cs` in Blazor Server projects and in `Program.cs` in Blazor WASM projects.

In bUnit, you register the services in the `Services` collection _before_ you render a component under test. 

> [!NOTE]
> The `AddSingleton()` method is only available on the `Services` collection if you **import the `Microsoft.Extensions.DependencyInjection` namespace in your test class**.

The following sections demonstrate how to do this. The examples we will cover will test the `<WeatherForecasts>` component listed below, which depends on the `IWeatherForecastService` service, injected in line 1:

[!code-cshtml[WeatherForecasts.razor](../../../samples/components/WeatherForecasts.razor?highlight=1)]

## Injecting services in tests

Here is a test that registers the `IWeatherForecastService` in the `Services` collection, which is a requirement of the `<WeatherForecasts>` component listed above.

[!code-csharp[WeatherForecastsTest.cs](../../../samples/tests/xunit/WeatherForecastsTest.cs?start=16&end=24&highlight=2)]

The highlighted line shows how the `IWeatherForecastService` is registered in the test context's `Services` collection, which is just a standard [`IServiceCollection`](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.dependencyinjection.iservicecollection), using the standard .NET Core dependency injection (DI) services method, [`AddSingleton`](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.dependencyinjection.servicecollectionserviceextensions.addsingleton?view=dotnet-plat-ext-3.1#Microsoft_Extensions_DependencyInjection_ServiceCollectionServiceExtensions_AddSingleton__1_Microsoft_Extensions_DependencyInjection_IServiceCollection___0_).

##  Fallback service provider

A fallback service provider can be registered with the built-in `BunitServiceProvider`. This enables a few interesting use cases, such as using an alternative IoC container (which should implement the `IServiceProvider` interface), or automatically creating mock services for your Blazor components. The latter can be achieved by using a combination of [AutoFixture](https://github.com/AutoFixture/AutoFixture) and your favorite mocking framework, e.g. Moq, NSubsitute, or Telerik JustMock.

### When is the fallback service provider used?

The logic inside the `BunitServiceProvider` for using the fallback service provider is as follows:

1. Try resolving the requested service from the standard service provider in bUnit.
2. If that fails, try resolving from a fallback service provider, if one exists.

In other words, the fallback service provider will always be tried after the default service provider has had a chance to fulfill a request for a service.

### Registering a fallback service provider

This is an example of how to implement and use a fallback service provider:

[!code-csharp[](../../../samples/tests/xunit/FallbackServiceProvider.cs?start=5&end=13)]

Here is a test where the fallback service provider is used:

[!code-csharp[](../../../samples/tests/xunit/FallBackServiceProviderUsage.cs?start=11&end=15)]

In this example, the `DummyService` is provided by the fallback service provider, since it is not registered in the default service provider.

## Using a custom IServiceProvider implementation
A custom service provider factory can be registered with the built-in `BunitServiceProvider`. It is used to create the underlying IServiceProvider. This enables a few interesting use cases, such as using an alternative IoC container (which should implement the `IServiceProvider` interface). This approach can be useful if the fallback service provider is not an option. For example, if you have dependencies in the fallback container, that rely on dependencies which are in the main container and vice versa.

### Registering Autofac service provider factory
The example makes use of `AutofacServiceProviderFactory` and `AutofacServiceProvider` from the package `Autofac.Extensions.DependencyInjection` and shows how to use an Autofac dependency container with bUnit.

Here is a test where the Autofac service provider factory is used:

[!code-csharp[](../../../samples/tests/xunit/CustomServiceProviderFactoryUsage.cs?start=32&end=56)]

Here is a test where the Autofac service provider is used via delegate:

[!code-csharp[](../../../samples/tests/xunit/CustomServiceProviderFactoryUsage.cs?start=58&end=88)]

### Registering a custom service provider factory
The examples contain dummy implementations of `IServiceProvider` and `IServiceProviderFactory<TContainerBuilder>`. Normally those implementations are supplied by the creator of your custom dependency injection solution (e.g. Autofac example above). This dummy implementations are not intended to use as is.

This is an example of how to implement and use a dummy custom service provider factory.

[!code-csharp[](../../../samples/tests/xunit/CustomServiceProviderFactory.cs?start=8&end=49)]

Here is a test where the custom service provider factory is used:

[!code-csharp[](../../../samples/tests/xunit/CustomServiceProviderFactoryUsage.cs?start=15&end=19)]

Here is a test where the custom service provider is used via delegate:

[!code-csharp[](../../../samples/tests/xunit/CustomServiceProviderFactoryUsage.cs?start=25&end=29)]

## Further reading

A closely related topic is mocking. To learn more about mocking in bUnit, go to the <xref:test-doubles> page.
