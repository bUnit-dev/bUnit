using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Bunit.Rendering;

internal static class TestRendererLoggerExtensions
{
	private static readonly Action<ILogger, int, Exception?> ComponentDisposed
		= LoggerMessage.Define<int>(
			LogLevel.Debug,
			new EventId(11, "ComponentDisposed"),
			"Component {Id} has been disposed.");

	internal static void LogComponentDisposed(this ILogger<TestRenderer> logger, int componentId)
	{
		if (logger.IsEnabled(LogLevel.Debug))
		{
			ComponentDisposed(logger, componentId, null);
		}
	}

	private static readonly Action<ILogger, int, Exception?> ComponentRendered
		= LoggerMessage.Define<int>(
			LogLevel.Debug,
			new EventId(12, "ComponentRendered"),
			"Component {ComponentId} has been rendered.");

	internal static void LogComponentRendered(this ILogger<TestRenderer> logger, int componentId)
	{
		if (logger.IsEnabled(LogLevel.Debug))
		{
			ComponentRendered(logger, componentId, null);
		}
	}

	private static readonly Action<ILogger, int, int, Exception?> DisposedChildInRenderTreeFrame
		= LoggerMessage.Define<int, int>(
			LogLevel.Warning,
			new EventId(14, "DisposedChildInRenderTreeFrame"),
			"A parent components {ParentComponentId} has a disposed component {ComponentId} as its child.");

	internal static void LogDisposedChildInRenderTreeFrame(this ILogger<TestRenderer> logger, int parentComponentId, int componentId)
	{
		if (logger.IsEnabled(LogLevel.Warning))
		{
			DisposedChildInRenderTreeFrame(logger, parentComponentId, componentId, null);
		}
	}

	private static readonly Action<ILogger, Exception?> AsyncInitialRender
		= LoggerMessage.Define(
			LogLevel.Debug,
			new EventId(20, "AsyncInitialRender"),
			"The initial render task did not complete immediately.");

	internal static void LogAsyncInitialRender(this ILogger<TestRenderer> logger)
	{
		if (logger.IsEnabled(LogLevel.Debug))
		{
			AsyncInitialRender(logger, null);
		}
	}

	private static readonly Action<ILogger, int, Exception?> InitialRenderCompleted
		= LoggerMessage.Define<int>(
			LogLevel.Debug,
			new EventId(21, "InitialRenderCompleted"),
			"The initial render of component {ComponentId} is completed.");

	internal static void LogInitialRenderCompleted(this ILogger<TestRenderer> logger, int componentId)
	{
		if (logger.IsEnabled(LogLevel.Debug))
		{
			InitialRenderCompleted(logger, componentId, null);
		}
	}

	private static readonly Action<ILogger, string, string, Exception> UnhandledException
		= LoggerMessage.Define<string, string>(
			LogLevel.Error,
			new EventId(30, "UnhandledException"),
			"An unhandled exception happened during rendering: {Message}" + Environment.NewLine + "{StackTrace}");

	internal static void LogUnhandledException(this ILogger<TestRenderer> logger, Exception exception)
	{
		if (logger.IsEnabled(LogLevel.Error))
		{
			UnhandledException(logger, exception.Message, exception.StackTrace ?? string.Empty, exception);
		}
	}

	private static readonly Action<ILogger, Exception?> RenderCycleActiveAfterDispose
		= LoggerMessage.Define(
			LogLevel.Warning,
			new EventId(31, "RenderCycleActiveAfterDispose"),
			"A component attempted to update the render tree after the renderer was disposed.");

	internal static void LogRenderCycleActiveAfterDispose(this ILogger<TestRenderer> logger)
	{
		if (logger.IsEnabled(LogLevel.Warning))
		{
			RenderCycleActiveAfterDispose(logger, null);
		}
	}

	private static readonly Action<ILogger, string, string, ulong, int, Exception?> DispatchingEventWithFieldValue
		= LoggerMessage.Define<string, string, ulong, int>(
			LogLevel.Debug,
			new EventId(40, "DispatchingEvent"),
			"Dispatching {EventArgs} to {FieldValue} handler (id = {EventHandlerId}) on component {ComponentId}.");

	private static readonly Action<ILogger, string, ulong, int, Exception?> DispatchingEvent
		= LoggerMessage.Define<string, ulong, int>(
			LogLevel.Debug,
			new EventId(40, "DispatchingEvent"),
			"Dispatching {EventArgs} to handler (id = {EventHandlerId}) on component {ComponentId}.");

	internal static void LogDispatchingEvent(this ILogger<TestRenderer> logger, ulong eventHandlerId, EventFieldInfo fieldInfo, EventArgs eventArgs)
	{
		if (logger.IsEnabled(LogLevel.Debug))
		{
			var eventType = eventArgs.GetType();
			var eventArgsText = eventType.Name;
			if (eventArgsText != nameof(EventArgs))
			{
				var eventArgsContent = JsonSerializer.Serialize(eventArgs, eventType);
				eventArgsText = eventArgsContent == "{}"
					? eventArgsText
					: $"{eventArgsText} = {eventArgsContent}";
			}

			if (fieldInfo.FieldValue is not null)
			{
				var fieldValueText = JsonSerializer.Serialize(fieldInfo.FieldValue, fieldInfo.FieldValue.GetType());
				DispatchingEventWithFieldValue(logger, eventArgsText, fieldValueText, eventHandlerId, fieldInfo.ComponentId, null);
			}
			else
			{
				DispatchingEvent(logger, eventArgsText, eventHandlerId, fieldInfo.ComponentId, null);
			}
		}
	}

}
