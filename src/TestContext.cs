using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Components;

namespace Egil.RazorComponents.Testing
{
    public class TestContext<TComponent> where TComponent : IComponent
    {
        private ITestRenderingContext RenderContext { get; }

        public TComponent Component { get; }

        public int Id { get; }

        public TestContext(int id, TComponent component, ITestRenderingContext renderContext)
        {
            Id = id;
            Component = component;
            RenderContext = renderContext;
        }

        public void Render() => SetParametersAndRender(ParameterView.Empty);

        public void SetParametersAndRender(params (string ParameterName, object ParameterValue)[] parameters)
        {
            var parameterView = parameters.Length > 0
                ? ParameterView.FromDictionary(parameters.ToDictionary(x => x.ParameterName, x => x.ParameterValue))
                : ParameterView.Empty;
            SetParametersAndRender(parameterView);
        }

        public void SetParametersAndRender(ParameterView parameterView)
        {
            RenderContext.DispatchAndAssertNoSynchronousErrors(() => Component.SetParametersAsync(parameterView));
        }

        public string GetHtml()
        {
            return RenderContext.GetHtml(Id);
        }

        public void WaitForNextRender(Action trigger)
        {
            RenderContext.WaitForNextRender(trigger);
        }
    }
}