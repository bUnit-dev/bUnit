using Microsoft.AspNetCore.Components;

namespace Bunit
{
    /// <summary>
    /// Represents a rendered component-under-test. 
    /// </summary>
    /// <typeparam name="TComponent">The type of the component under test</typeparam>
    public interface IRenderedComponent<out TComponent> : IRenderedFragment where TComponent : IComponent
    {
        /// <summary>
        /// Gets the component under test
        /// </summary>
        TComponent Instance { get; }

        /// <summary>
        /// Render the component under test again.
        /// </summary>
        void Render();

        /// <summary>
        /// Render the component under test again.
        /// </summary>
        /// <param name="parameters">Parameters to pass to the component upon rendered</param>
        void SetParametersAndRender(ParameterView parameters);

        /// <summary>
        /// Render the component under test again.
        /// </summary>
        /// <param name="parameters">Parameters to pass to the component upon rendered</param>
        void SetParametersAndRender(params ComponentParameter[] parameters);
    }
}
