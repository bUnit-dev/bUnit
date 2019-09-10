using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.RenderTree;
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace Egil.RazorComponents.Testing.Render
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
    internal class ContainerComponent : IComponent
    {
        private readonly TestRenderer _renderer;
        private readonly int _componentId;
        private RenderHandle _renderHandle;

        public ContainerComponent(TestRenderer renderer)
        {
            _renderer = renderer;
            _componentId = renderer.AttachTestRootComponent(this);
        }

        public void Attach(RenderHandle renderHandle)
        {
            _renderHandle = renderHandle;
        }

        public Task SetParametersAsync(ParameterView parameters)
        {
            throw new NotImplementedException($"{nameof(ContainerComponent)} shouldn't receive any parameters");
        }

        public (int Id, TComponent Component) FindComponentUnderTest<TComponent>() where TComponent : IComponent
        {
            var ownFrames = _renderer.GetCurrentRenderTreeFrames(_componentId);
            if (ownFrames.Count == 0)
            {
                throw new InvalidOperationException($"{nameof(ContainerComponent)} hasn't yet rendered");
            }

            ref var childComponentFrame = ref ownFrames.Array[0];
            if (childComponentFrame.FrameType == RenderTreeFrameType.Component && childComponentFrame.Component is TComponent component)
            {
                return (childComponentFrame.ComponentId, component);
            }
            else throw new Exception("Component not found");
        }

        public void RenderComponentUnderTest(Type componentType, ParameterView parameters)
        {
            _renderer.DispatchAndAssertNoSynchronousErrors(() =>
            {
                _renderHandle.Render(builder =>
                {
                    builder.OpenComponent(0, componentType);

                    foreach (var parameterValue in parameters)
                    {
                        builder.AddAttribute(1, parameterValue.Name, parameterValue.Value);
                    }

                    builder.CloseComponent();
                });
            });
        }
    }
}
