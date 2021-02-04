using System.Threading.Tasks;
using AngleSharp.Dom;
using Microsoft.AspNetCore.Components.Web;

namespace Bunit
{
	/// <summary>
	/// Pointer event dispatch helper extension methods.
	/// </summary>
	public static class PointerEventDispatchExtensions
	{
		/// <summary>
		/// Raises the <c>@ongotpointercapture</c> event on <paramref name="element"/>, passing the provided
		/// properties to the event handler via a <see cref="PointerEventArgs"/> object.
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
		/// <param name="pointerId">A unique identifier for the pointer causing the event.</param>
		/// <param name="width">The width (magnitude on the X axis), in CSS pixels, of the contact geometry of the pointer.</param>
		/// <param name="height">The height (magnitude on the Y axis), in CSS pixels, of the contact geometry of the pointer.</param>
		/// <param name="pressure">The normalized pressure of the pointer input in the range of 0 to 1, where 0
		/// and 1 represent the minimum and maximum pressure the hardware is capable of detecting, respectively.</param>
		/// <param name="tiltX">The plane angle (in degrees, in the range of -90 to 90) between the Y-Z plane
		/// and the plane containing both the transducer (e.g. pen stylus) axis and the Y axis.</param>
		/// <param name="tiltY">The plane angle (in degrees, in the range of -90 to 90) between the X-Z plane
		/// and the plane containing both the transducer (e.g. pen stylus) axis and the X axis.</param>
		/// <param name="pointerType">Indicates the device type that caused the event. Must be one of the strings mouse,
		/// pen or touch, or an empty string.</param>
		/// <param name="isPrimary">Indicates if the pointer represents the primary pointer of this pointer type.</param>
		public static void GotPointerCapture(this IElement element, long detail = default, double screenX = default, double screenY = default, double clientX = default, double clientY = default, long button = default, long buttons = default, bool ctrlKey = default, bool shiftKey = default, bool altKey = default, bool metaKey = default, string? type = default, long pointerId = default, float width = default, float height = default, float pressure = default, float tiltX = default, float tiltY = default, string? pointerType = default, bool isPrimary = default)
			=> _ = GotPointerCaptureAsync(element, new PointerEventArgs
			{
				Detail = detail,
				ScreenX = screenX,
				ScreenY = screenY,
				ClientX = clientX,
				ClientY = clientY,
				Button = button,
				Buttons = buttons,
				CtrlKey = ctrlKey,
				ShiftKey = shiftKey,
				AltKey = altKey,
				MetaKey = metaKey,
				Type = type!,
				PointerId = pointerId,
				Width = width,
				Height = height,
				Pressure = pressure,
				TiltX = tiltX,
				TiltY = tiltY,
				PointerType = pointerType!,
				IsPrimary = isPrimary,
			});

		/// <summary>
		/// Raises the <c>@ongotpointercapture</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
		public static void GotPointerCapture(this IElement element, PointerEventArgs eventArgs) => _ = GotPointerCaptureAsync(element, eventArgs);

		/// <summary>
		/// Raises the <c>@ongotpointercapture</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
		/// <returns>A task that completes when the event handler is done.</returns>
		private static Task GotPointerCaptureAsync(this IElement element, PointerEventArgs eventArgs) => element.TriggerEventAsync("ongotpointercapture", eventArgs);

		/// <summary>
		/// Raises the <c>@onlostpointercapture</c> event on <paramref name="element"/>, passing the provided
		/// properties to the event handler via a <see cref="PointerEventArgs"/> object.
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
		/// <param name="pointerId">A unique identifier for the pointer causing the event.</param>
		/// <param name="width">The width (magnitude on the X axis), in CSS pixels, of the contact geometry of the pointer.</param>
		/// <param name="height">The height (magnitude on the Y axis), in CSS pixels, of the contact geometry of the pointer.</param>
		/// <param name="pressure">The normalized pressure of the pointer input in the range of 0 to 1, where 0
		/// and 1 represent the minimum and maximum pressure the hardware is capable of detecting, respectively.</param>
		/// <param name="tiltX">The plane angle (in degrees, in the range of -90 to 90) between the Y-Z plane
		/// and the plane containing both the transducer (e.g. pen stylus) axis and the Y axis.</param>
		/// <param name="tiltY">The plane angle (in degrees, in the range of -90 to 90) between the X-Z plane
		/// and the plane containing both the transducer (e.g. pen stylus) axis and the X axis.</param>
		/// <param name="pointerType">Indicates the device type that caused the event. Must be one of the strings mouse,
		/// pen or touch, or an empty string.</param>
		/// <param name="isPrimary">Indicates if the pointer represents the primary pointer of this pointer type.</param>
		public static void LostPointerCapture(this IElement element, long detail = default, double screenX = default, double screenY = default, double clientX = default, double clientY = default, long button = default, long buttons = default, bool ctrlKey = default, bool shiftKey = default, bool altKey = default, bool metaKey = default, string? type = default, long pointerId = default, float width = default, float height = default, float pressure = default, float tiltX = default, float tiltY = default, string? pointerType = default, bool isPrimary = default)
			=> _ = LostPointerCaptureAsync(element, new PointerEventArgs
			{
				Detail = detail,
				ScreenX = screenX,
				ScreenY = screenY,
				ClientX = clientX,
				ClientY = clientY,
				Button = button,
				Buttons = buttons,
				CtrlKey = ctrlKey,
				ShiftKey = shiftKey,
				AltKey = altKey,
				MetaKey = metaKey,
				Type = type!,
				PointerId = pointerId,
				Width = width,
				Height = height,
				Pressure = pressure,
				TiltX = tiltX,
				TiltY = tiltY,
				PointerType = pointerType!,
				IsPrimary = isPrimary,
			});

		/// <summary>
		/// Raises the <c>@onlostpointercapture</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
		public static void LostPointerCapture(this IElement element, PointerEventArgs eventArgs) => _ = LostPointerCaptureAsync(element, eventArgs);

		/// <summary>
		/// Raises the <c>@onlostpointercapture</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
		/// <returns>A task that completes when the event handler is done.</returns>
		private static Task LostPointerCaptureAsync(this IElement element, PointerEventArgs eventArgs) => element.TriggerEventAsync("onlostpointercapture", eventArgs);

		/// <summary>
		/// Raises the <c>@onpointerdown</c> event on <paramref name="element"/>, passing the provided
		/// properties to the event handler via a <see cref="PointerEventArgs"/> object.
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
		/// <param name="pointerId">A unique identifier for the pointer causing the event.</param>
		/// <param name="width">The width (magnitude on the X axis), in CSS pixels, of the contact geometry of the pointer.</param>
		/// <param name="height">The height (magnitude on the Y axis), in CSS pixels, of the contact geometry of the pointer.</param>
		/// <param name="pressure">The normalized pressure of the pointer input in the range of 0 to 1, where 0
		/// and 1 represent the minimum and maximum pressure the hardware is capable of detecting, respectively.</param>
		/// <param name="tiltX">The plane angle (in degrees, in the range of -90 to 90) between the Y-Z plane
		/// and the plane containing both the transducer (e.g. pen stylus) axis and the Y axis.</param>
		/// <param name="tiltY">The plane angle (in degrees, in the range of -90 to 90) between the X-Z plane
		/// and the plane containing both the transducer (e.g. pen stylus) axis and the X axis.</param>
		/// <param name="pointerType">Indicates the device type that caused the event. Must be one of the strings mouse,
		/// pen or touch, or an empty string.</param>
		/// <param name="isPrimary">Indicates if the pointer represents the primary pointer of this pointer type.</param>
		public static void PointerDown(this IElement element, long detail = default, double screenX = default, double screenY = default, double clientX = default, double clientY = default, long button = default, long buttons = default, bool ctrlKey = default, bool shiftKey = default, bool altKey = default, bool metaKey = default, string? type = default, long pointerId = default, float width = default, float height = default, float pressure = default, float tiltX = default, float tiltY = default, string? pointerType = default, bool isPrimary = default)
			=> _ = PointerDownAsync(element, new PointerEventArgs
			{
				Detail = detail,
				ScreenX = screenX,
				ScreenY = screenY,
				ClientX = clientX,
				ClientY = clientY,
				Button = button,
				Buttons = buttons,
				CtrlKey = ctrlKey,
				ShiftKey = shiftKey,
				AltKey = altKey,
				MetaKey = metaKey,
				Type = type!,
				PointerId = pointerId,
				Width = width,
				Height = height,
				Pressure = pressure,
				TiltX = tiltX,
				TiltY = tiltY,
				PointerType = pointerType!,
				IsPrimary = isPrimary,
			});

		/// <summary>
		/// Raises the <c>@onpointerdown</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
		public static void PointerDown(this IElement element, PointerEventArgs eventArgs) => _ = PointerDownAsync(element, eventArgs);

		/// <summary>
		/// Raises the <c>@onpointerdown</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
		/// <returns>A task that completes when the event handler is done.</returns>
		private static Task PointerDownAsync(this IElement element, PointerEventArgs eventArgs) => element.TriggerEventAsync("onpointerdown", eventArgs);

		/// <summary>
		/// Raises the <c>@onpointercancel</c> event on <paramref name="element"/>, passing the provided
		/// properties to the event handler via a <see cref="PointerEventArgs"/> object.
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
		/// <param name="pointerId">A unique identifier for the pointer causing the event.</param>
		/// <param name="width">The width (magnitude on the X axis), in CSS pixels, of the contact geometry of the pointer.</param>
		/// <param name="height">The height (magnitude on the Y axis), in CSS pixels, of the contact geometry of the pointer.</param>
		/// <param name="pressure">The normalized pressure of the pointer input in the range of 0 to 1, where 0
		/// and 1 represent the minimum and maximum pressure the hardware is capable of detecting, respectively.</param>
		/// <param name="tiltX">The plane angle (in degrees, in the range of -90 to 90) between the Y-Z plane
		/// and the plane containing both the transducer (e.g. pen stylus) axis and the Y axis.</param>
		/// <param name="tiltY">The plane angle (in degrees, in the range of -90 to 90) between the X-Z plane
		/// and the plane containing both the transducer (e.g. pen stylus) axis and the X axis.</param>
		/// <param name="pointerType">Indicates the device type that caused the event. Must be one of the strings mouse,
		/// pen or touch, or an empty string.</param>
		/// <param name="isPrimary">Indicates if the pointer represents the primary pointer of this pointer type.</param>
		public static void PointerCancel(this IElement element, long detail = default, double screenX = default, double screenY = default, double clientX = default, double clientY = default, long button = default, long buttons = default, bool ctrlKey = default, bool shiftKey = default, bool altKey = default, bool metaKey = default, string? type = default, long pointerId = default, float width = default, float height = default, float pressure = default, float tiltX = default, float tiltY = default, string? pointerType = default, bool isPrimary = default)
			=> _ = PointerCancelAsync(element, new PointerEventArgs
			{
				Detail = detail,
				ScreenX = screenX,
				ScreenY = screenY,
				ClientX = clientX,
				ClientY = clientY,
				Button = button,
				Buttons = buttons,
				CtrlKey = ctrlKey,
				ShiftKey = shiftKey,
				AltKey = altKey,
				MetaKey = metaKey,
				Type = type!,
				PointerId = pointerId,
				Width = width,
				Height = height,
				Pressure = pressure,
				TiltX = tiltX,
				TiltY = tiltY,
				PointerType = pointerType!,
				IsPrimary = isPrimary,
			});

		/// <summary>
		/// Raises the <c>@onpointercancel</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
		public static void PointerCancel(this IElement element, PointerEventArgs eventArgs) => _ = PointerCancelAsync(element, eventArgs);

		/// <summary>
		/// Raises the <c>@onpointercancel</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
		/// <returns>A task that completes when the event handler is done.</returns>
		private static Task PointerCancelAsync(this IElement element, PointerEventArgs eventArgs) => element.TriggerEventAsync("onpointercancel", eventArgs);

		/// <summary>
		/// Raises the <c>@onpointerenter</c> event on <paramref name="element"/>, passing the provided
		/// properties to the event handler via a <see cref="PointerEventArgs"/> object.
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
		/// <param name="pointerId">A unique identifier for the pointer causing the event.</param>
		/// <param name="width">The width (magnitude on the X axis), in CSS pixels, of the contact geometry of the pointer.</param>
		/// <param name="height">The height (magnitude on the Y axis), in CSS pixels, of the contact geometry of the pointer.</param>
		/// <param name="pressure">The normalized pressure of the pointer input in the range of 0 to 1, where 0
		/// and 1 represent the minimum and maximum pressure the hardware is capable of detecting, respectively.</param>
		/// <param name="tiltX">The plane angle (in degrees, in the range of -90 to 90) between the Y-Z plane
		/// and the plane containing both the transducer (e.g. pen stylus) axis and the Y axis.</param>
		/// <param name="tiltY">The plane angle (in degrees, in the range of -90 to 90) between the X-Z plane
		/// and the plane containing both the transducer (e.g. pen stylus) axis and the X axis.</param>
		/// <param name="pointerType">Indicates the device type that caused the event. Must be one of the strings mouse,
		/// pen or touch, or an empty string.</param>
		/// <param name="isPrimary">Indicates if the pointer represents the primary pointer of this pointer type.</param>
		public static void PointerEnter(this IElement element, long detail = default, double screenX = default, double screenY = default, double clientX = default, double clientY = default, long button = default, long buttons = default, bool ctrlKey = default, bool shiftKey = default, bool altKey = default, bool metaKey = default, string? type = default, long pointerId = default, float width = default, float height = default, float pressure = default, float tiltX = default, float tiltY = default, string? pointerType = default, bool isPrimary = default)
			=> _ = PointerEnterAsync(element, new PointerEventArgs
			{
				Detail = detail,
				ScreenX = screenX,
				ScreenY = screenY,
				ClientX = clientX,
				ClientY = clientY,
				Button = button,
				Buttons = buttons,
				CtrlKey = ctrlKey,
				ShiftKey = shiftKey,
				AltKey = altKey,
				MetaKey = metaKey,
				Type = type!,
				PointerId = pointerId,
				Width = width,
				Height = height,
				Pressure = pressure,
				TiltX = tiltX,
				TiltY = tiltY,
				PointerType = pointerType!,
				IsPrimary = isPrimary,
			});

		/// <summary>
		/// Raises the <c>@onpointerenter</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
		public static void PointerEnter(this IElement element, PointerEventArgs eventArgs) => _ = PointerEnterAsync(element, eventArgs);

		/// <summary>
		/// Raises the <c>@onpointerenter</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
		/// <returns>A task that completes when the event handler is done.</returns>
		private static Task PointerEnterAsync(this IElement element, PointerEventArgs eventArgs) => element.TriggerEventAsync("onpointerenter", eventArgs);

		/// <summary>
		/// Raises the <c>@onpointerleave</c> event on <paramref name="element"/>, passing the provided
		/// properties to the event handler via a <see cref="PointerEventArgs"/> object.
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
		/// <param name="pointerId">A unique identifier for the pointer causing the event.</param>
		/// <param name="width">The width (magnitude on the X axis), in CSS pixels, of the contact geometry of the pointer.</param>
		/// <param name="height">The height (magnitude on the Y axis), in CSS pixels, of the contact geometry of the pointer.</param>
		/// <param name="pressure">The normalized pressure of the pointer input in the range of 0 to 1, where 0
		/// and 1 represent the minimum and maximum pressure the hardware is capable of detecting, respectively.</param>
		/// <param name="tiltX">The plane angle (in degrees, in the range of -90 to 90) between the Y-Z plane
		/// and the plane containing both the transducer (e.g. pen stylus) axis and the Y axis.</param>
		/// <param name="tiltY">The plane angle (in degrees, in the range of -90 to 90) between the X-Z plane
		/// and the plane containing both the transducer (e.g. pen stylus) axis and the X axis.</param>
		/// <param name="pointerType">Indicates the device type that caused the event. Must be one of the strings mouse,
		/// pen or touch, or an empty string.</param>
		/// <param name="isPrimary">Indicates if the pointer represents the primary pointer of this pointer type.</param>
		public static void PointerLeave(this IElement element, long detail = default, double screenX = default, double screenY = default, double clientX = default, double clientY = default, long button = default, long buttons = default, bool ctrlKey = default, bool shiftKey = default, bool altKey = default, bool metaKey = default, string? type = default, long pointerId = default, float width = default, float height = default, float pressure = default, float tiltX = default, float tiltY = default, string? pointerType = default, bool isPrimary = default)
			=> _ = PointerLeaveAsync(element, new PointerEventArgs
			{
				Detail = detail,
				ScreenX = screenX,
				ScreenY = screenY,
				ClientX = clientX,
				ClientY = clientY,
				Button = button,
				Buttons = buttons,
				CtrlKey = ctrlKey,
				ShiftKey = shiftKey,
				AltKey = altKey,
				MetaKey = metaKey,
				Type = type!,
				PointerId = pointerId,
				Width = width,
				Height = height,
				Pressure = pressure,
				TiltX = tiltX,
				TiltY = tiltY,
				PointerType = pointerType!,
				IsPrimary = isPrimary,
			});

		/// <summary>
		/// Raises the <c>@onpointerleave</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
		public static void PointerLeave(this IElement element, PointerEventArgs eventArgs) => _ = PointerLeaveAsync(element, eventArgs);

		/// <summary>
		/// Raises the <c>@onpointerleave</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
		/// <returns>A task that completes when the event handler is done.</returns>
		private static Task PointerLeaveAsync(this IElement element, PointerEventArgs eventArgs) => element.TriggerEventAsync("onpointerleave", eventArgs);

		/// <summary>
		/// Raises the <c>@onpointermove</c> event on <paramref name="element"/>, passing the provided
		/// properties to the event handler via a <see cref="PointerEventArgs"/> object.
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
		/// <param name="pointerId">A unique identifier for the pointer causing the event.</param>
		/// <param name="width">The width (magnitude on the X axis), in CSS pixels, of the contact geometry of the pointer.</param>
		/// <param name="height">The height (magnitude on the Y axis), in CSS pixels, of the contact geometry of the pointer.</param>
		/// <param name="pressure">The normalized pressure of the pointer input in the range of 0 to 1, where 0
		/// and 1 represent the minimum and maximum pressure the hardware is capable of detecting, respectively.</param>
		/// <param name="tiltX">The plane angle (in degrees, in the range of -90 to 90) between the Y-Z plane
		/// and the plane containing both the transducer (e.g. pen stylus) axis and the Y axis.</param>
		/// <param name="tiltY">The plane angle (in degrees, in the range of -90 to 90) between the X-Z plane
		/// and the plane containing both the transducer (e.g. pen stylus) axis and the X axis.</param>
		/// <param name="pointerType">Indicates the device type that caused the event. Must be one of the strings mouse,
		/// pen or touch, or an empty string.</param>
		/// <param name="isPrimary">Indicates if the pointer represents the primary pointer of this pointer type.</param>
		public static void PointerMove(this IElement element, long detail = default, double screenX = default, double screenY = default, double clientX = default, double clientY = default, long button = default, long buttons = default, bool ctrlKey = default, bool shiftKey = default, bool altKey = default, bool metaKey = default, string? type = default, long pointerId = default, float width = default, float height = default, float pressure = default, float tiltX = default, float tiltY = default, string? pointerType = default, bool isPrimary = default)
			=> _ = PointerMoveAsync(element, new PointerEventArgs
			{
				Detail = detail,
				ScreenX = screenX,
				ScreenY = screenY,
				ClientX = clientX,
				ClientY = clientY,
				Button = button,
				Buttons = buttons,
				CtrlKey = ctrlKey,
				ShiftKey = shiftKey,
				AltKey = altKey,
				MetaKey = metaKey,
				Type = type!,
				PointerId = pointerId,
				Width = width,
				Height = height,
				Pressure = pressure,
				TiltX = tiltX,
				TiltY = tiltY,
				PointerType = pointerType!,
				IsPrimary = isPrimary,
			});

		/// <summary>
		/// Raises the <c>@onpointermove</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
		public static void PointerMove(this IElement element, PointerEventArgs eventArgs) => _ = PointerMoveAsync(element, eventArgs);

		/// <summary>
		/// Raises the <c>@onpointermove</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
		/// <returns>A task that completes when the event handler is done.</returns>
		private static Task PointerMoveAsync(this IElement element, PointerEventArgs eventArgs) => element.TriggerEventAsync("onpointermove", eventArgs);

		/// <summary>
		/// Raises the <c>@onpointerout</c> event on <paramref name="element"/>, passing the provided
		/// properties to the event handler via a <see cref="PointerEventArgs"/> object.
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
		/// <param name="pointerId">A unique identifier for the pointer causing the event.</param>
		/// <param name="width">The width (magnitude on the X axis), in CSS pixels, of the contact geometry of the pointer.</param>
		/// <param name="height">The height (magnitude on the Y axis), in CSS pixels, of the contact geometry of the pointer.</param>
		/// <param name="pressure">The normalized pressure of the pointer input in the range of 0 to 1, where 0
		/// and 1 represent the minimum and maximum pressure the hardware is capable of detecting, respectively.</param>
		/// <param name="tiltX">The plane angle (in degrees, in the range of -90 to 90) between the Y-Z plane
		/// and the plane containing both the transducer (e.g. pen stylus) axis and the Y axis.</param>
		/// <param name="tiltY">The plane angle (in degrees, in the range of -90 to 90) between the X-Z plane
		/// and the plane containing both the transducer (e.g. pen stylus) axis and the X axis.</param>
		/// <param name="pointerType">Indicates the device type that caused the event. Must be one of the strings mouse,
		/// pen or touch, or an empty string.</param>
		/// <param name="isPrimary">Indicates if the pointer represents the primary pointer of this pointer type.</param>
		public static void PointerOut(this IElement element, long detail = default, double screenX = default, double screenY = default, double clientX = default, double clientY = default, long button = default, long buttons = default, bool ctrlKey = default, bool shiftKey = default, bool altKey = default, bool metaKey = default, string? type = default, long pointerId = default, float width = default, float height = default, float pressure = default, float tiltX = default, float tiltY = default, string? pointerType = default, bool isPrimary = default)
			=> _ = PointerOutAsync(element, new PointerEventArgs
			{
				Detail = detail,
				ScreenX = screenX,
				ScreenY = screenY,
				ClientX = clientX,
				ClientY = clientY,
				Button = button,
				Buttons = buttons,
				CtrlKey = ctrlKey,
				ShiftKey = shiftKey,
				AltKey = altKey,
				MetaKey = metaKey,
				Type = type!,
				PointerId = pointerId,
				Width = width,
				Height = height,
				Pressure = pressure,
				TiltX = tiltX,
				TiltY = tiltY,
				PointerType = pointerType!,
				IsPrimary = isPrimary,
			});

		/// <summary>
		/// Raises the <c>@onpointerout</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
		public static void PointerOut(this IElement element, PointerEventArgs eventArgs) => _ = PointerOutAsync(element, eventArgs);

		/// <summary>
		/// Raises the <c>@onpointerout</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
		/// <returns>A task that completes when the event handler is done.</returns>
		private static Task PointerOutAsync(this IElement element, PointerEventArgs eventArgs) => element.TriggerEventAsync("onpointerout", eventArgs);

		/// <summary>
		/// Raises the <c>@onpointerover</c> event on <paramref name="element"/>, passing the provided
		/// properties to the event handler via a <see cref="PointerEventArgs"/> object.
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
		/// <param name="pointerId">A unique identifier for the pointer causing the event.</param>
		/// <param name="width">The width (magnitude on the X axis), in CSS pixels, of the contact geometry of the pointer.</param>
		/// <param name="height">The height (magnitude on the Y axis), in CSS pixels, of the contact geometry of the pointer.</param>
		/// <param name="pressure">The normalized pressure of the pointer input in the range of 0 to 1, where 0
		/// and 1 represent the minimum and maximum pressure the hardware is capable of detecting, respectively.</param>
		/// <param name="tiltX">The plane angle (in degrees, in the range of -90 to 90) between the Y-Z plane
		/// and the plane containing both the transducer (e.g. pen stylus) axis and the Y axis.</param>
		/// <param name="tiltY">The plane angle (in degrees, in the range of -90 to 90) between the X-Z plane
		/// and the plane containing both the transducer (e.g. pen stylus) axis and the X axis.</param>
		/// <param name="pointerType">Indicates the device type that caused the event. Must be one of the strings mouse,
		/// pen or touch, or an empty string.</param>
		/// <param name="isPrimary">Indicates if the pointer represents the primary pointer of this pointer type.</param>
		public static void PointerOver(this IElement element, long detail = default, double screenX = default, double screenY = default, double clientX = default, double clientY = default, long button = default, long buttons = default, bool ctrlKey = default, bool shiftKey = default, bool altKey = default, bool metaKey = default, string? type = default, long pointerId = default, float width = default, float height = default, float pressure = default, float tiltX = default, float tiltY = default, string? pointerType = default, bool isPrimary = default)
			=> _ = PointerOverAsync(element, new PointerEventArgs
			{
				Detail = detail,
				ScreenX = screenX,
				ScreenY = screenY,
				ClientX = clientX,
				ClientY = clientY,
				Button = button,
				Buttons = buttons,
				CtrlKey = ctrlKey,
				ShiftKey = shiftKey,
				AltKey = altKey,
				MetaKey = metaKey,
				Type = type!,
				PointerId = pointerId,
				Width = width,
				Height = height,
				Pressure = pressure,
				TiltX = tiltX,
				TiltY = tiltY,
				PointerType = pointerType!,
				IsPrimary = isPrimary,
			});

		/// <summary>
		/// Raises the <c>@onpointerover</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
		public static void PointerOver(this IElement element, PointerEventArgs eventArgs) => _ = PointerOverAsync(element, eventArgs);

		/// <summary>
		/// Raises the <c>@onpointerover</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
		/// <returns>A task that completes when the event handler is done.</returns>
		private static Task PointerOverAsync(this IElement element, PointerEventArgs eventArgs) => element.TriggerEventAsync("onpointerover", eventArgs);

		/// <summary>
		/// Raises the <c>@onpointerup</c> event on <paramref name="element"/>, passing the provided
		/// properties to the event handler via a <see cref="PointerEventArgs"/> object.
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
		/// <param name="pointerId">A unique identifier for the pointer causing the event.</param>
		/// <param name="width">The width (magnitude on the X axis), in CSS pixels, of the contact geometry of the pointer.</param>
		/// <param name="height">The height (magnitude on the Y axis), in CSS pixels, of the contact geometry of the pointer.</param>
		/// <param name="pressure">The normalized pressure of the pointer input in the range of 0 to 1, where 0
		/// and 1 represent the minimum and maximum pressure the hardware is capable of detecting, respectively.</param>
		/// <param name="tiltX">The plane angle (in degrees, in the range of -90 to 90) between the Y-Z plane
		/// and the plane containing both the transducer (e.g. pen stylus) axis and the Y axis.</param>
		/// <param name="tiltY">The plane angle (in degrees, in the range of -90 to 90) between the X-Z plane
		/// and the plane containing both the transducer (e.g. pen stylus) axis and the X axis.</param>
		/// <param name="pointerType">Indicates the device type that caused the event. Must be one of the strings mouse,
		/// pen or touch, or an empty string.</param>
		/// <param name="isPrimary">Indicates if the pointer represents the primary pointer of this pointer type.</param>
		public static void PointerUp(this IElement element, long detail = default, double screenX = default, double screenY = default, double clientX = default, double clientY = default, long button = default, long buttons = default, bool ctrlKey = default, bool shiftKey = default, bool altKey = default, bool metaKey = default, string? type = default, long pointerId = default, float width = default, float height = default, float pressure = default, float tiltX = default, float tiltY = default, string? pointerType = default, bool isPrimary = default)
			=> _ = PointerUpAsync(element, new PointerEventArgs
			{
				Detail = detail,
				ScreenX = screenX,
				ScreenY = screenY,
				ClientX = clientX,
				ClientY = clientY,
				Button = button,
				Buttons = buttons,
				CtrlKey = ctrlKey,
				ShiftKey = shiftKey,
				AltKey = altKey,
				MetaKey = metaKey,
				Type = type!,
				PointerId = pointerId,
				Width = width,
				Height = height,
				Pressure = pressure,
				TiltX = tiltX,
				TiltY = tiltY,
				PointerType = pointerType!,
				IsPrimary = isPrimary,
			});

		/// <summary>
		/// Raises the <c>@onpointerup</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
		public static void PointerUp(this IElement element, PointerEventArgs eventArgs) => _ = PointerUpAsync(element, eventArgs);

		/// <summary>
		/// Raises the <c>@onpointerup</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
		/// <returns>A task that completes when the event handler is done.</returns>
		private static Task PointerUpAsync(this IElement element, PointerEventArgs eventArgs) => element.TriggerEventAsync("onpointerup", eventArgs);
	}
}
