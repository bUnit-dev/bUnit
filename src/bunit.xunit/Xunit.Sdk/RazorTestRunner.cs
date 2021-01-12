using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace Xunit.Sdk
{
	internal sealed class RazorTestRunner : XunitTestRunner
	{
		private TestOutputHelper? testOutputHelper;

		public RazorTestRunner(ITest test, IMessageBus messageBus, Type testClass, object[] constructorArguments, MethodInfo testMethod, object[] testMethodArguments, string skipReason, IReadOnlyList<BeforeAfterTestAttribute> beforeAfterAttributes, ExceptionAggregator aggregator, CancellationTokenSource cancellationTokenSource)
			: base(test, messageBus, testClass, constructorArguments, testMethod, testMethodArguments, skipReason, beforeAfterAttributes, aggregator, cancellationTokenSource)
		{
		}

		protected override async Task<Tuple<decimal, string>> InvokeTestAsync(ExceptionAggregator aggregator)
		{
			var executionTime = await InvokeTestMethodAsync(aggregator).ConfigureAwait(false);
			var output = GetTestOutput();
			return Tuple.Create(executionTime, output);
		}

		protected override Task<decimal> InvokeTestMethodAsync(ExceptionAggregator aggregator)
		{
			var invoker = new RazorTestInvoker(CreateTestOutputHelper, Test, MessageBus, TestClass, ConstructorArguments, TestMethod, TestMethodArguments, BeforeAfterAttributes, aggregator, CancellationTokenSource);
			return invoker.RunAsync();
		}

		private string GetTestOutput()
		{
			string result = string.Empty;

			if (testOutputHelper is not null)
			{
				result = testOutputHelper.Output;
				testOutputHelper.Uninitialize();
			}

			return result;
		}

		private ITestOutputHelper CreateTestOutputHelper()
		{
			if (testOutputHelper is null)
			{
				testOutputHelper = new TestOutputHelper();
				testOutputHelper.Initialize(MessageBus, Test);
			}

			return testOutputHelper;
		}
	}
}
