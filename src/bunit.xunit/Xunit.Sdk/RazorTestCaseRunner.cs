using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using Xunit.Abstractions;

namespace Xunit.Sdk
{
	internal sealed class RazorTestCaseRunner : XunitTestCaseRunner
	{
		private readonly RazorTestCase razorTestCase;

		public RazorTestCaseRunner(RazorTestCase testCase, string displayName, string? skipReason, object[] constructorArguments, object[] testMethodArguments, IMessageBus messageBus, ExceptionAggregator aggregator, CancellationTokenSource cancellationTokenSource)
			: base(testCase, displayName, skipReason, constructorArguments, testMethodArguments, messageBus, aggregator, cancellationTokenSource)
		{
			razorTestCase = testCase;
		}

		protected override ITest CreateTest(IXunitTestCase testCase, string displayName)
		{
			var test = new RazorTest(razorTestCase, displayName);
			return test;
		}

		protected override XunitTestRunner CreateTestRunner(ITest test, IMessageBus messageBus, Type testClass, object[] constructorArguments, MethodInfo testMethod, object[] testMethodArguments, string skipReason, IReadOnlyList<BeforeAfterTestAttribute> beforeAfterAttributes, ExceptionAggregator aggregator, CancellationTokenSource cancellationTokenSource)
			=> new RazorTestRunner(test, messageBus, testClass, constructorArguments, testMethod, testMethodArguments, skipReason, beforeAfterAttributes, new ExceptionAggregator(aggregator), cancellationTokenSource);
	}
}
