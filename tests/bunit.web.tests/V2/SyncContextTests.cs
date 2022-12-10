using System;
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

	////public BunitContextTests(ITestOutputHelper outputHelper)
	////	: base(CreateXunitLoggerFactory(outputHelper))
	////{
	////}

	[UIFact]
	public async Task Can_accept_simultaneous_render_requests()
	{
		var expectedOutput = string.Join(
			string.Empty,
			Enumerable.Range(0, 100).Select(_ => "😊"));

		var cut = await RenderAsync<ConcurrentRenderParent>();

		await cut.AssertAfterRenderAsync(
			() =>
			{
				Logger.LogInformation("Checking concurrent-render-output");
				Assert.Equal(expectedOutput, cut.Nodes.GetElementById("concurrent-render-output").TextContent.Trim());
			},
			timeout: TimeSpan.FromMilliseconds(2000));
	}

	[UITheory]
	[InlineData(0)]
	[InlineData(1)]
	[InlineData(2)]
	[InlineData(3)]
	[InlineData(4)]
	[InlineData(5)]
	[InlineData(6)]
	[InlineData(7)]
	[InlineData(8)]
	[InlineData(9)]
	[InlineData(10)]
	public async Task TriggerEventAsync_avoids_race_condition_with_DOM_tree_updates(int i)
	{
		var cut = await RenderAsync<CounterComponentDynamic>();

		await cut.AssertAfterRenderAsync(() => cut.Find("[data-id=1]"));

		await cut.Renderer.DispatchEventAsync(cut.Find("[data-id=1]"), "onclick");

		await cut.AssertAfterRenderAsync(() => cut.Find("[data-id=2]"));
	}
}
