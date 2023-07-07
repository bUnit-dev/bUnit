using Bunit.Diffing;
using Bunit.Rendering;
using Bunit.TestDoubles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Localization;

namespace Bunit.Extensions;

/// <summary>
/// Helper methods for correctly registering test dependencies.
/// </summary>
public static class TestServiceProviderExtensions
{
	/// <summary>
	/// Registers the default services required by the web <see cref="TestContext"/>.
	/// </summary>
	public static IServiceCollection AddDefaultTestContextServices(this IServiceCollection services, TestContextBase testContext, BunitJSInterop jsInterop)
	{
		if (services is null)
			throw new System.ArgumentNullException(nameof(services));
		if (testContext is null)
			throw new System.ArgumentNullException(nameof(testContext));
		if (jsInterop is null)
			throw new System.ArgumentNullException(nameof(jsInterop));

		// Placeholders and defaults for common Blazor services
		services.AddLogging();
		services.AddScoped<AuthenticationStateProvider, PlaceholderAuthenticationStateProvider>();
		services.AddScoped<IAuthorizationService, PlaceholderAuthorizationService>();
		services.AddScoped<HttpClient, PlaceholderHttpClient>();
		services.AddScoped<IStringLocalizer, PlaceholderStringLocalization>();

		// bUnits fake JSInterop
		services.AddScoped<IJSRuntime>(_ => jsInterop.JSRuntime);

		// bUnits fake Navigation Manager
		services.AddScoped<FakeNavigationManager>();
		services.AddScoped<NavigationManager>(s => s.GetRequiredService<FakeNavigationManager>());
		services.AddScoped<INavigationInterception, FakeNavigationInterception>();

		// bUnits fake WebAssemblyHostEnvironment
		services.AddScoped<FakeWebAssemblyHostEnvironment>();
		services.AddScoped<IWebAssemblyHostEnvironment>(s => s.GetRequiredService<FakeWebAssemblyHostEnvironment>());

		// bUnit specific services
		services.AddScoped<TestContextBase>(_ => testContext);
		services.AddScoped<WebTestRenderer>();
		services.AddScoped<TestRenderer>(s => s.GetRequiredService<WebTestRenderer>());
		services.AddScoped<Renderer>(s => s.GetRequiredService<WebTestRenderer>());
		services.AddScoped<ITestRenderer>(s => s.GetRequiredService<WebTestRenderer>());
		services.AddScoped<HtmlComparer>();
		services.AddScoped<BunitHtmlParser>();
		services.AddScoped<IRenderedComponentActivator, RenderedComponentActivator>();

		services.AddMemoryCache();

#if NET6_0_OR_GREATER
		services.AddScoped<IErrorBoundaryLogger, BunitErrorBoundaryLogger>();
#endif
		return services;
	}
}
