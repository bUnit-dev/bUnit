using System;
using System.Collections.Generic;
using Bunit;
using Bunit.RazorTesting;
using Bunit.Rendering;
using Xunit.Abstractions;

namespace Xunit.Sdk
{
	internal class RazorTestDiscoverer : IXunitTestCaseDiscoverer
	{
		private IMessageSink DiagnosticMessageSink { get; }

		public RazorTestDiscoverer(IMessageSink diagnosticMessageSink)
		{
			DiagnosticMessageSink = diagnosticMessageSink;
		}

		/// <inheritdoc/>
		public IEnumerable<IXunitTestCase> Discover(ITestFrameworkDiscoveryOptions discoveryOptions, ITestMethod testMethod, IAttributeInfo theoryAttribute)
		{
			try
			{
				var razorTestComponentType = testMethod.TestClass.Class.ToRuntimeType();
				return DiscoverRazorTests(razorTestComponentType, testMethod);
			}
			catch (Exception ex)
			{
				return new[] { new ExecutionErrorTestCase(DiagnosticMessageSink, discoveryOptions.MethodDisplayOrDefault(), discoveryOptions.MethodDisplayOptionsOrDefault(), testMethod, $"Exception thrown during razor test discovery on '{testMethod.TestClass.Class.Name}'.{Environment.NewLine}{ex.Message}") };
			}
		}

		private IEnumerable<IXunitTestCase> DiscoverRazorTests(Type razorTestComponentType, ITestMethod testMethod)
		{
			using var razorRenderer = new TestComponentRenderer();			
			var tests = razorRenderer.GetRazorTestsFromComponent(razorTestComponentType).GetAwaiter().GetResult();

			var result = tests.Count == 0
				? Array.Empty<IXunitTestCase>()
				: new IXunitTestCase[tests.Count];

			for (int i = 0; i < tests.Count; i++)
			{
				var test = tests[i];

				// TODO: Find ISourceInformation for test and provide to RazorTestCase
				result[i] = new RazorTestCase(GetDisplayName(test, i), test.Timeout, test.Skip, i, testMethod);
			}

			return result;
		}

		private string GetDisplayName(RazorTestBase test, int index) => test.Description ?? $"{test.GetType().Name} #{index + 1}";

	}
}
