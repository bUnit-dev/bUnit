# Snapshot testing

The library has basic support for snapshot testing, declared via Razor syntax in Razor files. In snapshot testing, you declare your input (e.g. one or more component under test) and the expected output, and the library will automatically tell you if they do not match.

Notable features that are missing at the moment is the ability to auto-generate the expected output and to trigger updates of expected output.

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
              Timeout="1000"
              Skip="Reason to skip the test"
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

If the `Timeout` parameter is specified, the test will timeout after the specified time, in milliseconds, and if `Skip` parameter is specified, the test is skipped and the reason is printed in the test output.

## Executing test cases

Since Snapshot tests use xUnit under the hood as a test runner, you execute your tests them in exactly the same way as you would normal xUnit unit tests, i.e. by running `dotnet test` from the console or running the tests through the Test Explorer in Visual Studio.

## Examples

The following example shows how to test the the [TodoList.razor](https://github.com/egil/bunit/blob/master/sample/src/Components/TodoList.razor) component:

```cshtml
@inherits TestComponentBase

<SnapshotTest Description="A todolist with one todo added should render correctly"
              Setup="() => Services.AddMockJsRuntime()">
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

## Further reading:

- [Semantic HTML markup comparison](/docs/semantic-html-markup-comparison.html)
- [Mocking JsRuntime](/docs/mocking-jsruntime.html)
- [Snapshot test examples in the sample project](https://github.com/egil/bunit/tree/master/sample/tests/SnapshotTests)
