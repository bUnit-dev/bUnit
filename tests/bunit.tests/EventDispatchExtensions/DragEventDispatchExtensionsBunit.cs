namespace Bunit;

public class DragEventDispatchExtensionsBunit : EventDispatchExtensionsBunit<DragEventArgs>
{
	public DragEventDispatchExtensionsBunit(ITestOutputHelper outputHelper) : base(outputHelper)
	{
	}

	protected override string ElementName => "textarea";

	[Theory(DisplayName = "Drag events are raised correctly through helpers")]
	[MemberData(nameof(GetEventHelperMethods), typeof(DragEventDispatchExtensions))]
	public void CanRaiseEvents(MethodInfo helper)
	{
		var expected = new DragEventArgs
		{
			Detail = 2,
			ScreenX = 3,
			ScreenY = 4,
			ClientX = 5,
			ClientY = 6,
			Button = 7,
			Buttons = 8,
			CtrlKey = true,
			AltKey = true,
			MetaKey = true,
			ShiftKey = true,
			Type = "TYPE",
			DataTransfer = new DataTransfer(),
		};

		VerifyEventRaisesCorrectly(helper, expected);
	}
}
