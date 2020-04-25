using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Bunit.RazorTesting;
using Bunit.Rendering;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Xunit.Sdk
{
	public class RazorTestInvoker
	{
		private static readonly TestComponentRenderer RazorRenderer = new TestComponentRenderer();

		public RazorTestCase TestCase { get; }
		public IMessageSink DiagnosticMessageSink { get; }
		public IMessageBus MessageBus { get; }
		public object[] ConstructorArguments { get; }
		public ExceptionAggregator Aggregator { get; }
		public CancellationTokenSource CancellationTokenSource { get; }
		public ExecutionTimer Timer { get; } = new ExecutionTimer();

		public RazorTestInvoker(RazorTestCase testCase, IMessageSink diagnosticMessageSink, IMessageBus messageBus, object[] constructorArguments, ExceptionAggregator aggregator, CancellationTokenSource cancellationTokenSource)
		{
			TestCase = testCase;
			DiagnosticMessageSink = diagnosticMessageSink;
			MessageBus = messageBus;
			ConstructorArguments = constructorArguments;
			Aggregator = aggregator;
			CancellationTokenSource = cancellationTokenSource;
		}

		public async Task<RunSummary> RunAsync()
		{
			RunSummary summary = new RunSummary();
			if (!MessageBus.QueueMessage(new TestCaseStarting(TestCase)))
			{
				CancellationTokenSource.Cancel();
			}
			else
			{
				try
				{
					summary.Aggregate(await RunTestAsync());

					Aggregator.Clear();

					if (Aggregator.HasExceptions)
						if (!MessageBus.QueueMessage(new TestCaseCleanupFailure(TestCase, Aggregator.ToException())))
							CancellationTokenSource.Cancel();
				}
				finally
				{
					if (!MessageBus.QueueMessage(new TestCaseFinished(TestCase, summary.Time, summary.Total, summary.Failed, summary.Skipped)))
						CancellationTokenSource.Cancel();
				}
			}

			return summary;
		}

		private async Task<RunSummary> RunTestAsync()
		{
			var runSummary = new RunSummary { Total = 1 };
			var output = string.Empty;

			if (!MessageBus.QueueMessage(new TestStarting(TestCase)))
				CancellationTokenSource.Cancel();
			else
			{
				//AfterTestStarting();

				if (!string.IsNullOrEmpty(TestCase.SkipReason))
				{
					runSummary.Skipped++;

					if (!MessageBus.QueueMessage(new TestSkipped(TestCase, TestCase.SkipReason)))
						CancellationTokenSource.Cancel();
				}
				else
				{
					var aggregator = new ExceptionAggregator(Aggregator);

					if (!aggregator.HasExceptions)
					{
						var tuple = await aggregator.RunAsync(() => InvokeTestAsync(aggregator));
						if (tuple != null)
						{
							runSummary.Time = tuple.Item1;
							output = tuple.Item2;
						}
					}

					var exception = aggregator.ToException();
					TestResultMessage testResult;

					if (exception is null)
						testResult = new TestPassed(TestCase, runSummary.Time, output);
					else
					{
						testResult = new TestFailed(TestCase, runSummary.Time, output, exception);
						runSummary.Failed++;
					}

					if (!CancellationTokenSource.IsCancellationRequested)
						if (!MessageBus.QueueMessage(testResult))
							CancellationTokenSource.Cancel();
				}

				Aggregator.Clear();
				//BeforeTestFinished();

				if (Aggregator.HasExceptions)
					if (!MessageBus.QueueMessage(new TestCleanupFailure(TestCase, Aggregator.ToException())))
						CancellationTokenSource.Cancel();

				if (!MessageBus.QueueMessage(new TestFinished(TestCase, runSummary.Time, output)))
					CancellationTokenSource.Cancel();
			}

			return runSummary;
		}

		private async Task<Tuple<decimal, string>> InvokeTestAsync(ExceptionAggregator aggregator)
		{
			var output = string.Empty;

			TestOutputHelper? testOutputHelper = null;
			foreach (object obj in ConstructorArguments)
			{
				testOutputHelper = obj as TestOutputHelper;
				if (testOutputHelper != null)
					break;
			}

			if (testOutputHelper is null)
				testOutputHelper = new TestOutputHelper();

			testOutputHelper.Initialize(MessageBus, TestCase);

			var executionTime = await TestInvokerRunAsync(aggregator, testOutputHelper);

			output = testOutputHelper.Output;
			testOutputHelper.Uninitialize();

			return Tuple.Create(executionTime, output);
		}

		/// <summary>
		/// Creates the test class (if necessary), and invokes the test method.
		/// </summary>
		/// <returns>Returns the time (in seconds) spent creating the test class, running
		/// the test, and disposing of the test class.</returns>
		private Task<decimal> TestInvokerRunAsync(ExceptionAggregator aggregator, TestOutputHelper testOutputHelper)
		{
			return aggregator.RunAsync(async () =>
			{
				if (!CancellationTokenSource.IsCancellationRequested)
				{
					var test = CreateTestClass();

					if (!CancellationTokenSource.IsCancellationRequested)
					{
						try
						{
							test.Services.AddSingleton<ITestOutputHelper>(testOutputHelper);
							await aggregator.RunAsync(() => Timer.AggregateAsync(async () => await test.RunTest()));
						}
						finally
						{
							aggregator.Run(() => DisposeTestClass(test));
						}
					}
				}

				return Timer.Total;
			});
		}

		private RazorTest CreateTestClass()
		{
			RazorTest result = default!;

			if (!MessageBus.QueueMessage(new TestClassConstructionStarting(TestCase)))
				CancellationTokenSource.Cancel();
			else
			{
				try
				{
					if (!CancellationTokenSource.IsCancellationRequested)
					{
						Timer.Aggregate(async () =>
						{

							var tests = await RazorRenderer.GetRazorTestsFromComponent(TestCase.TestMethod.TestClass.Class.ToRuntimeType());
							// TODO: handle case where test index doesnt match, verify other parts of test perhaps?
							result = tests[TestCase.TestIndex];
						});
					}
				}
				finally
				{
					if (!MessageBus.QueueMessage(new TestClassConstructionFinished(TestCase)))
						CancellationTokenSource.Cancel();
				}
			}

			// TODO: Figure out how to handle null case properly 
			return result!;
		}

		private void DisposeTestClass(RazorTest test)
		{
			if (!MessageBus.QueueMessage(new TestClassDisposeStarting(TestCase)))
				CancellationTokenSource.Cancel();
			else
			{
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
}
