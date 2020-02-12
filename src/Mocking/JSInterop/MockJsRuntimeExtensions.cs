using System;
using Bunit.Mocking.JSInterop;
using Microsoft.Extensions.DependencyInjection;

namespace Bunit.Mocking.JSInterop
{
    /// <summary>
    /// Helper methods for registering the MockJsRuntime with a <see cref="TestServiceProvider"/>.
    /// </summary>
    public static class MockJsRuntimeExtensions
    {
        /// <summary>
        /// Adds the <see cref="MockJsRuntimeInvokeHandler"/> to the <see cref="TestServiceProvider"/>.
        /// </summary>
        /// <returns>The added <see cref="MockJsRuntimeInvokeHandler"/>.</returns>
        public static MockJsRuntimeInvokeHandler AddMockJsRuntime(this TestServiceProvider serviceProvider, JsRuntimeMockMode mode = JsRuntimeMockMode.Loose)
        {
            if (serviceProvider is null) throw new ArgumentNullException(nameof(serviceProvider));

            var result = new MockJsRuntimeInvokeHandler(mode);

            serviceProvider.AddSingleton(result.ToJsRuntime());

            return result;
        }
    }
}
