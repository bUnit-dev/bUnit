using System;
using System.Threading.Tasks;
using AngleSharp.Dom;

namespace Bunit
{
	/// <summary>
	/// General event dispatch helper extension methods.
	/// </summary>
	public static class GeneralEventDispatchExtensions
	{
		/// <summary>
		/// Raises the <c>@onactivate</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>)
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		public static void Activate(this IElement element) => ActivateAsync(element);

		/// <summary>
		/// Raises the <c>@onactivate</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>)
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		/// <returns>A task that completes when the event handler is done.</returns>
		private static Task ActivateAsync(this IElement element) => element.TriggerEventAsync("onactivate", EventArgs.Empty);

		/// <summary>
		/// Raises the <c>@onbeforeactivate</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>)
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		public static void BeforeActivate(this IElement element) => BeforeActivateAsync(element);

		/// <summary>
		/// Raises the <c>@onbeforeactivate</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>)
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		/// <returns>A task that completes when the event handler is done.</returns>
		private static Task BeforeActivateAsync(this IElement element) => element.TriggerEventAsync("onbeforeactivate", EventArgs.Empty);

		/// <summary>
		/// Raises the <c>@onbeforedeactivate</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>)
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		public static void BeforeDeactivate(this IElement element) => BeforeDeactivateAsync(element);

		/// <summary>
		/// Raises the <c>@onbeforedeactivate</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>)
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		/// <returns>A task that completes when the event handler is done.</returns>
		private static Task BeforeDeactivateAsync(this IElement element) => element.TriggerEventAsync("onbeforedeactivate", EventArgs.Empty);

		/// <summary>
		/// Raises the <c>@ondeactivate</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>)
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		public static void Deactivate(this IElement element) => DeactivateAsync(element);

		/// <summary>
		/// Raises the <c>@ondeactivate</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>)
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		/// <returns>A task that completes when the event handler is done.</returns>
		private static Task DeactivateAsync(this IElement element) => element.TriggerEventAsync("ondeactivate", EventArgs.Empty);

		/// <summary>
		/// Raises the <c>@onended</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>)
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		public static void Ended(this IElement element) => EndedAsync(element);

		/// <summary>
		/// Raises the <c>@onended</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>)
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		/// <returns>A task that completes when the event handler is done.</returns>
		private static Task EndedAsync(this IElement element) => element.TriggerEventAsync("onended", EventArgs.Empty);

		/// <summary>
		/// Raises the <c>@onfullscreenchange</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>)
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		public static void FullscreenChange(this IElement element) => FullscreenChangeAsync(element);

		/// <summary>
		/// Raises the <c>@onfullscreenchange</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>)
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		/// <returns>A task that completes when the event handler is done.</returns>
		private static Task FullscreenChangeAsync(this IElement element) => element.TriggerEventAsync("onfullscreenchange", EventArgs.Empty);

		/// <summary>
		/// Raises the <c>@onfullscreenerror</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>)
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		public static void FullscreenError(this IElement element) => FullscreenErrorAsync(element);

		/// <summary>
		/// Raises the <c>@onfullscreenerror</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>)
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		/// <returns>A task that completes when the event handler is done.</returns>
		private static Task FullscreenErrorAsync(this IElement element) => element.TriggerEventAsync("onfullscreenerror", EventArgs.Empty);

		/// <summary>
		/// Raises the <c>@onloadeddata</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>)
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		public static void LoadedData(this IElement element) => LoadedDataAsync(element);

		/// <summary>
		/// Raises the <c>@onloadeddata</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>)
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		/// <returns>A task that completes when the event handler is done.</returns>
		private static Task LoadedDataAsync(this IElement element) => element.TriggerEventAsync("onloadeddata", EventArgs.Empty);

		/// <summary>
		/// Raises the <c>@onloadedmetadata</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>)
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		public static void LoadedMetadata(this IElement element) => LoadedMetadataAsync(element);

		/// <summary>
		/// Raises the <c>@onloadedmetadata</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>)
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		/// <returns>A task that completes when the event handler is done.</returns>
		private static Task LoadedMetadataAsync(this IElement element) => element.TriggerEventAsync("onloadedmetadata", EventArgs.Empty);

		/// <summary>
		/// Raises the <c>@onpointerlockchange</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>)
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		public static void PointerlockChange(this IElement element) => PointerlockChangeAsync(element);

		/// <summary>
		/// Raises the <c>@onpointerlockchange</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>)
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		/// <returns>A task that completes when the event handler is done.</returns>
		private static Task PointerlockChangeAsync(this IElement element) => element.TriggerEventAsync("onpointerlockchange", EventArgs.Empty);

		/// <summary>
		/// Raises the <c>@onpointerlockerror</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>)
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		public static void PointerlockError(this IElement element) => PointerlockErrorAsync(element);

		/// <summary>
		/// Raises the <c>@onpointerlockerror</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>)
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		/// <returns>A task that completes when the event handler is done.</returns>
		private static Task PointerlockErrorAsync(this IElement element) => element.TriggerEventAsync("onpointerlockerror", EventArgs.Empty);

		/// <summary>
		/// Raises the <c>@onreadystatechange</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>)
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		public static void ReadystateChange(this IElement element) => ReadystateChangeAsync(element);

		/// <summary>
		/// Raises the <c>@onreadystatechange</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>)
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		/// <returns>A task that completes when the event handler is done.</returns>
		private static Task ReadystateChangeAsync(this IElement element) => element.TriggerEventAsync("onreadystatechange", EventArgs.Empty);

		/// <summary>
		/// Raises the <c>@onscroll</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>)
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		public static void Scroll(this IElement element) => ScrollAsync(element);

		/// <summary>
		/// Raises the <c>@onscroll</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>)
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		/// <returns>A task that completes when the event handler is done.</returns>
		private static Task ScrollAsync(this IElement element) => element.TriggerEventAsync("onscroll", EventArgs.Empty);
	}
}
