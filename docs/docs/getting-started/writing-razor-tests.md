---
uid: writing-razor-tests
title: Writing Tests in Razor Syntax for Blazor Components
---

# Writing Tests in Razor Syntax for Blazor Components

> [!WARNING]
> Razor tests are currently only compatible with using xUnit as the general purpose testing framework.

A test for a Blazor component can be written in a Blazor component, using a mix of Razor and C# syntax. The advantage of this is the familiarity in declaring the component under test, and other HTML or Razor fragments that will be used in the test, _in Razor and HTML markup_. This is especially useful when testing components that takes a lot of parameters and child content as input.

> [!INFO]
> Tests declared inside Blazor test components can be discovered and invoked individually, and will show up in e.g. Visual Studio's Test Explorer. 
> 
> However, they will _not_ show up before the Blazor test component has been compiled into C# by the Blazor compiler, and if there are compile-errors from the Blazor compiler, they might appear to come and go in the Test Explorer.

## Create a Test Specific `_Imports.razor` File

Razor tests are written in Blazor test components. To make our life's a little easier, let us first set up a `_Imports.razor` file, with the using statements we are going to be using throughout our tests. Simply add the following `_Imports.razor` to the root folder where you will be placing your Blazor test components:

[!code-html[_Imports.razor](../../samples/tests/razor/_Imports.razor#L3-)]

With that created, we are ready to create our first Razor test.

## Creating a Blazor Test Component

A Blazor test component is conceptually very similar to a regular test class in e.g. xUnit or NUnit. You can define multiple tests inside a single test component, as long as they are based on the special bUnit test components, currently either <xref:Bunit.Fixture> or <xref:Bunit.SnapshotTest>. 

Besides that, Blazor test components has to inherit from  <xref:Bunit.TestComponentBase>, e.g.:

[!code-html[](../../samples/tests/razor/HelloWorldTest.razor#L1)]

The following two sections will show how to create tests using bUnit's <xref:Bunit.Fixture> and <xref:Bunit.SnapshotTest> components.

### Creating a Test using the `<Fixture>` Component

Let's see a simple example, where we test the following `<HelloWorld>` component using the bUnit <xref:Bunit.Fixture> component:

[!code-html[HelloWorld.razor](../../samples/components/HelloWorld.razor)]

Here is the Razor code that tests the `<HelloWorld>` component:

[!code-html[HelloWorldTest.razor](../../samples/tests/razor/HelloWorldTest.razor#L1-L19)]

Let's break down what is going on in this test:

- The test component inherits from <xref:Bunit.TestComponentBase>. This is done in line 1 with `@inherits Bunit.TestComponentBase`.
- The test is defined using the <xref:Bunit.Fixture> component. It orchestrates the test.
- Inside the <xref:Bunit.Fixture> component, we add a <xref:Bunit.ComponentUnderTest> component, where the component under test is declared using regular Razor syntax. In this case, it is a very simple `<HelloWorld />` declaration.
- The <xref:Bunit.Fixture> component's `Test` parameter takes a method, which is called when the test runs, and is passed the <xref:Bunit.Fixture> component.
- In the test method, we:
  - Use the <xref:Bunit.Fixture.GetComponentUnderTest``1> to get the `HelloWorld` declared in the <xref:Bunit.Fixture>.
  - Verify the rendered markup from the `HelloWorld` component using the <xref:Bunit.MarkupMatchesAssertExtensions.MarkupMatches> method, which performs a semantic comparison of the expected markup with the rendered markup.

> [!TIP]
> Learn more about how the semantic HTML/markup comparison in bUnit work, and how to customize it on the <xref:semantic-html-comparison> page.

> [!TIP]
> In bUnit tests, we like to use the abbreviation `CUT`, short for "component under test", to indicate the component that is being tested. This is inspired by the common testing abbreviation `SUT`, short for "system under test".

### Creating a Test using the `<SnapshotTest>` Component

In snapshot testing, you declare your input (e.g. one or more component under test) and the expected output, and the library will automatically tell you if they do not match. With bUnit, this comparison is done using a smart built-in semantic HTML comparison logic.

Let's see a simple example, where we test the following `<HelloWorld>` component using the bUnit <xref:Bunit.SnapshotTest> component:

[!code-html[HelloWorld.razor](../../samples/components/HelloWorld.razor)]

Here is the Razor code that tests the `<HelloWorld>` component:

[!code-html[HelloWorldTest.razor](../../samples/tests/razor/HelloWorldTest.razor#L21-L28)]

Let's break down what is going on in this test with the <xref:Bunit.SnapshotTest> component:

- We specify the `Description` parameter. The text in that will be shown when test runs and in the Test Explorer in Visual Studio, just like regular unit tests names.
- Inside the `<TestInput>` child component of <xref:Bunit.SnapshotTest>, we declare the component under test, in this case the `<HelloWorld>` component.
- Inside the `<ExpectedOutput>` child component of <xref:Bunit.SnapshotTest>, we declare the expected rendered output from whatever is declared in the `<TestInput>` child component.

When the test runs, the <xref:Bunit.SnapshotTest> component will automatically compare the rendered output of the `<TestInput>` component with that of the `<ExpectedOutput>` component, using the semantic HTML comparison logic in bUnit.

> [!TIP]
> Learn more about how the semantic HTML/markup comparison in bUnit work, and how to customize it on the <xref:semantic-html-comparison> page.

### Passing Parameters to Components Under Test

Since we are declaring our component under test in Razor syntax, passing parameters to the component under test is the same as passing parameters in normal Blazor components. This is the same for tests created with both the <xref:Bunit.Fixture> and <xref:Bunit.SnapshotTest> components.

In this example, we are passing both attribute parameters and child content to the component under test, in this case, a basic `<Alert>` component:

[!code-html[HelloWorldTest.razor](../../samples/tests/razor/PassingParametersToComponents.razor#L3-L18)]

Injecting services into the components under test is covered on the <xref:inject-services-into-components> page.

## Further Reading

Now that we have covered the basics of writing tests using Razor syntax, you can continue digging deeper, e.g. by looking at:

- <xref:fixture-options>
- <xref:snapshot-options>
- <xref:inject-services-into-components>
- <xref:verify-markup>
- <xref:verify-component-state>
- <xref:trigger-event-handlers>