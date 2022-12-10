using Bunit.TestAssets.SampleComponents;
using Shouldly;
using Xunit;

namespace Bunit.EventDispatchExtensions;

public class DetailsElementEventDispatcherExtensionsTest : TestContext
{
	[UIFact(DisplayName = "Toggle raises ontoggle events")]
	public void Test200()
	{
		var cut = RenderComponent<ToggleableDetails>();
		cut.FindAll("div > p").Count.ShouldBe(0);

		cut.Find("details").Toggle();

		cut.FindAll("div > p").Count.ShouldBe(1);
	}
}
