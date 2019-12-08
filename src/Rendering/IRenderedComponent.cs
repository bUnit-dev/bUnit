using Microsoft.AspNetCore.Components;

namespace Egil.RazorComponents.Testing
{
    public interface IRenderedComponent<TComponent> : IRenderedFragment where TComponent : class, IComponent
    {
        TComponent Instance { get; }

        void Render();
        void SetParametersAndRender(ParameterView parameters);
        void SetParametersAndRender(params (string paramName, object valueValue)[] parameters);
    }
}