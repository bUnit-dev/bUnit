namespace Bunit.TestIds;

public class TestIdQueryExtensionsTests : BunitContext
{
	[Fact(DisplayName = "Should find span element with matching testid value")]
	public void Test001()
	{
		var cut = Render<Wrapper>(ps => ps.AddChildContent($"""<span data-testid="myTestId"><span>"""));

		var elem = cut.FindByTestId("myTestId");

		elem.ShouldNotBeNull();
		elem.NodeName.ShouldBe("SPAN", StringCompareShould.IgnoreCase);
		elem.GetAttribute("data-testid").ShouldBe("myTestId");
	}

	[Fact(DisplayName = "Should throw exception when testid does not exist in the DOM")]
	public void Test002()
	{
		var cut = Render<Wrapper>(ps => ps.AddChildContent("""<span data-testid="testId"><span>"""));

		Should.Throw<TestIdNotFoundException>(() => cut.FindByTestId("myTestId")).TestId.ShouldBe("myTestId");
	}

	[Fact(DisplayName = "Should throw exception when testid casing is different from DOM")]
	public void Test003()
	{
		var cut = Render<Wrapper>(ps => ps.AddChildContent("""<span data-testid="testId"><span>"""));

		Should.Throw<TestIdNotFoundException>(() => cut.FindByTestId("MYTESTID")).TestId.ShouldBe("MYTESTID");
	}

	[Fact(DisplayName = "Should find first div element with matching testid value")]
	public void Test004()
	{
		var cut = Render<Wrapper>(ps => ps.AddChildContent($"""
			<div data-testid="myTestId"></div>
			<span data-testid="myTestId"><span>
			"""));

		var elem = cut.FindByTestId("myTestId");

		elem.ShouldNotBeNull();
		elem.NodeName.ShouldBe("DIV", StringCompareShould.IgnoreCase);
		elem.GetAttribute("data-testid").ShouldBe("myTestId");
	}

	[Fact(DisplayName = "Should find first non-child div element with matching testid value")]
	public void Test005()
	{
		var cut = Render<Wrapper>(ps => ps.AddChildContent($"""
			<div data-testid="myTestId">
				<span data-testid="myTestId"><span>
			</div>
			"""));

		var elem = cut.FindByTestId("myTestId");

		elem.ShouldNotBeNull();
		elem.NodeName.ShouldBe("DIV", StringCompareShould.IgnoreCase);
		elem.GetAttribute("data-testid").ShouldBe("myTestId");
	}

	[Fact(DisplayName = "Should find span element with matching testid attribute name and value")]
	public void Test006()
	{
		var cut = Render<Wrapper>(ps => ps.AddChildContent($"""<span data-testidattr="myTestId"><span>"""));

		var elem = cut.FindByTestId("myTestId", opts => opts.TestIdAttribute = "data-testidattr");

		elem.ShouldNotBeNull();
		elem.NodeName.ShouldBe("SPAN", StringCompareShould.IgnoreCase);
		elem.GetAttribute("data-testidattr").ShouldBe("myTestId");
	}

	[Fact(DisplayName = "Should find span element with equivalent case-insensitive testid value")]
	public void Test007()
	{
		var cut = Render<Wrapper>(ps => ps.AddChildContent("""<span data-testid="myTestId"><span>"""));

		var elem = cut.FindByTestId("MYTESTID", opts => opts.ComparisonType = StringComparison.OrdinalIgnoreCase);

		elem.ShouldNotBeNull();
		elem.NodeName.ShouldBe("SPAN", StringCompareShould.IgnoreCase);
		elem.GetAttribute("data-testid").ShouldBe("myTestId");
	}

	[Fact(DisplayName = "Should find span element with equivalent case-sensitive testid value")]
	public void Test008()
	{
		var cut = Render<Wrapper>(ps => ps.AddChildContent("""
			<span data-testid="myTestId"><span>
			<span data-testid="MYTESTID"><span>
			"""));

		var elem = cut.FindByTestId("MYTESTID");

		elem.ShouldNotBeNull();
		elem.NodeName.ShouldBe("SPAN", StringCompareShould.IgnoreCase);
		elem.GetAttribute("data-testid").ShouldBe("MYTESTID");
	}
}
