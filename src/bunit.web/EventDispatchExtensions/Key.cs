using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components.Web;

namespace Bunit
{
	/// <summary>
	/// Representation of keyboard key that can be argument of keyboard events.
	/// </summary>
	public sealed class Key : IEquatable<Key>
	{
		private Key(string value) : this(value, value) { }

		private Key(string value, string code) : this(value, code, false, false, false, false) { }

		private Key(string value, string code, bool controlKey, bool shiftKey, bool altKey, bool commandKey)
		{
			Value = value;
			Code = code;
			ControlKey = controlKey;
			ShiftKey = shiftKey;
			AltKey = altKey;
			CommandKey = commandKey;
		}

		/// <summary>
		/// Gets the key value of the key represented. If the value has a printed
		/// representation, this attribute's value is the same as the char attribute.
		/// </summary>
		public string Value { get; }

		/// <summary>
		/// Gets the string that identifies the physical key being pressed. The value is not
		/// affected by the current keyboard layout or modifier state, so a particular key
		/// will always return the same value.
		/// </summary>
		public string Code { get; }

		/// <summary>
		/// Gets the value indicating whether Control key is pressed.
		/// </summary>
		public bool ControlKey { get; }

		/// <summary>
		/// Gets the value indicating whether Shift key is pressed.
		/// </summary>
		public bool ShiftKey { get; }

		/// <summary>
		/// Gets the value indicating whether Alt key is pressed.
		/// </summary>
		public bool AltKey { get; }

		/// <summary>
		/// Gets the value indicating whether Command key is pressed.
		/// </summary>
		public bool CommandKey { get; }

		/// <summary>
		/// Represents the Backspace key.
		/// </summary>
		public static Key Backspace { get; } = new Key("Backspace");

		/// <summary>
		/// Represents the Tab key.
		/// </summary>
		public static Key Tab { get; } = new Key("Tab");

		/// <summary>
		/// Represents the Enter key.
		/// </summary>
		public static Key Enter { get; } = new Key("Enter");

		/// <summary>
		/// Represents the Pause key.
		/// </summary>
		public static Key Pause { get; } = new Key("Pause");

		/// <summary>
		/// Represents the Escape key.
		/// </summary>
		public static Key Escape { get; } = new Key("Escape");

		/// <summary>
		/// Represents the Spacebar key.
		/// </summary>
		public static Key Space { get; } = new Key(" ", "Space");

		/// <summary>
		/// Represents the Page Up key.
		/// </summary>
		public static Key PageUp { get; } = new Key("PageUp");

		/// <summary>
		/// Represents the Page Down key.
		/// </summary>
		public static Key PageDown { get; } = new Key("PageDown");

		/// <summary>
		/// Represents the End key.
		/// </summary>
		public static Key End { get; } = new Key("End");

		/// <summary>
		/// Represents the Home key.
		/// </summary>
		public static Key Home { get; } = new Key("Home");

		/// <summary>
		/// Represents the left arrow key.
		/// </summary>
		public static Key Left { get; } = new Key("ArrowLeft");

		/// <summary>
		/// Represents the up arrow key.
		/// </summary>
		public static Key Up { get; } = new Key("ArrowUp");

		/// <summary>
		/// Represents the right arrow key.
		/// </summary>
		public static Key Right { get; } = new Key("ArrowRight");

		/// <summary>
		/// Represents the down arrow key.
		/// </summary>
		public static Key Down { get; } = new Key("ArrowDown");

		/// <summary>
		/// Represents the Insert key.
		/// </summary>
		public static Key Insert { get; } = new Key("Insert");

		/// <summary>
		/// Represents the Delete key.
		/// </summary>
		public static Key Delete { get; } = new Key("Delete");

		/// <summary>
		/// Represents the equal sign key.
		/// </summary>
		public static Key Equal { get; } = new Key("=", "Equal");

		/// <summary>
		/// Represents the number pad 0 key.
		/// </summary>
		public static Key NumberPad0 { get; } = new Key("0", "Numpad0");

		/// <summary>
		/// Represents the number pad 1 key.
		/// </summary>
		public static Key NumberPad1 { get; } = new Key("1", "Numpad1");

		/// <summary>
		/// Represents the number pad 2 key.
		/// </summary>
		public static Key NumberPad2 { get; } = new Key("2", "Numpad2");

		/// <summary>
		/// Represents the number pad 3 key.
		/// </summary>
		public static Key NumberPad3 { get; } = new Key("3", "Numpad3");

		/// <summary>
		/// Represents the number pad 4 key.
		/// </summary>
		public static Key NumberPad4 { get; } = new Key("4", "Numpad4");

		/// <summary>
		/// Represents the number pad 5 key.
		/// </summary>
		public static Key NumberPad5 { get; } = new Key("5", "Numpad5");

		/// <summary>
		/// Represents the number pad 6 key.
		/// </summary>
		public static Key NumberPad6 { get; } = new Key("6", "Numpad6");

		/// <summary>
		/// Represents the number pad 7 key.
		/// </summary>
		public static Key NumberPad7 { get; } = new Key("7", "Numpad7");

		/// <summary>
		/// Represents the number pad 8 key.
		/// </summary>
		public static Key NumberPad8 { get; } = new Key("8", "Numpad8");

		/// <summary>
		/// Represents the number pad 9 key.
		/// </summary>
		public static Key NumberPad9 { get; } = new Key("9", "Numpad9");

		/// <summary>
		/// Represents the number pad multiplication key.
		/// </summary>
		public static Key Multiply { get; } = new Key("*", "NumpadMultiply");

		/// <summary>
		/// Represents the number pad addition key.
		/// </summary>
		public static Key Add { get; } = new Key("+", "NumpadAdd");

		/// <summary>
		/// Represents the number pad subtraction key.
		/// </summary>
		public static Key Subtract { get; } = new Key("-", "NumpadSubtract");

		/// <summary>
		/// Represents the number pad decimal separator key.
		/// </summary>
		public static Key NumberPadDecimal { get; } = new Key(".", "NumpadDecimal");

		/// <summary>
		/// Represents the number pad division key.
		/// </summary>
		public static Key Divide { get; } = new Key("/", "NumpadDivide");

		/// <summary>
		/// Represents the function key F1.
		/// </summary>
		public static Key F1 { get; } = new Key("F1");

		/// <summary>
		/// Represents the function key F2.
		/// </summary>
		public static Key F2 { get; } = new Key("F2");

		/// <summary>
		/// Represents the function key F3.
		/// </summary>
		public static Key F3 { get; } = new Key("F3");

		/// <summary>
		/// Represents the function key F4.
		/// </summary>
		public static Key F4 { get; } = new Key("F4");

		/// <summary>
		/// Represents the function key F5.
		/// </summary>
		public static Key F5 { get; } = new Key("F5");

		/// <summary>
		/// Represents the function key F6.
		/// </summary>
		public static Key F6 { get; } = new Key("F6");

		/// <summary>
		/// Represents the function key F7.
		/// </summary>
		public static Key F7 { get; } = new Key("F7");

		/// <summary>
		/// Represents the function key F8.
		/// </summary>
		public static Key F8 { get; } = new Key("F8");

		/// <summary>
		/// Represents the function key F9.
		/// </summary>
		public static Key F9 { get; } = new Key("F9");

		/// <summary>
		/// Represents the function key F10.
		/// </summary>
		public static Key F10 { get; } = new Key("F10");

		/// <summary>
		/// Represents the function key F11.
		/// </summary>
		public static Key F11 { get; } = new Key("F11");

		/// <summary>
		/// Represents the function key F12.
		/// </summary>
		public static Key F12 { get; } = new Key("F12");

		/// <summary>
		/// Represents the Control key. This is a control key and it can be combined with other keys. E.g. Key.Enter + Key.Control
		/// </summary>
		public static Key Control { get; } = new Key("Control", "ControlLeft", true, false, false, false);

		/// <summary>
		/// Represents the Shift key. This is a control key and it can be combined with other keys. E.g. Key.Enter + Key.Shift
		/// </summary>
		public static Key Shift { get; } = new Key("Shift", "ShiftLeft", false, true, false, false);

		/// <summary>
		/// Represents the Alt key. This is a control key and it can be combined with other keys. E.g. Key.Enter + Keys.Alt
		/// </summary>
		public static Key Alt { get; } = new Key("Alt", "AltLeft", false, false, true, false);

		/// <summary>
		/// Represents the function key Command. This is a control key and it can be combined with other keys. E.g. Key.Enter + Key.Command
		/// </summary>
		public static Key Command { get; } = new Key("Meta", "MetaLeft", false, false, false, true);

		/// <summary>
		/// Gets <see cref="Key"/> object with specified value and code.
		/// </summary>
		/// <param name="value">The key value.</param>
		/// <param name="code">The key code of physical key.</param>
		public static Key Get(string value, string code)
		{
			if (string.IsNullOrEmpty(value))
				throw new ArgumentNullException(nameof(value));

			if (string.IsNullOrEmpty(code))
				throw new ArgumentNullException(nameof(code));

			if (PredefinedKeys.TryGetValue((value, code), out var key))
				return key;
			else
				return new Key(value, code);
		}

		/// <summary>
		/// Gets <see cref="Key"/> object with specified value.
		/// </summary>
		/// <param name="value">The key value.</param>
		public static Key Get(string value) => Get(value, value);

		/// <summary>
		/// Gets <see cref="Key"/> object from specified character.
		/// </summary>
		/// <param name="value">The key value.</param>
		public static Key Get(char value) => Get(value.ToString());

		/// <summary>
		/// Gets the value indicating whether the current object is equal to another object of the same type.
		/// </summary>
		/// <param name="obj">The object to compare with this object.</param>
		/// <returns><c>True</c> if the current object is equal to the other parameter; otherwise, <c>false</c>.</returns>
		public override bool Equals(object? obj) => obj is Key key && Equals(key);

		/// <summary>
		/// Gets the value indicating whether the current object is equal to another object of the same type.
		/// </summary>
		/// <param name="other">A key to compare with this object.</param>
		/// <returns><c>True</c> if the current object is equal to the other parameter; otherwise, <c>false</c>.</returns>
		public bool Equals(Key? other)
		{
			if (other is null)
				return false;

			return Value == other.Value &&
				   Code == other.Code &&
				   ControlKey == other.ControlKey &&
				   ShiftKey == other.ShiftKey &&
				   AltKey == other.AltKey &&
				   CommandKey == other.CommandKey;
		}

		/// <summary>
		/// Gets hash code of this object.
		/// </summary>
		/// <returns>A hash code for the current object.</returns>
		public override int GetHashCode() => HashCode.Combine(Value, Code, ControlKey, ShiftKey, AltKey, CommandKey);

		/// <summary>
		/// Gets a string that represents the current object.
		/// </summary>
		/// <returns>A string that represents the current object.</returns>
		public override string ToString() => Value;

		/// <summary>
		/// Gets the key with new value of Control key.
		/// </summary>
		/// <param name="value">New value of Control key.</param>
		/// <returns>The key with new value of Control key.</returns>
		public Key WithControlKey(bool value) => new Key(Value, Code, value, ShiftKey, AltKey, CommandKey);

		/// <summary>
		/// Gets the key with new value of Shift key.
		/// </summary>
		/// <param name="value">New value of Shift key.</param>
		/// <returns>The key with new value of Shift key.</returns>
		public Key WithShiftKey(bool value) => new Key(Value, Code, ControlKey, value, AltKey, CommandKey);

		/// <summary>
		/// Gets the key with new value of Alt key.
		/// </summary>
		/// <param name="value">New value of Alt key.</param>
		/// <returns>The key with new value of Alt key.</returns>
		public Key WithAltKey(bool value) => new Key(Value, Code, ControlKey, ShiftKey, value, CommandKey);

		/// <summary>
		/// Gets the key with new value of Command key.
		/// </summary>
		/// <param name="value">New value of Command key.</param>
		/// <returns>The key with new value of Command key.</returns>
		public Key WithCommandKey(bool value) => new Key(Value, Code, ControlKey, ShiftKey, AltKey, value);

		/// <summary>
		/// Gets a combination of current key with another key. A key instance can be combined only 
		/// with <see cref="Control" />, <see cref="Shift" />, <see cref="Alt" />, or <see cref="Command" /> keys.
		/// </summary>
		/// <param name="key">The other key to combine with.</param>
		/// <returns>A new key with combination of Control, Shift, Alt, and Command keys.</returns>
		public Key Combine(Key? key)
		{
			if (key is null)
				return this;
			else if (IsSuitableForCombination(key))
				return Combine(this, key);
			else if (IsSuitableForCombination(this))
				return Combine(key, this);
			else
				throw new ArgumentException($"Keys '{this}' and '{key}' cannot be combined. The other key must be Control, Shift, Alt, or Command key.", nameof(key));

			static bool IsSuitableForCombination(Key key)
				=> (key.Value == Control.Value && key.Code == Control.Code) ||
				   (key.Value == Shift.Value && key.Code == Shift.Code) ||
				   (key.Value == Alt.Value && key.Code == Alt.Code) ||
				   (key.Value == Command.Value && key.Code == Command.Code);

			static Key Combine(Key first, Key second)
				=> new Key(
					first.Value,
					first.Code,
					first.ControlKey || second.ControlKey,
					first.ShiftKey || second.ShiftKey,
					first.AltKey || second.AltKey,
					first.CommandKey || second.CommandKey);
		}

		/// <summary>
		/// Gets the value indicating whether 2 instances of <see cref="Key" /> are equal.
		/// </summary>
		/// <param name="x">The first key to compare.</param>
		/// <param name="y">The second key to compare.</param>
		/// <returns><c>True</c> if the instances of Key are equal; otherwise, <c>false</c>.</returns>
		public static bool operator ==(Key? x, Key? y)
		{
			if (x is null && y is null)
				return true;
			else if (x is null || y is null)
				return false;
			else if (object.ReferenceEquals(x, y))
				return true;

			return x is not null && x.Equals(y);
		}

		/// <summary>
		/// Gets the value indicating whether 2 instances of <see cref="Key" /> are different.
		/// </summary>
		/// <param name="x">The first key to compare.</param>
		/// <param name="y">The second key to compare.</param>
		/// <returns><c>True</c> if the instances of Key are different; otherwise, <c>false</c>.</returns>
		public static bool operator !=(Key? x, Key? y) => !(x == y);

		/// <summary>
		/// Gets a combination of 2 key objects. A key instance can be combined only with <see cref="Control" />,
		/// <see cref="Shift" />, <see cref="Alt" />, or <see cref="Command" /> keys.
		/// </summary>
		/// <param name="x">The first key to combine.</param>
		/// <param name="y">The second key to combine.</param>
		/// <returns>A new key with combination of Control, Shift, Alt, and Command keys.</returns>
		[return: NotNullIfNotNull("x")]
		[return: NotNullIfNotNull("y")]
		public static Key operator +(Key x, Key? y)
		{
			if (x is not null)
				return x.Combine(y);
			else if (y is not null)
				return y.Combine(x);
			else
				return null!;
		}

		/// <summary>
		/// Gets a new <see cref="Key" /> instance with value of string object.
		/// </summary>
		/// <param name="value">The string value to convert to Key instance.</param>
		/// <returns>The Key instance with the specified value.</returns>
		public static implicit operator Key(string value) => Key.Get(value);

		/// <summary>
		/// Gets a new <see cref="Key" /> instance with value of character.
		/// </summary>
		/// <param name="key">The character to convert to Key instance.</param>
		/// <returns>The Key instance with character value.</returns>
		public static implicit operator Key(char key) => Key.Get(key);

		/// <summary>
		/// Gets a new <see cref="Key" /> instance with value of character.
		/// </summary>
		/// <param name="key">The character to convert to Key instance.</param>
		/// <returns>The Key instance with character value.</returns>
		[return: NotNullIfNotNull("key")]
		public static implicit operator KeyboardEventArgs(Key key)
		{
			if (key is null)
				return null!;

			return new KeyboardEventArgs
			{
				Key = key.Value,
				Code = key.Code,
				CtrlKey = key.ControlKey,
				ShiftKey = key.ShiftKey,
				AltKey = key.AltKey,
				MetaKey = key.CommandKey
			};
		}

		// This has to be placed last since it is referencing other static fields, that must be initialized first.
		private static readonly Dictionary<(string value, string code), Key> PredefinedKeys = new()
		{
			{ (Key.Backspace.Value, Key.Backspace.Code), Key.Backspace },
			{ (Key.Tab.Value, Key.Tab.Code), Key.Tab },
			{ (Key.Enter.Value, Key.Enter.Code), Key.Enter },
			{ (Key.Pause.Value, Key.Pause.Code), Key.Pause },
			{ (Key.Escape.Value, Key.Escape.Code), Key.Escape },
			{ (Key.Space.Value, Key.Space.Code), Key.Space },
			{ (Key.PageUp.Value, Key.PageUp.Code), Key.PageUp },
			{ (Key.PageDown.Value, Key.PageDown.Code), Key.PageDown },
			{ (Key.End.Value, Key.End.Code), Key.End },
			{ (Key.Home.Value, Key.Home.Code), Key.Home },
			{ (Key.Left.Value, Key.Left.Code), Key.Left },
			{ (Key.Up.Value, Key.Up.Code), Key.Up },
			{ (Key.Right.Value, Key.Right.Code), Key.Right },
			{ (Key.Down.Value, Key.Down.Code), Key.Down },
			{ (Key.Insert.Value, Key.Insert.Code), Key.Insert },
			{ (Key.Delete.Value, Key.Delete.Code), Key.Delete },
			{ (Key.Equal.Value, Key.Equal.Code), Key.Equal },
			{ (Key.NumberPad0.Value, Key.NumberPad0.Code), Key.NumberPad0 },
			{ (Key.NumberPad1.Value, Key.NumberPad1.Code), Key.NumberPad1 },
			{ (Key.NumberPad2.Value, Key.NumberPad2.Code), Key.NumberPad2 },
			{ (Key.NumberPad3.Value, Key.NumberPad3.Code), Key.NumberPad3 },
			{ (Key.NumberPad4.Value, Key.NumberPad4.Code), Key.NumberPad4 },
			{ (Key.NumberPad5.Value, Key.NumberPad5.Code), Key.NumberPad5 },
			{ (Key.NumberPad6.Value, Key.NumberPad6.Code), Key.NumberPad6 },
			{ (Key.NumberPad7.Value, Key.NumberPad7.Code), Key.NumberPad7 },
			{ (Key.NumberPad8.Value, Key.NumberPad8.Code), Key.NumberPad8 },
			{ (Key.NumberPad9.Value, Key.NumberPad9.Code), Key.NumberPad9 },
			{ (Key.Multiply.Value, Key.Multiply.Code), Key.Multiply },
			{ (Key.Add.Value, Key.Add.Code), Key.Add },
			{ (Key.Subtract.Value, Key.Subtract.Code), Key.Subtract },
			{ (Key.NumberPadDecimal.Value, Key.NumberPadDecimal.Code), Key.NumberPadDecimal },
			{ (Key.Divide.Value, Key.Divide.Code), Key.Divide },
			{ (Key.F1.Value, Key.F1.Code), Key.F1 },
			{ (Key.F2.Value, Key.F2.Code), Key.F2 },
			{ (Key.F3.Value, Key.F3.Code), Key.F3 },
			{ (Key.F4.Value, Key.F4.Code), Key.F4 },
			{ (Key.F5.Value, Key.F5.Code), Key.F5 },
			{ (Key.F6.Value, Key.F6.Code), Key.F6 },
			{ (Key.F7.Value, Key.F7.Code), Key.F7 },
			{ (Key.F8.Value, Key.F8.Code), Key.F8 },
			{ (Key.F9.Value, Key.F9.Code), Key.F9 },
			{ (Key.F10.Value, Key.F10.Code), Key.F10 },
			{ (Key.F11.Value, Key.F11.Code), Key.F11 },
			{ (Key.F12.Value, Key.F12.Code), Key.F12 },
			{ (Key.Control.Value, Key.Control.Code), Key.Control },
			{ (Key.Shift.Value, Key.Shift.Code), Key.Shift },
			{ (Key.Alt.Value, Key.Alt.Code), Key.Alt },
			{ (Key.Command.Value, Key.Command.Code), Key.Command }
		};
	}
}
