using System;
using System.Collections.Generic;
using System.Linq;
using AngleSharp.Diffing.Core;
using Microsoft.AspNetCore.Components;

namespace Egil.RazorComponents.Testing
{
    public class RenderedComponent<TComponent> : RenderedFragment, IRenderedComponent<TComponent> where TComponent : class, IComponent
    {
        public TComponent Instance { get; }

        internal RenderedComponent(TestContext testContext, IReadOnlyList<ComponentParameter> parameters)
            : this(testContext, TypeBasedRenderTreeBuilder(parameters)) { }

        internal RenderedComponent(TestContext testContext, ParameterView parameters)
            : this(testContext, TypeBasedRenderTreeBuilder(parameters)) { }

        internal RenderedComponent(TestContext testContext, RenderFragment renderFragment)
            : base(testContext, renderFragment)
        {
            (ComponentId, Instance) = Container.GetComponent<TComponent>();
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

        private static RenderFragment TypeBasedRenderTreeBuilder(IReadOnlyList<ComponentParameter> parameters)
        {
            var cascadingParams = new Queue<ComponentParameter>(parameters.Where(x => x.IsCascadingValue && x.Value is { }));

            if (cascadingParams.Count > 0)
                return GetCascadingValueBuilder(cascadingParams, parameters);
            else
                return GetComponentBuilder(parameters);

            static RenderFragment GetCascadingValueBuilder(Queue<ComponentParameter> cascadingParams, IReadOnlyList<ComponentParameter> parameters)
            {
                var cp = cascadingParams.Dequeue();
                // BANG: Value should not be null at this point since any cascading params 
                // have been filtered out when the queue was build
                var value = cp.Value!;
                var cascadingValueType = GetCascadingValueType(value);
                return builder =>
                {
                    builder.OpenComponent(0, cascadingValueType);
                    //builder.AddAttribute(1, nameof(CascadingValue<object>.Name), cp.Name);
                    builder.AddAttribute(2, nameof(CascadingValue<object>.Value), value);
                    builder.AddAttribute(3, nameof(CascadingValue<object>.IsFixed), true);
                    if (cascadingParams.Count > 0)
                        builder.AddAttribute(4, nameof(CascadingValue<object>.ChildContent), GetCascadingValueBuilder(cascadingParams, parameters));
                    else
                        builder.AddAttribute(4, nameof(CascadingValue<object>.ChildContent), GetComponentBuilder(parameters));
                    builder.CloseComponent();
                };
            }

            static RenderFragment GetComponentBuilder(IReadOnlyList<ComponentParameter> parameters)
            {
                return builder =>
                {
                    builder.OpenComponent(0, typeof(TComponent));

                    foreach (var parameterValue in parameters.Where(x => !x.IsCascadingValue))
                        builder.AddAttribute(1, parameterValue.Name, parameterValue.Value);

                    builder.CloseComponent();
                };
            }
        }

        private static readonly Type CascadingValueType = typeof(CascadingValue<>);
        private static Type GetCascadingValueType(object cascadingValue)
        {
            var cascadingValueType = cascadingValue.GetType();
            return CascadingValueType.MakeGenericType(cascadingValueType);
        }
    }
}
