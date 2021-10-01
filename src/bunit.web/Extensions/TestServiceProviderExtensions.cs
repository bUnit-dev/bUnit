using System.Net.Http;
using Bunit.Diffing;
using Bunit.Rendering;
using Bunit.TestDoubles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.RenderTree;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.JSInterop;

namespace Bunit.Extensions
{
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
			services.AddSingleton<NavigationManager, FakeNavigationManager>();
			services.AddSingleton<INavigationInterception, FakeNavigationInterception>();

			// bUnit specific services
			services.AddSingleton<TestContextBase>(testContext);
			services.AddSingleton<WebTestRenderer>();
			services.AddSingleton<Renderer>(s => s.GetRequiredService<WebTestRenderer>());
			services.AddSingleton<ITestRenderer>(s => s.GetRequiredService<WebTestRenderer>());
			services.AddSingleton<HtmlComparer>();
			services.AddSingleton<BunitHtmlParser>();
			services.AddSingleton<IRenderedComponentActivator, RenderedComponentActivator>();

#if NET6_0_OR_GREATER
			services.AddSingleton<IErrorBoundaryLogger, BunitErrorBoundaryLogger>();
#endif
			return services;
		}
	}
}
