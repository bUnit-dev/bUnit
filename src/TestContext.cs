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
        private readonly Lazy<HtmlParser> _htmlParser;

        /// <inheritdoc/>
        public TestRenderer Renderer => _renderer.Value;

        /// <inheritdoc/>
        public HtmlParser HtmlParser => _htmlParser.Value;

        /// <inheritdoc/>
        public TestServiceProvider Services { get; } = new TestServiceProvider();

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
            _htmlParser = new Lazy<HtmlParser>(() =>
            {
                return new HtmlParser(Renderer, new HtmlComparer());
            });
        }

        /// <inheritdoc/>
        //public IRenderedComponent<TComponent> RenderComponent<TComponent>() where TComponent : class, IComponent
        //{
        //    return RenderComponent<TComponent>(ParameterView.Empty);
        //}

        /// <inheritdoc/>
        //public IRenderedComponent<TComponent> RenderComponent<TComponent>(params (string paramName, object? valueValue)[] parameters) where TComponent : class, IComponent
        //{
        //    var paramDict = parameters.ToDictionary(x => x.paramName, x => x.valueValue);
        //    var parameterView = ParameterView.FromDictionary(paramDict);
        //    return RenderComponent<TComponent>(parameterView);
        //}

        public IRenderedComponent<TComponent> RenderComponent<TComponent>(params ComponentParameter[] parameters) where TComponent : class, IComponent
        {
            var result = new RenderedComponent<TComponent>(this, parameters);
            return result;
        }

        /// <inheritdoc/>
        public IRenderedComponent<TComponent> RenderComponent<TComponent>(RenderFragment childContent, params ComponentParameter[] parameters) where TComponent : class, IComponent
        {
            var pAndCC = parameters.Concat(new[] { new ComponentParameter("ChildContent", childContent) }).ToArray();
            return RenderComponent<TComponent>(pAndCC);
        }

        /// <inheritdoc/>
        public IRenderedComponent<TComponent> RenderComponent<TComponent>(ParameterView parameters) where TComponent : class, IComponent
        {
            var result = new RenderedComponent<TComponent>(this, parameters);
            return result;
        }

        /// <inheritdoc/>
        public void WaitForNextRender(Action renderTrigger, TimeSpan? timeout = null)
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
