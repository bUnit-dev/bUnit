---
uid: writing-razor-tests
title: Writing Tests in Razor Syntax for Blazor Components
---

# Writing Tests in Razor Syntax for Blazor Components

A test for a Blazor component can be written in a Blazor _test_ component using a mix of Razor and C# syntax. The advantage of this is the familiarity in declaring the component under test, and other HTML or Razor fragments that will be used in the test, _in Razor and HTML markup_. This is especially useful when testing components that take a lot of parameters and child content as input.

> [!NOTE]
> Tests written in Blazor test components can be discovered and invoked individually, and will show up in Visual Studio's Test Explorer, for example, just like regular unit tests. 
> 
> However, they will _not_ show up before the Blazor test component has been compiled into C# by the Blazor compiler, and if there are compile-errors from the Blazor compiler, they might appear to come and go in the Test Explorer.

> [!WARNING]
> Razor tests are currently only compatible with using xUnit as the general-purpose testing framework.

> [!IMPORTANT]
> Make sure the project SDK type is set to `<Project Sdk="Microsoft.NET.Sdk.Razor">`, instead of the default `<Project Sdk="Microsoft.NET.Sdk">` that is used with standard testing projects. See <xref:create-test-project> for a guide to setting up bUnit test projects.

## Creating a Test-Specific `_Imports.razor` File

Razor tests are written in Blazor test components. To make life a little easier, let’s first set up an `_Imports.razor` file with the "using directives" we are going to be using throughout our tests. Simply add the following `_Imports.razor` to the root folder where you will be placing your Blazor test components:

[!code-cshtml[_Imports.razor](../../../samples/tests/razor/_Imports.razor#L4-)]

With that created, we are ready to create our first Razor test.

## Creating a Blazor Test Component

A Blazor test component is conceptually very similar to a regular test class in xUnit or NUnit, for example. You can define multiple tests inside a single test component as long as they are based on the special bUnit test components, currently either <xref:Bunit.Fixture> or <xref:Bunit.SnapshotTest>. 

In addition to that, Blazor test components have to inherit from <xref:Bunit.TestComponentBase>, e.g.:

[!code-cshtml[](../../../samples/tests/razor/HelloWorldTest.razor#L1)]

The following two sections will show you how to create tests using bUnit's <xref:Bunit.Fixture> and <xref:Bunit.SnapshotTest> components.

### Creating a Test using the `<Fixture>` Component

Let's see a simple example where we test the following `<HelloWorld>` component using the bUnit <xref:Bunit.Fixture> component:

[!code-cshtml[HelloWorld.razor](../../../samples/components/HelloWorld.razor)]

Here's the Razor code that tests the `<HelloWorld>` component:

[!code-cshtml[HelloWorldTest.razor](../../../samples/tests/razor/HelloWorldTest.razor#L1-L19)]

Let's break down what is going on in this test:

- The test component inherits from <xref:Bunit.TestComponentBase>. This is done in line 1 with `@inherits Bunit.TestComponentBase`.
- The test is defined using the <xref:Bunit.Fixture> component. It orchestrates the test.
- Inside the <xref:Bunit.Fixture> component, we add a <xref:Bunit.ComponentUnderTest> component where the component under test is declared using regular Razor syntax. In this case, it's a very simple `<HelloWorld />` declaration.
- The <xref:Bunit.Fixture> component's `Test` parameter takes a method which is called when the test runs, and is passed  to the <xref:Bunit.Fixture> component.
- In the test method, we use the <xref:Bunit.Fixture.GetComponentUnderTest``1> to get the `HelloWorld` declared in the <xref:Bunit.Fixture>. In addition, we verify the rendered markup from the `HelloWorld` component using the `MarkupMatches` method. This performs a semantic comparison of the expected markup with the rendered markup.

> [!TIP]
> To learn more about how the semantic HTML/markup comparison works in bUnit, as well as how to customize it, visit the <xref:semantic-html-comparison> page.

> [!TIP]
> In bUnit tests, we like to use the abbreviation `CUT`, short for "component under test", to indicate the component that is being tested. This is inspired by the common testing abbreviation `SUT`, short for "system under test".  

### Creating a Test using the `<SnapshotTest>` Component

In snapshot testing, you declare your input (e.g. one or more components under test) and the expected output, and the library will automatically tell you if they do not match. With bUnit, this comparison is done using smart built-in semantic HTML comparison logic.

Let's see a simple example where we test the following `<HelloWorld>` component using the bUnit <xref:Bunit.SnapshotTest> component:

[!code-cshtml[HelloWorld.razor](../../../samples/components/HelloWorld.razor)]

Here is the Razor code that tests the `<HelloWorld>` component:

[!code-cshtml[HelloWorldTest.razor](../../../samples/tests/razor/HelloWorldTest.razor?range=1-2,21-28)]

Let's break down what is going on in this test with the <xref:Bunit.SnapshotTest> component:

- We specify the `Description` parameter. The text it contains will be shown when the test runs and in the Test Explorer in Visual Studio, just like regular unit tests names.
- Inside the `<TestInput>` child component of <xref:Bunit.SnapshotTest> we declare the component under test. In this case, it is the `<HelloWorld>` component.
- Inside the `<ExpectedOutput>` child component of <xref:Bunit.SnapshotTest> we declare the expected rendered output from whatever is declared in the `<TestInput>` child component.

When the test runs, the <xref:Bunit.SnapshotTest> component will automatically compare the rendered output of the `<TestInput>` component with that of the `<ExpectedOutput>` component using the semantic HTML comparison logic in bUnit.

> [!TIP]
> Learn more about how the semantic HTML/markup comparison in bUnit works, and how to customize it, on the <xref:semantic-html-comparison> page.

### Passing Parameters to Components Under Test

Since we are declaring our component under test in Razor syntax, passing parameters to the component under test is the same as passing parameters in normal Blazor components. This is the same for tests created with both the <xref:Bunit.Fixture> and <xref:Bunit.SnapshotTest> components.

In this example, we are passing both attribute parameters and child content to the component under test. In this case, this is a basic `<Alert>` component:

[!code-cshtml[](../../../samples/tests/razor/PassingParametersToComponents.razor)]

Injecting services into the components under test is covered on the <xref:inject-services> page.

## Further Reading

Now that we have covered the basics of writing tests using Razor syntax, you can continue digging deeper. Here are some good places to start:

- <xref:fixture-details>
- <xref:snapshottest-details>
- <xref:inject-services>
- <xref:verify-markup>
- <xref:verify-component-state>
- <xref:trigger-event-handlers>
<!--stackedit_data:
eyJoaXN0b3J5IjpbMTMxMjU2NTcyNywtMjkyMjQwNzg2LDExMT
Y0OTc1MTJdfQ==
-->