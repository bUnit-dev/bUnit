using System;
using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html;
using Bunit.Rendering;

namespace Bunit.Diffing
{
 //   /// <summary>
 //   /// A markup formatter, that skips any special Blazor attributes added by the <see cref="TestRenderer"/>/<see cref="Htmlizer"/>.
 //   /// </summary>
	//public class DiffMarkupFormatter : PrettyMarkupFormatter, IMarkupFormatter
	//{
	//	/// <summary>
	//	/// Gets an instance of the <see cref="DiffMarkupFormatter"/>.
	//	/// </summary>
	//	public new static readonly DiffMarkupFormatter Instance = new DiffMarkupFormatter();

	//	/// <summary>
	//	/// Creates an instance of the <see cref="DiffMarkupFormatter"/>.
	//	/// </summary>
	//	public DiffMarkupFormatter()
	//	{
	//		NewLine = Environment.NewLine;
	//		Indentation = "  ";
	//	}

	//	/// <summary>
 //       /// Creates the string representation of the attribute.
	//	/// </summary>
	//	/// <param name="attr">The attribute to serialize.</param>
	//	/// <returns>The string representation.</returns>
	//	public string ConvertToString(IAttr attr) => base.Attribute(attr);

	//	/// <summary>
 //       /// Creates the string representation of the attribute.
	//	/// If it is a special Blazor renderer attribute, then it is ignored.
	//	/// </summary>
	//	/// <param name="attr">The attribute to serialize.</param>
	//	/// <returns>The string representation.</returns>
	//	protected override string Attribute(IAttr attr)
	//	{
	//		return Htmlizer.IsBlazorAttribute(attr?.Name ?? string.Empty)
	//			? string.Empty
	//			: base.Attribute(attr);
	//	}
	//}

	/// <summary>
	/// A markup formatter, that skips any special Blazor attributes added by the <see cref="TestRenderer"/>/<see cref="Htmlizer"/>.
	/// </summary>
	public class DiffMarkupFormatter : IMarkupFormatter
	{
		private readonly IMarkupFormatter _formatter = new PrettyMarkupFormatter()
		{
			NewLine = Environment.NewLine,
			Indentation = "  "
		};

		public new static readonly DiffMarkupFormatter Instance = new DiffMarkupFormatter();

		/// <summary>
        /// Creates the string representation of the attribute.
		/// </summary>
		/// <param name="attr">The attribute to serialize.</param>
		/// <returns>The string representation.</returns>
		public string ConvertToString(IAttr attr) => _formatter.Attribute(attr);

		/// <inheritdoc />
		public string Attribute(IAttr attribute)
			=> Htmlizer.IsBlazorAttribute(attribute?.Name ?? string.Empty)
				? string.Empty
				: _formatter.Attribute(attribute);

		/// <inheritdoc />
		public string CloseTag(IElement element, bool selfClosing) => _formatter.CloseTag(element, selfClosing);

		/// <inheritdoc />
		public string Comment(IComment comment) => _formatter.Comment(comment);

		/// <inheritdoc />
		public string Doctype(IDocumentType doctype) => _formatter.Doctype(doctype);

		public string LiteralText(ICharacterData text) => throw new NotImplementedException();

		/// <inheritdoc />
		public string OpenTag(IElement element, bool selfClosing)
		{
			if (element is null)
				throw new ArgumentNullException(nameof(element));

			var result = _formatter.OpenTag(element, selfClosing);

			foreach (var attr in element.Attributes)
			{
				if (Htmlizer.IsBlazorAttribute(attr.Name))
				{
					var attrToRemove = " " + HtmlMarkupFormatter.Instance.Attribute(attr);
					result = result.Replace(attrToRemove, "", StringComparison.Ordinal);
				}
			}

			return result;
		}

		/// <inheritdoc />
		public string Processing(IProcessingInstruction processing) => _formatter.Processing(processing);

		/// <inheritdoc />
		public string Text(ICharacterData text) => _formatter.Text(text);
	}
}
