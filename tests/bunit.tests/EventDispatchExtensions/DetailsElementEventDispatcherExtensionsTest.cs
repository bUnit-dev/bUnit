namespace Bunit.EventDispatchExtensions;

public class DetailsElementEventDispatcherExtensionsTest : TestContext
{
	[Fact(DisplayName = "Toggle raises ontoggle events")]
	public void Test200()
	{
		var cut = Render<ToggleableDetails>();
		cut.FindAll("div > p").Count.ShouldBe(0);

		cut.Find("details").Toggle();

		cut.FindAll("div > p").Count.ShouldBe(1);
	}
}
