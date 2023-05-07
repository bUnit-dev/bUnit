namespace Bunit;

public class MouseEventDispatchExtensionsTest : EventDispatchExtensionsTest<MouseEventArgs>
{
	public static IEnumerable<object[]> Helpers { get; }
		= GetEventHelperMethods(
			typeof(MouseEventDispatchExtensions),
			x => !x.Name.Contains("Wheel", StringComparison.OrdinalIgnoreCase)
			  && !x.Name.Contains("DoubleClick", StringComparison.OrdinalIgnoreCase));

	protected override string ElementName => "button";

	[Theory(DisplayName = "Mouse events are raised correctly through helpers")]
	[MemberData(nameof(Helpers))]
	public void CanRaiseEvents(MethodInfo helper)
	{
		var expected = new MouseEventArgs
		{
			Detail = 123,
			ScreenX = 3,
			ScreenY = 4,
			ClientX = 5,
			ClientY = 6,
			Button = 7,
			Buttons = 8,
			ShiftKey = true,
			CtrlKey = true,
			AltKey = true,
			MetaKey = true,
			Type = "TYPE",
		};

		VerifyEventRaisesCorrectly(helper, expected);
		//VerifyEventRaisesCorrectly(
		//	helper,
		//	expected,
		//	(nameof(MouseEventDispatchExtensions.DoubleClick), "ondblclick"));
	}

	[Fact(DisplayName = "Click sets MouseEventArgs.Detail to 1 by default")]
	public void Test001()
	{
		var spy = CreateTriggerSpy<MouseEventArgs>("button", "onclick");

		spy.Trigger(x => x.Click());

		spy.RaisedEvent.Detail.ShouldBe(1);
	}

	[Fact(DisplayName = "DoubleClick sets MouseEventArgs.Detail to 2 by default")]
	public void Test002()
	{
		var spy = CreateTriggerSpy<MouseEventArgs>("button", "ondblclick");

		spy.Trigger(x => x.DoubleClick());

		spy.RaisedEvent.Detail.ShouldBe(2);
	}

	[Fact(DisplayName = "DoubleClick events are raised correctly through helpers")]
	public void Test003()
	{
		var expected = new MouseEventArgs
		{
			Detail = 2,
			ScreenX = 3,
			ScreenY = 4,
			ClientX = 5,
			ClientY = 6,
			Button = 7,
			Buttons = 8,
			ShiftKey = true,
			CtrlKey = true,
			AltKey = true,
			MetaKey = true,
			Type = "TYPE",
		};
		var spy = CreateTriggerSpy<MouseEventArgs>("button", "ondblclick");

		spy.Trigger(x => x.DoubleClick(expected));

		spy.RaisedEvent.ShouldBe(expected);
	}

	[Fact(DisplayName = "DoubleClickAsync events are raised correctly through helpers")]
	public void Test004()
	{
		var expected = new MouseEventArgs
		{
			Detail = 2,
			ScreenX = 3,
			ScreenY = 4,
			ClientX = 5,
			ClientY = 6,
			Button = 7,
			Buttons = 8,
			ShiftKey = true,
			CtrlKey = true,
			AltKey = true,
			MetaKey = true,
			Type = "TYPE",
		};
		var spy = CreateTriggerSpy<MouseEventArgs>("button", "ondblclick");

		spy.Trigger(x => x.DoubleClickAsync(expected));

		spy.RaisedEvent.ShouldBe(expected);
	}
}
