using System.Linq.Expressions;
using System.Reflection;
using Bunit.Extensions;

namespace Bunit;

/// <summary>
/// A <see cref="ComponentParameterCollection"/> builder for a specific <typeparamref name="TComponent"/> component under test.
/// </summary>
/// <typeparam name="TComponent">The type of component under test to add the parameters.</typeparam>
public sealed class ComponentParameterCollectionBuilder<TComponent>
	where TComponent : IComponent
{
	private const string ChildContent = nameof(ChildContent);
	private static readonly Type TComponentType = typeof(TComponent);

	/// <summary>
	/// Gets a value indicating whether <typeparamref name="TComponent"/> has a [Parameter(CaptureUnmatchedValues = true)] parameter.
	/// </summary>
	private static bool HasUnmatchedCaptureParameter { get; }
		= typeof(TComponent).GetProperties(BindingFlags.Instance | BindingFlags.Public)
			.Select(x => x.GetCustomAttribute<ParameterAttribute>())
			.Any(x => x is { CaptureUnmatchedValues: true });

	private readonly ComponentParameterCollection parameters = new();

	/// <summary>
	/// Initializes a new instance of the <see cref="ComponentParameterCollectionBuilder{TComponent}"/> class.
	/// </summary>
	public ComponentParameterCollectionBuilder() { }

	/// <summary>
	/// Initializes a new instance of the <see cref="ComponentParameterCollectionBuilder{TComponent}"/> class
	/// and invokes the <paramref name="parameterAdder"/> with it as the argument.
	/// </summary>
	public ComponentParameterCollectionBuilder(Action<ComponentParameterCollectionBuilder<TComponent>>? parameterAdder)
	{
		parameterAdder?.Invoke(this);
	}

	/// <summary>
	/// Adds a component parameter for the parameter selected with <paramref name="parameterSelector"/>
	/// with the value <paramref name="value"/>.
	/// </summary>
	/// <typeparam name="TValue">Type of <paramref name="value"/>.</typeparam>
	/// <param name="parameterSelector">A lambda function that selects the parameter.</param>
	/// <param name="value">The value to pass to <typeparamref name="TComponent"/>.</param>
	/// <returns>This <see cref="ComponentParameterCollectionBuilder{TComponent}"/>.</returns>
	public ComponentParameterCollectionBuilder<TComponent> Add<TValue>(Expression<Func<TComponent, TValue>> parameterSelector, [AllowNull] TValue value)
	{
		var (name, cascadingValueName, isCascading) = GetParameterInfo(parameterSelector, value);

		return isCascading
			? AddCascadingValueParameter(cascadingValueName, value)
			: AddParameter<TValue>(name, value);
	}

	/// <summary>
	/// Adds a component parameter for a <see cref="RenderFragment"/> parameter selected with <paramref name="parameterSelector"/>,
	/// where the <see cref="RenderFragment"/> value is created through the <paramref name="childParameterBuilder"/> argument.
	/// </summary>
	/// <typeparam name="TChildComponent">The type of component to create a <see cref="RenderFragment"/> for.</typeparam>
	/// <param name="parameterSelector">A lambda function that selects the parameter.</param>
	/// <param name="childParameterBuilder">A parameter builder for the <typeparamref name="TChildComponent"/>.</param>
	/// <returns>This <see cref="ComponentParameterCollectionBuilder{TComponent}"/>.</returns>
	public ComponentParameterCollectionBuilder<TComponent> Add<TChildComponent>(Expression<Func<TComponent, RenderFragment?>> parameterSelector, Action<ComponentParameterCollectionBuilder<TChildComponent>>? childParameterBuilder = null)
		where TChildComponent : IComponent => Add(parameterSelector, GetRenderFragment(childParameterBuilder));

	/// <summary>
	/// Adds a component parameter for a <see cref="RenderFragment"/> parameter selected with <paramref name="parameterSelector"/>,
	/// where the <see cref="RenderFragment"/> value is the markup passed in through the <paramref name="markup"/> argument.
	/// </summary>
	/// <param name="parameterSelector">A lambda function that selects the parameter.</param>
	/// <param name="markup">The markup string to pass to the <see cref="RenderFragment"/>.</param>
	/// <returns>This <see cref="ComponentParameterCollectionBuilder{TComponent}"/>.</returns>
	public ComponentParameterCollectionBuilder<TComponent> Add(Expression<Func<TComponent, RenderFragment?>> parameterSelector, [StringSyntax("Html")]string markup)
		=> Add(parameterSelector, markup.ToMarkupRenderFragment());

	/// <summary>
	/// Adds a component parameter for a <see cref="RenderFragment{TValue}"/> template parameter selected with <paramref name="parameterSelector"/>,
	/// where the <see cref="RenderFragment{TValue}"/> template is based on the <paramref name="markupFactory"/> argument.
	/// </summary>
	/// <typeparam name="TValue">The context type of the <see cref="RenderFragment{TValue}"/>.</typeparam>
	/// <param name="parameterSelector">A lambda function that selects the parameter.</param>
	/// <param name="markupFactory">A markup factory used to create the <see cref="RenderFragment{TValue}"/> template with.</param>
	/// <returns>This <see cref="ComponentParameterCollectionBuilder{TComponent}"/>.</returns>
	public ComponentParameterCollectionBuilder<TComponent> Add<TValue>(Expression<Func<TComponent, RenderFragment<TValue>?>> parameterSelector, Func<TValue, string> markupFactory)
	{
		ArgumentNullException.ThrowIfNull(markupFactory);
		return Add(parameterSelector, v => b => b.AddMarkupContent(0, markupFactory(v)));
	}

	/// <summary>
	/// Adds a component parameter for a <see cref="RenderFragment{TValue}"/> template parameter selected with <paramref name="parameterSelector"/>,
	/// where the <see cref="RenderFragment{TValue}"/> template is based on the <paramref name="templateFactory"/>, which is used
	/// to create a <see cref="RenderFragment{TValue}"/> that renders a <typeparamref name="TChildComponent"/> inside the template.
	/// </summary>
	/// <typeparam name="TChildComponent">The type of component to create a <see cref="RenderFragment{TValue}"/> for.</typeparam>
	/// <typeparam name="TValue">The context type of the <see cref="RenderFragment{TValue}"/>.</typeparam>
	/// <param name="parameterSelector">A lambda function that selects the parameter.</param>
	/// <param name="templateFactory">A template factory used to create the parameters being passed to the <typeparamref name="TChildComponent"/>.</param>
	/// <returns>This <see cref="ComponentParameterCollectionBuilder{TComponent}"/>.</returns>
	public ComponentParameterCollectionBuilder<TComponent> Add<TChildComponent, TValue>(Expression<Func<TComponent, RenderFragment<TValue>?>> parameterSelector, Func<TValue, Action<ComponentParameterCollectionBuilder<TChildComponent>>> templateFactory)
		where TChildComponent : IComponent
	{
		ArgumentNullException.ThrowIfNull(templateFactory);
		return Add(parameterSelector, value => GetRenderFragment(templateFactory(value)));
	}

	/// <summary>
	/// Adds a component parameter for an <see cref="EventCallback"/> parameter selected with <paramref name="parameterSelector"/>,
	/// where the <paramref name="callback"/> is used as value.
	/// </summary>
	/// <param name="parameterSelector">A lambda function that selects the parameter.</param>
	/// <param name="callback">The callback to pass to the <see cref="EventCallback"/>.</param>
	/// <returns>This <see cref="ComponentParameterCollectionBuilder{TComponent}"/>.</returns>
	public ComponentParameterCollectionBuilder<TComponent> Add(Expression<Func<TComponent, EventCallback>> parameterSelector, Action callback)
		=> Add(parameterSelector, EventCallback.Factory.Create(callback?.Target!, callback!));

	/// <summary>
	/// Adds a component parameter for a nullable <see cref="EventCallback"/> parameter selected with <paramref name="parameterSelector"/>,
	/// where the <paramref name="callback"/> is used as value.
	/// </summary>
	/// <param name="parameterSelector">A lambda function that selects the parameter.</param>
	/// <param name="callback">The callback to pass to the <see cref="EventCallback"/>.</param>
	/// <returns>This <see cref="ComponentParameterCollectionBuilder{TComponent}"/>.</returns>
	public ComponentParameterCollectionBuilder<TComponent> Add(Expression<Func<TComponent, EventCallback?>> parameterSelector, Action callback)
		=> Add(parameterSelector, EventCallback.Factory.Create(callback?.Target!, callback!));

	/// <summary>
	/// Adds a component parameter for an <see cref="EventCallback"/> parameter selected with <paramref name="parameterSelector"/>,
	/// where the <paramref name="callback"/> is used as value.
	/// </summary>
	/// <param name="parameterSelector">A lambda function that selects the parameter.</param>
	/// <param name="callback">The callback to pass to the <see cref="EventCallback"/>.</param>
	/// <returns>This <see cref="ComponentParameterCollectionBuilder{TComponent}"/>.</returns>
	public ComponentParameterCollectionBuilder<TComponent> Add(Expression<Func<TComponent, EventCallback>> parameterSelector, Action<object> callback)
		=> Add(parameterSelector, EventCallback.Factory.Create(callback?.Target!, callback!));

	/// <summary>
	/// Adds a component parameter for a nullable <see cref="EventCallback"/> parameter selected with <paramref name="parameterSelector"/>,
	/// where the <paramref name="callback"/> is used as value.
	/// </summary>
	/// <param name="parameterSelector">A lambda function that selects the parameter.</param>
	/// <param name="callback">The callback to pass to the <see cref="EventCallback"/>.</param>
	/// <returns>This <see cref="ComponentParameterCollectionBuilder{TComponent}"/>.</returns>
	public ComponentParameterCollectionBuilder<TComponent> Add(Expression<Func<TComponent, EventCallback?>> parameterSelector, Action<object> callback)
		=> Add(parameterSelector, EventCallback.Factory.Create(callback?.Target!, callback!));

	/// <summary>
	/// Adds a component parameter for an <see cref="EventCallback"/> parameter selected with <paramref name="parameterSelector"/>,
	/// where the <paramref name="callback"/> is used as value.
	/// </summary>
	/// <param name="parameterSelector">A lambda function that selects the parameter.</param>
	/// <param name="callback">The callback to pass to the <see cref="EventCallback"/>.</param>
	/// <returns>This <see cref="ComponentParameterCollectionBuilder{TComponent}"/>.</returns>
	public ComponentParameterCollectionBuilder<TComponent> Add(Expression<Func<TComponent, EventCallback>> parameterSelector, Func<Task> callback)
		=> Add(parameterSelector, EventCallback.Factory.Create(callback?.Target!, callback!));

	/// <summary>
	/// Adds a component parameter for a nullable <see cref="EventCallback"/> parameter selected with <paramref name="parameterSelector"/>,
	/// where the <paramref name="callback"/> is used as value.
	/// </summary>
	/// <param name="parameterSelector">A lambda function that selects the parameter.</param>
	/// <param name="callback">The callback to pass to the <see cref="EventCallback"/>.</param>
	/// <returns>This <see cref="ComponentParameterCollectionBuilder{TComponent}"/>.</returns>
	public ComponentParameterCollectionBuilder<TComponent> Add(Expression<Func<TComponent, EventCallback?>> parameterSelector, Func<Task> callback)
		=> Add(parameterSelector, EventCallback.Factory.Create(callback?.Target!, callback!));

	/// <summary>
	/// Adds a component parameter for an <see cref="EventCallback"/> parameter selected with <paramref name="parameterSelector"/>,
	/// where the <paramref name="callback"/> is used as value.
	/// </summary>
	/// <param name="parameterSelector">A lambda function that selects the parameter.</param>
	/// <param name="callback">The callback to pass to the <see cref="EventCallback"/>.</param>
	/// <returns>This <see cref="ComponentParameterCollectionBuilder{TComponent}"/>.</returns>
	public ComponentParameterCollectionBuilder<TComponent> Add(Expression<Func<TComponent, EventCallback>> parameterSelector, Func<object, Task> callback)
		=> Add(parameterSelector, EventCallback.Factory.Create(callback?.Target!, callback!));

	/// <summary>
	/// Adds a component parameter for a nullable <see cref="EventCallback"/> parameter selected with <paramref name="parameterSelector"/>,
	/// where the <paramref name="callback"/> is used as value.
	/// </summary>
	/// <param name="parameterSelector">A lambda function that selects the parameter.</param>
	/// <param name="callback">The callback to pass to the <see cref="EventCallback"/>.</param>
	/// <returns>This <see cref="ComponentParameterCollectionBuilder{TComponent}"/>.</returns>
	public ComponentParameterCollectionBuilder<TComponent> Add(Expression<Func<TComponent, EventCallback?>> parameterSelector, Func<object, Task> callback)
		=> Add(parameterSelector, EventCallback.Factory.Create(callback?.Target!, callback!));

	/// <summary>
	/// Adds a component parameter for an <see cref="EventCallback{TValue}"/> parameter selected with <paramref name="parameterSelector"/>,
	/// where the <paramref name="callback"/> is used as value.
	/// </summary>
	/// <param name="parameterSelector">A lambda function that selects the parameter.</param>
	/// <param name="callback">The callback to pass to the <see cref="EventCallback"/>.</param>
	/// <typeparam name="TValue">The value returned in the <see cref="EventCallback{TValue}"/>.</typeparam>
	/// <returns>This <see cref="ComponentParameterCollectionBuilder{TComponent}"/>.</returns>
	public ComponentParameterCollectionBuilder<TComponent> Add<TValue>(Expression<Func<TComponent, EventCallback<TValue>>> parameterSelector, Action callback)
		=> Add(parameterSelector, EventCallback.Factory.Create<TValue>(callback?.Target!, callback!));

	/// <summary>
	/// Adds a component parameter for a nullable <see cref="EventCallback{TValue}"/> parameter selected with <paramref name="parameterSelector"/>,
	/// where the <paramref name="callback"/> is used as value.
	/// </summary>
	/// <param name="parameterSelector">A lambda function that selects the parameter.</param>
	/// <param name="callback">The callback to pass to the <see cref="EventCallback"/>.</param>
	/// <typeparam name="TValue">The value returned in the <see cref="EventCallback{TValue}"/>.</typeparam>
	/// <returns>This <see cref="ComponentParameterCollectionBuilder{TComponent}"/>.</returns>
	public ComponentParameterCollectionBuilder<TComponent> Add<TValue>(Expression<Func<TComponent, EventCallback<TValue>?>> parameterSelector, Action callback)
		=> Add(parameterSelector, EventCallback.Factory.Create<TValue>(callback?.Target!, callback!));

	/// <summary>
	/// Adds a component parameter for an <see cref="EventCallback{TValue}"/> parameter selected with <paramref name="parameterSelector"/>,
	/// where the <paramref name="callback"/> is used as value.
	/// </summary>
	/// <param name="parameterSelector">A lambda function that selects the parameter.</param>
	/// <param name="callback">The callback to pass to the <see cref="EventCallback"/>.</param>
	/// <typeparam name="TValue">The value returned in the <see cref="EventCallback{TValue}"/>.</typeparam>
	/// <returns>This <see cref="ComponentParameterCollectionBuilder{TComponent}"/>.</returns>
	public ComponentParameterCollectionBuilder<TComponent> Add<TValue>(Expression<Func<TComponent, EventCallback<TValue>>> parameterSelector, Action<TValue> callback)
		=> Add(parameterSelector, EventCallback.Factory.Create<TValue>(callback?.Target!, callback!));

	/// <summary>
	/// Adds a component parameter for a nullable <see cref="EventCallback{TValue}"/> parameter selected with <paramref name="parameterSelector"/>,
	/// where the <paramref name="callback"/> is used as value.
	/// </summary>
	/// <param name="parameterSelector">A lambda function that selects the parameter.</param>
	/// <param name="callback">The callback to pass to the <see cref="EventCallback"/>.</param>
	/// <typeparam name="TValue">The value returned in the <see cref="EventCallback{TValue}"/>.</typeparam>
	/// <returns>This <see cref="ComponentParameterCollectionBuilder{TComponent}"/>.</returns>
	public ComponentParameterCollectionBuilder<TComponent> Add<TValue>(Expression<Func<TComponent, EventCallback<TValue>?>> parameterSelector, Action<TValue> callback)
		=> Add(parameterSelector, EventCallback.Factory.Create<TValue>(callback?.Target!, callback!));

	/// <summary>
	/// Adds a component parameter for an <see cref="EventCallback{TValue}"/> parameter selected with <paramref name="parameterSelector"/>,
	/// where the <paramref name="callback"/> is used as value.
	/// </summary>
	/// <param name="parameterSelector">A lambda function that selects the parameter.</param>
	/// <param name="callback">The callback to pass to the <see cref="EventCallback"/>.</param>
	/// <typeparam name="TValue">The value returned in the <see cref="EventCallback{TValue}"/>.</typeparam>
	/// <returns>This <see cref="ComponentParameterCollectionBuilder{TComponent}"/>.</returns>
	public ComponentParameterCollectionBuilder<TComponent> Add<TValue>(Expression<Func<TComponent, EventCallback<TValue>>> parameterSelector, Func<Task> callback)
		=> Add(parameterSelector, EventCallback.Factory.Create<TValue>(callback?.Target!, callback!));

	/// <summary>
	/// Adds a component parameter for a nullable <see cref="EventCallback{TValue}"/> parameter selected with <paramref name="parameterSelector"/>,
	/// where the <paramref name="callback"/> is used as value.
	/// </summary>
	/// <param name="parameterSelector">A lambda function that selects the parameter.</param>
	/// <param name="callback">The callback to pass to the <see cref="EventCallback"/>.</param>
	/// <typeparam name="TValue">The value returned in the <see cref="EventCallback{TValue}"/>.</typeparam>
	/// <returns>This <see cref="ComponentParameterCollectionBuilder{TComponent}"/>.</returns>
	public ComponentParameterCollectionBuilder<TComponent> Add<TValue>(Expression<Func<TComponent, EventCallback<TValue>?>> parameterSelector, Func<Task> callback)
		=> Add(parameterSelector, EventCallback.Factory.Create<TValue>(callback?.Target!, callback!));

	/// <summary>
	/// Adds a component parameter for an <see cref="EventCallback{TValue}"/> parameter selected with <paramref name="parameterSelector"/>,
	/// where the <paramref name="callback"/> is used as value.
	/// </summary>
	/// <param name="parameterSelector">A lambda function that selects the parameter.</param>
	/// <param name="callback">The callback to pass to the <see cref="EventCallback"/>.</param>
	/// <typeparam name="TValue">The value returned in the <see cref="EventCallback{TValue}"/>.</typeparam>
	/// <returns>This <see cref="ComponentParameterCollectionBuilder{TComponent}"/>.</returns>
	public ComponentParameterCollectionBuilder<TComponent> Add<TValue>(Expression<Func<TComponent, EventCallback<TValue>>> parameterSelector, Func<TValue, Task> callback)
		=> Add(parameterSelector, EventCallback.Factory.Create<TValue>(callback?.Target!, callback!));

	/// <summary>
	/// Adds a component parameter for a nullable <see cref="EventCallback{TValue}"/> parameter selected with <paramref name="parameterSelector"/>,
	/// where the <paramref name="callback"/> is used as value.
	/// </summary>
	/// <param name="parameterSelector">A lambda function that selects the parameter.</param>
	/// <param name="callback">The callback to pass to the <see cref="EventCallback"/>.</param>
	/// <typeparam name="TValue">The value returned in the <see cref="EventCallback{TValue}"/>.</typeparam>
	/// <returns>This <see cref="ComponentParameterCollectionBuilder{TComponent}"/>.</returns>
	public ComponentParameterCollectionBuilder<TComponent> Add<TValue>(Expression<Func<TComponent, EventCallback<TValue>?>> parameterSelector, Func<TValue, Task> callback)
		=> Add(parameterSelector, EventCallback.Factory.Create<TValue>(callback?.Target!, callback!));

	/// <summary>
	/// Adds a ChildContent <see cref="RenderFragment"/> type parameter with the <paramref name="childContent"/> as value.
	///
	/// Note, this is equivalent to <c>Add(p => p.ChildContent, childContent)</c>.
	/// </summary>
	/// <param name="childContent">The <see cref="RenderFragment"/> to pass the ChildContent parameter.</param>
	/// <returns>This <see cref="ComponentParameterCollectionBuilder{TComponent}"/>.</returns>
	public ComponentParameterCollectionBuilder<TComponent> AddChildContent(RenderFragment childContent)
	{
		if (!HasChildContentParameter())
			throw new ArgumentException($"The component '{typeof(TComponent)}' does not have a {ChildContent} [Parameter] attribute.", nameof(childContent));

		if (HasGenericChildContentParameter())
			throw new ArgumentException($"Calling AddChildContent on component '{typeof(TComponent)}' with a generic {ChildContent} type (RenderFragment<T>) is not supported. Use the 'Add(p => p.ChildContent, p => {{content}})' method instead.", nameof(childContent));

		return AddParameter(ChildContent, childContent);
	}

	/// <summary>
	/// Adds a ChildContent <see cref="RenderFragment"/> type parameter with the <paramref name="markup"/> as value
	/// wrapped in a <see cref="RenderFragment"/>.
	///
	/// Note, this is equivalent to <c>Add(p => p.ChildContent, "...")</c>.
	/// </summary>
	/// <param name="markup">The markup string to pass the ChildContent parameter wrapped in a <see cref="RenderFragment"/>.</param>
	/// <returns>This <see cref="ComponentParameterCollectionBuilder{TComponent}"/>.</returns>
	public ComponentParameterCollectionBuilder<TComponent> AddChildContent([StringSyntax("Html")]string markup)
		=> AddChildContent(markup.ToMarkupRenderFragment());

	/// <summary>
	/// Adds a ChildContent <see cref="RenderFragment"/> type parameter, that is passed a <see cref="RenderFragment"/>,
	/// which will render the <typeparamref name="TChildComponent"/> with the parameters passed to <paramref name="childParameterBuilder"/>.
	/// </summary>
	/// <typeparam name="TChildComponent">Type of child component to pass to the ChildContent parameter.</typeparam>
	/// <param name="childParameterBuilder">A parameter builder for the <typeparamref name="TChildComponent"/>.</param>
	/// <returns>This <see cref="ComponentParameterCollectionBuilder{TComponent}"/>.</returns>
	public ComponentParameterCollectionBuilder<TComponent> AddChildContent<TChildComponent>(Action<ComponentParameterCollectionBuilder<TChildComponent>>? childParameterBuilder = null)
		where TChildComponent : IComponent
		=> AddChildContent(GetRenderFragment(childParameterBuilder));

	/// <summary>
	/// Adds an UNNAMED cascading value around the <typeparamref name="TComponent"/> when it is rendered. Used to
	/// pass cascading values to child components of <typeparamref name="TComponent"/>.
	/// </summary>
	/// <typeparam name="TValue">The type of cascading value.</typeparam>
	/// <param name="cascadingValue">The cascading value.</param>
	/// <returns>This <see cref="ComponentParameterCollectionBuilder{TComponent}"/>.</returns>
	public ComponentParameterCollectionBuilder<TComponent> AddCascadingValue<TValue>(TValue cascadingValue)
		where TValue : notnull
		=> AddCascadingValueParameter(name: null, cascadingValue);

	/// <summary>
	/// Adds an NAMED cascading value around the <typeparamref name="TComponent"/> when it is rendered. Used to
	/// pass cascading values to child components of <typeparamref name="TComponent"/>.
	/// </summary>
	/// <typeparam name="TValue">The type of cascading value.</typeparam>
	/// <param name="name">The name of the cascading value.</param>
	/// <param name="cascadingValue">The cascading value.</param>
	/// <returns>This <see cref="ComponentParameterCollectionBuilder{TComponent}"/>.</returns>
	public ComponentParameterCollectionBuilder<TComponent> AddCascadingValue<TValue>(string name, TValue cascadingValue)
		where TValue : notnull
		=> AddCascadingValueParameter(name, cascadingValue);

	/// <summary>
	/// Adds an unmatched attribute value to <typeparamref name="TComponent"/>.
	/// </summary>
	/// <param name="name">The name of the unmatched attribute.</param>
	/// <param name="value">The value of the unmatched attribute.</param>
	/// <returns>This <see cref="ComponentParameterCollectionBuilder{TComponent}"/>.</returns>
	public ComponentParameterCollectionBuilder<TComponent> AddUnmatched(string name, object? value = null)
	{
		if (string.IsNullOrWhiteSpace(name))
			throw new ArgumentException("An unmatched parameter (attribute) cannot have an empty name.", nameof(name));

		if (!HasUnmatchedCaptureParameter)
			throw new ArgumentException($"The component '{typeof(TComponent)}' does not have an [Parameter(CaptureUnmatchedValues = true)] parameter.", nameof(name));

		return AddParameter(name, value);
	}

	/// <summary>Adds two-way binding, simulating the <c>@bind-Parameter</c> directive, to a given pair of parameters.</summary>
	/// <param name="parameterSelector">Parameter-selector for the two-way binding.</param>
	/// <param name="initialValue">The initial value to pass to <typeparamref name="TComponent"/>.</param>
	/// <param name="changedAction">Action which gets invoked when the value has changed.</param>
	/// <param name="valueExpression">Optional value expression.</param>
	/// <returns>This <see cref="ComponentParameterCollectionBuilder{TComponent}"/>.</returns>
	/// <remarks>
	/// This function is a short-hand form for the following expression:
	/// <code>Render&lt;<typeparamref name="TComponent"/>&gt;(ps => ps
	///   .Add(c => c.Value, value)
	///   .Add(c => c.ValueChanged, newValue => value = newValue)
	///   .Add(c => c.ValueExpression, () => value));
	/// </code>
	/// With <c>Bind</c>, it can be written like this:
	/// <code>Render&lt;<typeparamref name="TComponent"/>&gt;(ps => ps
	///   .Bind(c => c.Value, value, newValue => value = newValue, () => value));
	/// </code>
	/// </remarks>
	public ComponentParameterCollectionBuilder<TComponent> Bind<TValue>(
		Expression<Func<TComponent, TValue>> parameterSelector,
		TValue initialValue,
		Action<TValue> changedAction,
		Expression<Func<TValue>>? valueExpression = null)
	{
		var (parameterName, _, isCascading) = GetParameterInfo(parameterSelector, initialValue);

		if (isCascading)
			throw new ArgumentException("Using Bind with a cascading parameter is not allowed.", parameterName);

		ArgumentNullException.ThrowIfNull(changedAction);

		var changedName = $"{parameterName}Changed";
		var expressionName = $"{parameterName}Expression";

		AssertBindTargetIsCorrect(parameterName, parameterSelector);

		if (!HasPublicParameterProperty(changedName))
			throw new InvalidOperationException($"The parameter selector '{parameterSelector}' does not resolve to a " +
												$"parameter that has a related parameter with the name {changedName}. " +
												$"This is required for two way binding.");

		AddParameter(parameterName, initialValue);
		AddParameter(changedName, EventCallback.Factory.Create(changedAction.Target!, changedAction));

		return !HasPublicParameterProperty(expressionName)
			? this
			: AddParameter(expressionName, valueExpression ?? (() => initialValue));

		static void AssertBindTargetIsCorrect(string parameterName, Expression<Func<TComponent, TValue>> parameterSelector)
		{
			var isBindEventParameter = parameterName.EndsWith("Changed", StringComparison.Ordinal) || parameterName.EndsWith("Expression", StringComparison.Ordinal);
			var isBindEventType = IsConcreteGenericOf(typeof(TValue), typeof(EventCallback<>)) || IsConcreteGenericOf(typeof(TValue), typeof(Expression<>));
			if (isBindEventParameter && isBindEventType)
			{
				var selectorExpression = parameterSelector.ToString();
				var possibleSelector = TrimEnd(parameterName, "Changed");
				possibleSelector = TrimEnd(possibleSelector, "Expression");
				throw new ArgumentException($"The parameter selector {selectorExpression} does not correspond " +
											$"to a valid target for a @bind expression.{Environment.NewLine}If the structure of the " +
											$"component is <MyComponent @bind-Value=\"value\" /> call " +
											$"Bind(p => p.Value, \"initial value\", p => p.ValueChanged, v => someVar = v);" +
											Environment.NewLine +
											$"Try {selectorExpression.Replace(parameterName, possibleSelector, StringComparison.Ordinal)} instead.");
			}
		}

		static string TrimEnd(string source, string value)
			=> source.EndsWith(value, StringComparison.Ordinal)
			? source.Remove(source.LastIndexOf(value, StringComparison.Ordinal))
			: source;
	}

	/// <summary>
	/// Try to add a <paramref name="value"/> for a parameter with the <paramref name="name"/>, if
	/// <typeparamref name="TComponent"/> has a property with that name, AND that property has a <see cref="ParameterAttribute"/>
	/// or a <see cref="CascadingParameterAttribute"/>.
	/// </summary>
	/// <remarks>
	/// This is an untyped version of this method named <see cref="AddUnmatched"/>. Always
	/// prefer the strongly typed <c>Add</c> methods whenever possible.
	/// </remarks>
	/// <typeparam name="TValue">Value type.</typeparam>
	/// <param name="name">Name of the property for the parameter.</param>
	/// <param name="value">Value to assign to the parameter.</param>
	/// <returns>True if parameter with the name exists and value was set, false otherwise.</returns>
	public bool TryAdd<TValue>(string name, [AllowNull] TValue value)
	{
		if (TComponentType.GetProperty(name, BindingFlags.Public | BindingFlags.Instance) is PropertyInfo ccProp)
		{
			if (ccProp.GetCustomAttribute<ParameterAttribute>(inherit: false) is not null)
			{
				AddParameter(name, value);
				return true;
			}

			if (ccProp.GetCustomAttribute<CascadingParameterAttribute>(inherit: false) is CascadingParameterAttribute cpa)
			{
				AddCascadingValueParameter(cpa.Name, value);
				return true;
			}
		}

		return false;
	}

	/// <summary>
	/// Builds the <see cref="ComponentParameterCollection"/>.
	/// </summary>
	/// <returns>The created <see cref="ComponentParameterCollection"/>.</returns>
	internal ComponentParameterCollection Build() => parameters;

	private static (string Name, string? CascadingValueName, bool IsCascading) GetParameterInfo<TValue>(
		Expression<Func<TComponent, TValue>> parameterSelector, object? value
		)
	{
		ArgumentNullException.ThrowIfNull(parameterSelector);

		if (parameterSelector.Body is not MemberExpression { Member: PropertyInfo propInfoCandidate })
			throw new ArgumentException($"The parameter selector '{parameterSelector}' does not resolve to a public property on the component '{typeof(TComponent)}'.", nameof(parameterSelector));

		var propertyInfo = propInfoCandidate.DeclaringType != TComponentType
			? TComponentType.GetProperty(propInfoCandidate.Name, propInfoCandidate.PropertyType)
			: propInfoCandidate;

		var paramAttr = propertyInfo?.GetCustomAttribute<ParameterAttribute>(inherit: true);
		var cascadingParamAttrBase = propertyInfo?.GetCustomAttribute<CascadingParameterAttributeBase>(inherit: true);

		if (propertyInfo is null || (paramAttr is null && cascadingParamAttrBase is null))
			throw new ArgumentException($"The parameter selector '{parameterSelector}' does not resolve to a public property on the component '{typeof(TComponent)}' with a [Parameter] or [CascadingParameter]attribute.", nameof(parameterSelector));

		if (cascadingParamAttrBase is null)
			return (propertyInfo.Name, CascadingValueName: null, IsCascading: false);

		var name = cascadingParamAttrBase switch
		{
			CascadingParameterAttribute cpa => cpa.Name,
			SupplyParameterFromQueryAttribute s => throw CreateErrorMessageForSupplyFromQuery(value, propertyInfo, s.Name),
			_ => throw new NotSupportedException($"The type '{cascadingParamAttrBase.GetType()}' is not supported"),
		};

		return (propertyInfo.Name, CascadingValueName: name, IsCascading: true);

		static ArgumentException CreateErrorMessageForSupplyFromQuery(
			object? value,
			MemberInfo propertyInfo,
			string? name)
		{
			var cascadingParameterName = name ?? propertyInfo.Name;

			return new ArgumentException($"""
			                              To pass a value to a SupplyParameterFromQuery parameter, use the NavigationManager and navigate to the URI.
			                              For example:

			                              var uri = NavigationManager.GetUriWithQueryParameter("{cascadingParameterName}", "{value}");
			                              NavigationManager.NavigateTo(uri);
			                              """);
		}
	}

	private static bool HasChildContentParameter()
		=> TComponentType.GetProperty(ChildContent, BindingFlags.Public | BindingFlags.Instance) is PropertyInfo ccProp
			&& ccProp.GetCustomAttribute<ParameterAttribute>(inherit: false) is not null;

	private static bool HasGenericChildContentParameter()
		=> TComponentType.GetProperty(ChildContent, BindingFlags.Public | BindingFlags.Instance) is PropertyInfo ccProp
			&& ccProp.PropertyType.IsGenericType;

	private ComponentParameterCollectionBuilder<TComponent> AddParameter<TValue>(string name, [AllowNull] TValue value)
	{
		parameters.Add(ComponentParameter.CreateParameter(name, value));
		return this;
	}

	private ComponentParameterCollectionBuilder<TComponent> AddCascadingValueParameter(string? name, object? cascadingValue)
	{
		var value = cascadingValue ?? throw new ArgumentNullException(nameof(cascadingValue), "Passing null values to cascading value parameters is not allowed.");
		parameters.Add(ComponentParameter.CreateCascadingValue(name, value));
		return this;
	}

	private static RenderFragment GetRenderFragment<TChildComponent>(Action<ComponentParameterCollectionBuilder<TChildComponent>>? childParameterBuilder)
		where TChildComponent : IComponent
	{
		var childBuilder = new ComponentParameterCollectionBuilder<TChildComponent>(childParameterBuilder);
		return childBuilder.Build().ToRenderFragment<TChildComponent>();
	}

	private static bool HasPublicParameterProperty(string parameterName)
	{
		var type = typeof(TComponent);
		var property = type.GetProperty(parameterName);

		return property is not null && Array.Exists(
			property.GetCustomAttributes(inherit: true),
			a => a is ParameterAttribute);
	}

	private static bool IsConcreteGenericOf(Type type, Type openGeneric)
	{
		if (!type.IsGenericType)
			return false;

		return type.GetGenericTypeDefinition() == openGeneric;
	}
}
