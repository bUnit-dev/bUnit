using Bunit.Xunit.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace Bunit
{
	/// <summary>
	/// Helper method for registering the xUnit test logger.
	/// </summary>
	public static class XunitLoggerExtensions
	{
		/// <summary>
		/// Adds the xUnit Logger to the service collection. All log statements logged during a test,
		/// matching the specified <see cref="LogLevel"/> (default <see cref="LogLevel.Debug"/>),
		/// will be available in the output from each unit tests.
		/// </summary>
		public static IServiceCollection AddXunitLogger(this IServiceCollection services, LogLevel minimumLogLevel = LogLevel.Debug)
		{
			services.AddSingleton(srv => new XunitLoggerProvider(srv.GetRequiredService<ITestOutputHelper>(), minimumLogLevel));
			services.AddSingleton<ILoggerFactory, XunitLoggerFactory>();
			return services;
		}

		/// <summary>
		/// Adds the xUnit Logger to the service collection. All log statements logged during a test,
		/// matching the specified <see cref="LogLevel"/> (default <see cref="LogLevel.Debug"/>),
		/// will be available in the output from each unit tests.
		/// </summary>
		public static IServiceCollection AddXunitLogger(this IServiceCollection services, ITestOutputHelper outputHelper, LogLevel minimumLogLevel = LogLevel.Debug)
		{
			services.AddSingleton<ITestOutputHelper>(outputHelper);
			return AddXunitLogger(services, minimumLogLevel);
		}
	}
}
