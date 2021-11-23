using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Bunit.Rendering;
using Microsoft.Extensions.Logging;

namespace Bunit.Extensions.WaitForHelpers;

internal static class WaitForHelperLoggerExtensions
{
	private static readonly Action<ILogger, int, Exception?> CheckingWaitCondition
		= LoggerMessage.Define<int>(LogLevel.Debug, new EventId(1, "OnAfterRender"), "Checking the wait condition for component {Id}.");

	private static readonly Action<ILogger, int, Exception?> CheckCompleted
		= LoggerMessage.Define<int>(LogLevel.Debug, new EventId(2, "OnAfterRender"), "The check completed successfully for component {Id}.");

	private static readonly Action<ILogger, int, Exception?> CheckFailed
		= LoggerMessage.Define<int>(LogLevel.Debug, new EventId(3, "OnAfterRender"), "The check failed for component {Id}.");

	private static readonly Action<ILogger, int, Exception> CheckThrow
		= LoggerMessage.Define<int>(LogLevel.Debug, new EventId(4, "OnAfterRender"), "The checker for component {Id} throw an exception.");

	private static readonly Action<ILogger, int, Exception?> WaiterTimedOut
		= LoggerMessage.Define<int>(LogLevel.Debug, new EventId(10, "OnTimeout"), "The waiter for component {Id} timed out.");

	private static readonly Action<ILogger, int, Exception?> WaiterDisposed
		= LoggerMessage.Define<int>(LogLevel.Debug, new EventId(20, "OnTimeout"), "The waiter for component {Id} disposed.");

	internal static void LogCheckingWaitCondition<T>(this ILogger<WaitForHelper<T>> logger, int componentId)
		=> CheckingWaitCondition(logger, componentId, null);

	internal static void LogCheckCompleted<T>(this ILogger<WaitForHelper<T>> logger, int componentId)
		=> CheckCompleted(logger, componentId, null);

	internal static void LogCheckFailed<T>(this ILogger<WaitForHelper<T>> logger, int componentId)
		=> CheckFailed(logger, componentId, null);

	internal static void LogCheckThrow<T>(this ILogger<WaitForHelper<T>> logger, int componentId, Exception exception)
		=> CheckThrow(logger, componentId, exception);

	internal static void LogWaiterTimedOut<T>(this ILogger<WaitForHelper<T>> logger, int componentId)
		=> WaiterTimedOut(logger, componentId, null);

	internal static void LogWaiterDisposed<T>(this ILogger<WaitForHelper<T>> logger, int componentId)
		=> WaiterDisposed(logger, componentId, null);
}
