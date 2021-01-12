using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Threading;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace Bunit.Xunit.Logging
{
	/// <summary>
	/// Represents a <see cref="ILogger"/> that will write logs to the provided <see cref="ITestOutputHelper"/>.
	/// </summary>
	public class XunitLogger : ILogger
	{
		private readonly ITestOutputHelper output;
		private readonly string name;
		private readonly LogLevel minimumLogLevel;

		/// <summary>
		/// Initializes a new instance of the <see cref="XunitLogger"/> class.
		/// </summary>
		/// <param name="output">The <see cref="ITestOutputHelper" /> to log to.</param>
		/// <param name="name">The name of the logger.</param>
		/// <param name="minimumLogLevel">The minimum log level to log.</param>
		public XunitLogger(ITestOutputHelper output, string name, LogLevel minimumLogLevel)
		{
			this.output = output;
			this.name = name;
			this.minimumLogLevel = minimumLogLevel;
		}

		/// <inheritdoc/>
		public IDisposable BeginScope<TState>(TState state) => throw new NotImplementedException("Scoped logging is not supported by XunitLogger.");

		/// <inheritdoc/>
		public bool IsEnabled(LogLevel logLevel) => logLevel >= minimumLogLevel;

		/// <inheritdoc/>
		[SuppressMessage("Design", "CA1031:Do not catch general exception types")]
		public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
		{
			if (!IsEnabled(logLevel))
				return;
			if (formatter is null)
				return;

			try
			{
				output.WriteLine($"{logLevel} | {Thread.GetCurrentProcessorId()} - {Thread.CurrentThread.ManagedThreadId} | {DateTime.UtcNow.ToString("o", CultureInfo.InvariantCulture)} | {name} | {eventId.Id}:{eventId.Name} | {formatter(state, exception)}");
			}
			catch
			{
				// This can throw an System.InvalidOperationException: There is no currently active test.
				// However, since there is nothing to do about it, we simply ignore it and continue.
			}
		}
	}
}
