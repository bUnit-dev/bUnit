using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Xunit.Sdk
{
	internal class RazorTestCaseRunner : TestCaseRunner<RazorTestCase>
	{
		public RazorTestCaseRunner(RazorTestCase testCase, IMessageBus messageBus, ExceptionAggregator aggregator, CancellationTokenSource cancellationTokenSource) : base(testCase, messageBus, aggregator, cancellationTokenSource)
		{
		}

		protected override async Task<RunSummary> RunTestAsync()
		{
			var runSummary = new RunSummary { Total = 1 };
			var output = string.Empty;

			if (!MessageBus.QueueMessage(new TestStarting(TestCase)))
				CancellationTokenSource.Cancel();
			else
			{
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
						if (tuple is { })
						{
							runSummary.Time = tuple.Time;
							output = tuple.Output;
						}
					}

					var exception = aggregator.ToException();
					TestResultMessage testResult;

					if (exception == null)
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

				if (Aggregator.HasExceptions)
					if (!MessageBus.QueueMessage(new TestCleanupFailure(TestCase, Aggregator.ToException())))
						CancellationTokenSource.Cancel();

				if (!MessageBus.QueueMessage(new TestFinished(TestCase, runSummary.Time, output)))
					CancellationTokenSource.Cancel();
			}

			return runSummary;
		}

		/// <summary>
		/// Override this method to invoke the test.
		/// </summary>
		/// <param name="aggregator">The exception aggregator used to run code and collect exceptions.</param>
		/// <returns>Returns a tuple which includes the execution time (in seconds) spent running the
		/// test method, and any output that was returned by the test.</returns>
		protected async Task<(decimal Time, string Output)> InvokeTestAsync(ExceptionAggregator aggregator)
		{
			var output = string.Empty;

			var executionTime = 0;

			return (executionTime, output);
		}
	}

	internal class RazorTestInvoker : TestInvoker<RazorTestCase>
	{
		public RazorTestInvoker(ITest test, IMessageBus messageBus, Type testClass, object[] constructorArguments, MethodInfo testMethod, object[] testMethodArguments, ExceptionAggregator aggregator, CancellationTokenSource cancellationTokenSource) : base(test, messageBus, testClass, constructorArguments, testMethod, testMethodArguments, aggregator, cancellationTokenSource)
		{
		}
	}
}
