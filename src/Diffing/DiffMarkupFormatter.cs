using System;
using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html;

namespace Egil.RazorComponents.Testing.Diffing
{
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

        /// <inheritdoc />
        public string OpenTag(IElement element, bool selfClosing)
        {
            if(element is null) throw new ArgumentNullException(nameof(element));

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
