using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace Bunit.Xunit.Logging
{
	/// <summary>
	/// Represents an <see cref="ILoggerProvider"/> for logging to <see cref="XunitLogger"/>.
	/// </summary>
	public sealed class XunitLoggerProvider : ILoggerProvider
	{
		private readonly ITestOutputHelper output;
		private readonly LogLevel minimumLogLevel;

		/// <summary>
		/// Initializes a new instance of the <see cref="XunitLoggerProvider"/> class.
		/// </summary>
		/// <param name="output">The <see cref="ITestOutputHelper" /> to log to.</param>
		/// <param name="minimumLogLevel">The minimum log level to log.</param>
		public XunitLoggerProvider(ITestOutputHelper output, LogLevel minimumLogLevel = LogLevel.Debug)
		{
			this.output = output;
			this.minimumLogLevel = minimumLogLevel;
		}

		/// <inheritdoc/>
		public ILogger CreateLogger(string categoryName) => new XunitLogger(output, categoryName, minimumLogLevel);

		/// <inheritdoc/>
		public void Dispose()
		{
			// No disposable resources that need disposing.
		}
	}
}
