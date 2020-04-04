using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Bunit
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TComponent"></typeparam>
    public sealed class ComponentParameterBuilder<TComponent> where TComponent : class, IComponent
    {
        private readonly ICollection<ComponentParameter> _componentParameters = new List<ComponentParameter>();

        /// <summary>
        /// Add a property or field-expression with normal value to this builder.
        /// </summary>
        /// <typeparam name="TValue">The generic Value type</typeparam>
        /// <param name="expression">The property or field expression</param>
        /// <param name="value">The value</param>
        /// <returns>A <see cref="ComponentParameterBuilder&lt;TComponent&gt;"/> which can be chained.</returns>
        public ComponentParameterBuilder<TComponent> Add<TValue>(Expression<Func<TComponent, TValue>> expression, TValue value)
        {
            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression));
            }

            AddParameterToList(ComponentParameter.CreateParameter(GetParameterName(expression), value));
            return this;
        }

        /// <summary>
        /// Add a property or field-expression with a cascading value to this builder.
        /// </summary>
        /// <typeparam name="TValue">The generic Value type</typeparam>
        /// <param name="expression">The property or field expression</param>
        /// <param name="value">The value</param>
        /// <returns>A <see cref="ComponentParameterBuilder&lt;TComponent&gt;"/> which can be chained.</returns>
        public ComponentParameterBuilder<TComponent> AddCascading<TValue>(Expression<Func<TComponent, TValue>> expression, TValue value)
        {
            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression));
            }

            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            AddParameterToList(ComponentParameter.CreateCascadingValue(GetParameterName(expression), value));
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

        public ComponentParameterBuilder<TComponent> Add(Expression<Func<TComponent, EventCallback>> expression, Func<Task> callback)
        {
            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression));
            }
            if (callback == null)
            {
                throw new ArgumentNullException(nameof(callback));
            }

            AddParameterToList(ComponentParameter.CreateParameter(GetParameterName(expression), EventCallback.Factory.Create(this, callback)));
            return this;
        }

        public ComponentParameterBuilder<TComponent> Add<TValue>(Expression<Func<TComponent, EventCallback<EventArgs>>> expression, Func<TValue, Task> callback)
        {
            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression));
            }
            if (callback == null)
            {
                throw new ArgumentNullException(nameof(callback));
            }

            AddParameterToList(ComponentParameter.CreateParameter(GetParameterName(expression), EventCallback.Factory.Create(this, callback)));
            return this;
        }

        private static string GetParameterName<TValue>(Expression<Func<TComponent, TValue>> expression)
        {
            if (expression.Body is MemberExpression memberExpression)
            {
                return memberExpression.Member.Name;
            }

            throw new ArgumentException($"The expression '{expression}' does not resolve to a Property or Field on the class '{typeof(TComponent)}'.");
        }

        private void AddParameterToList(ComponentParameter parameter)
        {
            if (!_componentParameters.Any(cp => string.Equals(cp.Name, parameter.Name, StringComparison.OrdinalIgnoreCase)))
            {
                _componentParameters.Add(parameter);
            }
            else
            {
                throw new ArgumentException($"A parameter with the name '{parameter.Name}' has already been added.");
            }
        }

        /// <summary>
        /// Create a <see cref="ComponentParameter"/> collection.
        /// </summary>
        /// <returns>A IReadOnlyCollection of <see cref="ComponentParameter"/></returns>
        public IReadOnlyCollection<ComponentParameter> Build()
        {
            return _componentParameters.ToArray();
        }
    }
}