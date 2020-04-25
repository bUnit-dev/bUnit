using System.Threading;
using System.Threading.Tasks;
using Bunit;
using Bunit.RazorTesting;
using Bunit.Rendering;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Xunit.Abstractions;

namespace Xunit.Sdk
{
	internal class RazorTestInvoker
	{
		private RazorTestCase TestCase { get; }
		private IMessageSink DiagnosticMessageSink { get; }
		private IMessageBus MessageBus { get; }
		private object[] ConstructorArguments { get; }
		private ExceptionAggregator Aggregator { get; }
		private CancellationTokenSource CancellationTokenSource { get; }
		private ExecutionTimer Timer { get; } = new ExecutionTimer();

		public RazorTestInvoker(RazorTestCase testCase, IMessageSink diagnosticMessageSink, IMessageBus messageBus, object[] constructorArguments, ExceptionAggregator aggregator, CancellationTokenSource cancellationTokenSource)
		{
			TestCase = testCase;
			DiagnosticMessageSink = diagnosticMessageSink;
			MessageBus = messageBus;
			ConstructorArguments = constructorArguments;
			Aggregator = aggregator;
			CancellationTokenSource = cancellationTokenSource;
		}

		public async Task<RunSummary> RunTestCaseAsync()
		{
			RunSummary summary = new RunSummary();

			if (!MessageBus.QueueMessage(new TestCaseStarting(TestCase)))
			{
				CancellationTokenSource.Cancel();
				return summary;
			}

			try
			{
				var result = await RunTestAsync();
				summary.Aggregate(result);
			}
			finally
			{
				if (!MessageBus.QueueMessage(new TestCaseFinished(TestCase, summary.Time, summary.Total, summary.Failed, summary.Skipped)))
					CancellationTokenSource.Cancel();
			}

			return summary;
		}

		private async Task<RunSummary> RunTestAsync()
		{
			var runSummary = new RunSummary { Total = 1 };

			if (!MessageBus.QueueMessage(new TestStarting(TestCase)))
			{
				CancellationTokenSource.Cancel();
				return runSummary;
			}

			if (!string.IsNullOrEmpty(TestCase.SkipReason))
			{
				runSummary.Skipped++;

				if (!MessageBus.QueueMessage(new TestSkipped(TestCase, TestCase.SkipReason)))
					CancellationTokenSource.Cancel();

				return runSummary;
			}

			var output = await Aggregator.RunAsync(PrepareRazorTestRun);
			runSummary.Time = Timer.Total;

			var exception = Aggregator.ToException();

			TestResultMessage testResult;
			if (exception is null)
				testResult = new TestPassed(TestCase, runSummary.Time, output);
			else
			{
				testResult = new TestFailed(TestCase, runSummary.Time, output, exception);
				runSummary.Failed++;
			}

			if (!MessageBus.QueueMessage(testResult))
				CancellationTokenSource.Cancel();

			if (!MessageBus.QueueMessage(new TestFinished(TestCase, runSummary.Time, output)))
				CancellationTokenSource.Cancel();

			return runSummary;
		}

		private async Task<string> PrepareRazorTestRun()
		{
			var testOutputHelper = GetOrCreateTestOutputHelper();

			testOutputHelper.Initialize(MessageBus, TestCase);

			await RazorTestExecutionAsync(testOutputHelper);

			var output = testOutputHelper.Output;
			testOutputHelper.Uninitialize();

			return output;

			TestOutputHelper GetOrCreateTestOutputHelper()
			{
				TestOutputHelper? testOutputHelper = null;
				foreach (object obj in ConstructorArguments)
				{
					testOutputHelper = obj as TestOutputHelper;
					if (testOutputHelper != null)
						break;
				}

				if (testOutputHelper is null)
					testOutputHelper = new TestOutputHelper();

				return testOutputHelper;
			}
		}

		private async Task RazorTestExecutionAsync(TestOutputHelper testOutputHelper)
		{
			var test = CreateRazorTestComponent();

			if (test is null || CancellationTokenSource.IsCancellationRequested)
			{
				return;
			}

			try
			{
				Timer.Aggregate(() => test.Services.TryAddSingleton<ITestOutputHelper>(testOutputHelper));
				await InvokeRazorTest(test);
			}
			finally
			{
				DisposeRazorTestComponent(test);
			}
		}

		private Task InvokeRazorTest(RazorTest test)
		{
			return test.Timeout > 0
				? InvokeRazorTestWithTimeout(test)
				: Timer.AggregateAsync(test.RunTest);
		}

		private async Task InvokeRazorTestWithTimeout(RazorTest test)
		{
			var baseTask = Timer.AggregateAsync(test.RunTest);
			var resultTask = await Task.WhenAny(baseTask, Task.Delay(test.Timeout, CancellationTokenSource.Token));

			if (resultTask != baseTask)
				throw new TestTimeoutException(test.Timeout);
		}

		private RazorTest? CreateRazorTestComponent()
		{
			if (!MessageBus.QueueMessage(new TestClassConstructionStarting(TestCase)))
			{
				CancellationTokenSource.Cancel();
				return null;
			}

			try
			{
				RazorTest? test = null;

				Timer.Aggregate(async () =>
				{
					using var razorRenderer = new TestComponentRenderer();
					var tests = await razorRenderer.GetRazorTestsFromComponent(TestCase.TestMethod.GetTestComponentType());
					// TODO: handle case where test index doesn't match, verify other parts of test perhaps?
					test = tests[TestCase.TestIndex];
				});

				return test;
			}
			finally
			{
				if (!MessageBus.QueueMessage(new TestClassConstructionFinished(TestCase)))
					CancellationTokenSource.Cancel();
			}
		}

		private void DisposeRazorTestComponent(RazorTest test)
		{
			if (!MessageBus.QueueMessage(new TestClassDisposeStarting(TestCase)))
			{
				CancellationTokenSource.Cancel();
				return;
			}

			try
			{
				Timer.Aggregate(test.Dispose);
			}
			finally
			{
				if (!MessageBus.QueueMessage(new TestClassDisposeFinished(TestCase)))
					CancellationTokenSource.Cancel();
			}
		}
	}
}
