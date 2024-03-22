using Microsoft.Extensions.Logging;

namespace Bunit.Extensions.WaitForHelpers;

internal static class WaitForHelperLoggerExtensions
{
	private static readonly Action<ILogger, int, Exception?> CheckingWaitCondition
		= LoggerMessage.Define<int>(LogLevel.Debug, new EventId(1, "CheckingWaitCondition"), "Checking the wait condition for component {Id}.");

	private static readonly Action<ILogger, int, Exception?> CheckCompleted
		= LoggerMessage.Define<int>(LogLevel.Debug, new EventId(2, "CheckCompleted"), "The check completed successfully for component {Id}.");

	private static readonly Action<ILogger, int, Exception?> CheckFailed
		= LoggerMessage.Define<int>(LogLevel.Debug, new EventId(3, "CheckFailed"), "The check failed for component {Id}.");

	private static readonly Action<ILogger, int, Exception> CheckThrow
		= LoggerMessage.Define<int>(LogLevel.Debug, new EventId(4, "CheckThrow"), "The checker for component {Id} throw an exception.");

	private static readonly Action<ILogger, int, Exception?> WaiterTimedOut
		= LoggerMessage.Define<int>(LogLevel.Debug, new EventId(10, "WaiterTimedOut"), "The waiter for component {Id} timed out.");

	private static readonly Action<ILogger, int, Exception?> WaiterDisposed
		= LoggerMessage.Define<int>(LogLevel.Debug, new EventId(20, "WaiterDisposed"), "The waiter for component {Id} disposed.");

	internal static void LogCheckingWaitCondition(this ILogger logger, int componentId)
	{
		if (logger.IsEnabled(LogLevel.Debug))
		{
			CheckingWaitCondition(logger, componentId, null);
		}
	}

	internal static void LogCheckCompleted(this ILogger logger, int componentId)
	{
		if (logger.IsEnabled(LogLevel.Debug))
		{
			CheckCompleted(logger, componentId, null);
		}
	}

	internal static void LogCheckFailed(this ILogger logger, int componentId)
	{
		if (logger.IsEnabled(LogLevel.Debug))
		{
			CheckFailed(logger, componentId, null);
		}
	}

	internal static void LogCheckThrow(this ILogger logger, int componentId, Exception exception)
	{
		if (logger.IsEnabled(LogLevel.Debug))
		{
			CheckThrow(logger, componentId, exception);
		}
	}

	internal static void LogWaiterTimedOut(this ILogger logger, int componentId)
	{
		if (logger.IsEnabled(LogLevel.Debug))
		{
			WaiterTimedOut(logger, componentId, null);
		}
	}

	internal static void LogWaiterDisposed(this ILogger logger, int componentId)
	{
		if (logger.IsEnabled(LogLevel.Debug))
		{
			WaiterDisposed(logger, componentId, null);
		}
	}
}
