using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.JSInterop;

[assembly: InternalsVisibleTo("Egil.RazorComponents.Testing.Library.Tests")]

namespace Egil.RazorComponents.Testing.Mocking.JSInterop
{
    /// <summary>
    /// This JsRuntime is used to provide users with helpful exceptions if they fail to provide a mock when required. 
    /// </summary>
    internal class DefaultJsRuntime : IJSRuntime
    {
        internal const string MissingJsRuntimeMessage = "This test requires a IJsRuntime to be supplied, because the component under test invokes the IJsRuntime during the test. Guidance on mocking the IJsRuntime is available in the testing library's Wiki. See exception data for the specific method and arguments used.";
        internal const string MissingJsRuntimeHelpLink = "https://github.com/egil/razor-components-testing-library/wiki/Mocking-JsRuntime";

        public ValueTask<TValue> InvokeAsync<TValue>(string identifier, object[] args)
        {
            var ex =  new Exception(MissingJsRuntimeMessage);
            ex.HelpLink = MissingJsRuntimeHelpLink;
            ex.Data.Add("identifier", identifier);
            ex.Data.Add("args", args);
            throw ex;
        }

        public ValueTask<TValue> InvokeAsync<TValue>(string identifier, CancellationToken cancellationToken, object[] args)
        {
            var ex = new Exception(MissingJsRuntimeMessage);
            ex.HelpLink = MissingJsRuntimeHelpLink;
            ex.Data.Add("identifier", identifier);
            ex.Data.Add("CancelationToken", cancellationToken);
            ex.Data.Add("args", args);
            throw ex;
        }
    }
}
