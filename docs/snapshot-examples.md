# Writing Snapshot Tests for Blazor Components

The library has basic support for snapshot testing, declared via Razor syntax in Razor files. In snapshot testing, you declare your input (e.g. one or more component under test) and the expected output, and the library will automatically tell you if they do not match.

Notable features that are missing at the moment is the ability to auto-generate the expected output and to trigger updates of expected output.

**NOTE:** This feature is _EXPERIMENTAL_, and syntax and API will likely be changed. See [Contribute](readme.md/#contribute) for info on how to provide feedback and suggestions.

1. [Creating new snapshot test component](#creating-new-snapshot-test-component)
2. [Defining snapshot test cases](#defining-snapshot-test-cases)
3. [Examples](#examples)
4. [Known issues](#known-issues)

## Creating new snapshot test component

To create Razor-based snapshot tests, we need to create snapshot test components.

All snapshot test components must inherit from `TestComponentBase`, e.g. by adding `@inherits TestComponentBase` to the top of your .razor file.

**Tip:** In the folder you keep your Razor-based snapshot tests, add a `_Imports.razor` file, and put `@inherits TestComponentBase` into that as well as any using statements you need. Then all snapshot test components inherit the correct base component by default and have the default imports available.

## Defining snapshot test cases

All you need to define a snapshot test case is the `<SnapshotTest>` component added to a test component, e.g.:

```cshtml
<SnapshotTest Description="Helpful description of the test case - displayed if test fails"
              Setup="ctx => ctx.Services.AddMockJsRuntime()">
    <TestInput><!-- Declare your test input here, e.g. one or more components --></TestInput>
    <ExpectedOutput><!-- Declare your expected output here --></ExpectedOutput>
</SnapshotTest>
```

You can add as many `<SnapshotTest>` components to a test component as you want. Each `<SnapshotTest>` component will go through this life cycle:

1. Create a new test context (of type `ITestContext`).
2. Call the `Setup` method, if specified, and pass it the test context. Use the `Setup` method to e.g. configure services, like registering a mock `IJsRuntime`.
3. Render the child content of the `<TestInput>` component and capture its output.
4. Render the child content of the `<ExpectedOutput>` component and capture its output.
5. Verify that the two outputs are equal. If they are not, the test will fail with an `HtmlEqualException`.

## Examples

The following example shows how to test the the [TodoList.razor](../sample/src/Pages/TodoList.razor) component:

```cshtml
@inherits TestComponentBase

<SnapshotTest Description="A todolist with one todo added should render correctly"
              Setup="ctx => ctx.Services.AddMockJsRuntime()">
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

## Known issues

These are the known issues:

1. The xUnit test runner is not able to distinguish the individual `<SnapshotTest>`'s from each other. So they are all executed together, one at the time. This is less than a problem with snapshot tests than with Razor based tests, since a failing test will have its description printed along with the differences found in the snapshot. The solution is to develop a custom test runner, see [issue #4 - Create custom xUnit test runner/discoverer](/issues/4) for more details.
