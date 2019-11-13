using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Egil.RazorComponents.Testing
{
    public abstract class TestingComponentBase : IComponent, ITest, IDisposable
    {
        private readonly Lazy<TestRenderer> _renderer;
        private readonly Lazy<ITestRenderingContext> _renderingContext;
        private readonly ServiceCollection _serviceCollection = new ServiceCollection();

        protected ITestRenderingContext RenderContext => _renderingContext.Value;

        [Parameter] public Action<IServiceCollection>? AddServices { get; set; }

        public TestingComponentBase()
        {
            _renderer = new Lazy<TestRenderer>(() =>
            {
                if(AddServices is { }) AddServices(_serviceCollection);
                var serviceProvider = _serviceCollection.BuildServiceProvider();
                var loggerFactory = serviceProvider.GetService<ILoggerFactory>() ?? new NullLoggerFactory();
                return new TestRenderer(serviceProvider, loggerFactory);
            });
            _renderingContext = new Lazy<ITestRenderingContext>(() => new TestRenderingContext(_renderer.Value));
        }

        public void Attach(RenderHandle renderHandle) { }

        public abstract void ExecuteTest();

        public Task SetParametersAsync(ParameterView parameters)
        {
            parameters.SetParameterProperties(this);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            if(_renderer.IsValueCreated) _renderer.Value.Dispose();
        }
    }
}