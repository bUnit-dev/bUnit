using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components.Web;

namespace Bunit
{
	/// <summary>
	/// Representation of keyboard key that can be argument of keyboard events.
	/// </summary>
	public sealed class Key : IEquatable<Key>
	{
		/// <summary>
		/// Creates a new instance of the <see cref="Key"/> class.
		/// </summary>
		/// <param name="value">The key value.</param>
		public Key(string value)
			: this(value, value)
		{
		}

		/// <summary>
		/// Creates a new instance of the <see cref="Key"/> class.
		/// </summary>
		/// <param name="value">The key value.</param>
		/// <param name="code">The key code of physical key.</param>
		public Key(string value, string code)
		{
			if (string.IsNullOrEmpty(value))
			{
				throw new ArgumentNullException(nameof(value));
			}

			if (string.IsNullOrEmpty(code))
			{
				throw new ArgumentNullException(nameof(code));
			}

			Value = value;
			Code = code;
		}

		/// <summary>
		/// Creates a new instance of the <see cref="Key"/> class.
		/// </summary>
		/// <param name="value">The key value.</param>
		public Key(char value)
			: this(value.ToString())
		{
		}

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
		/// Creates a new instance of the <see cref="Key"/> class from specified character.
		/// </summary>
		/// <param name="value">The key value.</param>
		public static Key FromChar(char value) => new Key(value);

		/// <summary>
		/// Gets the value indicating whether the current object is equal to another object of the same type.
		/// </summary>
		/// <param name="obj">The object to compare with this object.</param>
		/// <returns><c>True</c> if the current object is equal to the other parameter; otherwise, <c>false</c>.</returns>
		public override bool Equals(object? obj)
		{
			return obj is Key key && Equals(key);
		}

		/// <summary>
		/// Gets the value indicating whether the current object is equal to another object of the same type.
		/// </summary>
		/// <param name="other">A key to compare with this object.</param>
		/// <returns><c>True</c> if the current object is equal to the other parameter; otherwise, <c>false</c>.</returns>
		public bool Equals(Key? other)
		{
			if (other is null)
			{
				return false;
			}

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
		public override int GetHashCode()
		{
			return HashCode.Combine(Value, Code, ControlKey, ShiftKey, AltKey, CommandKey);
		}

		/// <summary>
		/// Gets a string that represents the current object.
		/// </summary>
		/// <returns>A string that represents the current object.</returns>
		public override string ToString()
		{
			return Value;
		}

		/// <summary>
		/// Gets the key with new value of Control key.
		/// </summary>
		/// <param name="value">New value of Control key.</param>
		/// <returns>The key with new value of Control key.</returns>
		public Key WithControlKey(bool value)
		{
			return new Key(Value, Code, value, ShiftKey, AltKey, CommandKey);
		}

		/// <summary>
		/// Gets the key with new value of Shift key.
		/// </summary>
		/// <param name="value">New value of Shift key.</param>
		/// <returns>The key with new value of Shift key.</returns>
		public Key WithShiftKey(bool value)
		{
			return new Key(Value, Code, ControlKey, value, AltKey, CommandKey);
		}

		/// <summary>
		/// Gets the key with new value of Alt key.
		/// </summary>
		/// <param name="value">New value of Alt key.</param>
		/// <returns>The key with new value of Alt key.</returns>
		public Key WithAltKey(bool value)
		{
			return new Key(Value, Code, ControlKey, ShiftKey, value, CommandKey);
		}

		/// <summary>
		/// Gets the key with new value of Command key.
		/// </summary>
		/// <param name="value">New value of Command key.</param>
		/// <returns>The key with new value of Command key.</returns>
		public Key WithCommandKey(bool value)
		{
			return new Key(Value, Code, ControlKey, ShiftKey, AltKey, value);
		}

		/// <summary>
		/// Gets a combination of current key with another key. A key instance can be combined only 
		/// with <see cref="Control" />, <see cref="Shift" />, <see cref="Alt" />, or <see cref="Command" /> keys.
		/// </summary>
		/// <param name="key">The other key to combine with.</param>
		/// <returns>A new key with combination of Control, Shift, Alt, and Command keys.</returns>
		public Key Combine(Key? key)
		{
			if (key is null)
			{
				return this;
			}
			else if (IsSuitableForCombination(key))
			{
				return new Key(Value, Code, ControlKey || key.ControlKey, ShiftKey || key.ShiftKey, AltKey || key.AltKey, CommandKey || key.CommandKey);
			}
			else if (IsSuitableForCombination(this))
			{
				return new Key(key.Value, key.Code, ControlKey || key.ControlKey, ShiftKey || key.ShiftKey, AltKey || key.AltKey, CommandKey || key.CommandKey);
			}
			else
			{
				throw new ArgumentException($"Keys '{this}' and '{key}' cannot be combined. The other key must be Control, Shift, Alt, or Command key.", nameof(key));
			}

			static bool IsSuitableForCombination(Key key)
			{
				return (key.Value == Control.Value && key.Code == Control.Code) ||
					(key.Value == Shift.Value && key.Code == Shift.Code) ||
					(key.Value == Alt.Value && key.Code == Alt.Code) ||
					(key.Value == Command.Value && key.Code == Command.Code);
			}
		}

		/// <summary>
		/// Gets the value indicating whether 2 instances of <see cref="Key" /> are equal.
		/// </summary>
		/// <param name="x">The first key to compare.</param>
		/// <param name="y">The second key to compare.</param>
		/// <returns><c>True</c> if the instances of Key are equal; otherwise, <c>false</c>.</returns>
		public static bool operator ==(Key? x, Key? y)
		{
			if (object.ReferenceEquals(x, y))
			{
				return true;
			}

			return !(x is null) && x.Equals(y);
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
		[SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "Alternative method is named Combine")]
		public static Key operator +(Key x, Key? y)
		{
			if (!(x is null))
			{
				return x.Combine(y);
			}
			else if (!(y is null))
			{
				return y.Combine(x);
			}
			else
			{
				// In future 'x' should be supported as nullable.
				// However, NotNullIfNotNull does not work correctly with operators.
				// Therfore workaround is to make 'x' non-nullable. 
				return null!;
			}
		}

		/// <summary>
		/// Gets a new <see cref="Key" /> instance with value of character.
		/// </summary>
		/// <param name="key">The character to convert to Key instance.</param>
		/// <returns>The Key instance with character value.</returns>
		public static implicit operator Key(char key) => new Key(key);

		/// <summary>
		/// Gets a new <see cref="Key" /> instance with value of character.
		/// </summary>
		/// <param name="key">The character to convert to Key instance.</param>
		/// <returns>The Key instance with character value.</returns>
		[return: NotNullIfNotNull("key")]
		[SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "Key should have minimal relation to KeyboardEventArgs type")]
		public static implicit operator KeyboardEventArgs(Key key)
		{
			if (key is null)
			{
				// In future the operator should support null as input.
				// However, NotNullIfNotNull does not work correctly with operators.
				// Therfore workaround is to make input non-nullable. 
				return null!;
			}

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
	}
}
