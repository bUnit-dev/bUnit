#if NET5_0_OR_GREATER
using System;
using System.Threading.Tasks;
using AngleSharp.Dom;

namespace Bunit;

/// <summary>
/// General event dispatch helper extension methods.
/// </summary>
public static class DetailsElementEventDispatchExtensions
{
	/// <summary>
	/// Raises the <c>@ontoggle</c> event on <paramref name="element"/>
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	public static void Toggle(this IElement element)
		=> _ = ToggleAsync(element, EventArgs.Empty);

	/// <summary>
	/// Raises the <c>@ontoggle</c> event on <paramref name="element"/>, passing the provided <paramref name="eventArgs"/>
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <param name="eventArgs">The event arguments to pass to the event handler.</param>
	/// <returns>A task that completes when the event handler is done.</returns>
	public static Task ToggleAsync(this IElement element, EventArgs eventArgs) => element.TriggerEventAsync("ontoggle", eventArgs);
}
#endif
