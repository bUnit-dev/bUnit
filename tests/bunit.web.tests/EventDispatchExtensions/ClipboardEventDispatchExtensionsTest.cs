namespace Bunit;

public class ClipboardEventDispatchExtensionsTest : EventDispatchExtensionsTest<ClipboardEventArgs>
{
	protected override string ElementName => "textarea";

	[UITheory(DisplayName = "Clipboard events are raised correctly through helpers")]
	[MemberData(nameof(GetEventHelperMethods), typeof(ClipboardEventDispatchExtensions))]
	public void CanRaiseEvents(MethodInfo helper)
	{
		var expected = new ClipboardEventArgs
		{
			Type = "SOME TYPE",
		};

		VerifyEventRaisesCorrectly(helper, expected);
	}
}
