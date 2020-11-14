using System;

namespace Bunit
{
	/// <summary>
	/// Helper methods for registering the MockJSRuntime with a <see cref="TestServiceProvider"/>.
	/// </summary>
	public static class MockJSRuntimeExtensions
	{
		/// <summary>
		/// Obsolete: bUnits JSRuntime is now always available through the <see cref="TestContext.JSInterop" /> property.
		/// Use that instead going forward. To change the <see cref="JSRuntimeMode"/>, change it through <see cref="BunitJSInterop.Mode"/>.
		/// </summary>
		/// <returns>The added <see cref="BunitJSInterop"/>.</returns>
		[Obsolete("bUnits JSRuntime is now always available through the TeBunitJSInteropstContext.JSInterop method.", true)]
		public static BunitJSInterop AddMockJSRuntime(this TestServiceProvider serviceProvider, JSRuntimeMode mode = JSRuntimeMode.Loose)
			=> throw new InvalidOperationException("Obsolete: bUnits JSRuntime is now always available through the TeBunitJSInteropstContext.JSInterop method. See release notes for details.");
	}
}
