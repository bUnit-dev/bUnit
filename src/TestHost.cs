using Egil.RazorComponents.Testing.Diffing;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.RenderTree;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace Egil.RazorComponents.Testing
{
    public class TestHost
    {
        private readonly ServiceCollection _serviceCollection = new ServiceCollection();
        private readonly Lazy<TestRenderer> _renderer;
        private readonly Lazy<HtmlParser> _htmlParser;

        public TestHost()
        {
            _renderer = new Lazy<TestRenderer>(() =>
            {
                var serviceProvider = _serviceCollection.BuildServiceProvider();
                var loggerFactory = serviceProvider.GetService<ILoggerFactory>() ?? new NullLoggerFactory();
                return new TestRenderer(serviceProvider, loggerFactory);
            });
            _htmlParser = new Lazy<HtmlParser>(() => new HtmlParser(_renderer.Value, new HtmlComparer()));
        }

        public void AddService<T>(T implementation) => AddService<T, T>(implementation);

        public void AddService<TContract, TImplementation>(TImplementation implementation) where TImplementation : TContract
        {
            if (_renderer.IsValueCreated)
            {
                throw new InvalidOperationException("Cannot configure services after the host has started operation.");
            }

            _serviceCollection.AddSingleton(typeof(TContract), implementation);
        }

        public void WaitForNextRender(Action trigger)
        {
            if(trigger is null) throw new ArgumentNullException(nameof(trigger));
            var task = Renderer.NextRender;
            trigger();
            task.Wait(millisecondsTimeout: 1000);

            if (!task.IsCompleted)
            {
                throw new TimeoutException("No render occurred within the timeout period.");
            }
        }

        public RenderedComponent<TComponent> AddComponent<TComponent>() where TComponent : class, IComponent
        {
            var result = new RenderedComponent<TComponent>(Renderer, _htmlParser.Value);
            result.SetParametersAndRender(ParameterView.Empty);
            return result;
        }

        private TestRenderer Renderer => _renderer.Value;
    }
}
