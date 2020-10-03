using System;
using Bunit.Rendering;

namespace Bunit
{
	/// <summary>
	/// A test context is a factory that makes it possible to create components under tests.
	/// </summary>
	public partial interface ITestContext : IDisposable
	{
		/// <summary>
		/// Gets the renderer used by the test context.
		/// </summary>
		ITestRenderer Renderer { get; }

		/// <summary>
		/// Gets the service collection and service provider that is used when a 
		/// component is rendered by the test context.
		/// </summary>
		TestServiceProvider Services { get; }
	}
}
