using System;
using System.Net.Http;
using Bunit.Diffing;
using Bunit.Rendering;
using Bunit.TestDoubles.Authorization;
using Bunit.TestDoubles.HttpClient;
using Bunit.TestDoubles.JSInterop;
using Bunit.TestDoubles.Localization;
using Bunit.TestDoubles.NavigationManagement;
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
		// Have to make these fields so that the compiler thinks we will dispose of them
		// later
		private static HttpClient? _implementationInstance;
		private static PlaceholderHttpMessageHandler? _placeholderHttpMessageHandler;

		/// <summary>
		/// Registers the default services required by the web <see cref="TestContext"/>.
		/// </summary>
		public static IServiceCollection AddDefaultTestContextServices(this IServiceCollection services)
		{
			_placeholderHttpMessageHandler = new PlaceholderHttpMessageHandler();
			_implementationInstance = new HttpClient(_placeholderHttpMessageHandler)
				{BaseAddress = new Uri("http://localhost:5000")};

			services.AddSingleton<ILoggerFactory>(NullLoggerFactory.Instance);
			services.AddSingleton<AuthenticationStateProvider, PlaceholderAuthenticationStateProvider>();
			services.AddSingleton<IAuthorizationService, PlaceholderAuthorizationService>();
			services.AddSingleton<IJSRuntime, PlaceholderJSRuntime>();
			services.AddSingleton<NavigationManager, PlaceholderNavigationManager>();
			services.AddSingleton<HtmlComparer>();
			services.AddSingleton(_implementationInstance);
			// services.AddSingleton<ILoggerFactory, PlaceholderLogFactory>();
			services.AddSingleton<IStringLocalizer, PlaceholderStringLocalization>();
			services.AddSingleton<BunitHtmlParser>();
			services.AddSingleton<IRenderedComponentActivator, RenderedComponentActivator>();
			return services;
		}
	}
}
