# Contributing to the Documentation

This folder contains the documentation site for bUnit.

Here is a small getting started guide for contributing to the documentation...

## Structures

1. The `site` folder contains the code for generating the documentation site, _and_ the documentation in markdown files, located in `site/docs`.
2. The `samples` folder projects for the sample code that is displayed in the documentation. It has the following projects:  
  - `samples/components`: A Blazor component library project where components under test are placed.
  - `samples/tests/mstest`: An MSTest project where MSTest test samples are placed.
  - `samples/tests/nunit`: An NUnit project where NUnit test samples are placed.
  - `samples/tests/razor`: An xUnit-based test project where razor test samples are placed.
  - `samples/tests/xunit`: An xUnit project where xUnit C#-only test samples are placed.
  
These sample components, source files, and tests source files are included in the documentation using [DocFx's Code Snippet syntax](https://dotnet.github.io/docfx/spec/docfx_flavored_markdown.html?tabs=tabid-1%2Ctabid-a#code-snippet). They are created as real projects, making them runnable, which helps ensure that the code samples shown in the documentation pages are correct and in working order.  

## Building and Viewing Docs Locally

The following chapter describes how you can run and view the documentation locally

### Requirements
You will need the following tool installed:
* The [`dotnet serve` tool](https://github.com/natemcmaster/dotnet-serve)
* [DocFx](https://dotnet.github.io/docfx/tutorial/docfx_getting_started).

### View the documentation

1. Build the `bunit.sln` solution in the root folder.
2. From `docs/site` run `dotnet build`. If you get warnings from running `dotnet build`, try running it again.
3. Inside `docs/site` run `dotnet run` to start the generation.
4. From `docs/` run `serve-docs.cmd`. This will start up a local web server, hosting the generated documentation site.
5. After changing samples from `docs/samples`, run `dotnet test`. This will compile the sample components and run the sample tests.

## Documentation Writing Rules

- All pages should have a [YAML header](https://dotnet.github.io/docfx/spec/docfx_flavored_markdown.html#yaml-header) with a `UID` to make it easy to [cross reference](https://dotnet.github.io/docfx/spec/docfx_flavored_markdown.html#cross-reference) between pages
- All page and code references should be created using the [`xref:UID` cross reference syntax](https://dotnet.github.io/docfx/tutorial/links_and_cross_references.html#using-cross-reference).
-   By default, you should include code snippets as sample files in the `samples` projects, using the [code snippet syntax](https://dotnet.github.io/docfx/spec/docfx_flavored_markdown.html#code-snippet).
- All code snippets should use 2 spaces as an indention unit (1 tab = 2 spaces).
