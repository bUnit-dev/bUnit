using System;
#if NET5_0
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
#endif
using Microsoft.Extensions.Logging;
#if NET5_0
using Microsoft.JSInterop;
#endif

namespace Bunit.Rendering
{
	/// <summary>
	/// Represents a <see cref="ITestRenderer"/> that is used when rendering
	/// Blazor components for the web.
	/// </summary>
	public class WebTestRenderer : TestRenderer
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="WebTestRenderer"/> class.
		/// </summary>
		public WebTestRenderer(IRenderedComponentActivator activator, IServiceProvider services, ILoggerFactory loggerFactory)
			: base(activator, services, loggerFactory)
		{
#if NET5_0
			ElementReferenceContext = new WebElementReferenceContext(services.GetRequiredService<IJSRuntime>());
#endif
		}
	}
}
