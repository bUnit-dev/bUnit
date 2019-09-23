using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngleSharp;
using AngleSharp.Dom;
using Shouldly;
using Xunit;

namespace Egil.RazorComponents.Testing.AngleSharpDiffing
{
    public class HtmlDifferTest
    {
        private IBrowsingContext _context = BrowsingContext.New();
        private IDocument _document;

        public HtmlDifferTest()
        {
            _document = _context.OpenNewAsync().Result;
        }

        #pragma warning disable CS8604 // Possible null reference argument.
        [Fact(DisplayName = "Passsing null values to builder throws ArgumentNullException")]
        public void BuildNullArgsThrows()
        {
            Should.Throw<ArgumentNullException>(() => DiffBuilder.Compare(null))
                .ParamName.ShouldBe("controlNode");
            Should.Throw<ArgumentNullException>(() => DiffBuilder.Compare(CreateTextNode()).WithTest(null))
                .ParamName.ShouldBe("testNode"); ;
        }
        #pragma warning restore CS8604 // Possible null reference argument.
    
        private IText CreateTextNode(string? text = null)
        {
            return _document.CreateTextNode(text ?? string.Empty);
        }
    }
}
