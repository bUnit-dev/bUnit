using System;
using System.Diagnostics;
using System.Threading;

namespace Bunit
{
    /// <summary>
    /// Helper methods for working with <see cref="TimeSpan"/>.
    /// </summary>
    public static class TimeSpanExtensions
    {
        /// <summary>
        /// Returns a timeout time as a <see cref="TimeSpan"/>, set to <see cref="Timeout.InfiniteTimeSpan"/>
        /// if <see cref="Debugger.IsAttached"/>, or the provided <paramref name="timeout"/> if it is not null.
        /// If it is null, the default of one second is used.
        /// </summary>
        public static TimeSpan GetRuntimeTimeout(this TimeSpan? timeout)
        {
            return Debugger.IsAttached ? Timeout.InfiniteTimeSpan : timeout ?? TimeSpan.FromSeconds(1);
        }
    }
}
