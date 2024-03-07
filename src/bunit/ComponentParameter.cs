namespace Bunit;

/// <summary>
/// Represents a single parameter supplied to an <see cref="Microsoft.AspNetCore.Components.IComponent"/>
/// component under test.
/// </summary>
[SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "No need in this case.")]
internal readonly record struct ComponentParameter
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
	/// Gets a value indicating whether the parameter is for use by a <see cref="Microsoft.AspNetCore.Components.CascadingValue{TValue}"/>.
	/// </summary>
	public bool IsCascadingValue { get; }

	/// <summary>
	/// Initializes a new instance of the <see cref="ComponentParameter"/> struct.
	/// </summary>
	/// <param name="name">An optional name.</param>
	/// <param name="value">An optional value.</param>
	/// <param name="isCascadingValue">Whether or not this is a cascading value.</param>
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
	/// <param name="name">Name of the parameter to pass to the component.</param>
	/// <param name="value">Value or null to pass the component.</param>
	/// <returns>The created <see cref="ComponentParameter"/>.</returns>
	public static ComponentParameter CreateParameter(string name, object? value)
		=> new ComponentParameter(name, value, isCascadingValue: false);

	/// <summary>
	/// Create a Cascading Value parameter for a component under test.
	/// </summary>
	/// <param name="name">A optional name for the cascading value.</param>
	/// <param name="value">The cascading value.</param>
	/// <returns>The created <see cref="ComponentParameter"/>.</returns>
	public static ComponentParameter CreateCascadingValue(string? name, object value)
		=> new ComponentParameter(name, value, isCascadingValue: true);

	/// <summary>
	/// Create a parameter for a component under test.
	/// </summary>
	/// <param name="input">A name/value pair for the parameter.</param>
	/// <returns>The created <see cref="ComponentParameter"/>.</returns>
	public static implicit operator ComponentParameter((string Name, object? Value) input)
		=> CreateParameter(input.Name, input.Value);

	/// <summary>
	/// Create a parameter or cascading value for a component under test.
	/// </summary>
	/// <param name="input">A name/value/isCascadingValue triple for the parameter.</param>
	/// <returns>The created <see cref="ComponentParameter"/>.</returns>
	public static implicit operator ComponentParameter((string? Name, object? Value, bool IsCascadingValue) input)
		=> new ComponentParameter(input.Name, input.Value, input.IsCascadingValue);

	/// <inheritdoc/>
	public override int GetHashCode() => HashCode.Combine(Name, Value, IsCascadingValue);
}
