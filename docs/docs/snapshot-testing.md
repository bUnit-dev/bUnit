# Snapshot testing

The library has basic support for snapshot testing, declared via Razor syntax in Razor files. In snapshot testing, you declare your input (e.g. one or more component under test) and the expected output, and the library will automatically tell you if they do not match.

Notable features that are missing at the moment is the ability to auto-generate the expected output and to trigger updates of expected output.

> **NOTE:** This feature is _EXPERIMENTAL_ and syntax and API will likely changed. Here are a few limitations to be aware of at the moment:
>
> - The xUnit test runner can detect and execute tests in Razor test components, but is not able to distinguish the individual `<SnapshotTest>`'s from each other. They are all executed together, one at the time. The solution is planned, see the [related issue](https://github.com/egil/razor-components-testing-library/issues/4) for details.
> - Go to the [Contribute](/docs/Contribute.html) page for info on how to provide feedback and suggestions.

**Content:**

- [Creating new snapshot test component](#creating-new-snapshot-test-component)
- [Defining snapshot test cases](#defining-snapshot-test-cases)
- [Executing test cases](#executing-test-cases)
- [Examples](#examples)

**Further reading:**

- [Semantic HTML markup comparison](/docs/Semantic-HTML-markup-comparison.html)

## Creating new snapshot test component

To create Razor-based snapshot tests, we need to create snapshot Razor test components.

All snapshot test components must inherit from `TestComponentBase`, e.g. by adding `@inherits TestComponentBase` to the top of your .razor file.

For example:

```cshtml
@inherits TestComponentBase

<SnapshotTest Description="Test 1">
  <TestInput>...</TestInput>
  <ExpectedOutput>...</ExpectedOutput>
</SnapshotTest>

<SnapshotTest Description="Test 2">
  <TestInput>...</TestInput>
  <ExpectedOutput>...</ExpectedOutput>
</SnapshotTest>
```

## Defining snapshot test cases

All you need to define a snapshot test case is the `<SnapshotTest>` component added to a test component, e.g.:

```cshtml
<SnapshotTest Description="Helpful description of the test case - displayed if test fails"
              Setup="() => Services.AddMockJsRuntime()"
              SetupAsync="() => Task.CompletedTask">
    <TestInput><!-- Declare your test input here, e.g. one or more components --></TestInput>
    <ExpectedOutput><!-- Declare your expected output here --></ExpectedOutput>
</SnapshotTest>
```

You can add as many `<SnapshotTest>` components to a test component as you want. Each `<SnapshotTest>` component will go through this life cycle:

1. Call the `Setup` and `SetupAsync` methods, if specified. Use the `Setup`/`SetupAsync` method to e.g. configure services, like registering a mock `IJsRuntime`.
2. Render the child content of the `<TestInput>` component and capture its output.
3. Render the child content of the `<ExpectedOutput>` component and capture its output.
4. Verify that the two outputs are equal. If they are not, the test will fail with an `HtmlEqualException`.

## Executing test cases

Since Snapshot tests use xUnit under the hood as a test runner, you execute your tests them in exactly the same way as you would normal xUnit unit tests, i.e. by running `dotnet test` from the console or running the tests through the Test Explorer in Visual Studio.

Do note the current limitations mentioned at the top of the page.

## Examples

The following example shows how to test the the [TodoList.razor](https://github.com/egil/razor-components-testing-library/tree/master/sample/src/Pages/TodoList.razor) component:

```cshtml
@inherits TestComponentBase

<SnapshotTest Description="A todolist with one todo added should render correctly"
              Setup="() => Services.AddMockJsRuntime()"
              SetupAsync="() => Task.CompletedTask">
    <TestInput>
        <TodoList Label="My label" Items=@(new Todo[]{ new Todo{ Id=42, Text="Check out this new thing called Blazor" } })>
            <ItemsTemplate Context="todo">
                <TodoItem Todo="todo"></TodoItem>
            </ItemsTemplate>
        </TodoList>
    </TestInput>
    <ExpectedOutput>
        <form>
            <div class="input-group">
                <input type="text" class="form-control" placeholder="My label" aria-label="My label" value="" />
                <div class="input-group-append">
                    <button class="btn btn-secondary" type="submit">Add task</button>
                </div>
            </div>
        </form>
        <ol class="list-group">
            <li id:regex="todo-42" class="list-group-item list-group-item-action">
                <span>Check out this new thing called Blazor</span>
                <span class="float-right text-danger">(click to complete)</span>
            </li>
        </ol>
    </ExpectedOutput>
</SnapshotTest>
```
