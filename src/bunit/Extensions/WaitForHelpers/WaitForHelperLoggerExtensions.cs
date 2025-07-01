using Microsoft.Extensions.Logging;

namespace Bunit.Extensions.WaitForHelpers;

internal static partial class WaitForHelperLoggerExtensions
{
	[LoggerMessage(
		EventId = 1,
		EventName = "CheckingWaitCondition",
		Level = LogLevel.Debug,
		Message = "Checking the wait condition for component {ComponentId}.")]

	internal static partial void LogCheckingWaitCondition(this ILogger logger, int componentId);

	[LoggerMessage(
		EventId = 2,
		EventName = "CheckCompleted",
		Level = LogLevel.Debug,
		Message = "The check completed successfully for component {ComponentId}.")]
	internal static partial void LogCheckCompleted(this ILogger logger, int componentId);

	[LoggerMessage(
		EventId = 3,
		EventName = "CheckFailed",
		Level = LogLevel.Debug,
		Message = "The check failed for component {ComponentId}.")]
	internal static partial void LogCheckFailed(this ILogger logger, int componentId);

	[LoggerMessage(
		EventId = 4,
		EventName = "CheckThrow",
		Level = LogLevel.Debug,
		Message = "The checker for component {ComponentId} throw an exception.")]
	internal static partial void LogCheckThrow(this ILogger logger, int componentId, Exception exception);

	[LoggerMessage(
		EventId = 10,
		EventName = "WaiterTimedOut",
		Level = LogLevel.Debug,
		Message = "The waiter for component {ComponentId} timed out.")]
	internal static partial void LogWaiterTimedOut(this ILogger logger, int componentId);

	[LoggerMessage(
		EventId = 20,
		EventName = "WaiterDisposed",
		Level = LogLevel.Debug,
		Message = "The waiter for component {ComponentId} disposed.")]
	internal static partial void LogWaiterDisposed(this ILogger logger, int componentId);
}
