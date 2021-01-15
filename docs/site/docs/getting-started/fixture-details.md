---
uid: fixture-details
title: Fixture Test Details 
---

# `<Fixture>` Test Details

bUnit's <xref:Bunit.Fixture> component provides different parameters you can set to change the behavior of the test. It also allows you to set up both a component under test and additional fragments that can be used in the test. Fragments are a way to define additional markup that is needed in a test, for example to serve as an expected value in an assertion.

> [!WARNING]
> Razor tests, where <xref:Bunit.Fixture> components are used, are currently only compatible with using xUnit as the general-purpose testing framework.

## Parameters

All the parameters the <xref:Bunit.Fixture> component supports are shown in the listing below:

[!code-cshtml[](../../../samples/tests/razor/AllFixtureParameters.razor)]

**Setup and Test Methods:**

Let us start by looking at the parameters that take a method as input first [__AP: do you mean that we'll first look at \<blah\> or that we'll look first at parameters that take a method as input?__]. The methods are called in the order they are listed below, and should be used for the described purpose:

1. **<xref:Bunit.RazorTesting.FixtureBase`1.Setup>** and **<xref:Bunit.RazorTesting.FixtureBase`1.SetupAsync>:**  
The `Setup` and `SetupAsync` methods are called first, and you can provide both if needed. If both are provided, `Setup` is called first.   
These are usually used to configure the <xref:Bunit.ITestContext.Services> collection of the <xref:Bunit.Fixture> component before the component under test or any fragments are rendered.
2. **<xref:Bunit.RazorTesting.FixtureBase`1.Test>** or **<xref:Bunit.RazorTesting.FixtureBase`1.TestAsync>:**  
 The `Test` or `TestAsync` method is called after the setup methods.   
  _One, and only one_ of the test methods can be specified per fixture. Use the test method to access the component under test and any fragments defined in the fixture, and interact and assert against them.
  
In the example above, the setup and test methods are declared in a `@code { }` block nested inside the <xref:Bunit.Fixture> component. This visually groups the methods nicely with the <xref:Bunit.Fixture> component, making it easier to see what parts of a test belong together, especially when you have multiple tests inside the same test component. 

You can place the methods anywhere you want inside the test component, which can be useful. For example, if you have the same setup steps for multiple tests, you can avoid code duplication by placing them in a common setup method that can be shared by the tests in the same test component.

TODO EGIL [__AP: <== !!__]

**Other parameters**

There are other parameters that affect how the test runs, and how it is displayed in something like Visual Studio's Test Explorer:

1. **<xref:Bunit.RazorTesting.RazorTestBase.Description>:**   
   If a description is provided, it will be displayed by the test runner when the test runs, and in Visual Studio's Test Explorer. If no description is provided, the name of the provided test method is used.
2. **<xref:Bunit.RazorTesting.RazorTestBase.Skip>:**  
   If the skip parameter is provided, the test is skipped and the text entered in the skip parameter is passed to the test runner as the reason to skip the test.
3. **<xref:Bunit.RazorTesting.RazorTestBase.Timeout>:**  
   If provided, the test runner will terminate the test after the specified amount of time if it has not completed already.

## `<ComponentUnderTest>` and `<Fragment>`

The <xref:Bunit.Fixture> component only accepts the <xref:Bunit.ComponentUnderTest> and <xref:Bunit.Fragment> components as its child content. All other components and markup are ignored. For example:

[!code-cshtml[](../../../samples/tests/razor/FixtureWithCutAndFragments.html)]

Here are the rules for the <xref:Bunit.Fixture> component’s child content:

1. One <xref:Bunit.ComponentUnderTest> component must be added, and it should not be empty.
2. Zero or more <xref:Bunit.Fragment> components can be added.
3. The order in which the <xref:Bunit.ComponentUnderTest> and <xref:Bunit.Fragment> components are added does not matter.
4. The <xref:Bunit.ComponentUnderTest> and <xref:Bunit.Fragment> components can contain both Razor markup and regular HTML markup.
5. If more than one <xref:Bunit.Fragment> component is added, give each fragment an `Id` to be able to identify them when retrieving them in the test method.
6. The first <xref:Bunit.Fragment> component added can always be retrieved without an id.

## Using `<ComponentUnderTest>` and `<Fragment>` in the Test Methods

The <xref:Bunit.Fixture>'s setup and test methods receive the <xref:Bunit.Fixture> instance as input when they are called. It is through the <xref:Bunit.Fixture> instance that we can get the component under test and any fragments declared inside it. The relevant methods come in both generic and non-generic variants:

- **<xref:Bunit.Fixture.GetComponentUnderTest>:**  
  Use this to return an <xref:Bunit.IRenderedFragment>, which represents the content declared inside the <xref:Bunit.ComponentUnderTest> component. A <xref:Bunit.IRenderedFragment> does not give you access to the instance of the component under test, but it does give you access to the rendered markup. 

- **<xref:Bunit.Fixture.GetComponentUnderTest``1>:**  
  Use this to return an <xref:Bunit.IRenderedComponent`1>, which represents a component of type `TComponent` declared inside the <xref:Bunit.ComponentUnderTest> component. The <xref:Bunit.IRenderedComponent`1> gives you access to the `TComponent` instance, as well as the rendered markup of it.

- **<xref:Bunit.Fixture.GetFragment(System.String)>:**  
  Use this to get a <xref:Bunit.IRenderedFragment>, which represents the content declared inside the <xref:Bunit.Fragment> component. 

- **<xref:Bunit.Fixture.GetFragment``1(System.String)>**  
  Use this to return an <xref:Bunit.IRenderedComponent`1>, which represents a component of type `TComponent` declared inside the <xref:Bunit.Fragment> component.

For both `GetFragment` methods, the `id` string parameter is optional. If it is not provided, the first <xref:Bunit.Fragment> is used to return a <xref:Bunit.IRenderedFragment> or <xref:Bunit.IRenderedComponent`1>. Otherwise, the <xref:Bunit.Fragment> with an `Id` parameter that matches the `id` specified in the `GetFragment` method call will be used.

The generic versions of <xref:Bunit.Fixture.GetComponentUnderTest``1> and <xref:Bunit.Fixture.GetFragment``1(System.String)> can specify a component of type `TComponent` which is not the first child of <xref:Bunit.ComponentUnderTest> or <xref:Bunit.Fragment>. This is useful in situations such as the component under test being wrapped inside a `<CascadingValue>`. The methods [__AP: method or methods?__] will return the _first_ component it [__AP: it or they?__] finds that matches the requested type through a depth-first search of the render tree.

> [!NOTE]
> You can call the `GetComponentUnderTest` or `GetFragment` methods multiple times on the same `Fixture` instance. Each call will return the same instance for the same input. However, you cannot mix the generic and non-generic versions.

## `<SimpleTodo>` Test Example

Let's look at an example of an elaborate test that tests the lifecycle of a simple task list component, `<SimpleTodo>` (listed below), which has a service injected, receives a cascading value, and changes between renders:

[!code-cshtml[SimpleTodo.razor](../../../samples/components/SimpleTodo.razor)]

In the test, we want to verify that:

- The `<form>` resets itself correctly after a task has been added
- The task is added correctly to the task list
- The "Theme" cascading value is correctly assigned to the task list

The test looks like this:

[!code-cshtml[SimpleTodoTest.razor](../../../samples/tests/razor/SimpleTodoTest.razor?highlight=4,5,8-10,13,20,29,30,35-37)]

Let's look at what's going on in this test:

1. The fixture has both a setup and test method specified. The setup method is used to register an empty list of tasks that the `<SimpleTodo>` component requires.
2. The `<SimpleTodo>` component is wrapped in a `<CascadingValue>` component which passes down the "Theme" cascading value.
3. The first `<Fragment>` does not have an `id`, since the `GetFragment()` method will pick the first fragment if no `id` is provided.
4. The second `<Fragment Id="expected tasks">` does have an `Id`, to make it possible to get the second fragment through a call to the `GetFragment("expected tasks")` method. [__AP: Id or id?__]
5. The test uses the generic version of `GetComponentUnderTest<SimpleTodo>()`, which gives us access to the instance of `SimpleTodo` and allows us to inspect its properties, e.g. `ThemeClass`.

This covers the “arrange”-steps in the test above and shows how you can easily access both the component under test and other fragments that you might need to write your test concisely. We will cover the details of the "act" and "assertion" steps in the <xref:interaction> and <xref:verification> pages. 

Learn more about injecting services into components under test on the <xref:inject-services> page. 
<!--stackedit_data:
eyJoaXN0b3J5IjpbLTEwNTQ2MTE0MTYsLTEyODc0NTI4MzEsLT
E5OTE3NTMxMzAsLTE4NzkxMTEzMTksLTE4Mjk1OTMzMTRdfQ==

-->