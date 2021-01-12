using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Bunit.RazorTesting;
using Bunit.Rendering;
using Xunit.Abstractions;

namespace Xunit.Sdk
{
	internal sealed class RazorTestDiscoverer : IXunitTestCaseDiscoverer, IDisposable
	{
		private readonly RazorTestSourceInformationProvider sourceInfoDiscoverer;

		private IMessageSink DiagnosticMessageSink { get; }

		public RazorTestDiscoverer(IMessageSink diagnosticMessageSink)
		{
			DiagnosticMessageSink = diagnosticMessageSink;
			sourceInfoDiscoverer = new RazorTestSourceInformationProvider(diagnosticMessageSink);
		}

		/// <inheritdoc/>
		[SuppressMessage("Design", "CA1031:Do not catch general exception types")]
		public IEnumerable<IXunitTestCase> Discover(ITestFrameworkDiscoveryOptions discoveryOptions, ITestMethod testMethod, IAttributeInfo factAttribute)
		{
			try
			{
				var razorTestComponentType = testMethod.TestClass.Class.ToRuntimeType();
				return DiscoverRazorTests(razorTestComponentType, testMethod);
			}
			catch (Exception ex)
			{
				return new[]
				{
					new ExecutionErrorTestCase(
						DiagnosticMessageSink,
						discoveryOptions.MethodDisplayOrDefault(),
						discoveryOptions.MethodDisplayOptionsOrDefault(),
						testMethod,
						$"Exception thrown during razor test discovery on '{testMethod.TestClass.Class.Name}'.{Environment.NewLine}{ex.Message}"),
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
				var sourceInfo = sourceInfoDiscoverer.GetSourceInformation(testComponent, test, testNumber);
				result[index] = new RazorTestCase(GetDisplayName(test, testNumber), test.Timeout, test.Skip, testNumber, testMethod, sourceInfo);
			}

			return result;
		}

		private static string GetDisplayName(RazorTestBase test, int testNumber) => test.DisplayName ?? $"{test.GetType().Name} #{testNumber}";

		public void Dispose() => sourceInfoDiscoverer?.Dispose();
	}
}
