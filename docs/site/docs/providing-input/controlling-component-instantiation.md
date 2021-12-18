---
uid: controlling-component-instantiation
title: Controlling component instantiation
---

# Controlling component instantiation

Components are, by default in bUnit, instantiated in the exact same way the regular Blazor runtime does it. For example, if the component under test has a `<Counter />` component inside it, an instance of the `Counter` class will be created at runtime and added to the render tree below the component under test.

It is however possible to control how Blazor components build using .NET 5 or newer are instantiated by bUnit. This enables the possibility to replace one component during testing with another, e.g., to replace a 3rd party component during testing with a "[stub component](https://en.wikipedia.org/wiki/Test_stub)", to make the test easier to write and maintain.

The following sections will describe how to use component factories to control instantiation of components during testing.

## Using component factories to control instantiation

To take control of how components are instantiated during testing, add one or more <xref:Bunit.IComponentFactory> types to the `ComponentFactories` collection.

The added component factories <xref:Bunit.IComponentFactory.CanCreate(System.Type)> methods will be called in a last- to first-added order, e.g., the last one added will be called first, then the second-to-last, and so on, and the first that returns `true` will have its <xref:Bunit.IComponentFactory.Create(System.Type)> method invoked. If none of the factories can create the requested type, the default Blazor factory will be used.

## Example – replacing `<Foo>` with `<Bar>`

To create a component factory that replaces `<Foo>` with `<Bar>` create the following component factory:

[!code-csharp[FooBarComponentFactory.cs](../../../samples/tests/xunit/FooBarComponentFactory.cs#L3-L14)]

Make sure that the replacement component is compatible with the component it replaces, i.e., has the same parameters. This can be done by copying all parameters from the replacee to the replacer, or by using [attribute splatting and arbitrary parameters](https://docs.microsoft.com/en-us/aspnet/core/blazor/components/#attribute-splatting-and-arbitrary-parameters) in the replacer.

Then, before rendering the component under test, add the `FooBarComponentFactory` to the test context’s component factories collection:

[!code-csharp[ComponentFactoryExampleTest.cs](../../../samples/tests/xunit/ComponentFactoryExampleTest.cs#L9-L27)]

## Built-in factories

bUnit comes with several built-in factories that allow shallow rendering or replacing components with test dummies and test stubs. See the <xref:substituting-components> page for details.
