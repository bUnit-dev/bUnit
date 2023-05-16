namespace Bunit;

/// <summary>
/// Represents a failure to find an element in the searched target
/// using a CSS selector.
/// </summary>
public class ElementNotFoundException : Exception
{
	/// <summary>
	/// Gets the CSS selector used to search with.
	/// </summary>
	public string CssSelector { get; }

	/// <summary>
	/// Initializes a new instance of the <see cref="ElementNotFoundException"/> class.
	/// </summary>
	public ElementNotFoundException(string cssSelector)
		: base($"No elements were found that matches the selector '{cssSelector}'")
	{
		CssSelector = cssSelector;
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="ElementNotFoundException"/> class.
	/// </summary>
	protected ElementNotFoundException(string message, string cssSelector)
		: base(message)
	{
		CssSelector = cssSelector;
	}
}
