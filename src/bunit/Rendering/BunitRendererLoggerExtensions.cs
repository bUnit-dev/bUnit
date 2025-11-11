using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Bunit.Rendering;

internal static partial class BunitRendererLoggerExtensions
{
	[LoggerMessage(
		EventId = 20,
		EventName = "AsyncInitialRender",
		Level = LogLevel.Debug,
		Message = "The initial render task did not complete immediately.")]
	internal static partial void LogAsyncInitialRender(this ILogger<BunitRenderer> logger);

	[LoggerMessage(
		EventId = 21,
		EventName = "InitialRenderCompleted",
		Level = LogLevel.Debug,
		Message = "The initial render of component {ComponentId} is completed.")]
	internal static partial void LogInitialRenderCompleted(this ILogger<BunitRenderer> logger, int componentId);

	private static readonly Action<ILogger, string, string, Exception> UnhandledException
		= LoggerMessage.Define<string, string>(
			LogLevel.Error,
			new EventId(30, "UnhandledException"),
			"An unhandled exception happened during rendering: {Message}" + Environment.NewLine + "{StackTrace}");

	internal static void LogUnhandledException(this ILogger<BunitRenderer> logger, Exception exception)
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

	internal static void LogRenderCycleActiveAfterDispose(this ILogger<BunitRenderer> logger)
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

	internal static void LogDispatchingEvent(this ILogger<BunitRenderer> logger, ulong eventHandlerId, EventFieldInfo fieldInfo, EventArgs eventArgs)
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
