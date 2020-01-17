using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;

namespace Egil.RazorComponents.Testing.Mocking.JSInterop
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
            : base($"The invocation of '{invocation.Identifier}' {PrintArguments(invocation.Arguments)} was not expected.")
        {
            Invocation = invocation;
        }

        private static string PrintArguments(IReadOnlyList<object> arguments)
        {
            if (arguments.Count == 0)
            {
                return "without arguments";
            }
            else if (arguments.Count == 1)
            { 
                return $"with the argument [{arguments[0].ToString()}]";
            }
            else
            {
                return $"with arguments [{string.Join(", ", arguments.Select(x => x.ToString()))}]";
            }                
        }
    }
}
