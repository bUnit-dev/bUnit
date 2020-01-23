using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.RenderTree;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Egil.RazorComponents.Testing.Extensions;
using AngleSharp.Css.Dom;

namespace Egil.RazorComponents.Testing
{
    /// <summary>
    /// Represents a Razor component that can be used to render and re-render a render fragment into.
    /// </summary>
    [SuppressMessage("Usage", "BL0006:Do not use RenderTree types", Justification = "<Pending>")]
    public class ContainerComponent : IComponent
    {
        private readonly TestRenderer _renderer;
        private RenderHandle _renderHandle;

        /// <summary>
        /// Gets the id of the <see cref="ContainerComponent"/> after it has been rendered the first time.
        /// </summary>
        public int ComponentId { get; }

        /// <summary>
        /// Creates a new instance of the <see cref="ContainerComponent"/> class.
        /// </summary>
        /// <param name="renderer"></param>
        public ContainerComponent(TestRenderer renderer)
        {
            if (renderer is null) throw new ArgumentNullException(nameof(renderer));
            _renderer = renderer;
            ComponentId = _renderer.AttachTestRootComponent(this);
        }

        /// <inheritdoc/>
        public void Attach(RenderHandle renderHandle) => _renderHandle = renderHandle;

        /// <inheritdoc/>
        public Task SetParametersAsync(ParameterView parameters) => throw new InvalidOperationException($"{nameof(ContainerComponent)} shouldn't receive any parameters");

        /// <summary>
        /// Renders a <see cref="RenderFragment"/> inside the <see cref="ContainerComponent"/>.
        /// </summary>
        /// <param name="renderFragment">The render fragment to render.</param>
        public void Render(RenderFragment renderFragment)
        {
            _renderer.DispatchAndAssertNoSynchronousErrors(() =>
            {
                _renderHandle.Render(renderFragment);
            });
        }

        /// <summary>
        /// Gets the first component of type <typeparamref name="TComponent"/>. If an <see cref="CascadingValue{TValue}"/>
        /// component is found, its child content is also searched recursively.
        /// </summary>
        /// <typeparam name="TComponent">The type of component to find</typeparam>
        /// <exception cref="InvalidOperationException">When a component of type <typeparamref name="TComponent"/> was not found.</exception>
        public (int Id, TComponent Component) GetComponent<TComponent>() where TComponent : IComponent
        {
            var result = GetComponent<TComponent>(ComponentId);
            if (result.HasValue)
                return result.Value;
            else
                throw new InvalidOperationException($"No components of type {typeof(TComponent)} were found in the render tree.");
        }

        /// <summary>
        /// Gets all components of type <typeparamref name="TComponent"/>. If an <see cref="CascadingValue{TValue}"/>
        /// component is found, its child content is also searched recursively.
        /// </summary>
        /// <typeparam name="TComponent">The type of component to find</typeparam>
        public IReadOnlyList<(int Id, TComponent Component)> GetComponents<TComponent>() where TComponent : IComponent
            => GetComponents<TComponent>(ComponentId);

        private IReadOnlyList<(int Id, TComponent Component)> GetComponents<TComponent>(int componentId) where TComponent : IComponent
        {
            var ownFrames = _renderer.GetCurrentRenderTreeFrames(componentId);
            if (ownFrames.Count == 0)
            {
                return Array.Empty<(int Id, TComponent Component)>();
            }

            var result = new List<(int Id, TComponent Component)>();
            for (int i = 0; i < ownFrames.Count; i++)
            {
                ref var frame = ref ownFrames.Array[i];
                if (frame.FrameType == RenderTreeFrameType.Component)
                {
                    if (frame.Component is TComponent component)
                    {
                        result.Add((frame.ComponentId, component));
                    }
                    result.AddRange(GetComponents<TComponent>(frame.ComponentId));
                }
            }

            return result;
        }

        private (int Id, TComponent Component)? GetComponent<TComponent>(int componentId) where TComponent : IComponent
        {
            var ownFrames = _renderer.GetCurrentRenderTreeFrames(componentId);

            for (int i = 0; i < ownFrames.Count; i++)
            {
                ref var frame = ref ownFrames.Array[i];
                if (frame.FrameType == RenderTreeFrameType.Component)
                {
                    if (frame.Component is TComponent component)
                    {
                        return (frame.ComponentId, component);
                    }
                    var result = GetComponent<TComponent>(frame.ComponentId);
                    if (result != null) return result;
                }
            }

            return null;
        }
    }
}
