---
uid: getting-started
title: Getting Started with bUnit
---

# Getting Started with bUnit

To get started writing tests for your Blazor components, you first need to set up a test project. Then you can start writing your tests, using either C# or Razor syntax. 

These three pages will teach you *the basics*:

1. **<xref:create-test-project>** covers setting up a bUnit test project.
2. **<xref:writing-csharp-tests>** covers the basics of writing tests in C#.  
   With C# based tests, you write all your testing logic in C# files, i.e. like regular unit tests.
3. **<xref:writing-razor-tests>** covers the basics of writing tests in Razor and C# syntax.  
   With Razor based tests, you write tests in `.razor` files, which allows you to declare, in Razor syntax, the component under test and other markup fragments you need. You still write your assertions via C# in the .razor file, inside `@code {...}` blocks.

After reading those, these topics will take you to the *next level*:

1. **[Providing different types of input](xref:providing-input)** to a component under test, e.g. passing parameters or injecting services.
2. **[Verifying output in various ways](xref:verification)** from a component under test, e.g. inspecting the rendered markup.
3. **[Mocking dependencies](xref:mocking)** a component under test has, e.g. the `IJsRuntime` or `HttpClient`.

## Examples

To see examples of how to work assert and manipulate a rendered component, go to the following pages:

- [C# test examples in the sample project](https://github.com/egil/bunit/tree/master/sample/tests/Tests)
- [Razor based test examples in the sample project](https://github.com/egil/bunit/tree/master/sample/tests/RazorTestComponents)
- [Snapshot test examples in the sample project](https://github.com/egil/bunit/tree/master/sample/tests/SnapshotTests)

