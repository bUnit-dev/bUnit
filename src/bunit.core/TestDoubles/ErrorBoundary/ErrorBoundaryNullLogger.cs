#if NET6_0_OR_GREATER
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Web;

namespace Bunit.TestDoubles
{
	/// <summary>
	/// Dummy implementation of an IErrorBoundaryLogger (needed for ErrorBoundary component).
	/// </summary>
	public class ErrorBoundaryNullLogger: IErrorBoundaryLogger
	{
		/// <summary>
		/// Gets the default instance of ErrorBoundaryNullLogger.
		/// </summary>
		public static IErrorBoundaryLogger Instance { get; } = new ErrorBoundaryNullLogger();

		/// <summary>
		/// Initializes the instance of the <see cref="ErrorBoundaryNullLogger"/> class.
		/// </summary>
		private ErrorBoundaryNullLogger()
		{}

		/// <summary>
		/// Logs the supplied <paramref name="exception"/>.
		/// </summary>
		public ValueTask LogErrorAsync(Exception exception) => ValueTask.CompletedTask;
	}
}
#endif
