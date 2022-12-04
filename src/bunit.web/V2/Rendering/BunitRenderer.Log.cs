using Microsoft.Extensions.Logging;

namespace Bunit.V2.Rendering;

public partial class BunitRenderer : Renderer
{
	[LoggerMessage(
		EventId = 1,
		Level = LogLevel.Error,
		Message = "An unhandled exception happened during rendering.")]
	private static partial void LogUnhandledException(ILogger logger, Exception exception);

	[LoggerMessage(
		EventId = 2,
		Level = LogLevel.Debug,
		Message = "Processing pending renders.")]
	private static partial void LogProcessingPendingRenders(ILogger logger);

	[LoggerMessage(
		EventId = 3,
		Level = LogLevel.Debug,
		Message = "New render batch received.")]
	private static partial void LogNewRenderBatchReceived(ILogger logger);

	[LoggerMessage(
		EventId = 4,
		Level = LogLevel.Debug,
		Message = "Component {ComponentId} has been disposed.")]
	private static partial void LogComponentDisposed(ILogger logger, int componentId);

	[LoggerMessage(
		EventId = 5,
		Level = LogLevel.Debug,
		Message = "Component {ComponentId} has been rendered.")]
	private static partial void LogComponentRendered(ILogger logger, int componentId);

	[LoggerMessage(
		EventId = 6,
		Level = LogLevel.Debug,
		Message = "Finished updating the markup of changed components.")]
	private static partial void LogChangedComponentsMarkupUpdated(ILogger logger);

	[LoggerMessage(
		EventId = 7,
		Level = LogLevel.Debug,
		Message = "Finished updating the markup of changed components.")]
	private static partial void LogAsyncInitialRender(ILogger logger);
}
