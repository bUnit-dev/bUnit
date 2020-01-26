using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace Egil.RazorComponents.Testing.TestUtililities
{
    /// <summary>
    /// Represents a <see cref="ILogger"/> that will write logs to the provided <see cref="ITestOutputHelper"/>.
    /// </summary>
    public class XunitLogger : ILogger
    {
        private readonly ITestOutputHelper _output;
        private readonly string _name;
        private readonly LogLevel _minimumLogLevel;

        /// <inheritdoc/>
        public XunitLogger(ITestOutputHelper output, string name, LogLevel minimumLogLevel)
        {
            _output = output;
            _name = name;
            _minimumLogLevel = minimumLogLevel;
        }

        /// <inheritdoc/>
        public IDisposable BeginScope<TState>(TState state) => throw new NotImplementedException("Scoped logging is not supported by XunitLogger.");

        /// <inheritdoc/>
        public bool IsEnabled(LogLevel logLevel) => logLevel >= _minimumLogLevel;

        /// <inheritdoc/>
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel)) return;
            if (formatter is null) return;
            _output.WriteLine($"{logLevel} | {_name} | {eventId.Id}:{eventId.Name} | {formatter(state, exception)}");
        }
    }
}
