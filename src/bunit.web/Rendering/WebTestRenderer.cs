#if NET5_0_OR_GREATER
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
#endif
using Microsoft.Extensions.Logging;
#if NET5_0_OR_GREATER
using Microsoft.JSInterop;
#endif

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
#if NET5_0_OR_GREATER
		ElementReferenceContext = new WebElementReferenceContext(services.GetRequiredService<IJSRuntime>());
#endif
	}

#if NET5_0_OR_GREATER
	/// <summary>
	/// Initializes a new instance of the <see cref="WebTestRenderer"/> class.
	/// </summary>
	public WebTestRenderer(IRenderedComponentActivator renderedComponentActivator, TestServiceProvider services, ILoggerFactory loggerFactory, IComponentActivator componentActivator)
		: base(renderedComponentActivator, services, loggerFactory, componentActivator)
	{
		ElementReferenceContext = new WebElementReferenceContext(services.GetRequiredService<IJSRuntime>());
	}
#endif

	/// <inheritdoc/>
	public override Task DispatchEventAsync(ulong eventHandlerId, EventFieldInfo fieldInfo, EventArgs eventArgs, bool ignoreUnknownEventHandlers)
	{
		try
		{
			return base.DispatchEventAsync(eventHandlerId, fieldInfo, eventArgs, ignoreUnknownEventHandlers);
		}
		catch (ArgumentException ex) when (string.Equals(ex.Message, $"There is no event handler associated with this event. EventId: '{eventHandlerId}'. (Parameter 'eventHandlerId')", StringComparison.Ordinal))
		{
			if (ignoreUnknownEventHandlers)
			{
				return Task.CompletedTask;
			}

			var betterExceptionMsg = new UnknownEventHandlerIdException(eventHandlerId, fieldInfo, ex);
			return Task.FromException(betterExceptionMsg);
		}
	}
}
