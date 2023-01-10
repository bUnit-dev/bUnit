using Bunit.Diffing;
using Bunit.Rendering;
using Bunit.TestDoubles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Caching.Memory;
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
		services.AddSingleton<AuthenticationStateProvider, PlaceholderAuthenticationStateProvider>();
		services.AddSingleton<IAuthorizationService, PlaceholderAuthorizationService>();
		services.AddSingleton<HttpClient, PlaceholderHttpClient>();
		services.AddSingleton<IStringLocalizer, PlaceholderStringLocalization>();

		// bUnits fake JSInterop
		services.AddSingleton<IJSRuntime>(jsInterop.JSRuntime);

		// bUnits fake Navigation Manager
		services.AddSingleton<FakeNavigationManager>();
		services.AddSingleton<NavigationManager>(s => s.GetRequiredService<FakeNavigationManager>());
		services.AddSingleton<INavigationInterception, FakeNavigationInterception>();

		// bUnits fake WebAssemblyHostEnvironment
		services.AddSingleton<FakeWebAssemblyHostEnvironment>();
		services.AddSingleton<IWebAssemblyHostEnvironment>(s => s.GetRequiredService<FakeWebAssemblyHostEnvironment>());

		// bUnit specific services
		services.AddSingleton<TestContextBase>(testContext);
		services.AddSingleton<WebTestRenderer>();
		services.AddSingleton<Renderer>(s => s.GetRequiredService<WebTestRenderer>());
		services.AddSingleton<ITestRenderer>(s => s.GetRequiredService<WebTestRenderer>());
		services.AddSingleton<HtmlComparer>();
		services.AddSingleton<BunitHtmlParser>();
		services.AddSingleton<IRenderedComponentActivator, RenderedComponentActivator>();

		services.AddSingleton<IMemoryCache, MemoryCache>();

#if NET6_0_OR_GREATER
		services.AddSingleton<IErrorBoundaryLogger, BunitErrorBoundaryLogger>();
#endif
		return services;
	}
}
