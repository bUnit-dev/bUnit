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
	public static IServiceCollection AddDefaultTestContextServices(this IServiceCollection services, TestContext testContext, BunitJSInterop jsInterop)
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
		services.AddSingleton(jsInterop.JSRuntime);

		// bUnits fake Navigation Manager
		services.AddSingleton<BunitNavigationManager>();
		services.AddSingleton<NavigationManager>(s => s.GetRequiredService<BunitNavigationManager>());
		services.AddSingleton<INavigationInterception, BunitNavigationInterception>();

		// bUnits fake WebAssemblyHostEnvironment
		services.AddSingleton<BunitWebAssemblyHostEnvironment>();
		services.AddSingleton<IWebAssemblyHostEnvironment>(s => s.GetRequiredService<BunitWebAssemblyHostEnvironment>());

		// bUnits fake ScrollToLocationHash
		services.AddSingleton<IScrollToLocationHash, BunitScrollToLocationHash>();

		// bUnit specific services
		services.AddSingleton(testContext);
		services.AddSingleton<BunitRenderer>();
		services.AddSingleton<Renderer>(s => s.GetRequiredService<BunitRenderer>());
		services.AddSingleton<HtmlComparer>();
		services.AddSingleton<BunitHtmlParser>();
		services.AddSingleton<IRenderedComponentActivator, RenderedComponentActivator>();

		services.AddMemoryCache();

		services.AddSingleton<IErrorBoundaryLogger, BunitErrorBoundaryLogger>();
		return services;
	}
}
