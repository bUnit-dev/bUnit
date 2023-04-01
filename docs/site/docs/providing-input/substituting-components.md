---
uid: substituting-components
title: Substituting (mocking) component
---

# Substituting (mocking) components

bUnit makes it possible to substitute child components of a component under test with other components, e.g. mock components. This makes it possible to isolate a component under test from other components it depends on, e.g. 3rd party components.

To substitute a component during a test, you must register the substitute, or a substitute factory, with the `ComponentFactories` collection on bUnit's `TestContext`.

> [!NOTE] 
> This feature is only available for test projects that target .NET 5 or later.

The following sections will explain how to create substitute components and how to register them with the `ComponentFactories` collection.

## Creating substitute (mock) components

These are the requirements substitute components must meet:

1. A substitute component must implement `IComponent`, i.e. be a Blazor component.
2. A substitute component must have the same parameters as the original component, _OR_ have a `CaptureUnmatchedValues` parameter that Blazor can pass all parameters to.
3. _If_ the original component is assigned to a variable in component under test, e.g. via the `@ref` attribute, a substitute must be assignable to the original component (inherit from it).

Most popular mocking libraries are able to create substitute/mock components easily, based on the original component, that follow the requirement specified above.

If the substitute only has to match the two first requirements, bUnit's built-in `Stub<T>` can be used.

Finally, for complex scenarios, a hand-coded substitute component can be created.

### Substituting components with bUnit's `Stub<T>`

When the component that should be substitute out is not referenced in the component under test with `@ref`, use bUnit's built-in "stubbing" capability.

For example, supposed you want to test the `<Foo>` component and substitute out it's child component `<Bar>`:

```cshtml
<Foo>
  <Bar />
</Foo>
```

To stub it out, use the `AddStub<T>()` method:

```csharp
[Fact]
public void Foo_Doesnt_Have_A_Bar_But_Stub()
{
  // Register the a stub substitution.
  ComponentFactories.AddStub<Bar>();

  // Render the component under test.
  IRenderedFragment cut = Render(@<Foo />);
  
  // Verify that the Bar component has 
  // been substituted in the render tree.
  Assert.False(cut.HasComponent<Bar>());
  Assert.True(cut.HasComponent<Stub<Bar>>());
}
```

It is also possible to specify a base type/component for the component you want to substitute. For example, if `<Bar>` inherits from `<BarBase>`, you can specify `<BarBase>` and all components that inherit from `<BarBase>` will be substituted.

```csharp
ComponentFactories.AddStub<BarBase>();
```

To add substitute markup to the output, pass it in one of the following ways:

```csharp
// Add the markup specified in the string to the rendered output
// instead of that from <Bar>.
ComponentFactories.AddStub<Bar>("<div>NOT FROM BAR</div>");

// Add the markup specified in the render fragment to the rendered
// output instead of that from <Bar>.
ComponentFactories.AddStub<Bar>(@<div>NOT FROM BAR</div>);
```

It is also possible to access the parameter that is passed to the substituted component, both when specifying alternative render output or when verifying the correct parameters was passed to the substituted component. For example, suppose `<Foo>` has a parameter named `Baz`:

```csharp
// Add the markup specified in the template function to the rendered output
// instead of that from <Bar>.
ComponentFactories.AddStub<Bar>(parameters => $"<div>{parameters.Get(x => Baz)}</div>");

// Add the markup produced by the render template to the rendered
// output instead of that from <Bar>.
ComponentFactories.AddStub<Bar>(parameters => @<div>@(parameters.Get(x => Baz))</div>);
```

To verify that the expected value was passed to the `Baz` parameter of `<Foo>`, first find the substituted component in the render tree using the `FindComponent`/`FindComponents` methods, and then inspect the `Parameters` property. E.g.:

```csharp
[Fact]
public void Foo_Doesnt_Have_A_Bar_But_Stub()
{
  ComponentFactories.AddStub<Bar>();

  IRenderedFragment cut = Render(@<Foo />);

  // Find the stubbed component in the render tree
  IRenderedComponent<Stub<Bar>> barStub = cut.FindComponent<Stub<Bar>>();

  // Access parameters passed to it through the stubbed components
  // Parameters property, using the selector to pick out the parameter.  
  var valuePassedToBaz = barStub.Instance.Parameters.Get(x => x.Baz);
  
  // assert valuePassedToBaz is as expected...
}
```

#### Dynamic matching components to stub

To stub more than one component, e.g. all components from a 3rd party component library, pass a `Predicate<Type>` to the `AddStub` method, that returns `true` for all components that should be stubbed. For example:

```csharp
// Stub all components of type `Bar`
ComponentFactories.AddStub(type => type == typeof(Bar));

// Stub all components in the Third.Party.Lib namespace
ComponentFactories.AddStub(type => type.Namespace == "Third.Party.Lib");
```

It is also possible to specify replacement markup or a `RenderFragment` for components substituted using the "component predicate" method:

```csharp
// Add the markup specified in the string to the rendered output
// instead of the components that match the predicate,
ComponentFactories.AddStub(type => type.Namespace == "Third.Party.Lib",
                               "<div>NOT FROM BAR</div>");

// Add the markup produced by the render fragment to the rendered
// output instead of the components that match the predicate.
ComponentFactories.AddStub(type => type.Namespace == "Third.Party.Lib",
                               @<div>NOT FROM BAR</div>);
```

### Creating a mock component with mocking libraries

To get more control over the substituted component or when having a reference to it in the component under test, use a mock created by a mocking library.


Mocking libraries usually offer options of setting up expectations and specify responses to calls made to their methods and properties, as long as these are virtual. 

> [!TIP]
> To learn how to configure a mock object, consult your favorite mocking frameworks documentation.

#### Mocking limitations

Since the standard life-cycle methods in Blazor are all virtual, i.e. `OnInitialized` or `OnAfterRender`, etc., components in Blazor are generally very mock friendly.

However, if a mocked component has a *constructor*, *field* or *property initializers*, or implements `Dispose`/`DisposeAsync`, these will usually not be overridable by the mocking framework and will run when the component is instantiated and disposed.

If that is undesirable, consider creating a wrapper component around the component you wish to mock, or, if you own the component, avoid using a constructor and make use of virtual where ever possible.

#### Mocking examples

Supposed you want to test the `<Foo>` component and substitute out it's child component `<Bar>`:

```cshtml
<Foo>
  <Bar />
</Foo>
```

Here are two examples of using the [Moq](https://github.com/Moq) and [NSubstitute](https://github.com/nsubstitute/NSubstitute) mocking libraries to substitute `<Bar>`:

# [MOQ](#tab/moq)

```csharp
[Fact]
public void Foo_Doesnt_Have_A_Bar_But_Mock()
{
  // Register the mock instance for Bar
  Mock<Bar> barMock = new Mock<Bar>();
  ComponentFactories.Add<Bar>(barMock.Object);
  
  // Render the component under test
  IRenderedFragment cut = Render(@<Foo />);
 
  // Verify that the Bar component has 
  // been substituted in the render tree
  IRenderedComponent<Bar> bar = cut.FindComponent<Bar>();  
  Assert.Same(barMock.Object, bar.Instance);
}
```

Moq the exposes the mocked component instance through the `Object` property.

# [NSubstitute](#tab/nsubstitute)

```csharp
[Fact]
public void Foo_Doesnt_Have_A_Bar_But_Mock()
{
  // Register the mock instance for Bar
  Bar barMock = Substitute.For<FancyParagraph>();
  ComponentFactories.Add<Bar>(barMock);
  
  // Render the component under test
  IRenderedFragment cut = Render(@<Foo />);
 
  // Verify that the Bar component has 
  // been substituted in the render tree
  IRenderedComponent<Bar> bar = cut.FindComponent<Bar>();  
  Assert.Same(barMock, bar.Instance);
}
```

***

> [!WARNING]
> A mock instance can only be used once, i.e. can only be used substitute a single component in the render tree. To substitute more components with one `Add` call on `ComponentFactories`, pass a mock component factory in instead. See below for example.

To mock multiple components of the same type, pass in a mocking component factory:

# [MOQ](#tab/moq)

```csharp
// Register a mock component factory to replace multiple Bar components
ComponentFactories.Add<Bar>(() => Mock.Of<Bar>());
```

# [NSubstitute](#tab/nsubstitute)

```csharp
// Register a mock component factory to replace multiple Bar components
ComponentFactories.Add<Bar>(() => Substitute.For<FancyParagraph>()));
```

***

To mock components conditionally, pass a type predicate to the add method, along with a mock component factory. The mock component factory will be passed the type to create a mock of.

In the example below, an extension method is used to create a mock using Moq with reflection. The example also uses Moq's `MockRepository` type that makes it possible to set up the mocked components separately from when they are created. Other mocking frameworks may need similar helper method:

```csharp
var mockRepo = new MockRepository(MockBehavior.Loose);
ComponentFactories.Add(type => type.Namespace == "Thrid.Party.Lib",
                       type => mockRepo.CreateComponent(type));
```

And this is the extension method that can create mock components dynamically based on a type:

```csharp
// Extension method that can create mock components dynamically
// based on a type.
internal static class MockRepositoryExtensions
{
	private static readonly MethodInfo CreateMethodInfo = typeof(MockRepository)
		.GetMethod(nameof(MockRepository.Create), Array.Empty<Type>());

	public static IComponent CreateComponent(this MockRepository repository, Type type)
	{
		var genericCreateMethod = CreateMethodInfo.MakeGenericMethod(type);
		var mock = (Mock)genericCreateMethod.Invoke(repository, null);
		return (IComponent)mock.Object;
	}
}
```

### Shallow rendering

A popular technique in JavaScript-based frontend testing is "shallow rendering". 

> _"Shallow rendering lets you render a component "one level deep" and assert facts about what its render method returns, without worrying about the behavior of child components, which are not instantiated or rendered"._ -- [React.js docs](https://reactjs.org/docs/shallow-renderer.html).

This is possible in bUnit as well, using the type predicate technique discussed above. For example, to shallow render `<Foo>` using the built-in stub in bUnit, do the following:

```csharp
ComponentFactories.AddStub<Foo>(type => type != typeof(Foo));
```

This will tell bUnit to stub out all components in the render tree that is NOT `<Foo>`. This can also be achieved using a mocking framework. See the example in the previous section above for how to dynamically create component mocks using Moq.
