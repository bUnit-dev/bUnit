using System.Threading.Tasks;
using AngleSharp.Dom;
using Microsoft.AspNetCore.Components.Web;

namespace Bunit
{
	/// <summary>
	/// Drag event dispatch helper extension methods.
	/// </summary>
	public static class DragEventDispatchExtensions
	{
		/// <summary>
		/// Raises the <c>@ondrag</c> event on <paramref name="element"/>, passing the provided
		/// properties to the event handler via a <see cref="DragEventArgs"/> object.
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
		/// <param name="dataTransfer">The data that underlies a drag-and-drop operation, known as the drag data store. See <see cref="DataTransfer"/>.</param>
		public static void Drag(this IElement element, long detail = default, double screenX = default, double screenY = default, double clientX = default, double clientY = default, long button = default, long buttons = default, bool ctrlKey = default, bool shiftKey = default, bool altKey = default, bool metaKey = default, string type = default!, DataTransfer dataTransfer = default!)
			=> _ = DragAsync(element, new DragEventArgs { Detail = detail, ScreenX = screenX, ScreenY = screenY, ClientX = clientX, ClientY = clientY, Button = button, Buttons = buttons, CtrlKey = ctrlKey, ShiftKey = shiftKey, AltKey = altKey, MetaKey = metaKey, Type = type, DataTransfer = dataTransfer });

		/// <summary>
		/// Raises the <c>@ondrag</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
		public static void Drag(this IElement element, DragEventArgs eventArgs) => _ = DragAsync(element, eventArgs);

		/// <summary>
		/// Raises the <c>@ondrag</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
		/// <returns>A task that completes when the event handler is done.</returns>
		private static Task DragAsync(this IElement element, DragEventArgs eventArgs) => element.TriggerEventAsync("ondrag", eventArgs);

		/// <summary>
		/// Raises the <c>@ondragend</c> event on <paramref name="element"/>, passing the provided
		/// properties to the event handler via a <see cref="DragEventArgs"/> object.
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
		/// <param name="dataTransfer">The data that underlies a drag-and-drop operation, known as the drag data store. See <see cref="DataTransfer"/>.</param>
		public static void DragEnd(this IElement element, long detail = default, double screenX = default, double screenY = default, double clientX = default, double clientY = default, long button = default, long buttons = default, bool ctrlKey = default, bool shiftKey = default, bool altKey = default, bool metaKey = default, string? type = default, DataTransfer? dataTransfer = default)
			=> _ = DragEndAsync(element, new DragEventArgs { Detail = detail, ScreenX = screenX, ScreenY = screenY, ClientX = clientX, ClientY = clientY, Button = button, Buttons = buttons, CtrlKey = ctrlKey, ShiftKey = shiftKey, AltKey = altKey, MetaKey = metaKey, Type = type!, DataTransfer = dataTransfer! });

		/// <summary>
		/// Raises the <c>@ondragend</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
		public static void DragEnd(this IElement element, DragEventArgs eventArgs) => _ = DragEndAsync(element, eventArgs);

		/// <summary>
		/// Raises the <c>@ondragend</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
		/// <returns>A task that completes when the event handler is done.</returns>
		private static Task DragEndAsync(this IElement element, DragEventArgs eventArgs) => element.TriggerEventAsync("ondragend", eventArgs);

		/// <summary>
		/// Raises the <c>@ondragenter</c> event on <paramref name="element"/>, passing the provided
		/// properties to the event handler via a <see cref="DragEventArgs"/> object.
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
		/// <param name="dataTransfer">The data that underlies a drag-and-drop operation, known as the drag data store. See <see cref="DataTransfer"/>.</param>
		public static void DragEnter(this IElement element, long detail = default, double screenX = default, double screenY = default, double clientX = default, double clientY = default, long button = default, long buttons = default, bool ctrlKey = default, bool shiftKey = default, bool altKey = default, bool metaKey = default, string? type = default, DataTransfer? dataTransfer = default)
			=> _ = DragEnterAsync(element, new DragEventArgs { Detail = detail, ScreenX = screenX, ScreenY = screenY, ClientX = clientX, ClientY = clientY, Button = button, Buttons = buttons, CtrlKey = ctrlKey, ShiftKey = shiftKey, AltKey = altKey, MetaKey = metaKey, Type = type!, DataTransfer = dataTransfer! });

		/// <summary>
		/// Raises the <c>@ondragenter</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
		public static void DragEnter(this IElement element, DragEventArgs eventArgs) => _ = DragEnterAsync(element, eventArgs);

		/// <summary>
		/// Raises the <c>@ondragenter</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
		/// <returns>A task that completes when the event handler is done.</returns>
		private static Task DragEnterAsync(this IElement element, DragEventArgs eventArgs) => element.TriggerEventAsync("ondragenter", eventArgs);

		/// <summary>
		/// Raises the <c>@ondragleave</c> event on <paramref name="element"/>, passing the provided
		/// properties to the event handler via a <see cref="DragEventArgs"/> object.
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
		/// <param name="dataTransfer">The data that underlies a drag-and-drop operation, known as the drag data store. See <see cref="DataTransfer"/>.</param>
		public static void DragLeave(this IElement element, long detail = default, double screenX = default, double screenY = default, double clientX = default, double clientY = default, long button = default, long buttons = default, bool ctrlKey = default, bool shiftKey = default, bool altKey = default, bool metaKey = default, string? type = default, DataTransfer? dataTransfer = default)
			=> _ = DragLeaveAsync(element, new DragEventArgs { Detail = detail, ScreenX = screenX, ScreenY = screenY, ClientX = clientX, ClientY = clientY, Button = button, Buttons = buttons, CtrlKey = ctrlKey, ShiftKey = shiftKey, AltKey = altKey, MetaKey = metaKey, Type = type!, DataTransfer = dataTransfer! });

		/// <summary>
		/// Raises the <c>@ondragleave</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
		public static void DragLeave(this IElement element, DragEventArgs eventArgs) => _ = DragLeaveAsync(element, eventArgs);

		/// <summary>
		/// Raises the <c>@ondragleave</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
		/// <returns>A task that completes when the event handler is done.</returns>
		private static Task DragLeaveAsync(this IElement element, DragEventArgs eventArgs) => element.TriggerEventAsync("ondragleave", eventArgs);

		/// <summary>
		/// Raises the <c>@ondragover</c> event on <paramref name="element"/>, passing the provided
		/// properties to the event handler via a <see cref="DragEventArgs"/> object.
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
		/// <param name="dataTransfer">The data that underlies a drag-and-drop operation, known as the drag data store. See <see cref="DataTransfer"/>.</param>
		public static void DragOver(this IElement element, long detail = default, double screenX = default, double screenY = default, double clientX = default, double clientY = default, long button = default, long buttons = default, bool ctrlKey = default, bool shiftKey = default, bool altKey = default, bool metaKey = default, string? type = default, DataTransfer? dataTransfer = default)
			=> _ = DragOverAsync(element, new DragEventArgs { Detail = detail, ScreenX = screenX, ScreenY = screenY, ClientX = clientX, ClientY = clientY, Button = button, Buttons = buttons, CtrlKey = ctrlKey, ShiftKey = shiftKey, AltKey = altKey, MetaKey = metaKey, Type = type!, DataTransfer = dataTransfer! });

		/// <summary>
		/// Raises the <c>@ondragover</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
		public static void DragOver(this IElement element, DragEventArgs eventArgs) => _ = DragOverAsync(element, eventArgs);

		/// <summary>
		/// Raises the <c>@ondragover</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
		/// <returns>A task that completes when the event handler is done.</returns>
		private static Task DragOverAsync(this IElement element, DragEventArgs eventArgs) => element.TriggerEventAsync("ondragover", eventArgs);

		/// <summary>
		/// Raises the <c>@ondragstart</c> event on <paramref name="element"/>, passing the provided
		/// properties to the event handler via a <see cref="DragEventArgs"/> object.
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
		/// <param name="dataTransfer">The data that underlies a drag-and-drop operation, known as the drag data store. See <see cref="DataTransfer"/>.</param>
		public static void DragStart(this IElement element, long detail = default, double screenX = default, double screenY = default, double clientX = default, double clientY = default, long button = default, long buttons = default, bool ctrlKey = default, bool shiftKey = default, bool altKey = default, bool metaKey = default, string? type = default, DataTransfer? dataTransfer = default)
			=> _ = DragStartAsync(element, new DragEventArgs { Detail = detail, ScreenX = screenX, ScreenY = screenY, ClientX = clientX, ClientY = clientY, Button = button, Buttons = buttons, CtrlKey = ctrlKey, ShiftKey = shiftKey, AltKey = altKey, MetaKey = metaKey, Type = type!, DataTransfer = dataTransfer! });

		/// <summary>
		/// Raises the <c>@ondragstart</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
		public static void DragStart(this IElement element, DragEventArgs eventArgs) => _ = DragStartAsync(element, eventArgs);

		/// <summary>
		/// Raises the <c>@ondragstart</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
		/// <returns>A task that completes when the event handler is done.</returns>
		private static Task DragStartAsync(this IElement element, DragEventArgs eventArgs) => element.TriggerEventAsync("ondragstart", eventArgs);

		/// <summary>
		/// Raises the <c>@ondrop</c> event on <paramref name="element"/>, passing the provided
		/// properties to the event handler via a <see cref="DragEventArgs"/> object.
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
		/// <param name="dataTransfer">The data that underlies a drag-and-drop operation, known as the drag data store. See <see cref="DataTransfer"/>.</param>
		public static void Drop(this IElement element, long detail = default, double screenX = default, double screenY = default, double clientX = default, double clientY = default, long button = default, long buttons = default, bool ctrlKey = default, bool shiftKey = default, bool altKey = default, bool metaKey = default, string? type = default, DataTransfer? dataTransfer = default)
			=> _ = DropAsync(element, new DragEventArgs { Detail = detail, ScreenX = screenX, ScreenY = screenY, ClientX = clientX, ClientY = clientY, Button = button, Buttons = buttons, CtrlKey = ctrlKey, ShiftKey = shiftKey, AltKey = altKey, MetaKey = metaKey, Type = type!, DataTransfer = dataTransfer! });

		/// <summary>
		/// Raises the <c>@ondrop</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
		public static void Drop(this IElement element, DragEventArgs eventArgs) => _ = DropAsync(element, eventArgs);

		/// <summary>
		/// Raises the <c>@ondrop</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
		/// <returns>A task that completes when the event handler is done.</returns>
		private static Task DropAsync(this IElement element, DragEventArgs eventArgs) => element.TriggerEventAsync("ondrop", eventArgs);
	}
}
