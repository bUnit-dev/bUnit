# Changelog

All notable changes to **bUnit** will be documented in this file. The project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

<!-- The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/) -->

## [UNRELEASED]

### Added
List of new features.

### Changed
List of changes in existing functionality.

### Removed
List of now removed features.

### Fixed
List of any bug fixes.

## [1.0.0-preview-01] - 2020-12-24

The following section list all changes in 1.0.0 preview 01.

### Added
List of new features.

- Added support for casting `BUnitJSRuntime` to `IJSInProcessRuntime` and `IJSUnmarshalledRuntime`. By [@KristofferStrube](https://github.com/KristofferStrube) in [#279](https://github.com/egil/bUnit/pull/279)

- Added support for triggering `@ontoggle` event handlers through a dedicated `Toggle()` method. By [@egil](https://github.com/egil) in [#256](https://github.com/egil/bUnit/pull/256).

- Added out of the box support for `<Virtualize>` component. When a `<Virtualize>` component is used in a component under test, it's JavaScript interop-calls are faked by bUnits JSInterop, and it should result in all items being rendered immediately. By [@egil](https://github.com/egil) in [#240](https://github.com/egil/bUnit/issues/240).

- Added support for components that call `ElementReference.FocusAsync`. These calls are handled by the bUnits JSInterop, that also allows you to verify that `FocusAsync` has been called for a specific element. For example, if a component has rendered an `<input>` element, then the following code will verify that it has been focused using `FocusAsync`:

  ```csharp
  var cut = RenderComponent<FocusingComponent>();

  var input = cut.Find("input");

  JSInterop.VerifyFocusAsyncInvoke()
    .Arguments[0] // the first argument is the ElemenetReference
    .ShouldBeElementReferenceTo(input);
  ```

  By [@egil](https://github.com/egil) in [#260](https://github.com/egil/bUnit/pull/260).

- Added `Render(RenderFragment)` and `Render<TComponent>(RenderFragment)` methods to `TestContext`, as well as various overloads to the `MarkupMatches` methods, that also takes a `RenderFragment` as the expected value.

  The difference between the generic `Render` method and the non-generic one is that the generic returns an `IRenderedComponent<TComponent>`, whereas the non-generic one returns a `IRenderedFragment`.

  Calling `Render<TComponent>(RenderFragent)` is equivalent to calling `Render(RenderFragment).FindComponent<TComponent>()`, e.g. it returns the first component in the render tree of type `TComponent`. This is different from the `RenderComponent<TComponent>()` method, where `TComponent` _is_ the root component of the render tree.

  The main usecase for these are when writing tests inside .razor files. Here the inline syntax for declaring render fragments make these methods very useful.

  For example, to tests the `<Counter>` page/component that is part of new Blazor apps, do the following (inside a `CounterTest.razor` file):

  ```cshtml
  @code
  {
    [Fact]
    public void Counter_Increments_When_Button_Is_Clicked()
    {
      using var ctx = new TestContext();
      var cut = ctx.Render(@<Counter />);

      cut.Find("button").Click();

      cut.Find("p").MarkupMatches(@<p>Current count: 1</p>);
    }
  }
  ```

  Note: This example uses xUnit, but NUnit or MSTest works equally well.

  In addition to the new `Render` methods, a empty `BuildRenderTree` method has been added to the `TestContext` type. This makes it possible to inherit from the `TestContext` type in test components, removing the need for newing up the `TestContext` in each test.

  This means the test component above ends up looking like this:

  ```cshtml
  @inherts TestContext
  @code
  {
    [Fact]
    public void Counter_Increments_When_Button_Is_Clicked()
    {
      var cut = Render(@<Counter />);

      cut.Find("button").Click();

      cut.Find("p").MarkupMatches(@<p>Current count: 1</p>);
    }
  }
  ```

  Tip: If you have multiple test components in the same folder, you can add a `_Imports.razor` file inside it and add the `@inherits TestContext` statement in that, removing the need to add it to every test component.

  By [@egil](https://github.com/egil) in [#262](https://github.com/egil/bUnit/pull/262).

- Added support for `IJSRuntime.InvokeAsync<IJSObjectReference>(...)` calls from components. There is now a new setup helper methods for configuring how invocations towards JS modules should be handled. This is done with the various `SetupModule` methods available on the `BunitJSInterop` type available through the `TestContext.JSInterop` property. For example, to set up a module for handling calls to `foo.js`, do the following:

  ```c#
  using var ctx = new TestContext();
  var moduleJsInterop = ctx.JSInterop.SetupModule("foo.js");
  ```

  The returned `moduleJsInterop` is a `BunitJSInterop` type, which means all the normal `Setup<TResult>` and `SetupVoid` methods can be used to configure it to handle calls to the module from a component. For example, to configure a handler for a call to `hello` in the `foo.js` module, do the following:

  ```c#
  moduleJsInterop.SetupVoid("hello");
  ```

  By [@egil](https://github.com/egil) in [#288](https://github.com/egil/bUnit/pull/288).

- Added support for registering services in bUnits `Services` collection that implements `IAsyncDisposable`. Suggested by [@jmaillet](https://github.com/jmaillet) in [#249](https://github.com/egil/bUnit/issues/249). 

### Changed
List of changes in existing functionality.

- bUnit's mock IJSRuntime has been moved to an "always on" state by default, in strict mode, and is now available through `TestContext`'s `JSInterop` property. This makes it possible for first party Blazor components like the `<Virtualize>` component, which depend on JSInterop, to "just work" in tests.

  **Compatible with previous releases:** To get the same effect as calling `Services.AddMockJSRuntime()` in beta-11, which used to add the mock IJSRuntime in "loose" mode, you now just need to change the mode of the already on JSInterop, i.e. `ctx.JSInterop.Mode = JSRuntimeMode.Loose`.

  **Inspect registered handlers:** Since the new design allows registering invoke handlers in the context of the `TestContext`, you might need to get already registered handlers in your individual tests. This can be done with the `TryGetInvokeHandler()` method, that will return handler that can handle the parameters passed to it. E.g. to get a handler for a `IJSRuntime.InvokaAsync<string>("getValue")`, call `ctx.JSInterop.TryGetInvokeHandler<string>("getValue")`.

  Learn more [issue #237](https://github.com/egil/bUnit/issues/237). By [@egil](https://github.com/egil) in [#247](https://github.com/egil/bUnit/pull/247).

- The `Setup<TResult>(string identifier, Func<IReadOnlyList<object?>, bool> argumentsMatcher)` and `SetupVoid(string identifier, Func<IReadOnlyList<object?>, bool> argumentsMatcher)` methods in bUnits JSInterop/MockJSRuntime has a new second parameter, an `InvocationMatcher`.

  The `InvocationMatcher` type is a delegate that receives a `JSRuntimeInvoation` and returns true. The `JSRuntimeInvoation` type contains the arguments of the invocation and the identifier for the invocation. This means old code using the `Setup` and `SetupVoid` methods should be updated to use the arguments list in `JSRuntimeInvoation`, e.g., change the following call:

   `ctx.JSInterop.Setup<string>("foo", args => args.Count == 2)` to this:  
   `ctx.JSInterop.Setup<string>("foo", invocation => invocation.Arguments.Count == 2)`.

  Changed added in relation to [#240](https://github.com/egil/bUnit/issues/240) in [#257](https://github.com/egil/bUnit/issues/257) by [@egil](https://github.com/egil).

- Changed `AddTestAuthorization` such that it works in Razor-based test contexts, i.e. on the `Fixture` and `SnapshotTest` types.

### Removed
List of now removed features.

- A few bUnit internal xUnit assert helper methods, the custom `ShouldAllBe` methods, has mistakingly been part of the bunit.xunit package. These have been removed.

### Fixed
List of any bug fixes.

- When an `Add` call to the component parameter collection builder was used to select a parameter that was inherited from a base component, the builder incorrectly reported the selected property/parameter as missing on the type. Reported by [@nickmuller](https://github.com/nickmuller) in [#250](https://github.com/egil/bUnit/issues/250).

- When an element, found in the DOM tree using the `Find()`, method was removed because of an event handler trigger on it, e.g. an `cut.Find("button").Click()` event trigger method, an `ElementNotFoundException` was thrown. Reported by [@nickmuller](https://github.com/nickmuller) in [#251](https://github.com/egil/bUnit/issues/251).

- In the built-in fake authentication system in bUnit, roles and claims were not available in components through the a cascading parameter of type `Task<AuthenticationState>`. Reported by [@AFAde](https://github.com/AFAde) in [#253](https://github.com/egil/bUnit/discussions/253) and fixed in [#291](https://github.com/egil/bUnit/pull/291) by [@egil](https://github.com/egil).

## [1.0.0-beta 11] - 2020-10-26

The following section list all changes in beta-11.

### Added
List of new features.

- Two new overloads to the `RenderFragment()` and `ChildContent()` component parameter factory methods have been added that takes a `RenderFragment` as input. By [@egil](https://github.com/egil) in [#203](https://github.com/egil/bUnit/pull/203).

- Added a `ComponentParameterCollection` type. The `ComponentParameterCollection` is a collection of component parameters, that knows how to turn those components parameters into a `RenderFragment`, which will render a component and pass any parameters inside the collection to that component. That logic was spread out over multiple places in bUnit, and is now owned by the `ComponentParameterCollection` type. By [@egil](https://github.com/egil) in [#203](https://github.com/egil/bUnit/pull/203).

- Added additional placeholder services for `NavigationManager`, `HttpClient`, and `IStringLocalizer`, to make it easier for users to figure out why a test is failing due to missing service registration before rendering a component. By [@joro550](https://github.com/joro550) in [#223](https://github.com/egil/bUnit/pull/223).

- Added `Key` class that represents a keyboard key and helps to avoid constructing `KeyboardEventArgs` object manually. The key can be passed to `KeyPress`, `KeyDown`, or `KeyUp` helper methods to raise keyboard events. The `Key` class provides static special keys or can be obtained from character or string. Keys can be combined with key modifiers: `Key.Enter + Key.Alt`.

  For example, this makes it easier to trigger keyboard events on an element:
  
  ```csharp
  var cut = ctx.RenderComponent<ComponentWithKeyboardEvents>();
  var element = cut.Find("input");
  
  element.KeyDown(Key.Enter + Key.Control); // Triggers onkeydown event with Ctrl + Enter
  element.KeyUp(Key.Control + Key.Shift + 'B'); // Triggers onkeyup event with Ctrl + Shift  + B
  element.KeyPress('1'); // Triggers onkeypress event with key 1
  element.KeyDown(Key.Alt + "<"); // Triggers onkeydown event with Alt + <
  ```

  By [@duracellko](https://github.com/duracellko) in [#101](https://github.com/egil/bUnit/issues/101).

- Added support for registering/adding components to a test context root render tree, which components under test is rendered inside. This allows you to simplify the "arrange" step of a test when a component under test requires a certain render tree as its parent, e.g. a cascading value.
  
  For example, to pass a cascading string value `foo` to all components rendered with the test context, do the following:

  ```csharp
  ctx.RenderTree<CascadingValue<string>>(parameters => parameters.Add(p => p.Value, "foo"));
  var cut = ctx.RenderComponent<ComponentReceivingFoo>();
  ```

  By [@egil](https://github.com/egil) in [#236](https://github.com/egil/bUnit/pull/236).

- Added "catch-all" `Setup` method to bUnit's mock JS runtime, that allows you to specify only the type when setting up a planned invocation. By [@nemesv](https://github.com/nemesv) in [#234](https://github.com/egil/bUnit/issues/234).

### Changed
List of changes in existing functionality.

- The `ComponentParameterBuilder` has been renamed to `ComponentParameterCollectionBuilder`, since it now builds the `ComponentParameterCollection` type, introduced in this release of bUnit. By [@egil](https://github.com/egil) in [#203](https://github.com/egil/bUnit/pull/203).

- `ComponentParameterCollectionBuilder` now allows adding cascading values that is not directly used by the component type it targets. This makes it possible to add cascading values to children of the target component. By [@egil](https://github.com/egil) in [#203](https://github.com/egil/bUnit/pull/203).

- The `Add(object)` has been replaced by `AddCascadingValue(object)` in `ComponentParameterCollectionBuilder`, to make it more clear that an unnamed cascading value is being passed to the target component or one of its child components. It is also possible to pass unnamed cascading values using the `Add(parameterSelector, value)` method, which now correctly detect if the selected cascading value parameter is named or unnamed. By [@egil](https://github.com/egil) in [#203](https://github.com/egil/bUnit/pull/203).

- It is now possible to call the `Add()`, `AddChildContent()` methods on `ComponentParameterCollectionBuilder`, and the factory methods `RenderFragment()`, `ChildContent()`, and `Template()`, _**multiple times**_ for the same parameter, if it is of type `RenderFragment` or `RenderFragment<TValue>`. Doing so previously would either result in an exception or just the last passed `RenderFragment` to be used. Now all the provided `RenderFragment` or `RenderFragment<TValue>` will be combined at runtime into a single `RenderFragment` or `RenderFragment<TValue>`.

  For example, this makes it easier to pass e.g. both a markup string and a component to a `ChildContent` parameter:
   
  ```csharp
  var cut = ctx.RenderComponent<Component>(parameters => parameters
    .AddChildContent("<h1>Below you will find a most interesting alert!</h1>")
    .AddChildContent<Alert>(childParams => childParams
      .Add(p => p.Heading, "Alert heading")
      .Add(p => p.Type, AlertType.Warning)
      .AddChildContent("<p>Hello World</p>")
    )
  );
  ```
  By [@egil](https://github.com/egil) in [#203](https://github.com/egil/bUnit/pull/203).

- All test doubles are now in the same namespace, `Bunit.TestDoubles`. So all import statements for `Bunit.TestDoubles.JSInterop` and `Bunit.TestDoubles.Authorization` must be changed to `Bunit.TestDoubles`. By [@egil](https://github.com/egil) in [#223](https://github.com/egil/bUnit/pull/223).

- Marked MarkupMatches methods as assertion methods to stop SonarSource analyzers complaining about missing assertions in tests. By [@egil](https://github.com/egil) in [#229](https://github.com/egil/bUnit/pull/229).

- `AddTestAuthorization` now extends `TestContext` instead of `TestServiceProvider`, and also automatically adds the `CascadingAuthenticationState` component to the root render tree. [@egil](https://github.com/egil) in [#237](https://github.com/egil/bUnit/pull/367).

### Removed
List of now removed features.

- The async event dispatcher helper methods have been removed (e.g. `ClickAsync()`), as they do not provide any benefit. If you have an event that triggers async operations in the component under test, instead use `cut.WaitForState()` or `cut.WaitForAssertion()` to await the expected state in the component.  

### Fixed
List of any bug fixes.

- Using the ComponentParameterCollectionBuilder's `Add(p => p.Param, value)` method to add a unnamed cascading value didn't create an unnnamed cascading value parameter. By [@egil](https://github.com/egil) in [#203](https://github.com/egil/bUnit/pull/203). Credits to [Ben Sampica (@benjaminsampica)](https://github.com/benjaminsampica) for reporting and helping investigate this issue.
- Triggered events now bubble correctly up the DOM tree and triggers other events of the same type. This is a **potentially breaking change,** since this changes the behaviour of event triggering and thus you might see tests start breaking as a result hereof. By [@egil](https://github.com/egil) in [#119](https://github.com/egil/bUnit/issues/119).

## [1.0.0-beta 10] - 2020-09-15

The following section list all changes in beta-10.

### Added
List of new features.

- Added support for .NET 5 RC-1.

### Changed
List of changes in existing functionality.

- Related to [#189](https://github.com/egil/bUnit/issues/189), a bunch of the core `ITestRenderer` and related types have changed. The internals of `ITestRenderer` is now less exposed and the test renderer is now in control of when rendered components and rendered fragments are created, and when they are updated. This enables the test renderer to protect against race conditions when the `FindComponent`, `FindComponents`, `RenderFragment`, and `RenderComponent` methods are called.

### Fixed
List of any bug fixes.

- Fixes [#189](https://github.com/egil/bUnit/issues/189): The test renderer did not correctly protect against a race condition during initial rendering of a component, and that could in some rare circumstances cause a test to fail when it should not. This has been addressed in this release with a major rewrite of the test renderer, which now controls and owns the rendered component and rendered fragment instances which is created when a component is rendered. By [@egil](https://github.com/egil) in [#201](https://github.com/egil/bUnit/pull/201). Credits to [@Smurf-IV](https://github.com/Smurf-IV) for reporting and helping investigate this issue.

## [1.0.0-beta-9] - 2020-08-26

This release contains a couple of fixes, and adds support for .NET Preview 8 and later. There are no breaking changes in this release.

Thanks to [pharry22](https://github.com/pharry22) for submitting fixes and improvements to the documentation.

### Added
List of new features.

- Added `InvokeAsync(Func<Task>)` to `RenderedComponentInvokeAsyncExtensions`. By [@JeroenBos](https://github.com/JeroenBos) in [#151](https://github.com/egil/bUnit/pull/177).
- Added `ITestRenderer Renderer { get ; }` to `IRenderedFragment` to make it possible to simplify the `IRenderedComponentBase<TComponent>` interface. By [@JeroenBos](https://github.com/JeroenBos) in [#151](https://github.com/egil/bUnit/pull/177).
- Added support for scoped CSS to `MarkupMatches` and related comparer methods. By [@egil](https://github.com/egil) in [#195](https://github.com/egil/bUnit/pull/195).

### Changed
List of changes in existing functionality.

- Moved `InvokeAsync()`, `Render()` and `SetParametersAndRender()` methods out of `IRenderedComponentBase<TComponent>` into extension methods. By [@JeroenBos](https://github.com/JeroenBos) in [#151](https://github.com/egil/bUnit/pull/177).
- Accessing `Markup`, `Nodes` and related methods on a rendered fragment whose underlying component has been removed from the render tree (disposed) now throws a `ComponentDisposedException`. By [@egil](https://github.com/egil) in [#184](https://github.com/egil/bUnit/pull/184).
- Changed bUnit's build to target both .net 5.0 and .net standard 2.1. By [@egil](https://github.com/egil) in [#187](https://github.com/egil/bUnit/pull/187).

### Fixed
List of any bug fixes.

- Fixes [#175](https://github.com/egil/bUnit/issues/175): When a component referenced in a test, e.g. through the `FindComponent()` method was removed from the render tree, accessing the reference could caused bUnit to look for updates to it in the renderer, causing a exception to be thrown. By [@egil](https://github.com/egil) in [#184](https://github.com/egil/bUnit/pull/184).

## [1.0.0-beta-8] - 2020-07-15

Here is beta-8, a small summer vacation release this time. A few needed additions, especially around testing components that use Blazor's authentication and authorization. In addition to this, a lot of documentation has been added to https://bunit.egilhansen.com/docs/getting-started/.

### Added
List of new features.

- Authorization fakes added to make it much easier to test components that use authentication and authorization. Learn more in the [Faking Blazor's Authentication and Authorization](https://bunit.egilhansen.com/docs/test-doubles/faking-auth) page. By [@DarthPedro](https://github.com/DarthPedro) in [#151](https://github.com/egil/bUnit/pull/151).

- Added `MarkupMatches(this string actual ...)` extension methods. Make it easier to compare just the text content from a DON text node with a string, while still getting the benefit of the semantic HTML comparer.

### Changed
List of changes in existing functionality.

- `TestContextBase.Dispose` made virtual to allow inheritor's to override it. By [@SimonCropp](https://github.com/SimonCropp) in [#137](https://github.com/egil/bunit/pull/137).
 
- **[Breaking change]** Changed naming convention for JSMock feature and moved to new namespace, `Bunit.TestDoubles.JSInterop`. All classes and methods containing `Js` (meaning JavaScript) renamed to `JS` for consistency with Blazor's `IJSRuntime`. By [@yourilima](https://github.com/yourilima) in [#150](https://github.com/egil/bUnit/pull/150)

## [1.0.0-beta-7] - 2020-05-19## [1.0.0-beta-7] - 2020-05-19

There are three big changes in bUnit in this release, as well as a whole host of small new features, improvements to the API, and bug fixes. The three big changes are:

1. A splitting of the library
2. Discovery of razor base tests, and
3. A strongly typed way to pass parameters to a component under test.

There are also some breaking changes, which we will cover first.

**NOTE:** The documentation is next on the TODO list, so please bear with me while I update it to reflect all the recent changes.

### Breaking changes

Due to the big restructuring of the library, there are some breaking changes, hopefully for the better.

#### Razor test changes

Previously, the `Test` and `Setup` methods on `<Fixture>` and `<SnapshotTest>` did not have any arguments, and the test context they represented when running, was implicitly available in the scope. This has changed with this release, such that all `Test` and `Setup` methods now receive the text context as an argument, and that should be used to call e.g. `GetComponentUnderTest()` on.

For example, if you have a razor based test that looks like this currently:

```c#
<Fixture Test="Test001" Setup="TestSetup">
    <ComponentUnderTest><Counter /></ComponentUnderTest>
    <Fragment>...</Fragment>
</Fixture>
@code {
    void TestSetup() => Services.AddMockJsRuntime();

    void Test001()
    {
        var cut = GetComponentUnderTest<Counter>();
        var fragment = GetFragment();
    }
}
```

You have to change it to this:

```c#
<Fixture Test="Test001" Setup="TestSetup">
    <ComponentUnderTest><Counter /></ComponentUnderTest>
</Fixture>
@code {
    // Add a Fixture fixture argument to the setup method and use
    // the services collection inside the fixture to register dependencies
    void TestSetup(Fixture fixture) => fixture.Services.AddMockJsRuntime();

    // Add a Fixture fixture argument to the test method
    void Test001(Fixture fixture) 
    {
        // Use the fixture instance to get the component under test
        var cut = fixture.GetComponentUnderTest<Counter>();
        var fragment = fixture.GetFragment();
    }
}
```

It is a little more typing, but it is also a lot more obvious what is going on, e.g. where the component under test or fragment is coming from.

In addition to this, the `Tests` and `TestsAsync` methods on `<Fixture>` have been deprecated in this release and throws a runtime exception if used. They were not very used and caused confusion about the state of the components under test between the method calls. Now you can only specify either a `Test` or `TestAsync` method per `<Fixture>`.

#### WaitForRender removed

The `WaitForRender` method has been removed entirely from the library. Since it would only wait for one render, it had a very specific use case, where as the more general `WaitForAssertion` or `WaitForState` will wait for any number of renders, until the assertion passes, or the state predicate returns true. These make them much better suited to create stable tests.

With `WaitForRender`, you would pass in an action that would cause a render before attempting your assertion, e.g.:

```c#
cut.WaitForRender(() => mockForecastService.Task.SetResult(forecasts));

Assert.Equal("...", cut.Markup);
```

This can now be changed to first call the action that will trigger the render, and then wait for an assertion to pass, using `WaitForAssertion`:

```c#
mockForecastService.Task.SetResult(forecasts);

cut.WaitForAssertion(() => Assert.Equal("...", cut.Markup));
```

The two "wait for" methods are also only available through a rendered fragment or rendered component now.

#### ComponentTestFixture deprecated

Previously, the recommended method for creating xUnit component test classes was to inherit from `ComponentTestFixture`. Due to the restructuring of the library, this type is now just a `TestContext` with static component parameters factory methods, so it does not add much value anymore. 

The component parameter factory methods are now also available in the more general purpose `ComponentParameterFactory` type, which can be imported into all test classes, not just xUnit ones, using the `import static Bunit.ComponentParameterFactory` method, and then you can change your existing xUnit test classes to inherit from `TestContext` instead of `ComponentTestFixture` to keep the current functionality for xUnit test classes.

That covers the most important breaking changes. Now lets look at the other big changes.

### Splitting up the library

In this release sees bUnit refactored and split up into three different sub libraries. The reasons for doing this are:

- To make it possible to extract the direct dependency on xUnit and easily add support for NUnit or MSTest
- To make it easier to maintain distinct parts of the library going forward
- To enable future support for other non-web variants of Blazor, e.g. the Blazor Mobile Bindings.

The three parts of the library is now:

- **bUnit.core**: The core library only contains code related to the general Blazor component model, i.e. it is not specific to the web version of Blazor.
- **bUnit.web**: The web library, which has a dependency on core, provides all the specific types for rendering and testing Blazor web components.
- **bUnit.xUnit**: The xUnit library, which has a dependency on core, has xUnit specific extensions to bUnit, that enable logging to the test output through the `ILogger` interface in .net core, and an extension to xUnit's test runners, that enable it to discover and run razor based tests defined in `.razor` files.

To keep things compatible with previous releases, an additional package is available, **bUnit**, which includes all of three libraries. That means existing users should be able to keep their single `<PackageReference Include="bunit">` in their projects.

### Discovery of Razor based tests

One of the pain points of writing Razor based tests in `.razor` files was that the individual tests was not correctly discovered. That meant that if had multiple tests in a file, you would not see them in Visual Studios Test Explorer individually, you could not run them individually, and error was not reported individually.

This has changed with the _bUnit.xUnit_ library, that now includes a way for it to discover individual razor tests, currently either a `<Fixture>` or `<SnapshotTest>` inside test components defined in `.razor` files. It also enables you to navigate to the test by double clicking on it in the Test Explorer, and you can run each test individually, and see error reports individually.

**WARNING:** You still have to wait for the Blazor compiler to translate the `.razor` files into `.cs` files, before the tests show up in the Test Explorer, and the this can trip up the Test Explorer. So while this feature is a big improvement to razor based testing, it is still not perfect, and more works need to be done to refine it.

### Strongly typed component parameters

If you prefer writing your tests in C# only, you will be happy to know that there is now a new strongly typed way to pass parameters to components, using a builder. E.g., to render a `ContactInfo` component:

```c#
var cut = RenderComponent<ContactInfo>(parameters => parameters
    .Add(p => p.Name, "Egil Hansen")
    .Add(p => p.Country, "Iceland")
);
```

There are a bunch of different `Add` methods available on the builder, that allows you to easily pass in a `EventCallback`, `ChildContent`, or `RenderFragment`.

The old way using the component parameter factory methods are still available if you prefer that syntax.

NOTE: The parameter builder API is experimental at this point, and will likely change.

### NuGet downloads
The latest version of the library is available on NuGet in various incarnations:

| Name | Type | NuGet Download Link |
| ----- | ----- | ---- |
| bUnit | Library, includes core, web, and xUnit | [![Nuget](https://img.shields.io/nuget/dt/bunit?logo=nuget&style=flat-square)](https://www.nuget.org/packages/bunit/) | 
| bUnit.core | Library, only core | [![Nuget](https://img.shields.io/nuget/dt/bunit.core?logo=nuget&style=flat-square)](https://www.nuget.org/packages/bunit.core/) | 
| bUnit.web | Library, web and core | [![Nuget](https://img.shields.io/nuget/dt/bunit.web?logo=nuget&style=flat-square)](https://www.nuget.org/packages/bunit.web/) | 
| bUnit.xUnit |Library, xUnit and core | [![Nuget](https://img.shields.io/nuget/dt/bunit.xunit?logo=nuget&style=flat-square)](https://www.nuget.org/packages/bunit.xunit/) | 
| bUnit.template | Template, which currently creates an xUnit based bUnit test projects only | [![Nuget](https://img.shields.io/nuget/dt/bunit.template?logo=nuget&style=flat-square)](https://www.nuget.org/packages/bunit.template/) | 

### Contributions
Thanks to [Martin Stühmer (@samtrion)](https://github.com/samtrion) and [Stef Heyenrath (@StefH)](https://github.com/StefH) for their code contributions in this release, and to [Brad Wilson (@bradwilson)](https://github.com/bradwilson) for his help with enabling xUnit to discover and run Razor based tests.

Also a big thank to all you who have contributed by raising issues, participated in issues by helping answer questions and providing input on design and technical issues. 

### Added
- A new event, `OnAfterRender`, has been added to `IRenderedFragmentBase`, which `IRenderedFragment` inherits from. Subscribers will be invoked each time the rendered fragment is re-rendered. Related issue [#118](https://github.com/egil/bunit/issues/118).
- A new property, `RenderCount`, has been added to `IRenderedFragmentBase`, which `IRenderedFragment` inherits from. Its represents the number of times a rendered fragment has been rendered. Related issue [#118](https://github.com/egil/bunit/issues/118).
- A new event, `OnMarkupUpdated`, has been added to `IRenderedFragmentBase`. Subscribers will be notifid each time the rendered fragments markup has been regenerated. Related issue [#118](https://github.com/egil/bunit/issues/118).
-  Due to the [concurrency bug discovered](https://github.com/egil/bunit/issues/108), the entire render notification and markup notification system has been changed. 
- A new overload `RenderComponent()` and `SetParameterAndRender()`, which takes a `Action<ComponentParameterBuilder<TComponent>>` as input. That allows you to pass parameters to a component under test in a strongly typed way. Thanks to [@StefH](https://github.com/StefH) for the work on this. Related issues: [#79](https://github.com/egil/bunit/issues/79) and [#36](https://github.com/egil/bunit/issues/36).
- The two razor test types, `<Fixture>` and `<SnapshotTest>`, can now be **skipped**. by setting the `Skip="some reason for skipping"` parameter. Note, this requires support from the test runner, which current only includes bUnit.xUnit. Related issue: [#77](https://github.com/egil/bunit/issues/77).
- The two razor test types, `<Fixture>` and `<SnapshotTest>`, can now have a **timeout** specified, by setting the `Timeout="TimeSpan.FromSeconds(2)"` parameter. Note, this requires support from the test runner, which current only includes bUnit.xUnit.
- An `InvokeAsync` method has been added to the `IRenderedFragmentBase` type, which allows invoking of an action in the context of the associated renderer. Related issue: [#82](https://github.com/egil/bunit/issues/82).
- Enabled the "navigate to test" in Test Explorer. Related issue: [#106](https://github.com/egil/bunit/issues/106).
- Enabled xUnit to discover and run Razor-based tests. Thanks to [Brad Wilson (@bradwilson)](https://github.com/bradwilson) for his help with this. Related issue: [#4](https://github.com/egil/bunit/issues/4).

### Changed
- Better error description from `MarkupMatches` when two sets of markup are different.
- The `JsRuntimePlannedInvocation` can now has its response to an invocation set both before and after an invocation is received. It can also have a new response set at any time, which will be used for new invocations. Related issue: [#78](https://github.com/egil/bunit/issues/78).
- The `IDiff` assertion helpers like `ShouldHaveChanges` now takes an `IEnumerable<IDiff>` as input to make it easier to call in scenarios where only an enumerable is available. Related issue: [#87](https://github.com/egil/bunit/issues/87). 
- `TextContext` now registers all its test dependencies as services in the `Services` collection. This now includes the `HtmlParser` and `HtmlComparer`. Related issue: [#114](https://github.com/egil/bunit/issues/114).

### Deprecated
- The `ComponentTestFixture` has been deprecated in this release, since it just inherits from `TestContex` and surface the component parameter factory methods. Going forward, users are encouraged to instead inherit directly from `TestContext` in their xUnit tests classes, and add a `import static Bunit.ComponentParameterFactory` to your test classes, to continue to use the component parameter factory methods. Related issue: [#108](https://github.com/egil/bunit/issues/108).

### Removed
- `<Fixture>` tests no longer supports splitting the test method/assertion step into multiple methods through the `Tests` and `TestsAsync` parameters.
- `WaitForRender` has been removed entirely from the library, as the more general purpose `WaitForAssertion` or `WaitForState` covers its use case.
- `WaitForAssertion` or `WaitForState` is no longer available on `ITestContext` types. They are _still_ available on rendered components and rendered fragments.
- `CreateNodes` method has been removed from `ITextContext`. The ability to convert a markup string to a `INodeList` is available through the `HtmlParser` type registered in `ITextContext.Services` service provider.
- `RenderEvents` has been removed from `IRenderedFragment`, and replaced by the `OnMarkupUpdated` and `OnAfterRender` events. Related issue [#118](https://github.com/egil/bunit/issues/118).
- The generic collection assertion methods `ShouldAllBe<T>(this IEnumerable<T> collection, params Action<T, int>[] elementInspectors)` and `ShouldAllBe<T>(this IEnumerable<T> collection, params Action<T>[] elementInspectors)` have been removed from the library.

### Fixed
- A concurrency issue would surface when a component under test caused asynchronous renders that was awaited using the `WaitForRender`, `WaitForState`, or `WaitForAssertion` methods. Related issue [#118](https://github.com/egil/bunit/issues/118).
- `MarkupMatches` and the related semantic markup diffing, didn't correctly ignore the `__internal_stopPropagation_` and `__internal_preventDefault_` added by Blazor to the rendered markup, when users use the `:stopPropagation` and `:preventDefault` modifiers. Thanks to [@samtrion](https://github.com/samtrion) for reporting and solving this. Related issue: [#111](https://github.com/egil/bunit/issues/111).
- `cut.FindComponent<TComponent>()` didn't return the component inside the component under test. It now searches and finds the first child component of the specified type.

---

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

- **Added some of the Blazor frameworks end-2-end tests.** To get better test coverage of the many rendering scenarios supported by Blazor, the [ComponentRenderingTest.cs](https://github.com/dotnet/aspnetcore/blob/main/src/Components/test/E2ETest/Tests/ComponentRenderingTest.cs) tests from the Blazor frameworks test suite has been converted from a Selenium to a bUnit. The testing style is very similar, so few changes was necessary to port the tests. The two test classes are here, if you want to compare:

  -  [bUnit's ComponentRenderingTest.cs](/main/tests/BlazorE2E/ComponentRenderingTest.cs)
  -  [Blazor's ComponentRenderingTest.cs](https://github.com/dotnet/aspnetcore/blob/main/src/Components/test/E2ETest/Tests/ComponentRenderingTest.cs)

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
