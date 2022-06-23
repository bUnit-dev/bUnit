namespace Bunit;

public class ClipboardEventDispatchExtensionsTest : EventDispatchExtensionsTest<ClipboardEventArgs>
{
	protected override string ElementName => "textarea";

	[Theory(DisplayName = "Clipboard events are raised correctly through helpers")]
	[MemberData(nameof(GetEventHelperMethods), typeof(ClipboardEventDispatchExtensions))]
	public async Task CanRaiseEvents(MethodInfo helper)
	{
		var expected = new ClipboardEventArgs()
		{
			Type = "SOME TYPE",
		};

		await VerifyEventRaisesCorrectly(helper, expected);
	}
}
