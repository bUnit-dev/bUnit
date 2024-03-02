namespace Bunit;

public class MediaEventDispatchExtensionsTest : EventDispatchExtensionsTest<EventArgs>
{
	public MediaEventDispatchExtensionsTest(ITestOutputHelper outputHelper) : base(outputHelper)
	{
	}

	protected override string ElementName => "audio";

	[Theory(DisplayName = "Media events are raised correctly through helpers")]
	[MemberData(nameof(GetEventHelperMethods), typeof(MediaEventDispatchExtensions))]
	public void CanRaiseEvents(MethodInfo helper)
	{
		VerifyEventRaisesCorrectly(helper, EventArgs.Empty);
	}
}
