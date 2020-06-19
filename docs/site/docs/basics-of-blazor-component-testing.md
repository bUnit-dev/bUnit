# Basics of Blazor component testing

To test a component, you first have to render it with parameters, cascading values, and services passed into it. Then, you need access to the component's instance and the markup it has produced, so you can inspect and interact with both.

There are three different ways of doing this in the library:

1. **C# based tests**  
   With C# based tests, you write all your testing logic in C# files, i.e. like regular unit tests.
2. **Razor based tests**  
   With Razor based tests, you write tests in `.razor` files, which allows you to declare, in Razor syntax, the component under test and other markup fragments you need. You still write your assertions via C# in the .razor file, inside `@code {...}` blocks.
3. **Snapshot tests**  
   Snapshot tests are written in `.razor` files. A test contains a definition of an input markup/component and the expected output markup. The library will then automatically perform an semantic HTML comparison. Very little C# is needed in this, usually only to configure services.

In _Snapshot testing_, the rendering and verification is automatic.

For _C# based tests_ and _Razor based tests_, we have the following concepts to help us render our components and markup fragments:

- `ITestContext` for rendering using the RenderComponent method. The test context also allows you to configure services that should be available during rendering of components.

And the following concepts to help us access the rendered markup and component:

- `IRenderedFragment` is returned when a fragment is rendered. It has query methods (`Find` and `FindAll`) for querying the rendered markup using CSS selectors. It also provides access to the raw markup via the `Markup` property and to a DOM tree representation of the rendered markup via the `Nodes` property. The library also provides extension methods attached to `elements` in the DOM tree, that allow you to trigger attached Razor event handlers, e.g. an `@onclick` event handler on a `<button @onclick="...">`.

  _NOTE:_ The DOM tree implementation is provided by the [AngleSharp](https://anglesharp.github.io/) library, which provides a full HTML5 compatible implementation of DOM APIs. That means you can use all the DOM APIs you know from the browser to inspect the rendered nodes.

- `IRenderedComponent<TComponent>` extends `IRenderedFragment` with methods for rendering a component again with new parameters if needed, and a property for accessing the instance of the component.

This is the basics of how components and markup is rendered and accessed for verification and further inspection.
