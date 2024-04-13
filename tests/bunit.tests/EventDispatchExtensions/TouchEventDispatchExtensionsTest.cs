namespace Bunit;

public class TouchEventDispatchExtensionsTest : EventDispatchExtensionsTest<TouchEventArgs>
{
	public TouchEventDispatchExtensionsTest(ITestOutputHelper outputHelper) : base(outputHelper)
	{
	}

	protected override string ElementName => "p";

	[Theory(DisplayName = "Touch events are raised correctly through helpers")]
	[MemberData(nameof(GetEventHelperMethods), typeof(TouchEventDispatchExtensions))]
	public void CanRaiseEvents(MethodInfo helper)
	{
		var expected = new TouchEventArgs
		{
			AltKey = true,
			ChangedTouches = Array.Empty<TouchPoint>(),
			CtrlKey = true,
			Detail = 13L,
			MetaKey = true,
			ShiftKey = true,
			TargetTouches = Array.Empty<TouchPoint>(),
			Touches = Array.Empty<TouchPoint>(),
			Type = "TOUCH",
		};

		VerifyEventRaisesCorrectly(helper, expected);
	}
}
