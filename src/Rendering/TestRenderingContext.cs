using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.RenderTree;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace Egil.RazorComponents.Testing
{
    // This provides the ability for test code to trigger rendering at arbitrary times,
    // and to supply arbitrary parameters to the component being tested (including ones
    // flagged as 'cascading').
    //
    // This also avoids the use of Renderer's RenderRootComponentAsync APIs, which are
    // not a good entrypoint for unit tests, because their asynchrony is all about waiting
    // for quiescence. We don't want that in tests because we want to assert about all
    // possible states, including loading states.
    [SuppressMessage("Usage", "BL0006:Do not use RenderTree types", Justification = "<Pending>")]
    public class TestRenderingContext : IComponent, ITestRenderingContext
    {
        private readonly int _componentId;
        private RenderHandle _renderHandle;

        internal TestRenderer Renderer { get; }

        public TestRenderingContext(TestRenderer renderer)
        {
            if (renderer is null) throw new ArgumentNullException(nameof(renderer));

            Renderer = renderer;
            _componentId = renderer.AttachTestRootComponent(this);
        }

        public void Attach(RenderHandle renderHandle)
        {
            _renderHandle = renderHandle;
        }

        public Task SetParametersAsync(ParameterView parameters)
        {
            throw new NotImplementedException($"{nameof(TestRenderingContext)} shouldn't receive any parameters");
        }

        public void RenderComponentUnderTest(RenderFragment renderFragment)
        {
            Renderer.DispatchAndAssertNoSynchronousErrors(() =>
            {
                _renderHandle.Render(renderFragment);
            });
        }

        public List<(int Id, IComponent Component)> GetComponents() => GetComponents<IComponent>();

        public List<(int Id, T Component)> GetComponents<T>()
        {
            var ownFrames = Renderer.GetCurrentRenderTreeFrames(_componentId);
            if (ownFrames.Count == 0)
            {
                throw new InvalidOperationException($"{nameof(TestRenderingContext)} hasn't yet rendered");
            }

            var result = new List<(int Id, T Component)>();
            for (int i = 0; i < ownFrames.Count; i++)
            {
                ref var frame = ref ownFrames.Array[i];
                if (frame.FrameType == RenderTreeFrameType.Component &&
                    frame.Component != null &&
                    frame.Component is T component)
                {
                    result.Add((frame.ComponentId, component));
                }
            }
            return result;
        }

        public string GetHtml(int componentId)
        {
            return Htmlizer.GetHtml(Renderer, componentId);
        }

        public void WaitForNextRender(Action trigger)
        {
            var task = Renderer.NextRender;
            if (!(trigger is null)) trigger();
            task.Wait(millisecondsTimeout: 1000);

            if (!task.IsCompleted)
            {
                throw new TimeoutException("No render occurred within the timeout period.");
            }
        }

        public void DispatchAndAssertNoSynchronousErrors(Action dispatchAction)
        {
            Renderer.DispatchAndAssertNoSynchronousErrors(dispatchAction);
        }
    }
}
