#if NET5_0_OR_GREATER
using Bunit.ComponentFactories;
using Bunit.TestDoubles;

namespace Bunit;

/// <summary>
/// Extension methods for using component doubles.
/// </summary>
public static class StubComponentFactoryCollectionExtensions
{
	/// <summary>
	/// Configures bUnit to use replace all components of type <typeparamref name="TComponent"/> (including derived components)
	/// with a <see cref="Stub{TComponent}"/> component in the render tree.
	/// </summary>
	/// <remarks>NOTE: This will replace any component of type <typeparamref name="TComponent"/> or components that derives/inherits from it.</remarks>
	/// <typeparam name="TComponent">The type of component to replace with a <see cref="Stub{TComponent}"/> component.</typeparam>
	/// <param name="factories">The bUnit <see cref="ComponentFactoryCollection"/> to configure.</param>
	/// <returns>A <see cref="ComponentFactoryCollection"/>.</returns>
	public static ComponentFactoryCollection AddStub<TComponent>(this ComponentFactoryCollection factories) where TComponent : IComponent
	{
		if (factories is null)
			throw new ArgumentNullException(nameof(factories));

		factories.Add(new StubComponentFactory(CreatePredicate(typeof(TComponent)), null));

		return factories;

		static Predicate<Type> CreatePredicate(Type componentTypeToStub)
			=> componentType => componentType == componentTypeToStub || componentType.IsAssignableTo(componentTypeToStub);
	}

	/// <summary>
	/// Configures bUnit to use replace all components of type <typeparamref name="TComponent"/> (including derived components)
	/// with a <see cref="Stub{TComponent}"/> component in the render tree.
	/// </summary>
	/// <remarks>NOTE: This will replace any component of type <typeparamref name="TComponent"/> or components that derives/inherits from it.</remarks>
	/// <typeparam name="TComponent">The type of component to replace with a <see cref="Stub{TComponent}"/> component.</typeparam>
	/// <param name="factories">The bUnit <see cref="ComponentFactoryCollection"/> to configure.</param>
	/// <param name="replacementMarkup">Markup that will be used as render output instead of the stubbed out component.</param>
	/// <returns>A <see cref="ComponentFactoryCollection"/>.</returns>
	public static ComponentFactoryCollection AddStub<TComponent>(this ComponentFactoryCollection factories, [StringSyntax("Html")]string replacementMarkup) where TComponent : IComponent
		=> AddStub<TComponent>(factories, b => b.AddMarkupContent(0, replacementMarkup));

	/// <summary>
	/// Configures bUnit to use replace all components of type <typeparamref name="TComponent"/> (including derived components)
	/// with a <see cref="Stub{TComponent}"/> component in the render tree.
	/// </summary>
	/// <remarks>NOTE: This will replace any component of type <typeparamref name="TComponent"/> or components that derives/inherits from it.</remarks>
	/// <typeparam name="TComponent">The type of component to replace with a <see cref="Stub{TComponent}"/> component.</typeparam>
	/// <param name="factories">The bUnit <see cref="ComponentFactoryCollection"/> to configure.</param>
	/// <param name="replacementFragment">Replacement render fragment that will be used as render output instead of the stubbed out component.</param>
	/// <returns>A <see cref="ComponentFactoryCollection"/>.</returns>
	public static ComponentFactoryCollection AddStub<TComponent>(this ComponentFactoryCollection factories, RenderFragment replacementFragment) where TComponent : IComponent
	{
		if (factories is null)
			throw new ArgumentNullException(nameof(factories));

		factories.Add(new StubComponentFactory(CreatePredicate(typeof(TComponent)), replacementFragment));

		return factories;

		static Predicate<Type> CreatePredicate(Type componentTypeToStub)
			=> componentType => componentType == componentTypeToStub || componentType.IsAssignableTo(componentTypeToStub);
	}

	/// <summary>
	/// Configures bUnit to use replace all components of type <typeparamref name="TComponent"/> (including derived components)
	/// with a <see cref="Stub{TComponent}"/> component in the render tree.
	/// </summary>
	/// <remarks>NOTE: This will replace any component of type <typeparamref name="TComponent"/> or components that derives/inherits from it.</remarks>
	/// <typeparam name="TComponent">The type of component to replace with a <see cref="Stub{TComponent}"/> component.</typeparam>
	/// <param name="factories">The bUnit <see cref="ComponentFactoryCollection"/> to configure.</param>
	/// <param name="replacementTemplate">Replacement template that will be used to render output instead of the stubbed out component.</param>
	/// <returns>A <see cref="ComponentFactoryCollection"/>.</returns>
	public static ComponentFactoryCollection AddStub<TComponent>(
		this ComponentFactoryCollection factories,
		Func<CapturedParameterView<TComponent>, string> replacementTemplate)
		where TComponent : IComponent
	{
		return AddStub<TComponent>(factories, ps => b => b.AddMarkupContent(0, replacementTemplate(ps)));
	}

	/// <summary>
	/// Configures bUnit to use replace all components of type <typeparamref name="TComponent"/> (including derived components)
	/// with a <see cref="Stub{TComponent}"/> component in the render tree.
	/// </summary>
	/// <remarks>NOTE: This will replace any component of type <typeparamref name="TComponent"/> or components that derives/inherits from it.</remarks>
	/// <typeparam name="TComponent">The type of component to replace with a <see cref="Stub{TComponent}"/> component.</typeparam>
	/// <param name="factories">The bUnit <see cref="ComponentFactoryCollection"/> to configure.</param>
	/// <param name="replacementTemplate">Replacement template that will be used to render output instead of the stubbed out component.</param>
	/// <returns>A <see cref="ComponentFactoryCollection"/>.</returns>
	public static ComponentFactoryCollection AddStub<TComponent>(
		this ComponentFactoryCollection factories,
		RenderFragment<CapturedParameterView<TComponent>> replacementTemplate)
		where TComponent : IComponent
	{
		if (factories is null)
			throw new ArgumentNullException(nameof(factories));
		if (replacementTemplate is null)
			throw new ArgumentNullException(nameof(replacementTemplate));

		factories.Add(new StubComponentFactory(CreatePredicate(typeof(TComponent)), replacementTemplate));

		return factories;

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
			string.Empty);

	/// <summary>
	/// Configures bUnit to use replace all components whose type make the <paramref name="componentTypePredicate"/> predicate return <c>true</c>
	/// with a <see cref="Stub{TComponent}"/> component in the render tree.
	/// </summary>
	/// <param name="factories">The bUnit <see cref="ComponentFactoryCollection"/> to configure.</param>
	/// <param name="componentTypePredicate">The predicate which decides if a component should be replaced with a <see cref="Stub{TComponent}"/> component.</param>
	/// <param name="replacementMarkup">Replacement markup that will be used to render output instead of the stubbed out components.</param>
	/// <returns>A <see cref="ComponentFactoryCollection"/>.</returns>
	public static ComponentFactoryCollection AddStub(
		this ComponentFactoryCollection factories,
		Predicate<Type> componentTypePredicate,
		string replacementMarkup)
		=> AddStub(
			factories,
			componentTypePredicate,
			b => b.AddMarkupContent(0, replacementMarkup));

	/// <summary>
	/// Configures bUnit to use replace all components whose type make the <paramref name="componentTypePredicate"/> predicate return <c>true</c>
	/// with a <see cref="Stub{TComponent}"/> component in the render tree.
	/// </summary>
	/// <param name="factories">The bUnit <see cref="ComponentFactoryCollection"/> to configure.</param>
	/// <param name="componentTypePredicate">The predicate which decides if a component should be replaced with a <see cref="Stub{TComponent}"/> component.</param>
	/// <param name="replacementFragment">Replacement <see cref="RenderFragment"/> that will be used to render output instead of the stubbed out components.</param>
	/// <returns>A <see cref="ComponentFactoryCollection"/>.</returns>
	public static ComponentFactoryCollection AddStub(
		this ComponentFactoryCollection factories,
		Predicate<Type> componentTypePredicate,
		RenderFragment replacementFragment)
	{
		if (factories is null)
			throw new ArgumentNullException(nameof(factories));
		if (componentTypePredicate is null)
			throw new ArgumentNullException(nameof(componentTypePredicate));

		factories.Add(new StubComponentFactory(componentTypePredicate, replacementFragment));
		return factories;
	}

#if NET8_0_OR_GREATER
	/// <summary>
	/// TODO.
	/// </summary>
#pragma warning disable S2326
	public static ComponentFactoryCollection AddGeneratedStub<TComponent>(this ComponentFactoryCollection factories) where TComponent : IComponent
#pragma warning restore S2326
	{
		if (factories is null)
			throw new ArgumentNullException(nameof(factories));

		return factories;
	}
#endif
}
#endif
