# Blazor Component Testing

This framework's goal is to make it easy to write _comprehensive, stable unit tests_ for Blazor Components/Razor Components.

1. [Introduction](#introduction)
2. [Getting started](#getting-started)
3. [Examples](#examples)
4. [References](#references)
5. [Contribute](#contribute)

## Introduction

**To make tests easy to write**, the framework provides a few different ways of define the **component under test** (CUT):

1. Render components from C# code via the `RenderComponent<TComponent>(params...)` method, that allow you to easily pass component parameters, cascading values, event callbacks to the component.

2. Define components and other markup in Razor syntax using the `<Fixture>` component, and execute the _act_ and _assert_ steps inside `@code { }` blocks in the Razor file (experimental).

3. Create _snapshot tests_ in Razor syntax that automatically compare the rendered input to the expected output (experimental).

**To make it easier to create stable tests**, the library comes with growing set of assertion helpers and strategies. You can:

1. Inspect components state/properties directly.
2. Query the rendered HTML and assert against that directly. This is supported by [AngleSharp](https://anglesharp.github.io/)'s full implementation of the HTML5 DOM API.
3. Perform a stable semantic comparison the rendered HTML from a component under test with expected HTML. This is supported by the [AngleSharp.Diffing](https://github.com/AngleSharp/AngleSharp.Diffing) library, which e.g. will ignore insignificant whitespace, attribute order on elements, order of classes in `class="..."` attributes, handle boolean attributes correctly, among other things.

## Getting started

Follow these steps to set up a new test project:

1. Create a new Razor Class Library (`dotnet new razorclasslib`).
2. Add the following package references to your testing library:

   - `Razor.Components.Testing.Library`
   - `Microsoft.NET.Test.Sdk`
   - `xunit.core`
   - `xunit.assert`
   - `xunit.runner.visualstudio` (if using Visual Studio)

   Optionally, but recommended packages are [`Moq`](https://github.com/Moq) and [`Shouldly`](https://github.com/shouldly). _Moq_ is a good generic mocking library, and _Shouldly_ is a fluent syntax assert library, that makes test more readable and produce easily readable assert errors.

3. Add a reference to the Blazor (class library) project(s) where the components you want to test is located.

4. Start writing tests (see examples below to get started).

## Examples

Examples are split into three sections, one for each style/declaration type.

1. [C#-based tests](csharp-examples.md)
2. [Razor/C# mixed-mode tests](razor-examples.md)
3. [Snapshot tests](snapshot-examples.md)

## References

## Contribute

To get in touch, ask questions or provide feedback, you can:

- Create a new [issue](https://github.com/egil/razor-components-testing-library/issues)
- Join the library's Gitter channel ([![Gitter](https://badges.gitter.im/razor-components-testing-library/community.svg)](https://gitter.im/razor-components-testing-library/community?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge))
- Ping me on Twitter ([@egilhansen](https://twitter.com/egilhansen))

There are a few ways you can help improve this library.

1. Tell me if a certain type of component/scenario is hard to test. Create an issue with a minimal example of the component and the kind of assertions you would like to perform.

2. Suggest tweaks to the library's API or assertion helpers (create issue).

3. Find a bug or mistake in the library, create an issue, or even better, send in a pull request.

4. Help with documentation and/or good examples. If you figured out a elegant way to test a scenario, share it through an issue, or add it to the samples project (pull request), or add it to the documentation (pull request).
