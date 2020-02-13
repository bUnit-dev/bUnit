using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Bunit
{
    /// <summary>
    /// Helper methods for <see cref="ITestContext"/>.
    /// </summary>
    public static class TestContextExtensions
    {
        /// <summary>
        /// Executes the provided <paramref name="renderTrigger"/> action and waits for a render to occur.
        /// Use this when you have a component that is awaiting e.g. a service to return data to it before rendering again.
        /// </summary>
        /// <param name="testContext">The context to wait against.</param>
        /// <param name="renderTrigger">The action that somehow causes one or more components to render.</param>
        /// <param name="timeout">The maximum time to wait for the next render. If not provided the default is 1 second.</param>
        /// <exception cref="TimeoutException">Thrown when the next render did not happen within the specified <paramref name="timeout"/>.</exception>
        public static void WaitForNextRender(this ITestContext testContext, Action? renderTrigger = null, TimeSpan? timeout = null)
        {
            if (testContext is null) throw new ArgumentNullException(nameof(testContext));

            var waitTime = Debugger.IsAttached ? Timeout.InfiniteTimeSpan : timeout ?? TimeSpan.FromSeconds(1);

            using var rvs = new RenderEventSubscriber(testContext.Renderer.RenderEvents);

            renderTrigger?.Invoke();

            if (rvs.RenderCount >= 1) return;

            if (SpinWait.SpinUntil(() => rvs.RenderCount >= 1, waitTime))
                return;
            else
                throw new TimeoutException("No render occurred within the timeout period.");
        }
    }


}
