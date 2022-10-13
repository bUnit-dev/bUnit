#if !NET7_0_OR_GREATER
namespace System.Diagnostics.CodeAnalysis;

/// <summary>Fake version of the StringSyntaxAttribute, which was introduced in .NET 7</summary>
[AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
public sealed class StringSyntaxAttribute : Attribute
{
	/// <summary>
	/// Initializes the <see cref="StringSyntaxAttribute"/> with the identifier of the syntax used.
	/// </summary>
	public StringSyntaxAttribute(string syntax)
	{
	}

	/// <summary>
	/// Initializes the <see cref="StringSyntaxAttribute"/> with the identifier of the syntax used.
	/// </summary>
	public StringSyntaxAttribute(string syntax, params object?[] arguments)
	{
	}

	/// <summary>The syntax identifier for strings containing URIs.</summary>
	public const string Uri = nameof(Uri);
}
#endif
