using System;
using System.Net.Http;
using Bunit.Diffing;
using Bunit.Rendering;
using Bunit.TestDoubles.Authorization;
using Bunit.TestDoubles.HttpClient;
using Bunit.TestDoubles.JSInterop;
using Bunit.TestDoubles.Logging;
using Bunit.TestDoubles.NavigationManagement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
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
			services.AddSingleton<ILoggerFactory>(NullLoggerFactory.Instance);
			services.AddSingleton<AuthenticationStateProvider, PlaceholderAuthenticationStateProvider>();
			services.AddSingleton<IAuthorizationService, PlaceholderAuthorizationService>();
			services.AddSingleton<IJSRuntime, PlaceholderJSRuntime>();
			services.AddSingleton<NavigationManager, PlaceholderNavigationManager>();
			services.AddSingleton<HtmlComparer>();
			services.AddSingleton(new HttpClient(new PlaceholderHttpMessageHandler())
				{BaseAddress = new Uri("http://localhost:5000")});
			services.AddSingleton<ILoggerFactory, PlaceholderLogFactory>();
			services.AddSingleton<HtmlParser>();
			services.AddSingleton<IRenderedComponentActivator, RenderedComponentActivator>();
			return services;
		}
	}
}
