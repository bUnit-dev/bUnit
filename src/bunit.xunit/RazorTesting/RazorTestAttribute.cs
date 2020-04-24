using System;
using Xunit;
using Xunit.Sdk;

namespace Xunit.Sdk
{
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
	[XunitTestCaseDiscoverer("Xunit.Sdk." + nameof(RazorTestDiscoverer), "Bunit.Xunit")]
	public class RazorTestAttribute : FactAttribute { }
}
