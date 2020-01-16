using AngleSharp.Dom;
using Egil.RazorComponents.Testing.Diffing;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.RenderTree;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Egil.RazorComponents.Testing
{
    /// <summary>
    /// A test context is a factory that makes it possible to create components under tests.
    /// </summary>
    public class TestContext : ITestContext, IDisposable
    {
        private readonly Lazy<TestRenderer> _renderer;
        private readonly Lazy<TestHtmlParser> _htmlParser;

        /// <inheritdoc/>
        public virtual TestRenderer Renderer => _renderer.Value;

        /// <inheritdoc/>
        public virtual TestServiceProvider Services { get; } = new TestServiceProvider();

        /// <summary>
        /// Creates a new instance of the <see cref="TestContext"/> class.
        /// </summary>
        public TestContext()
        {
            _renderer = new Lazy<TestRenderer>(() =>
            {
                var loggerFactory = Services.GetService<ILoggerFactory>() ?? new NullLoggerFactory();
                return new TestRenderer(Services, loggerFactory);
            });
            _htmlParser = new Lazy<TestHtmlParser>(() =>
            {
                return new TestHtmlParser(Renderer, new HtmlComparer());
            });
        }

        /// <inheritdoc/>
        public virtual INodeList CreateNodes(string markup)
            => _htmlParser.Value.Parse(markup);

        /// <inheritdoc/>
        public virtual IRenderedComponent<TComponent> RenderComponent<TComponent>(params ComponentParameter[] parameters) where TComponent : class, IComponent
        {
            var result = new RenderedComponent<TComponent>(this, parameters);
            return result;
        }

        /// <inheritdoc/>
        public virtual void WaitForNextRender(Action renderTrigger, TimeSpan? timeout = null)
        {
            if (renderTrigger is null) throw new ArgumentNullException(nameof(renderTrigger));
            var task = Renderer.NextRender;
            renderTrigger();
            task.Wait(timeout ?? TimeSpan.FromSeconds(1));

            if (!task.IsCompleted)
            {
                throw new TimeoutException("No render occurred within the timeout period.");
            }
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
                    if (_renderer.IsValueCreated)
                        _renderer.Value.Dispose();
                    if (_htmlParser.IsValueCreated)
                        _htmlParser.Value.Dispose();

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
