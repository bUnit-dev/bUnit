using System;
using Microsoft.Extensions.DependencyInjection;

namespace Bunit.TestDoubles.JSInterop
{
	/// <summary>
	/// Helper methods for registering the MockJSRuntime with a <see cref="TestServiceProvider"/>.
	/// </summary>
	public static class MockJSRuntimeExtensions
	{
		/// <summary>
		/// Adds the <see cref="MockJSRuntimeInvokeHandler"/> to the <see cref="TestServiceProvider"/>.
		/// </summary>
		/// <returns>The added <see cref="MockJSRuntimeInvokeHandler"/>.</returns>
		public static MockJSRuntimeInvokeHandler AddMockJSRuntime(this TestServiceProvider serviceProvider, JSRuntimeMockMode mode = JSRuntimeMockMode.Loose)
		{
			if (serviceProvider is null)
				throw new ArgumentNullException(nameof(serviceProvider));

			var result = new MockJSRuntimeInvokeHandler(mode);

			serviceProvider.AddSingleton(result.ToJSRuntime());

			return result;
		}
	}
}
