using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Components.Web;
using Shouldly;
using Xunit;

namespace Bunit
{
	public class KeyboardEventDispatchExtensionsTest : EventDispatchExtensionsTest<KeyboardEventArgs>
	{
		public static IEnumerable<MethodInfo[]> Helpers { get; } = GetEventHelperMethods(typeof(KeyboardEventDispatchExtensions), x => x.GetParameters().All(p => p.ParameterType != typeof(Key)));

		public static IEnumerable<object?[]> KeyTestData { get; } = new[]
		{
			new object?[]
			{
				Key.Null,
				null
			},
			new object[]
			{
				Key.Enter,
				new KeyboardEventArgs
				{
					Key = "Enter",
					Code = "Enter"
				}
			},
			new object[]
			{
				Key.Right,
				new KeyboardEventArgs
				{
					Key = "ArrowRight",
					Code = "ArrowRight"
				}
			},
			new object[]
			{
				Key.Add,
				new KeyboardEventArgs
				{
					Key = "+",
					Code = "NumpadAdd"
				}
			},
			new object[]
			{
				Key.Shift,
				new KeyboardEventArgs
				{
					Key = "Shift",
					Code = "ShiftLeft",
					ShiftKey = true
				}
			},
			new object[]
			{
				Key.Control,
				new KeyboardEventArgs
				{
					Key = "Control",
					Code = "ControlLeft",
					CtrlKey = true
				}
			},
			new object[]
			{
				Key.Alt,
				new KeyboardEventArgs
				{
					Key = "Alt",
					Code = "AltLeft",
					AltKey = true
				}
			},
			new object[]
			{
				Key.Enter | Key.Alt,
				new KeyboardEventArgs
				{
					Key = "Enter",
					Code = "Enter",
					AltKey = true
				}
			},
			new object[]
			{
				Key.Right | Key.Control,
				new KeyboardEventArgs
				{
					Key = "ArrowRight",
					Code = "ArrowRight",
					CtrlKey = true
				}
			},
			new object[]
			{
				Key.Add | Key.Shift | Key.Control,
				new KeyboardEventArgs
				{
					Key = "+",
					Code = "NumpadAdd",
					ShiftKey = true,
					CtrlKey = true
				}
			},
			new object[]
			{
				Key.Control | Key.Alt | Key.Shift,
				new KeyboardEventArgs
				{
					Key = "Shift",
					Code = "ShiftLeft",
					ShiftKey = true,
					CtrlKey = true,
					AltKey = true
				}
			}
		};

		protected override string ElementName => "input";

		[Theory(DisplayName = "Keyboard events are raised correctly through helpers")]
		[MemberData(nameof(Helpers))]
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

		[Theory(DisplayName = "KeyDown event is raised correctly through helper using special keys")]
		[MemberData(nameof(KeyTestData))]
		public void CanRaiseKeyDownWithSpecialKey(Key key, KeyboardEventArgs expected)
		{
			var spy = CreateTriggerSpy(ElementName, "onkeydown");
			spy.Trigger(element =>
			{
				element.KeyDown(key);
			});
			spy.RaisedEvent.ShouldBeEquivalentTo(expected);
		}

		[Theory(DisplayName = "KeyDown event is raised correctly through helper using special keys")]
		[MemberData(nameof(KeyTestData))]
		public void CanRaiseKeyUpWithSpecialKey(Key key, KeyboardEventArgs expected)
		{
			var spy = CreateTriggerSpy(ElementName, "onkeyup");
			spy.Trigger(element =>
			{
				element.KeyUp(key);
			});
			spy.RaisedEvent.ShouldBeEquivalentTo(expected);
		}
	}
}
