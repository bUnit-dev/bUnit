using System.Collections;
using System.Collections.Immutable;
using System.Linq.Expressions;
using System.Reflection;

namespace Bunit.TestDoubles;

/// <summary>
/// Represents a view of parameters captured by a <see cref="ComponentDoubleBase{TComponent}"/>.
/// </summary>
/// <typeparam name="TComponent"></typeparam>
public class CapturedParameterView<TComponent> : IReadOnlyDictionary<string, object?>
	where TComponent : IComponent
{
	/// <summary>
	/// Gets a empty <see cref="CapturedParameterView{TComponent}"/>.
	/// </summary>
	public static CapturedParameterView<TComponent> Empty { get; } = new(ImmutableDictionary<string, object?>.Empty);

	private static readonly Type ComponentType = typeof(TComponent);

	private readonly IReadOnlyDictionary<string, object?> parameters;

	private CapturedParameterView(IReadOnlyDictionary<string, object?> parameters)
		=> this.parameters = parameters;

	/// <summary>
	/// Gets the value of the parameter with the <paramref name="key"/>.
	/// </summary>
	/// <param name="key">Name of the parameter to get.</param>
	/// <returns>The value of the parameter</returns>
	public object? this[string key]
		=> parameters[key];

	/// <inheritdoc/>
	public IEnumerable<string> Keys
		=> parameters.Keys;

	/// <inheritdoc/>
	public IEnumerable<object?> Values
		=> parameters.Values;

	/// <inheritdoc/>
	public int Count
		=> parameters.Count;

	/// <inheritdoc/>
	public bool ContainsKey(string key)
		=> parameters.ContainsKey(key);

	/// <inheritdoc/>
	public bool TryGetValue(string key, [MaybeNullWhen(false)] out object value)
		=> parameters.TryGetValue(key, out value);

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
	public TValue? Get<TValue>(Expression<Func<TComponent, TValue?>> parameterSelector)
	{
		ArgumentNullException.ThrowIfNull(parameterSelector);

		if (parameterSelector.Body is not MemberExpression { Member: PropertyInfo propInfoCandidate })
			throw new ArgumentException($"The parameter selector '{parameterSelector}' does not resolve to a public property on the component '{typeof(TComponent)}'.", nameof(parameterSelector));

		var propertyInfo = propInfoCandidate.DeclaringType != ComponentType
			? ComponentType.GetProperty(propInfoCandidate.Name, propInfoCandidate.PropertyType)
			: propInfoCandidate;

		var paramAttr = propertyInfo?.GetCustomAttribute<ParameterAttribute>(inherit: true);
		var cascadingParamAttr = propertyInfo?.GetCustomAttribute<CascadingParameterAttribute>(inherit: true);

		if (propertyInfo is null || (paramAttr is null && cascadingParamAttr is null))
			throw new ArgumentException($"The parameter selector '{parameterSelector}' does not resolve to a public property on the component '{typeof(TComponent)}' with a [Parameter] or [CascadingParameter] attribute.", nameof(parameterSelector));

		if (!parameters.TryGetValue(propertyInfo.Name, out var objectResult))
			throw new ParameterNotFoundException(propertyInfo.Name, ComponentType.ToString());

		return (TValue?)objectResult;
	}

	/// <inheritdoc/>
	public IEnumerator<KeyValuePair<string, object?>> GetEnumerator()
		=> parameters.GetEnumerator();

	/// <inheritdoc/>
	IEnumerator IEnumerable.GetEnumerator()
		=> ((IEnumerable)parameters).GetEnumerator();

	/// <summary>
	/// Create an instances of the <see cref="CapturedParameterView{TComponent}"/>
	/// from the <paramref name="parameters"/> <see cref="ParameterView"/>.
	/// </summary>
	/// <param name="parameters">Parameters to create from.</param>
	/// <returns>An instance of <see cref="CapturedParameterView{TComponent}"/>.</returns>
	public static CapturedParameterView<TComponent> From(ParameterView parameters)
		=> new(parameters.ToDictionary()!);
}
