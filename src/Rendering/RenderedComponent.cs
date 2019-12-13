using System;
using System.Collections.Generic;
using System.Linq;
using AngleSharp.Diffing.Core;
using Microsoft.AspNetCore.Components;
using Egil.RazorComponents.Testing.Extensions;

namespace Egil.RazorComponents.Testing
{
    /// <inheritdoc/>
    public class RenderedComponent<TComponent> : RenderedFragment, IRenderedComponent<TComponent> where TComponent : class, IComponent
    {
        /// <inheritdoc/>
        public TComponent Instance { get; }

        internal RenderedComponent(TestContext testContext, IReadOnlyList<ComponentParameter> parameters)
            : this(testContext, parameters.ToComponentRenderFragment<TComponent>()) { }

        internal RenderedComponent(TestContext testContext, ParameterView parameters)
            : this(testContext, parameters.ToComponentRenderFragment<TComponent>()) { }

        internal RenderedComponent(TestContext testContext, RenderFragment renderFragment)
            : base(testContext, renderFragment)
        {
            (ComponentId, Instance) = Container.GetComponent<TComponent>();
        }

        /// <inheritdoc/>
        public void Render() => SetParametersAndRender(ParameterView.Empty);

        /// <inheritdoc/>
        public void SetParametersAndRender(params ComponentParameter[] parameters)
        {
            var parameterView = ParameterView.Empty;
            if (parameters.Length > 0)
            {
                var paramDict = parameters.ToDictionary(x => x.Name, x => x.Value);
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
