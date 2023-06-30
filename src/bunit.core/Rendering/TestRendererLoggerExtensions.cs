using Microsoft.Extensions.Logging;

namespace Bunit.Rendering;

internal static class TestRendererLoggerExtensions
{
	private static readonly Action<ILogger, int, Exception?> ComponentDisposed
		= LoggerMessage.Define<int>(LogLevel.Debug, new EventId(11, "ComponentDisposed"), "Component {Id} has been disposed.");

	private static readonly Action<ILogger, int, Exception?> ComponentRendered
		= LoggerMessage.Define<int>(LogLevel.Debug, new EventId(12, "ComponentRendered"), "Component {Id} has been rendered.");

	private static readonly Action<ILogger, int, int, Exception?> DisposedChildInRenderTreeFrame
		= LoggerMessage.Define<int, int>(LogLevel.Warning, new EventId(14, "DisposedChildInRenderTreeFrame"), "A parent components {ParentComponentId} has a disposed component {ComponentId} as its child.");

	private static readonly Action<ILogger, Exception?> AsyncInitialRender
		= LoggerMessage.Define(LogLevel.Debug, new EventId(20, "AsyncInitialRender"), "The initial render task did not complete immediately.");

	private static readonly Action<ILogger, int, Exception?> InitialRenderCompleted
		= LoggerMessage.Define<int>(LogLevel.Debug, new EventId(21, "InitialRenderCompleted"), "The initial render of component {Id} is completed.");

	private static readonly Action<ILogger, string, string, Exception> UnhandledException
		= LoggerMessage.Define<string, string>(LogLevel.Error, new EventId(30, "UnhandledException"), "An unhandled exception happened during rendering: {Message}" + Environment.NewLine + "{StackTrace}");

	private static readonly Action<ILogger, Exception?> RenderCycleActiveAfterDispose
		= LoggerMessage.Define(LogLevel.Warning, new EventId(31, "RenderCycleActiveAfterDispose"), "A component attempted to update the render tree after the renderer was disposed.");


	internal static void LogComponentDisposed(this ILogger<TestRenderer> logger, int componentId)
	{
		if (logger.IsEnabled(LogLevel.Debug))
		{
			ComponentDisposed(logger, componentId, null);
		}
	}

	internal static void LogComponentRendered(this ILogger<TestRenderer> logger, int componentId)
	{
		if (logger.IsEnabled(LogLevel.Debug))
		{
			ComponentRendered(logger, componentId, null);
		}
	}

	internal static void LogAsyncInitialRender(this ILogger<TestRenderer> logger)
	{
		if (logger.IsEnabled(LogLevel.Debug))
		{
			AsyncInitialRender(logger, null);
		}
	}

	internal static void LogInitialRenderCompleted(this ILogger<TestRenderer> logger, int componentId)
	{
		if (logger.IsEnabled(LogLevel.Debug))
		{
			InitialRenderCompleted(logger, componentId, null);
		}
	}

	internal static void LogUnhandledException(this ILogger<TestRenderer> logger, Exception exception)
	{
		if (logger.IsEnabled(LogLevel.Error))
		{
			UnhandledException(logger, exception.Message, exception.StackTrace ?? string.Empty, exception);
		}
	}

	internal static void LogRenderCycleActiveAfterDispose(this ILogger<TestRenderer> logger)
	{
		if (logger.IsEnabled(LogLevel.Warning))
		{
			RenderCycleActiveAfterDispose(logger, null);
		}
	}

	internal static void LogDisposedChildInRenderTreeFrame(this ILogger<TestRenderer> logger, int parentComponentId, int componentId)
	{
		if (logger.IsEnabled(LogLevel.Warning))
		{
			DisposedChildInRenderTreeFrame(logger, parentComponentId, componentId, null);
		}
	}
}
