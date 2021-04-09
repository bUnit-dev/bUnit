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

The following sections demonstrate how to do. The examples we will cover will test the `<WeatherForecasts>` component listed below, which depends on the `IWeatherForecastService` service, injected in line 1:

[!code-cshtml[WeatherForecasts.razor](../../../samples/components/WeatherForecasts.razor?highlight=1)]

## Injecting services in tests

Here is a test that registers the `IWeatherForecastService` in the `Services` collection, which is a requirement of the `<WeatherForecasts>` component listed above.

[!code-csharp[WeatherForecastsTest.cs](../../../samples/tests/xunit/WeatherForecastsTest.cs?start=17&end=27&highlight=4)]

The highlighted line shows how the `IWeatherForecastService` is registered in the test context's `Services` collection, which is just a standard [`IServiceCollection`](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.dependencyinjection.iservicecollection), using the standard .NET Core dependency injection (DI) services method, [`AddSingleton`](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.dependencyinjection.servicecollectionserviceextensions.addsingleton?view=dotnet-plat-ext-3.1#Microsoft_Extensions_DependencyInjection_ServiceCollectionServiceExtensions_AddSingleton__1_Microsoft_Extensions_DependencyInjection_IServiceCollection___0_).

##  Fallback service provider

A fallback service provider can be registered with the built-in `TestServiceProvider`. This enables a few interesting use cases, such as using an alternative IoC container (which should implement the `IServiceProvider` interface), or automatically creating mock services for your Blazor components. The latter can be achieved by using a combination of [AutoFixture](https://github.com/AutoFixture/AutoFixture) and your favorite mocking framework, e.g. Moq, NSubsitute, or Telerik JustMock.

### When is the fallback service provider used?

The logic inside the `TestServiceProvider` for using the fallback service provider is as follows:

1. Try resolving the requested service from the standard service provider in bUnit.
2. If that fails, try resolving from a fallback service provider, if one exists.

In other words, the fallback service provider will always be tried after the default service provider has had a chance to fulfill a request for a service.

### Registering a fallback service provider

This is an example of how to implement and use a fallback service provider:

[!code-csharp[](../../../samples/tests/xunit/FallbackServiceProvider.cs?start=5&end=13)]

Here is a test where the fallback service provider is used:

[!code-csharp[](../../../samples/tests/xunit/FallBackServiceProviderUsage.cs?start=11&end=16)]

In this example, the `DummyService` is provided by the fallback service provider, since it is not register in the default service provider.

## Further reading

A closely related topic is mocking. To learn more about mocking in bUnit, go to the <xref:test-doubles> page.
