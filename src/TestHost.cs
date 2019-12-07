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
    public class TestHost : IDisposable
    {
        private readonly Lazy<TestRenderer> _renderer;
        private readonly Lazy<HtmlParser> _htmlParser;

        public TestRenderer Renderer => _renderer.Value;

        public HtmlParser HtmlParser => _htmlParser.Value;

        public TestServiceProvider Services { get; } = new TestServiceProvider();

        public TestHost()
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

        public void WaitForNextRender(Action trigger)
        {
            if (trigger is null) throw new ArgumentNullException(nameof(trigger));
            var task = Renderer.NextRender;
            trigger();
            task.Wait(millisecondsTimeout: 1000);

            if (!task.IsCompleted)
            {
                throw new TimeoutException("No render occurred within the timeout period.");
            }
        }

        public RenderedComponent<TComponent> RenderComponent<TComponent>() where TComponent : class, IComponent
        {
            return RenderComponent<TComponent>(ParameterView.Empty);
        }

        public RenderedComponent<TComponent> RenderComponent<TComponent>(params (string paramName, object? valueValue)[] parameters) where TComponent : class, IComponent
        {
            var paramDict = parameters.ToDictionary(x => x.paramName, x => x.valueValue);
            var parameterView = ParameterView.FromDictionary(paramDict);
            return RenderComponent<TComponent>(parameterView);
        }

        public RenderedComponent<TComponent> RenderComponent<TComponent>(RenderFragment childContent, params (string paramName, object? valueValue)[] parameters) where TComponent : class, IComponent
        {
            var paramDict = parameters.ToDictionary(x => x.paramName, x => x.valueValue);
            paramDict.Add("ChildContent", childContent);
            var parameterView = ParameterView.FromDictionary(paramDict);
            return RenderComponent<TComponent>(parameterView);
        }

        public RenderedComponent<TComponent> RenderComponent<TComponent>(ParameterView parameters) where TComponent : class, IComponent
        {
            var result = new RenderedComponent<TComponent>(this, parameters);
            return result;
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

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
