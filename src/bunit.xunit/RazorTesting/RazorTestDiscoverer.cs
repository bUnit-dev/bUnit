using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Bunit.Rendering;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Xunit.Sdk
{
	public class RazorTestDiscoverer : IXunitTestCaseDiscoverer
	{
		private static readonly ServiceProvider ServiceProvider = new ServiceCollection().BuildServiceProvider();
		private readonly TestComponentRenderer RazorRenderer = new TestComponentRenderer(ServiceProvider, NullLoggerFactory.Instance);

		/// <summary>
		/// The diagnostic message sink provided to the constructor.
		/// </summary>
		private readonly IMessageSink _diagnosticMessageSink;

		/// <summary>
		/// Initializes a new instance of the <see cref="RazorTestDiscoverer"/> class.
		/// </summary>
		/// <param name="diagnosticMessageSink">The message sink used to send diagnostic messages</param>
		public RazorTestDiscoverer(IMessageSink diagnosticMessageSink)
		{
			_diagnosticMessageSink = diagnosticMessageSink;
		}

		/// <inheritdoc />
		public virtual IEnumerable<IXunitTestCase> Discover(ITestFrameworkDiscoveryOptions discoveryOptions, ITestMethod testMethod, IAttributeInfo theoryAttribute)
		{
			var tests = RazorRenderer.GetRazorTestsFromComponent(testMethod.TestClass.Class.ToRuntimeType()).GetAwaiter().GetResult();

			for (var i = 0; i < tests.Count; i++)
				yield return new RazorTestCase(tests[i], i + 1, tests.Count, testMethod);
		}
	}
}
