using AngleSharp.Dom;
using Bunit.Diffing;
using Bunit.Mocking.JSInterop;
using Bunit.Rendering;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.JSInterop;
using System;

namespace Bunit
{
	/// <summary>
	/// A test context is a factory that makes it possible to create components under tests.
	/// </summary>
	public class TestContext : ITestContext, IDisposable
    {
		private TestRenderer? _testRenderer;

		/// <summary>
		/// Gets the renderer used by the test context.
		/// </summary>
		protected TestRenderer Renderer
		{
			get
			{
				if(_testRenderer is null)_testRenderer = Services.GetRequiredService<TestRenderer>();
				return _testRenderer;
			}
		}

        /// <inheritdoc/>
        public virtual TestServiceProvider Services { get; } = new TestServiceProvider();

		/// <inheritdoc/>
		public IObservable<RenderEvent> RenderEvents => Renderer.RenderEvents;

		/// <summary>
		/// Creates a new instance of the <see cref="TestContext"/> class.
		/// </summary>
		public TestContext()
        {
            Services.AddSingleton<IJSRuntime>(new PlaceholderJsRuntime());
			Services.AddSingleton<TestRenderer>(srv => new TestRenderer(srv, srv.GetService<ILoggerFactory>() ?? NullLoggerFactory.Instance));
			Services.AddSingleton<TestHtmlParser>(srv => new TestHtmlParser(srv.GetRequiredService<TestRenderer>()));
        }

        /// <inheritdoc/>
        public virtual IRenderedComponent<TComponent> RenderComponent<TComponent>(params ComponentParameter[] parameters) where TComponent : IComponent
        {
			var renderResult = Renderer.RenderComponent<TComponent>(parameters).GetAwaiter().GetResult();
            return new RenderedComponent<TComponent>(Services, renderResult.ComponentId, renderResult.Component);
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
