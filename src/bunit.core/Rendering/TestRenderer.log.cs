using System;
using Microsoft.Extensions.Logging;

namespace Bunit.Rendering
{
	public partial class TestRenderer
	{
		private void LogUnhandledException(Exception unhandled)
		{
			var evt = new EventId(3, nameof(AssertNoUnhandledExceptions));
			_logger.LogError(evt, unhandled, $"An unhandled exception happened during rendering: {unhandled.Message}{Environment.NewLine}{unhandled.StackTrace}");
		}
	}
}
