# Changelog
All notable changes to **bUnit** will be documented in this file. The project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

## [1.0.0-beta-6] - 2020-03-01
This release includes a **name change from Blazor Components Testing Library to bUnit**. It also brings along two extra helper methods for working with asynchronously rendering components during testing, and a bunch of internal optimizations and tweaks to the code.

*Why change the name?* Naming is hard, and I initial chose a very product-namy name, that quite clearly stated what the library was for. However, the name isn't very searchable, since it just contains generic keywords, plus, bUnit is just much cooler. It also gave me the opportunity to remove my name from all the namespaces and simplify those.

### Contributions
Hugh thanks to [Rastislav Novotný (@duracellko)](https://github.com/duracellko)) for his input and review of the `WaitForX` logic added in this release.

### NuGet
The latest version of the library is availble on NuGet:

|  | Type | Link |
| ------------- | ----- | ---- |
| [![Nuget](https://img.shields.io/nuget/dt/bunit?logo=nuget&style=flat-square)](https://www.nuget.org/packages/bunit/) | Library | [https://www.nuget.org/packages/bunit/](https://www.nuget.org/packages/bunit/) | 
| [![Nuget](https://img.shields.io/nuget/dt/bunit.template?logo=nuget&style=flat-square)](https://www.nuget.org/packages/bunit.template/) | Template | [https://www.nuget.org/packages/bunit.template/](https://www.nuget.org/packages/bunit.template/) | 

### Added
- **`WaitForState(Func<bool> statePredicate, TimeSpan? timeout = 1 second)` has been added to `ITestContext` and `IRenderedFragment`.**  
  This method will wait (block) until the provided statePredicate returns true, or the timeout is reached (during debugging the timeout is disabled). Each time the renderer in the test context renders, or the rendered fragment renders, the statePredicate is evaluated. 
  
  You use this method, if you have a component under test, that requires _one or more asynchronous triggered renders_, to get to a desired state, before the test can continue. 

  The following example tests the `DelayedRenderOnClick.razor` component:

  ```cshtml
  // DelayedRenderOnClick.razor
  <p>Times Clicked: @TimesClicked</p>
  <button @onclick="ClickCounter">Trigger Render</button>
  @code
  {
      public int TimesClicked { get; private set; }
  
      async Task ClickCounter()
      {
          await Task.Delay(1); // wait 1 millisecond
          TimesClicked += 1;
      }
  }
  ```

  This is a test that uses `WaitForState` to wait until the component under test has a desired state, before the test continues:

  ```csharp
  [Fact]
  public void WaitForStateExample()
  {
      // Arrange
      var cut = RenderComponent<DelayedRenderOnClick>();
  
      // Act
      cut.Find("button").Click();
      cut.WaitForState(() => cut.Instance.TimesClicked == 1);
  
      // Assert
      cut.Find("p").TextContent.ShouldBe("Times Clicked: 1");
  }
  ```

- **`WaitForAssertion(Action assertion, TimeSpan? timeout = 1 second)` has been added to `ITestContext` and `IRenderedFragment`.**   
  This method will wait (block) until the provided assertion method passes, i.e. runs without throwing an assert exception, or until the timeout is reached (during debugging the timeout is disabled). Each time the renderer in the test context renders, or the rendered fragment renders, the assertion is attempted.

  You use this method, if you have a component under test, that requires _one or more asynchronous triggered renders_, to get to a desired state, before the test can continue. 

  This is a test that tests the `DelayedRenderOnClick.razor` listed above, and that uses `WaitForAssertion` to attempt the assertion each time the component under test renders:

  ```csharp
  [Fact]
  public void WaitForAssertionExample()
  {
      // Arrange
      var cut = RenderComponent<DelayedRenderOnClick>();
  
      // Act
      cut.Find("button").Click();
  
      // Assert
      cut.WaitForAssertion(
          () => cut.Find("p").TextContent.ShouldBe("Times Clicked: 1")
      );
  }
  ```

- **Added support for capturing log statements from the renderer and components under test into the test output.**   
  To enable this, add a constructor to your test classes that takes the `ITestOutputHelper` as input, then in the constructor call `Services.AddXunitLogger` and pass the `ITestOutputHelper` to it, e.g.:  

  ```csharp
  // ComponentTest.cs
  public class ComponentTest : ComponentTestFixture
  {
      public ComponentTest(ITestOutputHelper output)
      {
          Services.AddXunitLogger(output, minimumLogLevel: LogLevel.Debug);
      }
  
      [Fact]
      public void Test1() ...
  }
  ```
  
  For Razor and Snapshot tests, the logger can be added almost the same way. The big difference is that it must be added during *Setup*, e.g.:

  ```cshtml
  // RazorComponentTest.razor
  <Fixture Setup="Setup" ...>
      ...
  </Fixture>
  @code {
      private ITestOutputHelper _output;
      
      public RazorComponentTest(ITestOutputHelper output)
      {
          _output = output;
      }
      
      void Setup()
      {
          Services.AddXunitLogger(_output, minimumLogLevel: LogLevel.Debug);
      }
  }
  ```

- **Added simpler `Template` helper method**  
  To make it easier to test components with `RenderFragment<T>` parameters (template components) in C# based tests, a new `Template<TValue>(string name, Func<TValue, string> markupFactory)` helper methods have been added. It allows you to create a mock template that uses the `markupFactory` to create the rendered markup from the template. 

  This is an example of testing the `SimpleWithTemplate.razor`, which looks like this:

  ```cshtml
  @typeparam T
  @foreach (var d in Data)
  {
      @Template(d);      
  }
  @code
  {
      [Parameter] public RenderFragment<T> Template { get; set; }
      [Parameter] public IReadOnlyList<T> Data { get; set; } = Array.Empty<T>();
  }
  ```

  And the test code:

  ```csharp
  var cut = RenderComponent<SimpleWithTemplate<int>>(
      ("Data", new int[] { 1, 2 }),
      Template<int>("Template", num => $"<p>{num}</p>")
  );
  
  cut.MarkupMatches("<p>1</p><p>2</p>");
  ```

  Using the more general `Template` helper methods, you need to write the `RenderTreeBuilder` logic yourself, e.g.:

  ```csharp
  var cut = RenderComponent<SimpleWithTemplate<int>>(
      ("Data", new int[] { 1, 2 }),
      Template<int>("Template", num => builder => builder.AddMarkupContent(0, $"<p>{num}</p>"))
  );
  ```

- **Added logging to TestRenderer.** To make it easier to understand the rendering life-cycle during a test, the `TestRenderer` will now log when ever it dispatches an event or renders a component (the log statements can be access by capturing debug logs in the test results, as mentioned above).

- **Added some of the Blazor frameworks end-2-end tests.** To get better test coverage of the many rendering scenarios supported by Blazor, the [ComponentRenderingTest.cs](https://github.com/dotnet/aspnetcore/blob/master/src/Components/test/E2ETest/Tests/ComponentRenderingTest.cs) tests from the Blazor frameworks test suite has been converted from a Selenium to a bUnit. The testing style is very similar, so few changes was necessary to port the tests. The two test classes are here, if you want to compare:

  -  [bUnit's ComponentRenderingTest.cs](/master/tests/BlazorE2E/ComponentRenderingTest.cs)
  -  [Blazor's ComponentRenderingTest.cs](https://github.com/dotnet/aspnetcore/blob/master/src/Components/test/E2ETest/Tests/ComponentRenderingTest.cs)

### Changed
- **Namespaces is now `Bunit`**  
  The namespaces have changed from `Egil.RazorComponents.Testing.Library.*` to simply `Bunit` for the library, and `Bunit.Mocking.JSInterop` for the JSInterop mocking support.

- **Auto-refreshing `IElement`s returned from `Find()`**  
  `IRenderedFragment.Find(string cssSelector)` now returns a `IElement`, which internally will refresh itself, whenever the rendered fragment it was found in, changes. This means you can now search for an element once in your test and assign it to a variable, and then continue to assert against the same instance, even after triggering renders of the component under test.  

  For example, instead of having `cut.Find("p")` in multiple places in the same test, you can do `var p = cut.Find("p")` once, and the use the variable `p` all the places you would otherwise have the `Find(...)` statement.

- **Refreshable element collection returned from `FindAll`.**  
  The `FindAll` query method on `IRenderedFragment` now returns a new type, the `IRefreshableElementCollection<IElement>` type, and the method also takes a second optional argument now, `bool enableAutoRefresh = false`.

  The `IRefreshableElementCollection` is a special collection type that can rerun the query to refresh its the collection of elements that are found by the CSS selector. This can either be done manually by calling the `Refresh()` method, or automatically whenever the rendered fragment renders and has changes, by setting the property `EnableAutoRefresh` to `true` (default set to `false`).

  Here are two example tests, that both test the following `ClickAddsLi.razor` component:

  ```cshtml
  <ul>
      @foreach (var x in Enumerable.Range(0, Counter))
      {
          <li>@x</li>
      }
  </ul>
  <button @onclick="() => Counter++"></button>
  @code {
      public int Counter { get; set; } = 0;
  }
  ```
  
  The first tests uses auto refresh, set through the optional parameter `enableAutoRefresh` passed to FindAll:

  ```csharp
  public void AutoRefreshQueriesForNewElementsAutomatically()
  {
      var cut = RenderComponent<ClickAddsLi>();
      var liElements = cut.FindAll("li", enableAutoRefresh: true);
      liElements.Count.ShouldBe(0);
  
      cut.Find("button").Click();
  
      liElements.Count.ShouldBe(1);
  }
  ```

  The second test refreshes the collection manually through the `Refresh()` method on the collection:
  
  ```csharp
  public void RefreshQueriesForNewElements()
  {
      var cut = RenderComponent<ClickAddsLi>();
      var liElements = cut.FindAll("li");
      liElements.Count.ShouldBe(0);
  
      cut.Find("button").Click();
  
      liElements.Refresh(); // Refresh the collection
      liElements.Count.ShouldBe(1);
  }
  ```

- **Custom exception when event handler is missing.** Attempting to triggering a event handler on an element which does not have an handler attached now throws a `MissingEventHandlerException` exception, instead of an `ArgumentException`.

### Deprecated
- **`WaitForNextRender` has been deprecated (marked as obsolete)**, since the added `WaitForState` and `WaitForAssertion` provide a much better foundation to build stable tests on. The plan is to remove completely from the library with the final 1.0.0 release.

### Removed
- **`AddMockHttp` and related helper methods have been removed.**  
  The mocking of HTTPClient, supported through the [mockhttp](https://github.com/richardszalay/mockhttp) library, has been removed from the library. This was done because the library really shouldn't have a dependency on a 3. party mocking library. It adds maintenance overhead and uneeded dependencies to it.  
  
  If you are using mockhttp, you can easily add again to your testing project. See [TODO Guide to mocking HttpClient](#) in the docs to learn how.

### Fixed
- **Wrong casing on keyboard event dispatch helpers.**  
  The helper methods for the keyboard events was not probably cased, so that has been updated. E.g. from `Keypress(...)` to `KeyPress(...)`.