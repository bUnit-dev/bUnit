using System;
using System.Threading;
using System.Diagnostics.CodeAnalysis;
using Bunit.Rendering;

namespace Bunit
{

	/// <summary>
	/// Helper methods dealing with async rendering during testing.
	/// </summary>
	[SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "<Pending>")]
    public static class RenderWaitingHelperExtensions
    {
        /// <summary>
        /// Wait for the next render to happen, or the <paramref name="timeout"/> is reached (default is one second).
        /// If a <paramref name="renderTrigger"/> action is provided, it is invoked before the waiting.
        /// </summary>
        /// <param name="testContext">The test context to wait for renders from.</param>
        /// <param name="renderTrigger">The action that somehow causes one or more components to render.</param>
        /// <param name="timeout">The maximum time to wait for the next render. If not provided the default is 1 second. During debugging, the timeout is automatically set to infinite.</param>        
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="testContext"/> is null.</exception>
        /// <exception cref="WaitForRenderFailedException">Thrown if no render happens within the specified <paramref name="timeout"/>, or the default of 1 second, if non is specified.</exception>
        [Obsolete("Use either the WaitForState or WaitForAssertion method instead. It will make your test more resilient to insignificant changes, as they will wait across multiple renders instead of just one. To make the change, run any render trigger first, then call either WaitForState or WaitForAssertion with the appropriate input. This method will be removed before the 1.0.0 release.", false)]
        public static void WaitForNextRender(this ITestContext testContext, Action? renderTrigger = null, TimeSpan? timeout = null)
            => WaitForRender(testContext.RenderEvents, renderTrigger, timeout);

        /// <summary>
        /// Wait until the provided <paramref name="statePredicate"/> action returns true,
        /// or the <paramref name="timeout"/> is reached (default is one second).
        /// 
        /// The <paramref name="statePredicate"/> is evaluated initially, and then each time
        /// the renderer in the <paramref name="testContext"/> renders.
        /// </summary>
        /// <param name="testContext">The test context to wait for renders from.</param>
        /// <param name="statePredicate">The predicate to invoke after each render, which returns true when the desired state has been reached.</param>
        /// <param name="timeout">The maximum time to wait for the desired state.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="testContext"/> is null.</exception>
        /// <exception cref="WaitForStateFailedException">Thrown if the <paramref name="statePredicate"/> throw an exception during invocation, or if the timeout has been reached. See the inner exception for details.</exception>
        public static void WaitForState(this ITestContext testContext, Func<bool> statePredicate, TimeSpan? timeout = null)
            => WaitForState(testContext.RenderEvents, statePredicate, timeout);

        /// <summary>
        /// Wait until the provided <paramref name="assertion"/> action passes (i.e. does not throw an 
        /// assertion exception), or the <paramref name="timeout"/> is reached (default is one second).
        /// 
        /// The <paramref name="assertion"/> is attempted initially, and then each time
        /// the renderer in the <paramref name="testContext"/> renders.
        /// </summary>
        /// <param name="testContext">The test context to wait for renders from.</param>
        /// <param name="assertion">The verification or assertion to perform.</param>
        /// <param name="timeout">The maximum time to attempt the verification.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="testContext"/> is null.</exception>
        /// <exception cref="WaitForAssertionFailedException">Thrown if the timeout has been reached. See the inner exception to see the captured assertion exception.</exception>
        public static void WaitForAssertion(this ITestContext testContext, Action assertion, TimeSpan? timeout = null)
            => WaitForAssertion(testContext.RenderEvents, assertion, timeout);

        /// <summary>
        /// Wait until the provided <paramref name="statePredicate"/> action returns true,
        /// or the <paramref name="timeout"/> is reached (default is one second).
        /// The <paramref name="statePredicate"/> is evaluated initially, and then each time
        /// the <paramref name="renderedFragment"/> renders.
        /// </summary>
        /// <param name="renderedFragment">The rendered fragment to wait for renders from.</param>
        /// <param name="statePredicate">The predicate to invoke after each render, which returns true when the desired state has been reached.</param>
        /// <param name="timeout">The maximum time to wait for the desired state.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="renderedFragment"/> is null.</exception>
        /// <exception cref="WaitForStateFailedException">Thrown if the <paramref name="statePredicate"/> throw an exception during invocation, or if the timeout has been reached. See the inner exception for details.</exception>
        public static void WaitForState(this IRenderedFragment renderedFragment, Func<bool> statePredicate, TimeSpan? timeout = null)
            => WaitForState(renderedFragment.RenderEvents, statePredicate, timeout);

        /// <summary>
        /// Wait until the provided <paramref name="assertion"/> action passes (i.e. does not throw an 
        /// assertion exception), or the <paramref name="timeout"/> is reached (default is one second).
        /// 
        /// The <paramref name="assertion"/> is attempted initially, and then each time
        /// the <paramref name="renderedFragment"/> renders.
        /// </summary>
        /// <param name="renderedFragment">The rendered fragment to wait for renders from.</param>
        /// <param name="assertion">The verification or assertion to perform.</param>
        /// <param name="timeout">The maximum time to attempt the verification.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="renderedFragment"/> is null.</exception>
        /// <exception cref="WaitForAssertionFailedException">Thrown if the timeout has been reached. See the inner exception to see the captured assertion exception.</exception>
        public static void WaitForAssertion(this IRenderedFragment renderedFragment, Action assertion, TimeSpan? timeout = null)
            => WaitForAssertion(renderedFragment.RenderEvents, assertion, timeout);

        private static void WaitForRender(IObservable<RenderEvent> renderEventObservable, Action? renderTrigger = null, TimeSpan? timeout = null)
        {
            if (renderEventObservable is null) throw new ArgumentNullException(nameof(renderEventObservable));

            var waitTime = timeout.GetRuntimeTimeout();

            var rvs = new ConcurrentRenderEventSubscriber(renderEventObservable);

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
                    throw new WaitForRenderFailedException();
            }
            finally
            {
                rvs.Unsubscribe();
            }

            bool ShouldSpin() => rvs.RenderCount > 0 || rvs.IsCompleted;
        }

        private static void WaitForState(IObservable<RenderEvent> renderEventObservable, Func<bool> statePredicate, TimeSpan? timeout = null)
        {
            if (renderEventObservable is null) throw new ArgumentNullException(nameof(renderEventObservable));
            if (statePredicate is null) throw new ArgumentNullException(nameof(statePredicate));

            const int STATE_MISMATCH = 0;
            const int STATE_MATCH = 1;
            const int STATE_EXCEPTION = -1;

            var spinTime = timeout.GetRuntimeTimeout();
            var failure = default(Exception);
            var status = STATE_MISMATCH;

            var rvs = new ConcurrentRenderEventSubscriber(renderEventObservable, onRender: TryVerification);
            try
            {
                TryVerification();
                WaitingResultHandler(continueIfMisMatch: true);

                // ComponentChangeEventSubscriber (rvs) receive render events on the renderer's thread, where as 
                // the VerifyAsyncChanges is started from the test runners thread.
                // Thus it is safe to SpinWait on the test thread and wait for verification to pass.
                // When a render event is received by rvs, the verification action will execute on the
                // renderer thread.
                // 
                // Therefore, we use Volatile.Read/Volatile.Write in the helper methods below to ensure
                // that an update to the variable status is not cached in a local CPU, and 
                // not available in a secondary CPU, if the two threads are running on a different CPUs
                SpinWait.SpinUntil(ShouldSpin, spinTime);
                WaitingResultHandler(continueIfMisMatch: false);
            }
            finally
            {
                rvs.Unsubscribe();
            }

            void WaitingResultHandler(bool continueIfMisMatch)
            {
                switch (status)
                {
                    case STATE_MATCH: return;
                    case STATE_MISMATCH when !continueIfMisMatch && failure is null:
                        throw WaitForStateFailedException.CreateNoMatchBeforeTimeout();
                    case STATE_EXCEPTION when failure is { }:
                        throw WaitForStateFailedException.CreatePredicateThrowException(failure);
                }
            }

            void TryVerification(RenderEvent _ = default!)
            {
                try
                {
                    if (statePredicate()) Volatile.Write(ref status, STATE_MATCH);
                }
                catch (Exception e)
                {
                    failure = e;
                    Volatile.Write(ref status, STATE_EXCEPTION);
                }
            }

            bool ShouldSpin() => Volatile.Read(ref status) == STATE_MATCH || rvs.IsCompleted;
        }

        private static void WaitForAssertion(IObservable<RenderEvent> renderEventObservable, Action assertion, TimeSpan? timeout = null)
        {
            if (renderEventObservable is null) throw new ArgumentNullException(nameof(renderEventObservable));
            if (assertion is null) throw new ArgumentNullException(nameof(assertion));

            const int FAILING = 0;
            const int PASSED = 1;

            var spinTime = timeout.GetRuntimeTimeout();
            var failure = default(Exception);
            var status = FAILING;

            var rvs = new ConcurrentRenderEventSubscriber(renderEventObservable, onRender: TryVerification);
            try
            {
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

                if (status == FAILING && failure is { })
                {
                    throw new WaitForAssertionFailedException(failure);
                }
            }
            finally
            {
                rvs.Unsubscribe();
            }

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

            bool ShouldSpin() => Volatile.Read(ref status) == PASSED || rvs.IsCompleted;
        }
    }
}
