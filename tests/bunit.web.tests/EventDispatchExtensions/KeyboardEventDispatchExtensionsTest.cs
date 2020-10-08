using System.Reflection;
using Microsoft.AspNetCore.Components.Web;
using Shouldly;
using Xunit;

namespace Bunit
{
	public class KeyboardEventDispatchExtensionsTest : EventDispatchExtensionsTest<KeyboardEventArgs>
	{
		protected override string ElementName => "input";

		[Theory(DisplayName = "Keyboard events are raised correctly through helpers")]
		[MemberData(nameof(GetEventHelperMethods), typeof(KeyboardEventDispatchExtensions))]
		public void CanRaiseEvents(MethodInfo helper)
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
				Type = "ASDF"
			};

			VerifyEventRaisesCorrectly(helper, expected);
		}

		[Fact(DisplayName = "KeyDown event is raised correctly through helper using special key")]
		public void CanRaiseKeyDownWithCtrlEnter()
		{
			var spy = CreateTriggerSpy(ElementName, "onkeydown");
			spy.Trigger(element =>
			{
				element.KeyDown(Key.Enter + Key.Control);
			});

			var expected = new KeyboardEventArgs
			{
				Key = "Enter",
				Code = "Enter",
				CtrlKey = true
			};
			spy.RaisedEvent.ShouldBeEquivalentTo(expected);
		}

		[Fact(DisplayName = "KeyDown event is raised correctly through helper using character keys")]
		public void CanRaiseKeyUpWithAKey()
		{
			var spy = CreateTriggerSpy(ElementName, "onkeyup");
			spy.Trigger(element =>
			{
				element.KeyUp((Key)'A');
			});

			var expected = new KeyboardEventArgs
			{
				Key = "A",
				Code = "A"
			};
			spy.RaisedEvent.ShouldBeEquivalentTo(expected);
		}
	}
}
