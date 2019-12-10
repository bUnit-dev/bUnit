using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;

namespace Egil.RazorComponents.Testing
{
    /// <summary>
    /// Exception use to indicate that an unplanned invocation was
    /// received by the <see cref="MockJsRuntimeInvokeHandler"/> running in <see cref="JsRuntimeMockMode.Strict"/>.
    /// </summary>
    [SuppressMessage("Design", "CA1032:Implement standard exception constructors", Justification = "<Pending>")]
    public class UnplannedJsInvocationException : Exception
    {
        /// <summary>
        /// Gets the unplanned invocation.
        /// </summary>
        public JsRuntimeInvocation Invocation { get; }

        /// <summary>
        /// Creates a new instance of the <see cref="UnplannedJsInvocationException"/>
        /// with the provided <see cref="Invocation"/> attached.
        /// </summary>
        /// <param name="invocation">The unplanned invocation.</param>
        public UnplannedJsInvocationException(JsRuntimeInvocation invocation)
            : base($"The invocation of '{invocation.Identifier} with arguments '[{PrintArguments(invocation.Arguments)}]")
        {
            Invocation = invocation;
        }

        private static string PrintArguments(IReadOnlyList<object> arguments)
        {
            return string.Join(", ", arguments.Select(x => x.ToString()));
        }
    }
}
