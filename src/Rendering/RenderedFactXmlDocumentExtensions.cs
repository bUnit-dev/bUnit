using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Egil.RazorComponents.Testing.Rendering
{
    public static class RenderedFactXmlDocumentExtensions
    {
        public const string RegexXmlNamespace = "urn:egil.razorcomponents.testing.library.regex";

        static RenderedFactXmlDocumentExtensions()
        {
        }

        public static void LoadRenderResultXml(this XmlDocument xmldoc, string renderResultXml)
        {
            if (xmldoc is null) throw new ArgumentNullException(nameof(xmldoc));
            try
            {
                var namespaceManager = new XmlNamespaceManager(new NameTable());
                namespaceManager.AddNamespace("regex", RegexXmlNamespace);
                var xmlContext = new XmlParserContext(null, namespaceManager, null, XmlSpace.Default);
                using XmlTextReader reader = new XmlTextReader(renderResultXml, XmlNodeType.Element, xmlContext);
                xmldoc.Load(reader);
            }
            catch (XmlException ex)
            {
                throw new RazorComponentRenderResultParseException($"An error occurred while trying to parse the render result of the test. {Environment.NewLine}" +
                    $"{ex.Message} Result XML:{Environment.NewLine}" +
                    $"{renderResultXml}", ex);
            }
        }
    }
}
