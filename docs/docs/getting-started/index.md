---
uid: getting-started
title: Getting started with bUnit
---

# Getting started with bUnit

To get started writing tests for your Blazor components, you first need to set up a test project. Then you can start writing your tests, using either C# or Razor syntax.

1. **<xref:create-test-project>** covers setting up a bUnit test project.
2. **<xref:writing-csharp-tests>** covers the basics of writing tests in C#.  
   With C# based tests, you write all your testing logic in C# files, i.e. like regular unit tests.
3. **<xref:writing-razor-tests>** covers the basics of writing tests in Razor and C# syntax.  
   With Razor based tests, you write tests in `.razor` files, which allows you to declare, in Razor syntax, the component under test and other markup fragments you need. You still write your assertions via C# in the .razor file, inside `@code {...}` blocks.

## Examples

To see examples of how to work assert and manipulate a rendered component, go to the following pages:

- [C# test examples in the sample project](https://github.com/egil/bunit/tree/master/sample/tests/Tests)
- [Razor based test examples in the sample project](https://github.com/egil/bunit/tree/master/sample/tests/RazorTestComponents)
- [Snapshot test examples in the sample project](https://github.com/egil/bunit/tree/master/sample/tests/SnapshotTests)

