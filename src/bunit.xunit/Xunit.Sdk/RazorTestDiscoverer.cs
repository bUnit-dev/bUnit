using System;
using System.Collections.Generic;
using Bunit;
using Bunit.RazorTesting;
using Bunit.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Xunit.Abstractions;

namespace Xunit.Sdk
{
	internal class RazorTestDiscoverer : IXunitTestCaseDiscoverer, IDisposable
	{
		private readonly RazorTestSourceInformationProvider _sourceInfoDiscoverer;

		private IMessageSink DiagnosticMessageSink { get; }

		public RazorTestDiscoverer(IMessageSink diagnosticMessageSink)
		{
			DiagnosticMessageSink = diagnosticMessageSink;
			_sourceInfoDiscoverer = new RazorTestSourceInformationProvider(diagnosticMessageSink);
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
				return new[] {
					new ExecutionErrorTestCase(
						DiagnosticMessageSink,
						discoveryOptions.MethodDisplayOrDefault(),
						discoveryOptions.MethodDisplayOptionsOrDefault(),
						testMethod,
						$"Exception thrown during razor test discovery on '{testMethod.TestClass.Class.Name}'.{Environment.NewLine}{ex.Message}")
				};
			}
		}

		private IEnumerable<IXunitTestCase> DiscoverRazorTests(Type testComponent, ITestMethod testMethod)
		{
			DiagnosticMessageSink.OnMessage(new DiagnosticMessage($"{nameof(DiscoverRazorTests)}: Discovering in {testComponent.FullName}."));

			using var razorRenderer = new TestComponentRenderer();
			var tests = razorRenderer.GetRazorTestsFromComponent(testComponent);

			var result = tests.Count == 0
				? Array.Empty<IXunitTestCase>()
				: new IXunitTestCase[tests.Count];

			for (int index = 0; index < tests.Count; index++)
			{
				var test = tests[index];
				var testNumber = index + 1;
				var sourceInfo = _sourceInfoDiscoverer.GetSourceInformation(testComponent, test, testNumber);
				result[index] = new RazorTestCase(GetDisplayName(test, testNumber), test.Timeout, test.Skip, testNumber, testMethod, sourceInfo);
			}

			return result;
		}

		private string GetDisplayName(RazorTestBase test, int testNumber) => test.Description ?? $"{test.GetType().Name} #{testNumber}";

		public void Dispose()
		{
			_sourceInfoDiscoverer?.Dispose();
		}
	}
}
