#if !NETCOREAPP3_1
using Bunit.TestAssets.SampleComponents;
using Shouldly;
using Xunit;

namespace Bunit.EventDispatchExtensions;

public class DetailsElementEventDispatcherExtensionsTest : TestContext
{
	[Fact(DisplayName = "Toggle raises ontoggle events")]
	public async Task Test200()
	{
		var cut = await RenderComponent<ToggleableDetails>();
		cut.FindAll("div > p").Count.ShouldBe(0);

		cut.Find("details").Toggle();

		cut.FindAll("div > p").Count.ShouldBe(1);
	}
}
#endif
