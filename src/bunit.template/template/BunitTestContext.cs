using Bunit;
#if (testFramework_nunit)
using NUnit.Framework;
#elif (testFramework_mstest)
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

namespace Company.BlazorTests1;

/// <summary>
/// Test context wrapper for bUnit.
/// Read more about using <see cref="BunitTestContext"/> <seealso href="https://bunit.dev/docs/getting-started/writing-tests.html#remove-boilerplate-code-from-tests">here</seealso>.
/// </summary>
public abstract class BunitTestContext : TestContextWrapper
{
#if (testFramework_nunit)
	[SetUp]
#elif (testFramework_mstest)
	[TestInitialize]
#endif
	public void Setup() => TestContext = new Bunit.TestContext();

#if (testFramework_nunit)
	[TearDown]
#elif (testFramework_mstest)
	[TestCleanup]
#endif
	public void TearDown() => TestContext?.Dispose();
}
