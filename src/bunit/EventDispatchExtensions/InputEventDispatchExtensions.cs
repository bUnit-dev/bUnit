using System.Collections;
using AngleSharp.Dom;

namespace Bunit;

/// <summary>
/// Input event dispatch helper extension methods.
/// </summary>
public static partial class InputEventDispatchExtensions
{
	/// <summary>
	/// Raises the <c>@onchange</c> event on <paramref name="element"/>,  passing the provided
	/// properties to the event handler via a <see cref="ChangeEventArgs"/> object.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <param name="value">The new value.</param>
	public static void Change<T>(this IElement element, T value)
		=> _ = ChangeAsync(element, CreateFrom(value));

	/// <summary>
	/// Raises the <c>@onchange</c> event on the element returned by <paramref name="elementTask"/>,  passing the provided
	/// properties to the event handler via a <see cref="ChangeEventArgs"/> object.
	/// </summary>
	/// <param name="elementTask">A task that returns the element to raise the event on.</param>
	/// <param name="value">The new value.</param>
	public static async Task Change<T>(this Task<IElement> elementTask, T value)
	{
		var element = await elementTask;
		await ChangeAsync(element, CreateFrom(value));
	}

	/// <summary>
	/// Raises the <c>@oninput</c> event on <paramref name="element"/>,  passing the provided
	/// properties to the event handler via a <see cref="ChangeEventArgs"/> object.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <param name="value">The new value.</param>
	public static void Input<T>(this IElement element, T value)
		=> _ = InputAsync(element, CreateFrom(value));

	/// <summary>
	/// Raises the <c>@oninput</c> event on the element returned by <paramref name="elementTask"/>,  passing the provided
	/// properties to the event handler via a <see cref="ChangeEventArgs"/> object.
	/// </summary>
	/// <param name="elementTask">A task that returns the element to raise the event on.</param>
	/// <param name="value">The new value.</param>
	public static async Task Input<T>(this Task<IElement> elementTask, T value)
	{
		var element = await elementTask;
		await InputAsync(element, CreateFrom(value));
	}

	/// <summary>
	/// Raises the <c>@oninvalid</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>)
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	public static void Invalid(this IElement element) => _ = InvalidAsync(element);

	/// <summary>
	/// Raises the <c>@oninvalid</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>)
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <returns>A task that completes when the event handler is done.</returns>
	public static Task InvalidAsync(this IElement element) => element.TriggerEventAsync("oninvalid", EventArgs.Empty);

	/// <summary>
	/// Raises the <c>@oninvalid</c> event on the element returned by <paramref name="elementTask"/>, passing an empty (<see cref="EventArgs.Empty"/>)
	/// to the event handler.
	/// </summary>
	/// <param name="elementTask">A task that returns the element to raise the event on.</param>
	/// <returns>A task that completes when the event handler is done.</returns>
	public static async Task InvalidAsync(this Task<IElement> elementTask)
	{
		var element = await elementTask;
		await InvalidAsync(element);
	}


	/// <summary>
	/// Raises the <c>@onreset</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>)
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	public static void Reset(this IElement element) => _ = ResetAsync(element);

	/// <summary>
	/// Raises the <c>@onreset</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>)
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <returns>A task that completes when the event handler is done.</returns>
	public static Task ResetAsync(this IElement element) => element.TriggerEventAsync("onreset", EventArgs.Empty);

	/// <summary>
	/// Raises the <c>@onreset</c> event on the element returned by <paramref name="elementTask"/>, passing an empty (<see cref="EventArgs.Empty"/>)
	/// to the event handler.
	/// </summary>
	/// <param name="elementTask">A task that returns the element to raise the event on.</param>
	/// <returns>A task that completes when the event handler is done.</returns>
	public static async Task ResetAsync(this Task<IElement> elementTask)
	{
		var element = await elementTask;
		await ResetAsync(element);
	}

	/// <summary>
	/// Raises the <c>@onselect</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>)
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	public static void Select(this IElement element) => _ = SelectAsync(element);

	/// <summary>
	/// Raises the <c>@onselect</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>)
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <returns>A task that completes when the event handler is done.</returns>
	public static Task SelectAsync(this IElement element) => element.TriggerEventAsync("onselect", EventArgs.Empty);

	/// <summary>
	/// Raises the <c>@onselect</c> event on the element returned by <paramref name="elementTask"/>, passing an empty (<see cref="EventArgs.Empty"/>)
	/// to the event handler.
	/// </summary>
	/// <param name="elementTask">A task that returns the element to raise the event on.</param>
	/// <returns>A task that completes when the event handler is done.</returns>
	public static async Task SelectAsync(this Task<IElement> elementTask)
	{
		var element = await elementTask;
		await SelectAsync(element);
	}

	/// <summary>
	/// Raises the <c>@onselectstart</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>)
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	public static void SelectStart(this IElement element) => _ = SelectStartAsync(element);

	/// <summary>
	/// Raises the <c>@onselectstart</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>)
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <returns>A task that completes when the event handler is done.</returns>
	public static Task SelectStartAsync(this IElement element) => element.TriggerEventAsync("onselectstart", EventArgs.Empty);

	/// <summary>
	/// Raises the <c>@onselectstart</c> event on <paramref name="elementTask"/>, passing an empty (<see cref="EventArgs.Empty"/>)
	/// to the event handler.
	/// </summary>
	/// <param name="elementTask">The element to raise the event on.</param>
	/// <returns>A task that completes when the event handler is done.</returns>
	public static async Task SelectStartAsync(this Task<IElement> elementTask)
	{
		var element = await elementTask;
		await SelectStartAsync(element);
	}

	/// <summary>
	/// Raises the <c>@onselectionchange</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>)
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	public static void SelectionChange(this IElement element) => _ = SelectionChangeAsync(element);

	/// <summary>
	/// Raises the <c>@onselectionchange</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>)
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <returns>A task that completes when the event handler is done.</returns>
	public static Task SelectionChangeAsync(this IElement element) => element.TriggerEventAsync("onselectionchange", EventArgs.Empty);

	/// <summary>
	/// Raises the <c>@onselectionchange</c> event on <paramref name="elementTask"/>, passing an empty (<see cref="EventArgs.Empty"/>)
	/// to the event handler.
	/// </summary>
	/// <param name="elementTask">The element to raise the event on.</param>
	/// <returns>A task that completes when the event handler is done.</returns>
	public static async Task SelectionChangeAsync(this Task<IElement> elementTask)
	{
		var element = await elementTask;
		await SelectionChangeAsync(element);
	}

	/// <summary>
	/// Raises the <c>@onsubmit</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>)
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	public static void Submit(this IElement element) => _ = SubmitAsync(element);

	/// <summary>
	/// Raises the <c>@onsubmit</c> event on <paramref name="element"/>, passing an empty (<see cref="EventArgs.Empty"/>)
	/// to the event handler.
	/// </summary>
	/// <param name="element">The element to raise the event on.</param>
	/// <returns>A task that completes when the event handler is done.</returns>
	public static Task SubmitAsync(this IElement element) => element.TriggerEventAsync("onsubmit", EventArgs.Empty);

	/// <summary>
	/// Raises the <c>@onsubmit</c> event on <paramref name="elementTask"/>, passing an empty (<see cref="EventArgs.Empty"/>)
	/// to the event handler.
	/// </summary>
	/// <param name="elementTask">The element to raise the event on.</param>
	/// <returns>A task that completes when the event handler is done.</returns>
	public static async Task SubmitAsync(this Task<IElement> elementTask)
	{
		var element = await elementTask;
		await SubmitAsync(element);
	}

	private static ChangeEventArgs CreateFrom<T>(T value) => new() { Value = FormatValue(value) };

	private static object? FormatValue<T>(T value)
		=> value switch
		{
			null => null,
			bool _ => value,
			string _ => value,
			ICollection values => FormatValues(values),
			IEnumerable values => FormatValues(values),
			_ => BindConverter.FormatValue(value)
		};

	private static object?[] FormatValues(ICollection values)
	{
		var result = new object?[values.Count];

		var index = 0;
		foreach (var value in values)
			result[index++] = FormatValue(value);

		return result;
	}

	private static object?[] FormatValues(IEnumerable values)
	{
		var result = new List<object?>();

		foreach (var value in values)
			result.Add(FormatValue(value));

		return result.ToArray();
	}
}
