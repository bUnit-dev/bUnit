using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Egil.RazorComponents.Testing.Diffing;
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
        private bool _isDisposed = false;

        public TestComponentBase()
        {
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
            var container = new ContainerComponent(_renderer.Value);
            container.RenderComponentUnderTest(BuildRenderTree);

            foreach (var (_, fixture) in container.GetComponents<Fixture>())
            {
                container.RenderComponentUnderTest(fixture.ChildContent);
                var testData = container.GetComponents<FragmentBase>().Select(x => x.Component).ToArray();

                using var context = new TestContext(testData);
                fixture.Setup(context);
                fixture.Test(context);
                foreach (var test in fixture.Tests)
                {
                    test(context);
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
