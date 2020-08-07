---
uid: getting-started
title: Getting Started with bUnit
---

# Getting Started with bUnit

To start writing tests for Blazor components, first set up a test project and then you can start adding tests, using either using C# or Razor syntax.

The *basic* getting started topics are:

1. **<xref:create-test-project>** covers setting up a bUnit test project.
2. **<xref:writing-csharp-tests>** covers the basics of writing tests in C#, i.e. like regular unit tests.
3. **<xref:writing-razor-tests>** covers the basics of writing tests in `.razor` files in Razor and C# syntax.  

The *next level* topics are:

1. **[Providing different types of input](xref:providing-input)** to a component under test in C# based tests, e.g. passing parameters or injecting services.
2. **[Verifying output in various ways](xref:verification)** from a component under test, e.g. inspecting the rendered markup.
3. **[Mocking dependencies](xref:test-doubles)** a component under test has, e.g. the `IJsRuntime` or `HttpClient`.

## Getting Help

Cannot figure out how to write a test for a testing scenario? Found a testing scenario that is hard or inelegant with bUnit? Found a bug in bUnit? Head over to the [GitHub issues list](https://github.com/egil/bunit/issues) and ask a question, suggest a new feature, or join [bUnits Gitter channel](https://gitter.im/egil/bunit) and let us know. There are no stupid questions, and all questions are welcome!
