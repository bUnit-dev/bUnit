namespace Bunit.RazorTesting;

/// <summary>
/// Represents an missing or invalid Blazor parameter on a Blazor component.
/// </summary>
public sealed class ParameterException : ArgumentException
{
	/// <summary>
	/// Initializes a new instance of the <see cref="ParameterException"/> class.
	/// </summary>
	/// <param name="message">Validation message.</param>
	/// <param name="parameterName">Name of the Blazor parameter.</param>
	public ParameterException(string message, string parameterName)
		: base(message, parameterName)
	{ }
}
