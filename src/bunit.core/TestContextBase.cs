using Bunit.Rendering;
using Bunit.Rendering.RenderEvents;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;

namespace Bunit
{
	/// <summary>
	/// A test context is a factory that makes it possible to create components under tests.
	/// </summary>
	public class TestContextBase : ITestContext, IDisposable
	{
		private ITestRenderer? _testRenderer;

		/// <inheritdoc/>
		public ITestRenderer Renderer
		{
			get
			{
				if (_testRenderer is null)
					_testRenderer = Services.GetRequiredService<ITestRenderer>();
				return _testRenderer;
			}
		}

		/// <inheritdoc/>
		public virtual TestServiceProvider Services { get; } = new TestServiceProvider();

		/// <inheritdoc/>
		public IObservable<RenderEvent> RenderEvents => Renderer.RenderEvents;

		/// <summary>
		/// Creates a new instance of the <see cref="ITestContext"/> class.
		/// </summary>
		public TestContextBase()
		{
			Services.AddSingleton<ITestRenderer>(srv => new TestRenderer(srv, srv.GetService<ILoggerFactory>() ?? NullLoggerFactory.Instance));
		}

		#region IDisposable Support
		private bool _disposed = false;

		/// <inheritdoc/>
		protected virtual void Dispose(bool disposing)
		{
			if (!_disposed)
			{
				if (disposing)
				{
					Services.Dispose();
				}
				_disposed = true;
			}
		}

		/// <inheritdoc/>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
		#endregion
	}
}
