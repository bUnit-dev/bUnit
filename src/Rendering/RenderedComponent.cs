using System.Collections.Generic;
using System.Linq;
using AngleSharp.Diffing.Core;
using Microsoft.AspNetCore.Components;

namespace Egil.RazorComponents.Testing
{
    public class RenderedComponent<TComponent> : RenderedFragment, IRenderedFragment
        where TComponent : class, IComponent
    {
        public TComponent Instance { get; }

        internal RenderedComponent(TestHost testContext, ParameterView parameters)
            : this(testContext, TypeBasedRenderTreeBuilder(parameters)) { }

        internal RenderedComponent(TestHost testContext, RenderFragment renderFragment)
            : base(testContext, renderFragment)
        {
            (_, Instance) = Container.GetComponent<TComponent>();
        }

        public void Render() => SetParametersAndRender(ParameterView.Empty);

        public void SetParametersAndRender(params (string paramName, object valueValue)[] parameters)
        {
            if (parameters.Length > 0)
            {
                var paramDict = parameters.ToDictionary(x => x.paramName, x => x.valueValue);
                var parameterView = ParameterView.FromDictionary(paramDict);
                SetParametersAndRender(parameterView);
            }
            else
            {
                SetParametersAndRender(ParameterView.Empty);
            }
        }

        public void SetParametersAndRender(ParameterView parameters)
        {
            TestContext.Renderer.DispatchAndAssertNoSynchronousErrors(() =>
            {
                Instance.SetParametersAsync(parameters);
            });
        }

        private static RenderFragment TypeBasedRenderTreeBuilder(ParameterView parameters)
        {
            return builder =>
            {
                builder.OpenComponent(0, typeof(TComponent));

                foreach (var parameterValue in parameters)
                    builder.AddAttribute(1, parameterValue.Name, parameterValue.Value);

                builder.CloseComponent();
            };
        }
    }
}
