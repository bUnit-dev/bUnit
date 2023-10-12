using AngleSharp.Dom;

#if NET8_0_OR_GREATER
namespace Bunit;

/// <summary>
/// Dialog helper events.
/// </summary>
public static class DialogEventDispatchExtensions
{
	/// <summary>
	/// Raises the <c>@oncancel</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>).
	/// </summary>
	public static void Cancel(this IElement element) => _ = CancelAsync(element);

	/// <summary>
	/// Raises the <c>@oncancel</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>).
	/// </summary>
	public static Task CancelAsync(this IElement element) => element.TriggerEventAsync("oncancel", EventArgs.Empty);

	/// <summary>
	///	Raises the <c>@onclose</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>.
	///	</summary>
	public static void Close(this IElement element) => _ = CloseAsync(element);

	/// <summary>
	///	Raises the <c>@onclose</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>.
	///	</summary>
	public static Task CloseAsync(this IElement element) => element.TriggerEventAsync("onclose", EventArgs.Empty);
}
#endif
