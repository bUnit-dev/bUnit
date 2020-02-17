using System;
using System.Diagnostics;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Diagnostics.CodeAnalysis;

namespace Bunit
{
    /// <summary>
    /// Helper methods dealing with async rendering during testing.
    /// </summary>
    [SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "<Pending>")]
    public static class AsyncRenderingHelperExtensions
    {
        /// <summary>
        /// Executes the provided <paramref name="renderTrigger"/> action and waits for a render to occur.
        /// Use this when you have a component that is awaiting e.g. a service to return data to it before rendering again.
        /// </summary>
        /// <param name="testContext">The context to wait against.</param>
        /// <param name="renderTrigger">The action that somehow causes one or more components to render.</param>
        /// <param name="timeout">The maximum time to wait for the next render. If not provided the default is 1 second. During debugging, the timeout is automatically set to infinite.</param>
        public static void WaitForNextRender(this ITestContext testContext, Action? renderTrigger = null, TimeSpan? timeout = null)
        {
            if (testContext is null) throw new ArgumentNullException(nameof(testContext));

            var waitTime = timeout.GetRuntimeTimeout();

            var rvs = new RenderEventSubscriber(testContext.Renderer.RenderEvents);

            try
            {
                renderTrigger?.Invoke();

                if (rvs.RenderCount > 0) return;

                if (SpinWait.SpinUntil(ShouldSpin, waitTime) && rvs.RenderCount > 0)
                    return;
                else
                    throw new TimeoutException("No render occurred within the timeout period.");
            }
            finally
            {
                rvs.Unsubscribe();
            }

            bool ShouldSpin() => rvs.RenderCount > 0 || rvs.IsCompleted;
        }

        /// <summary>
        /// Uses the provided <paramref name="verification"/> action to verify 
        /// that an expected change has occurred in the <paramref name="renderedFragment"/>
        /// without the specified <paramref name="timeout"/> (default is one second).
        /// </summary>
        /// <param name="renderedFragment"></param>
        /// <param name="verification"></param>
        /// <param name="timeout">  </param>
        public static void VerifyAsyncChanges(this IRenderedFragment renderedFragment, Action verification, TimeSpan? timeout = null)
        {
            if (renderedFragment is null) throw new ArgumentNullException(nameof(renderedFragment));
            if (verification is null) throw new ArgumentNullException(nameof(verification));

            const int FAILING = 0;
            const int PASSED = 1;

            var spinTime = timeout.GetRuntimeTimeout();
            var failure = default(Exception);
            var status = FAILING;

            using var rvs = new HasChangesRenderEventSubscriber(renderedFragment, TryVerification);

            TryVerification();
            if (status == PASSED) return;

            SpinWait.SpinUntil(ShouldSpin, spinTime);

            if (status != PASSED && failure is { }) throw failure;

            void TryVerification(RenderEvent _ = default)
            {
                try
                {
                    verification();
                    status = PASSED;
                    failure = null;
                }
                catch (Exception e)
                {
                    failure = e;
                }
            }

            bool ShouldSpin() => status != FAILING || rvs.IsCompleted;
        }
    }
}
