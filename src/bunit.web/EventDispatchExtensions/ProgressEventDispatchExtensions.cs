using System.Threading.Tasks;
using AngleSharp.Dom;
using Microsoft.AspNetCore.Components.Web;

namespace Bunit
{
	/// <summary>
	/// Pointer event dispatch helper extension methods.
	/// </summary>
	public static class ProgressEventDispatchExtensions
	{
		/// <summary>
		/// Raises the <c>@onloadstart</c> event on <paramref name="element"/>, passing the provided
		/// properties to the event handler via a <see cref="ProgressEventArgs"/> object.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		/// <param name="lengthComputable">Whether or not the total size of the transfer is known.</param>
		/// <param name="loaded">The number of bytes transferred since the beginning of the operation. This doesn't include headers and other overhead, but only the content itself.</param>
		/// <param name="total">The total number of bytes of content that will be transferred during the operation. If the total size is unknown, this value is zero.</param>
		/// <param name="type">Gets or sets the type of the event.</param>
		public static void LoadStart(this IElement element, bool lengthComputable = default, long loaded = default, long total = default, string? type = default)
			=> _ = LoadStartAsync(element, new ProgressEventArgs { LengthComputable = lengthComputable, Loaded = loaded, Total = total, Type = type! });

		/// <summary>
		/// Raises the <c>@onloadstart</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
		public static void LoadStart(this IElement element, ProgressEventArgs eventArgs) => _ = LoadStartAsync(element, eventArgs);

		/// <summary>
		/// Raises the <c>@onloadstart</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
		/// <returns>A task that completes when the event handler is done.</returns>
		private static Task LoadStartAsync(this IElement element, ProgressEventArgs eventArgs) => element.TriggerEventAsync("onloadstart", eventArgs);

		/// <summary>
		/// Raises the <c>@ontimeout</c> event on <paramref name="element"/>, passing the provided
		/// properties to the event handler via a <see cref="ProgressEventArgs"/> object.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		/// <param name="lengthComputable">Whether or not the total size of the transfer is known.</param>
		/// <param name="loaded">The number of bytes transferred since the beginning of the operation. This doesn't include headers and other overhead, but only the content itself.</param>
		/// <param name="total">The total number of bytes of content that will be transferred during the operation. If the total size is unknown, this value is zero.</param>
		/// <param name="type">Gets or sets the type of the event.</param>
		public static void Timeout(this IElement element, bool lengthComputable = default, long loaded = default, long total = default, string? type = default)
			=> _ = TimeoutAsync(element, new ProgressEventArgs { LengthComputable = lengthComputable, Loaded = loaded, Total = total, Type = type! });

		/// <summary>
		/// Raises the <c>@ontimeout</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
		public static void Timeout(this IElement element, ProgressEventArgs eventArgs) => _ = TimeoutAsync(element, eventArgs);

		/// <summary>
		/// Raises the <c>@ontimeout</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
		/// <returns>A task that completes when the event handler is done.</returns>
		private static Task TimeoutAsync(this IElement element, ProgressEventArgs eventArgs) => element.TriggerEventAsync("ontimeout", eventArgs);

		/// <summary>
		/// Raises the <c>@onabort</c> event on <paramref name="element"/>, passing the provided
		/// properties to the event handler via a <see cref="ProgressEventArgs"/> object.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		/// <param name="lengthComputable">Whether or not the total size of the transfer is known.</param>
		/// <param name="loaded">The number of bytes transferred since the beginning of the operation. This doesn't include headers and other overhead, but only the content itself.</param>
		/// <param name="total">The total number of bytes of content that will be transferred during the operation. If the total size is unknown, this value is zero.</param>
		/// <param name="type">Gets or sets the type of the event.</param>
		public static void Abort(this IElement element, bool lengthComputable = default, long loaded = default, long total = default, string? type = default)
			=> _ = AbortAsync(element, new ProgressEventArgs { LengthComputable = lengthComputable, Loaded = loaded, Total = total, Type = type! });

		/// <summary>
		/// Raises the <c>@onabort</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
		public static void Abort(this IElement element, ProgressEventArgs eventArgs) => _ = AbortAsync(element, eventArgs);

		/// <summary>
		/// Raises the <c>@onabort</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
		/// <returns>A task that completes when the event handler is done.</returns>
		private static Task AbortAsync(this IElement element, ProgressEventArgs eventArgs) => element.TriggerEventAsync("onabort", eventArgs);

		/// <summary>
		/// Raises the <c>@onload</c> event on <paramref name="element"/>, passing the provided
		/// properties to the event handler via a <see cref="ProgressEventArgs"/> object.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		/// <param name="lengthComputable">Whether or not the total size of the transfer is known.</param>
		/// <param name="loaded">The number of bytes transferred since the beginning of the operation. This doesn't include headers and other overhead, but only the content itself.</param>
		/// <param name="total">The total number of bytes of content that will be transferred during the operation. If the total size is unknown, this value is zero.</param>
		/// <param name="type">Gets or sets the type of the event.</param>
		public static void Load(this IElement element, bool lengthComputable = default, long loaded = default, long total = default, string? type = default)
			=> _ = LoadAsync(element, new ProgressEventArgs { LengthComputable = lengthComputable, Loaded = loaded, Total = total, Type = type! });

		/// <summary>
		/// Raises the <c>@onload</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
		public static void Load(this IElement element, ProgressEventArgs eventArgs) => _ = LoadAsync(element, eventArgs);

		/// <summary>
		/// Raises the <c>@onload</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
		/// <returns>A task that completes when the event handler is done.</returns>
		private static Task LoadAsync(this IElement element, ProgressEventArgs eventArgs) => element.TriggerEventAsync("onload", eventArgs);

		/// <summary>
		/// Raises the <c>@onloadend</c> event on <paramref name="element"/>, passing the provided
		/// properties to the event handler via a <see cref="ProgressEventArgs"/> object.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		/// <param name="lengthComputable">Whether or not the total size of the transfer is known.</param>
		/// <param name="loaded">The number of bytes transferred since the beginning of the operation. This doesn't include headers and other overhead, but only the content itself.</param>
		/// <param name="total">The total number of bytes of content that will be transferred during the operation. If the total size is unknown, this value is zero.</param>
		/// <param name="type">Gets or sets the type of the event.</param>
		public static void LoadEnd(this IElement element, bool lengthComputable = default, long loaded = default, long total = default, string? type = default)
			=> _ = LoadEndAsync(element, new ProgressEventArgs { LengthComputable = lengthComputable, Loaded = loaded, Total = total, Type = type! });

		/// <summary>
		/// Raises the <c>@onloadend</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
		public static void LoadEnd(this IElement element, ProgressEventArgs eventArgs) => _ = LoadEndAsync(element, eventArgs);

		/// <summary>
		/// Raises the <c>@onloadend</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
		/// <returns>A task that completes when the event handler is done.</returns>
		private static Task LoadEndAsync(this IElement element, ProgressEventArgs eventArgs) => element.TriggerEventAsync("onloadend", eventArgs);

		/// <summary>
		/// Raises the <c>@onprogress</c> event on <paramref name="element"/>, passing the provided
		/// properties to the event handler via a <see cref="ProgressEventArgs"/> object.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		/// <param name="lengthComputable">Whether or not the total size of the transfer is known.</param>
		/// <param name="loaded">The number of bytes transferred since the beginning of the operation. This doesn't include headers and other overhead, but only the content itself.</param>
		/// <param name="total">The total number of bytes of content that will be transferred during the operation. If the total size is unknown, this value is zero.</param>
		/// <param name="type">Gets or sets the type of the event.</param>
		public static void Progress(this IElement element, bool lengthComputable = default, long loaded = default, long total = default, string? type = default)
			=> _ = ProgressAsync(element, new ProgressEventArgs { LengthComputable = lengthComputable, Loaded = loaded, Total = total, Type = type! });

		/// <summary>
		/// Raises the <c>@onprogress</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
		public static void Progress(this IElement element, ProgressEventArgs eventArgs) => _ = ProgressAsync(element, eventArgs);

		/// <summary>
		/// Raises the <c>@onprogress</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
		/// <returns>A task that completes when the event handler is done.</returns>
		private static Task ProgressAsync(this IElement element, ProgressEventArgs eventArgs) => element.TriggerEventAsync("onprogress", eventArgs);

		/// <summary>
		/// Raises the <c>@onerror</c> event on <paramref name="element"/>, passing the provided
		/// properties to the event handler via a <see cref="ProgressEventArgs"/> object.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		/// <param name="lengthComputable">Whether or not the total size of the transfer is known.</param>
		/// <param name="loaded">The number of bytes transferred since the beginning of the operation. This doesn't include headers and other overhead, but only the content itself.</param>
		/// <param name="total">The total number of bytes of content that will be transferred during the operation. If the total size is unknown, this value is zero.</param>
		/// <param name="type">Gets or sets the type of the event.</param>
		public static void Error(this IElement element, bool lengthComputable = default, long loaded = default, long total = default, string? type = default)
			=> _ = ErrorAsync(element, new ProgressEventArgs { LengthComputable = lengthComputable, Loaded = loaded, Total = total, Type = type! });

		/// <summary>
		/// Raises the <c>@onerror</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
		public static void Error(this IElement element, ProgressEventArgs eventArgs) => _ = ErrorAsync(element, eventArgs);

		/// <summary>
		/// Raises the <c>@onerror</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
		/// to the event handler.
		/// </summary>
		/// <param name="element">The element to raise the event on.</param>
		/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
		/// <returns>A task that completes when the event handler is done.</returns>
		private static Task ErrorAsync(this IElement element, ProgressEventArgs eventArgs) => element.TriggerEventAsync("onerror", eventArgs);
	}
}
