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

                // RenderEventSubscriber (rvs) receive render events on the renderer's thread, where as 
                // the WaitForNextRender is started from the test runners thread.
                // Thus it is safe to SpinWait on the test thread and wait for the RenderCount to go above 0.
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
        /// Uses the provided <paramref name="statePredicate"/> action to verify 
        /// that an expected state has been reached in the <paramref name="renderedFragment"/>
        /// within the specified <paramref name="timeout"/> (default is one second).
        /// </summary>
        /// <param name="renderedFragment"></param>
        /// <param name="statePredicate"></param>
        /// <param name="timeout">  </param>
        public static void WaitForState(this IRenderedFragment renderedFragment, Func<bool> statePredicate, TimeSpan? timeout = null)
        {
            if (renderedFragment is null) throw new ArgumentNullException(nameof(renderedFragment));
            if (statePredicate is null) throw new ArgumentNullException(nameof(statePredicate));

            var spinTime = timeout.GetRuntimeTimeout();
            var predicateResult = false;

            var rvs = new RenderEventSubscriber(renderedFragment.RenderEvents, onRender: TryPredicate);
            try
            {
                TryPredicate();
                if (predicateResult) return;

                // RenderEventSubscriber (rvs) receive render events on the renderer's thread, where as 
                // the WaitForState is started from the test runners thread.
                // Thus it is safe to SpinWait on the test thread and wait for statePredicate to pass.
                // When a render event is received by rvs, the state predicate will execute on the
                // renderer thread.
                // 
                // Therefore, we use Volatile.Read/Volatile.Write in the helper methods below to ensure
                // that an update to the variable predicateResult is not cached in a local CPU, and 
                // not available in a secondary CPU, if the two threads are running on a different CPUs
                SpinWait.SpinUntil(ShouldSpin, spinTime);

                if (!predicateResult) throw new TimeoutException("The predicate did not pass within the timeout period.");
            }
            finally
            {
                rvs.Unsubscribe();
            }
            bool ShouldSpin() => Volatile.Read(ref predicateResult) || rvs.IsCompleted;
            void TryPredicate(RenderEvent _ = default!) => Volatile.Write(ref predicateResult, statePredicate());
        }

        /// <summary>
        /// Uses the provided <paramref name="assertion"/> action to verify/assert 
        /// that an expected change has occurred in the <paramref name="renderedFragment"/>
        /// within the specified <paramref name="timeout"/> (default is one second).
        /// </summary>
        /// <param name="renderedFragment">The rendered component or fragment to verify against.</param>
        /// <param name="assertion">The verification or assertion to perform.</param>
        /// <param name="timeout">The maximum time to attempt the verification.</param>
        public static void WaitForAssertion(this IRenderedFragment renderedFragment, Action assertion, TimeSpan? timeout = null)
        {
            if (renderedFragment is null) throw new ArgumentNullException(nameof(renderedFragment));
            if (assertion is null) throw new ArgumentNullException(nameof(assertion));

            const int FAILING = 0;
            const int PASSED = 1;

            var spinTime = timeout.GetRuntimeTimeout();
            var failure = default(Exception);
            var status = FAILING;

            using var rvs = new ComponentChangeEventSubscriber(renderedFragment, onChange: TryVerification);

            TryVerification();
            if (status == PASSED) return;

            // HasChangesRenderEventSubscriber (rvs) receive render events on the renderer's thread, where as 
            // the VerifyAsyncChanges is started from the test runners thread.
            // Thus it is safe to SpinWait on the test thread and wait for verification to pass.
            // When a render event is received by rvs, the verification action will execute on the
            // renderer thread.
            // 
            // Therefore, we use Volatile.Read/Volatile.Write in the helper methods below to ensure
            // that an update to the variable status is not cached in a local CPU, and 
            // not available in a secondary CPU, if the two threads are running on a different CPUs
            SpinWait.SpinUntil(ShouldSpin, spinTime);

            if (status != PASSED && failure is { }) throw failure;

            void TryVerification(RenderEvent _ = default!)
            {
                try
                {
                    assertion();
                    Volatile.Write(ref status, PASSED);
                    failure = null;
                }
                catch (Exception e)
                {
                    failure = e;
                }
            }

            bool ShouldSpin() => Volatile.Read(ref status) != FAILING || rvs.IsCompleted;
        }
    }
}
