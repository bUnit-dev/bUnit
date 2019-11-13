using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngleSharp.Html.Dom;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace Egil.RazorComponents.Testing
{
    public abstract class TestComponentBase : IDisposable
    {
        private readonly ServiceCollection _serviceCollection = new ServiceCollection();
        private readonly Lazy<TestRenderer> _renderer;
        private readonly Lazy<IHtmlComparer> _htmlComparer;
        private bool _isDisposed = false;

        public TestComponentBase()
        {
            _htmlComparer = new Lazy<IHtmlComparer>(() => new HtmlComparer());
            _serviceCollection.AddSingleton(_ => _htmlComparer.Value);
            _renderer = new Lazy<TestRenderer>(() =>
            {
                var serviceProvider = _serviceCollection.BuildServiceProvider();
                var loggerFactory = serviceProvider.GetService<ILoggerFactory>() ?? new NullLoggerFactory();
                return new TestRenderer(serviceProvider, loggerFactory);
            });
        }

        [Fact]
        public void ComponentTest()
        {
            var renderingContext = new TestRenderingContext(_renderer.Value);
            renderingContext.RenderComponentUnderTest(BuildRenderTree);

            foreach (var (id, component) in renderingContext.GetComponents())
            {
                if (component is ITest test)
                {
                    test.ExecuteTest();
                }
            }
        }

        protected virtual void BuildRenderTree(RenderTreeBuilder builder) { }

        #region IDisposable Support
        protected virtual void Dispose(bool disposing)
        {
            if (_isDisposed) return;
            if (disposing)
            {
                if (_renderer.IsValueCreated) _renderer.Value.Dispose();
                if(_htmlComparer.IsValueCreated) _htmlComparer.Value.Dispose();
            }
            _isDisposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
