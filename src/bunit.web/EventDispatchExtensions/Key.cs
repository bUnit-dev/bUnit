namespace Bunit
{
	// Integer values are same as in Selenium Keys class. The Selenium Keys class provides values of strings with single characters.
	// These values are integer code of the characters in Selenium Keys class. Exceptions are Null and control keys (e.g. Shift, Alt, Control).
	// Principle is that control keys are encoded as bit flags in high 16 bits. Non-control key values must be in low 16 bits.
	// This way it is possible to combine special keys with control keys. E.g. Key.Enter | Key.Alt
	/// <summary>
	/// Special keys that can be dispatched in keyboard events.
	/// </summary>
	public enum Key
	{
		/// <summary>
		/// No keyboard key pressed. This option raises no event.
		/// </summary>
		Null = 0,

		/// <summary>
		/// Represents the Backspace key.
		/// </summary>
		Backspace = 57347,

		/// <summary>
		/// Represents the Tab key.
		/// </summary>
		Tab = 57348,

		/// <summary>
		/// Represents the Enter key.
		/// </summary>
		Enter = 57351,

		/// <summary>
		/// Represents the Pause key.
		/// </summary>
		Pause = 57355,

		/// <summary>
		/// Represents the Escape key.
		/// </summary>
		Escape = 57356,

		/// <summary>
		/// Represents the Spacebar key.
		/// </summary>
		Space = 57357,

		/// <summary>
		/// Represents the Page Up key.
		/// </summary>
		PageUp = 57358,

		/// <summary>
		/// Represents the Page Down key.
		/// </summary>
		PageDown = 57359,

		/// <summary>
		/// Represents the End key.
		/// </summary>
		End = 57360,

		/// <summary>
		/// Represents the Home key.
		/// </summary>
		Home = 57361,

		/// <summary>
		/// Represents the left arrow key.
		/// </summary>
		Left = 57362,

		/// <summary>
		/// Represents the up arrow key.
		/// </summary>
		Up = 57363,

		/// <summary>
		/// Represents the right arrow key.
		/// </summary>
		Right = 57364,

		/// <summary>
		/// Represents the down arrow key.
		/// </summary>
		Down = 57365,

		/// <summary>
		/// Represents the Insert key.
		/// </summary>
		Insert = 57366,

		/// <summary>
		/// Represents the Delete key.
		/// </summary>
		Delete = 57367,

		/// <summary>
		/// Represents the equal sign key.
		/// </summary>
		Equal = 57369,

		/// <summary>
		/// Represents the number pad 0 key.
		/// </summary>
		NumberPad0 = 57370,

		/// <summary>
		/// Represents the number pad 1 key.
		/// </summary>
		NumberPad1 = 57371,

		/// <summary>
		/// Represents the number pad 2 key.
		/// </summary>
		NumberPad2 = 57372,

		/// <summary>
		/// Represents the number pad 3 key.
		/// </summary>
		NumberPad3 = 57373,

		/// <summary>
		/// Represents the number pad 4 key.
		/// </summary>
		NumberPad4 = 57374,

		/// <summary>
		/// Represents the number pad 5 key.
		/// </summary>
		NumberPad5 = 57375,

		/// <summary>
		/// Represents the number pad 6 key.
		/// </summary>
		NumberPad6 = 57376,

		/// <summary>
		/// Represents the number pad 7 key.
		/// </summary>
		NumberPad7 = 57377,

		/// <summary>
		/// Represents the number pad 8 key.
		/// </summary>
		NumberPad8 = 57378,

		/// <summary>
		/// Represents the number pad 9 key.
		/// </summary>
		NumberPad9 = 57379,

		/// <summary>
		/// Represents the number pad multiplication key.
		/// </summary>
		Multiply = 57380,

		/// <summary>
		/// Represents the number pad addition key.
		/// </summary>
		Add = 57381,

		/// <summary>
		/// Represents the number pad subtraction key.
		/// </summary>
		Subtract = 57383,

		/// <summary>
		/// Represents the number pad decimal separator key.
		/// </summary>
		NumberPadDecimal = 57384,

		/// <summary>
		/// Represents the number pad division key.
		/// </summary>
		Divide = 57385,

		/// <summary>
		/// Represents the function key F1.
		/// </summary>
		F1 = 57393,

		/// <summary>
		/// Represents the function key F2.
		/// </summary>
		F2 = 57394,

		/// <summary>
		/// Represents the function key F3.
		/// </summary>
		F3 = 57395,

		/// <summary>
		/// Represents the function key F4.
		/// </summary>
		F4 = 57396,

		/// <summary>
		/// Represents the function key F5.
		/// </summary>
		F5 = 57397,

		/// <summary>
		/// Represents the function key F6.
		/// </summary>
		F6 = 57398,

		/// <summary>
		/// Represents the function key F7.
		/// </summary>
		F7 = 57399,

		/// <summary>
		/// Represents the function key F8.
		/// </summary>
		F8 = 57400,

		/// <summary>
		/// Represents the function key F9.
		/// </summary>
		F9 = 57401,

		/// <summary>
		/// Represents the function key F10.
		/// </summary>
		F10 = 57402,

		/// <summary>
		/// Represents the function key F11.
		/// </summary>
		F11 = 57403,

		/// <summary>
		/// Represents the function key F12.
		/// </summary>
		F12 = 57404,

		/// <summary>
		/// Represents the Shift key. This is a control key and it can be combined with other keys. E.g. Keys.Enter | Keys.Shift
		/// </summary>
		Shift = 1 << 16,

		/// <summary>
		/// Represents the Control key. This is a control key and it can be combined with other keys. E.g. Keys.Enter | Keys.Control
		/// </summary>
		Control = 1 << 17,

		/// <summary>
		/// Represents the Alt key. This is a control key and it can be combined with other keys. E.g. Keys.Enter | Keys.Alt
		/// </summary>
		Alt = 1 << 18,

		/// <summary>
		/// Represents the function key Command. This is a control key and it can be combined with other keys. E.g. Keys.Enter | Keys.Command
		/// </summary>
		Command = 1 << 19
	}
}
