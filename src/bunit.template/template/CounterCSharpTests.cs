namespace Company.BlazorTests1;

/// <summary>
/// These tests are written entirely in C#.
/// Learn more at https://bunit.dev/docs/getting-started/writing-tests.html#creating-basic-tests-in-cs-files
/// </summary>
#if (testFramework_xunit)
public class CounterCSharpTests : TestContext
#elif (testFramework_nunit)
public class CounterCSharpTests : BunitTestContext
#elif (testFramework_mstest)
[TestClass]
public class CounterCSharpTests : BunitTestContext
#endif
{
#if (testFramework_xunit)
	[Fact]
#elif (testFramework_nunit)
	[Test]
#elif (testFramework_mstest)
	[TestMethod]
#endif
	public void CounterStartsAtZero()
	{
		// Arrange
		var cut = RenderComponent<Counter>();

		// Assert that content of the paragraph shows counter at zero
		cut.Find("p").MarkupMatches("<p>Current count: 0</p>");
	}

#if (testFramework_xunit)
	[Fact]
#elif (testFramework_nunit)
	[Test]
#elif (testFramework_mstest)
	[TestMethod]
#endif
	public void ClickingButtonIncrementsCounter()
	{
		// Arrange
		var cut = RenderComponent<Counter>();

		// Act - click button to increment counter
		cut.Find("button").Click();

		// Assert that the counter was incremented
		cut.Find("p").MarkupMatches("<p>Current count: 1</p>");
	}
}
