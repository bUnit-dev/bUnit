using AngleSharp.Dom;
using Bunit.TestAssets.BlazorE2E;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Xunit.Abstractions;

namespace Bunit.V2;

public class BunitContextTests : BunitContext
{
	private static ILoggerFactory CreateXunitLoggerFactory(ITestOutputHelper outputHelper)
	{
		var serilogLogger = new LoggerConfiguration()
			.MinimumLevel.Verbose()
			.Enrich.With(new ThreadIDEnricher())
			.WriteTo.TestOutput(
				testOutputHelper: outputHelper,
				restrictedToMinimumLevel: LogEventLevel.Verbose,
				outputTemplate: ThreadIDEnricher.DefaultConsoleOutputTemplate)
			.CreateLogger();

		return new LoggerFactory().AddSerilog(serilogLogger, dispose: true);
	}

	public BunitContextTests(ITestOutputHelper outputHelper)
		: base(CreateXunitLoggerFactory(outputHelper))
	{
	}

	[Fact]
	public async Task CanAcceptSimultaneousRenderRequests()
	{
		var expectedOutput = string.Join(
			string.Empty,
			Enumerable.Range(0, 100).Select(_ => "😊"));

		var cut = await RenderAsync<ConcurrentRenderParent>();

		await cut.OnAfterRenderAsync(
			() =>
			{
				Logger.LogInformation("Checking concurrent-render-output");
				Assert.Equal(expectedOutput, cut.Nodes.GetElementById("concurrent-render-output").TextContent.Trim());
			},
			timeout: TimeSpan.FromMilliseconds(2000));
	}
}
