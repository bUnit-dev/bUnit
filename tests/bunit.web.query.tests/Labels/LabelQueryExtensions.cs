using Bunit.TestAssets.BlazorE2E;

namespace Bunit.Labels;

public class LabelQueryExtensionsTests : TestContext
{
	[Fact(DisplayName = "Should return back input associated with label when using the for attribute")]
	public void Test001()
	{
		var cut = RenderComponent<LabelQueryComponent>();
		
		var input = cut.FindByLabelText("Label for Input 1");
		
		input.ShouldNotBeNull();
		input.Id.ShouldBe("input-with-label");
	}

	[Fact(DisplayName = "Should throw exception when label text does not exist in the DOM")]
	public void Test002()
	{
		var expectedLabelText = Guid.NewGuid().ToString();
		var cut = RenderComponent<LabelQueryComponent>();
		
		Should.Throw<ElementNotFoundException>(() => cut.FindByLabelText(expectedLabelText))
			.CssSelector.ShouldBe(expectedLabelText);
	}

	[Fact(DisplayName = "Should return back input associated with label when label when is wrapped around input")]
	public void Test003()
	{
		var cut = RenderComponent<LabelQueryComponent>();
		
		var input = cut.FindByLabelText("Wrapped Label");
		
		input.ShouldNotBeNull();
		input.Id.ShouldBe("wrapped-label");
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

	// TODO: get aria-labelledby
}
