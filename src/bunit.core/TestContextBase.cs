using System;
using Bunit.Rendering;
using Microsoft.Extensions.DependencyInjection;

namespace Bunit
{
	/// <summary>
	/// A test context is a factory that makes it possible to create components under tests.
	/// </summary>
	public abstract class TestContextBase : IDisposable
	{
		private ITestRenderer? _testRenderer;

		/// <summary>
		/// Gets the renderer used by the test context.
		/// </summary>
		public ITestRenderer Renderer
		{
			get
			{
				if (_testRenderer is null)
				{
					_testRenderer = Services.GetRequiredService<ITestRenderer>();
				}
				return _testRenderer;
			}
		}

		/// <summary>
		/// Gets the service collection and service provider that is used when a 
		/// component is rendered by the test context.
		/// </summary>
		public TestServiceProvider Services { get; }

		/// <summary>
		/// Creates a new instance of the <see cref="TestContextBase"/> class.
		/// </summary>
		protected TestContextBase()
		{
			Services = new TestServiceProvider();
			Services.AddSingleton<ITestRenderer, TestRenderer>();
		}

		/// <inheritdoc/>
		public virtual void Dispose()
		{
			// The service provider should dispose of any
			// disposable object it has created, when it is disposed.
			Services.Dispose();
		}
	}
}
