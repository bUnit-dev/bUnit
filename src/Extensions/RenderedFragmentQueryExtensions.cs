using System;
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
            return WrapperFactory.Create(new ElementFactory<IElement>(renderedFragment, result, cssSelector));
        }

        /// <summary>
        /// Returns a refreshable collection of <see cref="IElement"/>s from the rendered fragment or component under test, 
        /// using the provided <paramref name="cssSelector"/>, in a depth-first pre-order traversal 
        /// of the rendered nodes.
        /// </summary>
        /// <param name="renderedFragment">The rendered fragment to search.</param>
        /// <param name="cssSelector">The group of selectors to use.</param>
        /// <param name="enableAutoRefresh">If true, the returned <see cref="IRefreshableElementCollection{IElement}"/> will automatically refresh its <see cref="IElement"/>s whenever the <paramref name="renderedFragment"/> changes.</param>
        /// <returns>An <see cref="IRefreshableElementCollection{IElement}"/>, that can be refreshed to execute the search again.</returns>
        public static IRefreshableElementCollection<IElement> FindAll(this IRenderedFragment renderedFragment, string cssSelector, bool enableAutoRefresh = false)
        {
            if (renderedFragment is null) throw new ArgumentNullException(nameof(renderedFragment));
            return new RefreshableElementCollection(renderedFragment, cssSelector) { EnableAutoRefresh = enableAutoRefresh };
        }
    }
}
