using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

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
            var parameter = details.isCascading
                ? ComponentParameter.CreateCascadingValue(details.name, value)
                : ComponentParameter.CreateParameter(details.name, value);

            AddParameterToList(parameter);
            return this;
        }

        ///// <summary>
        ///// Add a property or field-expression with a cascading value to this builder.
        ///// </summary>
        ///// <typeparam name="TValue">The generic Value type</typeparam>
        ///// <param name="parameterSelector">The parameter selector</param>
        ///// <param name="value">The value</param>
        ///// <returns>A <see cref="ComponentParameterBuilder&lt;TComponent&gt;"/> which can be chained.</returns>
        //public ComponentParameterBuilder<TComponent> AddCascading<TValue>(Expression<Func<TComponent, TValue>> parameterSelector, [DisallowNull] TValue value)
        //{
        //    if (parameterSelector == null)
        //    {
        //        throw new ArgumentNullException(nameof(parameterSelector));
        //    }

        //    if (value == null)
        //    {
        //        throw new ArgumentNullException(nameof(value));
        //    }

        //    AddParameterToList(ComponentParameter.CreateCascadingValue(GetParameterNameFromExpression(parameterSelector), value));
        //    return this;
        //}

        /// <summary>
        /// Add an unnamed cascading parameter with a value to this builder.
        /// </summary>
        /// <param name="value">The value</param>
        /// <returns>A <see cref="ComponentParameterBuilder&lt;TComponent&gt;"/> which can be chained.</returns>
        public ComponentParameterBuilder<TComponent> Add(object value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            AddParameterToList(ComponentParameter.CreateCascadingValue(null, value));
            return this;
        }

        /// <summary>
        /// Add a non generic <see cref="EventCallback"/> parameter with a value for this builder.
        /// </summary>
        /// <param name="parameterSelector">The parameter selector</param>
        /// <param name="callback">The event callback.</param>
        /// <returns>A <see cref="ComponentParameterBuilder&lt;TComponent&gt;"/> which can be chained.</returns>
        public ComponentParameterBuilder<TComponent> Add(Expression<Func<TComponent, EventCallback>> parameterSelector, Func<Task> callback)
        {
            if (parameterSelector == null)
            {
                throw new ArgumentNullException(nameof(parameterSelector));
            }
            if (callback == null)
            {
                throw new ArgumentNullException(nameof(callback));
            }

            return Add(parameterSelector, EventCallback.Factory.Create(this, callback));
        }

        /// <summary>
        /// Add a generic <see cref="EventCallback"/> parameter with a value for this builder.
        /// </summary>
        /// <param name="parameterSelector">The parameter selector</param>
        /// <param name="callback">The event callback.</param>
        /// <returns>A <see cref="ComponentParameterBuilder&lt;TComponent&gt;"/> which can be chained.</returns>
        public ComponentParameterBuilder<TComponent> Add(Expression<Func<TComponent, EventCallback<EventArgs>>> parameterSelector, Func<EventArgs, Task> callback)
        {
            if (parameterSelector == null)
            {
                throw new ArgumentNullException(nameof(parameterSelector));
            }
            if (callback == null)
            {
                throw new ArgumentNullException(nameof(callback));
            }

            return Add(parameterSelector, EventCallback.Factory.Create(this, callback));
        }

        /// <summary>
        /// Create a <see cref="ComponentParameter"/> collection.
        /// </summary>
        /// <returns>A IReadOnlyCollection of <see cref="ComponentParameter"/></returns>
        public IReadOnlyCollection<ComponentParameter> Build()
        {
            return _componentParameters;
        }

        private static (string name, bool isCascading) GetDetailsFromExpression<T>(Expression<Func<TComponent, T>> parameterSelector)
        {
            if (parameterSelector == null)
            {
                throw new ArgumentNullException(nameof(parameterSelector));
            }

            if (parameterSelector.Body is MemberExpression memberExpression && memberExpression.Member is PropertyInfo propertyInfo)
            {
                var parameterAttribute = propertyInfo.GetCustomAttribute<ParameterAttribute>();
                var cascadingParameterAttribute = propertyInfo.GetCustomAttribute<CascadingParameterAttribute>();
                if (parameterAttribute is null && cascadingParameterAttribute is null)
                {
                    throw new ArgumentException($"The property '{propertyInfo.Name}' selected by the provided '{parameterSelector}' does not have the [Parameter] or [CascadingParameter] attribute in the component '{typeof(TComponent)}'.");
                }

                if (parameterAttribute is {})
                {
                    return (propertyInfo.Name, false);
                }

                if (!string.IsNullOrEmpty(cascadingParameterAttribute.Name))
                {
                    return (cascadingParameterAttribute.Name, true);
                }

                return (propertyInfo.Name, true);
            }

            throw new ArgumentException($"The parameterSelector '{parameterSelector}' does not resolve to a property on the class '{typeof(TComponent)}'.");
        }

        private void AddParameterToList(ComponentParameter parameter)
        {
            if (_componentParameters.All(cp => cp.Name != parameter.Name))
            {
                _componentParameters.Add(parameter);
            }
            else
            {
                throw new ArgumentException($"A parameter with the name '{parameter.Name}' has already been added to the {nameof(ComponentParameterBuilder<TComponent>)}.");
            }
        }
    }
}
