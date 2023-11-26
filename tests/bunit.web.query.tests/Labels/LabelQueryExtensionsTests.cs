using Bunit.TestAssets.BlazorE2E;

namespace Bunit.Labels;

public class LabelQueryExtensionsTests : TestContext
{
	public static TheoryData<string> HtmlElementsThatCanHaveALabel { get; } = new()
	{
		"input",
		"select",
		"button",
		"meter",
		"output",
		"progress",
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

		Should.Throw<LabelNotFoundException>(() => cut.FindByLabelText(expectedLabelText))
			.LabelText.ShouldBe(expectedLabelText);
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

		Should.Throw<LabelNotFoundException>(() => cut.FindByLabelText(expectedLabelText))
			.LabelText.ShouldBe(expectedLabelText);
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

	[Theory(DisplayName = "Should reflect latest value when element re-renders")]
	[InlineData("Re-rendered input with label")]
	[InlineData("Re-rendered input with wrapped label")]
	[InlineData("Re-rendered input With Aria Label")]
	[InlineData("Re-rendered input with Aria Labelledby")]
	public void Test007(string labelText)
	{
		var cut = RenderComponent<LabelQueryComponent>();

		var input = cut.FindByLabelText(labelText);
		input.GetAttribute("value").ShouldBe("0");

		cut.Find("#increment-button").Click();
		input.GetAttribute("value").ShouldBe("1");
	}
}
