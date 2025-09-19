using AngleSharp.Dom;

namespace Bunit;

/// <summary>
/// Keyboard event dispatch helper extension methods.
/// </summary>
public static partial class EventHandlerDispatchExtensions
{
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
	/// Raises the <c>@onkeydown</c> event on <paramref name="element"/>, passing the provided <paramref name="key"/>
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <param name="key">The keyboard key to pass to the event handler.</param>
	/// <param name="repeat"><c>true</c> if a key has been depressed long enough to trigger key repetition, otherwise <c>false</c>.</param>
	/// <param name="type">The type of the event.</param>
	public static Task KeyDownAsync(this IElement element, Key key, bool repeat = default, string? type = default)
	{
		KeyboardEventArgs eventArgs = key;
		eventArgs.Repeat = repeat;
		eventArgs.Type = type!; // Type property missing annotation
		return KeyDownAsync(element, eventArgs);
	}

	/// <summary>
	/// Raises the <c>@onkeydown</c> event on <paramref name="elementTask"/>, passing the provided <paramref name="key"/>
	/// to the event handler.
	/// </summary>
	/// <param name="elementTask">The element to raise the event on.</param>
	/// <param name="key">The keyboard key to pass to the event handler.</param>
	/// <param name="repeat"><c>true</c> if a key has been depressed long enough to trigger key repetition, otherwise <c>false</c>.</param>
	/// <param name="type">The type of the event.</param>
	public static async Task KeyDownAsync(this Task<IElement> elementTask, Key key, bool repeat = default, string? type = default)
	{
		var element = await elementTask;
		await KeyDownAsync(element, key, repeat, type);
	}

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
	/// Raises the <c>@onkeyup</c> event on <paramref name="element"/>, passing the provided <paramref name="key"/>
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <param name="key">The keyboard key to pass to the event handler.</param>
	/// <param name="repeat"><c>true</c> if a key has been depressed long enough to trigger key repetition, otherwise <c>false</c>.</param>
	/// <param name="type">The type of the event.</param>
	public static Task KeyUpAsync(this IElement element, Key key, bool repeat = default, string? type = default)
	{
		KeyboardEventArgs eventArgs = key;
		eventArgs.Repeat = repeat;
		eventArgs.Type = type!; // Type property missing annotation
		return KeyUpAsync(element, eventArgs);
	}

	/// <summary>
	/// Raises the <c>@onkeyup</c> event on <paramref name="elementTask"/>, passing the provided <paramref name="key"/>
	/// to the event handler.
	/// </summary>
	/// <param name="elementTask">The element to raise the event on.</param>
	/// <param name="key">The keyboard key to pass to the event handler.</param>
	/// <param name="repeat"><c>true</c> if a key has been depressed long enough to trigger key repetition, otherwise <c>false</c>.</param>
	/// <param name="type">The type of the event.</param>
	public static async Task KeyUpAsync(this Task<IElement> elementTask, Key key, bool repeat = default, string? type = default)
	{
		var element = await elementTask;
		await KeyUpAsync(element, key, repeat, type);
	}

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
	/// Raises the <c>@onkeypress</c> event on <paramref name="element"/>, passing the provided <paramref name="key"/>
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <param name="key">The keyboard key to pass to the event handler.</param>
	/// <param name="repeat"><c>true</c> if a key has been depressed long enough to trigger key repetition, otherwise <c>false</c>.</param>
	/// <param name="type">The type of the event.</param>
	public static Task KeyPressAsync(this IElement element, Key key, bool repeat = default, string? type = default)
	{
		KeyboardEventArgs eventArgs = key;
		eventArgs.Repeat = repeat;
		eventArgs.Type = type!; // Type property missing annotation
		return KeyPressAsync(element, eventArgs);
	}

	/// <summary>
	/// Raises the <c>@onkeypress</c> event on <paramref name="elementTask"/>, passing the provided <paramref name="key"/>
	/// to the event handler.
	/// </summary>
	/// <param name="elementTask">The element to raise the event on.</param>
	/// <param name="key">The keyboard key to pass to the event handler.</param>
	/// <param name="repeat"><c>true</c> if a key has been depressed long enough to trigger key repetition, otherwise <c>false</c>.</param>
	/// <param name="type">The type of the event.</param>
	public static async Task KeyPressAsync(this Task<IElement> elementTask, Key key, bool repeat = default, string? type = default)
	{
		var element = await elementTask;
		await KeyPressAsync(element, key, repeat, type);
	}
}
