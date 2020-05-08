using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

using Xunit.Abstractions;

namespace Xunit.Sdk
{

	internal class RazorTestRunner : XunitTestRunner
	{
		private TestOutputHelper? _testOutputHelper;

		public RazorTestRunner(ITest test, IMessageBus messageBus, Type testClass, object[] constructorArguments, MethodInfo testMethod, object[] testMethodArguments, string skipReason, IReadOnlyList<BeforeAfterTestAttribute> beforeAfterAttributes, ExceptionAggregator aggregator, CancellationTokenSource cancellationTokenSource)
			: base(test, messageBus, testClass, constructorArguments, testMethod, testMethodArguments, skipReason, beforeAfterAttributes, aggregator, cancellationTokenSource)
		{
		}

		protected override async Task<Tuple<decimal, string>> InvokeTestAsync(ExceptionAggregator aggregator)
		{
			var executionTime = await InvokeTestMethodAsync(aggregator);
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

			if (_testOutputHelper is { })
			{
				result = _testOutputHelper.Output;
				_testOutputHelper.Uninitialize();
			}

			return result;
		}

		private ITestOutputHelper CreateTestOutputHelper()
		{
			if (_testOutputHelper is null)
			{
				_testOutputHelper = new TestOutputHelper();
				_testOutputHelper.Initialize(MessageBus, Test);
			}
			return _testOutputHelper;
		}
	}
}
