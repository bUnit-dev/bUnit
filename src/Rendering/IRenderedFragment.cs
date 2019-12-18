using System;
using System.Collections.Generic;
using System.Linq;
using AngleSharp.Diffing.Core;
using AngleSharp.Dom;

namespace Egil.RazorComponents.Testing
{
    /// <summary>
    /// Represents a rendered fragment.
    /// </summary>
    public interface IRenderedFragment
    {
        /// <summary>
        /// Gets the <see cref="ITestContext"/> which this rendered fragment belongs to.
        /// </summary>
        ITestContext TestContext { get; }

        /// <summary>
        /// Gets the HTML markup from the rendered fragment/component.
        /// </summary>
        /// <returns></returns>
        string GetMarkup();

        /// <summary>
        /// Gets the AngleSharp <see cref="INodeList"/> based
        /// on the HTML markup from the rendered fragment/component.
        /// </summary>
        /// <returns></returns>
        INodeList GetNodes();

        /// <summary>
        /// Performs a comparison of the markup produced by the initial rendering of the 
        /// fragment or component under test with the current rendering of the fragment 
        /// or component under test.
        /// </summary>
        /// <returns>A list of differences found.</returns>
        IReadOnlyList<IDiff> GetChangesSinceFirstRender();

        /// <summary>
        /// Performs a comparison of the markup produced by the rendering of the 
        /// fragment or component under test at the time the <see cref="TakeSnapshot"/> was called
        /// with the current rendering of the fragment or component under test.
        /// </summary>
        /// <returns>A list of differences found.</returns>
        IReadOnlyList<IDiff> GetChangesSinceSnapshot();

        /// <summary>
        /// Saves the markup from the current rendering of the fragment or component under test.
        /// Use the method <see cref="GetChangesSinceSnapshot"/> later to get the difference between
        /// the snapshot and the rendered markup at that time.
        /// </summary>
        void TakeSnapshot();

        /// <summary>
        /// Returns the first element from the rendered fragment or component under test,
        /// using the provided <paramref name="cssSelector"/>, in a depth-first pre-order traversal 
        /// of the rendered nodes.
        /// </summary>
        /// <param name="cssSelector">The group of selectors to use.</param>
        public IElement Find(string cssSelector)
        {
            var result = GetNodes().QuerySelector(cssSelector);
            if (result is null)
                throw new ElementNotFoundException(cssSelector);
            else
                return result;
        }

        /// <summary>
        /// Returns a list of elements from the rendered fragment or component under test, 
        /// using the provided <paramref name="cssSelector"/>, in a depth-first pre-order traversal 
        /// of the rendered nodes.
        /// </summary>
        /// <param name="cssSelector">The group of selectors to use.</param>
        public IHtmlCollection<IElement> FindAll(string cssSelector)
        {
            return GetNodes().QuerySelectorAll(cssSelector);
        }
    }
}