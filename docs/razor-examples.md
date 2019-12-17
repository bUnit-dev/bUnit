# Writing Blazor Component tests in a mix of Razor and C# code

The library supports specifying the component under test and other markup/fragments using Razor syntax. The advantage is that we get Visual Studio's help writing Razor and HTML with IntelliSense and auto complete, which is a much nicer experience than writing HTML in a string in a C# class/file. This is especially useful for more complex scenarios, where e.g. a component under test has many parameters or complex child contents.

**NOTE:** This feature is _EXPERIMENTAL_, and syntax and API will likely be changed. See [Contribute](readme.md/#contribute) for info on how to provide feedback and suggestions.

1. [Creating new test component](#creating-new-test-component)
2. [Defining test cases](#defining-test-cases)
3. [Examples](#examples)
4. [Known issues](#known-issues)

## Creating new test component

To create Razor-based tests, we need to create test components.

All test components must inherit from `TestComponentBase`, e.g. by adding `@inherits TestComponentBase` to the top of your .razor file.

You will also need to import a few namespaces to make asserting and mocking possible, e.g.:

```cshtml
@inherits TestComponentBase
@using Egil.RazorComponents.Testing
@using Egil.RazorComponents.Testing.Asserting
@using Egil.RazorComponents.Testing.EventDispatchExtensions
@using Xunit @*or e.g. Shouldly*@
```

**Tip:** In the folder you keep your Razor-based tests, add a `_Imports.razor` file, and put the above into that. Then all test components inherit the correct base component by default and have the default imports available.

## Defining test cases

When you have a Razor test component created, it's time to add test cases. This is done via the `<Fixture>` component and related test methods and child components.

Let's look at what options we have by setting up an empty test case, first the code:

```cshtml
<Fixture Setup="Setup"
         Test="Test1"
         Tests="new Test[]{ Test2, Test3 }">
    <ComponentUnderTest>...</ComponentUnderTest>
    <Fragment Id="id 2"></Fragment>
    <Fragment Id="id 2"></Fragment>
</Fixture>
@code {
    void Setup(IRazorTestContext context)
    {
        // Add services and do other setup work in this method.
        context.Services.AddMockJsRuntime();
    }
    void Test1(IRazorTestContext context)
    {
        var cut = context.GetComponentUnderTest<TComponent>();
        var f1 = context.GetFragment<TComponent>("id 1");
        var f2 = context.GetFragment("id 2");
    }
    void Test2(IRazorTestContext context)
    {
        // do more testing on CUT, f1 and f2 by retriving them from the context.
    }
    void Test3(IRazorTestContext context)
    {
        // do more testing on CUT, f1 and f2 by retriving them from the context.
    }
}
```

The code above works as follows:

- All the `<Fixture>` component defined in the test component is found by the `TestComponentBase`'s test method. For each `Fixture`, it calls the related methods in the following order (if they are present):

  1. `Setup`
  2. `Test`
  3. `Tests`, one at the time, in the order they appear in the array.

- It is inside child component `<ComponentUnderTest>` where you declare the component under test.
- Any markup or component fragments that is needed for the test can be declared inside the optional `<Fragment>` components. The `Id` parameter is optional, and only needed if you have more than one.

- To render and get the component under test or any of the fragments, use the `GetComponentUnderTest<TComponent>()` method on the `IRazorTestContext` object. `GetFragment()` can be called both with and without a `TComponent`, e.g. if it's just markup defined in it.

- Inside the `Test` methods you can do all the things you can in C#-based tests, e.g. assert against the CUT. The only difference is that some methods such as `TakeSnapshot()` is not available on the local scope, but through the `IRazorTestContext` object passed to each Test method.

## Examples

Here is a few examples that demonstrate how Razor test components can be used. More can be found in the [samples/tests/RazorComponentTests](../samples/tests/RazorComponentTests) samples folder.

```cshtml
<Fixture Test="ThemedButtonUsesNamedCascadingValue">
    <ComponentUnderTest>
        <CascadingValue Name=@nameof(ThemedElement.Class) Value=@(new ThemeInfo { Value = "FOO" })>
            <CascadingValue Name=@nameof(ThemedElement.Title) Value=@(new ThemeInfo { Value = "BAR" })>
                <ThemedElement />
            </CascadingValue>
        </CascadingValue>
    </ComponentUnderTest>
</Fixture>
@code {
    void ThemedButtonUsesNamedCascadingValue(IRazorTestContext context)
    {
        var cut = context.GetComponentUnderTest();

        var elm = cut.Find("div");
        elm.ClassList.ShouldContain("FOO");
        elm.GetAttribute("title").ShouldContain("BAR");
    }
}
```

This example shows how [ThemedElement.razor](../sample/src/Components/ThemedElement.razor) can be tested with cascading values.

```cshtml
<Fixture Test=MarkupPassedViaChildContent>
    <ComponentUnderTest>
        <ThemedButton>
            <h1>Foo bar button</h1>
        </ThemedButton>
    </ComponentUnderTest>
    <Fragment><h1>Foo bar button</h1></Fragment>
</Fixture>
@code {
    void MarkupPassedViaChildContent(IRazorTestContext context)
    {
        var expectedChildContent = context.GetFragment();

        var cut = context.GetComponentUnderTest();

        cut.Find("button").ChildNodes.ShouldBe(expectedChildContent);
    }
}
```

This example shows how [ThemedButton.razor](../sample/src/Components/ThemedButton.razor) can be tested with child content, and how a `<Fragment>` can be used to specify the expected output.

Let's look at a more complex example, a test of the [TodoList.razor](../sample/src/Pages/TodoList.razor) component:

```cshtml
<Fixture Setup="ctx => ctx.Services.AddMockJsRuntime()"
         Test="EmptyTodoList"
         Tests="new Test[]{ SettingLabel, TaskListRendersItemsUsingItemTemplate }">
    <ComponentUnderTest>
        <TodoList>
            <ItemsTemplate>
                <TodoItem Todo=@context />
            </ItemsTemplate>
        </TodoList>
    </ComponentUnderTest>
    <Fragment Id="EmptyTodoListRender">
        <form>
            <div class="input-group">
                <input value="" type="text" class="form-control"
                       placeholder="Task description" aria-label="Task description" />
                <div class="input-group-append">
                    <button class="btn btn-secondary" type="submit">Add task</button>
                </div>
            </div>
        </form>
        <ol class="list-group"></ol>
    </Fragment>
    <Fragment Id="TodoItemRender">
        <TodoItem Todo="@TestItems[0]" />
    </Fragment>
</Fixture>
@code {
    Todo[] TestItems { get; } = new[] { new Todo { Id = 42 } };

    void EmptyTodoList(IRazorTestContext context)
    {
        // Act - get the CUT
        var cut = context.GetComponentUnderTest<TodoList>();

        // Assert - get the expected initial rendered HTML from the fragment
        // and use it to verify the initial rendered HTML
        var expectedInitialRender = context.GetFragment("EmptyTodoListRender");
        cut.ShouldBe(expectedInitialRender);
    }

    void SettingLabel(IRazorTestContext context)
    {
        // Arrange - get the CUT
        var cut = context.GetComponentUnderTest<TodoList>();

        // Act - update label
        cut.SetParametersAndRender((nameof(TodoList.Label), "LABEL"));

        // Assert - verifiy that the placeholder and aria-label has changed
        cut.GetChangesSinceFirstRender().ShouldAllBe(
            diff => diff.ShouldBeAttributeChange("placeholder", "LABEL"),
            diff => diff.ShouldBeAttributeChange("aria-label", "LABEL")
        );
    }

    void TaskListRendersItemsUsingItemTemplate(IRazorTestContext context)
    {
        // Arrange - get the cut and take a snapshot of the current render tree output
        var cut = context.GetComponentUnderTest<TodoList>();
        cut.TakeSnapshot();

        // Act - assign test todo items to the CUT
        cut.SetParametersAndRender((nameof(TodoList.Items), TestItems));

        // Assert - get the diffs since the snapshot and compare to the expected.
        var diffs = cut.GetChangesSinceSnapshot();
        var expected = context.GetFragment("TodoItemRender");
        diffs.ShouldHaveSingleChange().ShouldBeAddition(expected);
    }
}
```

A few things worth noting here:

- The `Fixture` methods are called in this order:

  1.  `Setup` (inline)
  2.  `EmptyTodoList`
  3.  `SettingLabel`
  4.  `TaskListRendersItemsUsingItemTemplate`

- The CUT is only initialized once, the first time `GetComponentUnderTest<TodoList>()` is called. Subsequent calls return the same instance.

The follow test verifies some of the component logic around new todo creation and setting focus to the input field on first render:

```cshtml
<Fixture Setup="Setup"
         Tests="new Test[]{ OnFirstRenderInputFieldGetsFocus,
                AfterFirstRenderInputFieldDoesntGetFocusAfterRerenders,
                WhenAddTaskFormIsSubmittedWithNoTextOnAddingTodoIsNotCalled }">
    <ComponentUnderTest>
        <TodoList OnAddingTodo="OnAddingTodoHandler">
            <ItemsTemplate>
                <TodoItem Todo=@context />
            </ItemsTemplate>
        </TodoList>
    </ComponentUnderTest>
</Fixture>
@code {
    MockJsRuntimeInvokeHandler jsRtMock = default!;
    Todo? createdTodo;

    void OnAddingTodoHandler(Todo todo) => createdTodo = todo;

    void Setup(IRazorTestContext context)
    {
        jsRtMock = context.Services.AddMockJsRuntime();
    }

    void OnFirstRenderInputFieldGetsFocus(IRazorTestContext context)
    {
        // Act
        var cut = context.GetComponentUnderTest<TodoList>();

        // Assert that there is a call to document.body.focus.call with a single argument,
        // a reference to the input element.
        jsRtMock.VerifyInvoke("document.body.focus.call")
            .Arguments.Single().ShouldBeElementReferenceTo(cut.Find("input"));
    }

    void AfterFirstRenderInputFieldDoesntGetFocusAfterRerenders(IRazorTestContext context)
    {
        // Arrange
        var cut = context.GetComponentUnderTest<TodoList>();

        // Act
        cut.Render(); // second render
        cut.Render(); // thrid render
        cut.Render(); // ...
        cut.Render();

        // Assert that focus logic only runs on first render (only called 1 time).
        jsRtMock.VerifyInvoke("document.body.focus.call", calledTimes: 1);
    }

    void WhenAddTaskFormIsSubmittedWithNoTextOnAddingTodoIsNotCalled(IRazorTestContext context)
    {
        // Arrange
        var cut = context.GetComponentUnderTest<TodoList>();

        // Act - submit the empty form
        cut.Find("form").Submit();

        // Assert - verify that no task was created
        Assert.Null(createdTodo);
    }

    void WhenAddTaskFormIsSubmittedWithTextOnAddingTodoIsCalled(IRazorTestContext context)
    {
        // Arrange - ensure createdTask is null
        createdTodo = null;
        var cut = context.GetComponentUnderTest<TodoList>();
        var taskValue = "HELLO WORLD TASK";

        // Act - find input field and change its value, then submit the form
        cut.Find("input").Change(taskValue);
        cut.Find("form").Submit();

        // Assert that a new task has been passed to the EventCallback listener and that the
        // new task has the expected value
        Assert.NotNull(createdTodo);
        Assert.Equal(taskValue, createdTodo?.Text);
    }
}
```

More examples to come.

## Known issues

These are the known issues:

1. The XUnit test runner is not able to distinguish the individual `<Fixture>`'s from each other. So they are all executed together, one at the time. The solution is to develop a custom test runner, see [issue #4](#4) for more details.

2. If you are using `Shouldly`, the overload resolution can end up picking the `ShouldBe()` extension method from `Shouldly` instead of the `ShouldBe()` extension method from this library, when we want to verify rendered HTML. This is not a problem in C# only tests, so I am currently investigating why the generated Razor C# code picks a different overload.

   The workaround is to write the affected test in C# for now.
