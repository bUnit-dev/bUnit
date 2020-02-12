using System;
using System.Collections.Generic;
using System.Linq;
using AngleSharp.Diffing.Core;
using Microsoft.AspNetCore.Components;

namespace Bunit
{
    /// <inheritdoc/>
    internal class RenderedComponent<TComponent> : RenderedFragmentBase, IRenderedComponent<TComponent>
        where TComponent : class, IComponent
    {
        /// <inheritdoc/>
        protected override int ComponentId { get; }

        /// <inheritdoc/>
        protected override string FirstRenderMarkup { get; }

        /// <inheritdoc/>
        public TComponent Instance { get; }


        /// <summary>
        /// Instantiates a <see cref="RenderedComponent{TComponent}"/> which will render a component of type <typeparamref name="TComponent"/>
        /// with the provided <paramref name="parameters"/>.
        /// </summary>
        public RenderedComponent(ITestContext testContext, IReadOnlyList<ComponentParameter> parameters)
            : this(testContext, parameters.ToComponentRenderFragment<TComponent>()) { }

        /// <summary>
        /// Instantiates a <see cref="RenderedComponent{TComponent}"/> which will render a component of type <typeparamref name="TComponent"/>
        /// with the provided <paramref name="parameters"/>.
        /// </summary>
        public RenderedComponent(ITestContext testContext, ParameterView parameters)
            : this(testContext, parameters.ToComponentRenderFragment<TComponent>()) { }

        /// <summary>
        /// Instantiates a <see cref="RenderedComponent{TComponent}"/> which will render the <paramref name="renderFragment"/> passed to it 
        /// and attempt to find a component of type <typeparamref name="TComponent"/> in the render result.
        /// </summary>
        public RenderedComponent(ITestContext testContext, RenderFragment renderFragment)
            : base(testContext, renderFragment)
        {
            (ComponentId, Instance) = Container.GetComponent<TComponent>();
            FirstRenderMarkup = Markup;
        }

        /// <inheritdoc/>
        public void Render() => SetParametersAndRender(ParameterView.Empty);

        /// <inheritdoc/>
        public void SetParametersAndRender(params ComponentParameter[] parameters)
        {
            var parameterView = ParameterView.Empty;
            if (parameters.Length > 0)
            {
                var paramDict = new Dictionary<string, object?>(parameters.Length);
                foreach (var param in parameters)
                {
                    if (param.IsCascadingValue)
                        throw new InvalidOperationException($"You cannot provide a new cascading value through the {nameof(SetParametersAndRender)} method.");

                    paramDict.Add(param.Name!, param.Value);
                }
                parameterView = ParameterView.FromDictionary(paramDict);
            }
            SetParametersAndRender(parameterView);
        }

        /// <inheritdoc/>
        public void SetParametersAndRender(ParameterView parameters)
        {
            TestContext.Renderer.DispatchAndAssertNoSynchronousErrors(() =>
            {
                Instance.SetParametersAsync(parameters);
            });

        }
    }
}
