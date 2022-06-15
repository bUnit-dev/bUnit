using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;

namespace Bunit.Rendering;

/// <summary>
/// Represents a <see cref="ITestRenderer"/> that is used when rendering
/// Blazor components for the web.
/// </summary>
public class WebTestRenderer : TestRenderer
{
	/// <summary>
	/// Initializes a new instance of the <see cref="WebTestRenderer"/> class.
	/// </summary>
	public WebTestRenderer(IRenderedComponentActivator renderedComponentActivator, TestServiceProvider services, ILoggerFactory loggerFactory)
		: base(renderedComponentActivator, services, loggerFactory)
	{
		ElementReferenceContext = new WebElementReferenceContext(services.GetRequiredService<IJSRuntime>());
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="WebTestRenderer"/> class.
	/// </summary>
	public WebTestRenderer(IRenderedComponentActivator renderedComponentActivator, TestServiceProvider services, ILoggerFactory loggerFactory, IComponentActivator componentActivator)
		: base(renderedComponentActivator, services, loggerFactory, componentActivator)
	{
		ElementReferenceContext = new WebElementReferenceContext(services.GetRequiredService<IJSRuntime>());
	}
}
