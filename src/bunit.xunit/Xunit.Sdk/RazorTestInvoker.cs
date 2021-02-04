using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using Bunit.RazorTesting;
using Bunit.Rendering;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Xunit.Sdk
{
	internal sealed class RazorTestInvoker : XunitTestInvoker
	{
		private readonly Func<ITestOutputHelper> testOutputHelperFactory;

		public RazorTestInvoker(Func<ITestOutputHelper> testOutputHelperFactory, ITest test, IMessageBus messageBus, Type testClass, object[] constructorArguments, MethodInfo testMethod, object[] testMethodArguments, IReadOnlyList<BeforeAfterTestAttribute> beforeAfterAttributes, ExceptionAggregator aggregator, CancellationTokenSource cancellationTokenSource)
			: base(test, messageBus, testClass, constructorArguments, testMethod, testMethodArguments, beforeAfterAttributes, aggregator, cancellationTokenSource)
		{
			this.testOutputHelperFactory = testOutputHelperFactory;
		}

		protected override object CreateTestClass()
		{
			if (!(Test is RazorTest razorTest))
				throw new InvalidOperationException($"The type of {nameof(Test)} is not a {nameof(RazorTest)}. Cannot continue. #1");

			using var razorRenderer = new TestComponentRenderer();
			var tests = razorRenderer.GetRazorTestsFromComponent(TestClass);

			if (tests.Count < razorTest.TestNumber)
				throw new InvalidOperationException($"The razor test '{Test.DisplayName}' was not found in the test component '{TestClass.FullName}'. ");

			var test = tests[razorTest.TestNumber - 1];

			return test;
		}

		protected override object CallTestMethod(object testClassInstance)
		{
			if (testClassInstance is RazorTestBase test)
			{
				RegisterXunitHelpersInTest(test);
				return test.RunTestAsync();
			}

			throw new InvalidOperationException($"The type of {nameof(testClassInstance)} is not an {typeof(Bunit.RazorTesting.RazorTestBase).FullName}. Cannot continue. #2");
		}

		private void RegisterXunitHelpersInTest(RazorTestBase test)
		{
			test.Services.AddSingleton<ITestOutputHelper>(_ => testOutputHelperFactory());
		}
	}
}
