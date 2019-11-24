using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.RenderTree;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
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
    public class ContainerComponent : IComponent
    {
        private readonly TestRenderer _renderer;
        private RenderHandle _renderHandle;

        public int ComponentId { get; private set; }

        public ContainerComponent(TestRenderer renderer)
        {
            if (renderer is null) throw new ArgumentNullException(nameof(renderer));
            _renderer = renderer;
            ComponentId = _renderer.AttachTestRootComponent(this);
        }

        public void Attach(RenderHandle renderHandle)
        {
            _renderHandle = renderHandle;
        }

        public Task SetParametersAsync(ParameterView parameters)
        {
            throw new InvalidOperationException($"{nameof(ContainerComponent)} shouldn't receive any parameters");
        }

        public (int Id, T Component) GetComponent<T>() => GetComponents<T>().First();

        public IEnumerable<(int Id, T Component)> GetComponents<T>()
        {
            var ownFrames = _renderer.GetCurrentRenderTreeFrames(ComponentId);
            if (ownFrames.Count == 0)
            {
                throw new InvalidOperationException($"{nameof(ContainerComponent)} hasn't yet rendered");
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

        public void RenderComponentUnderTest(RenderFragment renderFragment)
        {
            _renderer.DispatchAndAssertNoSynchronousErrors(() =>
            {
                _renderHandle.Render(renderFragment);
            });
        }
    }
}
