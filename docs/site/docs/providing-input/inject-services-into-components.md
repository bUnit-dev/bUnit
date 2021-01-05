---
uid: inject-services
title: Injecting Services into Components Under Test
---

# Injecting Services into Components Under Test

It is common for components under test to have a dependency on services, injected into them through the `@inject IMyService MyService` syntax in .razor files, or the `[Inject] private IMyService MyService { get; set; }` syntax in .cs files.

This is supported in bUnit through the `Services` collection available through the test contexts used in both C# and Razor based tests. The `Services` collection is just a `IServiceCollection`, which means services can be registered in the same manner as done in production code in `Startup.cs` in Blazor Server projects and in `Program.cs` in Blazor Wasm projects.

In bUnit, you register the services in the `Services` collection _before_ you render a component under test. 

The following sections demonstrate how to do this in C# and Razor based tests. The examples we will cover will test the `<WeatherForecasts>` component listed below, which depends on the `IWeatherForecastService` service, injected in line 1:

[!code-cshtml[WeatherForecasts.razor](../../../samples/components/WeatherForecasts.razor?highlight=1)]

## Injecting Services in C# Based Tests

Here is a C# based test that registers the `IWeatherForecastService` in the `Services` collection, which is a requirement of the `<WeatherForecasts>` component listed above.

[!code-csharp[WeatherForecastsTest.cs](../../../samples/tests/xunit/WeatherForecastsTest.cs?start=17&end=27&highlight=4)]

The highlighted line shows how the `IWeatherForecastService` is registered in the test context's `Services` collection, which is just a standard [`IServiceCollection`](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.dependencyinjection.iservicecollection), using the standard .NET Core DI services method, [`AddSingleton`](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.dependencyinjection.servicecollectionserviceextensions.addsingleton?view=dotnet-plat-ext-3.1#Microsoft_Extensions_DependencyInjection_ServiceCollectionServiceExtensions_AddSingleton__1_Microsoft_Extensions_DependencyInjection_IServiceCollection___0_).

> [!NOTE]
> The `AddSingleton()` method is only available on the `Services` collection if you import the `Microsoft.Extensions.DependencyInjection` type.

## Injecting Services in Razor Based Tests

Here is a Razor based test that registers the `IWeatherForecastService` in the `Services` collection during the `Setup` methods, which is a requirement of the `<WeatherForecasts>` component mentioned above:

[!code-cshtml[WeatherForecastsTest.razor](../../../samples/tests/razor/WeatherForecastsTest.razor?highlight=10-13)]

The highlighted line shows how the `IWeatherForecastService` is registered using the standard .NET Core DI services method, [`AddSingleton`](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.dependencyinjection.servicecollectionserviceextensions.addsingleton?view=dotnet-plat-ext-3.1#Microsoft_Extensions_DependencyInjection_ServiceCollectionServiceExtensions_AddSingleton__1_Microsoft_Extensions_DependencyInjection_IServiceCollection___0_).

This can either be done via the `Fixture`'s `Setup` method as in this example, if you want to separate the service registration from the test method, or it can be done in the test method _before_ calling `GetComponentUnderTest()`.

The following example shows how to do this with `<SnapshotTest>` tests:

[!code-cshtml[WeatherForecastsTest.razor](../../../samples/tests/razor/WeatherForecastsSnapshotTest.html?highlight=5-8)]

> [!TIP]
> If multiple Razor tests share the same setup logic, they can share the same dedicated setup method as well.

> [!NOTE]
> The `AddSingleton()` method is only available on the `Services` collection if you import the `Microsoft.Extensions.DependencyInjection` type.

## Further Reading

A closely related topic is mocking. To learn more about mocking in bUnit, go to the <xref:test-doubles> page.
