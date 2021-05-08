using System;
#if !NETSTANDARD2_1
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
#endif
using Microsoft.Extensions.Logging;
#if !NETSTANDARD2_1
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
		public WebTestRenderer(IRenderedComponentActivator activator, TestServiceProvider services, ILoggerFactory loggerFactory)
			: base(activator, services, loggerFactory)
		{
#if NET5_0_OR_GREATER
			ElementReferenceContext = new WebElementReferenceContext(services.GetRequiredService<IJSRuntime>());
#endif
		}

#if NET5_0_OR_GREATER
		/// <summary>
		/// Initializes a new instance of the <see cref="WebTestRenderer"/> class.
		/// </summary>
		public WebTestRenderer(IRenderedComponentActivator activator, IServiceProvider services, ILoggerFactory loggerFactory, IComponentActivator componentActivator)
			: base(activator, services, loggerFactory, componentActivator)
		{
			ElementReferenceContext = new WebElementReferenceContext(services.GetRequiredService<IJSRuntime>());
		}
#endif
	}
}
