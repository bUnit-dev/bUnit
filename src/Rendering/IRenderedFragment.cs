using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AngleSharp.Diffing.Core;
using AngleSharp.Dom;
using Xunit.Sdk;

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
        string Markup { get; }

        /// <summary>
        /// Gets the AngleSharp <see cref="INodeList"/> based
        /// on the HTML markup from the rendered fragment/component.
        /// </summary>
        INodeList Nodes { get; }

        /// <summary>
        /// Performs a comparison of the markup produced by the initial rendering of the 
        /// fragment or component under test with the current rendering of the fragment 
        /// or component under test.
        /// </summary>
        /// <returns>A list of differences found.</returns>
        IReadOnlyList<IDiff> GetChangesSinceFirstRender();

        /// <summary>
        /// Performs a comparison of the markup produced by the rendering of the 
        /// fragment or component under test at the time the <see cref="SaveSnapshot"/> was called
        /// with the current rendering of the fragment or component under test.
        /// </summary>
        /// <returns>A list of differences found.</returns>
        IReadOnlyList<IDiff> GetChangesSinceSnapshot();

        /// <summary>
        /// Saves the markup from the current rendering of the fragment or component under test.
        /// Use the method <see cref="GetChangesSinceSnapshot"/> later to get the difference between
        /// the snapshot and the rendered markup at that time.
        /// </summary>
        void SaveSnapshot();

        /// <summary>
        /// Trigger an update/render of the component under test using the 
        /// provided <paramref name="renderTrigger"/> and waits for 
        /// the next update to the component/fragment to happen.
        /// A timeout can be specified using the <paramref name="timeout"/> argument (default is 1 second).
        /// 
        /// Note: when a debugger is attached, the timeout is infinite.
        /// </summary>
        /// <param name="renderTrigger">The action that triggers a render/update of the component/fragment</param>
        /// <param name="timeout">An optional amount of time to wait before throwing.</param>
        /// <exception cref="TimeoutException">Thrown when the timeout is passed, and no update has been detected to the component/fragment.</exception>
        void WaitForNextUpdate(Action renderTrigger, TimeSpan? timeout = null);

        /// <summary>
        /// Returns the first element from the rendered fragment or component under test,
        /// using the provided <paramref name="cssSelector"/>, in a depth-first pre-order traversal 
        /// of the rendered nodes.
        /// </summary>
        /// <param name="cssSelector">The group of selectors to use.</param>
        public IElement Find(string cssSelector)
        {
            var result = Nodes.QuerySelector(cssSelector);
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
            return Nodes.QuerySelectorAll(cssSelector);
        }
    }
}