# Razor test examples

> **WARNING:** These examples are somewhat outdated. Some content in here might still apply:

In the following examples, the terminology **component under test** (abbreviated CUT) is used to mean the component that is the target of the test. The examples below use the `Shouldly` assertion library as well. If you prefer not to use that just replace the assertions with the ones from your own favorite assertion library.

All examples can be found in the [Tests](https://github.com/egil/bunit/tree/main/sample/tests/Tests) folder in the [Sample project](https://github.com/egil/bunit/tree/main/sample/).

## Examples

Here is a few examples that demonstrate how Razor test components can be used. More can be found in the [sample/tests/RazorComponentTests](https://github.com/egil/bunit/tree/main/sample/tests/RazorComponentTests) samples folder.

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
    void ThemedButtonUsesNamedCascadingValue()
    {
        var cut = GetComponentUnderTest();

        var elm = cut.Find("div");
        elm.ClassList.ShouldContain("FOO");
        elm.GetAttribute("title").ShouldContain("BAR");
    }
}
```

This example shows how [ThemedElement.razor](https://github.com/egil/bunit/tree/main/sample/src/Components/ThemedElement.razor) can be tested with cascading values.

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
    void MarkupPassedViaChildContent()
    {
        var expectedChildContent = GetFragment();

        var cut = GetComponentUnderTest();

        cut.Find("button").ChildNodes.MarkupMatches(expectedChildContent);
    }
}
```

This example shows how [ThemedButton.razor](https://github.com/egil/bunit/tree/main/sample/src/Components/ThemedButton.razor) can be tested with with child content, and how a `<Fragment>` can be used to specify the expected output.

Lets look at a more complex example, a test of the [TodoList.razor](https://github.com/egil/bunit/tree/main/sample/src/Pages/TodoList.razor) component:

```cshtml
<Fixture Setup="() => Services.AddMockJsRuntime()"
         Test="EmptyTodoList"
         Tests="new Action[]{ SettingLabel, TaskListRendersItemsUsingItemTemplate }">
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

    void EmptyTodoList()
    {
        // Act - get the CUT
        var cut = GetComponentUnderTest<TodoList>();

        // Assert - get the expected initial rendered HTML from the fragment
        // and use it to verify the initial rendered HTML
        var expectedInitialRender = GetFragment("EmptyTodoListRender");
        cut.MarkupMatches(expectedInitialRender);
    }

    void SettingLabel()
    {
        // Arrange - get the CUT
        var cut = GetComponentUnderTest<TodoList>();

        // Act - update label
        cut.SetParametersAndRender((nameof(TodoList.Label), "LABEL"));

        // Assert - verifiy that the placeholder and aria-label has changed
        cut.GetChangesSinceFirstRender().ShouldAllBe(
            diff => diff.ShouldBeAttributeChange("placeholder", "LABEL"),
            diff => diff.ShouldBeAttributeChange("aria-label", "LABEL")
        );
    }

    void TaskListRendersItemsUsingItemTemplate()
    {
        // Arrange - get the cut and take a snapshot of the current render tree output
        var cut = GetComponentUnderTest<TodoList>();
        cut.SaveSnapshot();

        // Act - assign test todo items to the CUT
        cut.SetParametersAndRender((nameof(TodoList.Items), TestItems));

        // Assert - get the diffs since the snapshot and compare to the expected.
        var diffs = cut.GetChangesSinceSnapshot();
        var expected = GetFragment("TodoItemRender");
        diffs.ShouldHaveSingleChange().ShouldBeAddition(expected);
    }
}
```

A few things worth noting here:

- The `Fixture` methods are called in this order:

  1. `Setup` (inline)
  2. `EmptyTodoList`
  3. `SettingLabel`
  4. `TaskListRendersItemsUsingItemTemplate`

- The CUT is only initialized once, the first time `GetComponentUnderTest<TodoList>()` is called. Subsequent calls return the same instance.

The follow test verifies some of the component logic around new todo creation and setting focus to the input field on first render:

```cshtml
<Fixture Setup="Setup"
         Tests="new Action[]{ OnFirstRenderInputFieldGetsFocus,
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

    void Setup()
    {
        jsRtMock = Services.AddMockJsRuntime();
    }

    void OnFirstRenderInputFieldGetsFocus()
    {
        // Act
        var cut = GetComponentUnderTest<TodoList>();

        // Assert that there is a call to document.body.focus.call with a single argument,
        // a reference to the input element.
        jsRtMock.VerifyInvoke("document.body.focus.call")
            .Arguments.Single().ShouldBeElementReferenceTo(cut.Find("input"));
    }

    void AfterFirstRenderInputFieldDoesntGetFocusAfterRerenders()
    {
        // Arrange
        var cut = GetComponentUnderTest<TodoList>();

        // Act
        cut.Render(); // second render
        cut.Render(); // thrid render
        cut.Render(); // ...
        cut.Render();

        // Assert that focus logic only runs on first render (only called 1 time).
        jsRtMock.VerifyInvoke("document.body.focus.call", calledTimes: 1);
    }

    void WhenAddTaskFormIsSubmittedWithNoTextOnAddingTodoIsNotCalled()
    {
        // Arrange
        var cut = GetComponentUnderTest<TodoList>();

        // Act - submit the empty form
        cut.Find("form").Submit();

        // Assert - verify that no task was created
        Assert.Null(createdTodo);
    }

    void WhenAddTaskFormIsSubmittedWithTextOnAddingTodoIsCalled()
    {
        // Arrange - ensure createdTask is null
        createdTodo = null;
        var cut = GetComponentUnderTest<TodoList>();
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
