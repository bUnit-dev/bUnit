using System.Net.Http;
using Bunit.Diffing;
using Bunit.Rendering;
using Bunit.TestDoubles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.JSInterop;

namespace Bunit.Extensions
{
	/// <summary>
	/// Helper methods for correctly registering test dependencies
	/// </summary>
	public static class TestServiceProviderExtensions
	{
		/// <summary>
		/// Registers the default services required by the web <see cref="TestContext"/>.
		/// </summary>
		public static IServiceCollection AddDefaultTestContextServices(this IServiceCollection services)
		{
			// Placeholders and defaults for common Blazor services
			services.AddSingleton<ILoggerFactory>(NullLoggerFactory.Instance);
			services.AddSingleton<AuthenticationStateProvider, PlaceholderAuthenticationStateProvider>();
			services.AddSingleton<IAuthorizationService, PlaceholderAuthorizationService>();
			services.AddSingleton<NavigationManager, PlaceholderNavigationManager>();
			services.AddSingleton<HttpClient, PlaceholderHttpClient>();
			services.AddSingleton<IStringLocalizer, PlaceholderStringLocalization>();

			// bUnit specific services
			services.AddSingleton<ITestRenderer, WebTestRenderer>();
			services.AddSingleton<HtmlComparer>();
			services.AddSingleton<BunitHtmlParser>();
			services.AddSingleton<IRenderedComponentActivator, RenderedComponentActivator>();

			return services;
		}
	}
}
