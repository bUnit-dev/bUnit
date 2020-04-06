using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.AspNetCore.Components;

namespace Bunit
{
    /// <summary>
    /// Represents a single parameter supplied to an <see cref="Microsoft.AspNetCore.Components.IComponent"/>
    /// component under test.
    /// </summary>    
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "<Pending>")]
    public readonly struct ComponentParameter<TComponent, TValue> : IEquatable<ComponentParameter<TComponent, TValue>> where TComponent : class, IComponent
    {
        /// <summary>
        /// Gets the name of the parameter.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the value being supplied to the component.
        /// </summary>
        [AllowNull]
        public TValue Value { get; }

        /// <summary>
        /// Gets a value to indicate whether the parameter is for use by a <see cref="CascadingValue{TValue}"/>.
        /// </summary>
        public bool IsCascadingValue { get; }

        private ComponentParameter(Expression<Func<TComponent, TValue>> parameterSelector, [AllowNull] TValue value, bool isCascadingValue)
        {
            if (parameterSelector == null)
            {
                throw new ArgumentNullException(nameof(parameterSelector));
            }

            if (isCascadingValue && value is null)
            {
                throw new ArgumentNullException(nameof(value), "Cascading values cannot be set to null");
            }

            Name = GetParameterNameFromParameterSelectorExpression(parameterSelector);
            Value = value;
            IsCascadingValue = isCascadingValue;
        }

        private static string GetParameterNameFromParameterSelectorExpression<T>(Expression<Func<TComponent, T>> parameterSelector)
        {
            if (parameterSelector == null)
            {
                throw new ArgumentNullException(nameof(parameterSelector));
            }

            if (parameterSelector.Body is MemberExpression memberExpression && memberExpression.Member is PropertyInfo propertyInfo)
            {
                return propertyInfo.Name;
            }

            throw new ArgumentException($"The expression '{parameterSelector}' does not resolve to a Property on the class '{typeof(TComponent)}'.");
        }

        /// <summary>
        /// Create a parameter for a component under test.
        /// </summary>
        /// <param name="parameterSelector">The parameter selector</param>
        /// <param name="value">Value or null to pass the component</param>
        public static ComponentParameter CreateParameter(Expression<Func<TComponent, TValue>> parameterSelector, [AllowNull] TValue value)
            => ComponentParameter.CreateParameter(GetParameterNameFromParameterSelectorExpression(parameterSelector), value);

        /// <summary>
        /// Create a Cascading Value parameter for a component under test.
        /// </summary>
        /// <param name="parameterSelector">The parameter selector</param>
        /// <param name="value">The cascading value</param>
        public static ComponentParameter CreateCascadingValue(Expression<Func<TComponent, TValue>> parameterSelector, [DisallowNull] TValue value)
            => ComponentParameter.CreateCascadingValue(GetParameterNameFromParameterSelectorExpression(parameterSelector), value);

        /// <summary>
        /// Create a parameter for a component under test.
        /// </summary>
        /// <param name="input">A name/value pair for the parameter</param>
        public static implicit operator ComponentParameter<TComponent, TValue>((Expression<Func<TComponent, TValue>> parameterSelector, TValue value) input)
            => new ComponentParameter<TComponent, TValue>(input.parameterSelector, input.value, false);

        /// <summary>
        /// Create a parameter or cascading value for a component under test.
        /// </summary>
        /// <param name="input">A name/value/isCascadingValue triple for the parameter</param>
        public static implicit operator ComponentParameter<TComponent, TValue>((Expression<Func<TComponent, TValue>> parameterSelector, TValue value, bool isCascadingValue) input)
            => new ComponentParameter<TComponent, TValue>(input.parameterSelector, input.value, input.isCascadingValue);

        /// <inheritdoc/>
        public bool Equals(ComponentParameter<TComponent, TValue> other)
            => string.Equals(Name, other.Name, StringComparison.Ordinal) && Equals(Value, other.Value) && IsCascadingValue == other.IsCascadingValue;

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is ComponentParameter<TComponent, TValue> other && Equals(other);

        /// <inheritdoc/>
        public override int GetHashCode() => HashCode.Combine(Name, Value, IsCascadingValue);

        /// <inheritdoc/>
        public static bool operator ==(ComponentParameter<TComponent, TValue> left, ComponentParameter<TComponent, TValue> right) => left.Equals(right);

        /// <inheritdoc/>
        public static bool operator !=(ComponentParameter<TComponent, TValue> left, ComponentParameter<TComponent, TValue> right) => !(left == right);
    }
}
