---
uid: home
title: bUnit - a Testing Library for Blazor Components
---

[![GitHub tag (latest SemVer pre-release)](https://img.shields.io/github/v/tag/egil/bunit?include_prereleases&logo=github&style=flat-square)](https://github.com/egil/bunit/releases)
[![Nuget](https://img.shields.io/nuget/dt/bunit?logo=nuget&style=flat-square)](https://www.nuget.org/packages/bunit/)
[![Issues Open](https://img.shields.io/github/issues/egil/bunit.svg?style=flat-square&logo=github)](https://github.com/egil/bunit/issues)
[![Gitter](https://img.shields.io/gitter/room/egil/bunit?logo=gitter&style=flat-square)](https://gitter.im/egil/bunit?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge)

# bUnit: a Testing Library for Blazor Components

**bUnit** is a testing library for Blazor Components. Its goal is to make it easy to write _comprehensive, stable_ unit tests. With bUnit, you can:

- Setup and define components under tests using C# or Razor syntax
- Verify outcomes using semantic HTML comparer
- Interact with and inspect components as well as trigger event handlers
- Pass parameters, cascading values and inject services into components under test [__AP: Are 'cascading values' things that are passed, like parameters, or should one say 'Pass parameters, cascade values and inject services...?__]
- Mock `IJSRuntime` and Blazor authentication and authorization
- Perform snapshot testing

bUnit builds on top of existing unit testing frameworks such as xUnit, NUnit, and MSTest, which [__AP: What does this 'which' refer to -- bUnit or xUnit et al? If the former, then would be better to say 'and' rather than 'which'.__] runs [__AP: If the 'which' referred not to bUnit but to xUnit et al, then this 'runs' should be 'run'.__] the Blazor components tests in just the same way as any normal unit test. bUnit runs a test in milliseconds, compared to browser-based UI tests which usually take seconds to run. 

**Go to the [Documentation](xref:getting-started) pages to learn more.**

### Test Example 

Let’s write a test for the `<Counter>` component listed below. This comes with the standard Blazor project template which verifies that the counter corrects increments when the button is clicked:

[!code-cshtml[Counter.razor](../samples/components/Counter.razor)]

To do this, you can carry out the following using bUnit and xUnit:

[!code-csharp[CounterTest.cs](../samples/tests/xunit/CounterTestWithCtx.cs#L8-L21)]

This test uses bUnit’s test context to render the ‘Counter’ component with the ‘RenderComponent’ method. It then finds the button the component rendered and clicks it with the ‘Find’ and ‘Click’ methods. Finally, it finds the paragraph (`<p>`) element and verifies that it matches the expected markup passed to the MarkupMatches method.

**Go to the [Documentation](xref:getting-started) pages to learn more.**


### NuGet Downloads

bUnit is available on NuGet in various incarnations. If you are using xUnit as your general purpose testing framework, you can use `bunit`, which includes everything in one package. If you want to use NUnit or MStest, then pick `bunit.core` and `bunit.web`:

| Name | Description | NuGet Download Link |
| ----- | ----- | ---- |
| [bUnit.web](https://www.nuget.org/packages/bunit.web/) | Adds support for testing Blazor components for the web. This includes bUnit.core. | [![Nuget](https://img.shields.io/nuget/dt/bunit.web?logo=nuget&style=flat-square)](https://www.nuget.org/packages/bunit.web/) | 
| [bUnit.xUnit](https://www.nuget.org/packages/bunit.xunit/) | Adds additional support for using bUnit with xUnit, including support for Razor-based tests. | [![Nuget](https://img.shields.io/nuget/dt/bunit.xunit?logo=nuget&style=flat-square)](https://www.nuget.org/packages/bunit.xunit/) |
| [bUnit.core](https://www.nuget.org/packages/bunit.core/) | Core library that enables rendering a Blazor component in a test context. | [![Nuget](https://img.shields.io/nuget/dt/bunit.core?logo=nuget&style=flat-square)](https://www.nuget.org/packages/bunit.core/) | 
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

## Milestones to v1.0.0

Going forward, we have a variety of milestones to reach. These are the current goals that should be reached before v1.0.0 is ready:

- **Stabilize the APIs**, such that they work equally well with both xUnit, NUnit, and MSTest as the underlying test framework. The general goal is to make it easy for developers to create their required tests successfully.
- **Get the Razor-based testing to stable**, e.g. make the discovery and running of tests defined in .razor files stable and efficient. This includes adding support for NUnit and MSTest as test runners.
- **Improve the documentation**. It is a good idea to get an experienced technical editor to review the documentation, making sure it is clear and understandable. In addition to this, more ‘How to’ guides are planned in the [Update Docs](https://github.com/egil/bunit/issues?q=is%3Aopen+is%3Aissue+milestone%3A%22updated+docs%22) milestone.
- **Join the .NET Foundation.**. This project is too large for one person to act as owner and sole maintainer, so the plan is to apply for membership as soon as possible, most likely close to or after v1.0.0 ships, and get the support and guidance needed to ensure the project's long term future.

In the post-v1.0.0 to v1.0.x time frame, focus will be on improving performance. In particular, it would be nice to reduce the spin-up time of about one second.

## Contributors

Shout outs and a big thank you [to all the contributors](https://github.com/egil/bunit/graphs/contributors) to the library, both those that raise issues, provide input to issues, and those who send pull requests. 

**Want to help out? You can help in a number of ways:**

- Provide feedback and input through [issues](https://github.com/egil/bunit/issues), [Twitter](https://twitter.com/egilhansen) or [bUnit Gitter chat room](https://gitter.im/egil/bunit).
- Help build the library, just pick an issue and submit pull-requests.
- Help write documentation.
- Create blog posts, presentations or video tutorials. If you do, I will be happy to showcase them in the [related section](xref:external-resources) on this site.
<!--stackedit_data:
eyJoaXN0b3J5IjpbMTg1NzYyODc4MiwtMTU2NTQzODU1NCwtMT
A1MjU2OTg0MCwxOTQ2NTU2MDc3XX0=
-->