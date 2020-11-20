using System;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;

namespace Bunit.Rendering
{
	/// <summary>
	/// Represents a <see cref="ITestRenderer"/> that is used when rendering
	/// Blazor components for the web.
	/// </summary>
	public class WebTestRenderer : TestRenderer
	{
		/// <summary>
		/// Creates an instance of the <see cref="WebTestRenderer"/>.
		/// </summary>
		public WebTestRenderer(IRenderedComponentActivator activator, IServiceProvider services, ILoggerFactory loggerFactory) : base(activator, services, loggerFactory)
		{
#if NET5_0
			ElementReferenceContext = new WebElementReferenceContext(services.GetRequiredService<IJSRuntime>());
#endif
		}
	}
}
