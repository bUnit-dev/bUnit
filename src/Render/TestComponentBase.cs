using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngleSharp;
using AngleSharp.Dom;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace Egil.RazorComponents.Testing.Render
{
    public abstract class TestComponentBase : IDisposable
    {
        private readonly ServiceCollection _serviceCollection = new ServiceCollection();
        private readonly Lazy<TestRenderer> _renderer;
        private readonly Lazy<TestBrowsingContext> _browsingContext;

        private TestRenderer Renderer => _renderer.Value;
        private TestBrowsingContext BrowsingContext => _browsingContext.Value;

        public TestComponentBase()
        {
            _renderer = new Lazy<TestRenderer>(() =>
            {
                var serviceProvider = _serviceCollection.BuildServiceProvider();
                var loggerFactory = serviceProvider.GetService<ILoggerFactory>() ?? new NullLoggerFactory();
                return new TestRenderer(serviceProvider, loggerFactory);
            });

            _browsingContext = new Lazy<TestBrowsingContext>(() =>
            {
                return new TestBrowsingContext(_renderer.Value);
            });
        }

        public void AddService<T>(T implementation) => AddService<T, T>(implementation);

        public void AddService<TService, TImplementation>(TImplementation implementation) where TImplementation : TService
        {
            if (_renderer.IsValueCreated)
            {
                throw new InvalidOperationException("Cannot configure services after the host has started operation");
            }

            _serviceCollection.AddSingleton(typeof(TService), implementation);
        }

        public void Dispose()
        {
            if(_browsingContext.IsValueCreated)
                _browsingContext.Value.Dispose();
            if(_renderer.IsValueCreated)
                _renderer.Value.Dispose();
        }

        [Fact]
        public void MyTestMethod()
        {
            var cut = new ContainerComponent(Renderer);
            cut.RenderComponentUnderTest(BuildRenderTree);

            var facts = cut.Find<Fact>();             
            foreach (var fact in facts)
            {
                var html = Htmlizer.GetHtml(Renderer, fact.Id);
                var document = BrowsingContext.OpenAsync(x => x.Content(html)).GetAwaiter().GetResult();
            }
        }

        protected virtual void BuildRenderTree(RenderTreeBuilder builder) { }
    }
}
