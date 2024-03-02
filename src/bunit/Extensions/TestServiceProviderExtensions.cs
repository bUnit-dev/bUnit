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
		ArgumentNullException.ThrowIfNull(services);
		ArgumentNullException.ThrowIfNull(testContext);
		ArgumentNullException.ThrowIfNull(jsInterop);

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

		// bUnits fake ScrollToLocationHash
		services.AddSingleton<IScrollToLocationHash, BunitScrollToLocationHash>();
		services.AddSupplyValueFromQueryProvider();

		// bUnit specific services
		services.AddSingleton(testContext);
		services.AddSingleton<HtmlComparer>();
		services.AddSingleton<BunitHtmlParser>();
		services.AddSingleton<IRenderedComponentActivator, RenderedComponentActivator>();

		services.AddMemoryCache();

		services.AddSingleton<IErrorBoundaryLogger, BunitErrorBoundaryLogger>();
		return services;
	}
}
