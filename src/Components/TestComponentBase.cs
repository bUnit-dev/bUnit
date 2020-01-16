using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using AngleSharp.Dom;
using Egil.RazorComponents.Testing.Asserting;
using Egil.RazorComponents.Testing.Diffing;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;
using Xunit.Sdk;

namespace Egil.RazorComponents.Testing
{
    /// <summary>
    /// Base test class/test runner, that runs Fixtures defined in razor files.
    /// </summary>
    public abstract class TestComponentBase : ComponentTestFixture, IRazorTestContext
    {
        private readonly ServiceCollection _serviceCollection = new ServiceCollection();
        private readonly Lazy<TestRenderer> _renderer;
        private readonly TestContextAdapter _testContextAdapter = new TestContextAdapter();
        private bool _isDisposed = false;

        /// <inheritdoc/>
        public override TestServiceProvider Services
            => _testContextAdapter.HasActiveContext ? _testContextAdapter.Services : base.Services;

        /// <inheritdoc/>
        public override TestRenderer Renderer
            => _testContextAdapter.HasActiveContext ? _testContextAdapter.Renderer : base.Renderer;

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
        [Fact(DisplayName = "Razor test runner")]
        public async Task RazorTest()
        {
            var container = new ContainerComponent(_renderer.Value);
            container.Render(BuildRenderTree);

            await ExecuteFixtureTests(container).ConfigureAwait(false);
            ExecuteSnapshotTests(container);
        }
        
        /// <inheritdoc/>
        public IRenderedFragment GetComponentUnderTest()
            => _testContextAdapter.GetComponentUnderTest();

        /// <inheritdoc/>
        public IRenderedComponent<TComponent> GetComponentUnderTest<TComponent>() where TComponent : class, IComponent
            => _testContextAdapter.GetComponentUnderTest<TComponent>();

        /// <inheritdoc/>
        public IRenderedFragment GetFragment(string? id = null)
            => _testContextAdapter.GetFragment(id);

        /// <inheritdoc/>
        public IRenderedComponent<TComponent> GetFragment<TComponent>(string? id = null) where TComponent : class, IComponent
            => _testContextAdapter.GetFragment<TComponent>(id);

        /// <inheritdoc/>
        public override INodeList CreateNodes(string markup)
            => _testContextAdapter.HasActiveContext
                ? _testContextAdapter.CreateNodes(markup)
                : base.CreateNodes(markup);

        /// <inheritdoc/>
        public override IRenderedComponent<TComponent> RenderComponent<TComponent>(params ComponentParameter[] parameters)
            => _testContextAdapter.HasActiveContext
                ? _testContextAdapter.RenderComponent<TComponent>(parameters)
                : base.RenderComponent<TComponent>(parameters);

        /// <inheritdoc/>
        public override void WaitForNextRender(Action renderTrigger, TimeSpan? timeout = null)
        {
            if (_testContextAdapter.HasActiveContext)
                _testContextAdapter.WaitForNextRender(renderTrigger, timeout);
            else
                base.WaitForNextRender(renderTrigger, timeout);
        }

        private async Task ExecuteFixtureTests(ContainerComponent container)
        {
            foreach (var (_, fixture) in container.GetComponents<Fixture>())
            {
                container.Render(fixture.ChildContent);
                var testData = container.GetComponents<FragmentBase>().Select(x => x.Component).ToArray();

                _testContextAdapter.ActivateRazorTestContext(testData);
                
                InvokeFixtureAction(fixture, fixture.Setup);
                await InvokeFixtureAction(fixture, fixture.SetupAsync).ConfigureAwait(false);
                InvokeFixtureAction(fixture, fixture.Test);
                await InvokeFixtureAction(fixture, fixture.TestAsync).ConfigureAwait(false);

                foreach (var test in fixture.Tests)
                {
                    InvokeFixtureAction(fixture, test);
                }

                foreach (var test in fixture.TestsAsync)
                {
                    await InvokeFixtureAction(fixture, test).ConfigureAwait(false);
                }

                _testContextAdapter.DisposeActiveTestContext();
            }
        }

        private static void InvokeFixtureAction(Fixture fixture, Action action)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                throw new FixtureFailedException(fixture.Description ?? $"{action.Method.Name} failed:", ex);
            }
        }

        private static async Task InvokeFixtureAction(Fixture fixture, Func<Task> action)
        {
            try
            {
                await action().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw new FixtureFailedException(fixture.Description ?? $"{action.Method.Name} failed:", ex);
            }
        }

        private void ExecuteSnapshotTests(ContainerComponent container)
        {
            foreach (var (_, snapshot) in container.GetComponents<SnapshotTest>())
            {
                container.Render(snapshot.ChildContent);
                var testData = container.GetComponents<FragmentBase>().Select(x => x.Component).ToArray();

                var context = _testContextAdapter.ActivateSnapshotTestContext(testData);
                snapshot.Setup();
                var actual = context.RenderTestInput();
                var expected = context.RenderExpectedOutput();
                actual.MarkupMatches(expected, snapshot.Description);
                _testContextAdapter.DisposeActiveTestContext();
            }
        }

        /// <inheritdoc/>
        protected virtual void BuildRenderTree(RenderTreeBuilder builder) { }

        /// <inheritdoc/>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (_isDisposed) return;

            if (disposing)
            {
                if (_renderer.IsValueCreated)
                    _renderer.Value.Dispose();
                _testContextAdapter.Dispose();
            }
            _isDisposed = true;
        }
    }
}
