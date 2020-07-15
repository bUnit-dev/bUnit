using Bunit.Diffing;
using Bunit.Rendering;
using Bunit.TestDoubles.Authorization;
using Bunit.TestDoubles.JSInterop;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
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
			services.AddSingleton<AuthenticationStateProvider>(new PlaceholderAuthenticationStateProvider());
			services.AddSingleton<IAuthorizationService>(new PlaceholderAuthorizationService());
			services.AddSingleton<IJSRuntime>(new PlaceholderJSRuntime());
			services.AddSingleton<HtmlComparer>(srv => new HtmlComparer());
			services.AddSingleton<HtmlParser>(srv => new HtmlParser(
					srv.GetRequiredService<ITestRenderer>(),
					srv.GetRequiredService<HtmlComparer>()
				)
			);
			return services;
		}
	}
}
