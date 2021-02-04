using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using AngleSharp.Dom;
using Microsoft.AspNetCore.Components.Web;

namespace Bunit
{
	/// <summary>
	/// Pointer event dispatch helper extension methods.
	/// </summary>
	public static class TouchEventDispatchExtensions
	{
		/// <summary>
		/// Raises the <c>@ontouchcancel</c> event on <paramref name="element"/>, passing the provided
		/// properties to the event handler via a <see cref="TouchEventArgs"/> object.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		/// <param name="detail">A count of consecutive clicks that happened in a short amount of time, incremented by one.</param>
		/// <param name="touches">A list of <see cref="TouchPoint"/> for every point of contact currently touching the surface.</param>
		/// <param name="targetTouches">A list of <see cref="TouchPoint"/> for every point of contact that is touching the surface and started on the element that is the target of the current event.</param>
		/// <param name="changedTouches">A list of Touches for every point of contact which contributed to the event.
		/// For the touchstart event this must be a list of the touch points that just became
		/// active with the current event. For the touchmove event this must be a list of
		/// the touch points that have moved since the last event. For the touchend and touchcancel
		/// events this must be a list of the touch points that have just been removed from
		/// the surface.
		/// </param>
		/// <param name="ctrlKey">true if the control key was down when the event was fired. false otherwise.</param>
		/// <param name="shiftKey">true if the shift key was down when the event was fired. false otherwise.</param>
		/// <param name="altKey">true if the alt key was down when the event was fired. false otherwise.</param>
		/// <param name="metaKey">true if the meta key was down when the event was fired. false otherwise.</param>
		/// <param name="type">Gets or sets the type of the event.</param>
		public static void TouchCancel(this IElement element, long detail = default, TouchPoint[]? touches = default, TouchPoint[]? targetTouches = default, TouchPoint[]? changedTouches = default, bool ctrlKey = default, bool shiftKey = default, bool altKey = default, bool metaKey = default, string? type = default)
			=> _ = TouchCancelAsync(element, new TouchEventArgs { Detail = detail, Touches = touches ?? Array.Empty<TouchPoint>(), TargetTouches = targetTouches ?? Array.Empty<TouchPoint>(), ChangedTouches = changedTouches ?? Array.Empty<TouchPoint>(), CtrlKey = ctrlKey, ShiftKey = shiftKey, AltKey = altKey, MetaKey = metaKey, Type = type! });

		/// <summary>
		/// Raises the <c>@ontouchcancel</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
		public static void TouchCancel(this IElement element, TouchEventArgs eventArgs)
			=> _ = TouchCancelAsync(element, eventArgs);

		/// <summary>
		/// Raises the <c>@ontouchcancel</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
		/// <returns>A task that completes when the event handler is done.</returns>
		private static Task TouchCancelAsync(this IElement element, TouchEventArgs eventArgs)
			=> element.TriggerEventAsync("ontouchcancel", eventArgs);

		/// <summary>
		/// Raises the <c>@ontouchend</c> event on <paramref name="element"/>, passing the provided
		/// properties to the event handler via a <see cref="TouchEventArgs"/> object.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		/// <param name="detail">A count of consecutive clicks that happened in a short amount of time, incremented by one.</param>
		/// <param name="touches">A list of <see cref="TouchPoint"/> for every point of contact currently touching the surface.</param>
		/// <param name="targetTouches">A list of <see cref="TouchPoint"/> for every point of contact that is touching the surface and started on the element that is the target of the current event.</param>
		/// <param name="changedTouches">A list of Touches for every point of contact which contributed to the event.
		/// For the touchstart event this must be a list of the touch points that just became
		/// active with the current event. For the touchmove event this must be a list of
		/// the touch points that have moved since the last event. For the touchend and touchcancel
		/// events this must be a list of the touch points that have just been removed from
		/// the surface.
		/// </param>
		/// <param name="ctrlKey">true if the control key was down when the event was fired. false otherwise.</param>
		/// <param name="shiftKey">true if the shift key was down when the event was fired. false otherwise.</param>
		/// <param name="altKey">true if the alt key was down when the event was fired. false otherwise.</param>
		/// <param name="metaKey">true if the meta key was down when the event was fired. false otherwise.</param>
		/// <param name="type">Gets or sets the type of the event.</param>
		public static void TouchEnd(this IElement element, long detail = default, TouchPoint[]? touches = default, TouchPoint[]? targetTouches = default, TouchPoint[]? changedTouches = default, bool ctrlKey = default, bool shiftKey = default, bool altKey = default, bool metaKey = default, string? type = default)
			=> _ = TouchEndAsync(element, new TouchEventArgs { Detail = detail, Touches = touches ?? Array.Empty<TouchPoint>(), TargetTouches = targetTouches ?? Array.Empty<TouchPoint>(), ChangedTouches = changedTouches ?? Array.Empty<TouchPoint>(), CtrlKey = ctrlKey, ShiftKey = shiftKey, AltKey = altKey, MetaKey = metaKey, Type = type! });

		/// <summary>
		/// Raises the <c>@ontouchend</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
		public static void TouchEnd(this IElement element, TouchEventArgs eventArgs)
			=> _ = TouchEndAsync(element, eventArgs);

		/// <summary>
		/// Raises the <c>@ontouchend</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
		/// <returns>A task that completes when the event handler is done.</returns>
		private static Task TouchEndAsync(this IElement element, TouchEventArgs eventArgs)
			=> element.TriggerEventAsync("ontouchend", eventArgs);

		/// <summary>
		/// Raises the <c>@ontouchmove</c> event on <paramref name="element"/>, passing the provided
		/// properties to the event handler via a <see cref="TouchEventArgs"/> object.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		/// <param name="detail">A count of consecutive clicks that happened in a short amount of time, incremented by one.</param>
		/// <param name="touches">A list of <see cref="TouchPoint"/> for every point of contact currently touching the surface.</param>
		/// <param name="targetTouches">A list of <see cref="TouchPoint"/> for every point of contact that is touching the surface and started on the element that is the target of the current event.</param>
		/// <param name="changedTouches">A list of Touches for every point of contact which contributed to the event.
		/// For the touchstart event this must be a list of the touch points that just became
		/// active with the current event. For the touchmove event this must be a list of
		/// the touch points that have moved since the last event. For the touchend and touchcancel
		/// events this must be a list of the touch points that have just been removed from
		/// the surface.
		/// </param>
		/// <param name="ctrlKey">true if the control key was down when the event was fired. false otherwise.</param>
		/// <param name="shiftKey">true if the shift key was down when the event was fired. false otherwise.</param>
		/// <param name="altKey">true if the alt key was down when the event was fired. false otherwise.</param>
		/// <param name="metaKey">true if the meta key was down when the event was fired. false otherwise.</param>
		/// <param name="type">Gets or sets the type of the event.</param>
		public static void TouchMove(this IElement element, long detail = default, TouchPoint[]? touches = default, TouchPoint[]? targetTouches = default, TouchPoint[]? changedTouches = default, bool ctrlKey = default, bool shiftKey = default, bool altKey = default, bool metaKey = default, string? type = default)
			=> _ = TouchMoveAsync(element, new TouchEventArgs { Detail = detail, Touches = touches ?? Array.Empty<TouchPoint>(), TargetTouches = targetTouches ?? Array.Empty<TouchPoint>(), ChangedTouches = changedTouches ?? Array.Empty<TouchPoint>(), CtrlKey = ctrlKey, ShiftKey = shiftKey, AltKey = altKey, MetaKey = metaKey, Type = type! });

		/// <summary>
		/// Raises the <c>@ontouchmove</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
		public static void TouchMove(this IElement element, TouchEventArgs eventArgs)
			=> _ = TouchMoveAsync(element, eventArgs);

		/// <summary>
		/// Raises the <c>@ontouchmove</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
		/// <returns>A task that completes when the event handler is done.</returns>
		private static Task TouchMoveAsync(this IElement element, TouchEventArgs eventArgs)
			=> element.TriggerEventAsync("ontouchmove", eventArgs);

		/// <summary>
		/// Raises the <c>@ontouchstart</c> event on <paramref name="element"/>, passing the provided
		/// properties to the event handler via a <see cref="TouchEventArgs"/> object.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		/// <param name="detail">A count of consecutive clicks that happened in a short amount of time, incremented by one.</param>
		/// <param name="touches">A list of <see cref="TouchPoint"/> for every point of contact currently touching the surface.</param>
		/// <param name="targetTouches">A list of <see cref="TouchPoint"/> for every point of contact that is touching the surface and started on the element that is the target of the current event.</param>
		/// <param name="changedTouches">A list of Touches for every point of contact which contributed to the event.
		/// For the touchstart event this must be a list of the touch points that just became
		/// active with the current event. For the touchmove event this must be a list of
		/// the touch points that have moved since the last event. For the touchend and touchcancel
		/// events this must be a list of the touch points that have just been removed from
		/// the surface.
		/// </param>
		/// <param name="ctrlKey">true if the control key was down when the event was fired. false otherwise.</param>
		/// <param name="shiftKey">true if the shift key was down when the event was fired. false otherwise.</param>
		/// <param name="altKey">true if the alt key was down when the event was fired. false otherwise.</param>
		/// <param name="metaKey">true if the meta key was down when the event was fired. false otherwise.</param>
		/// <param name="type">Gets or sets the type of the event.</param>
		public static void TouchStart(this IElement element, long detail = default, TouchPoint[]? touches = default, TouchPoint[]? targetTouches = default, TouchPoint[]? changedTouches = default, bool ctrlKey = default, bool shiftKey = default, bool altKey = default, bool metaKey = default, string? type = default)
			=> _ = TouchStartAsync(element, new TouchEventArgs { Detail = detail, Touches = touches ?? Array.Empty<TouchPoint>(), TargetTouches = targetTouches ?? Array.Empty<TouchPoint>(), ChangedTouches = changedTouches ?? Array.Empty<TouchPoint>(), CtrlKey = ctrlKey, ShiftKey = shiftKey, AltKey = altKey, MetaKey = metaKey, Type = type! });

		/// <summary>
		/// Raises the <c>@ontouchstart</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
		public static void TouchStart(this IElement element, TouchEventArgs eventArgs)
			=> _ = TouchStartAsync(element, eventArgs);

		/// <summary>
		/// Raises the <c>@ontouchstart</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
		/// <returns>A task that completes when the event handler is done.</returns>
		private static Task TouchStartAsync(this IElement element, TouchEventArgs eventArgs)
			=> element.TriggerEventAsync("ontouchstart", eventArgs);

		/// <summary>
		/// Raises the <c>@ontouchenter</c> event on <paramref name="element"/>, passing the provided
		/// properties to the event handler via a <see cref="TouchEventArgs"/> object.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		/// <param name="detail">A count of consecutive clicks that happened in a short amount of time, incremented by one.</param>
		/// <param name="touches">A list of <see cref="TouchPoint"/> for every point of contact currently touching the surface.</param>
		/// <param name="targetTouches">A list of <see cref="TouchPoint"/> for every point of contact that is touching the surface and started on the element that is the target of the current event.</param>
		/// <param name="changedTouches">A list of Touches for every point of contact which contributed to the event.
		/// For the touchstart event this must be a list of the touch points that just became
		/// active with the current event. For the touchmove event this must be a list of
		/// the touch points that have moved since the last event. For the touchend and touchcancel
		/// events this must be a list of the touch points that have just been removed from
		/// the surface.
		/// </param>
		/// <param name="ctrlKey">true if the control key was down when the event was fired. false otherwise.</param>
		/// <param name="shiftKey">true if the shift key was down when the event was fired. false otherwise.</param>
		/// <param name="altKey">true if the alt key was down when the event was fired. false otherwise.</param>
		/// <param name="metaKey">true if the meta key was down when the event was fired. false otherwise.</param>
		/// <param name="type">Gets or sets the type of the event.</param>
		public static void TouchEnter(this IElement element, long detail = default, TouchPoint[]? touches = default, TouchPoint[]? targetTouches = default, TouchPoint[]? changedTouches = default, bool ctrlKey = default, bool shiftKey = default, bool altKey = default, bool metaKey = default, string? type = default)
			=> _ = TouchEnterAsync(element, new TouchEventArgs { Detail = detail, Touches = touches ?? Array.Empty<TouchPoint>(), TargetTouches = targetTouches ?? Array.Empty<TouchPoint>(), ChangedTouches = changedTouches ?? Array.Empty<TouchPoint>(), CtrlKey = ctrlKey, ShiftKey = shiftKey, AltKey = altKey, MetaKey = metaKey, Type = type! });

		/// <summary>
		/// Raises the <c>@ontouchenter</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
		public static void TouchEnter(this IElement element, TouchEventArgs eventArgs)
			=> _ = TouchEnterAsync(element, eventArgs);

		/// <summary>
		/// Raises the <c>@ontouchenter</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
		/// <returns>A task that completes when the event handler is done.</returns>
		private static Task TouchEnterAsync(this IElement element, TouchEventArgs eventArgs)
			=> element.TriggerEventAsync("ontouchenter", eventArgs);

		/// <summary>
		/// Raises the <c>@ontouchleave</c> event on <paramref name="element"/>, passing the provided
		/// properties to the event handler via a <see cref="TouchEventArgs"/> object.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		/// <param name="detail">A count of consecutive clicks that happened in a short amount of time, incremented by one.</param>
		/// <param name="touches">A list of <see cref="TouchPoint"/> for every point of contact currently touching the surface.</param>
		/// <param name="targetTouches">A list of <see cref="TouchPoint"/> for every point of contact that is touching the surface and started on the element that is the target of the current event.</param>
		/// <param name="changedTouches">A list of Touches for every point of contact which contributed to the event.
		/// For the touchstart event this must be a list of the touch points that just became
		/// active with the current event. For the touchmove event this must be a list of
		/// the touch points that have moved since the last event. For the touchend and touchcancel
		/// events this must be a list of the touch points that have just been removed from
		/// the surface.
		/// </param>
		/// <param name="ctrlKey">true if the control key was down when the event was fired. false otherwise.</param>
		/// <param name="shiftKey">true if the shift key was down when the event was fired. false otherwise.</param>
		/// <param name="altKey">true if the alt key was down when the event was fired. false otherwise.</param>
		/// <param name="metaKey">true if the meta key was down when the event was fired. false otherwise.</param>
		/// <param name="type">Gets or sets the type of the event.</param>
		public static void TouchLeave(this IElement element, long detail = default, TouchPoint[]? touches = default, TouchPoint[]? targetTouches = default, TouchPoint[]? changedTouches = default, bool ctrlKey = default, bool shiftKey = default, bool altKey = default, bool metaKey = default, string? type = default)
			=> _ = TouchLeaveAsync(element, new TouchEventArgs { Detail = detail, Touches = touches ?? Array.Empty<TouchPoint>(), TargetTouches = targetTouches ?? Array.Empty<TouchPoint>(), ChangedTouches = changedTouches ?? Array.Empty<TouchPoint>(), CtrlKey = ctrlKey, ShiftKey = shiftKey, AltKey = altKey, MetaKey = metaKey, Type = type! });

		/// <summary>
		/// Raises the <c>@ontouchleave</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
		public static void TouchLeave(this IElement element, TouchEventArgs eventArgs)
			=> _ = TouchLeaveAsync(element, eventArgs);

		/// <summary>
		/// Raises the <c>@ontouchleave</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
		/// <returns>A task that completes when the event handler is done.</returns>
		private static Task TouchLeaveAsync(this IElement element, TouchEventArgs eventArgs)
			=> element.TriggerEventAsync("ontouchleave", eventArgs);
	}
}
