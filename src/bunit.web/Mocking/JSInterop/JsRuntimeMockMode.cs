using Microsoft.JSInterop;
using Xunit.Sdk;

namespace Bunit.Mocking.JSInterop
{
    /// <summary>
    /// The execution mode of the <see cref="MockJsRuntimeExtensions"/>.
    /// </summary>
    public enum JsRuntimeMockMode
    {
        /// <summary>
        /// <see cref="JsRuntimeMockMode.Loose"/> configures the <see cref="MockJsRuntimeExtensions"/> to return default TValue 
        /// for <see cref="IJSRuntime.InvokeAsync{TValue}(string, object[])"/> calls to the mock.
        /// </summary>
        Loose = 0,
        /// <summary>
        /// <see cref="JsRuntimeMockMode.Strict"/> configures the <see cref="MockJsRuntimeExtensions"/> to throw an
        /// <see cref="UnplannedJsInvocationException"/> exception when a call to 
        /// for <see cref="IJSRuntime.InvokeAsync{TValue}(string, object[])"/> has not been 
        /// setup in the mock.
        /// </summary>
        Strict
    }
}
