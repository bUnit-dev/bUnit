using Bunit.ComponentFactories;
using Bunit.TestDoubles;

namespace Bunit;

/// <summary>
/// Extension methods for using component doubles.
/// </summary>
public static class ComponentFactoryCollectionExtensions
{
	private static readonly RenderFragment<IReadOnlyDictionary<string, object>> NoopReplacementTemplate = _ => _ => { };

	/// <summary>
	/// Configures bUnit to use replace all components of type <typeparamref name="TComponent"/> (including derived components)
	/// with a <see cref="Stub{TComponent}"/> component in the render tree.
	/// </summary>
	/// <remarks>NOTE: This will replace any component of type <typeparamref name="TComponent"/> or components that derives/inherits from it.</remarks>
	/// <typeparam name="TComponent">The type of component to replace with a <see cref="Stub{TComponent}"/> component.</typeparam>
	/// <param name="factories">The bUnit <see cref="ComponentFactoryCollection"/> to configure.</param>
	/// <returns>A <see cref="ComponentFactoryCollection"/>.</returns>
	public static ComponentFactoryCollection AddStub<TComponent>(this ComponentFactoryCollection factories) where TComponent : IComponent
		=> AddStub<TComponent>(factories, NoopReplacementTemplate);

	/// <summary>
	/// Configures bUnit to use replace all components of type <typeparamref name="TComponent"/> (including derived components)
	/// with a <see cref="Stub{TComponent}"/> component in the render tree.
	/// </summary>
	/// <remarks>NOTE: This will replace any component of type <typeparamref name="TComponent"/> or components that derives/inherits from it.</remarks>
	/// <typeparam name="TComponent">The type of component to replace with a <see cref="Stub{TComponent}"/> component.</typeparam>
	/// <param name="factories">The bUnit <see cref="ComponentFactoryCollection"/> to configure.</param>
	/// <param name="replacementTemplate">Optional replacement template that will be used to render output instead of the stubbed out component.</param>
	/// <returns>A <see cref="ComponentFactoryCollection"/>.</returns>
	public static ComponentFactoryCollection AddStub<TComponent>(
		this ComponentFactoryCollection factories,
		Func<IReadOnlyDictionary<string, object>, string> replacementTemplate)
		where TComponent : IComponent
	{
		return AddStub<TComponent>(
			factories,
			ps => b => b.AddMarkupContent(0, replacementTemplate(ps)));
	}

	/// <summary>
	/// Configures bUnit to use replace all components of type <typeparamref name="TComponent"/> (including derived components)
	/// with a <see cref="Stub{TComponent}"/> component in the render tree.
	/// </summary>
	/// <remarks>NOTE: This will replace any component of type <typeparamref name="TComponent"/> or components that derives/inherits from it.</remarks>
	/// <typeparam name="TComponent">The type of component to replace with a <see cref="Stub{TComponent}"/> component.</typeparam>
	/// <param name="factories">The bUnit <see cref="ComponentFactoryCollection"/> to configure.</param>
	/// <param name="replacementTemplate">Optional replacement template that will be used to render output instead of the stubbed out component.</param>
	/// <returns>A <see cref="ComponentFactoryCollection"/>.</returns>
	public static ComponentFactoryCollection AddStub<TComponent>(
		this ComponentFactoryCollection factories,
		RenderFragment<IReadOnlyDictionary<string, object>> replacementTemplate)
		where TComponent : IComponent
	{
		return AddStub(factories, CreatePredicate(typeof(TComponent)), replacementTemplate);

		static Predicate<Type> CreatePredicate(Type componentTypeToStub)
			=> componentType => componentType == componentTypeToStub || componentType.IsAssignableTo(componentTypeToStub);
	}

	/// <summary>
	/// Configures bUnit to use replace all components whose type make the <paramref name="componentTypePredicate"/> predicate return <c>true</c>
	/// with a <see cref="Stub{TComponent}"/> component in the render tree.
	/// </summary>
	/// <param name="factories">The bUnit <see cref="ComponentFactoryCollection"/> to configure.</param>
	/// <param name="componentTypePredicate">The predicate which decides if a component should be replaced with a <see cref="Stub{TComponent}"/> component.</param>
	/// <returns>A <see cref="ComponentFactoryCollection"/>.</returns>
	public static ComponentFactoryCollection AddStub(
		this ComponentFactoryCollection factories,
		Predicate<Type> componentTypePredicate)
		=> AddStub(
			factories,
			componentTypePredicate,
			NoopReplacementTemplate);

	/// <summary>
	/// Configures bUnit to use replace all components whose type make the <paramref name="componentTypePredicate"/> predicate return <c>true</c>
	/// with a <see cref="Stub{TComponent}"/> component in the render tree.
	/// </summary>
	/// <param name="factories">The bUnit <see cref="ComponentFactoryCollection"/> to configure.</param>
	/// <param name="componentTypePredicate">The predicate which decides if a component should be replaced with a <see cref="Stub{TComponent}"/> component.</param>
	/// <param name="replacementTemplate">Optional replacement template that will be used to render output instead of the stubbed out component.</param>
	/// <returns>A <see cref="ComponentFactoryCollection"/>.</returns>
	public static ComponentFactoryCollection AddStub(
		this ComponentFactoryCollection factories,
		Predicate<Type> componentTypePredicate,
		Func<IReadOnlyDictionary<string, object>, string> replacementTemplate)
		=> AddStub(
			factories,
			componentTypePredicate,
			ps => b => b.AddMarkupContent(0, replacementTemplate(ps)));

	/// <summary>
	/// Configures bUnit to use replace all components whose type make the <paramref name="componentTypePredicate"/> predicate return <c>true</c>
	/// with a <see cref="Stub{TComponent}"/> component in the render tree.
	/// </summary>
	/// <param name="factories">The bUnit <see cref="ComponentFactoryCollection"/> to configure.</param>
	/// <param name="componentTypePredicate">The predicate which decides if a component should be replaced with a <see cref="Stub{TComponent}"/> component.</param>
	/// <param name="replacementTemplate">Optional replacement template that will be used to render output instead of the stubbed out component.</param>
	/// <returns>A <see cref="ComponentFactoryCollection"/>.</returns>
	public static ComponentFactoryCollection AddStub(
		this ComponentFactoryCollection factories,
		Predicate<Type> componentTypePredicate,
		RenderFragment<IReadOnlyDictionary<string, object>> replacementTemplate)
	{
		if (factories is null)
			throw new ArgumentNullException(nameof(factories));
		if (componentTypePredicate is null)
			throw new ArgumentNullException(nameof(componentTypePredicate));

		factories.Add(new StubComponentFactory(componentTypePredicate, replacementTemplate));
		return factories;
	}
}
