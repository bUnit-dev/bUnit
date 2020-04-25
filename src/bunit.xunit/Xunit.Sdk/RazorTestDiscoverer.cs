using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

using Bunit.RazorTesting;
using Bunit.Rendering;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;

using Xunit.Abstractions;

namespace Xunit.Sdk
{
	/// <summary>
	/// Implementation of <see cref="IXunitTestCaseDiscoverer"/> that supports finding test cases
	/// on methods decorated with <see cref="TheoryAttribute"/>.
	/// </summary>
	public class RazorTestDiscoverer : IXunitTestCaseDiscoverer
	{
		private readonly TestComponentRenderer RazorRenderer = new TestComponentRenderer();

		/// <summary>
		/// Initializes a new instance of the <see cref="TheoryDiscoverer"/> class.
		/// </summary>
		/// <param name="diagnosticMessageSink">The message sink used to send diagnostic messages</param>
		public RazorTestDiscoverer(IMessageSink diagnosticMessageSink)
		{
			DiagnosticMessageSink = diagnosticMessageSink;
		}

		/// <summary>
		/// Gets the message sink to be used to send diagnostic messages.
		/// </summary>
		protected IMessageSink DiagnosticMessageSink { get; }

		public IEnumerable<IXunitTestCase> Discover(ITestFrameworkDiscoveryOptions discoveryOptions, ITestMethod testMethod, IAttributeInfo theoryAttribute)
		{
			try
			{
				var tests = RazorRenderer.GetRazorTestsFromComponent(testMethod.TestClass.Class.ToRuntimeType()).GetAwaiter().GetResult();

				var result = tests.Count == 0 ? Array.Empty<IXunitTestCase>() : new IXunitTestCase[tests.Count];

				for (int i = 0; i < tests.Count; i++)
				{
					result[i] = CreateRazorTestCase(tests[i], i, discoveryOptions, testMethod);
				}
				return result;
			}
			catch (Exception ex)
			{
				DiagnosticMessageSink.OnMessage(new DiagnosticMessage($"Exception thrown during theory discovery on '{testMethod.TestClass.Class.Name}'.{Environment.NewLine}{ex}"));
				return new[]{ new ExecutionErrorTestCase(DiagnosticMessageSink,
														 discoveryOptions.MethodDisplayOrDefault(),
														 discoveryOptions.MethodDisplayOptionsOrDefault(),
														 testMethod,
														 $"Exception thrown during theory discovery on '{testMethod.TestClass.Class.Name}'.{Environment.NewLine}{ex}")
				};
			}
		}

		private IXunitTestCase CreateRazorTestCase(RazorTest test, int testIndex, ITestFrameworkDiscoveryOptions discoveryOptions, ITestMethod testMethod)
		{
			return new RazorTestCase(test, testIndex, testMethod);
			//try
			//{
			//	test.Validate();
			//}
			//catch (Exception ex)
			//{
			//	return new ExecutionErrorTestCase(DiagnosticMessageSink,
			//							   discoveryOptions.MethodDisplayOrDefault(),
			//							   discoveryOptions.MethodDisplayOptionsOrDefault(),
			//							   testMethod,
			//							   $"Exception thrown during theory discovery on '{testMethod.TestClass.Class.Name}'.{Environment.NewLine}{ex}");
			//}
		}
	}
}
