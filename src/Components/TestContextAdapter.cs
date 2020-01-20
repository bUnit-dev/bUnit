using System;
using System.Collections.Generic;
using AngleSharp.Dom;
using Egil.RazorComponents.Testing.Diffing;
using Microsoft.AspNetCore.Components;

namespace Egil.RazorComponents.Testing
{
    internal sealed class TestContextAdapter : IDisposable
    {
        private ITestContext? _testContext;
        private IRazorTestContext? _razorTestContext;

        public TestServiceProvider Services => _testContext?.Services ?? throw new InvalidOperationException("No active test context in the adapter");

        public TestRenderer Renderer => _testContext?.Renderer ?? throw new InvalidOperationException("No active test context in the adapter");

        public bool HasActiveContext => !(_testContext is null);

        public SnapshotTestContext ActivateSnapshotTestContext(IReadOnlyList<FragmentBase> testData)
        {
            var result = new SnapshotTestContext(testData);
            _razorTestContext = null;
            _testContext?.Dispose();
            _testContext = result;
            return result;
        }

        public RazorTestContext ActivateRazorTestContext(IReadOnlyList<FragmentBase> testData)
        {
            var result = new RazorTestContext(testData);
            _testContext?.Dispose();
            _testContext = result;
            _razorTestContext = result;
            return result;
        }

        public void DisposeActiveTestContext() => Dispose();

        public void Dispose()
        {
            _testContext?.Dispose();
            _razorTestContext?.Dispose();
            _testContext = null;
            _razorTestContext = null;
        }

        public IRenderedFragment GetComponentUnderTest()
            => _razorTestContext?.GetComponentUnderTest() ?? throw new InvalidOperationException($"{nameof(GetComponentUnderTest)} is only available in Razor based tests.");

        public IRenderedComponent<TComponent> GetComponentUnderTest<TComponent>() where TComponent : class, IComponent
            => _razorTestContext?.GetComponentUnderTest<TComponent>() ?? throw new InvalidOperationException($"{nameof(GetComponentUnderTest)} is only available in Razor based tests.");

        public IRenderedFragment GetFragment(string? id = null)
            => _razorTestContext?.GetFragment(id) ?? throw new InvalidOperationException($"{nameof(GetFragment)} is only available in Razor based tests.");

        public IRenderedComponent<TComponent> GetFragment<TComponent>(string? id) where TComponent : class, IComponent
            => _razorTestContext?.GetFragment<TComponent>(id) ?? throw new InvalidOperationException($"{nameof(GetFragment)} is only available in Razor based tests.");

        public void WaitForNextRender(Action renderTrigger, TimeSpan? timeout = null)
        {
            if (_testContext is null)
                throw new InvalidOperationException("No active test context in the adapter");
            else
                _testContext.WaitForNextRender(renderTrigger, timeout);
        }

        public IRenderedComponent<TComponent> RenderComponent<TComponent>(params ComponentParameter[] parameters) where TComponent : class, IComponent
            => _testContext?.RenderComponent<TComponent>(parameters) ?? throw new InvalidOperationException("No active test context in the adapter");

        public INodeList CreateNodes(string markup)
            => _testContext?.CreateNodes(markup) ?? throw new InvalidOperationException("No active test context in the adapter");
    }    
}
