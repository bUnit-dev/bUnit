using System;
using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html;

namespace Bunit.Diffing
{
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

		/// <summary>
		/// Creates the string representation of the attribute.
		/// If it is a special Blazor renderer attribute, then it is ignored.
		/// </summary>
		/// <param name="attr">The attribute to serialize.</param>
		/// <returns>The string representation.</returns>
		protected override string Attribute(IAttr attr)
		{
			return Htmlizer.IsBlazorAttribute(attr?.Name ?? string.Empty)
				? string.Empty
				: base.Attribute(attr);
		}
	}
}
