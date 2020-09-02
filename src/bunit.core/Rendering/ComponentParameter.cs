using System;
using System.Diagnostics.CodeAnalysis;

namespace Bunit.Rendering
{
	/// <summary>
	/// Represents a single parameter supplied to an <see cref="Microsoft.AspNetCore.Components.IComponent"/>
	/// component under test.
	/// </summary>    
	[SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "<Pending>")]
	public readonly struct ComponentParameter : IEquatable<ComponentParameter>
	{
		/// <summary>
		/// Gets the name of the parameter. Can be null if the parameter is for an unnamed cascading value.
		/// </summary>
		public string? Name { get; }

		/// <summary>
		/// Gets the value being supplied to the component.
		/// </summary>
		public object? Value { get; }

		/// <summary>
		/// Gets a value to indicate whether the parameter is for use by a <see cref="Microsoft.AspNetCore.Components.CascadingValue{TValue}"/>.
		/// </summary>
		public bool IsCascadingValue { get; }

		/// <summary>
		/// Creates an instance of the <see cref="ComponentParameter"/> type.
		/// </summary>
		/// <param name="name">An optional name</param>
		/// <param name="value">An optional value</param>
		/// <param name="isCascadingValue">Whether or not this is a cascading value</param>
		private ComponentParameter(string? name, object? value, bool isCascadingValue)
		{
			if (isCascadingValue && value is null)
				throw new ArgumentNullException(nameof(value), "Cascading values cannot be set to null");

			if (!isCascadingValue && name is null)
				throw new ArgumentNullException(nameof(name), "A parameters name cannot be set to null");

			Name = name;
			Value = value;
			IsCascadingValue = isCascadingValue;
		}

		/// <summary>
		/// Create a parameter for a component under test.
		/// </summary>
		/// <param name="name">Name of the parameter to pass to the component</param>
		/// <param name="value">Value or null to pass the component</param>
		public static ComponentParameter CreateParameter(string name, object? value)
			=> new ComponentParameter(name, value, false);

		/// <summary>
		/// Create a Cascading Value parameter for a component under test.
		/// </summary>
		/// <param name="name">A optional name for the cascading value</param>
		/// <param name="value">The cascading value</param>
		public static ComponentParameter CreateCascadingValue(string? name, object value)
			=> new ComponentParameter(name, value, true);

		/// <summary>
		/// Create a parameter for a component under test.
		/// </summary>
		/// <param name="input">A name/value pair for the parameter</param>
		public static implicit operator ComponentParameter((string name, object? value) input)
			=> CreateParameter(input.name, input.value);

		/// <summary>
		/// Create a parameter or cascading value for a component under test.
		/// </summary>
		/// <param name="input">A name/value/isCascadingValue triple for the parameter</param>
		public static implicit operator ComponentParameter((string? name, object? value, bool isCascadingValue) input)
			=> new ComponentParameter(input.name, input.value, input.isCascadingValue);

		/// <inheritdoc/>
		public bool Equals(ComponentParameter other)
			=> string.Equals(Name, other.Name, StringComparison.Ordinal) && Value == other.Value && IsCascadingValue == other.IsCascadingValue;

		/// <inheritdoc/>
		public override bool Equals(object? obj) => obj is ComponentParameter other && Equals(other);

		/// <inheritdoc/>
		public override int GetHashCode() => HashCode.Combine(Name, Value, IsCascadingValue);

		/// <inheritdoc/>
		public static bool operator ==(ComponentParameter left, ComponentParameter right) => left.Equals(right);

		/// <inheritdoc/>
		public static bool operator !=(ComponentParameter left, ComponentParameter right) => !(left == right);
	}
}
