using System;
using System.Collections.Generic;
using Bunit;
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
				using var razorRenderer = new TestComponentRenderer();

				var tests = razorRenderer.GetRazorTestsFromComponent(testMethod.GetTestComponentType()).GetAwaiter().GetResult();

				var result = tests.Count == 0
					? Array.Empty<IXunitTestCase>()
					: new IXunitTestCase[tests.Count];

				for (int i = 0; i < tests.Count; i++)
				{
					result[i] = new RazorTestCase(tests[i], i, testMethod);
				}

				return result;
			}
			catch (Exception ex)
			{
				return new[] { new ExecutionErrorTestCase(DiagnosticMessageSink, discoveryOptions.MethodDisplayOrDefault(), discoveryOptions.MethodDisplayOptionsOrDefault(), testMethod, $"Exception thrown during razor test discovery on '{testMethod.TestClass.Class.Name}'.{Environment.NewLine}{ex.Message}") };
			}
		}
	}
}
