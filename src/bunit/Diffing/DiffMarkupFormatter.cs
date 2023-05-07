using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html;

namespace Bunit.Diffing;

/// <summary>
/// A markup formatter, that skips any special Blazor attributes added by the <see cref="Htmlizer"/>.
/// </summary>
public class DiffMarkupFormatter : PrettyMarkupFormatter, IMarkupFormatter
{
	/// <summary>
	/// Gets an instance of the <see cref="DiffMarkupFormatter"/>.
	/// </summary>
	public static new readonly DiffMarkupFormatter Instance = new();

	/// <summary>
	/// Initializes a new instance of the <see cref="DiffMarkupFormatter"/> class.
	/// </summary>
	public DiffMarkupFormatter()
	{
		NewLine = Environment.NewLine;
		Indentation = "  ";
	}

	/// <summary>
	/// Creates the string representation of the attribute.
	/// </summary>
	/// <param name="attr">The attribute to serialize.</param>
	/// <returns>The string representation.</returns>
	public string ConvertToString(IAttr attr) => base.Attribute(attr);

	/// <inheritdoc/>
	protected override string Attribute(IAttr attr)
	{
		ArgumentNullException.ThrowIfNull(attr);

		return Htmlizer.IsBlazorAttribute(attr.Name ?? string.Empty)
			? string.Empty
			: base.Attribute(attr);
	}

	/// <inheritdoc/>
	public override string OpenTag(IElement element, bool selfClosing)
	{
		var result = base.OpenTag(element, selfClosing);

		if (TryGetOriginalName(element, out var originalName))
		{
			var tagStart = result.IndexOf('<', StringComparison.OrdinalIgnoreCase);
			var tagEnd = tagStart + originalName.Length + 1;
			return $"{result[..tagStart]}<{originalName}{result[tagEnd..]}";
		}

		return result;
	}

	/// <inheritdoc/>
	public override string CloseTag(IElement element, bool selfClosing)
	{
		var result = base.CloseTag(element, selfClosing);

		if (TryGetOriginalName(element, out var originalName))
		{
			var tagStart = result.IndexOf('<', StringComparison.OrdinalIgnoreCase);
			var tagEnd = tagStart + originalName.Length + 2;
			return $"{result[..tagStart]}</{originalName}{result[tagEnd..]}";
		}

		return result;
	}

	/// <summary>
	/// Gets the original element name, if that name is different from the
	/// value in <see cref="IElement.LocalName"/>.
	/// </summary>
	/// <param name="element">Element to get the original name for.</param>
	/// <param name="originalName">The original name, if found.</param>
	/// <returns>True if the original name is found; otherwise false.</returns>
	private static bool TryGetOriginalName(IElement? element, [NotNullWhen(true)] out string? originalName)
	{
		originalName = null;

		if (element is not null && element.SourceReference is not null && element.SourceReference.Position.Index != -1 && element.Owner is not null)
		{
			element.Owner.Source.Index = element.SourceReference.Position.Index + 1;
			originalName = element.Owner.Source.ReadCharacters(element.LocalName.Length);
			return !element.LocalName.Equals(originalName, StringComparison.Ordinal);
		}

		return false;
	}
}
