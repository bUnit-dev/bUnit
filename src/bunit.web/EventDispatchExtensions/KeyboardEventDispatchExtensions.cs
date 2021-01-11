using System.Threading.Tasks;
using AngleSharp.Dom;
using Microsoft.AspNetCore.Components.Web;

namespace Bunit
{
	/// <summary>
	/// Keyboard event dispatch helper extension methods.
	/// </summary>
	public static class KeyboardEventDispatchExtensions
	{
		/// <summary>
		/// Raises the <c>@onkeydown</c> event on <paramref name="element"/>,  passing the provided
		/// properties to the event handler via a <see cref="KeyboardEventArgs"/> object.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		/// <param name="key">
		///     The key value of the key represented by the event. If the value has a printed
		///     representation, this attribute's value is the same as the char attribute. Otherwise,
		///     it's one of the key value strings specified in 'Key values'. If the key can't
		///     be identified, this is the string "Unidentified".
		/// </param>
		/// <param name="code">
		///     Holds a string that identifies the physical key being pressed. The value is not
		///     affected by the current keyboard layout or modifier state, so a particular key
		///     will always return the same value.
		/// </param>
		/// <param name="location">The location of the key on the device.</param>
		/// <param name="repeat">true if a key has been depressed long enough to trigger key repetition, otherwise false.</param>
		/// <param name="ctrlKey">true if the control key was down when the event was fired. false otherwise.</param>
		/// <param name="shiftKey">true if the shift key was down when the event was fired. false otherwise.</param>
		/// <param name="altKey">true if the alt key was down when the event was fired. false otherwise.</param>
		/// <param name="metaKey">true if the meta key was down when the event was fired. false otherwise.</param>
		/// <param name="type">The type of the event.</param>
		public static void KeyDown(this IElement element, string key, string? code = default, float location = default, bool repeat = default, bool ctrlKey = default, bool shiftKey = default, bool altKey = default, bool metaKey = default, string? type = default)
			=> _ = KeyDownAsync(element, new KeyboardEventArgs { Key = key, Code = code!, Location = location, Repeat = repeat, CtrlKey = ctrlKey, ShiftKey = shiftKey, AltKey = altKey, MetaKey = metaKey, Type = type! });

		/// <summary>
		/// Raises the <c>@onkeydown</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
		public static void KeyDown(this IElement element, KeyboardEventArgs eventArgs) => _ = KeyDownAsync(element, eventArgs);

		/// <summary>
		/// Raises the <c>@onkeydown</c> event on <paramref name="element"/>, passing the provided <paramref name="key"/>
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		/// <param name="key">The keyboard key to pass to the event handler.</param>
		/// <param name="repeat"><c>true</c> if a key has been depressed long enough to trigger key repetition, otherwise <c>false</c>.</param>
		/// <param name="type">The type of the event.</param>
		public static void KeyDown(this IElement element, Key key, bool repeat = default, string? type = default)
		{
			KeyboardEventArgs eventArgs = key;
			eventArgs.Repeat = repeat;
			eventArgs.Type = type!; // Type property missing annotation
			_ = KeyDownAsync(element, eventArgs);
		}

		/// <summary>
		/// Raises the <c>@onkeydown</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
		/// <returns>A task that completes when the event handler is done.</returns>
		private static Task KeyDownAsync(this IElement element, KeyboardEventArgs eventArgs) => element.TriggerEventAsync("onkeydown", eventArgs);

		/// <summary>
		/// Raises the <c>@onkeyup</c> event on <paramref name="element"/>,  passing the provided
		/// properties to the event handler via a <see cref="KeyboardEventArgs"/> object.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		/// <param name="key">
		///     The key value of the key represented by the event. If the value has a printed
		///     representation, this attribute's value is the same as the char attribute. Otherwise,
		///     it's one of the key value strings specified in 'Key values'. If the key can't
		///     be identified, this is the string "Unidentified".
		/// </param>
		/// <param name="code">
		///     Holds a string that identifies the physical key being pressed. The value is not
		///     affected by the current keyboard layout or modifier state, so a particular key
		///     will always return the same value.
		/// </param>
		/// <param name="location">The location of the key on the device.</param>
		/// <param name="repeat">true if a key has been depressed long enough to trigger key repetition, otherwise false.</param>
		/// <param name="ctrlKey">true if the control key was down when the event was fired. false otherwise.</param>
		/// <param name="shiftKey">true if the shift key was down when the event was fired. false otherwise.</param>
		/// <param name="altKey">true if the alt key was down when the event was fired. false otherwise.</param>
		/// <param name="metaKey">true if the meta key was down when the event was fired. false otherwise.</param>
		/// <param name="type">The type of the event.</param>
		public static void KeyUp(this IElement element, string key, string? code = default, float location = default, bool repeat = default, bool ctrlKey = default, bool shiftKey = default, bool altKey = default, bool metaKey = default, string? type = default)
			=> _ = KeyUpAsync(element, new KeyboardEventArgs { Key = key, Code = code!, Location = location, Repeat = repeat, CtrlKey = ctrlKey, ShiftKey = shiftKey, AltKey = altKey, MetaKey = metaKey, Type = type! });

		/// <summary>
		/// Raises the <c>@onkeyup</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
		public static void KeyUp(this IElement element, KeyboardEventArgs eventArgs) => _ = KeyUpAsync(element, eventArgs);

		/// <summary>
		/// Raises the <c>@onkeyup</c> event on <paramref name="element"/>, passing the provided <paramref name="key"/>
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		/// <param name="key">The keyboard key to pass to the event handler.</param>
		/// <param name="repeat"><c>true</c> if a key has been depressed long enough to trigger key repetition, otherwise <c>false</c>.</param>
		/// <param name="type">The type of the event.</param>
		public static void KeyUp(this IElement element, Key key, bool repeat = default, string? type = default)
		{
			KeyboardEventArgs eventArgs = key;
			eventArgs.Repeat = repeat;
			eventArgs.Type = type!; // Type property missing annotation
			_ = KeyUpAsync(element, eventArgs);
		}

		/// <summary>
		/// Raises the <c>@onkeyup</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
		/// <returns>A task that completes when the event handler is done.</returns>
		private static Task KeyUpAsync(this IElement element, KeyboardEventArgs eventArgs) => element.TriggerEventAsync("onkeyup", eventArgs);

		/// <summary>
		/// Raises the <c>@onkeypress</c> event on <paramref name="element"/>,  passing the provided
		/// properties to the event handler via a <see cref="KeyboardEventArgs"/> object.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		/// <param name="key">
		///     The key value of the key represented by the event. If the value has a printed
		///     representation, this attribute's value is the same as the char attribute. Otherwise,
		///     it's one of the key value strings specified in 'Key values'. If the key can't
		///     be identified, this is the string "Unidentified".
		/// </param>
		/// <param name="code">
		///     Holds a string that identifies the physical key being pressed. The value is not
		///     affected by the current keyboard layout or modifier state, so a particular key
		///     will always return the same value.
		/// </param>
		/// <param name="location">The location of the key on the device.</param>
		/// <param name="repeat">true if a key has been depressed long enough to trigger key repetition, otherwise false.</param>
		/// <param name="ctrlKey">true if the control key was down when the event was fired. false otherwise.</param>
		/// <param name="shiftKey">true if the shift key was down when the event was fired. false otherwise.</param>
		/// <param name="altKey">true if the alt key was down when the event was fired. false otherwise.</param>
		/// <param name="metaKey">true if the meta key was down when the event was fired. false otherwise.</param>
		/// <param name="type">The type of the event.</param>
		public static void KeyPress(this IElement element, string key, string? code = default, float location = default, bool repeat = default, bool ctrlKey = default, bool shiftKey = default, bool altKey = default, bool metaKey = default, string? type = default)
			=> _ = KeyPressAsync(element, new KeyboardEventArgs { Key = key, Code = code!, Location = location, Repeat = repeat, CtrlKey = ctrlKey, ShiftKey = shiftKey, AltKey = altKey, MetaKey = metaKey, Type = type! });

		/// <summary>
		/// Raises the <c>@onkeypress</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
		public static void KeyPress(this IElement element, KeyboardEventArgs eventArgs) => _ = KeyPressAsync(element, eventArgs);

		/// <summary>
		/// Raises the <c>@onkeypress</c> event on <paramref name="element"/>, passing the provided <paramref name="key"/>
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		/// <param name="key">The keyboard key to pass to the event handler.</param>
		/// <param name="repeat"><c>true</c> if a key has been depressed long enough to trigger key repetition, otherwise <c>false</c>.</param>
		/// <param name="type">The type of the event.</param>
		public static void KeyPress(this IElement element, Key key, bool repeat = default, string? type = default)
		{
			KeyboardEventArgs eventArgs = key;
			eventArgs.Repeat = repeat;
			eventArgs.Type = type!; // Type property missing annotation
			_ = KeyPressAsync(element, eventArgs);
		}

		/// <summary>
		/// Raises the <c>@onkeypress</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
		/// <returns>A task that completes when the event handler is done.</returns>
		private static Task KeyPressAsync(this IElement element, KeyboardEventArgs eventArgs) => element.TriggerEventAsync("onkeypress", eventArgs);
	}
}
