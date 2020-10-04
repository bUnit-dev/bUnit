using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components.Web;

namespace Bunit
{
	internal static class KeysEventFactory
	{
		// Values were recorded using small Selenium application in Chrome browser.
		private static readonly Dictionary<Key, (string Key, string Code)> KeyEvents = new Dictionary<Key, (string key, string code)>()
		{
			{ Key.Backspace, ("Backspace", "Backspace") },
			{ Key.Tab, ("Tab", "Tab") },
			{ Key.Enter, ("Enter", "Enter") },
			{ Key.Pause, ("Pause", "Pause") },
			{ Key.Escape, ("Escape", "Escape") },
			{ Key.Space, (" ", "Space") },
			{ Key.PageUp, ("PageUp", "PageUp") },
			{ Key.PageDown, ("PageDown", "PageDown") },
			{ Key.End, ("End", "End") },
			{ Key.Home, ("Home", "Home") },
			{ Key.Left, ("ArrowLeft", "ArrowLeft") },
			{ Key.Up, ("ArrowUp", "ArrowUp") },
			{ Key.Right, ("ArrowRight", "ArrowRight") },
			{ Key.Down, ("ArrowDown", "ArrowDown") },
			{ Key.Insert, ("Insert", "Insert") },
			{ Key.Delete, ("Delete", "Delete") },
			{ Key.Equal, ("=", "Equal") },
			{ Key.NumberPad0, ("0", "Numpad0") },
			{ Key.NumberPad1, ("1", "Numpad1") },
			{ Key.NumberPad2, ("2", "Numpad2") },
			{ Key.NumberPad3, ("3", "Numpad3") },
			{ Key.NumberPad4, ("4", "Numpad4") },
			{ Key.NumberPad5, ("5", "Numpad5") },
			{ Key.NumberPad6, ("6", "Numpad6") },
			{ Key.NumberPad7, ("7", "Numpad7") },
			{ Key.NumberPad8, ("8", "Numpad8") },
			{ Key.NumberPad9, ("9", "Numpad9") },
			{ Key.Multiply, ("*", "NumpadMultiply") },
			{ Key.Add, ("+", "NumpadAdd") },
			{ Key.Subtract, ("-", "NumpadSubtract") },
			{ Key.NumberPadDecimal, (".", "NumpadDecimal") },
			{ Key.Divide, ("/", "NumpadDivide") },
			{ Key.F1, ("F1", "F1") },
			{ Key.F2, ("F2", "F2") },
			{ Key.F3, ("F3", "F3") },
			{ Key.F4, ("F4", "F4") },
			{ Key.F5, ("F5", "F5") },
			{ Key.F6, ("F6", "F6") },
			{ Key.F7, ("F7", "F7") },
			{ Key.F8, ("F8", "F8") },
			{ Key.F9, ("F9", "F9") },
			{ Key.F10, ("F10", "F10") },
			{ Key.F11, ("F11", "F11") },
			{ Key.F12, ("F12", "F12") },
			{ Key.Shift, ("Shift", "ShiftLeft") },
			{ Key.Control, ("Control", "ControlLeft") },
			{ Key.Alt, ("Alt", "AltLeft") },
			{ Key.Command, ("Meta", "MetaLeft") }
		};

		public static KeyboardEventArgs? CreateKeyboardEventArgs(Key key)
		{
			if (key == Key.Null)
			{
				return null;
			}

			// Reset high 16 bits to 0. High 16 bits represents a control key (e.g. Control, Alt) together with special key.
			Key keyWithoutControlKeys = (Key)((int)key & ((1 << 16) - 1));

			if (keyWithoutControlKeys == Key.Null)
			{
				// When only a control key is provided, create arguments from the first control key.
				if ((key & Key.Shift) != Key.Null)
				{
					keyWithoutControlKeys = Key.Shift;
				}
				else if ((key & Key.Control) != Key.Null)
				{
					keyWithoutControlKeys = Key.Control;
				}
				else if ((key & Key.Alt) != Key.Null)
				{
					keyWithoutControlKeys = Key.Alt;
				}
				else if ((key & Key.Command) != Key.Null)
				{
					keyWithoutControlKeys = Key.Command;
				}
			}

			if (KeyEvents.TryGetValue(keyWithoutControlKeys, out var keyArguments))
			{
				return new KeyboardEventArgs
				{
					Key = keyArguments.Key,
					Code = keyArguments.Code,
					ShiftKey = (key & Key.Shift) != Key.Null,
					CtrlKey = (key & Key.Control) != Key.Null,
					AltKey = (key & Key.Alt) != Key.Null,
					MetaKey = (key & Key.Command) != Key.Null
				};
			}

			throw new ArgumentException("Incorrect value of Key.", nameof(key));
		}
	}
}