namespace Bunit;

public class KeyboardEventDispatchExtensionsTest : EventDispatchExtensionsTest<KeyboardEventArgs>
{
	public static IEnumerable<object[]> Helpers { get; } = GetEventHelperMethods(typeof(KeyboardEventDispatchExtensions), x => x.GetParameters().All(p => p.ParameterType != typeof(Key)));

	protected override string ElementName => "input";

	[Theory(DisplayName = "Keyboard events are raised correctly through helpers")]
	[MemberData(nameof(Helpers))]
	public async Task CanRaiseEvents(MethodInfo helper)
	{
		var expected = new KeyboardEventArgs()
		{
			AltKey = true,
			CtrlKey = true,
			Code = "X",
			Key = "B",
			Location = 42F,
			MetaKey = true,
			Repeat = true,
			ShiftKey = true,
			Type = "ASDF",
		};

		await VerifyEventRaisesCorrectly(helper, expected);
	}

	[Fact(DisplayName = "KeyDown event is raised correctly through helper using special key")]
	public async Task CanRaiseKeyDownWithCtrlEnter()
	{
		var spy = CreateTriggerSpy(ElementName, "onkeydown");
		await spy.Trigger(element =>
		{
			element.KeyDown(Key.Enter + Key.Control);
		});

		var expected = new KeyboardEventArgs
		{
			Key = "Enter",
			Code = "Enter",
			CtrlKey = true,
		};
		spy.RaisedEvent.ShouldBeEquivalentTo(expected);
	}

	[Fact(DisplayName = "KeyDown event is raised correctly through helper using character key")]
	public async Task CanRaiseKeyDownWithAKey()
	{
		var spy = CreateTriggerSpy(ElementName, "onkeydown");
		await spy.Trigger(element =>
		{
			element.KeyDown('A');
		});

		var expected = new KeyboardEventArgs
		{
			Key = "A",
			Code = "A",
		};
		spy.RaisedEvent.ShouldBeEquivalentTo(expected);
	}

	[Fact(DisplayName = "KeyDown event is raised correctly through helper specifying repeat and type")]
	public async Task CanRaiseKeyDownWithRepeatAndType()
	{
		var spy = CreateTriggerSpy(ElementName, "onkeydown");
		await spy.Trigger(element =>
		{
			element.KeyDown(Key.Up + Key.Command, true, "Test Down");
		});

		var expected = new KeyboardEventArgs
		{
			Key = "ArrowUp",
			Code = "ArrowUp",
			MetaKey = true,
			Repeat = true,
			Type = "Test Down",
		};
		spy.RaisedEvent.ShouldBeEquivalentTo(expected);
	}

	[Fact(DisplayName = "KeyUp event is raised correctly through helper using special key")]
	public async Task CanRaiseKeyUpWithShiftSpace()
	{
		var spy = CreateTriggerSpy(ElementName, "onkeyup");
		await spy.Trigger(element =>
		{
			element.KeyUp(Key.Space + Key.Shift + Key.Alt);
		});

		var expected = new KeyboardEventArgs
		{
			Key = " ",
			Code = "Space",
			ShiftKey = true,
			AltKey = true,
		};
		spy.RaisedEvent.ShouldBeEquivalentTo(expected);
	}

	[Fact(DisplayName = "KeyUp event is raised correctly through helper using character key")]
	public async Task CanRaiseKeyUpWithBKey()
	{
		var spy = CreateTriggerSpy(ElementName, "onkeyup");
		await spy.Trigger(element =>
		{
			element.KeyUp(Key.Alt + 'B');
		});

		var expected = new KeyboardEventArgs
		{
			Key = "B",
			Code = "B",
			AltKey = true,
		};
		spy.RaisedEvent.ShouldBeEquivalentTo(expected);
	}

	[Fact(DisplayName = "KeyUp event is raised correctly through helper specifying repeat and type")]
	public async Task CanRaiseKeyUpWithWithRepeatAndType()
	{
		var spy = CreateTriggerSpy(ElementName, "onkeyup");
		await spy.Trigger(element =>
		{
			element.KeyUp(Key.Down + Key.Shift + Key.Command, true, "Test Up");
		});

		var expected = new KeyboardEventArgs
		{
			Key = "ArrowDown",
			Code = "ArrowDown",
			ShiftKey = true,
			MetaKey = true,
			Repeat = true,
			Type = "Test Up",
		};
		spy.RaisedEvent.ShouldBeEquivalentTo(expected);
	}

	[Fact(DisplayName = "KeyPress event is raised correctly through helper using special key")]
	public async Task CanRaiseKeyPressWithNum8Key()
	{
		var spy = CreateTriggerSpy(ElementName, "onkeypress");
		await spy.Trigger(element =>
		{
			element.KeyPress(Key.NumberPad8);
		});

		var expected = new KeyboardEventArgs
		{
			Key = "8",
			Code = "Numpad8",
		};
		spy.RaisedEvent.ShouldBeEquivalentTo(expected);
	}

	[Fact(DisplayName = "KeyPress event is raised correctly through helper using character key")]
	public async Task CanRaiseKeyPressWith8Key()
	{
		var spy = CreateTriggerSpy(ElementName, "onkeypress");
		await spy.Trigger(element =>
		{
			element.KeyPress('8');
		});

		var expected = new KeyboardEventArgs
		{
			Key = "8",
			Code = "8",
		};
		spy.RaisedEvent.ShouldBeEquivalentTo(expected);
	}

	[Fact(DisplayName = "KeyPress event is raised correctly through  specifying repeat and type")]
	public async Task CanRaiseKeyPressWithRepeatAndType()
	{
		var spy = CreateTriggerSpy(ElementName, "onkeypress");
		await spy.Trigger(element =>
		{
			element.KeyPress(Key.Shift + "P", true, "Press Test");
		});

		var expected = new KeyboardEventArgs
		{
			Key = "P",
			Code = "P",
			ShiftKey = true,
			Repeat = true,
			Type = "Press Test",
		};
		spy.RaisedEvent.ShouldBeEquivalentTo(expected);
	}
}
