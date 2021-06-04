#if NET6_0_OR_GREATER
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Logging;

namespace Bunit.TestDoubles
{
	/// <summary>
	/// Default implementation of an IErrorBoundaryLogger (needed for ErrorBoundary component).
    /// It delegates the implementation of LogErrorAsync to an instance created from ILoggerFactory.
	/// </summary>
	public class DefaultErrorBoundaryLogger : IErrorBoundaryLogger
	{
		private readonly ILogger logger;

		/// <summary>
		/// Initializes the instance of the <see cref="DefaultErrorBoundaryLogger"/> class.
		/// </summary>
		public DefaultErrorBoundaryLogger(ILoggerFactory loggerFactory)
		{
            logger = loggerFactory.CreateLogger<DefaultErrorBoundaryLogger>();
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
