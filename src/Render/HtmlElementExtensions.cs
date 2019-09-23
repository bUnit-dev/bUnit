using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngleSharp.Html.Dom;

namespace Egil.RazorComponents.Testing.Render
{
    public static class HtmlElementExtensions
    {
        public static void Click(this IHtmlElement element, string selector) { }
    }
}
