namespace Bunit;

public class ClipboardEventDispatchExtensionsBunit : EventDispatchExtensionsBunit<ClipboardEventArgs>
{
	public ClipboardEventDispatchExtensionsBunit(ITestOutputHelper outputHelper) : base(outputHelper)
	{
	}

	protected override string ElementName => "textarea";

	[Theory(DisplayName = "Clipboard events are raised correctly through helpers")]
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
