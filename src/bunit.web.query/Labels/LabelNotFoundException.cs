namespace Bunit;

/// <summary>
/// Represents a failure to find an element in the searched target
/// using the Label's text.
/// </summary>
[Serializable]
public class LabelNotFoundException : Exception
{
	/// <summary>
	/// Gets the Label Text used to search with.
	/// </summary>
	public string LabelText { get; }

	/// <summary>
	/// Initializes a new instance of the <see cref="LabelNotFoundException"/> class.
	/// </summary>
	/// <param name="labelText"></param>
	public LabelNotFoundException(string labelText)
		: base($"Unable to find a label with the text of '{labelText}'.")
	{
		LabelText = labelText;
	}


	/// <summary>
	/// Initializes a new instance of the <see cref="LabelNotFoundException"/> class.
	/// </summary>
	protected LabelNotFoundException(SerializationInfo info, StreamingContext context)
		: base(info, context) { }
}
