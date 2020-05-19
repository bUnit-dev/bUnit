using System;

using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html;

using Bunit.Rendering;

namespace Bunit.Diffing
{
	/// <summary>
	/// A markup formatter, that skips any special Blazor attributes added by the <see cref="TestRenderer"/>/<see cref="Htmlizer"/>.
	/// </summary>
	public class DiffMarkupFormatter : PrettyMarkupFormatter, IMarkupFormatter
	{
		/// <summary>
		/// Gets an instance of the <see cref="DiffMarkupFormatter"/>.
		/// </summary>
		public new static readonly DiffMarkupFormatter Instance = new DiffMarkupFormatter();

		/// <summary>
		/// Creates an instance of the <see cref="DiffMarkupFormatter"/>.
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
