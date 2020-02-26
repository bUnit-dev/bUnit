# Changelog
All notable changes to this project will be documented in this file. The project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]
This release includes a name change from Blazor Components Testing Library to **bUnit**. It also brings along two extra helper methods for working with asynchronously rendering components during testing, and a bunch of internal optimizations and tweaks to the code.

*Why change the name?* Naming is hard, and I initial chose a very product-namy name, that quite clearly stated what the library was for. However, the name isn't very searchable, since it just contains generic keywords, plus, bUnit is just much cooler. It also gave me the opportunity to remove my name from all the namespaces and simplify those.

### Added
- **`WaitForState(Func<bool> statePredicate, TimeSpan? timeout = 1 second)` has been added to `ITestContext` and `IRenderedFragment`.**  
  
  This method will wait (block) until the provided statePredicate returns true, or the timeout is reached (during debugging the timeout is disabled). Each time the renderer in the test context renders, or the rendered fragment renders, the statePredicate is evaluated. 
  
  You use this method, if you have a component under test, that requires _one or more asynchronous triggered renders_, to get to a desired state, before the test can continue. 

- **`WaitForAssertion(Action assertion, TimeSpan? timeout = 1 second)` has been added to `ITestContext` and `IRenderedFragment`.**   

  This method will wait (block) until the provided assertion method passes, i.e. runs without throwing an assert exception, or until the timeout is reached (during debugging the timeout is disabled). Each time the renderer in the test context renders, or the rendered fragment renders, the assertion is attempted.

  You use this method, if you have a component under test, that requires _one or more asynchronous triggered renders_, to get to a desired state, before the test can continue. 

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

### Changed
- **Namespaces is now `Bunit`**  
  The namespaces have changed from `Egil.RazorComponents.Testing.Library.*` to simply `Bunit` for the library, and `Bunit.Mocking.JSInterop` for the JSInterop mocking support.

- **Auto-updating `IElement`s returned from `Find()`**  
  `IRenderedFragment.Find(string cssSelector)` now returns a `IElement`, which internally will update itself, whenever the rendered fragment it was found in, changes. This means you can now search for an element once in your test and assign it to a variable, and then continue to assert against the same instance, even after triggering renders of the component under test.  

  For example, instead of having `cut.Find("p")` in multiple places in the same test, you can do `var p = cut.Find("p")` once, and the use the variable `p` all the places you would otherwise have the `Find(...)` statement.

### Deprecated
- `WaitForNextRender` has been deprecated (marked as obsolete), since the added `WaitForState` and `WaitForAssertion` provide a much better foundation to build stable tests on. The plan is to remove completely from the library with the final 1.0.0 release.

- `AddMockHttp` and related helper methods for working with the [mockhttp](https://github.com/richardszalay/mockhttp) library has been removed from the library. This was done because the library really shouldn't have a dependency on a 3. party mocking library. It adds maintenance overhead and uneeded dependencies to it.  
  
  If you are using mockhttp, you can easily add again to your testing project. See [TODO Guide to mocking HttpClient](#) in the docs to learn how.

### Removed
### Fixed
### Security