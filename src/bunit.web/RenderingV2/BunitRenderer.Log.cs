using Microsoft.Extensions.Logging;
using System.Reflection.Emit;

namespace Bunit.RenderingV2;

public partial class BunitRenderer
{
	[LoggerMessage(
		EventId = 1,
		Level = LogLevel.Error,
		Message = "An unhandled exception happened during rendering.")]
	private static partial void LogUnhandledException(ILogger logger, Exception exception);
}

