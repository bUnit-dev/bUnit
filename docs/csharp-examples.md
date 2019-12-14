# Writing Blazor Component tests in C

In the following examples, the terminology **component under test** (abbreviated CUT) is used to mean the component that is the target of the test. The examples below use the `Shouldly` assertion library as well. If you prefer not to use that just replace the assertions with the ones from your own favorite assertion library.

All examples can be found in the [Tests](../sample/tests/Tests) folder in the [Sample project](../sample/).

1. [Creating new test classes](#creating-new-test-classes)
2. [Testing components without parameters](#testing-components-without-parameters)
3. [Testing components with parameters](#testing-components-with-parameters)  
   3.1. [Passing new parameters to an already rendered component](#passing-new-parameters-to-an-already-rendered-component)
4. [Testing components with child content](#testing-components-with-child-content)
5. [Testing components with EventCallback parameters](#testing-components-with-eventcallback-parameters)
6. [Testing components with cascading-value parameters](#testing-components-with-cascading-value-parameters)
7. [Testing components that use on IJsRuntime](#testing-components-that-use-on-ijsruntime)  
   7.1 [Verifying element references passed to InvokeAsync](#verifying-element-references-passed-to-invokeasync)
8. [Testing components with injected dependencies](#testing-components-with-injected-dependencies)
9. [Dispatching @on-events from tests](#Dispatching-on-events-from-tests)

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

The [CounterTest.cs](../sample/tests/Tests/Pages/CounterTest.cs) looks like this:

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

## Testing components with parameters

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

The [AsideTest.cs](../sample/tests/Tests/Components/AsideTest.cs) looks like this:

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

### Passing new parameters to an already rendered component

Sometimes we want to test what happens when a component is re-rendered, possible with new parameters. This can be done using the `cut.Render()` and the `cut.SetParametersAndRender()` methods, for example:

```csharp
    [Fact(DisplayName = "Passing new parameters to Aside updates the rendered HTML correctly")]
    public void Test002()
    {
        // Arrange - initial render of Aside
        var cut = RenderComponent<Aside>();

        // Act - set the Header parameter and re-render the CUT
        cut.SetParametersAndRender((nameof(Aside.Header), "HEADER"));

        // Assert - Check that we have exactly one change since the first render,
        // and that it is an addition to the DOM tree
        cut.GetChangesSinceFirstRender()
            .ShouldHaveSingleChange()
            .ShouldBeAddition("<header>HEADER</header>");

        // Arrange - Create a snapshot of the current rendered HTML for later comparisons
        cut.TakeSnapshot();

        // Act - Set the Header parameter to null again and re-render
        cut.SetParametersAndRender((nameof(Aside.Header), null));

        // Assert - Check that we have exactly one change since compared with the snapshot we took,
        // and that it is an addition to the DOM tree.
        cut.GetChangesSinceSnapshot()
            .ShouldHaveSingleChange()
            .ShouldBeRemoval("<header>HEADER</header>");
    }
```

Some notes on `Test002` above:

- The `cut.SetParametersAndRender()` method has the same overloads as the `RenderComponent()` method.
- The `ShouldHaveSingleChange()` method asserts that only a single difference is found by the compare method, and returns that diff object.
- The `ShouldBeAddition()` method verifies that a difference is an addition with the specified content (doing a semantic HTML comparison).
- The `cut.TakeSnapshot()` method saves the current rendered HTML for later comparisons.
- The `cut.GetChangesSinceSnapshot()` compares the current rendered HTML with the one saved by the `TakeSnapshot()` method.

## Testing components with child content

The [Aside.razor](../sample/src/Components/Aside.razor) component listed in the previous section also has a `ChildContent` parameter, so lets add a few tests that passes markup and components to it through that.

```csharp
public class AsideTest : ComponentTestFixture
{
   [Fact(DisplayName = "Aside should render child markup content correctly")]
    public void Test003()
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
    public void Test004()
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

- In `Test003` above we use the `ChildContent(...)` helper method to create a ChildContent parameter and pass that to the `Aside` component.
- The overload, `ChildContent<TComponent>(...)`, used in `Test004`, allows us to create a render fragment that will render a component (of type `TComponent`) with the specified parameters.  
  The `ChildContent<TComponent>(...)` has the same parameter options as the `RenderComponent<TComponent>` method has.

## Testing components with `EventCallback` parameters

To show how to pass an `EventCallback` to a component under test, we will use the [ThemedButton.razor](../sample/src/Components/ThemedButton.razor), which looks like this:

```cshtml
<button @onclick="HandleOnClick"
        class=@Theme?.Value
        title=@Title?.Value
        @attributes="Attributes">
    @ChildContent
</button>
@code {
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? Attributes { get; set; }

    [CascadingParameter] public ThemeInfo? Theme { get; set; }
    [CascadingParameter(Name = nameof(Title))] public ThemeInfo? Title { get; set; }
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public EventCallback<MouseEventArgs> OnClick { get; set; }

    private Task HandleOnClick(MouseEventArgs args) => OnClick.InvokeAsync(args);
}
```

The relevant part of [ThemedButtonTest.cs](../sample/tests/Tests/Components/ThemedButtonTest.cs) looks like this:

```csharp
public class ThemedButtonTest : ComponentTestFixture
{
    [Fact(DisplayName = "When button is clicked, the OnClick event callback is triggered")]
    public void Test001()
    {
        var wasCalled = false;
        // Arrange - pass a lambda in as parameter to the OnClick parameter.
        //
        // This is equivalent to the follow Razor code:
        //
        // <ThemedButton OnClick="(_) => wasCalled = true"></ThemedButton>
        var cut = RenderComponent<ThemedButton>(
            EventCallback(nameof(ThemedButton.OnClick), (MouseEventArgs _) => wasCalled = true)
        );

        // Act - click the button in CUT
        cut.Find("button").Click();

        // Assert - check if callback was triggered
        wasCalled.ShouldBeTrue();
    }
}
```

`Test001` above uses the `EventCallback(parammeterName, callback)` helper method the generate a proper `EventCallback` object. There are many overloads, that should enable all the normal scenarios that is possible via Razor code.

## Testing components with cascading-value parameters

If a component under test accepts cascading values, like [ThemedButton.razor](../sample/src/Components/ThemedButton.razor) listed above, we can pass one or more cascading values to it like so:

```csharp
public class ThemedButtonTest : ComponentTestFixture
{
    [Fact(DisplayName = "Themed button uses provided theme info to set class attribute")]
    public void Test002()
    {
        // Arrange - create an instance of the ThemeInfo class to passs to the ThemedButton
        var theme = new ThemeInfo() { Value = "BUTTON" };

        // Act - Render the ThemedButton component, passing in the instance of ThemeInfo
        // as an _unnamed_ cascading value.
        //
        // This is equivalent to the follow Razor code:
        //
        // <CascadingValue Value="theme">
        //     <ThemedButton></ThemedButton>
        // </CascadingValue>
        var cut = RenderComponent<ThemedButton>(
            CascadingValue(theme)
        );

        // Assert - check that the class specified in the cascading value was indeed used.
        cut.Find("button").ClassList.ShouldContain(theme.Value);
    }

    [Fact(DisplayName = "Named cascading values are passed to components")]
    public void Test003()
    {
        // Arrange - create two instances of the ThemeInfo class to passs to the ThemedButton
        var theme = new ThemeInfo() { Value = "BUTTON" };
        var titleTheme = new ThemeInfo() { Value = "BAR" };

        // Act - Render the ThemedButton component, passing in the instances of ThemeInfo
        // as an _unnamed_ and a _named_ cascading value.
        //
        // This is equivalent to the follow Razor code:
        //
        // <CascadingValue Value="theme">
        //     <CascadingValue Name="Title" Value="titleTheme">
        //         <ThemedButton></ThemedButton>
        //     </CascadingValue>
        // </CascadingValue>
        var cut = RenderComponent<ThemedButton>(
            CascadingValue(theme),
            CascadingValue(nameof(ThemedButton.Title), titleTheme)
        );

        // Assert - check that the class and title specified in the cascading values was indeed used.
        var elm = cut.Find("button");
        elm.ClassList.ShouldContain(theme.Value);
        elm.GetAttribute("title").ShouldContain(titleTheme.Value);
    }
}
```

- `Test002` above uses the `CascadingValue(object value)` helper method to pass an **unnamed** cascading value to the CUT.
- `Test003` above demonstrates how multiple (named) cascading values can be passed to a component under test.

## Testing components that use on `IJsRuntime`

It is not uncommon to have components use Blazor's JSInterop functionality to call JavaScript, e.g. after first render.

To make it easy to mock calls to JavaScript, the library comes with a `IJsRuntime` mocking helper, that allows you to specify return how JSInterop calls should be handled, and to verify that they have happened.

If you have more complex mocking needs, you could look to frameworks like [Moq](https://github.com/Moq).

To help us test the Mock JSRuntime, we have the [WikiSearch.razor](../sample/src/Components/WikiSearch.razor) component, which looks like this:

```cshtml
@inject IJSRuntime jsRuntime

<p>@searchResult</p>

@code {
    string searchResult = string.Empty;

    // Assumes the following function is available in the DOM
    // <script>
    //     function queryWiki(query) {
    //         return fetch('https://en.wikipedia.org/w/api.php?origin=*&action=opensearch&search=' + query)
    //             .then(x => x.text());
    //     }
    // </script>
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            searchResult = await jsRuntime.InvokeAsync<string>("queryWiki", "blazor");
            StateHasChanged();
        }
    }
}
```

The [WikiSearchTest.cs](../sample/tests/Tests/Components/WikiSearchTest.cs) looks like this:

```csharp
public class WikiSearchTest : ComponentTestFixture
{
    [Fact(DisplayName = "WikiSearch renders an empty P element initially")]
    public void Test001()
    {
        // Arrange
        // Registered the MockJsRuntime in "Loose" mode with the service provider used when rendering components.
        // JsRuntimeMockMode.Loose is the default. It configures the mock to just return the default
        // value for whatever is requested in a InvokeAsync call if no call has explicitly been set up.
        var jsMock = Services.AddMockJsRuntime();

        // Act - render the WikiSearch component
        var cut = RenderComponent<WikiSearch>();

        // Assert
        // Check that the components initial HTML is as expected
        // and that the mock was called with the expected JS identifier and arguments.
        cut.ShouldBe("<p></p>");
        jsMock.VerifyInvoke("queryWiki").Arguments.Single().ShouldBe("blazor");
    }

    [Fact(DisplayName = "On first render WikiSearch uses JSInterop to query wiki and display the result")]
    public void Test002()
    {
        // Arrange
        // Registered the MockJsRuntime in "strict" mode with the service provider used when rendering components.
        // JsRuntimeMockMode.Strict mode configures the mock to throw an error if it receives an InvokeAsync call
        // it has not been set up to handle.
        var jsMock = Services.AddMockJsRuntime(JsRuntimeMockMode.Strict);

        // Set up the mock to handle the expected call
        var expectedSearchResult = "SEARCH RESULT";
        var plannedInvocation = jsMock.Setup<string>("queryWiki", "blazor");

        // Render the WikiSearch and verify that there is no content in the paragraph element
        var cut = RenderComponent<WikiSearch>();
        cut.Find("p").InnerHtml.ShouldBeEmpty();

        // Act
        // Use the WaitForNextRender to block until the component has finished re-rendered.
        // The plannedInvocation.SetResult will return the result to the component is waiting
        // for in its OnAfterRender from the await jsRuntime.InvokeAsync<string>("queryWiki", "blazor") call.
        WaitForNextRender(() => plannedInvocation.SetResult(expectedSearchResult));

        // Assert
        // Verify that the result was received and correct placed in the paragraph element.
        cut.Find("p").InnerHtml.ShouldBe(expectedSearchResult);
    }
}
```

- `Test001` just injects the mock in "Loose" mode. It means it will only returns a `default(TValue)` for calls to `InvokeAsync<TValue>(...)` it receives. This allows us to test components that expects a `IJsRuntime` to be injected, but where the test we want to perform isn't dependent on it providing any specific return value.

  In "Loose" mode it is still possible to call `VerifyInvoke(identifier)` and assert against the expected invocation.

- `Test002` injects and configures the mock in strict mode. That requires us to configure all the expected calls the mock should handle. If it receives a call it has not been configured for, an exception is thrown and the test fails.

- The `WaitForNextRender(Action)` helper method is used to block until a (async) render completes, that the action passed to it has triggered.
  In `Test002` we trigger a render by setting the result on the planned invocation, which causes the `await jsRuntime.InvokeAsync<string>("queryWiki", "blazor")` call in the CUT to complete, and the component to trigger a re-render by calling the `StateHasChanged()` method.

### Verifying element references passed to InvokeAsync

If you want to verify that a element reference (`ElementReference`) passed to a IJsRuntime.InvokeAsync call is references the expected DOM element, you can do so with the `ShouldBeElementReferenceTo()` assert helper.

For example, consider the [FocussingInput.razor](../sample/src/Components/FocussingInput.razor) component, which looks like this:

```cshtml
@inject IJSRuntime jsRuntime

<input @ref="_inputRef" @attributes="Attributes" />

@code {
    private ElementReference _inputRef;

    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? Attributes { get; set; }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await jsRuntime.InvokeVoidAsync("document.body.focus.call", _inputRef);
        }
    }
}
```

The the [FocussingInputTest.cs](../sample/tests/Tests/Components/FocussingInputTest.cs) looks like this:

```csharp
public class FocussingInputTest : ComponentTestFixture
{
    [Fact(DisplayName = "After first render, the new input field has focus")]
    public void Test001()
    {
        // Arrange - add the IJsRuntime mock
        var jsRtMock = Services.AddMockJsRuntime();

        // Act - render the FocussingInput component, causing
        // the OnAfterRender(firstRender: true) to be called
        var cut = RenderComponent<FocussingInput>();

        // Assert
        // that there is a single call to document.body.focus.call
        var invocation = jsRtMock.VerifyInvoke("document.body.focus.call");
        // Assert that the invocation received a single argument
        // and that it was a reference to the input element.
        var expectedReferencedElement = cut.Find("input");
        invocation.Arguments.Single().ShouldBeElementReferenceTo(expectedReferencedElement);
    }
}
```

The last line verifies that there was a single argument to the invocation, and via the `ShouldBeElementReferenceTo` checks, that the `<input />` was indeed the referenced element.

## Testing components with injected dependencies

The demonstrate service injection, lets refactor the [FetchData.razor](../sample/src/Pages/FetchData.razor) component that comes with the default Razor app template, to make it more testable:

- Extract an interface from [WeatherForecastService](../sample/src/Data/WeatherForecastService.cs), name it [IWeatherForecastService](../sample/src/Data/IWeatherForecastService.cs), and have `FetchData` take a dependency on it.

- Extract the `<table>` inside the `else` branch in the [FetchData.razor](../sample/src/Pages/FetchData.razor) component into its own component. Lets name it [ForecastDataTable](../sample/src/Pages/FetchData.razor).

- In the [FetchData.razor](../sample/src/Pages/FetchData.razor), pass the variable `forecasts` to the [ForecastDataTable](../sample/src/Pages/FetchData.razor) component.

Now we just need a [MockForecastService.cs](../sample/tests/MockForecastService.cs). It looks like this:

```csharp
internal class MockForecastService : IWeatherForecastService
{
    public TaskCompletionSource<WeatherForecast[]> Task { get; } = new TaskCompletionSource<WeatherForecast[]>();
    public Task<WeatherForecast[]> GetForecastAsync(DateTime startDate) => Task.Task;
}
```

With the mock in place, we can write the [FetchDataTest.cs](../sample/tests/Tests/Pages/FetchDataTest.cs), which looks like this:

```csharp
public class FetchDataTest : ComponentTestFixture
{
    [Fact(DisplayName = "Fetch data component renders expected initial markup")]
    public void Test001()
    {
        // Arrange - add the mock forecast service
        Services.AddService<IWeatherForecastService, MockForecastService>();

        // Act - render the FetchData component
        var cut = RenderComponent<FetchData>();

        // Assert that it renders the initial loading message
        var initialExpectedHtml = @"<h1>Weather forecast</h1>
                                    <p>This component demonstrates fetching data from a service.</p>
                                    <p><em>Loading...</em></p>";
        cut.ShouldBe(initialExpectedHtml);
    }

    [Fact(DisplayName = "After data loads it is displayed in a ForecastTable component")]
    public void Test002()
    {
        // Setup the mock forecast service
        var forecasts = new[] { new WeatherForecast { Date = DateTime.Now, Summary = "Testy", TemperatureC = 42 } };
        var mockForecastService = new MockForecastService();
        Services.AddService<IWeatherForecastService>(mockForecastService);

        // Arrange - render the FetchData component
        var cut = RenderComponent<FetchData>();

        // Act - pass the test forecasts to the component via the mock services
        WaitForNextRender(() => mockForecastService.Task.SetResult(forecasts));

        // Assert
        // Render an new instance of the ForecastDataTable, passing in the test data
        var expectedDataTable = RenderComponent<ForecastDataTable>((nameof(ForecastDataTable.Forecasts), forecasts));
        // Assert that the CUT has two changes, one removal of the loading message and one addition which matched the
        // rendered HTML from the expectedDataTable.
        cut.GetChangesSinceFirstRender().ShouldHaveChanges(
            diff => diff.ShouldBeRemoval("<p><em>Loading...</em></p>"),
            diff => diff.ShouldBeAddition(expectedDataTable)
        );
    }
}
```

- In `Test001` we use the `Services.AddService` method to register the dependency and the performs a regular "initial render" verification.

- `Test002` creates a new instance of the mock service and registers that with the the service provider. It then renders the CUT and uses `WaitForNextRender` to pass the test data to the mock services task, which then completes and the CUT gets the data.

- In the assert step we expect the CUT to use a `ForecastDataTable` to render the forecast data. Thus, to make our assertion more simple and stable to changes, we render an instance of the `ForecastDataTable` use that to verify that the expected addition after the CUT receives the forecast data is as it should be.

## Dispatching `@on-events` during testing

In the previous sections we have seen a few examples of method calls that trigger `@on-event` handlers, e.g. `cut.Find(selector).Click()` that triggers the `@onclick` event handler attached to the element that matches the search query.

The following triggers are currently available in PascalCase, without the `@on`-prefix. E.g. the `@onbeforeactivate` event is available as `BeforeActivate()` in various overloads. I expect to add the missing events soon.

The currently available event triggers are:

```
// General events
@onactivate
@onbeforeactivate
@onbeforedeactivate
@ondeactivate
@onended
@onfullscreenchange
@onfullscreenerror
@onloadeddata
@onloadedmetadata
@onpointerlockchange
@onpointerlockerror
@onreadystatechange
@onscroll

// Focus events
@onfocus
@onblur
@onfocusin
@onfocusout

// Input events
@onchange
@oninput
@oninvalid
@onreset
@onselect
@onselectstart
@onselectionchange
@onsubmit

// Keyboard events
@onkeydown
@onkeyup
@onkeypress

// Mouse events
@onmouseover
@onmouseout
@onmousemove
@onmousedown
@onmouseup
@onclick
@ondblclick
@onwheel
@onmousewheel
@oncontextmenu
```
