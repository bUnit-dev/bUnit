# Razor-based testing

This pages documents how to do Blazor/Razor component testing from `.razor` files.

Before you get started, make sure you have read the [Getting started](/docs/getting-started.html) page and in particular the [Basics of Blazor component testing](/docs/basics-of-blazor-component-testing.html) section. It wont take long, and it will ensure you get a good start at component testing.

> **NOTE:** This feature is _EXPERIMENTAL_ and syntax and API will likely changed. Here are a few limitations to be aware of at the moment:
>
> - The xUnit test runner can detect and execute tests in Razor test components, but is not able to distinguish the individual `<Fixture>`'s from each other. They are all executed together, one at the time. The solution is planned, see the [related issue](https://github.com/egil/razor-components-testing-library/issues/4) for details.
> - Go to the [Contribute](/docs/contribute) page for info on how to provide feedback and suggestions.

> **TIP:** Working with and asserting against the rendered component and its output is covered on the [Working with rendered components and fragments](/docs/working-with-rendered-components-and-fragments.html) page.

**Content:**

- [Creating a new Razor test component](#creating-a-new-razor-test-component)
- [Defining tests/fixtures in test components](#defining-testsfixtures-in-test-components)
- [Executing test cases](#executing-test-cases)

**Further reading:**

- [Working with rendered components and fragments](/docs/working-with-rendered-components-and-fragments.html)
- [Semantic HTML markup comparison](/docs/semantic-html-markup-comparison.html)
- [Mocking JsRuntime](/docs/mocking-jsruntime.html)
- [Razor test examples](/docs/razor-test-examples.html)

## Creating a new Razor test component

To create Razor based tests, we need to create test components. All test components must inherit from `TestComponentBase`, e.g. by adding `@inherits TestComponentBase` to the top of your .razor file. The `TestComponentBase` contains all the logic for rendering components and correctly dispose of renderers, components, and HTML parsers after each test.

For example:

```cshtml
@inherits TestComponentBase

<Fixture Test="Test1">
  <ComponentUnderTest>
    <MyComponent />
  </ComponentUnderTest>
</Fixture>
@code {
  void Test1()
  {
    // assert and verification
  }
}
```

You will also need to import a few namespaces to make asserting and mocking possible. They are best placed in an `_Imports.razor` file next to your Razor test components, e.g.:

```cshtml
@using Microsoft.AspNetCore.Components.Web
@using Microsoft.Extensions.DependencyInjection
@using Bunit
@using Bunit.Mocking.JSInterop
@using Xunit
```

> **NOTE:** The `_Imports.razor` has already been created for you if you are using the [Blazor test project template](/docs/creating-a-new-test-project.html).

## Defining tests/fixtures in test components

When you have a Razor test component created, its time to add test cases/fixtures to it. This is done via the `<Fixture>` component and related test methods and child components.

Lets look at what options we have by setting up an empty test case, first the code:

```cshtml
<Fixture Description="MyComponent renders as expected" @* Optional - description is shown in error message if test fails *@
         Setup="Setup" @* Optional - method called first *@
         SetupAsync="SetupAsync" @* Optional - method called after Setup *@
         Test="Test1" @*  Optional - method called after Setup/SetupAsync *@
         TestAsync="Test1Async" @*  Optional - method called after Test *@
         Tests="new Action[]{ Test2, Test3 }"> @*  Optional - methods are called after Test/TestAsync, one at the time *@
         TestsAsync="new Func<Task>[]{ Test2Async, Test3Async }"> @*  Optional - methods are called after Tests, one at the time *@
  <ComponentUnderTest>
    <MyComponent />
  </ComponentUnderTest>
  <Fragment id="first">
    <h1>First Fragment</h1>
  </Fragment>
  <Fragment id="second">
    <MyOtherComponent />
  </Fragment>
</Fixture>
@code {
  // Called first if present when added to the Setup parameter
  // on a <Fixture> component (can be named anything)
  void Setup()
  {
    // Add services and do other setup work in this method.
    Services.AddMockJsRuntime();
  }

  // Called after Setup if present when added to the Setup parameter
  // on a <Fixture> component (can be named anything)
  Task SetupAsync() => Task.CompletedTask;

  // Called after Setup when added to the Test parameter to a
  // <Fixture> component (can be named anything)
  void Test1()
  {
    // Renders a MyComponent component and assigns the result to
    // a cut variable. CUT is short for Component Under Test.
    IRenderedComponent<MyComponent> cut = GetComponentUnderTest<MyComponent>();

    // Renders the markup in the "first" fragment by calling GetFragment without an id.
    IRenderedFragment firstFragment = GetFragment();

    // Renders the markup in the "first" fragment by calling GetFragment with an id.
    IRenderedFragment alsoFirstFragment = GetFragment("first");

    // Both first fragments refers to the same instance.
    Assert.Equal(firstFragment, alsoFirstFragment);

    // Renders a MyOtherComponent component defined in the second fragment.
    IRenderedComponent<MyOtherComponent> myOtherComponent = GetFragment<MyOtherComponent>("second");
  }

  Task Test1Async() => Task.CompletedTask;

  // Called after Test when added to the Tests parameter to a
  // <Fixture> component (can be named anything). Methods in
  // the Tests parameter is called in the order they are present in the
  // array.
  void Test2()
  {
    // do more testing on CUT, f1 and f2 by retriving them.
  }

  void Test3()
  {
    // do more testing on CUT, f1 and f2 by retriving them.
  }

  Task Test2Async() => Task.CompletedTask;

  Task Test3Async() => Task.CompletedTask;
}
```

The code above works as follows:

- All the `<Fixture>` components defined in the test component is found by the `TestComponentBase`'s test method. For each `Fixture`, it calls the related methods in the following order (if they are present):

  1. `Setup`
  2. `SetupAsync`
  3. `Test`
  4. `TestAsync`
  5. `Tests`, one at the time, in the order they appear in the array.
  6. `TestsAsync`, one at the time, in the order they appear in the array.

- The `Description` parameter on the `<Fixture>` element is displayed in the test runners error window if the test fails.
- It is inside child component `<ComponentUnderTest>` where you declare the component under test.
- Any markup or component fragments that is needed for the test can be declared inside the optional `<Fragment>` components. The `Id` parameter is optional, and only needed if you have more than one.

- To render and get the component under test or any of the fragments, use the `GetComponentUnderTest<TComponent>()` method, where `TComponent` is the type of the component you have defined under the `<ComponentUnderTest>` element.

- `GetFragment()` can be called both with and without a `TComponent`, e.g. if its just markup defined in it. If an `id` is not provided to the `GetFragment` method, the first declared `<Fragment>` is returned.

- Inside the test methods you can do all the things you can in C#-based tests, e.g. assert against the CUT.

## Executing test cases

Since Blazor test component use xUnit under the hood as a test runner, you execute your tests them in exactly the same way as you would normal xUnit unit tests, i.e. by running `dotnet test` from the console or running the tests through the Test Explorer in Visual Studio.

Do note the current limitations mentioned at the top of the page.
