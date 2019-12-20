using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Egil.RazorComponents.Testing.Asserting;
using Egil.RazorComponents.Testing.Diffing;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace Egil.RazorComponents.Testing
{
    /// <summary>
    /// Base test class/test runner, that runs Fixtures defined in razor files.
    /// </summary>
    public abstract class TestComponentBase : IDisposable
    {
        private readonly ServiceCollection _serviceCollection = new ServiceCollection();
        private readonly Lazy<TestRenderer> _renderer;
        private bool _isDisposed = false;

        /// <inheritdoc/>
        public TestComponentBase()
        {
            _renderer = new Lazy<TestRenderer>(() =>
            {
                var serviceProvider = _serviceCollection.BuildServiceProvider();
                var loggerFactory = serviceProvider.GetService<ILoggerFactory>() ?? new NullLoggerFactory();
                return new TestRenderer(serviceProvider, loggerFactory);
            });
        }

        /// <summary>
        /// Called by the XUnit test runner. Finds all Fixture components
        /// in the file and runs their associated tests.
        /// </summary>
        [Fact]
        public void RazorTest()
        {
            var container = new ContainerComponent(_renderer.Value);
            container.RenderComponentUnderTest(BuildRenderTree);

            ExecuteFixtureTests(container);
            ExecuteSnapshotTests(container);
        }

        private void ExecuteFixtureTests(ContainerComponent container)
        {
            foreach (var (_, fixture) in container.GetComponents<Fixture>())
            {
                container.RenderComponentUnderTest(fixture.ChildContent);
                var testData = container.GetComponents<FragmentBase>().Select(x => x.Component).ToArray();

                using var context = new RazorTestContext(testData);
                fixture.Setup(context);
                fixture.Test(context);
                foreach (var test in fixture.Tests)
                {
                    test(context);
                }
            }
        }

        private void ExecuteSnapshotTests(ContainerComponent container)
        {
            foreach (var (_, snapshot) in container.GetComponents<SnapshotTest>())
            {
                container.RenderComponentUnderTest(snapshot.ChildContent);
                var testinput = container.GetComponents<FragmentBase>().Select(x => x.Component).ToArray();

                using var context = new SnapshotTestContext(testinput);
                snapshot.Setup(context);
                var actual = context.RenderTestInput();
                var expected = context.RenderExpectedOutput();
                actual.ShouldBe(expected, snapshot.Description);
            }
        }

        /// <inheritdoc/>
        protected virtual void BuildRenderTree(RenderTreeBuilder builder) { }

        #region IDisposable Support
        /// <inheritdoc/>
        protected virtual void Dispose(bool disposing)
        {
            if (_isDisposed) return;
            if (disposing)
            {
                if (_renderer.IsValueCreated) _renderer.Value.Dispose();
            }
            _isDisposed = true;
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
