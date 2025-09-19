using System.Collections;
using AngleSharp.Dom;

namespace Bunit;

/// <summary>
/// Input event dispatch helper extension methods.
/// </summary>
public static partial class EventHandlerDispatchExtensions
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
	public static async Task ChangeAsync<T>(this Task<IElement> elementTask, T value)
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
	public static async Task InputAsync<T>(this Task<IElement> elementTask, T value)
	{
		var element = await elementTask;
		await InputAsync(element, CreateFrom(value));
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
