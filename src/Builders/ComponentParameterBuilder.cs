using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
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
        private readonly ICollection<ComponentParameter> _componentParameters = new List<ComponentParameter>();

        /// <summary>
        /// Add a property or field-expression with normal value to this builder.
        /// </summary>
        /// <typeparam name="TValue">The generic Value type</typeparam>
        /// <param name="parameterSelector">The property or field parameter selector</param>
        /// <param name="value">The value</param>
        /// <returns>A <see cref="ComponentParameterBuilder&lt;TComponent&gt;"/> which can be chained.</returns>
        public ComponentParameterBuilder<TComponent> Add<TValue>(Expression<Func<TComponent, TValue>> parameterSelector, [AllowNull] TValue value)
        {
            if (parameterSelector == null)
            {
                throw new ArgumentNullException(nameof(parameterSelector));
            }

            AddParameterToList(ComponentParameter<TComponent, TValue>.CreateParameter(parameterSelector, value));

            //AddParameterToList(ComponentParameter.CreateParameter(GetParameterNameFromMethodExpression(expression), value));
            return this;
        }

        /// <summary>
        /// Add a property or field-expression with a cascading value to this builder.
        /// </summary>
        /// <typeparam name="TValue">The generic Value type</typeparam>
        /// <param name="parameterSelector">The property or field parameter selector</param>
        /// <param name="value">The value</param>
        /// <returns>A <see cref="ComponentParameterBuilder&lt;TComponent&gt;"/> which can be chained.</returns>
        public ComponentParameterBuilder<TComponent> AddCascading<TValue>(Expression<Func<TComponent, TValue>> parameterSelector, [DisallowNull] TValue value)
        {
            if (parameterSelector == null)
            {
                throw new ArgumentNullException(nameof(parameterSelector));
            }

            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            AddParameterToList(ComponentParameter<TComponent, TValue>.CreateCascadingValue(parameterSelector, value));

            // AddParameterToList(ComponentParameter.CreateCascadingValue(GetParameterNameFromMethodExpression(expression), value));
            return this;
        }

        /// <summary>
        /// Add an unnamed cascading parameter with a value to this builder.
        /// </summary>
        /// <param name="value">The value</param>
        /// <returns>A <see cref="ComponentParameterBuilder&lt;TComponent&gt;"/> which can be chained.</returns>
        public ComponentParameterBuilder<TComponent> AddCascading(object value)
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
        /// <param name="parameterSelector">The property or field parameter selector</param>
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

            AddParameterToList(ComponentParameter<TComponent, EventCallback>.CreateParameter(parameterSelector, EventCallback.Factory.Create(this, callback)));

            // AddParameterToList(ComponentParameter.CreateParameter(GetParameterNameFromMethodExpression(expression), EventCallback.Factory.Create(this, callback)));
            return this;
        }

        /// <summary>
        /// Add a generic <see cref="EventCallback"/> parameter with a value for this builder.
        /// </summary>
        /// <param name="parameterSelector">The property or field parameter selector</param>
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

            AddParameterToList(ComponentParameter<TComponent, EventCallback<EventArgs>>.CreateParameter(parameterSelector, EventCallback.Factory.Create(this, callback)));

            // AddParameterToList(ComponentParameter.CreateParameter(GetParameterNameFromMethodExpression(expression), EventCallback.Factory.Create(this, callback)));
            return this;
        }

        /// <summary>
        /// Create a <see cref="ComponentParameter"/> collection.
        /// </summary>
        /// <returns>A IReadOnlyCollection of <see cref="ComponentParameter"/></returns>
        public IReadOnlyCollection<ComponentParameter> Build()
        {
            return _componentParameters.ToArray();
        }

        //private static string GetParameterNameFromParameterSelectorExpression<TValue>(Expression<Func<TComponent, TValue>> parameterSelector)
        //{
        //    if (parameterSelector.Body is MemberExpression memberExpression)
        //    {
        //        return memberExpression.Member.Name;
        //    }

        //    throw new ArgumentException($"The parameterSelector '{parameterSelector}' does not resolve to a Property or Field on the class '{typeof(TComponent)}'.");
        //}

        private void AddParameterToList(ComponentParameter parameter)
        {
            if (_componentParameters.All(cp => cp.Name != parameter.Name))
            {
                _componentParameters.Add(parameter);
            }
            else
            {
                throw new ArgumentException($"A parameter with the name '{parameter.Name}' has already been added.");
            }
        }
    }
}