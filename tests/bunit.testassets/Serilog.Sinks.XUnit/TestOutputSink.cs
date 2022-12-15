using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Serilog.Sinks.XUnit;

internal sealed class TestOutputSink : ILogEventSink
{
	private readonly IMessageSink messageSink;
	private readonly ITestOutputHelper testOutputHelper;
	private readonly ITextFormatter textFormatter;

	public TestOutputSink(IMessageSink messageSink, ITextFormatter textFormatter)
	{
		this.messageSink = messageSink ?? throw new ArgumentNullException(nameof(messageSink));
		this.textFormatter = textFormatter ?? throw new ArgumentNullException(nameof(textFormatter));
	}

	public TestOutputSink(ITestOutputHelper testOutputHelper, ITextFormatter textFormatter)
	{
		this.testOutputHelper = testOutputHelper ?? throw new ArgumentNullException(nameof(testOutputHelper));
		this.textFormatter = textFormatter ?? throw new ArgumentNullException(nameof(textFormatter));
	}

	public void Emit(LogEvent logEvent)
	{
		if (logEvent == null)
			throw new ArgumentNullException(nameof(logEvent));

		using var renderSpace = new StringWriter();
		textFormatter.Format(logEvent, renderSpace);
		var message = renderSpace.ToString().Trim();
		messageSink?.OnMessage(new DiagnosticMessage(message));
		testOutputHelper?.WriteLine(message);
	}
}
