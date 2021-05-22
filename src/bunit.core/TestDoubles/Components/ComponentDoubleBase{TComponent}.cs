#if NET5_0_OR_GREATER
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Bunit.TestDoubles
{
    /// <summary>
    /// Represents a
    /// </summary>
    /// <typeparam name="TComponent"></typeparam>
    public abstract class ComponentDoubleBase<TComponent> : IComponent
        where TComponent : IComponent
    {
        private RenderHandle renderHandle;

        /// <summary>
        /// The type of the doubled component.
        /// </summary>
        protected static readonly Type DoubledType = typeof(TComponent);

        /// <summary>
        /// Gets the parameters that was passed to the <typeparamref name="TComponent"/>
        /// that this stub replaced in the component tree.
        /// </summary>
        [Parameter(CaptureUnmatchedValues = true)]
        public IReadOnlyDictionary<string, object> Parameters { get; private set; } = ImmutableDictionary<string, object>.Empty;

        /// <summary>
        /// Gets the value of a parameter passed to the captured <typeparamref name="TComponent"/>,
        /// using the <paramref name="parameterSelector"/>.
        /// </summary>
        /// <typeparam name="TValue">The type of the parameter to find.</typeparam>
        /// <param name="parameterSelector">A parameter selector that selects the parameter property of <typeparamref name="TComponent"/>.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameterSelector"/> is null.</exception>
        /// <exception cref="ArgumentException">Thrown when the member of <typeparamref name="TComponent"/> selected by the <paramref name="parameterSelector"/> is not a Blazor parameter.</exception>
        /// <exception cref="ParameterNotFoundException">Thrown when the selected parameter was not passed to the captured <typeparamref name="TComponent"/>.</exception>
        /// <exception cref="InvalidCastException">Throw when the type of the value passed to the selected parameter is not the same as the selected parameters type, i.e. <typeparamref name="TValue"/>.</exception>
        /// <returns>The <typeparamref name="TValue"/>.</returns>
        public TValue GetParameter<TValue>(Expression<Func<TComponent, TValue>> parameterSelector)
        {
            if (parameterSelector is null)
                throw new ArgumentNullException(nameof(parameterSelector));

            if (!(parameterSelector.Body is MemberExpression memberExpression) || !(memberExpression.Member is PropertyInfo propInfoCandidate))
                throw new ArgumentException($"The parameter selector '{parameterSelector}' does not resolve to a public property on the component '{typeof(TComponent)}'.", nameof(parameterSelector));

            var propertyInfo = propInfoCandidate.DeclaringType != DoubledType
                ? DoubledType.GetProperty(propInfoCandidate.Name, propInfoCandidate.PropertyType)
                : propInfoCandidate;

            var paramAttr = propertyInfo?.GetCustomAttribute<ParameterAttribute>(inherit: true);
            var cascadingParamAttr = propertyInfo?.GetCustomAttribute<CascadingParameterAttribute>(inherit: true);

            if (propertyInfo is null || (paramAttr is null && cascadingParamAttr is null))
                throw new ArgumentException($"The parameter selector '{parameterSelector}' does not resolve to a public property on the component '{typeof(TComponent)}' with a [Parameter] or [CascadingParameter] attribute.", nameof(parameterSelector));

            if (!Parameters.TryGetValue(propertyInfo.Name, out var objectResult))
                throw new ParameterNotFoundException(propertyInfo.Name, DoubledType.ToString());

            return (TValue)objectResult;
        }

        /// <inheritdoc/>
        public virtual Task SetParametersAsync(ParameterView parameters)
        {
            Parameters = parameters.ToDictionary();
            renderHandle.Render(BuildRenderTree);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Override to generate a DOM tree from the doubled component.
        /// </summary>
        /// <param name="builder">A <see cref="RenderTreeBuilder"/> to build DOM tree.</param>
        protected virtual void BuildRenderTree(RenderTreeBuilder builder) { }

        /// <inheritdoc/>
        [SuppressMessage("Design", "CA1033:Interface methods should be callable by child types", Justification = "The IComponent.Attach method are only used by the Blazor renderer, and should not be called from other places.")]
        void IComponent.Attach(RenderHandle renderHandle) => this.renderHandle = renderHandle;
    }
}
#endif