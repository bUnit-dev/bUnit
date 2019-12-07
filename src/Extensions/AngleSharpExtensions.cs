using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html;
using AngleSharp.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Egil.RazorComponents.Testing.Extensions
{
    public static class AngleSharpExtensions
    {
        public static string ToDiffMarkup(this IMarkupFormattable markup)
        {
            if (markup is null) throw new ArgumentNullException(nameof(markup));

            using var sw = new StringWriter();
            markup.ToHtml(sw, new DiffMarkupFormatter());
            return sw.ToString();
        }

        public static string ToMarkup(this IMarkupFormattable markup)
        {
            if (markup is null) throw new ArgumentNullException(nameof(markup));

            using var sw = new StringWriter();
            var formatter = new PrettyMarkupFormatter()
            {
                NewLine = Environment.NewLine,
                Indentation = "  "
            };
            markup.ToHtml(sw, formatter);
            return sw.ToString();
        }

        public static string ToMarkupElementOnly(this IElement element)
        {
            if (element is null) throw new ArgumentNullException(nameof(element));

            var result = new StringBuilder();
            result.Append(Symbols.LessThan);

            var prefix = element.Prefix;
            var name = element.LocalName;
            var tag = !string.IsNullOrEmpty(prefix) ? string.Concat(prefix, ":", name) : name;

            result.Append(tag);

            foreach (var attribute in element.Attributes)
            {
                result.Append(' ').Append(HtmlMarkupFormatter.Instance.Attribute(attribute));
            }

            if (element.HasChildNodes)
            {
                result.Append(Symbols.GreaterThan);
                result.Append("...");
                result.Append($"</{tag}>");
            }
            else
            {
                result.Append(" />");
            }

            return result.ToString();
        }
    }
}
