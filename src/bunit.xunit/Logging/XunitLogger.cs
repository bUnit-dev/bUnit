using System;
using System.Threading;

using Microsoft.Extensions.Logging;

using Xunit.Abstractions;

namespace Bunit.Logging
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
			if (!IsEnabled(logLevel))
				return;
			if (formatter is null)
				return;

			try
			{
				_output.WriteLine($"{logLevel} | {Thread.GetCurrentProcessorId()} - {Thread.CurrentThread.ManagedThreadId} | {DateTime.UtcNow.ToString("o")} | {_name} | {eventId.Id}:{eventId.Name} | {formatter(state, exception)}");
			}
			catch
			{
				// This can throw an System.InvalidOperationException: There is no currently active test.
				// However, since there is nothing to do about it, we simply ignore it and continue.
			}
		}
	}
}
