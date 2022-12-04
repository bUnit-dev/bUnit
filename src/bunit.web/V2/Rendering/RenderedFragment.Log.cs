using Microsoft.Extensions.Logging;

namespace Bunit.V2.Rendering;

public partial class RenderedFragment
{
	[LoggerMessage(
		EventId = 1,
		Level = LogLevel.Debug,
		Message = "Rendered fragment updated.")]
	private static partial void LogRenderedFragmentUpdated(ILogger logger);

	[LoggerMessage(
		EventId = 10,
		Level = LogLevel.Debug,
		Message = "Trying on after render action.")]
	private static partial void LogTryOnAfterAction(ILogger logger);

	[LoggerMessage(
		EventId = 11,
		Level = LogLevel.Debug,
		Message = "On after render action completed successfully.")]
	private static partial void LogOnAfterActionCompleted(ILogger logger);

	[LoggerMessage(
		EventId = 12,
		Level = LogLevel.Debug,
		Message = "On after render action failed.")]
	private static partial void LogOnAfterActionFailed(ILogger logger);
}
