using AngleSharp.Dom;

namespace Bunit;

/// <summary>
/// Mouse event dispatch helper extension methods.
/// </summary>
public static class MouseEventDispatchExtensions
{
	/// <summary>
	/// Raises the <c>@onmouseover</c> event on <paramref name="element"/>,  passing the provided
	/// properties to the event handler via a <see cref="MouseEventArgs"/> object.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <param name="detail">A count of consecutive clicks that happened in a short amount of time, incremented by one.</param>
	/// <param name="screenX">The X coordinate of the mouse pointer in global (screen) coordinates.</param>
	/// <param name="screenY">The Y coordinate of the mouse pointer in global (screen) coordinates.</param>
	/// <param name="clientX">The X coordinate of the mouse pointer in local (DOM content) coordinates.</param>
	/// <param name="clientY">The Y coordinate of the mouse pointer in local (DOM content) coordinates.</param>
	/// <param name="button">
	///     The button number that was pressed when the mouse event was fired: Left button=0,
	///     middle button=1 (if present), right button=2. For mice configured for left handed
	///     use in which the button actions are reversed the values are instead read from
	///     right to left.
	/// </param>
	/// <param name="buttons">
	///     The buttons being pressed when the mouse event was fired: Left button=1, Right
	///     button=2, Middle (wheel) button=4, 4th button (typically, "Browser Back" button)=8,
	///     5th button (typically, "Browser Forward" button)=16. If two or more buttons are
	///     pressed, returns the logical sum of the values. E.g., if Left button and Right
	///     button are pressed, returns 3 (=1 | 2).
	/// </param>
	/// <param name="ctrlKey">true if the control key was down when the event was fired. false otherwise.</param>
	/// <param name="shiftKey">true if the shift key was down when the event was fired. false otherwise.</param>
	/// <param name="altKey">true if the alt key was down when the event was fired. false otherwise.</param>
	/// <param name="metaKey">true if the meta key was down when the event was fired. false otherwise.</param>
	/// <param name="type">Gets or sets the type of the event.</param>
	public static void MouseOver(this IElement element, long detail = default, double screenX = default, double screenY = default, double clientX = default, double clientY = default, long button = default, long buttons = default, bool ctrlKey = default, bool shiftKey = default, bool altKey = default, bool metaKey = default, string? type = default)
		=> _ = MouseOverAsync(element, new MouseEventArgs { Detail = detail, ScreenX = screenX, ScreenY = screenY, ClientX = clientX, ClientY = clientY, Button = button, Buttons = buttons, CtrlKey = ctrlKey, ShiftKey = shiftKey, AltKey = altKey, MetaKey = metaKey, Type = type! });

	/// <summary>
	/// Raises the <c>@onmouseover</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
	public static void MouseOver(this IElement element, MouseEventArgs eventArgs)
		=> _ = MouseOverAsync(element, eventArgs);

	/// <summary>
	/// Raises the <c>@onmouseover</c> event on <paramref name="element"/>,  passing the provided
	/// properties to the event handler via a <see cref="MouseEventArgs"/> object.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <param name="detail">A count of consecutive clicks that happened in a short amount of time, incremented by one.</param>
	/// <param name="screenX">The X coordinate of the mouse pointer in global (screen) coordinates.</param>
	/// <param name="screenY">The Y coordinate of the mouse pointer in global (screen) coordinates.</param>
	/// <param name="clientX">The X coordinate of the mouse pointer in local (DOM content) coordinates.</param>
	/// <param name="clientY">The Y coordinate of the mouse pointer in local (DOM content) coordinates.</param>
	/// <param name="button">
	///     The button number that was pressed when the mouse event was fired: Left button=0,
	///     middle button=1 (if present), right button=2. For mice configured for left handed
	///     use in which the button actions are reversed the values are instead read from
	///     right to left.
	/// </param>
	/// <param name="buttons">
	///     The buttons being pressed when the mouse event was fired: Left button=1, Right
	///     button=2, Middle (wheel) button=4, 4th button (typically, "Browser Back" button)=8,
	///     5th button (typically, "Browser Forward" button)=16. If two or more buttons are
	///     pressed, returns the logical sum of the values. E.g., if Left button and Right
	///     button are pressed, returns 3 (=1 | 2).
	/// </param>
	/// <param name="ctrlKey">true if the control key was down when the event was fired. false otherwise.</param>
	/// <param name="shiftKey">true if the shift key was down when the event was fired. false otherwise.</param>
	/// <param name="altKey">true if the alt key was down when the event was fired. false otherwise.</param>
	/// <param name="metaKey">true if the meta key was down when the event was fired. false otherwise.</param>
	/// <param name="type">Gets or sets the type of the event.</param>
	public static Task MouseOverAsync(this IElement element, long detail = default, double screenX = default, double screenY = default, double clientX = default, double clientY = default, long button = default, long buttons = default, bool ctrlKey = default, bool shiftKey = default, bool altKey = default, bool metaKey = default, string? type = default)
		=> MouseOverAsync(element, new MouseEventArgs { Detail = detail, ScreenX = screenX, ScreenY = screenY, ClientX = clientX, ClientY = clientY, Button = button, Buttons = buttons, CtrlKey = ctrlKey, ShiftKey = shiftKey, AltKey = altKey, MetaKey = metaKey, Type = type! });

	/// <summary>
	/// Raises the <c>@onmouseover</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
	/// <returns>A task that completes when the event handler is done.</returns>
	public static Task MouseOverAsync(this IElement element, MouseEventArgs eventArgs) => element.TriggerEventAsync("onmouseover", eventArgs);

	/// <summary>
	/// Raises the <c>@onmouseout</c> event on <paramref name="element"/>,  passing the provided
	/// properties to the event handler via a <see cref="MouseEventArgs"/> object.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <param name="detail">A count of consecutive clicks that happened in a short amount of time, incremented by one.</param>
	/// <param name="screenX">The X coordinate of the mouse pointer in global (screen) coordinates.</param>
	/// <param name="screenY">The Y coordinate of the mouse pointer in global (screen) coordinates.</param>
	/// <param name="clientX">The X coordinate of the mouse pointer in local (DOM content) coordinates.</param>
	/// <param name="clientY">The Y coordinate of the mouse pointer in local (DOM content) coordinates.</param>
	/// <param name="button">
	///     The button number that was pressed when the mouse event was fired: Left button=0,
	///     middle button=1 (if present), right button=2. For mice configured for left handed
	///     use in which the button actions are reversed the values are instead read from
	///     right to left.
	/// </param>
	/// <param name="buttons">
	///     The buttons being pressed when the mouse event was fired: Left button=1, Right
	///     button=2, Middle (wheel) button=4, 4th button (typically, "Browser Back" button)=8,
	///     5th button (typically, "Browser Forward" button)=16. If two or more buttons are
	///     pressed, returns the logical sum of the values. E.g., if Left button and Right
	///     button are pressed, returns 3 (=1 | 2).
	/// </param>
	/// <param name="ctrlKey">true if the control key was down when the event was fired. false otherwise.</param>
	/// <param name="shiftKey">true if the shift key was down when the event was fired. false otherwise.</param>
	/// <param name="altKey">true if the alt key was down when the event was fired. false otherwise.</param>
	/// <param name="metaKey">true if the meta key was down when the event was fired. false otherwise.</param>
	/// <param name="type">Gets or sets the type of the event.</param>
	public static void MouseOut(this IElement element, long detail = default, double screenX = default, double screenY = default, double clientX = default, double clientY = default, long button = default, long buttons = default, bool ctrlKey = default, bool shiftKey = default, bool altKey = default, bool metaKey = default, string? type = default)
		=> _ = MouseOutAsync(element, new MouseEventArgs { Detail = detail, ScreenX = screenX, ScreenY = screenY, ClientX = clientX, ClientY = clientY, Button = button, Buttons = buttons, CtrlKey = ctrlKey, ShiftKey = shiftKey, AltKey = altKey, MetaKey = metaKey, Type = type! });

	/// <summary>
	/// Raises the <c>@onmouseout</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
	public static void MouseOut(this IElement element, MouseEventArgs eventArgs)
		=> _ = MouseOutAsync(element, eventArgs);

	/// <summary>
	/// Raises the <c>@onmouseout</c> event on <paramref name="element"/>,  passing the provided
	/// properties to the event handler via a <see cref="MouseEventArgs"/> object.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <param name="detail">A count of consecutive clicks that happened in a short amount of time, incremented by one.</param>
	/// <param name="screenX">The X coordinate of the mouse pointer in global (screen) coordinates.</param>
	/// <param name="screenY">The Y coordinate of the mouse pointer in global (screen) coordinates.</param>
	/// <param name="clientX">The X coordinate of the mouse pointer in local (DOM content) coordinates.</param>
	/// <param name="clientY">The Y coordinate of the mouse pointer in local (DOM content) coordinates.</param>
	/// <param name="button">
	///     The button number that was pressed when the mouse event was fired: Left button=0,
	///     middle button=1 (if present), right button=2. For mice configured for left handed
	///     use in which the button actions are reversed the values are instead read from
	///     right to left.
	/// </param>
	/// <param name="buttons">
	///     The buttons being pressed when the mouse event was fired: Left button=1, Right
	///     button=2, Middle (wheel) button=4, 4th button (typically, "Browser Back" button)=8,
	///     5th button (typically, "Browser Forward" button)=16. If two or more buttons are
	///     pressed, returns the logical sum of the values. E.g., if Left button and Right
	///     button are pressed, returns 3 (=1 | 2).
	/// </param>
	/// <param name="ctrlKey">true if the control key was down when the event was fired. false otherwise.</param>
	/// <param name="shiftKey">true if the shift key was down when the event was fired. false otherwise.</param>
	/// <param name="altKey">true if the alt key was down when the event was fired. false otherwise.</param>
	/// <param name="metaKey">true if the meta key was down when the event was fired. false otherwise.</param>
	/// <param name="type">Gets or sets the type of the event.</param>
	public static Task MouseOutAsync(this IElement element, long detail = default, double screenX = default, double screenY = default, double clientX = default, double clientY = default, long button = default, long buttons = default, bool ctrlKey = default, bool shiftKey = default, bool altKey = default, bool metaKey = default, string? type = default)
		=> MouseOutAsync(element, new MouseEventArgs { Detail = detail, ScreenX = screenX, ScreenY = screenY, ClientX = clientX, ClientY = clientY, Button = button, Buttons = buttons, CtrlKey = ctrlKey, ShiftKey = shiftKey, AltKey = altKey, MetaKey = metaKey, Type = type! });

	/// <summary>
	/// Raises the <c>@onmouseout</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
	/// <returns>A task that completes when the event handler is done.</returns>
	public static Task MouseOutAsync(this IElement element, MouseEventArgs eventArgs) => element.TriggerEventAsync("onmouseout", eventArgs);

	/// <summary>
	/// Raises the <c>@onmousemove</c> event on <paramref name="element"/>,  passing the provided
	/// properties to the event handler via a <see cref="MouseEventArgs"/> object.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <param name="detail">A count of consecutive clicks that happened in a short amount of time, incremented by one.</param>
	/// <param name="screenX">The X coordinate of the mouse pointer in global (screen) coordinates.</param>
	/// <param name="screenY">The Y coordinate of the mouse pointer in global (screen) coordinates.</param>
	/// <param name="clientX">The X coordinate of the mouse pointer in local (DOM content) coordinates.</param>
	/// <param name="clientY">The Y coordinate of the mouse pointer in local (DOM content) coordinates.</param>
	/// <param name="button">
	///     The button number that was pressed when the mouse event was fired: Left button=0,
	///     middle button=1 (if present), right button=2. For mice configured for left handed
	///     use in which the button actions are reversed the values are instead read from
	///     right to left.
	/// </param>
	/// <param name="buttons">
	///     The buttons being pressed when the mouse event was fired: Left button=1, Right
	///     button=2, Middle (wheel) button=4, 4th button (typically, "Browser Back" button)=8,
	///     5th button (typically, "Browser Forward" button)=16. If two or more buttons are
	///     pressed, returns the logical sum of the values. E.g., if Left button and Right
	///     button are pressed, returns 3 (=1 | 2).
	/// </param>
	/// <param name="ctrlKey">true if the control key was down when the event was fired. false otherwise.</param>
	/// <param name="shiftKey">true if the shift key was down when the event was fired. false otherwise.</param>
	/// <param name="altKey">true if the alt key was down when the event was fired. false otherwise.</param>
	/// <param name="metaKey">true if the meta key was down when the event was fired. false otherwise.</param>
	/// <param name="type">Gets or sets the type of the event.</param>
	public static void MouseMove(this IElement element, long detail = default, double screenX = default, double screenY = default, double clientX = default, double clientY = default, long button = default, long buttons = default, bool ctrlKey = default, bool shiftKey = default, bool altKey = default, bool metaKey = default, string? type = default)
		=> _ = MouseMoveAsync(element, new MouseEventArgs { Detail = detail, ScreenX = screenX, ScreenY = screenY, ClientX = clientX, ClientY = clientY, Button = button, Buttons = buttons, CtrlKey = ctrlKey, ShiftKey = shiftKey, AltKey = altKey, MetaKey = metaKey, Type = type! });

	/// <summary>
	/// Raises the <c>@onmousemove</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
	public static void MouseMove(this IElement element, MouseEventArgs eventArgs)
		=> _ = MouseMoveAsync(element, eventArgs);

	/// <summary>
	/// Raises the <c>@onmousemove</c> event on <paramref name="element"/>,  passing the provided
	/// properties to the event handler via a <see cref="MouseEventArgs"/> object.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <param name="detail">A count of consecutive clicks that happened in a short amount of time, incremented by one.</param>
	/// <param name="screenX">The X coordinate of the mouse pointer in global (screen) coordinates.</param>
	/// <param name="screenY">The Y coordinate of the mouse pointer in global (screen) coordinates.</param>
	/// <param name="clientX">The X coordinate of the mouse pointer in local (DOM content) coordinates.</param>
	/// <param name="clientY">The Y coordinate of the mouse pointer in local (DOM content) coordinates.</param>
	/// <param name="button">
	///     The button number that was pressed when the mouse event was fired: Left button=0,
	///     middle button=1 (if present), right button=2. For mice configured for left handed
	///     use in which the button actions are reversed the values are instead read from
	///     right to left.
	/// </param>
	/// <param name="buttons">
	///     The buttons being pressed when the mouse event was fired: Left button=1, Right
	///     button=2, Middle (wheel) button=4, 4th button (typically, "Browser Back" button)=8,
	///     5th button (typically, "Browser Forward" button)=16. If two or more buttons are
	///     pressed, returns the logical sum of the values. E.g., if Left button and Right
	///     button are pressed, returns 3 (=1 | 2).
	/// </param>
	/// <param name="ctrlKey">true if the control key was down when the event was fired. false otherwise.</param>
	/// <param name="shiftKey">true if the shift key was down when the event was fired. false otherwise.</param>
	/// <param name="altKey">true if the alt key was down when the event was fired. false otherwise.</param>
	/// <param name="metaKey">true if the meta key was down when the event was fired. false otherwise.</param>
	/// <param name="type">Gets or sets the type of the event.</param>
	public static Task MouseMoveAsync(this IElement element, long detail = default, double screenX = default, double screenY = default, double clientX = default, double clientY = default, long button = default, long buttons = default, bool ctrlKey = default, bool shiftKey = default, bool altKey = default, bool metaKey = default, string? type = default)
		=> MouseMoveAsync(element, new MouseEventArgs { Detail = detail, ScreenX = screenX, ScreenY = screenY, ClientX = clientX, ClientY = clientY, Button = button, Buttons = buttons, CtrlKey = ctrlKey, ShiftKey = shiftKey, AltKey = altKey, MetaKey = metaKey, Type = type! });

	/// <summary>
	/// Raises the <c>@onmousemove</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
	/// <returns>A task that completes when the event handler is done.</returns>
	public static Task MouseMoveAsync(this IElement element, MouseEventArgs eventArgs) => element.TriggerEventAsync("onmousemove", eventArgs);

	/// <summary>
	/// Raises the <c>@onmousedown</c> event on <paramref name="element"/>,  passing the provided
	/// properties to the event handler via a <see cref="MouseEventArgs"/> object.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <param name="detail">A count of consecutive clicks that happened in a short amount of time, incremented by one.</param>
	/// <param name="screenX">The X coordinate of the mouse pointer in global (screen) coordinates.</param>
	/// <param name="screenY">The Y coordinate of the mouse pointer in global (screen) coordinates.</param>
	/// <param name="clientX">The X coordinate of the mouse pointer in local (DOM content) coordinates.</param>
	/// <param name="clientY">The Y coordinate of the mouse pointer in local (DOM content) coordinates.</param>
	/// <param name="button">
	///     The button number that was pressed when the mouse event was fired: Left button=0,
	///     middle button=1 (if present), right button=2. For mice configured for left handed
	///     use in which the button actions are reversed the values are instead read from
	///     right to left.
	/// </param>
	/// <param name="buttons">
	///     The buttons being pressed when the mouse event was fired: Left button=1, Right
	///     button=2, Middle (wheel) button=4, 4th button (typically, "Browser Back" button)=8,
	///     5th button (typically, "Browser Forward" button)=16. If two or more buttons are
	///     pressed, returns the logical sum of the values. E.g., if Left button and Right
	///     button are pressed, returns 3 (=1 | 2).
	/// </param>
	/// <param name="ctrlKey">true if the control key was down when the event was fired. false otherwise.</param>
	/// <param name="shiftKey">true if the shift key was down when the event was fired. false otherwise.</param>
	/// <param name="altKey">true if the alt key was down when the event was fired. false otherwise.</param>
	/// <param name="metaKey">true if the meta key was down when the event was fired. false otherwise.</param>
	/// <param name="type">Gets or sets the type of the event.</param>
	public static void MouseDown(this IElement element, long detail = default, double screenX = default, double screenY = default, double clientX = default, double clientY = default, long button = default, long buttons = default, bool ctrlKey = default, bool shiftKey = default, bool altKey = default, bool metaKey = default, string? type = default)
		=> _ = MouseDownAsync(element, new MouseEventArgs { Detail = detail, ScreenX = screenX, ScreenY = screenY, ClientX = clientX, ClientY = clientY, Button = button, Buttons = buttons, CtrlKey = ctrlKey, ShiftKey = shiftKey, AltKey = altKey, MetaKey = metaKey, Type = type! });

	/// <summary>
	/// Raises the <c>@onmousedown</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
	public static void MouseDown(this IElement element, MouseEventArgs eventArgs)
		=> _ = MouseDownAsync(element, eventArgs);

	/// <summary>
	/// Raises the <c>@onmousedown</c> event on <paramref name="element"/>,  passing the provided
	/// properties to the event handler via a <see cref="MouseEventArgs"/> object.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <param name="detail">A count of consecutive clicks that happened in a short amount of time, incremented by one.</param>
	/// <param name="screenX">The X coordinate of the mouse pointer in global (screen) coordinates.</param>
	/// <param name="screenY">The Y coordinate of the mouse pointer in global (screen) coordinates.</param>
	/// <param name="clientX">The X coordinate of the mouse pointer in local (DOM content) coordinates.</param>
	/// <param name="clientY">The Y coordinate of the mouse pointer in local (DOM content) coordinates.</param>
	/// <param name="button">
	///     The button number that was pressed when the mouse event was fired: Left button=0,
	///     middle button=1 (if present), right button=2. For mice configured for left handed
	///     use in which the button actions are reversed the values are instead read from
	///     right to left.
	/// </param>
	/// <param name="buttons">
	///     The buttons being pressed when the mouse event was fired: Left button=1, Right
	///     button=2, Middle (wheel) button=4, 4th button (typically, "Browser Back" button)=8,
	///     5th button (typically, "Browser Forward" button)=16. If two or more buttons are
	///     pressed, returns the logical sum of the values. E.g., if Left button and Right
	///     button are pressed, returns 3 (=1 | 2).
	/// </param>
	/// <param name="ctrlKey">true if the control key was down when the event was fired. false otherwise.</param>
	/// <param name="shiftKey">true if the shift key was down when the event was fired. false otherwise.</param>
	/// <param name="altKey">true if the alt key was down when the event was fired. false otherwise.</param>
	/// <param name="metaKey">true if the meta key was down when the event was fired. false otherwise.</param>
	/// <param name="type">Gets or sets the type of the event.</param>
	public static Task MouseDownAsync(this IElement element, long detail = default, double screenX = default, double screenY = default, double clientX = default, double clientY = default, long button = default, long buttons = default, bool ctrlKey = default, bool shiftKey = default, bool altKey = default, bool metaKey = default, string? type = default)
		=> MouseDownAsync(element, new MouseEventArgs { Detail = detail, ScreenX = screenX, ScreenY = screenY, ClientX = clientX, ClientY = clientY, Button = button, Buttons = buttons, CtrlKey = ctrlKey, ShiftKey = shiftKey, AltKey = altKey, MetaKey = metaKey, Type = type! });

	/// <summary>
	/// Raises the <c>@onmousedown</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
	/// <returns>A task that completes when the event handler is done.</returns>
	public static Task MouseDownAsync(this IElement element, MouseEventArgs eventArgs) => element.TriggerEventAsync("onmousedown", eventArgs);

	/// <summary>
	/// Raises the <c>@onmouseup</c> event on <paramref name="element"/>,  passing the provided
	/// properties to the event handler via a <see cref="MouseEventArgs"/> object.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <param name="detail">A count of consecutive clicks that happened in a short amount of time, incremented by one.</param>
	/// <param name="screenX">The X coordinate of the mouse pointer in global (screen) coordinates.</param>
	/// <param name="screenY">The Y coordinate of the mouse pointer in global (screen) coordinates.</param>
	/// <param name="clientX">The X coordinate of the mouse pointer in local (DOM content) coordinates.</param>
	/// <param name="clientY">The Y coordinate of the mouse pointer in local (DOM content) coordinates.</param>
	/// <param name="button">
	///     The button number that was pressed when the mouse event was fired: Left button=0,
	///     middle button=1 (if present), right button=2. For mice configured for left handed
	///     use in which the button actions are reversed the values are instead read from
	///     right to left.
	/// </param>
	/// <param name="buttons">
	///     The buttons being pressed when the mouse event was fired: Left button=1, Right
	///     button=2, Middle (wheel) button=4, 4th button (typically, "Browser Back" button)=8,
	///     5th button (typically, "Browser Forward" button)=16. If two or more buttons are
	///     pressed, returns the logical sum of the values. E.g., if Left button and Right
	///     button are pressed, returns 3 (=1 | 2).
	/// </param>
	/// <param name="ctrlKey">true if the control key was down when the event was fired. false otherwise.</param>
	/// <param name="shiftKey">true if the shift key was down when the event was fired. false otherwise.</param>
	/// <param name="altKey">true if the alt key was down when the event was fired. false otherwise.</param>
	/// <param name="metaKey">true if the meta key was down when the event was fired. false otherwise.</param>
	/// <param name="type">Gets or sets the type of the event.</param>
	public static void MouseUp(this IElement element, long detail = default, double screenX = default, double screenY = default, double clientX = default, double clientY = default, long button = default, long buttons = default, bool ctrlKey = default, bool shiftKey = default, bool altKey = default, bool metaKey = default, string? type = default)
		=> _ = MouseUpAsync(element, new MouseEventArgs { Detail = detail, ScreenX = screenX, ScreenY = screenY, ClientX = clientX, ClientY = clientY, Button = button, Buttons = buttons, CtrlKey = ctrlKey, ShiftKey = shiftKey, AltKey = altKey, MetaKey = metaKey, Type = type! });

	/// <summary>
	/// Raises the <c>@onmouseup</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
	public static void MouseUp(this IElement element, MouseEventArgs eventArgs)
		=> _ = MouseUpAsync(element, eventArgs);

	/// <summary>
	/// Raises the <c>@onmouseup</c> event on <paramref name="element"/>,  passing the provided
	/// properties to the event handler via a <see cref="MouseEventArgs"/> object.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <param name="detail">A count of consecutive clicks that happened in a short amount of time, incremented by one.</param>
	/// <param name="screenX">The X coordinate of the mouse pointer in global (screen) coordinates.</param>
	/// <param name="screenY">The Y coordinate of the mouse pointer in global (screen) coordinates.</param>
	/// <param name="clientX">The X coordinate of the mouse pointer in local (DOM content) coordinates.</param>
	/// <param name="clientY">The Y coordinate of the mouse pointer in local (DOM content) coordinates.</param>
	/// <param name="button">
	///     The button number that was pressed when the mouse event was fired: Left button=0,
	///     middle button=1 (if present), right button=2. For mice configured for left handed
	///     use in which the button actions are reversed the values are instead read from
	///     right to left.
	/// </param>
	/// <param name="buttons">
	///     The buttons being pressed when the mouse event was fired: Left button=1, Right
	///     button=2, Middle (wheel) button=4, 4th button (typically, "Browser Back" button)=8,
	///     5th button (typically, "Browser Forward" button)=16. If two or more buttons are
	///     pressed, returns the logical sum of the values. E.g., if Left button and Right
	///     button are pressed, returns 3 (=1 | 2).
	/// </param>
	/// <param name="ctrlKey">true if the control key was down when the event was fired. false otherwise.</param>
	/// <param name="shiftKey">true if the shift key was down when the event was fired. false otherwise.</param>
	/// <param name="altKey">true if the alt key was down when the event was fired. false otherwise.</param>
	/// <param name="metaKey">true if the meta key was down when the event was fired. false otherwise.</param>
	/// <param name="type">Gets or sets the type of the event.</param>
	public static Task MouseUpAsync(this IElement element, long detail = default, double screenX = default, double screenY = default, double clientX = default, double clientY = default, long button = default, long buttons = default, bool ctrlKey = default, bool shiftKey = default, bool altKey = default, bool metaKey = default, string? type = default)
		=> MouseUpAsync(element, new MouseEventArgs { Detail = detail, ScreenX = screenX, ScreenY = screenY, ClientX = clientX, ClientY = clientY, Button = button, Buttons = buttons, CtrlKey = ctrlKey, ShiftKey = shiftKey, AltKey = altKey, MetaKey = metaKey, Type = type! });

	/// <summary>
	/// Raises the <c>@onmouseup</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
	/// <returns>A task that completes when the event handler is done.</returns>
	public static Task MouseUpAsync(this IElement element, MouseEventArgs eventArgs) => element.TriggerEventAsync("onmouseup", eventArgs);

#if NET6_0_OR_GREATER
	/// <summary>
	/// Raises the <c>@onclick</c> event on <paramref name="element"/>,  passing the provided
	/// properties to the event handler via a <see cref="MouseEventArgs"/> object.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <param name="detail">A count of consecutive clicks that happened in a short amount of time, incremented by one.</param>
	/// <param name="screenX">The X coordinate of the mouse pointer in global (screen) coordinates.</param>
	/// <param name="screenY">The Y coordinate of the mouse pointer in global (screen) coordinates.</param>
	/// <param name="clientX">The X coordinate of the mouse pointer in local (DOM content) coordinates.</param>
	/// <param name="clientY">The Y coordinate of the mouse pointer in local (DOM content) coordinates.</param>
	/// <param name="pageX">The X coordinate of the mouse pointer relative to the whole document.</param>
	/// <param name="pageY">The Y coordinate of the mouse pointer relative to the whole document.</param>
	/// <param name="offsetX">The X coordinate of the mouse pointer in relative (Target Element) coordinates.</param>
	/// <param name="offsetY">The Y coordinate of the mouse pointer in relative (Target Element) coordinates.</param>
	/// <param name="button">
	///     The button number that was pressed when the mouse event was fired: Left button=0,
	///     middle button=1 (if present), right button=2. For mice configured for left handed
	///     use in which the button actions are reversed the values are instead read from
	///     right to left.
	/// </param>
	/// <param name="buttons">
	///     The buttons being pressed when the mouse event was fired: Left button=1, Right
	///     button=2, Middle (wheel) button=4, 4th button (typically, "Browser Back" button)=8,
	///     5th button (typically, "Browser Forward" button)=16. If two or more buttons are
	///     pressed, returns the logical sum of the values. E.g., if Left button and Right
	///     button are pressed, returns 3 (=1 | 2).
	/// </param>
	/// <param name="ctrlKey">true if the control key was down when the event was fired. false otherwise.</param>
	/// <param name="shiftKey">true if the shift key was down when the event was fired. false otherwise.</param>
	/// <param name="altKey">true if the alt key was down when the event was fired. false otherwise.</param>
	/// <param name="metaKey">true if the meta key was down when the event was fired. false otherwise.</param>
	/// <param name="type">Gets or sets the type of the event.</param>
	public static void Click(this IElement element, long detail = 1, double screenX = default, double screenY = default, double clientX = default, double clientY = default, double pageX = 0, double pageY = 0, double offsetX = 0, double offsetY = 0, long button = default, long buttons = default, bool ctrlKey = default, bool shiftKey = default, bool altKey = default, bool metaKey = default, string? type = default)
		=> _ = ClickAsync(element, new MouseEventArgs { Detail = detail, ScreenX = screenX, ScreenY = screenY, ClientX = clientX, PageX = pageX, PageY = pageY, OffsetX = offsetX, OffsetY = offsetY, ClientY = clientY, Button = button, Buttons = buttons, CtrlKey = ctrlKey, ShiftKey = shiftKey, AltKey = altKey, MetaKey = metaKey, Type = type! });

	/// <summary>
	/// Raises the <c>@onclick</c> event on <paramref name="element"/>,  passing the provided
	/// properties to the event handler via a <see cref="MouseEventArgs"/> object.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <param name="detail">A count of consecutive clicks that happened in a short amount of time, incremented by one.</param>
	/// <param name="screenX">The X coordinate of the mouse pointer in global (screen) coordinates.</param>
	/// <param name="screenY">The Y coordinate of the mouse pointer in global (screen) coordinates.</param>
	/// <param name="clientX">The X coordinate of the mouse pointer in local (DOM content) coordinates.</param>
	/// <param name="clientY">The Y coordinate of the mouse pointer in local (DOM content) coordinates.</param>
	/// <param name="pageX">The X coordinate of the mouse pointer relative to the whole document.</param>
	/// <param name="pageY">The Y coordinate of the mouse pointer relative to the whole document.</param>
	/// <param name="offsetX">The X coordinate of the mouse pointer in relative (Target Element) coordinates.</param>
	/// <param name="offsetY">The Y coordinate of the mouse pointer in relative (Target Element) coordinates.</param>
	/// <param name="button">
	///     The button number that was pressed when the mouse event was fired: Left button=0,
	///     middle button=1 (if present), right button=2. For mice configured for left handed
	///     use in which the button actions are reversed the values are instead read from
	///     right to left.
	/// </param>
	/// <param name="buttons">
	///     The buttons being pressed when the mouse event was fired: Left button=1, Right
	///     button=2, Middle (wheel) button=4, 4th button (typically, "Browser Back" button)=8,
	///     5th button (typically, "Browser Forward" button)=16. If two or more buttons are
	///     pressed, returns the logical sum of the values. E.g., if Left button and Right
	///     button are pressed, returns 3 (=1 | 2).
	/// </param>
	/// <param name="ctrlKey">true if the control key was down when the event was fired. false otherwise.</param>
	/// <param name="shiftKey">true if the shift key was down when the event was fired. false otherwise.</param>
	/// <param name="altKey">true if the alt key was down when the event was fired. false otherwise.</param>
	/// <param name="metaKey">true if the meta key was down when the event was fired. false otherwise.</param>
	/// <param name="type">Gets or sets the type of the event.</param>
	public static Task ClickAsync(this IElement element, long detail = 1, double screenX = default, double screenY = default, double clientX = default, double clientY = default, double pageX = 0, double pageY = 0, double offsetX = 0, double offsetY = 0, long button = default, long buttons = default, bool ctrlKey = default, bool shiftKey = default, bool altKey = default, bool metaKey = default, string? type = default)
		=> ClickAsync(element, new MouseEventArgs { Detail = detail, ScreenX = screenX, ScreenY = screenY, ClientX = clientX, PageX = pageX, PageY = pageY, OffsetX = offsetX, OffsetY = offsetY, ClientY = clientY, Button = button, Buttons = buttons, CtrlKey = ctrlKey, ShiftKey = shiftKey, AltKey = altKey, MetaKey = metaKey, Type = type! });
#elif NET5_0
	/// <summary>
	/// Raises the <c>@onclick</c> event on <paramref name="element"/>,  passing the provided
	/// properties to the event handler via a <see cref="MouseEventArgs"/> object.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <param name="detail">A count of consecutive clicks that happened in a short amount of time, incremented by one.</param>
	/// <param name="screenX">The X coordinate of the mouse pointer in global (screen) coordinates.</param>
	/// <param name="screenY">The Y coordinate of the mouse pointer in global (screen) coordinates.</param>
	/// <param name="clientX">The X coordinate of the mouse pointer in local (DOM content) coordinates.</param>
	/// <param name="clientY">The Y coordinate of the mouse pointer in local (DOM content) coordinates.</param>
	/// <param name="offsetX">The X coordinate of the mouse pointer in relative (Target Element) coordinates.</param>
	/// <param name="offsetY">The Y coordinate of the mouse pointer in relative (Target Element) coordinates.</param>
	/// <param name="button">
	///     The button number that was pressed when the mouse event was fired: Left button=0,
	///     middle button=1 (if present), right button=2. For mice configured for left handed
	///     use in which the button actions are reversed the values are instead read from
	///     right to left.
	/// </param>
	/// <param name="buttons">
	///     The buttons being pressed when the mouse event was fired: Left button=1, Right
	///     button=2, Middle (wheel) button=4, 4th button (typically, "Browser Back" button)=8,
	///     5th button (typically, "Browser Forward" button)=16. If two or more buttons are
	///     pressed, returns the logical sum of the values. E.g., if Left button and Right
	///     button are pressed, returns 3 (=1 | 2).
	/// </param>
	/// <param name="ctrlKey">true if the control key was down when the event was fired. false otherwise.</param>
	/// <param name="shiftKey">true if the shift key was down when the event was fired. false otherwise.</param>
	/// <param name="altKey">true if the alt key was down when the event was fired. false otherwise.</param>
	/// <param name="metaKey">true if the meta key was down when the event was fired. false otherwise.</param>
	/// <param name="type">Gets or sets the type of the event.</param>
	public static void Click(this IElement element, long detail = 1, double screenX = default, double screenY = default, double clientX = default, double clientY = default, double offsetX = 0, double offsetY = 0, long button = default, long buttons = default, bool ctrlKey = default, bool shiftKey = default, bool altKey = default, bool metaKey = default, string? type = default)
		=> _ = ClickAsync(element, new MouseEventArgs { Detail = detail, ScreenX = screenX, ScreenY = screenY, ClientX = clientX, OffsetX = offsetX, OffsetY = offsetY, ClientY = clientY, Button = button, Buttons = buttons, CtrlKey = ctrlKey, ShiftKey = shiftKey, AltKey = altKey, MetaKey = metaKey, Type = type! });

	/// <summary>
	/// Raises the <c>@onclick</c> event on <paramref name="element"/>,  passing the provided
	/// properties to the event handler via a <see cref="MouseEventArgs"/> object.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <param name="detail">A count of consecutive clicks that happened in a short amount of time, incremented by one.</param>
	/// <param name="screenX">The X coordinate of the mouse pointer in global (screen) coordinates.</param>
	/// <param name="screenY">The Y coordinate of the mouse pointer in global (screen) coordinates.</param>
	/// <param name="clientX">The X coordinate of the mouse pointer in local (DOM content) coordinates.</param>
	/// <param name="clientY">The Y coordinate of the mouse pointer in local (DOM content) coordinates.</param>
	/// <param name="offsetX">The X coordinate of the mouse pointer in relative (Target Element) coordinates.</param>
	/// <param name="offsetY">The Y coordinate of the mouse pointer in relative (Target Element) coordinates.</param>
	/// <param name="button">
	///     The button number that was pressed when the mouse event was fired: Left button=0,
	///     middle button=1 (if present), right button=2. For mice configured for left handed
	///     use in which the button actions are reversed the values are instead read from
	///     right to left.
	/// </param>
	/// <param name="buttons">
	///     The buttons being pressed when the mouse event was fired: Left button=1, Right
	///     button=2, Middle (wheel) button=4, 4th button (typically, "Browser Back" button)=8,
	///     5th button (typically, "Browser Forward" button)=16. If two or more buttons are
	///     pressed, returns the logical sum of the values. E.g., if Left button and Right
	///     button are pressed, returns 3 (=1 | 2).
	/// </param>
	/// <param name="ctrlKey">true if the control key was down when the event was fired. false otherwise.</param>
	/// <param name="shiftKey">true if the shift key was down when the event was fired. false otherwise.</param>
	/// <param name="altKey">true if the alt key was down when the event was fired. false otherwise.</param>
	/// <param name="metaKey">true if the meta key was down when the event was fired. false otherwise.</param>
	/// <param name="type">Gets or sets the type of the event.</param>
	public static Task ClickAsync(this IElement element, long detail = 1, double screenX = default, double screenY = default, double clientX = default, double clientY = default, double offsetX = 0, double offsetY = 0, long button = default, long buttons = default, bool ctrlKey = default, bool shiftKey = default, bool altKey = default, bool metaKey = default, string? type = default)
		=> ClickAsync(element, new MouseEventArgs { Detail = detail, ScreenX = screenX, ScreenY = screenY, ClientX = clientX, OffsetX = offsetX, OffsetY = offsetY, ClientY = clientY, Button = button, Buttons = buttons, CtrlKey = ctrlKey, ShiftKey = shiftKey, AltKey = altKey, MetaKey = metaKey, Type = type! });
#else
	/// <summary>
	/// Raises the <c>@onclick</c> event on <paramref name="element"/>,  passing the provided
	/// properties to the event handler via a <see cref="MouseEventArgs"/> object.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <param name="detail">A count of consecutive clicks that happened in a short amount of time, incremented by one.</param>
	/// <param name="screenX">The X coordinate of the mouse pointer in global (screen) coordinates.</param>
	/// <param name="screenY">The Y coordinate of the mouse pointer in global (screen) coordinates.</param>
	/// <param name="clientX">The X coordinate of the mouse pointer in local (DOM content) coordinates.</param>
	/// <param name="clientY">The Y coordinate of the mouse pointer in local (DOM content) coordinates.</param>
	/// <param name="button">
	///     The button number that was pressed when the mouse event was fired: Left button=0,
	///     middle button=1 (if present), right button=2. For mice configured for left handed
	///     use in which the button actions are reversed the values are instead read from
	///     right to left.
	/// </param>
	/// <param name="buttons">
	///     The buttons being pressed when the mouse event was fired: Left button=1, Right
	///     button=2, Middle (wheel) button=4, 4th button (typically, "Browser Back" button)=8,
	///     5th button (typically, "Browser Forward" button)=16. If two or more buttons are
	///     pressed, returns the logical sum of the values. E.g., if Left button and Right
	///     button are pressed, returns 3 (=1 | 2).
	/// </param>
	/// <param name="ctrlKey">true if the control key was down when the event was fired. false otherwise.</param>
	/// <param name="shiftKey">true if the shift key was down when the event was fired. false otherwise.</param>
	/// <param name="altKey">true if the alt key was down when the event was fired. false otherwise.</param>
	/// <param name="metaKey">true if the meta key was down when the event was fired. false otherwise.</param>
	/// <param name="type">Gets or sets the type of the event.</param>
	public static void Click(this IElement element, long detail = 1, double screenX = default, double screenY = default, double clientX = default, double clientY = default, long button = default, long buttons = default, bool ctrlKey = default, bool shiftKey = default, bool altKey = default, bool metaKey = default, string? type = default)
		=> _ = ClickAsync(element, new MouseEventArgs { Detail = detail, ScreenX = screenX, ScreenY = screenY, ClientX = clientX, ClientY = clientY, Button = button, Buttons = buttons, CtrlKey = ctrlKey, ShiftKey = shiftKey, AltKey = altKey, MetaKey = metaKey, Type = type! });

	/// <summary>
	/// Raises the <c>@onclick</c> event on <paramref name="element"/>,  passing the provided
	/// properties to the event handler via a <see cref="MouseEventArgs"/> object.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <param name="detail">A count of consecutive clicks that happened in a short amount of time, incremented by one.</param>
	/// <param name="screenX">The X coordinate of the mouse pointer in global (screen) coordinates.</param>
	/// <param name="screenY">The Y coordinate of the mouse pointer in global (screen) coordinates.</param>
	/// <param name="clientX">The X coordinate of the mouse pointer in local (DOM content) coordinates.</param>
	/// <param name="clientY">The Y coordinate of the mouse pointer in local (DOM content) coordinates.</param>
	/// <param name="button">
	///     The button number that was pressed when the mouse event was fired: Left button=0,
	///     middle button=1 (if present), right button=2. For mice configured for left handed
	///     use in which the button actions are reversed the values are instead read from
	///     right to left.
	/// </param>
	/// <param name="buttons">
	///     The buttons being pressed when the mouse event was fired: Left button=1, Right
	///     button=2, Middle (wheel) button=4, 4th button (typically, "Browser Back" button)=8,
	///     5th button (typically, "Browser Forward" button)=16. If two or more buttons are
	///     pressed, returns the logical sum of the values. E.g., if Left button and Right
	///     button are pressed, returns 3 (=1 | 2).
	/// </param>
	/// <param name="ctrlKey">true if the control key was down when the event was fired. false otherwise.</param>
	/// <param name="shiftKey">true if the shift key was down when the event was fired. false otherwise.</param>
	/// <param name="altKey">true if the alt key was down when the event was fired. false otherwise.</param>
	/// <param name="metaKey">true if the meta key was down when the event was fired. false otherwise.</param>
	/// <param name="type">Gets or sets the type of the event.</param>
	public static Task ClickAsync(this IElement element, long detail = 1, double screenX = default, double screenY = default, double clientX = default, double clientY = default, long button = default, long buttons = default, bool ctrlKey = default, bool shiftKey = default, bool altKey = default, bool metaKey = default, string? type = default)
		=> ClickAsync(element, new MouseEventArgs { Detail = detail, ScreenX = screenX, ScreenY = screenY, ClientX = clientX, ClientY = clientY, Button = button, Buttons = buttons, CtrlKey = ctrlKey, ShiftKey = shiftKey, AltKey = altKey, MetaKey = metaKey, Type = type! });
#endif


	/// <summary>
	/// Raises the <c>@onclick</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
	public static void Click(this IElement element, MouseEventArgs eventArgs)
		=> _ = ClickAsync(element, eventArgs);

	/// <summary>
	/// Raises the <c>@onclick</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
	/// <returns>A task that completes when the event handler is done.</returns>
	public static Task ClickAsync(this IElement element, MouseEventArgs eventArgs) => element.TriggerEventAsync("onclick", eventArgs);

#if NET6_0_OR_GREATER
	/// <summary>
	/// Raises the <c>@ondblclick</c> event on <paramref name="element"/>,  passing the provided
	/// properties to the event handler via a <see cref="MouseEventArgs"/> object.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <param name="detail">A count of consecutive clicks that happened in a short amount of time, incremented by one.</param>
	/// <param name="screenX">The X coordinate of the mouse pointer in global (screen) coordinates.</param>
	/// <param name="screenY">The Y coordinate of the mouse pointer in global (screen) coordinates.</param>
	/// <param name="clientX">The X coordinate of the mouse pointer in local (DOM content) coordinates.</param>
	/// <param name="clientY">The Y coordinate of the mouse pointer in local (DOM content) coordinates.</param>
	/// <param name="pageX">The X coordinate of the mouse pointer relative to the whole document.</param>
	/// <param name="pageY">The Y coordinate of the mouse pointer relative to the whole document.</param>
	/// <param name="offsetX">The X coordinate of the mouse pointer in relative (Target Element) coordinates.</param>
	/// <param name="offsetY">The Y coordinate of the mouse pointer in relative (Target Element) coordinates.</param>
	/// <param name="button">
	///     The button number that was pressed when the mouse event was fired: Left button=0,
	///     middle button=1 (if present), right button=2. For mice configured for left handed
	///     use in which the button actions are reversed the values are instead read from
	///     right to left.
	/// </param>
	/// <param name="buttons">
	///     The buttons being pressed when the mouse event was fired: Left button=1, Right
	///     button=2, Middle (wheel) button=4, 4th button (typically, "Browser Back" button)=8,
	///     5th button (typically, "Browser Forward" button)=16. If two or more buttons are
	///     pressed, returns the logical sum of the values. E.g., if Left button and Right
	///     button are pressed, returns 3 (=1 | 2).
	/// </param>
	/// <param name="ctrlKey">true if the control key was down when the event was fired. false otherwise.</param>
	/// <param name="shiftKey">true if the shift key was down when the event was fired. false otherwise.</param>
	/// <param name="altKey">true if the alt key was down when the event was fired. false otherwise.</param>
	/// <param name="metaKey">true if the meta key was down when the event was fired. false otherwise.</param>
	/// <param name="type">Gets or sets the type of the event.</param>
	public static void DoubleClick(this IElement element, long detail = 2, double screenX = default, double screenY = default, double clientX = default, double clientY = default, double pageX = 0, double pageY = 0, double offsetX = 0, double offsetY = 0, long button = default, long buttons = default, bool ctrlKey = default, bool shiftKey = default, bool altKey = default, bool metaKey = default, string? type = default)
		=> _ = DoubleClickAsync(element, new MouseEventArgs { Detail = detail, ScreenX = screenX, ScreenY = screenY, ClientX = clientX, ClientY = clientY, PageX = pageX, PageY = pageY, OffsetX = offsetX, OffsetY = offsetY, Button = button, Buttons = buttons, CtrlKey = ctrlKey, ShiftKey = shiftKey, AltKey = altKey, MetaKey = metaKey, Type = type! });

	/// <summary>
	/// Raises the <c>@ondblclick</c> event on <paramref name="element"/>,  passing the provided
	/// properties to the event handler via a <see cref="MouseEventArgs"/> object.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <param name="detail">A count of consecutive clicks that happened in a short amount of time, incremented by one.</param>
	/// <param name="screenX">The X coordinate of the mouse pointer in global (screen) coordinates.</param>
	/// <param name="screenY">The Y coordinate of the mouse pointer in global (screen) coordinates.</param>
	/// <param name="clientX">The X coordinate of the mouse pointer in local (DOM content) coordinates.</param>
	/// <param name="clientY">The Y coordinate of the mouse pointer in local (DOM content) coordinates.</param>
	/// <param name="pageX">The X coordinate of the mouse pointer relative to the whole document.</param>
	/// <param name="pageY">The Y coordinate of the mouse pointer relative to the whole document.</param>
	/// <param name="offsetX">The X coordinate of the mouse pointer in relative (Target Element) coordinates.</param>
	/// <param name="offsetY">The Y coordinate of the mouse pointer in relative (Target Element) coordinates.</param>
	/// <param name="button">
	///     The button number that was pressed when the mouse event was fired: Left button=0,
	///     middle button=1 (if present), right button=2. For mice configured for left handed
	///     use in which the button actions are reversed the values are instead read from
	///     right to left.
	/// </param>
	/// <param name="buttons">
	///     The buttons being pressed when the mouse event was fired: Left button=1, Right
	///     button=2, Middle (wheel) button=4, 4th button (typically, "Browser Back" button)=8,
	///     5th button (typically, "Browser Forward" button)=16. If two or more buttons are
	///     pressed, returns the logical sum of the values. E.g., if Left button and Right
	///     button are pressed, returns 3 (=1 | 2).
	/// </param>
	/// <param name="ctrlKey">true if the control key was down when the event was fired. false otherwise.</param>
	/// <param name="shiftKey">true if the shift key was down when the event was fired. false otherwise.</param>
	/// <param name="altKey">true if the alt key was down when the event was fired. false otherwise.</param>
	/// <param name="metaKey">true if the meta key was down when the event was fired. false otherwise.</param>
	/// <param name="type">Gets or sets the type of the event.</param>
	public static Task DoubleClickAsync(this IElement element, long detail = 2, double screenX = default, double screenY = default, double clientX = default, double clientY = default, double pageX = 0, double pageY = 0, double offsetX = 0, double offsetY = 0, long button = default, long buttons = default, bool ctrlKey = default, bool shiftKey = default, bool altKey = default, bool metaKey = default, string? type = default)
		=> DoubleClickAsync(element, new MouseEventArgs { Detail = detail, ScreenX = screenX, ScreenY = screenY, ClientX = clientX, ClientY = clientY, PageX = pageX, PageY = pageY, OffsetX = offsetX, OffsetY = offsetY, Button = button, Buttons = buttons, CtrlKey = ctrlKey, ShiftKey = shiftKey, AltKey = altKey, MetaKey = metaKey, Type = type! });
#elif NET5_0
	/// <summary>
	/// Raises the <c>@ondblclick</c> event on <paramref name="element"/>,  passing the provided
	/// properties to the event handler via a <see cref="MouseEventArgs"/> object.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <param name="detail">A count of consecutive clicks that happened in a short amount of time, incremented by one.</param>
	/// <param name="screenX">The X coordinate of the mouse pointer in global (screen) coordinates.</param>
	/// <param name="screenY">The Y coordinate of the mouse pointer in global (screen) coordinates.</param>
	/// <param name="clientX">The X coordinate of the mouse pointer in local (DOM content) coordinates.</param>
	/// <param name="clientY">The Y coordinate of the mouse pointer in local (DOM content) coordinates.</param>
	/// <param name="offsetX">The X coordinate of the mouse pointer in relative (Target Element) coordinates.</param>
	/// <param name="offsetY">The Y coordinate of the mouse pointer in relative (Target Element) coordinates.</param>
	/// <param name="button">
	///     The button number that was pressed when the mouse event was fired: Left button=0,
	///     middle button=1 (if present), right button=2. For mice configured for left handed
	///     use in which the button actions are reversed the values are instead read from
	///     right to left.
	/// </param>
	/// <param name="buttons">
	///     The buttons being pressed when the mouse event was fired: Left button=1, Right
	///     button=2, Middle (wheel) button=4, 4th button (typically, "Browser Back" button)=8,
	///     5th button (typically, "Browser Forward" button)=16. If two or more buttons are
	///     pressed, returns the logical sum of the values. E.g., if Left button and Right
	///     button are pressed, returns 3 (=1 | 2).
	/// </param>
	/// <param name="ctrlKey">true if the control key was down when the event was fired. false otherwise.</param>
	/// <param name="shiftKey">true if the shift key was down when the event was fired. false otherwise.</param>
	/// <param name="altKey">true if the alt key was down when the event was fired. false otherwise.</param>
	/// <param name="metaKey">true if the meta key was down when the event was fired. false otherwise.</param>
	/// <param name="type">Gets or sets the type of the event.</param>
	public static void DoubleClick(this IElement element, long detail = 2, double screenX = default, double screenY = default, double clientX = default, double clientY = default, double offsetX = 0, double offsetY = 0, long button = default, long buttons = default, bool ctrlKey = default, bool shiftKey = default, bool altKey = default, bool metaKey = default, string? type = default)
		=> _ = DoubleClickAsync(element, new MouseEventArgs { Detail = detail, ScreenX = screenX, ScreenY = screenY, ClientX = clientX, ClientY = clientY, OffsetX = offsetX, OffsetY = offsetY, Button = button, Buttons = buttons, CtrlKey = ctrlKey, ShiftKey = shiftKey, AltKey = altKey, MetaKey = metaKey, Type = type! });

	/// <summary>
	/// Raises the <c>@ondblclick</c> event on <paramref name="element"/>,  passing the provided
	/// properties to the event handler via a <see cref="MouseEventArgs"/> object.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <param name="detail">A count of consecutive clicks that happened in a short amount of time, incremented by one.</param>
	/// <param name="screenX">The X coordinate of the mouse pointer in global (screen) coordinates.</param>
	/// <param name="screenY">The Y coordinate of the mouse pointer in global (screen) coordinates.</param>
	/// <param name="clientX">The X coordinate of the mouse pointer in local (DOM content) coordinates.</param>
	/// <param name="clientY">The Y coordinate of the mouse pointer in local (DOM content) coordinates.</param>
	/// <param name="offsetX">The X coordinate of the mouse pointer in relative (Target Element) coordinates.</param>
	/// <param name="offsetY">The Y coordinate of the mouse pointer in relative (Target Element) coordinates.</param>
	/// <param name="button">
	///     The button number that was pressed when the mouse event was fired: Left button=0,
	///     middle button=1 (if present), right button=2. For mice configured for left handed
	///     use in which the button actions are reversed the values are instead read from
	///     right to left.
	/// </param>
	/// <param name="buttons">
	///     The buttons being pressed when the mouse event was fired: Left button=1, Right
	///     button=2, Middle (wheel) button=4, 4th button (typically, "Browser Back" button)=8,
	///     5th button (typically, "Browser Forward" button)=16. If two or more buttons are
	///     pressed, returns the logical sum of the values. E.g., if Left button and Right
	///     button are pressed, returns 3 (=1 | 2).
	/// </param>
	/// <param name="ctrlKey">true if the control key was down when the event was fired. false otherwise.</param>
	/// <param name="shiftKey">true if the shift key was down when the event was fired. false otherwise.</param>
	/// <param name="altKey">true if the alt key was down when the event was fired. false otherwise.</param>
	/// <param name="metaKey">true if the meta key was down when the event was fired. false otherwise.</param>
	/// <param name="type">Gets or sets the type of the event.</param>
	public static Task DoubleClickAsync(this IElement element, long detail = 2, double screenX = default, double screenY = default, double clientX = default, double clientY = default, double offsetX = 0, double offsetY = 0, long button = default, long buttons = default, bool ctrlKey = default, bool shiftKey = default, bool altKey = default, bool metaKey = default, string? type = default)
		=> DoubleClickAsync(element, new MouseEventArgs { Detail = detail, ScreenX = screenX, ScreenY = screenY, ClientX = clientX, ClientY = clientY, OffsetX = offsetX, OffsetY = offsetY, Button = button, Buttons = buttons, CtrlKey = ctrlKey, ShiftKey = shiftKey, AltKey = altKey, MetaKey = metaKey, Type = type! });
#else

	/// <summary>
	/// Raises the <c>@ondblclick</c> event on <paramref name="element"/>,  passing the provided
	/// properties to the event handler via a <see cref="MouseEventArgs"/> object.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <param name="detail">A count of consecutive clicks that happened in a short amount of time, incremented by one.</param>
	/// <param name="screenX">The X coordinate of the mouse pointer in global (screen) coordinates.</param>
	/// <param name="screenY">The Y coordinate of the mouse pointer in global (screen) coordinates.</param>
	/// <param name="clientX">The X coordinate of the mouse pointer in local (DOM content) coordinates.</param>
	/// <param name="clientY">The Y coordinate of the mouse pointer in local (DOM content) coordinates.</param>
	/// <param name="button">
	///     The button number that was pressed when the mouse event was fired: Left button=0,
	///     middle button=1 (if present), right button=2. For mice configured for left handed
	///     use in which the button actions are reversed the values are instead read from
	///     right to left.
	/// </param>
	/// <param name="buttons">
	///     The buttons being pressed when the mouse event was fired: Left button=1, Right
	///     button=2, Middle (wheel) button=4, 4th button (typically, "Browser Back" button)=8,
	///     5th button (typically, "Browser Forward" button)=16. If two or more buttons are
	///     pressed, returns the logical sum of the values. E.g., if Left button and Right
	///     button are pressed, returns 3 (=1 | 2).
	/// </param>
	/// <param name="ctrlKey">true if the control key was down when the event was fired. false otherwise.</param>
	/// <param name="shiftKey">true if the shift key was down when the event was fired. false otherwise.</param>
	/// <param name="altKey">true if the alt key was down when the event was fired. false otherwise.</param>
	/// <param name="metaKey">true if the meta key was down when the event was fired. false otherwise.</param>
	/// <param name="type">Gets or sets the type of the event.</param>
	public static void DoubleClick(this IElement element, long detail = 2, double screenX = default, double screenY = default, double clientX = default, double clientY = default, long button = default, long buttons = default, bool ctrlKey = default, bool shiftKey = default, bool altKey = default, bool metaKey = default, string? type = default)
		=> _ = DoubleClickAsync(element, new MouseEventArgs { Detail = detail, ScreenX = screenX, ScreenY = screenY, ClientX = clientX, ClientY = clientY, Button = button, Buttons = buttons, CtrlKey = ctrlKey, ShiftKey = shiftKey, AltKey = altKey, MetaKey = metaKey, Type = type! });

	/// <summary>
	/// Raises the <c>@ondblclick</c> event on <paramref name="element"/>,  passing the provided
	/// properties to the event handler via a <see cref="MouseEventArgs"/> object.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <param name="detail">A count of consecutive clicks that happened in a short amount of time, incremented by one.</param>
	/// <param name="screenX">The X coordinate of the mouse pointer in global (screen) coordinates.</param>
	/// <param name="screenY">The Y coordinate of the mouse pointer in global (screen) coordinates.</param>
	/// <param name="clientX">The X coordinate of the mouse pointer in local (DOM content) coordinates.</param>
	/// <param name="clientY">The Y coordinate of the mouse pointer in local (DOM content) coordinates.</param>
	/// <param name="button">
	///     The button number that was pressed when the mouse event was fired: Left button=0,
	///     middle button=1 (if present), right button=2. For mice configured for left handed
	///     use in which the button actions are reversed the values are instead read from
	///     right to left.
	/// </param>
	/// <param name="buttons">
	///     The buttons being pressed when the mouse event was fired: Left button=1, Right
	///     button=2, Middle (wheel) button=4, 4th button (typically, "Browser Back" button)=8,
	///     5th button (typically, "Browser Forward" button)=16. If two or more buttons are
	///     pressed, returns the logical sum of the values. E.g., if Left button and Right
	///     button are pressed, returns 3 (=1 | 2).
	/// </param>
	/// <param name="ctrlKey">true if the control key was down when the event was fired. false otherwise.</param>
	/// <param name="shiftKey">true if the shift key was down when the event was fired. false otherwise.</param>
	/// <param name="altKey">true if the alt key was down when the event was fired. false otherwise.</param>
	/// <param name="metaKey">true if the meta key was down when the event was fired. false otherwise.</param>
	/// <param name="type">Gets or sets the type of the event.</param>
	public static Task DoubleClickAsync(this IElement element, long detail = 2, double screenX = default, double screenY = default, double clientX = default, double clientY = default, long button = default, long buttons = default, bool ctrlKey = default, bool shiftKey = default, bool altKey = default, bool metaKey = default, string? type = default)
		=> DoubleClickAsync(element, new MouseEventArgs { Detail = detail, ScreenX = screenX, ScreenY = screenY, ClientX = clientX, ClientY = clientY, Button = button, Buttons = buttons, CtrlKey = ctrlKey, ShiftKey = shiftKey, AltKey = altKey, MetaKey = metaKey, Type = type! });
#endif
	/// <summary>
	/// Raises the <c>@ondblclick</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
	public static void DoubleClick(this IElement element, MouseEventArgs eventArgs)
		=> _ = DoubleClickAsync(element, eventArgs);

	/// <summary>
	/// Raises the <c>@ondblclick</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
	/// <returns>A task that completes when the event handler is done.</returns>
	public static Task DoubleClickAsync(this IElement element, MouseEventArgs eventArgs) => element.TriggerEventAsync("ondblclick", eventArgs);

	/// <summary>
	/// Raises the <c>@onwheel</c> event on <paramref name="element"/>,  passing the provided
	/// properties to the event handler via a <see cref="MouseEventArgs"/> object.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <param name="detail">A count of consecutive clicks that happened in a short amount of time, incremented by one.</param>
	/// <param name="screenX">The X coordinate of the mouse pointer in global (screen) coordinates.</param>
	/// <param name="screenY">The Y coordinate of the mouse pointer in global (screen) coordinates.</param>
	/// <param name="clientX">The X coordinate of the mouse pointer in local (DOM content) coordinates.</param>
	/// <param name="clientY">The Y coordinate of the mouse pointer in local (DOM content) coordinates.</param>
	/// <param name="button">
	///     The button number that was pressed when the mouse event was fired: Left button=0,
	///     middle button=1 (if present), right button=2. For mice configured for left handed
	///     use in which the button actions are reversed the values are instead read from
	///     right to left.
	/// </param>
	/// <param name="buttons">
	///     The buttons being pressed when the mouse event was fired: Left button=1, Right
	///     button=2, Middle (wheel) button=4, 4th button (typically, "Browser Back" button)=8,
	///     5th button (typically, "Browser Forward" button)=16. If two or more buttons are
	///     pressed, returns the logical sum of the values. E.g., if Left button and Right
	///     button are pressed, returns 3 (=1 | 2).
	/// </param>
	/// <param name="ctrlKey">true if the control key was down when the event was fired. false otherwise.</param>
	/// <param name="shiftKey">true if the shift key was down when the event was fired. false otherwise.</param>
	/// <param name="altKey">true if the alt key was down when the event was fired. false otherwise.</param>
	/// <param name="metaKey">true if the meta key was down when the event was fired. false otherwise.</param>
	/// <param name="type">Gets or sets the type of the event.</param>
	/// <param name="deltaX">The horizontal scroll amount.</param>
	/// <param name="deltaY">The vertical scroll amount.</param>
	/// <param name="deltaZ">The scroll amount for the z-axis.</param>
	/// <param name="deltaMode">The unit of the delta values scroll amount.</param>
	public static void Wheel(this IElement element, long detail = default, double screenX = default, double screenY = default, double clientX = default, double clientY = default, long button = default, long buttons = default, bool ctrlKey = default, bool shiftKey = default, bool altKey = default, bool metaKey = default, string? type = default, double deltaX = default, double deltaY = default, double deltaZ = default, long deltaMode = default)
		=> _ = WheelAsync(element, new WheelEventArgs { Detail = detail, ScreenX = screenX, ScreenY = screenY, ClientX = clientX, ClientY = clientY, Button = button, Buttons = buttons, CtrlKey = ctrlKey, ShiftKey = shiftKey, AltKey = altKey, MetaKey = metaKey, Type = type!, DeltaX = deltaX, DeltaY = deltaY, DeltaZ = deltaZ, DeltaMode = deltaMode });

	/// <summary>
	/// Raises the <c>@onwheel</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
	public static void Wheel(this IElement element, WheelEventArgs eventArgs)
		=> _ = WheelAsync(element, eventArgs);

	/// <summary>
	/// Raises the <c>@onwheel</c> event on <paramref name="element"/>,  passing the provided
	/// properties to the event handler via a <see cref="MouseEventArgs"/> object.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <param name="detail">A count of consecutive clicks that happened in a short amount of time, incremented by one.</param>
	/// <param name="screenX">The X coordinate of the mouse pointer in global (screen) coordinates.</param>
	/// <param name="screenY">The Y coordinate of the mouse pointer in global (screen) coordinates.</param>
	/// <param name="clientX">The X coordinate of the mouse pointer in local (DOM content) coordinates.</param>
	/// <param name="clientY">The Y coordinate of the mouse pointer in local (DOM content) coordinates.</param>
	/// <param name="button">
	///     The button number that was pressed when the mouse event was fired: Left button=0,
	///     middle button=1 (if present), right button=2. For mice configured for left handed
	///     use in which the button actions are reversed the values are instead read from
	///     right to left.
	/// </param>
	/// <param name="buttons">
	///     The buttons being pressed when the mouse event was fired: Left button=1, Right
	///     button=2, Middle (wheel) button=4, 4th button (typically, "Browser Back" button)=8,
	///     5th button (typically, "Browser Forward" button)=16. If two or more buttons are
	///     pressed, returns the logical sum of the values. E.g., if Left button and Right
	///     button are pressed, returns 3 (=1 | 2).
	/// </param>
	/// <param name="ctrlKey">true if the control key was down when the event was fired. false otherwise.</param>
	/// <param name="shiftKey">true if the shift key was down when the event was fired. false otherwise.</param>
	/// <param name="altKey">true if the alt key was down when the event was fired. false otherwise.</param>
	/// <param name="metaKey">true if the meta key was down when the event was fired. false otherwise.</param>
	/// <param name="type">Gets or sets the type of the event.</param>
	/// <param name="deltaX">The horizontal scroll amount.</param>
	/// <param name="deltaY">The vertical scroll amount.</param>
	/// <param name="deltaZ">The scroll amount for the z-axis.</param>
	/// <param name="deltaMode">The unit of the delta values scroll amount.</param>
	public static Task WheelAsync(this IElement element, long detail = default, double screenX = default, double screenY = default, double clientX = default, double clientY = default, long button = default, long buttons = default, bool ctrlKey = default, bool shiftKey = default, bool altKey = default, bool metaKey = default, string? type = default, double deltaX = default, double deltaY = default, double deltaZ = default, long deltaMode = default)
		=> WheelAsync(element, new WheelEventArgs { Detail = detail, ScreenX = screenX, ScreenY = screenY, ClientX = clientX, ClientY = clientY, Button = button, Buttons = buttons, CtrlKey = ctrlKey, ShiftKey = shiftKey, AltKey = altKey, MetaKey = metaKey, Type = type!, DeltaX = deltaX, DeltaY = deltaY, DeltaZ = deltaZ, DeltaMode = deltaMode });

	/// <summary>
	/// Raises the <c>@onwheel</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
	/// <returns>A task that completes when the event handler is done.</returns>
	public static Task WheelAsync(this IElement element, WheelEventArgs eventArgs) => element.TriggerEventAsync("onwheel", eventArgs);

	/// <summary>
	/// Raises the <c>@onmousewheel</c> event on <paramref name="element"/>,  passing the provided
	/// properties to the event handler via a <see cref="MouseEventArgs"/> object.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <param name="detail">A count of consecutive clicks that happened in a short amount of time, incremented by one.</param>
	/// <param name="screenX">The X coordinate of the mouse pointer in global (screen) coordinates.</param>
	/// <param name="screenY">The Y coordinate of the mouse pointer in global (screen) coordinates.</param>
	/// <param name="clientX">The X coordinate of the mouse pointer in local (DOM content) coordinates.</param>
	/// <param name="clientY">The Y coordinate of the mouse pointer in local (DOM content) coordinates.</param>
	/// <param name="button">
	///     The button number that was pressed when the mouse event was fired: Left button=0,
	///     middle button=1 (if present), right button=2. For mice configured for left handed
	///     use in which the button actions are reversed the values are instead read from
	///     right to left.
	/// </param>
	/// <param name="buttons">
	///     The buttons being pressed when the mouse event was fired: Left button=1, Right
	///     button=2, Middle (wheel) button=4, 4th button (typically, "Browser Back" button)=8,
	///     5th button (typically, "Browser Forward" button)=16. If two or more buttons are
	///     pressed, returns the logical sum of the values. E.g., if Left button and Right
	///     button are pressed, returns 3 (=1 | 2).
	/// </param>
	/// <param name="ctrlKey">true if the control key was down when the event was fired. false otherwise.</param>
	/// <param name="shiftKey">true if the shift key was down when the event was fired. false otherwise.</param>
	/// <param name="altKey">true if the alt key was down when the event was fired. false otherwise.</param>
	/// <param name="metaKey">true if the meta key was down when the event was fired. false otherwise.</param>
	/// <param name="type">Gets or sets the type of the event.</param>
	/// <param name="deltaX">The horizontal scroll amount.</param>
	/// <param name="deltaY">The vertical scroll amount.</param>
	/// <param name="deltaZ">The scroll amount for the z-axis.</param>
	/// <param name="deltaMode">The unit of the delta values scroll amount.</param>
	public static void MouseWheel(this IElement element, long detail = default, double screenX = default, double screenY = default, double clientX = default, double clientY = default, long button = default, long buttons = default, bool ctrlKey = default, bool shiftKey = default, bool altKey = default, bool metaKey = default, string? type = default, double deltaX = default, double deltaY = default, double deltaZ = default, long deltaMode = default)
		=> MouseWheelAsync(element, new WheelEventArgs { Detail = detail, ScreenX = screenX, ScreenY = screenY, ClientX = clientX, ClientY = clientY, Button = button, Buttons = buttons, CtrlKey = ctrlKey, ShiftKey = shiftKey, AltKey = altKey, MetaKey = metaKey, Type = type!, DeltaX = deltaX, DeltaY = deltaY, DeltaZ = deltaZ, DeltaMode = deltaMode });

	/// <summary>
	/// Raises the <c>@onmousewheel</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
	public static void MouseWheel(this IElement element, WheelEventArgs eventArgs)
		=> _ = MouseWheelAsync(element, eventArgs);

	/// <summary>
	/// Raises the <c>@onmousewheel</c> event on <paramref name="element"/>,  passing the provided
	/// properties to the event handler via a <see cref="MouseEventArgs"/> object.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <param name="detail">A count of consecutive clicks that happened in a short amount of time, incremented by one.</param>
	/// <param name="screenX">The X coordinate of the mouse pointer in global (screen) coordinates.</param>
	/// <param name="screenY">The Y coordinate of the mouse pointer in global (screen) coordinates.</param>
	/// <param name="clientX">The X coordinate of the mouse pointer in local (DOM content) coordinates.</param>
	/// <param name="clientY">The Y coordinate of the mouse pointer in local (DOM content) coordinates.</param>
	/// <param name="button">
	///     The button number that was pressed when the mouse event was fired: Left button=0,
	///     middle button=1 (if present), right button=2. For mice configured for left handed
	///     use in which the button actions are reversed the values are instead read from
	///     right to left.
	/// </param>
	/// <param name="buttons">
	///     The buttons being pressed when the mouse event was fired: Left button=1, Right
	///     button=2, Middle (wheel) button=4, 4th button (typically, "Browser Back" button)=8,
	///     5th button (typically, "Browser Forward" button)=16. If two or more buttons are
	///     pressed, returns the logical sum of the values. E.g., if Left button and Right
	///     button are pressed, returns 3 (=1 | 2).
	/// </param>
	/// <param name="ctrlKey">true if the control key was down when the event was fired. false otherwise.</param>
	/// <param name="shiftKey">true if the shift key was down when the event was fired. false otherwise.</param>
	/// <param name="altKey">true if the alt key was down when the event was fired. false otherwise.</param>
	/// <param name="metaKey">true if the meta key was down when the event was fired. false otherwise.</param>
	/// <param name="type">Gets or sets the type of the event.</param>
	/// <param name="deltaX">The horizontal scroll amount.</param>
	/// <param name="deltaY">The vertical scroll amount.</param>
	/// <param name="deltaZ">The scroll amount for the z-axis.</param>
	/// <param name="deltaMode">The unit of the delta values scroll amount.</param>
	public static Task MouseWheelAsync(this IElement element, long detail = default, double screenX = default, double screenY = default, double clientX = default, double clientY = default, long button = default, long buttons = default, bool ctrlKey = default, bool shiftKey = default, bool altKey = default, bool metaKey = default, string? type = default, double deltaX = default, double deltaY = default, double deltaZ = default, long deltaMode = default)
		=> MouseWheelAsync(element, new WheelEventArgs { Detail = detail, ScreenX = screenX, ScreenY = screenY, ClientX = clientX, ClientY = clientY, Button = button, Buttons = buttons, CtrlKey = ctrlKey, ShiftKey = shiftKey, AltKey = altKey, MetaKey = metaKey, Type = type!, DeltaX = deltaX, DeltaY = deltaY, DeltaZ = deltaZ, DeltaMode = deltaMode });

	/// <summary>
	/// Raises the <c>@onmousewheel</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
	/// <returns>A task that completes when the event handler is done.</returns>
	public static Task MouseWheelAsync(this IElement element, WheelEventArgs eventArgs) => element.TriggerEventAsync("onmousewheel", eventArgs);

	/// <summary>
	/// Raises the <c>@oncontextmenu</c> event on <paramref name="element"/>,  passing the provided
	/// properties to the event handler via a <see cref="MouseEventArgs"/> object.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <param name="detail">A count of consecutive clicks that happened in a short amount of time, incremented by one.</param>
	/// <param name="screenX">The X coordinate of the mouse pointer in global (screen) coordinates.</param>
	/// <param name="screenY">The Y coordinate of the mouse pointer in global (screen) coordinates.</param>
	/// <param name="clientX">The X coordinate of the mouse pointer in local (DOM content) coordinates.</param>
	/// <param name="clientY">The Y coordinate of the mouse pointer in local (DOM content) coordinates.</param>
	/// <param name="button">
	///     The button number that was pressed when the mouse event was fired: Left button=0,
	///     middle button=1 (if present), right button=2. For mice configured for left handed
	///     use in which the button actions are reversed the values are instead read from
	///     right to left.
	/// </param>
	/// <param name="buttons">
	///     The buttons being pressed when the mouse event was fired: Left button=1, Right
	///     button=2, Middle (wheel) button=4, 4th button (typically, "Browser Back" button)=8,
	///     5th button (typically, "Browser Forward" button)=16. If two or more buttons are
	///     pressed, returns the logical sum of the values. E.g., if Left button and Right
	///     button are pressed, returns 3 (=1 | 2).
	/// </param>
	/// <param name="ctrlKey">true if the control key was down when the event was fired. false otherwise.</param>
	/// <param name="shiftKey">true if the shift key was down when the event was fired. false otherwise.</param>
	/// <param name="altKey">true if the alt key was down when the event was fired. false otherwise.</param>
	/// <param name="metaKey">true if the meta key was down when the event was fired. false otherwise.</param>
	/// <param name="type">Gets or sets the type of the event.</param>
	public static void ContextMenu(this IElement element, long detail = default, double screenX = default, double screenY = default, double clientX = default, double clientY = default, long button = default, long buttons = default, bool ctrlKey = default, bool shiftKey = default, bool altKey = default, bool metaKey = default, string? type = default)
		=> _ = ContextMenuAsync(element, new MouseEventArgs { Detail = detail, ScreenX = screenX, ScreenY = screenY, ClientX = clientX, ClientY = clientY, Button = button, Buttons = buttons, CtrlKey = ctrlKey, ShiftKey = shiftKey, AltKey = altKey, MetaKey = metaKey, Type = type! });

	/// <summary>
	/// Raises the <c>@oncontextmenu</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
	public static void ContextMenu(this IElement element, MouseEventArgs eventArgs)
		=> _ = ContextMenuAsync(element, eventArgs);

	/// <summary>
	/// Raises the <c>@oncontextmenu</c> event on <paramref name="element"/>,  passing the provided
	/// properties to the event handler via a <see cref="MouseEventArgs"/> object.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <param name="detail">A count of consecutive clicks that happened in a short amount of time, incremented by one.</param>
	/// <param name="screenX">The X coordinate of the mouse pointer in global (screen) coordinates.</param>
	/// <param name="screenY">The Y coordinate of the mouse pointer in global (screen) coordinates.</param>
	/// <param name="clientX">The X coordinate of the mouse pointer in local (DOM content) coordinates.</param>
	/// <param name="clientY">The Y coordinate of the mouse pointer in local (DOM content) coordinates.</param>
	/// <param name="button">
	///     The button number that was pressed when the mouse event was fired: Left button=0,
	///     middle button=1 (if present), right button=2. For mice configured for left handed
	///     use in which the button actions are reversed the values are instead read from
	///     right to left.
	/// </param>
	/// <param name="buttons">
	///     The buttons being pressed when the mouse event was fired: Left button=1, Right
	///     button=2, Middle (wheel) button=4, 4th button (typically, "Browser Back" button)=8,
	///     5th button (typically, "Browser Forward" button)=16. If two or more buttons are
	///     pressed, returns the logical sum of the values. E.g., if Left button and Right
	///     button are pressed, returns 3 (=1 | 2).
	/// </param>
	/// <param name="ctrlKey">true if the control key was down when the event was fired. false otherwise.</param>
	/// <param name="shiftKey">true if the shift key was down when the event was fired. false otherwise.</param>
	/// <param name="altKey">true if the alt key was down when the event was fired. false otherwise.</param>
	/// <param name="metaKey">true if the meta key was down when the event was fired. false otherwise.</param>
	/// <param name="type">Gets or sets the type of the event.</param>
	public static Task ContextMenuAsync(this IElement element, long detail = default, double screenX = default, double screenY = default, double clientX = default, double clientY = default, long button = default, long buttons = default, bool ctrlKey = default, bool shiftKey = default, bool altKey = default, bool metaKey = default, string? type = default)
		=> ContextMenuAsync(element, new MouseEventArgs { Detail = detail, ScreenX = screenX, ScreenY = screenY, ClientX = clientX, ClientY = clientY, Button = button, Buttons = buttons, CtrlKey = ctrlKey, ShiftKey = shiftKey, AltKey = altKey, MetaKey = metaKey, Type = type! });

	/// <summary>
	/// Raises the <c>@oncontextmenu</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
	/// <returns>A task that completes when the event handler is done.</returns>
	public static Task ContextMenuAsync(this IElement element, MouseEventArgs eventArgs) => element.TriggerEventAsync("oncontextmenu", eventArgs);
}
