---
uid: fixture-details
title: Fixture Test Details
---

# `<Fixture>` Test Details

bUnit's <xref:Bunit.Fixture> component provides different parameters you can set on it, that changes the behavior of the test. It also allows you to both set up a component under test, and additional fragments, that can be used in the test.

> [!WARNING]
> Razor tests, where <xref:Bunit.Fixture> components are used, are currently only compatible with using xUnit as the general purpose testing framework.

## Parameters

All the parameters the <xref:Bunit.Fixture> component support is shown in the listing below:

[!code-html[](../../samples/tests/razor/AllFixtureParameters.razor)]

**Setup and Test methods:**

Let us start by looking at the parameters that takes a method as input first. The methods are called in the order they are listed in below, if provided, and should be used to the following:

1. **<xref:Bunit.RazorTesting.FixtureBase`1.Setup>** and **<xref:Bunit.RazorTesting.FixtureBase`1.SetupAsync>:**  
   The `Setup` and `SetupAsync` methods are called first, and you can provide both if needed. If both are provided, `Setup` is called first.   
  They are usually used to configure the <xref:Bunit.ITestContext.Services> collection of the <xref:Bunit.Fixture> component before the component under test or any fragments are rendered.
2. **<xref:Bunit.RazorTesting.FixtureBase`1.Test>** or **<xref:Bunit.RazorTesting.FixtureBase`1.TestAsync>:**  
  The `Test` or `TestAsync` method is called after the setup methods.   
  _One, and only one_ of the test methods can be specified per fixture. Use the test method to access the component under test and any fragments defined in the fixture and interact and assert against them.
  
In the example above, the setup and test methods are declared in a `@code { }` block nested inside the <xref:Bunit.Fixture> component. This visually groups the methods nicely to the <xref:Bunit.Fixture> component, but it's not a requirement. 

You can have the methods anywhere inside the test component you want, which can be useful, if you for example have a common setup method that multiple Razor tests in the same test component shares. The methods can also be declared in the parameter directly, e.g.: `<Fixture Setup="f => f.Services.AddMockJsRuntime()" ...>`.

> [!TIP]
> Learn more about mocking and `AddMockJsRuntime()` on the <xref:mocking-ijsruntime> page.

**Other parameters**

The other parameters affect how the test runs, and how it is displayed in e.g. Visual Studio's Test Explorer:

1. **<xref:Bunit.RazorTesting.RazorTestBase.Description>:**   
   If a description is provided, it will be displayed by the test runner when the test runs, and in Visual Studio's Test Explorer. If no description is provided, the name of the provided test method is used.
2. **<xref:Bunit.RazorTesting.RazorTestBase.Skip>:**  
   If the skip parameter is provided, the test is skipped, and the text entered in the skip parameter is passed to the test runner as the reason to skip the test.
3. **<xref:Bunit.RazorTesting.RazorTestBase.Timeout>:**  
   If provided, the test runner will terminate the test after the specified amount of time, if it has not completed already.

## `<ComponentUnderTest>` and `<Fragment>`

The <xref:Bunit.Fixture> component only accepts the <xref:Bunit.ComponentUnderTest> and <xref:Bunit.Fragment> components as its child content. All other components and markup are ignored. E.g.:

[!code-html[](../../samples/tests/razor/FixtureWithCutAndFragments.html)]

Here are the rules for the <xref:Bunit.Fixture> components child content:

1. One <xref:Bunit.ComponentUnderTest> component must be added, and it should not be empty.
2. Zero or more <xref:Bunit.Fragment> components can be added.
3. The order the <xref:Bunit.ComponentUnderTest> and <xref:Bunit.Fragment> components are added in does not matter.
4. The <xref:Bunit.ComponentUnderTest> and <xref:Bunit.Fragment> components can contain both Razor markup and regular HTML markup.
5. If more than one <xref:Bunit.Fragment> component is added, give each fragment an `Id` to be able to identify them when retrieving them in the test method.
6. The first <xref:Bunit.Fragment> component is added can always be retrieved without an id.

## Getting `<ComponentUnderTest>` and `<Fragment>` in the Test Methods

The <xref:Bunit.Fixture>'s setup and test methods receives the <xref:Bunit.Fixture> instance as input when they are called. It is through it, the <xref:Bunit.Fixture> instance, we can get the component under test and any fragments declared inside it. The relevant methods comes in both a generic and non-generic variants:

- **<xref:Bunit.Fixture.GetComponentUnderTest>:**  
  Use this to return an <xref:Bunit.IRenderedFragment>, which represents the content declared inside the <xref:Bunit.ComponentUnderTest> component. A <xref:Bunit.IRenderedFragment> does not give you access to the instance of the component under test, but it does give you access to the rendered markup. 

- **<xref:Bunit.Fixture.GetComponentUnderTest``1>:**  
  Use this to return an <xref:Bunit.IRenderedComponent`1>, which represents a component of type `TComponent` declared inside the <xref:Bunit.ComponentUnderTest> component. The <xref:Bunit.IRenderedComponent`1> does give you access to the `TComponent` instance, as well as the rendered markup of it.

- **<xref:Bunit.Fixture.GetFragment(System.String)>:**  
  Use this to get a <xref:Bunit.IRenderedFragment>, which represents the content declared inside the <xref:Bunit.Fragment> component. 

- **<xref:Bunit.Fixture.GetFragment``1(System.String)>**  
  Use this to return an <xref:Bunit.IRenderedComponent`1>, which represents a component of type `TComponent` declared inside the <xref:Bunit.Fragment> component.

For both the `GetFragment` methods the `id` string parameter is optional. If it is not provided, the first <xref:Bunit.Fragment> is used to return a <xref:Bunit.IRenderedFragment> or <xref:Bunit.IRenderedComponent`1>. Otherwise, the <xref:Bunit.Fragment> with an `Id` parameter that matches the `id` specified in the `GetFragment` method call will be used.

The generic versions of <xref:Bunit.Fixture.GetComponentUnderTest``1> and <xref:Bunit.Fixture.GetFragment``1(System.String)> can specify a component of type `TComponent` that is not the first child of <xref:Bunit.ComponentUnderTest> or <xref:Bunit.Fragment>. This is useful if e.g. the component under test is wrapped inside a `<CascadingValue>`. The methods will return the _first_ component it finds that matches the requested type, through a depth-first search of the render tree.

> [!NOTE]
> You can call the `GetComponentUnderTest` or `GetFragment` methods multiple times on the same `Fixture` instance. Each time will return the same instance for the same input. However, you cannot mix the generic and non-generic versions.

## Example

Let's look at a complete example, where we have a simple task list component, `<SimpleTodo>`, listed below, that have a service injected, receive a cascading value, and changes between renders:

[!code-html[SimpleTodo.razor](../../samples/components/SimpleTodo.razor)]

In the test, we want to verify that:

- The `<form>` resets itself correctly after a task has been added
- That the task was added correctly to the task list
- That the "Theme" cascading value was correctly assigned to the task list

The test looks like this:

[!code-html[SimpleTodoTest.razor](../../samples/tests/razor/SimpleTodoTest.razor?highlight=4,5,8-10,13,20,29,30,35-37,44)]

Let's look at whats going on in this test:

1. The fixture has both a setup and test method specified. The setup methods is used to register an empty list of tasks, that the `<SimpleTodo>` component requires.
2. The `<SimpleTodo>` component is wrapped in a `<CascadingValue>` component that passes down the "Theme" cascading value.
3. The first `<Fragment>` does not have an `Id`, since the `GetFragment()` method will pick the first fragment, if no `id` is provided to it.
4. The second `<Fragment Id="expected tasks">` does have an `Id` to make it possible to get it, through a call to the `GetFragment("expected tasks")` method.
5. The test uses the generic version of `GetComponentUnderTest<SimpleTodo>()`, which gives us access to the instance of `SimpleTodo`, and allows us to inspect its properties, e.g. `ThemeClass`.

We will cover the details of the "act" and "assertion" step in the <xref:interaction> and <xref:verification> pages. 

Learn more about injecting services into components under test on the <xref:inject-services-into-components> page.