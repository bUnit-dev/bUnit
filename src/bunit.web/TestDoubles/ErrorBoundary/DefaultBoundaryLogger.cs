#if NET6_0_OR_GREATER
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Logging;

namespace Bunit.TestDoubles
{
	/// <summary>
	/// Default implementation of an IErrorBoundaryLogger (needed for ErrorBoundary component).
    /// It lays the LogErrorAsync logic to an existing ILoggerFactory instance.
	/// </summary>
	public class DefaultBoundaryLogger : IErrorBoundaryLogger
	{
		private readonly ILogger logger;

		/// <summary>
		/// Initializes the instance of the <see cref="DefaultBoundaryLogger"/> class.
		/// </summary>
		public DefaultBoundaryLogger(ILoggerFactory loggerFactory)
		{
            logger = loggerFactory.CreateLogger<DefaultBoundaryLogger>();
        }

		/// <summary>
		/// Logs the supplied <paramref name="exception"/>.
		/// </summary>
		public ValueTask LogErrorAsync(Exception exception)
		{
			logger.LogError(exception, string.Empty);
			return ValueTask.CompletedTask;
		}
	}
}
#endif
