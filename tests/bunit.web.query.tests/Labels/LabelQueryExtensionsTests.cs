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

	[Theory(DisplayName = "Should return back element associated with label when element uses aria-label")]
	[MemberData(nameof(HtmlElementsThatCanHaveALabel))]
	public void Test005(string htmlElementWithLabel)
	{
		var cut = RenderComponent<LabelQueryComponent>();

		var input = cut.FindByLabelText($"{htmlElementWithLabel} Aria Label");

		input.ShouldNotBeNull();
		input.NodeName.ShouldBe(htmlElementWithLabel, StringCompareShould.IgnoreCase);
		input.Id.ShouldBe($"{htmlElementWithLabel}-with-aria-label");
	}

	[Theory(DisplayName = "Should return back element associated with another element when that other element uses aria-labelledby")]
	[MemberData(nameof(HtmlElementsThatCanHaveALabel))]
	public void Test006(string htmlElementWithLabel)
	{
		var cut = RenderComponent<LabelQueryComponent>();

		var input = cut.FindByLabelText($"{htmlElementWithLabel} Aria Labelled By");

		input.ShouldNotBeNull();
		input.NodeName.ShouldBe(htmlElementWithLabel, StringCompareShould.IgnoreCase);
		input.GetAttribute("aria-labelledby").ShouldBe($"{htmlElementWithLabel}-with-aria-labelledby");
	}
}
