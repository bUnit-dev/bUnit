---
uid: home
title: bUnit - a testing library for Blazor components
---

[![GitHub tag (latest SemVer pre-release)](https://img.shields.io/github/v/tag/egil/bunit?include_prereleases&logo=github&style=flat-square)](https://github.com/egil/bunit/releases)
[![Nuget](https://img.shields.io/nuget/dt/bunit?logo=nuget&style=flat-square)](https://www.nuget.org/packages/bunit/)
[![Issues Open](https://img.shields.io/github/issues/egil/bunit.svg?style=flat-square&logo=github)](https://github.com/egil/bunit/issues)
[![Gitter](https://img.shields.io/gitter/room/egil/bunit?logo=gitter&style=flat-square)](https://gitter.im/egil/bunit?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge)

# bUnit: a testing library for Blazor components

**bUnit** is a testing library for Blazor Components. Its goal is to make it easy to write _comprehensive, stable_ unit tests. With bUnit, you can:

- Setup and define components under tests using C# or Razor syntax
- Verify outcomes using semantic HTML comparer
- Interact with and inspect components as well as trigger event handlers
- Pass parameters, cascading values and inject services into components under test
- Mock `IJSRuntime`, Blazor authentication and authorization, and others

bUnit builds on top of existing unit testing frameworks such as xUnit, NUnit, and MSTest, which run the Blazor components tests in just the same way as any normal unit test. bUnit runs a test in milliseconds, compared to browser-based UI tests which usually take seconds to run. 

**Go to the [Documentation](xref:getting-started) pages to learn more.**

### Test example 

Let’s write a test for the `<Counter>` component listed below. This comes with the standard Blazor project template which verifies that the counter corrects increments when the button is clicked:

[!code-cshtml[Counter.razor](../samples/components/Counter.razor)]

To do this, you can carry out the following using bUnit and xUnit:

[!code-csharp[CounterTest.cs](../samples/tests/xunit/CounterTestWithCtx.cs#L8-L21)]

This test uses bUnit's test context to render the `Counter` component with the `RenderComponent` method. It then finds the button the component rendered and clicks it with the `Find` and `Click` methods. Finally, it finds the paragraph (`<p>`) element and verifies that it matches the expected markup passed to the MarkupMatches method.

**Go to the [Documentation](xref:getting-started) pages to learn more.**

### NuGet downloads

bUnit is available on NuGet in various incarnations. Most should just pick the [bUnit](https://www.nuget.org/packages/bunit/) package:

| Name | Description | NuGet Download Link |
| ----- | ----- | ---- |
| [bUnit](https://www.nuget.org/packages/bunit/) | Includes the bUnit.core and bUnit.web packages. | [![Nuget](https://img.shields.io/nuget/dt/bunit?logo=nuget&style=flat-square)](https://www.nuget.org/packages/bunit/) |
| [bUnit.core](https://www.nuget.org/packages/bunit.core/) | Core library that enables rendering a Blazor component in a test context. | [![Nuget](https://img.shields.io/nuget/dt/bunit.core?logo=nuget&style=flat-square)](https://www.nuget.org/packages/bunit.core/) | 
| [bUnit.web](https://www.nuget.org/packages/bunit.web/) | Adds support for testing Blazor components for the web. This includes bUnit.core. | [![Nuget](https://img.shields.io/nuget/dt/bunit.web?logo=nuget&style=flat-square)](https://www.nuget.org/packages/bunit.web/) | 
| [bUnit.template](https://www.nuget.org/packages/bunit.template/) | Template, which currently creates xUnit-based bUnit test projects only | [![Nuget](https://img.shields.io/nuget/dt/bunit.template?logo=nuget&style=flat-square)](https://www.nuget.org/packages/bunit.template/) | 

## Sponsors

A huge thank you to the [sponsors of my work with bUnit](https://github.com/sponsors/egil). The higher tier sponsors are:

<table class="sponsors">
	<tr>
		<td align="center">
			<a src="https://github.com/Progress-Telerik"><img src="https://avatars3.githubusercontent.com/u/57092419?s=460&u=fd421a2b423c3cad85866976935df3d4bec2ace3&v=4" alt="@Progress-Telerik" width="40" height="40" /></a><br/><a href="https://github.com/Progress-Telerik">Progress-Telerik</a>
		</td>
		<td align="center">
			<a src="https://github.com/hassanhabib"><img src="https://avatars0.githubusercontent.com/u/1453985?s=460&v=4" alt="Hassan Rezk Habib (@hassanhabib)" width="40" height="40" /></a><br/><a href="https://github.com/hassanhabib">Hassan Rezk Habib (@hassanhabib)</a>
		</td>
	</tr>
</table>

## Contributors

Shout outs and a big thank you [to all the contributors](https://github.com/egil/bunit/graphs/contributors) to the library, including those who raise issues, those who provide input to issues, and those who send pull requests. 

**Want to help out? You can help in a number of ways:**

- Provide feedback and input through [issues](https://github.com/egil/bunit/issues), via [Twitter](https://twitter.com/egilhansen) or by using the [bUnit Gitter chat room](https://gitter.im/egil/bunit).
- Help build the library. Just pick an issue and submit pull-requests.
- Help write documentation.
- Create blog posts, presentations or video tutorials. If you do, I'll be happy to showcase them in the [related section](xref:external-resources) on this site.
