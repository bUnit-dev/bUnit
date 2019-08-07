using System;
using System.Linq;
using System.Text;
using System.Xml;
using Org.XmlUnit.Diff;
using Xunit.Sdk;

namespace Egil.RazorComponents.Testing
{
    public class RazorComponentDoesNotMatchException : AssertActualExpectedException
    {
        public RazorComponentDoesNotMatchException(XmlNode expectedHtml, XmlNode renderedHtml, Diff diffResult)
            : base(PrettyXml(expectedHtml),
                  PrettyXml(renderedHtml),
                  CreateDiffMessage(diffResult),
                  "Expected HTML",
                  "Rendered HTML")
        {
        }

        private static string CreateDiffMessage(Diff diffResult)
        {
            var diffs = diffResult.Differences.ToArray();
            var result = new StringBuilder();
            result.AppendLine();
            result.AppendLine($"Found {diffs.Length} differences between expected HTML and rendered HTML:");

            for (int i = 0; i < diffs.Length; i++)
            {
                result.AppendLine($"{i + 1}) {diffs[i].ToString()}");
            }

            return result.ToString();
        }

        private static string PrettyXml(XmlNode? xml)
        {
            if (xml is null) return string.Empty;

            var result = new StringBuilder();
            var settings = new XmlWriterSettings
            {
                OmitXmlDeclaration = true,
                Indent = true,
                NewLineOnAttributes = false,
                ConformanceLevel = ConformanceLevel.Fragment,
                IndentChars = "  ",
            };

            using (var xmlWriter = XmlWriter.Create(result, settings))
            {
                xml.WriteTo(xmlWriter);
            }

            result.Insert(0, Environment.NewLine);
            result.AppendLine();

            return result.ToString();
        }
    }
}
