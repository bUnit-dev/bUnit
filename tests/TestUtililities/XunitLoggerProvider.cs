using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit.Abstractions;

namespace Egil.RazorComponents.Testing.TestUtililities
{
    /// <summary>
    /// Represents an <see cref="ILoggerProvider"/> for logging to <see cref="XunitLogger"/>.
    /// </summary>
    public sealed class XunitLoggerProvider : ILoggerProvider
    {
        private readonly ITestOutputHelper _output;
        private readonly LogLevel _minimumLogLevel;

        /// <summary>
        /// Creates an instance of the <see cref="XunitLoggerProvider"/>.
        /// </summary>
        public XunitLoggerProvider(ITestOutputHelper output, LogLevel minimumLogLevel = LogLevel.Debug)
        {
            _output = output;
            _minimumLogLevel = minimumLogLevel;
        }

        /// <inheritdoc/>
        public ILogger CreateLogger(string categoryName) => new XunitLogger(_output, categoryName, _minimumLogLevel);

        /// <inheritdoc/>
        public void Dispose() { }
    }

    public class XunitLoggerFactory : LoggerFactory
    {
        public XunitLoggerFactory(XunitLoggerProvider xunitLoggerProvider)
        {
            AddProvider(xunitLoggerProvider);
        }
    }

    public static class XunitLoggerExtensions
    {
        public static IServiceCollection AddXunitLogger(this IServiceCollection services, ITestOutputHelper testOutput, LogLevel minimumLogLevel = LogLevel.Debug)
        {
            services.AddSingleton(srv => new XunitLoggerProvider(testOutput, minimumLogLevel));
            services.AddSingleton<ILoggerFactory, XunitLoggerFactory>();
            return services;
        }
    }

}
