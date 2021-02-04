using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components.Web;

namespace Bunit
{
	/// <summary>
	/// Representation of keyboard key that can be argument of keyboard events.
	/// </summary>
	[SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "It seems unlikely that VB.NET would be used with this library, which might have issues with some of the overloads.")]
	public sealed class Key : IEquatable<Key>
	{
		private Key(string value)
			: this(value, value) { }

		private Key(string value, string code)
			: this(value, code, controlKey: false, shiftKey: false, altKey: false, commandKey: false) { }

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
		/// Gets a value indicating whether Control key is pressed.
		/// </summary>
		public bool ControlKey { get; }

		/// <summary>
		/// Gets a value indicating whether Shift key is pressed.
		/// </summary>
		public bool ShiftKey { get; }

		/// <summary>
		/// Gets a value indicating whether Alt key is pressed.
		/// </summary>
		public bool AltKey { get; }

		/// <summary>
		/// Gets a value indicating whether Command key is pressed.
		/// </summary>
		public bool CommandKey { get; }

		/// <summary>
		/// Gets a <see cref="Key"/> that represents the Backspace key.
		/// </summary>
		public static Key Backspace { get; } = new Key("Backspace");

		/// <summary>
		/// Gets a <see cref="Key"/> that represents the Tab key.
		/// </summary>
		public static Key Tab { get; } = new Key("Tab");

		/// <summary>
		/// Gets a <see cref="Key"/> that represents the Enter key.
		/// </summary>
		public static Key Enter { get; } = new Key("Enter");

		/// <summary>
		/// Gets a <see cref="Key"/> that represents the Pause key.
		/// </summary>
		public static Key Pause { get; } = new Key("Pause");

		/// <summary>
		/// Gets a <see cref="Key"/> that represents the Escape key.
		/// </summary>
		public static Key Escape { get; } = new Key("Escape");

		/// <summary>
		/// Gets a <see cref="Key"/> that represents the Spacebar key.
		/// </summary>
		public static Key Space { get; } = new Key(" ", "Space");

		/// <summary>
		/// Gets a <see cref="Key"/> that represents the Page Up key.
		/// </summary>
		public static Key PageUp { get; } = new Key("PageUp");

		/// <summary>
		/// Gets a <see cref="Key"/> that represents the Page Down key.
		/// </summary>
		public static Key PageDown { get; } = new Key("PageDown");

		/// <summary>
		/// Gets a <see cref="Key"/> that represents the End key.
		/// </summary>
		public static Key End { get; } = new Key("End");

		/// <summary>
		/// Gets a <see cref="Key"/> that represents the Home key.
		/// </summary>
		public static Key Home { get; } = new Key("Home");

		/// <summary>
		/// Gets a <see cref="Key"/> that represents the left arrow key.
		/// </summary>
		public static Key Left { get; } = new Key("ArrowLeft");

		/// <summary>
		/// Gets a <see cref="Key"/> that represents the up arrow key.
		/// </summary>
		public static Key Up { get; } = new Key("ArrowUp");

		/// <summary>
		/// Gets a <see cref="Key"/> that represents the right arrow key.
		/// </summary>
		public static Key Right { get; } = new Key("ArrowRight");

		/// <summary>
		/// Gets a <see cref="Key"/> that represents the down arrow key.
		/// </summary>
		public static Key Down { get; } = new Key("ArrowDown");

		/// <summary>
		/// Gets a <see cref="Key"/> that represents the Insert key.
		/// </summary>
		public static Key Insert { get; } = new Key("Insert");

		/// <summary>
		/// Gets a <see cref="Key"/> that represents the Delete key.
		/// </summary>
		public static Key Delete { get; } = new Key("Delete");

		/// <summary>
		/// Gets a <see cref="Key"/> that represents the equal sign key.
		/// </summary>
		public static Key Equal { get; } = new Key("=", "Equal");

		/// <summary>
		/// Gets a <see cref="Key"/> that represents the number pad 0 key.
		/// </summary>
		public static Key NumberPad0 { get; } = new Key("0", "Numpad0");

		/// <summary>
		/// Gets a <see cref="Key"/> that represents the number pad 1 key.
		/// </summary>
		public static Key NumberPad1 { get; } = new Key("1", "Numpad1");

		/// <summary>
		/// Gets a <see cref="Key"/> that represents the number pad 2 key.
		/// </summary>
		public static Key NumberPad2 { get; } = new Key("2", "Numpad2");

		/// <summary>
		/// Gets a <see cref="Key"/> that represents the number pad 3 key.
		/// </summary>
		public static Key NumberPad3 { get; } = new Key("3", "Numpad3");

		/// <summary>
		/// Gets a <see cref="Key"/> that represents the number pad 4 key.
		/// </summary>
		public static Key NumberPad4 { get; } = new Key("4", "Numpad4");

		/// <summary>
		/// Gets a <see cref="Key"/> that represents the number pad 5 key.
		/// </summary>
		public static Key NumberPad5 { get; } = new Key("5", "Numpad5");

		/// <summary>
		/// Gets a <see cref="Key"/> that represents the number pad 6 key.
		/// </summary>
		public static Key NumberPad6 { get; } = new Key("6", "Numpad6");

		/// <summary>
		/// Gets a <see cref="Key"/> that represents the number pad 7 key.
		/// </summary>
		public static Key NumberPad7 { get; } = new Key("7", "Numpad7");

		/// <summary>
		/// Gets a <see cref="Key"/> that represents the number pad 8 key.
		/// </summary>
		public static Key NumberPad8 { get; } = new Key("8", "Numpad8");

		/// <summary>
		/// Gets a <see cref="Key"/> that represents the number pad 9 key.
		/// </summary>
		public static Key NumberPad9 { get; } = new Key("9", "Numpad9");

		/// <summary>
		/// Gets a <see cref="Key"/> that represents the number pad multiplication key.
		/// </summary>
		public static Key Multiply { get; } = new Key("*", "NumpadMultiply");

		/// <summary>
		/// Gets a <see cref="Key"/> that represents the number pad addition key.
		/// </summary>
		public static Key Add { get; } = new Key("+", "NumpadAdd");

		/// <summary>
		/// Gets a <see cref="Key"/> that represents the number pad subtraction key.
		/// </summary>
		public static Key Subtract { get; } = new Key("-", "NumpadSubtract");

		/// <summary>
		/// Gets a <see cref="Key"/> that represents the number pad decimal separator key.
		/// </summary>
		public static Key NumberPadDecimal { get; } = new Key(".", "NumpadDecimal");

		/// <summary>
		/// Gets a <see cref="Key"/> that represents the number pad division key.
		/// </summary>
		public static Key Divide { get; } = new Key("/", "NumpadDivide");

		/// <summary>
		/// Gets a <see cref="Key"/> that represents the function key F1.
		/// </summary>
		public static Key F1 { get; } = new Key("F1");

		/// <summary>
		/// Gets a <see cref="Key"/> that represents the function key F2.
		/// </summary>
		public static Key F2 { get; } = new Key("F2");

		/// <summary>
		/// Gets a <see cref="Key"/> that represents the function key F3.
		/// </summary>
		public static Key F3 { get; } = new Key("F3");

		/// <summary>
		/// Gets a <see cref="Key"/> that represents the function key F4.
		/// </summary>
		public static Key F4 { get; } = new Key("F4");

		/// <summary>
		/// Gets a <see cref="Key"/> that represents the function key F5.
		/// </summary>
		public static Key F5 { get; } = new Key("F5");

		/// <summary>
		/// Gets a <see cref="Key"/> that represents the function key F6.
		/// </summary>
		public static Key F6 { get; } = new Key("F6");

		/// <summary>
		/// Gets a <see cref="Key"/> that represents the function key F7.
		/// </summary>
		public static Key F7 { get; } = new Key("F7");

		/// <summary>
		/// Gets a <see cref="Key"/> that represents the function key F8.
		/// </summary>
		public static Key F8 { get; } = new Key("F8");

		/// <summary>
		/// Gets a <see cref="Key"/> that represents the function key F9.
		/// </summary>
		public static Key F9 { get; } = new Key("F9");

		/// <summary>
		/// Gets a <see cref="Key"/> that represents the function key F10.
		/// </summary>
		public static Key F10 { get; } = new Key("F10");

		/// <summary>
		/// Gets a <see cref="Key"/> that represents the function key F11.
		/// </summary>
		public static Key F11 { get; } = new Key("F11");

		/// <summary>
		/// Gets a <see cref="Key"/> that represents the function key F12.
		/// </summary>
		public static Key F12 { get; } = new Key("F12");

		/// <summary>
		/// Gets a <see cref="Key"/> that represents the Control key. This is a control key and it can be combined with other keys. E.g. Key.Enter + Key.Control.
		/// </summary>
		public static Key Control { get; } = new Key("Control", "ControlLeft", controlKey: true, shiftKey: false, altKey: false, commandKey: false);

		/// <summary>
		/// Gets a <see cref="Key"/> that represents the Shift key. This is a control key and it can be combined with other keys. E.g. Key.Enter + Key.Shift.
		/// </summary>
		public static Key Shift { get; } = new Key("Shift", "ShiftLeft", controlKey: false, shiftKey: true, altKey: false, commandKey: false);

		/// <summary>
		/// Gets a <see cref="Key"/> that represents the Alt key. This is a control key and it can be combined with other keys. E.g. Key.Enter + Keys.Alt.
		/// </summary>
		public static Key Alt { get; } = new Key("Alt", "AltLeft", controlKey: false, shiftKey: false, altKey: true, commandKey: false);

		/// <summary>
		/// Gets a <see cref="Key"/> that represents the function key Command. This is a control key and it can be combined with other keys. E.g. Key.Enter + Key.Command.
		/// </summary>
		public static Key Command { get; } = new Key("Meta", "MetaLeft", controlKey: false, shiftKey: false, altKey: false, commandKey: true);

		/// <summary>
		/// Gets a <see cref="Key"/> object with specified value and code.
		/// </summary>
		/// <param name="value">The key value.</param>
		/// <param name="code">The key code of physical key.</param>
		public static Key Get(string value, string code)
		{
			if (string.IsNullOrEmpty(value))
				throw new ArgumentNullException(nameof(value));

			if (string.IsNullOrEmpty(code))
				throw new ArgumentNullException(nameof(code));

			return PredefinedKeys.TryGetValue((value, code), out var key)
				? key
				: new Key(value, code);
		}

		/// <summary>
		/// Gets a <see cref="Key"/> object with specified value.
		/// </summary>
		/// <param name="value">The key value.</param>
		public static Key Get(string value) => Get(value, value);

		/// <summary>
		/// Gets a <see cref="Key"/> object from specified character.
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

			return string.Equals(Value, other.Value, StringComparison.Ordinal) &&
				   string.Equals(Code, other.Code, StringComparison.Ordinal) &&
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
			if (IsSuitableForCombination(key))
				return Combine(this, key);
			if (IsSuitableForCombination(this))
				return Combine(key, this);

			throw new ArgumentException($"Keys '{this}' and '{key}' cannot be combined. The other key must be Control, Shift, Alt, or Command key.", nameof(key));

			static bool IsSuitableForCombination(Key key)
				=> (string.Equals(key.Value, Control.Value, StringComparison.Ordinal) && string.Equals(key.Code, Control.Code, StringComparison.Ordinal)) ||
				   (string.Equals(key.Value, Shift.Value, StringComparison.Ordinal) && string.Equals(key.Code, Shift.Code, StringComparison.Ordinal)) ||
				   (string.Equals(key.Value, Alt.Value, StringComparison.Ordinal) && string.Equals(key.Code, Alt.Code, StringComparison.Ordinal)) ||
				   (string.Equals(key.Value, Command.Value, StringComparison.Ordinal) && string.Equals(key.Code, Command.Code, StringComparison.Ordinal));

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
			if (x is null || y is null)
				return false;
			if (ReferenceEquals(x, y))
				return true;
			return x.Equals(y);
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
			if (y is not null)
				return y.Combine(x);

			return null!;
		}

		/// <summary>
		/// Gets a new <see cref="Key" /> instance with value of string object.
		/// </summary>
		/// <param name="value">The string value to convert to Key instance.</param>
		/// <returns>The Key instance with the specified value.</returns>
		public static implicit operator Key(string value) => Get(value);

		/// <summary>
		/// Gets a new <see cref="Key" /> instance with value of character.
		/// </summary>
		/// <param name="key">The character to convert to Key instance.</param>
		/// <returns>The Key instance with character value.</returns>
		public static implicit operator Key(char key) => Get(key);

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
				MetaKey = key.CommandKey,
			};
		}

		// This has to be placed last since it is referencing other static fields, that must be initialized first.
		private static readonly Dictionary<(string Value, string Code), Key> PredefinedKeys = new()
		{
			{ (Backspace.Value, Backspace.Code), Backspace },
			{ (Tab.Value, Tab.Code), Tab },
			{ (Enter.Value, Enter.Code), Enter },
			{ (Pause.Value, Pause.Code), Pause },
			{ (Escape.Value, Escape.Code), Escape },
			{ (Space.Value, Space.Code), Space },
			{ (PageUp.Value, PageUp.Code), PageUp },
			{ (PageDown.Value, PageDown.Code), PageDown },
			{ (End.Value, End.Code), End },
			{ (Home.Value, Home.Code), Home },
			{ (Left.Value, Left.Code), Left },
			{ (Up.Value, Up.Code), Up },
			{ (Right.Value, Right.Code), Right },
			{ (Down.Value, Down.Code), Down },
			{ (Insert.Value, Insert.Code), Insert },
			{ (Delete.Value, Delete.Code), Delete },
			{ (Equal.Value, Equal.Code), Equal },
			{ (NumberPad0.Value, NumberPad0.Code), NumberPad0 },
			{ (NumberPad1.Value, NumberPad1.Code), NumberPad1 },
			{ (NumberPad2.Value, NumberPad2.Code), NumberPad2 },
			{ (NumberPad3.Value, NumberPad3.Code), NumberPad3 },
			{ (NumberPad4.Value, NumberPad4.Code), NumberPad4 },
			{ (NumberPad5.Value, NumberPad5.Code), NumberPad5 },
			{ (NumberPad6.Value, NumberPad6.Code), NumberPad6 },
			{ (NumberPad7.Value, NumberPad7.Code), NumberPad7 },
			{ (NumberPad8.Value, NumberPad8.Code), NumberPad8 },
			{ (NumberPad9.Value, NumberPad9.Code), NumberPad9 },
			{ (Multiply.Value, Multiply.Code), Multiply },
			{ (Add.Value, Add.Code), Add },
			{ (Subtract.Value, Subtract.Code), Subtract },
			{ (NumberPadDecimal.Value, NumberPadDecimal.Code), NumberPadDecimal },
			{ (Divide.Value, Divide.Code), Divide },
			{ (F1.Value, F1.Code), F1 },
			{ (F2.Value, F2.Code), F2 },
			{ (F3.Value, F3.Code), F3 },
			{ (F4.Value, F4.Code), F4 },
			{ (F5.Value, F5.Code), F5 },
			{ (F6.Value, F6.Code), F6 },
			{ (F7.Value, F7.Code), F7 },
			{ (F8.Value, F8.Code), F8 },
			{ (F9.Value, F9.Code), F9 },
			{ (F10.Value, F10.Code), F10 },
			{ (F11.Value, F11.Code), F11 },
			{ (F12.Value, F12.Code), F12 },
			{ (Control.Value, Control.Code), Control },
			{ (Shift.Value, Shift.Code), Shift },
			{ (Alt.Value, Alt.Code), Alt },
			{ (Command.Value, Command.Code), Command },
		};
	}
}
