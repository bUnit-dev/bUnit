using System;
using System.Collections.Generic;
using System.Linq;
using AngleSharp.Dom;

namespace Egil.RazorComponents.Testing
{
    public interface IRenderedFragment
    {
        string GetMarkup();

        INodeList GetNodes();
    }

    public static class RenderedFragmentExtensions
    {
        public static IElement Find(this IRenderedFragment renderedFragment, string selector)
        {
            if (renderedFragment is null) throw new ArgumentNullException(nameof(renderedFragment));
            var nodes = renderedFragment.GetNodes();            
            return nodes.QuerySelector(selector);
        }

        public static TElement Find<TElement>(this IRenderedFragment renderedFragment, string selector)
        {
            return (TElement)Find(renderedFragment, selector);
        }

        public static IHtmlCollection<IElement> FindAll(this IRenderedFragment renderedFragment, string selector)
        {
            if (renderedFragment is null) throw new ArgumentNullException(nameof(renderedFragment));
            return renderedFragment.GetNodes().QuerySelectorAll(selector);
        }
    }
}