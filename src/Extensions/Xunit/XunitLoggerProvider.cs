using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace Bunit.Extensions.Xunit
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
}
