using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using EC = Microsoft.AspNetCore.Components.EventCallback;

namespace Bunit
{
    /// <summary>
    /// A builder which makes it possible to add typed ComponentParameters.
    /// </summary>
    /// <typeparam name="TComponent">The type of component to render</typeparam>
    public sealed class ComponentParameterBuilder<TComponent> where TComponent : class, IComponent
    {
        private readonly List<ComponentParameter> _componentParameters = new List<ComponentParameter>();

        /// <summary>
        /// Add a property selector with a value to this builder.
        /// </summary>
        /// <typeparam name="TValue">The generic Value type</typeparam>
        /// <param name="parameterSelector">The parameter selector</param>
        /// <param name="value">The value, which cannot be null in case of cascading parameter</param>
        /// <returns>A <see cref="ComponentParameterBuilder&lt;TComponent&gt;"/> which can be chained.</returns>
        public ComponentParameterBuilder<TComponent> Add<TValue>(Expression<Func<TComponent, TValue>> parameterSelector, [AllowNull] TValue value)
        {
            if (parameterSelector is null)
            {
                throw new ArgumentNullException(nameof(parameterSelector));
            }

            var details = GetDetailsFromExpression(parameterSelector);
            return AddParameterToList(details.name, value, details.isCascading);
        }

        /// <summary>
        /// Add a non generic <see cref="EventCallback"/> parameter with a value for this builder.
        /// </summary>
        /// <param name="parameterSelector">The parameter selector</param>
        /// <param name="callback">The event callback.</param>
        /// <returns>A <see cref="ComponentParameterBuilder&lt;TComponent&gt;"/> which can be chained.</returns>
        public ComponentParameterBuilder<TComponent> Add(Expression<Func<TComponent, EC>> parameterSelector, Func<Task> callback)
        {
            if (parameterSelector is null)
            {
                throw new ArgumentNullException(nameof(parameterSelector));
            }
            if (callback is null)
            {
                throw new ArgumentNullException(nameof(callback));
            }

            return Add(parameterSelector, EC.Factory.Create(this, callback));
        }

        /// <summary>
        /// Add a generic <see cref="EventCallback"/> parameter with a value for this builder.
        /// </summary>
        /// <param name="parameterSelector">The parameter selector</param>
        /// <param name="callback">The event callback.</param>
        /// <returns>A <see cref="ComponentParameterBuilder&lt;TComponent&gt;"/> which can be chained.</returns>
        public ComponentParameterBuilder<TComponent> Add<TValue>(Expression<Func<TComponent, EventCallback<TValue>>> parameterSelector, Func<TValue, Task> callback)
        {
            if (parameterSelector is null)
            {
                throw new ArgumentNullException(nameof(parameterSelector));
            }
            if (callback is null)
            {
                throw new ArgumentNullException(nameof(callback));
            }

            var details = GetDetailsFromExpression(parameterSelector);
            return AddParameterToList(details.name, EC.Factory.Create(this, callback), details.isCascading);
        }

        /// <summary>
        /// Create a <see cref="ComponentParameter"/> collection.
        /// </summary>
        /// <returns>A IReadOnlyCollection of <see cref="ComponentParameter"/></returns>
        public IReadOnlyCollection<ComponentParameter> Build()
        {
            return _componentParameters;
        }

        private static (string name, bool isCascading) GetDetailsFromExpression<T>([NotNull] Expression<Func<TComponent, T>> parameterSelector)
        {
            if (parameterSelector.Body is MemberExpression memberExpression && memberExpression.Member is PropertyInfo propertyInfo)
            {
                var parameterAttribute = propertyInfo.GetCustomAttribute<ParameterAttribute>();
                var cascadingParameterAttribute = propertyInfo.GetCustomAttribute<CascadingParameterAttribute>();

                // If both attributes are missing -> throw exception
                if (parameterAttribute is null && cascadingParameterAttribute is null)
                {
                    throw new ArgumentException($"The property '{propertyInfo.Name}' selected by the provided '{parameterSelector}' does not have the [Parameter] or [CascadingParameter] attribute defined in the component '{typeof(TComponent)}'.");
                }

                // If ParameterAttribute is defined, get the name from the property and indicate that it's a normal property
                if (parameterAttribute is { })
                {
                    return (propertyInfo.Name, false);
                }

                if (!string.IsNullOrEmpty(cascadingParameterAttribute.Name))
                {
                    // The CascadingParameterAttribute is defined, get the defined name from this attribute and indicate that it's a cascading property
                    return (cascadingParameterAttribute.Name, true);
                }

                // The CascadingParameterAttribute is defined, get the name from the property and indicate that it's a cascading property
                return (propertyInfo.Name, true);
            }

            throw new ArgumentException($"The parameterSelector '{parameterSelector}' does not resolve to a property on the component '{typeof(TComponent)}'.");
        }

        private ComponentParameterBuilder<TComponent> AddParameterToList(string? name, object? value, bool isCascading)
        {
            if (_componentParameters.All(cp => cp.Name != name))
            {
                var parameter = isCascading ?
                    ComponentParameter.CreateCascadingValue(name, value) :
                    ComponentParameter.CreateParameter(name, value);
                _componentParameters.Add(parameter);

                return this;
            }

            throw new ArgumentException($"A parameter with the name '{name}' has already been added to the {nameof(ComponentParameterBuilder<TComponent>)}.");
        }
    }
}
