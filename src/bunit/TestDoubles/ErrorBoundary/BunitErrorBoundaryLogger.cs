using Microsoft.Extensions.Logging;

namespace Bunit.TestDoubles;

/// <summary>
/// Default implementation of an IErrorBoundaryLogger (needed for ErrorBoundary component).
/// It delegates the implementation of LogErrorAsync to an instance created from ILoggerFactory.
/// </summary>
internal partial class BunitErrorBoundaryLogger : IErrorBoundaryLogger
{
	private readonly ILogger logger;

	/// <summary>
	/// Initializes the instance of the <see cref="BunitErrorBoundaryLogger"/> class.
	/// </summary>
	public BunitErrorBoundaryLogger(ILoggerFactory loggerFactory)
	{
		logger = loggerFactory.CreateLogger<BunitErrorBoundaryLogger>();
	}

	/// <summary>
	/// Logs the supplied <paramref name="exception"/>.
	/// </summary>
	public ValueTask LogErrorAsync(Exception exception)
	{
		ExceptionCaughtByErrorBoundary(logger, exception.Message, exception);
		return ValueTask.CompletedTask;
	}

	[LoggerMessage(
		EventId = 100,
		EventName = "ExceptionCaughtByErrorBoundary",
		Level = LogLevel.Warning,
		Message = "Unhandled exception rendering component: {Message}")]
	private static partial void ExceptionCaughtByErrorBoundary(
		ILogger logger,
		string message,
		Exception exception);
}
