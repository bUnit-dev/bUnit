using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;

namespace Egil.RazorComponents.Testing
{
    /// <summary>
    /// Exception use to indicate that a MockJsRuntime is required by a test
    /// but was not provided.
    /// </summary>
    [SuppressMessage("Design", "CA1032:Implement standard exception constructors", Justification = "<Pending>")]
    public class MissingMockJsRuntimeException : Exception
    {
        /// <summary>
        /// Identifer string used in the JSInvoke method.
        /// </summary>
        public string Identifier { get; }

        /// <summary>
        /// Arguments passed to the JSInvoke method.
        /// </summary>
        public object[] Args { get; }

        /// <summary>
        /// Creates a new instance of the <see cref="MissingMockJsRuntimeException"/>
        /// with the provided <see cref="Invocation"/> attached.
        /// </summary>
        /// <param name="identifier">The identifer used in the invocation.</param>
        /// <param name="args">The args used in the invocation, if any</param>
        public MissingMockJsRuntimeException(string identifier, object[] args)
            : base($"This test requires a IJsRuntime to be supplied, because the component under test invokes the IJsRuntime during the test. The invoked method is '{identifier}' and the invocation args are stored in the Args property of this exception. Guidance on mocking the IJsRuntime is available in the testing library's Wiki.") 
        {
            Identifier = identifier;
            Args = args;
            HelpLink = "https://github.com/egil/razor-components-testing-library/wiki/Mocking-JsRuntime";
        }
    }
}
