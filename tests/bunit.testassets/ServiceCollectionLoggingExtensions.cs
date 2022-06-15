using System.Globalization;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Xunit.Abstractions;

namespace Xunit;

public static class ServiceCollectionLoggingExtensions
{
	public static IServiceCollection AddXunitLogger(this IServiceCollection services, ITestOutputHelper outputHelper)
	{
		var serilogLogger = new LoggerConfiguration()
			.MinimumLevel.Verbose()
			.Enrich.With(new ThreadIDEnricher())
			.WriteTo.TestOutput(
				testOutputHelper: outputHelper,
				restrictedToMinimumLevel: LogEventLevel.Verbose,
				outputTemplate: ThreadIDEnricher.DefaultConsoleOutputTemplate)
			.CreateLogger();

		services.AddSingleton<ILoggerFactory>(new LoggerFactory().AddSerilog(serilogLogger, dispose: true));
		services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));
		return services;
	}

	private class ThreadIDEnricher : ILogEventEnricher
	{
		internal const string DefaultConsoleOutputTemplate = "{Timestamp:yyyy-MM-dd HH:mm:ss} ({ThreadID}) [{Level}] {Message}{NewLine}{Exception}";

		public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
		{
			logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(
			  "ThreadID", Environment.CurrentManagedThreadId.ToString("D4", CultureInfo.InvariantCulture)));
		}
	}
}
