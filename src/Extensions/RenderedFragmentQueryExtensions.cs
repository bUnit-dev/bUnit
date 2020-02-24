using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngleSharp.Dom;
using AngleSharpWrappers;
using Xunit.Sdk;

namespace Bunit
{
    /// <summary>
    /// Helper methods for querying <see cref="IRenderedFragment"/>.
    /// </summary>
    public static class RenderedFragmentQueryExtensions
    {
        /// <summary>
        /// Returns the first element from the rendered fragment or component under test,
        /// using the provided <paramref name="cssSelector"/>, in a depth-first pre-order traversal 
        /// of the rendered nodes.
        /// </summary>
        /// <param name="renderedFragment">The rendered fragment to search.</param>
        /// <param name="cssSelector">The group of selectors to use.</param>
        public static IElement Find(this IRenderedFragment renderedFragment, string cssSelector)
        {
            if (renderedFragment is null) throw new ArgumentNullException(nameof(renderedFragment));
            var result = renderedFragment.Nodes.QuerySelector(cssSelector);
            if (result is null) throw new ElementNotFoundException(cssSelector);
            return WrapperFactory.Create(new ElemenFactory<IElement>(renderedFragment, result, cssSelector));
        }

        /// <summary>
        /// Returns a list of elements from the rendered fragment or component under test, 
        /// using the provided <paramref name="cssSelector"/>, in a depth-first pre-order traversal 
        /// of the rendered nodes.
        /// </summary>
        /// <param name="renderedFragment">The rendered fragment to search.</param>
        /// <param name="cssSelector">The group of selectors to use.</param>
        public static IHtmlCollection<IElement> FindAll(this IRenderedFragment renderedFragment, string cssSelector)
        {
            if (renderedFragment is null) throw new ArgumentNullException(nameof(renderedFragment));
            return renderedFragment.Nodes.QuerySelectorAll(cssSelector);
        }
    }

    internal sealed class ElemenFactory<TElement> : RenderEventSubscriber, IElementFactory<TElement>
        where TElement : class, IElement
    {
        private readonly IRenderedFragment _testTarget;
        private readonly string _cssSelector;
        private TElement? _element;

        public ElemenFactory(IRenderedFragment testTarget, TElement initialElement, string cssSelector)
            : base((testTarget ?? throw new ArgumentNullException(nameof(testTarget))).RenderEvents)
        {
            _testTarget = testTarget;
            _cssSelector = cssSelector;
            _element = initialElement;
        }

        public override void OnNext(RenderEvent value)
        {
            if (value.HasChangesTo(_testTarget))
                _element = null;
        }

        TElement IElementFactory<TElement>.GetElement()
        {
            if (_element is null)
            {
                var queryResult = _testTarget.Nodes.QuerySelector(_cssSelector);
                if(queryResult is TElement element)
                    _element = element;                
            }
            return _element ?? throw new ElementNotFoundException();
        }
    }

    /// <summary>
    /// Represents an exception that is thrown when a wrapped element is no longer available in the DOM tree.
    /// </summary>
    [SuppressMessage("Design", "CA1032:Implement standard exception constructors", Justification = "<Pending>")]
    public class ElementRemovedException : Exception
    {
        /// <inheritdoc/>
        public ElementRemovedException() : base("The DOM element you tried to access is no longer available in the DOM tree. It has probably been removed after a render.")
        {
        }
    }
}
