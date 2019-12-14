# Writing Blazor Component tests in C

In the following examples, the terminology **component under test** (abbreviated CUT) is used to mean the component that is the target of the test. The examples below use the `Shouldly` assertion library as well. If you prefer not to use that just replace the assertions with the ones from your own favorite assertion library.

All examples can be found in the [CodeOnlyTests](../sample/tests/CodeOnlyTests) folder in the [Sample project](../sample/).

1. [Creating new test classes](creating-new-test-classes)
2. [Testing components without parameters](testing-components-without-parameters)
3. [Testing components with regular parameters](testing-components-with-regular-parameters)
4. [Testing components with child content](testing-components-with-child-content)

## Creating new test classes

All test classes are currently expected to inherit from `ComponentTestFixture`, which contains all the logic for rendering components and correctly dispose of renderers and HTML parsers after each test. For example:

```csharp
public class MyComponentTest : ComponentTestFixture
{
  [Fact]
  public void MyFirstTest()
  {
    // ...
  }
}
```

## Testing components without parameters

The following unit-tests verifies that the [Counter.razor](../sample/src/Pages/Counter.razor) component behaves correctly. Here is the source for the Counter component:

```razor
@page "/counter"

<h1>Counter</h1>

<p>
    Current count: @currentCount
</p>

<button class="btn btn-primary" @onclick="IncrementCount">Click me</button>

@code {
    int currentCount = 0;

    void IncrementCount()
    {
        currentCount++;
    }
}
```

These are the unit tests:

```csharp
public class CounterTest : ComponentTestFixture
{
    [Fact]
    public void InitialHtmlIsCorrect()
    {
        // Arrange - renders the Counter component
        var cut = RenderComponent<Counter>();

        // Assert
        // Here we specify expected HTML from CUT.
        var expectedHtml = @"<h1>Counter</h1>
                                <p>Current count: 0</p>
                                <button class=""btn-primary btn"">Click me</button>";

        // Here we use the HTML diffing library to assert that the rendered HTML
        // from CUT is semantically the same as the expected HTML string above.
        cut.ShouldBe(expectedHtml);
    }

    [Fact]
    public void ClickingButtonIncreasesCountStrict()
    {
        // Arrange - renders the Counter component
        var cut = RenderComponent<Counter>();

        // Act
        // Use a Find to query the rendered DOM tree and find the button element
        // and trigger the @onclick event handler by calling Click
        cut.Find("button").Click();

        // Assert
        // GetChangesSinceFirstRender returns list of differences since the first render,
        // in which we assert that there should only be one change, a text change where
        // the new value is provided to the ShouldHaveSingleTextChange assert method.
        cut.GetChangesSinceFirstRender().ShouldHaveSingleTextChange("Current count: 1");

        // Repeat the above steps to ensure that counter works for multiple clicks
        cut.Find("button").Click();
        cut.GetChangesSinceFirstRender().ShouldHaveSingleTextChange("Current count: 2");
    }

    [Fact]
    public void ClickingButtonIncreasesCountTargeted()
    {
        // Arrange - renders the Counter component
        var cut = RenderComponent<Counter>();

        // Act
        // Use a Find to query the rendered DOM tree and find the button element
        // and trigger the @onclick event handler by calling Click
        cut.Find("button").Click();

        // Assert
        // Use a Find to query the rendered DOM tree and find the paragraph element
        // and assert that its text content is the expected (calling Trim first to remove insignificant whitespace)
        cut.Find("p").TextContent.Trim().ShouldBe("Current count: 1");

        // Repeat the above steps to ensure that counter works for multiple clicks
        cut.Find("button").Click();
        cut.Find("p").TextContent.Trim().ShouldBe("Current count: 2");
    }
}
```

A few things worth noting about the tests above:

1. `InitialHtmlIsCorrect` uses the `ShouldBe` method that performs a semantic comparison of the generated HTML from CUT and the expected HTML string. That ensures that insignificant whitespace doesn't give false positives, among other things.

2. The "**strict**" test (`ClickingButtonIncreasesCountStrict`) and the "**targeted**" test (`ClickingButtonIncreasesCountTargeted`) takes two different approaches to verifying CUT renders the expected output:

   - The **strict** version generates a diff between the initial rendered HTML and the rendered HTML after the button click, and then asserts that the compare result only contains the expected change.
   - The **targeted** version finds the `<p>` element expect to have changed, and asserts against its text content.

   With the _targeted_ version, we cannot guarantee that there are not other changes in other places of the rendered HTML, if that is a concern, use the strict style. If it is not, then the targeted style can lead to simpler test.

## Testing components with regular parameters

In the following tests we will pass regular parameters to a component under test, e.g. `[Parameter] public SomeType PropName { get; set; }` style properties, where `SomeType` **is not** a `RenderFragment` or a `EventCallback` type.

The component under test will be the [Aside.razor](../sample/src/Components/Aside.razor) component, which looks like this:

```cshtml
<aside @attributes="Attributes">
    @if (Header is { })
    {
        <header>@Header</header>
    }
    @ChildContent
</aside>
@code {
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? Attributes { get; set; }

    [Parameter] public string? Header { get; set; }

    [Parameter] public RenderFragment? ChildContent { get; set; }
}
```

Here is a test:

```csharp
public class AsideTest : ComponentTestFixture
{
    [Fact(DisplayName = "Aside should render header and additional parameters correctly")]
    public void Test001()
    {
        // Arrange
        var header = "Hello testers";
        var cssClass = "some-class";

        // Act - render the Aside component with two parameters (passed as pairs of name, value tuples).
        // Note the use of the nameof operator to get the name of the Header parameter. This
        // helps keeps the test passing if the name of the parameter is refactored.
        //
        // This is equivalent to the follow Razor code:
        //
        // <Aside Header="Hello testers" class="some-class">
        // </Aside>
        var cut = RenderComponent<Aside>(
            (nameof(Aside.Header), header),
            ("class", cssClass)
        );

        // Assert - verify that the rendered HTML from the Aside component matches the expected output.
        cut.ShouldBe($@"<aside class=""{cssClass}""><header>{header}</header></aside>");
    }
}
```

In the test above, we use an overload of the `RenderComponent<TComponent>()` method, that allow us to pass regular parameters as pairs of `(string name, object? value)`.

As highlighted in the code, I recommend using the [`nameof`](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/nameof) to get the name of declared parameters from the component, so any changes to the name through refactoring automatically updates the test.

The second parameter, `class` is explicitly declared in the `Aside` class. It is instead `Attributes` parameter, that captures all unmatched parameters.

## Testing components with child content

The `Aside` component listed in the previous section also has a `ChildContent` parameter, so lets add a few tests that passes markup and components to it through that.

```csharp
public class AsideTest : ComponentTestFixture
{
    [Fact(DisplayName = "Aside should render child markup content correctly")]
    public void Test002()
    {
        // Arrange
        var content = "<p>I like simple tests and I cannot lie</p>";

        // Act
        // Act - render the Aside component with a child content parameter,
        // which is constructed through the ChildContent helper method.
        //
        // This is equivalent to the follow Razor code:
        //
        // <Aside>
        //     <p>I like simple tests and I cannot lie</p>
        // </Aside>
        var cut = RenderComponent<Aside>(
            ChildContent(content)
        );

        // Assert - verify that the rendered HTML from the Aside component matches the expected output.
        cut.ShouldBe($@"<aside>{content}</aside>");
    }

    [Fact(DisplayName = "Aside should render a child component correctly")]
    public void Test003()
    {
        // Arrange - set up test data
        var outerAsideHeader = "Hello outside";
        var nestedAsideHeader = "Hello inside";

        // Act - render the Aside component, passing a header to it
        // and a component to its child content. The ChildContent helper
        // method will pass the parameters it is given to the nested Aside
        // component.
        //
        // This is equivalent to the follow Razor code:
        //
        // <Aside Header="Hello outside">
        //     <Aside Header="Hello inside"></Aside>
        // </Aside>
        var cut = RenderComponent<Aside>(
            (nameof(Aside.Header), outerAsideHeader),
            ChildContent<Aside>(
                (nameof(Aside.Header), nestedAsideHeader)
            )
        );

        // Assert - verify that the rendered HTML from the Aside component matches the expected output.
        cut.ShouldBe($@"<aside>
                            <header>{outerAsideHeader}</header>
                            <aside>
                                <header>{nestedAsideHeader}</header>
                            </aside>
                        </aside>");
    }
}
```

In `Test002` above we use the `ChildContent(...)` helper method to create a ChildContent parameter and pass that to the `Aside` component. The overload, `ChildContent<TComponent>(component params)`, used in `Test003`, allows us to create render fragment that will render a component (of type `TComponent`) with the specified parameters. The `ChildContent<TComponent>(...)` has the same parameter options as the `RenderComponent<TComponent>` method has.
