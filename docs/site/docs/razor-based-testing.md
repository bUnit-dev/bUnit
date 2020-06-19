# Razor-based testing

> **WARNINIG**: This feature is only supported when using bUnit with xUNit. NUnit and MSTest is planned.

This pages documents how to do Blazor/Razor component testing from `.razor` files.

## Creating a new Razor test component

To create Razor based tests, we need to create Blazor test components. All test components must inherit from `TestComponentBase`, e.g. by adding `@inherits TestComponentBase` to the top of your .razor file.

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
@using Bunit
@using Bunit.Mocking.JSInterop
@using Xunit
```

> **NOTE:** The `_Imports.razor` has already been created for you if you are using the [Blazor test project template](/docs/creating-a-new-bunit-xunit-project.html).

## Defining tests/fixtures in test components

When you have a Razor test component created, its time to add test cases/fixtures to it. This is done via the `<Fixture>` component and related test methods and child components.

Lets look at what options we have by setting up an empty test case, first the code:

```cshtml
<Fixture Description="MyComponent renders as expected"
         Timeout="1000" 
         Skip="Reason to skip the test"
         Setup="Setup
         SetupAsync="SetupAsync"
         Test="Test1" @* or *@ TestAsync="Test1Async">
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
  void Setup(Fixture fixture)
  {
    // Add services and do other setup work in this method.
    fixture.Services.AddMockJsRuntime();
  }

  // Called after Setup if present when added to the Setup parameter
  // on a <Fixture> component (can be named anything)
  Task SetupAsync(Fixture fixture) => Task.CompletedTask;

  // Called after Setup when added to the Test parameter to a
  // <Fixture> component (can be named anything)
  void Test1(Fixture fixture)
  {
    // Renders a MyComponent component and assigns the result to
    // a cut variable. CUT is short for Component Under Test.
    IRenderedComponent<MyComponent> cut = fixture.GetComponentUnderTest<MyComponent>();

    // Renders the markup in the "first" fragment by calling GetFragment without an id.
    IRenderedFragment firstFragment = fixture.GetFragment();

    // Renders the markup in the "first" fragment by calling GetFragment with an id.
    IRenderedFragment alsoFirstFragment = fixture.GetFragment("first");

    // Both first fragments refers to the same instance.
    Assert.Equal(firstFragment, alsoFirstFragment);

    // Renders a MyOtherComponent component defined in the second fragment.
    IRenderedComponent<MyOtherComponent> myOtherComponent = fixture.GetFragment<MyOtherComponent>("second");
  }

  Task Test1Async(Fixture fixture) => Task.CompletedTask;
}
```

The code above works as follows:

- All the `<Fixture>` components defined in the test component is found by the `TestComponentBase`'s test method. For each `Fixture`, it calls the related methods in the following order (if they are present):

  1. `Setup`
  2. `SetupAsync`
  3. `Test` or `TestAsync`

- If the `Description` parameter is present, it is used as the name of the test in the Test Explorer and is displayed in the test runners error window if the test fails.
- It is inside child component `<ComponentUnderTest>` where you declare the component under test.
- If the `Timeout` parameter is specified, the test will timeout after the specified time, in milliseconds.
- If `Skip` parameter is specified, the test is skipped and the reason is printed in the test output.
- Any markup or component fragments that is needed for the test can be declared inside the optional `<Fragment>` components. The `Id` parameter is optional, and only needed if you have more than one.

- To render and get the component under test or any of the fragments, use the `GetComponentUnderTest<TComponent>()` method, where `TComponent` is the type of the component you have defined under the `<ComponentUnderTest>` element.

- `GetFragment()` can be called both with and without a `TComponent`, e.g. if its just markup defined in it. If an `id` is not provided to the `GetFragment` method, the first declared `<Fragment>` is returned.

- Inside the test methods you can do all the things you can in C#-based tests, e.g. assert against the CUT.

## Executing test cases

Since Blazor test component use xUnit under the hood as a test runner, you execute your tests them in exactly the same way as you would normal xUnit unit tests, i.e. by running `dotnet test` from the console or running the tests through the Test Explorer in Visual Studio.

## Further reading:

- [Semantic HTML markup comparison](/docs/semantic-html-markup-comparison.html)
- [Mocking JsRuntime](/docs/mocking-jsruntime.html)
- [Razor based test examples in the sample project](https://github.com/egil/bunit/tree/main/sample/tests/RazorTestComponents)
