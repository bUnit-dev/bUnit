using System;
using System.Diagnostics;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Diagnostics.CodeAnalysis;

namespace Bunit
{
    /// <summary>
    /// Helper methods for <see cref="IRenderedFragment"/>.
    /// </summary>
    public static class RenderedFragmentExtensions
    {
        /// <summary>
        /// Uses the provided <paramref name="verification"/> action to verify 
        /// that an expected change has occurred in the <paramref name="renderedFragment"/>
        /// without the specified <paramref name="timeout"/> (default is one second).
        /// </summary>
        /// <param name="renderedFragment"></param>
        /// <param name="verification"></param>
        /// <param name="timeout">  </param>
        [SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "<Pending>")]
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
