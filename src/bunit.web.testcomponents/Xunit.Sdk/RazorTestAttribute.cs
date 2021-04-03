using System;
using Bunit;
using Xunit.Sdk;

namespace Xunit
{
	/// <summary>
	/// Add this attribute to an stub method in a razor test component to make
	/// it discoverable by the xUnit test framework. See <see cref="TestComponentBase"/>
	/// for an example of its usage.
	/// </summary>
	[XunitTestCaseDiscoverer("Xunit.Sdk.RazorTestDiscoverer", "Bunit.Xunit")]
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
	public sealed class RazorTestAttribute : FactAttribute { }
}
