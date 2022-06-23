namespace Bunit;

public class MediaEventDispatchExtensionsTest : EventDispatchExtensionsTest<EventArgs>
{
	protected override string ElementName => "audio";

	[Theory(DisplayName = "Media events are raised correctly through helpers")]
	[MemberData(nameof(GetEventHelperMethods), typeof(MediaEventDispatchExtensions))]
	public Task CanRaiseEvents(MethodInfo helper)
	{
		return VerifyEventRaisesCorrectly(helper, EventArgs.Empty);
	}
}
