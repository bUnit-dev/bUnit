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
public static class BunitServiceProviderExtensions
{
	/// <summary>
	/// Registers the default services required by the web <see cref="BunitContext"/>.
	/// </summary>
	public static IServiceCollection AddDefaultBunitContextServices(this IServiceCollection services, BunitContext bunitContext, BunitJSInterop jsInterop)
	{
		ArgumentNullException.ThrowIfNull(services);
		ArgumentNullException.ThrowIfNull(bunitContext);
		ArgumentNullException.ThrowIfNull(jsInterop);

		// Placeholders and defaults for common Blazor services
		services.AddLogging();
		services.AddSingleton<AuthenticationStateProvider, PlaceholderAuthenticationStateProvider>();
		services.AddSingleton<IAuthorizationService, PlaceholderAuthorizationService>();
		services.AddSingleton<HttpClient, PlaceholderHttpClient>();
		services.AddSingleton<IStringLocalizer, PlaceholderStringLocalization>();

		// bUnits JSInterop
		services.AddSingleton(jsInterop.JSRuntime);

		// bUnits Navigation Manager
		services.AddSingleton<BunitNavigationManager>();
		services.AddSingleton<NavigationManager>(s => s.GetRequiredService<BunitNavigationManager>());
		services.AddSingleton<INavigationInterception, BunitNavigationInterception>();

		// bUnits WebAssemblyHostEnvironment
		services.AddSingleton<BunitWebAssemblyHostEnvironment>();
		services.AddSingleton<IWebAssemblyHostEnvironment>(s => s.GetRequiredService<BunitWebAssemblyHostEnvironment>());

		// bUnits ScrollToLocationHash
		services.AddSingleton<IScrollToLocationHash, BunitScrollToLocationHash>();
		services.AddSupplyValueFromQueryProvider();

		// bUnit specific services
		services.AddSingleton(bunitContext);
		services.AddSingleton<HtmlComparer>();
		services.AddSingleton<BunitHtmlParser>();

		services.AddMemoryCache();

		services.AddSingleton<IErrorBoundaryLogger, BunitErrorBoundaryLogger>();
		return services;
	}
}
