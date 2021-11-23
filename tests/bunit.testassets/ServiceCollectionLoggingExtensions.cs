using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Xunit.Abstractions;

namespace Xunit;

public static class ServiceCollectionLoggingExtensions
{
	[SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope", Justification = "Serilog should dispose of its logger itself")]
	public static IServiceCollection AddXunitLogger(this IServiceCollection services, ITestOutputHelper outputHelper)
	{
		var serilogLogger = new LoggerConfiguration()
			.MinimumLevel.Verbose()
			.WriteTo.TestOutput(outputHelper, LogEventLevel.Verbose)
			.CreateLogger();

		services.AddSingleton<ILoggerFactory>(new LoggerFactory().AddSerilog(serilogLogger, dispose: true));
		services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));
		return services;
	}
}
