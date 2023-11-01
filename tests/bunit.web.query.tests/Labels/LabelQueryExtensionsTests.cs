using Bunit.TestAssets.BlazorE2E;

namespace Bunit.Labels;

public class LabelQueryExtensionsTests : TestContext
{
	public static IEnumerable<object[]> HtmlElementsThatCanHaveALabel =>
		new List<object[]>
		{
			new object[] { "input" },
			new object[] { "select" },
			new object[] { "button" },
			new object[] { "meter" },
			new object[] { "output" },
			new object[] { "progress" },
		};

	[Theory(DisplayName = "Should return back associated element with label when using the for attribute")]
	[MemberData(nameof(HtmlElementsThatCanHaveALabel))]
	public void Test001(string htmlElementWithLabel)
	{
		var cut = RenderComponent<LabelQueryComponent>();

		var input = cut.FindByLabelText($"Label for {htmlElementWithLabel} 1");

		input.ShouldNotBeNull();
		input.NodeName.ShouldBe(htmlElementWithLabel, StringCompareShould.IgnoreCase);
		input.Id.ShouldBe($"{htmlElementWithLabel}-with-label");
	}

	[Fact(DisplayName = "Should throw exception when label text does not exist in the DOM")]
	public void Test002()
	{
		var expectedLabelText = Guid.NewGuid().ToString();
		var cut = RenderComponent<LabelQueryComponent>();

		Should.Throw<ElementNotFoundException>(() => cut.FindByLabelText(expectedLabelText))
			.CssSelector.ShouldBe(expectedLabelText);
	}

	[Theory(DisplayName = "Should return back element associated with label when label when is wrapped around element")]
	[MemberData(nameof(HtmlElementsThatCanHaveALabel))]
	public void Test003(string htmlElementWithLabel)
	{
		var cut = RenderComponent<LabelQueryComponent>();

		var input = cut.FindByLabelText($"{htmlElementWithLabel} Wrapped Label");

		input.ShouldNotBeNull();
		input.NodeName.ShouldBe(htmlElementWithLabel, StringCompareShould.IgnoreCase);
		input.Id.ShouldBe($"{htmlElementWithLabel}-wrapped-label");
	}

	[Fact(DisplayName = "Should throw exception when label text exists but is not tied to any input")]
	public void Test004()
	{
		var expectedLabelText = "Label With Missing Input";
		var cut = RenderComponent<LabelQueryComponent>();

		Should.Throw<ElementNotFoundException>(() => cut.FindByLabelText(expectedLabelText))
			.CssSelector.ShouldBe(expectedLabelText);
	}

	[Fact(DisplayName = "Should return back input associated with label when label using the aria-label")]
	public void Test005()
	{
		var cut = RenderComponent<LabelQueryComponent>();

		var input = cut.FindByLabelText("Aria Label");

		input.ShouldNotBeNull();
		input.Id.ShouldBe("input-with-aria-label");
	}

	// Throw error that says why
	// TODO: test with button, input (except for type="hidden" ), meter, output, progress, select and textarea
	// TODO: get aria-labelledby
}
