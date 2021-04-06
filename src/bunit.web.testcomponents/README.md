# Writing Tests using in Razor Syntax for Blazor Components

_**NOTE:** The `bunit.web.testcomponents` project contains an experimental feature that was part of the eariler beta and preview versions of bUnit. It is kept around to not break early adopters, but no additional features or improvements is planned to it. The following text is from the original documentation written for the feature._

---

## Introduction

A test for a Blazor component can be written in a Blazor _test_ component using a mix of Razor and C# syntax. The advantage of this is the familiarity in declaring the component under test, and other HTML or Razor fragments that will be used in the test, _in Razor and HTML markup_. This is especially useful when testing components that take a lot of parameters and child content as input.

> **NOTE:**
> Tests written in Blazor test components can be discovered and invoked individually, and will show up in Visual Studio's Test Explorer, for example, just like regular unit tests. 
> 
> However, they will _not_ show up before the Blazor test component has been compiled into C# by the Blazor compiler, and if there are compile-errors from the Blazor compiler, they might appear to come and go in the Test Explorer.

> **WARNING:**
> Razor tests are only compatible with using xUnit as the general-purpose testing framework.

> **IMPORTANT:**
> Make sure the project SDK type is set to `<Project Sdk="Microsoft.NET.Sdk.Razor">`, instead of the default `<Project Sdk="Microsoft.NET.Sdk">` that is used with standard testing projects. See [Creating a new bUnit Test Project](https://bunit.egilhansen.com/docs/getting-started/create-test-project) for a guide to setting up bUnit test projects.

## Creating a Test-Specific `_Imports.razor` File

Razor tests are written in Blazor test components. To make life a little easier, let’s first set up an `_Imports.razor` file with the "using directives" we are going to be using throughout our tests. Simply add the following `_Imports.razor` to the root folder where you will be placing your Blazor test components:

```razor
@using Microsoft.AspNetCore.Components.Forms
@using Microsoft.AspNetCore.Components.Routing
@using Microsoft.AspNetCore.Components.Web
@using Microsoft.JSInterop
@using Microsoft.Extensions.DependencyInjection
@using AngleSharp.Dom
@using Bunit
@using Bunit.TestDoubles
@using Xunit
```

With that created, we are ready to create our first Razor test.

## Creating a Blazor Test Component

A Blazor test component is conceptually very similar to a regular test class in xUnit or NUnit, for example. You can define multiple tests inside a single test component as long as they are based on the special bUnit test components, currently either `<Fixture>` or `<SnapshotTest>`. 

In addition to that, Blazor test components have to inherit from `TestComponentBase`, e.g.:

```razor
@inherits TestComponentBase
```

The following two sections will show you how to create tests using bUnit's `<Fixture>` and `<SnapshotTest>` components.

### Creating a Test using the `<Fixture>` Component

Let's see a simple example where we test the following `<HelloWorld>` component using the bUnit `<Fixture>` component:

```razor
<h1>Hello world from Blazor</h1>
```

Here's the Razor code that tests the `<HelloWorld>` component:

```razor
@inherits TestComponentBase

<Fixture Test="HelloWorldComponentRendersCorrectly">
  <ComponentUnderTest>
    <HelloWorld />
  </ComponentUnderTest>

  @code
  {
    void HelloWorldComponentRendersCorrectly(Fixture fixture)
    {
      // Act
      var cut = fixture.GetComponentUnderTest<HelloWorld>();

      // Assert
      cut.MarkupMatches("<h1>Hello world from Blazor</h1>");
    }
  }
</Fixture>
```

Let's break down what is going on in this test:

- The test component inherits from `TestComponentBase`. This is done in line 1 with `@inherits Bunit.TestComponentBase`.
- The test is defined using the `<Fixture>` component. It orchestrates the test.
- Inside the `<Fixture>` component, we add a `<ComponentUnderTest>` component where the component under test is declared using regular Razor syntax. In this case, it's a very simple `<HelloWorld />` declaration.
- The `<Fixture>` component's `Test` parameter takes a method which is called when the test runs, and is passed  to the `<Fixture>` component.
- In the test method, we use the `GetComponentUnderTest<TComponent>()` to get the `HelloWorld` declared in the `<Fixture>`. In addition, we verify the rendered markup from the `HelloWorld` component using the `MarkupMatches` method. This performs a semantic comparison of the expected markup with the rendered markup.

> **TIP:**
> To learn more about how the semantic HTML/markup comparison works in bUnit, as well as how to customize it, visit the [Customizing the Semantic HTML Comparison](https://bunit.egilhansen.com/docs/verification/semantic-html-comparison) page.

> **TIP:**
> In bUnit tests, we like to use the abbreviation `CUT`, short for "component under test", to indicate the component that is being tested. This is inspired by the common testing abbreviation `SUT`, short for "system under test".  

### Creating a Test using the `<SnapshotTest>` Component

In snapshot testing, you declare your input (e.g. one or more components under test) and the expected output, and the library will automatically tell you if they do not match. With bUnit, this comparison is done using smart built-in semantic HTML comparison logic.

Let's see a simple example where we test the following `<HelloWorld>` component using the bUnit `<SnapshotTest>` component:

```razor
<h1>Hello world from Blazor</h1>
```

Here is the Razor code that tests the `<HelloWorld>` component:

```razor
@inherits TestComponentBase

<SnapshotTest Description="HelloWorld component renders correctly">
  <TestInput>
    <HelloWorld />
  </TestInput>
  <ExpectedOutput>
    <h1>Hello world from Blazor</h1>
  </ExpectedOutput>
</SnapshotTest>
```

Let's break down what is going on in this test with the `<SnapshotTest>` component:

- We specify the `Description` parameter. The text it contains will be shown when the test runs and in the Test Explorer in Visual Studio, just like regular unit tests names.
- Inside the `<TestInput>` child component of `<SnapshotTest>` we declare the component under test. In this case, it is the `<HelloWorld>` component.
- Inside the `<ExpectedOutput>` child component of `<SnapshotTest>` we declare the expected rendered output from whatever is declared in the `<TestInput>` child component.

When the test runs, the `<SnapshotTest>` component will automatically compare the rendered output of the `<TestInput>` component with that of the `<ExpectedOutput>` component using the semantic HTML comparison logic in bUnit.

> **TIP:**
> Learn more about how the semantic HTML/markup comparison in bUnit works, and how to customize it, on the [Customizing the Semantic HTML Comparison](https://bunit.egilhansen.com/docs/verification/semantic-html-comparison) page.

### Passing Parameters to Components Under Test

Since we are declaring our component under test in Razor syntax, passing parameters to the component under test is the same as passing parameters into normal Blazor components. This is the same for tests created with both the `<Fixture>` and `<SnapshotTest>` components.

In this example, we are passing both attribute parameters and child content to the component under test. In this case, it's a basic `<Alert>` component:

```razor
@inherits TestComponentBase

<Fixture Test="fixture => { }">
  <ComponentUnderTest>
    <Alert Type="AlertType.Warning" Heading="TDD takes practise">
      Before you really get the benefit of TDD, you need to practice...
    </Alert>
  </ComponentUnderTest>
</Fixture>

<SnapshotTest>
  <TestInput>
    <Alert Type="AlertType.Warning" Heading="TDD takes practise">
      Before you really get the benefit of TDD, you need to practice...
    </Alert>
  </TestInput>
  <ExpectedOutput>
    <div diff:ignore>...</div>  
  </ExpectedOutput> 
</SnapshotTest>
```

# `<Fixture>` Test Details

bUnit's `<Fixture>` component provides different parameters you can set to change the behavior of the test. It also allows you to set up both a component under test and additional fragments that can be used in the test. Fragments are a way to define additional markup that is needed in a test, for example to serve as an expected value in an assertion.

## Parameters

All the parameters the `<Fixture>` component supports are shown in the listing below:

```razor
@inherits TestComponentBase

<Fixture Setup=@Setup
         SetupAsync=@SetupAsync
         Test=@Test
         TestAsync=@TestAsync
         Description="Description of test"
         Timeout="TimeSpan.FromSeconds(2)"
         Skip="Reason to skip the test">

  @code
  {
    void Setup(Fixture fixture) { }
    Task SetupAsync(Fixture fixture) => Task.CompletedTask;
    // NOTE: Only one of Test/TestAsync can be used at the same time.
    //       Both are included here for illustration purposes only.
    void Test(Fixture fixture) { }
    Task TestAsync(Fixture fixture) => Task.CompletedTask;
  }
</Fixture>
```

**Setup and Test Methods:**

Let us start by looking at the parameters that take a method or lambda as input. The methods are called in the order they are listed below, and should be used for the described purpose:

1. **`Setup` and `SetupAsync`:**  
The `Setup` and `SetupAsync` methods are called first, and you can provide both if needed. If both are provided, `Setup` is called first.   
These are usually used to configure the `Services` collection of the `<Fixture>` component before the component under test or any fragments are rendered.
2. **`Test` or `TestAsync`:**  
 The `Test` or `TestAsync` method is called after the setup methods.   
  _One, and only one_ of the test methods can be specified per fixture. Use the test method to access the component under test and any fragments defined in the fixture, and interact and assert against them.
  
In the example above, the setup and test methods are declared in a `@code { }` block nested inside the `<Fixture>` component. This visually groups the methods nicely with the `<Fixture>` component, making it easier to see what parts of a test belong together, especially when you have multiple tests inside the same test component. 

You can place the methods anywhere you want inside the test component, which can be useful. For example, if you have the same setup steps for multiple tests, you can avoid code duplication by placing them in a common setup method that can be shared by the tests in the same test component.

**Other parameters**

There are other parameters that affect how the test runs, and how it is displayed in something like Visual Studio's Test Explorer:

1. **`Description`:**   
   If a description is provided, it will be displayed by the test runner when the test runs, and in Visual Studio's Test Explorer. If no description is provided, the name of the provided test method is used.
2. **`Skip`:**  
   If the skip parameter is provided, the test is skipped and the text entered in the skip parameter is passed to the test runner as the reason to skip the test.
3. **`Timeout`:**  
   If provided, the test runner will terminate the test after the specified amount of time if it has not completed already.

## `<ComponentUnderTest>` and `<Fragment>`

The `<Fixture>` component only accepts the `<ComponentUnderTest>` and `<Fragment>` components as its child content. All other components and markup are ignored. For example:

```razor
@inherits TestComponentBase

<Fixture Test="...">
  <ComponentUnderTest>
    <!-- Razor or HTML markup goes here -->
  </ComponentUnderTest>
  <Fragment>
    <!-- Razor or HTML markup goes here -->
  </Fragment>
  <Fragment Id="some id">
    <!-- Razor or HTML markup goes here -->
  </Fragment>
</Fixture>
```

Here are the rules for the `<Fixture>` component’s child content:

1. One `<ComponentUnderTest>` component must be added, and it should not be empty.
2. Zero or more `<Fragment>` components can be added.
3. The order in which the `<ComponentUnderTest>` and `<Fragment>` components are added does not matter.
4. The `<ComponentUnderTest>` and `<Fragment>` components can contain both Razor markup and regular HTML markup.
5. If more than one `<Fragment>` component is added, give each fragment an `Id` to be able to identify them when retrieving them in the test method.
6. The first `<Fragment>` component added can always be retrieved without an id.

## Using `<ComponentUnderTest>` and `<Fragment>` in the Test Methods

The `<Fixture>`'s setup and test methods receive the `<Fixture>` instance as input when they are called. It is through the `<Fixture>` instance that we can get the component under test and any fragments declared inside it. The relevant methods come in both generic and non-generic variants:

- **`IRenderedFragment`:**  
  Use this to return an `IRenderedFragment`, which represents the content declared inside the `<ComponentUnderTest>` component. A `IRenderedFragment` does not give you access to the instance of the component under test, but it does give you access to the rendered markup. 

- **`IRenderedComponent<TComponent>`:**  
  Use this to return an `IRenderedComponent<TComponent>`, which represents a component of type `TComponent` declared inside the `<ComponentUnderTest>` component. The `IRenderedComponent<TComponent>` gives you access to the `TComponent` instance, as well as the rendered markup of it.

- **`GetFragment(String)`:**  
  Use this to get a `IRenderedFragment`, which represents the content declared inside the `<Fragment>` component. 

- **`GetFragment<TComponent>(string)`:**  
  Use this to return an `IRenderedComponent<TComponent>`, which represents a component of type `TComponent` declared inside the `<Fragment>` component.

For both `GetFragment` methods, the `id` string parameter is optional. If it is not provided, the first `<Fragment>` is used to return a `IRenderedFragment` or `IRenderedComponent<TComponent>`. Otherwise, the `<Fragment>` with an `Id` parameter that matches the `id` specified in the `GetFragment` method call will be used.

The generic versions of `GetComponentUnderTest<TComponent>` and `GetFragment<TComponent>(string)` can specify a component of type `TComponent` which is not the first child of `<ComponentUnderTest>` or `<Fragment>`. This is useful in situations such as the component under test being wrapped inside a `<CascadingValue>`. The `GetComponentUnderTest` and `GetFragment` methods will return the _first_ component they find that matches the requested type through a depth-first search of the render tree.

> **NOTE:**
> You can call the `GetComponentUnderTest` or `GetFragment` methods multiple times on the same `Fixture` instance. Each call will return the same instance for the same input. However, you cannot mix the generic and non-generic versions.

## `<SimpleTodo>` Test Example

Let's look at an example of an elaborate test that tests the lifecycle of a simple task list component, `<SimpleTodo>` (listed below), which has a service injected, receives a cascading value, and changes between renders:

```razor
@inject List<string> Tasks

<form>
    <input @bind-value=@newTaskValue placeholder="Add todo here . . ." type="text" />
    <button type="submit" @onclick=@HandleTaskAdded>Add task</button>
</form>
<ul id="tasks" class=@ThemeClass>
    @foreach (var task in Tasks)
    {
        <li>@task</li>
    }
</ul>
@code {
  [CascadingParameter(Name = "Theme")]
  public string ThemeClass { get; set; } = string.Empty;

  private string newTaskValue = string.Empty;

  private void HandleTaskAdded()
  {
    if (!string.IsNullOrWhiteSpace(newTaskValue))
      Tasks.Add(newTaskValue);

    newTaskValue = string.Empty;
  }
}
```

In the test, we want to verify that:

- The `<form>` resets itself correctly after a task has been added
- The task is added correctly to the task list
- The "Theme" cascading value is correctly assigned to the task list

The test looks like this:

```razor
@inherits TestComponentBase

<Fixture Description="When a task is added, then the input field gets reset"
         Setup="RegisterTasksService"
         Test="WhenTaskIsAddedInputGetsReset">

  <ComponentUnderTest>
    <CascadingValue Name="Theme" Value=@("dark-theme")>
      <SimpleTodo></SimpleTodo>
    </CascadingValue>
  </ComponentUnderTest>

  <Fragment>
    <form>
      <input placeholder="Add todo here . . ." type="text" value="" />
        <button type="submit">Add task</button>
      </form>
  </Fragment>

  <Fragment Id="expected tasks">
    <ul class="dark-theme" id="tasks">
      <li>Existing task</li>
      <li>FOO BAR BAZ</li>
    </ul>
  </Fragment>

  @code
  {
    void RegisterTasksService(Fixture fixture)
      => fixture.Services.AddSingleton(new List<string>{ "Existing task" });

    void WhenTaskIsAddedInputGetsReset(Fixture fixture)
    {
      // Arrange - get the component under test and fragments
      IRenderedComponent<SimpleTodo> cut = fixture.GetComponentUnderTest<SimpleTodo>();
      IRenderedFragment expectedFormAfterClick = fixture.GetFragment();
      IRenderedFragment expectedTasks = fixture.GetFragment("expected tasks");

      // Act - change the value of the input element and click the submit button
      cut.Find("input").Change("FOO BAR BAZ");
      cut.Find("button").Click();

      // Assert - verify cascading value was received
      Assert.Equal("dark-theme", cut.Instance.ThemeClass);

      // Assert - verify that tasks had the correct theme applied
      //          and the task added.
      IElement actualTasks = cut.Find("#tasks");
      actualTasks.MarkupMatches(expectedTasks);

      // Assert - verify that the input form was reset after task was added
      IElement actualForm = cut.Find("form");
      actualForm.MarkupMatches(expectedFormAfterClick);
    }
  }
</Fixture>
```

Let's look at what's going on in this test:

1. The fixture has both a setup and test method specified. The setup method is used to register an empty list of tasks that the `<SimpleTodo>` component requires.
2. The `<SimpleTodo>` component is wrapped in a `<CascadingValue>` component which passes down the "Theme" cascading value.
3. The first `<Fragment>` does not have an `Id` parameter. It is not needed since the `GetFragment(string? id = null)` method will pick the first `<Fragment>` if no `id` argument is passed to it.
4. The second `<Fragment Id="expected tasks">` does have an `Id` parameter, to make it possible to get the second fragment through a call to the `GetFragment("expected tasks")` method.
5. The test uses the generic version of `GetComponentUnderTest<SimpleTodo>()`, which gives us access to the instance of `SimpleTodo` and allows us to inspect its properties, e.g. `ThemeClass`.

This covers the “arrange”-steps in the test above and shows how you can easily access both the component under test and other fragments that you might need to write your test concisely. We will cover the details of the "act" and "assertion" steps in the [Interacting with a Component Under Test
](https://bunit.egilhansen.com/docs/interaction) and [Verifying Output from a Component Under Test](https://bunit.egilhansen.com/docs/verification) pages. 

Learn more about injecting services into components under test on the [Injecting Services into Components Under Test](https://bunit.egilhansen.com/docs/providing-input/inject-services-into-components) page. 

# `<SnapshotTest>` Details

bUnit's support for snapshot testing comes with the `<SnapshotTest>` component. In snapshot testing, you declare your input (e.g. one or more components under test) and the expected output, and the library will automatically tell you if they do not match.

> **NOTE:**
> One notable snapshot testing feature is missing: the ability to auto-generate the expected output initially, when it is not specified.

> **WARNING:**
> Razor tests, where `<SnapshotTest>` components are used, are currently only compatible with xUnit as the general-purpose testing framework.

## Parameters

All parameters that the `<SnapshotTest>` component supports are listed below:

```razor
@inherits TestComponentBase

<SnapshotTest Setup=@Setup
              SetupAsync=@SetupAsync
              Description="Description of test"
              Timeout="TimeSpan.FromSeconds(2)"
              Skip="Reason to skip the test">
  <TestInput>...</TestInput>
  <ExpectedOutput>...</ExpectedOutput>
  @code
  {
    void Setup(SnapshotTest test) { }
    Task SetupAsync(SnapshotTest test) => Task.CompletedTask;
  }
</SnapshotTest>
```

Let us go over each of these:

1. **`Setup` and `SetupAsync`:**  
   The `Setup` and `SetupAsync` methods can be used to register any services that should be injected into the components declared inside the `<TestInput>` and `<ExpectedOutput>`, and you can use both `Setup` and `SetupAsync` if needed. If both are provided, `Setup` is called first.   
2. **`Description`:**   
   If a description is provided, it will be displayed by the test runner when the test runs, as well as in Visual Studio's Test Explorer. If no description is provided then "SnapshotTest #NUM" is used, where NUM is the position the test has in the file it is declared in.
3. **`Skip`:**  
   If the skip parameter is provided, the test is skipped and the text entered in the skip parameter is passed to the test runner as the reason for skipping the test.
4. **`Timeout`:**  
   If provided, the test runner will terminate the test after the specified amount of time if it has not completed already.
5. **`TestInput` child component:**  
   Inside the `<TestInput>` child component is where you put all Razor and HTML markup that constitutes the test input or component under test.
6. **`ExpectedOutput` child component:**  
   Inside the `<ExpectedOutput>` child component is where you put all Razor and HTML markup that represents what the rendered result of `<TestInput>` should be. 

## What Happens When the Test Runs?

When a `<SnapshotTest>` runs, this happens:

1. It will first call the setup methods
2. Then it will render the `<TestInput>` and `<ExpectedOutput>` child components
3. Finally, it will compare the rendered markup from the `<TestInput>` and `<ExpectedOutput>` child components using the semantic HTML comparer built into bUnit

The semantic comparison in bUnit allows you to customize the snapshot verification  through _"comparison modifiers"_ in the `<ExpectedOutput>` markup. For example, if you want to tell the semantic comparer to ignore the case of the text content inside an element, you can add the `diff:ignoreCase` attribute to the element inside `<ExpectedOutput>`. 

To learn more about semantic comparison modifiers, go to the [Customizing the Semantic HTML Comparison](https://bunit.egilhansen.com/docs/verification/semantic-html-comparison) page.
