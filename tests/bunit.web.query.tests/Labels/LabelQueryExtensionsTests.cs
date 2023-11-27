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

	[Theory(DisplayName = "Should return back associated element with label when using the for attribute with the correct casing")]
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

	[Theory(DisplayName = "Should return back element associated with label when label when is wrapped around element with the correct casing")]
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

	[Theory(DisplayName = "Should return back element associated with label when element uses aria-label with the correct casing")]
	[MemberData(nameof(HtmlElementsThatCanHaveALabel))]
	public void Test005(string htmlElementWithLabel)
	{
		var cut = RenderComponent<LabelQueryComponent>();

		var input = cut.FindByLabelText($"{htmlElementWithLabel} Aria Label");

		input.ShouldNotBeNull();
		input.NodeName.ShouldBe(htmlElementWithLabel, StringCompareShould.IgnoreCase);
		input.Id.ShouldBe($"{htmlElementWithLabel}-with-aria-label");
	}

	[Theory(DisplayName = "Should return back element associated with another element that uses aria-labelledby with the correct casing")]
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

	[Theory(DisplayName = "Should throw LabelNotFoundException when ComparisonType is case sensitive and incorrect casing is used with for attribute")]
	[InlineData(StringComparison.Ordinal)]
	[InlineData(StringComparison.InvariantCulture)]
	[InlineData(StringComparison.CurrentCulture)]
	public void Test009(StringComparison comparison)
	{
		var labelTextOptions = new ByLabelTextOptions { ComparisonType = comparison };
		var expectedLabelText = "LABEL TEXT";
		var cut = RenderComponent<Wrapper>(ps => 
			ps.AddChildContent("""<label for="input-with-label">Label Text</label><input id="input-with-label" />"""));

		Should.Throw<LabelNotFoundException>(() => cut.FindByLabelText(expectedLabelText, labelTextOptions))
			.LabelText.ShouldBe(expectedLabelText);
	}

	[Theory(DisplayName = "Should return back element associated with label when ComparisonType is case insensitive and incorrect casing is used with for attribute")]
	[InlineData(StringComparison.OrdinalIgnoreCase)]
	[InlineData(StringComparison.InvariantCultureIgnoreCase)]
	[InlineData(StringComparison.CurrentCultureIgnoreCase)]
	public void Test010(StringComparison comparison)
	{
		var labelTextOptions = new ByLabelTextOptions { ComparisonType = comparison };
		var expectedLabelText = "LABEL TEXT";
		var cut = RenderComponent<Wrapper>(ps => 
			ps.AddChildContent("""<label for="input-1">Label Text</label><input id="input-1" />"""));

		var input = cut.FindByLabelText(expectedLabelText, labelTextOptions);

		input.ShouldNotBeNull();
		input.NodeName.ShouldBe("INPUT");
		input.Id.ShouldBe("input-1");
	}

	[Theory(DisplayName = "Should throw LabelNotFoundException when ComparisonType is case sensitive and incorrect casing is used with wrapped label")]
	[InlineData(StringComparison.Ordinal)]
	[InlineData(StringComparison.InvariantCulture)]
	[InlineData(StringComparison.CurrentCulture)]
	public void Test011(StringComparison comparison)
	{
		var labelTextOptions = new ByLabelTextOptions { ComparisonType = comparison };
		var expectedLabelText = "LABEL TEXT";
		var cut = RenderComponent<Wrapper>(ps => 
			ps.AddChildContent("""<label>Label Text<input id="input-wrapped-label" /></label>"""));

		Should.Throw<LabelNotFoundException>(() => cut.FindByLabelText(expectedLabelText, labelTextOptions))
			.LabelText.ShouldBe(expectedLabelText);
	}

	[Theory(DisplayName = "Should return back element associated with label when ComparisonType is case insensitive and incorrect casing is used with wrapped label")]
	[InlineData(StringComparison.OrdinalIgnoreCase)]
	[InlineData(StringComparison.InvariantCultureIgnoreCase)]
	[InlineData(StringComparison.CurrentCultureIgnoreCase)]
	public void Test012(StringComparison comparison)
	{
		var labelTextOptions = new ByLabelTextOptions { ComparisonType = comparison };
		var expectedLabelText = "LABEL TEXT";
		var cut = RenderComponent<Wrapper>(ps => 
			ps.AddChildContent("""<label>Label Text<input id="input-1" /></label>"""));

		var input = cut.FindByLabelText(expectedLabelText, labelTextOptions);

		input.ShouldNotBeNull();
		input.NodeName.ShouldBe("INPUT");
		input.Id.ShouldBe("input-1");
	}

	[Theory(DisplayName = "Should throw LabelNotFoundException when ComparisonType is case sensitive and incorrect casing is used with aria-label")]
	[InlineData(StringComparison.Ordinal)]
	[InlineData(StringComparison.InvariantCulture)]
	[InlineData(StringComparison.CurrentCulture)]
	public void Test013(StringComparison comparison)
	{
		var labelTextOptions = new ByLabelTextOptions { ComparisonType = comparison };
		var expectedLabelText = "LABEL TEXT";
		var cut = RenderComponent<Wrapper>(ps => 
			ps.AddChildContent("""<input id="input-1" aria-label="Label Text" />"""));

		Should.Throw<LabelNotFoundException>(() => cut.FindByLabelText(expectedLabelText, labelTextOptions))
			.LabelText.ShouldBe(expectedLabelText);
	}

	[Theory(DisplayName = "Should return back element associated with label when ComparisonType is case insensitive and incorrect casing is used with aria-label")]
	[InlineData(StringComparison.OrdinalIgnoreCase)]
	[InlineData(StringComparison.InvariantCultureIgnoreCase)]
	[InlineData(StringComparison.CurrentCultureIgnoreCase)]
	public void Test014(StringComparison comparison)
	{
		var labelTextOptions = new ByLabelTextOptions { ComparisonType = comparison };
		var expectedLabelText = "LABEL TEXT";
		var cut = RenderComponent<Wrapper>(ps => 
			ps.AddChildContent("""<input id="input-1" aria-label="Label Text" />"""));

		var input = cut.FindByLabelText(expectedLabelText, labelTextOptions);

		input.ShouldNotBeNull();
		input.NodeName.ShouldBe("INPUT");
		input.Id.ShouldBe("input-1");
	}

	[Theory(DisplayName = "Should throw LabelNotFoundException when ComparisonType is case insensitive and incorrect casing is used with aria-labelledby")]
	[InlineData(StringComparison.Ordinal)]
	[InlineData(StringComparison.InvariantCulture)]
	[InlineData(StringComparison.CurrentCulture)]
	public void Test015(StringComparison comparison)
	{
		var labelTextOptions = new ByLabelTextOptions { ComparisonType = comparison };
		var expectedLabelText = "LABEL TEXT";
		var cut = RenderComponent<Wrapper>(ps => 
			ps.AddChildContent("""<h2 id="heading-1">Label Text</h2><input aria-labelledby="heading-1" />"""));

		Should.Throw<LabelNotFoundException>(() => cut.FindByLabelText(expectedLabelText, labelTextOptions))
			.LabelText.ShouldBe(expectedLabelText);
	}

	[Theory(DisplayName = "Should return back element associated with label when  ComparisonType is case insensitive and incorrect casing is used with aria-labelledby")]
	[InlineData(StringComparison.OrdinalIgnoreCase)]
	[InlineData(StringComparison.InvariantCultureIgnoreCase)]
	[InlineData(StringComparison.CurrentCultureIgnoreCase)]
	public void Test016(StringComparison comparison)
	{
		var labelTextOptions = new ByLabelTextOptions { ComparisonType = comparison };
		var expectedLabelText = "LABEL TEXT";
		var cut = RenderComponent<Wrapper>(ps => 
			ps.AddChildContent("""<h2 id="heading-1">Label Text</h2><input id="input-1" aria-labelledby="heading-1" />"""));

		var input = cut.FindByLabelText(expectedLabelText, labelTextOptions);

		input.ShouldNotBeNull();
		input.NodeName.ShouldBe("INPUT");
		input.Id.ShouldBe("input-1");
	}
}
