using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Bunit.Extensions
{
	/// <summary>
	/// Helper extension methods for getting a logger.
	/// </summary>
	public static class LoggerHelperExtensions
	{
		/// <summary>
		/// Creates a logger from the <see cref="ILoggerFactory"/> registered in the <see cref="IServiceProvider"/>.
		/// </summary>
		/// <param name="services">The service to get the <see cref="ILoggerFactory"/> from.</param>
		/// <typeparam name="TCategoryName">The category for the logger.</typeparam>
		/// <returns>The <see cref="ILogger{TCategoryName}"/>.</returns>
		public static ILogger<TCategoryName> CreateLogger<TCategoryName>(this IServiceProvider services)
		{
			var loggerFactory = services.GetService<ILoggerFactory>() ?? NullLoggerFactory.Instance;
			return loggerFactory.CreateLogger<TCategoryName>();
		}
	}
}
