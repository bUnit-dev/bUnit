using AngleSharp.Dom;

namespace Bunit;

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
	public static void KeyDown(this IElement element)
		=> _ = KeyDownAsync(element, new KeyboardEventArgs());

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
	public static Task KeyDownAsync(this IElement element, KeyboardEventArgs eventArgs) => element.TriggerEventAsync("onkeydown", eventArgs);

	/// <summary>
	/// Raises the <c>@onkeyup</c> event on <paramref name="element"/>,  passing the provided
	/// properties to the event handler via a <see cref="KeyboardEventArgs"/> object.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	public static void KeyUp(this IElement element)
		=> _ = KeyUpAsync(element, new KeyboardEventArgs());

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
	public static Task KeyUpAsync(this IElement element, KeyboardEventArgs eventArgs) => element.TriggerEventAsync("onkeyup", eventArgs);

	/// <summary>
	/// Raises the <c>@onkeypress</c> event on <paramref name="element"/>,  passing the provided
	/// properties to the event handler via a <see cref="KeyboardEventArgs"/> object.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	public static void KeyPress(this IElement element)
		=> _ = KeyPressAsync(element, new KeyboardEventArgs());

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
	public static Task KeyPressAsync(this IElement element, KeyboardEventArgs eventArgs) => element.TriggerEventAsync("onkeypress", eventArgs);
}