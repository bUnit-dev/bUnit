namespace Bunit;

/// <summary>
/// Represents a failure to find an element in the searched target
/// using the element's text content.
/// </summary>
public sealed class TextNotFoundException : Exception
{
	/// <summary>
	/// Gets the text used to search with.
	/// </summary>
	public string SearchText { get; }

	/// <summary>
	/// Initializes a new instance of the <see cref="TextNotFoundException"/> class.
	/// </summary>
	/// <param name="searchText">The text that was searched for.</param>
	public TextNotFoundException(string searchText)
		: base($"Unable to find an element with the text '{searchText}'.")
	{
		SearchText = searchText;
	}
}
