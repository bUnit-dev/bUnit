using Microsoft.Extensions.Logging;

namespace Bunit.Rendering;

internal static class TestRendererLoggerExtensions
{
	private static readonly Action<ILogger, Exception?> ProcessingPendingRenders
		= LoggerMessage.Define(LogLevel.Debug, new EventId(1, "ProcessPendingRender"), "Processing pending renders.");

	private static readonly Action<ILogger, Exception?> NewRenderBatchReceived
		= LoggerMessage.Define(LogLevel.Debug, new EventId(10, "UpdateDisplayAsync"), "New render batch received.");

	private static readonly Action<ILogger, int, Exception?> ComponentDisposed
		= LoggerMessage.Define<int>(LogLevel.Debug, new EventId(11, "UpdateDisplayAsync"), "Component {Id} has been disposed.");

	private static readonly Action<ILogger, int, Exception?> ComponentRendered
		= LoggerMessage.Define<int>(LogLevel.Debug, new EventId(12, "UpdateDisplayAsync"), "Component {Id} has been rendered.");

	private static readonly Action<ILogger, Exception?> ChangedComponentsMarkupUpdated
		= LoggerMessage.Define(LogLevel.Debug, new EventId(13, "UpdateDisplayAsync"), "Finished updating the markup of changed components.");

	private static readonly Action<ILogger, Exception?> AsyncInitialRender
		= LoggerMessage.Define(LogLevel.Debug, new EventId(20, "Render"), "The initial render task did not complete immediately.");

	private static readonly Action<ILogger, int, Exception?> InitialRenderCompleted
		= LoggerMessage.Define<int>(LogLevel.Debug, new EventId(21, "UpdateDisplayAsync"), "The initial render of component {Id} is completed.");

	private static readonly Action<ILogger, string, string, Exception> UnhandledException
		= LoggerMessage.Define<string, string>(LogLevel.Error, new EventId(30, "LogUnhandledException"), "An unhandled exception happened during rendering: {Message}" + Environment.NewLine + "{StackTrace}");

	internal static void LogProcessingPendingRenders(this ILogger<TestRenderer> logger)
		=> ProcessingPendingRenders(logger, null);

	internal static void LogNewRenderBatchReceived(this ILogger<TestRenderer> logger)
		=> NewRenderBatchReceived(logger, null);

	internal static void LogComponentDisposed(this ILogger<TestRenderer> logger, int componentId)
		=> ComponentDisposed(logger, componentId, null);

	internal static void LogComponentRendered(this ILogger<TestRenderer> logger, int componentId)
		=> ComponentRendered(logger, componentId, null);

	internal static void LogChangedComponentsMarkupUpdated(this ILogger<TestRenderer> logger)
		=> ChangedComponentsMarkupUpdated(logger, null);

	internal static void LogAsyncInitialRender(this ILogger<TestRenderer> logger)
		=> AsyncInitialRender(logger, null);

	internal static void LogInitialRenderCompleted(this ILogger<TestRenderer> logger, int componentId)
		=> InitialRenderCompleted(logger, componentId, null);

	internal static void LogUnhandledException(this ILogger<TestRenderer> logger, Exception exception)
		=> UnhandledException(logger, exception.Message, exception.StackTrace ?? string.Empty, exception);
}
