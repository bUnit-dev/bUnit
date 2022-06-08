using Bunit.Rendering;
using Bunit.TestAssets.SampleComponents.DynamicComponents;
using System.Text.RegularExpressions;
using Xunit.Abstractions;

namespace Bunit;

public class ReferenceCaptureIdTest : TestContext
{
	private readonly ITestOutputHelper output;

	public ReferenceCaptureIdTest(ITestOutputHelper output)
	{
		this.output = output;
	}

	[Fact(DisplayName = "Changing a DynamicComponent type should not null out the ReferenceCaptureId of other elements")]
	public void Test001()
	{
		var cut = RenderComponent<DynamicContainer>();
		output.WriteLine($"Before changing DynamicComponent: {cut.Markup}");
		// All @ref elements have a blazor:elementReference GUID
		Guid.Parse(GetRefRerenceCaptureId(cut.Markup, "h3"));
		Guid.Parse(GetRefRerenceCaptureId(cut.Markup, "button"));

		cut.Find("#button").Click();

		output.WriteLine($"After changing DynamicComponent: {cut.Markup}");
		// Issue: The @ref elements have an empty blazor:elementReference, thus this throws now:
		Guid.Parse(GetRefRerenceCaptureId(cut.Markup, "h3"));
		Guid.Parse(GetRefRerenceCaptureId(cut.Markup, "button"));
	}

	// Extract the blazor:elementReference Id string for a HTML element
	private string GetRefRerenceCaptureId(string markup, string element)
	{
		var re = new Regex($"<{element}.*blazor:elementReference=\"([^\"]+).*");
		var match = re.Match(markup);
		return match.Groups[1].Value;
	}
}
