[![GitHub tag](https://img.shields.io/github/v/tag/bUnit-dev/bUnit?include_prereleases&logo=github&style=flat-square)](https://github.com/bUnit-dev/bUnit/releases)
[![Nuget](https://img.shields.io/nuget/dt/bunit.core?logo=nuget&style=flat-square)](https://www.nuget.org/packages/bunit/)
[![Issues Open](https://img.shields.io/github/issues/bUnit-dev/bUnit.svg?style=flat-square&logo=github)](https://github.com/bUnit-dev/bUnit/issues)

# bUnit - a testing library for Blazor components

<a href="https://www.telerik.com/blazor-ui?utm_source=egilhansen&utm_medium=cpm&utm_campaign=blazor-trial-readme-sponsored-message#gh-light-mode-only">
<img align="right" width="300" src="https://raw.githubusercontent.com/bUnit-dev/bUnit/main/docs/site/sponsors/progress-ad-2022-light-mode.svg#gh-light-mode-only" />
</a>
<a href="https://www.telerik.com/blazor-ui?utm_source=egilhansen&utm_medium=cpm&utm_campaign=blazor-trial-readme-sponsored-message#gh-dark-mode-only">
<img align="right" width="300" src="https://raw.githubusercontent.com/bUnit-dev/bUnit/main/docs/site/sponsors/progress-ad-2022-dark-mode.svg#gh-dark-mode-only" />
</a>

**bUnit** is a testing library for Blazor Components. Its goal is to make it easy to write _comprehensive, stable_ unit tests. With bUnit, you can:

- Setup and define components under tests using C# or Razor syntax
- Verify outcomes using semantic HTML comparer
- Interact with and inspect components as well as trigger event handlers
- Pass parameters, cascading values and inject services into components under test
- Mock `IJSRuntime`, Blazor authentication and authorization, and others

bUnit builds on top of existing unit testing frameworks such as xUnit, NUnit, and MSTest, which run the Blazor component tests in just the same way as any normal unit test. bUnit runs a test in milliseconds, compared to browser-based UI tests which usually take seconds to run.

**Go to [bUnit.dev](https://bunit.dev) to learn more.**

### NuGet Downloads

bUnit is available on NuGet in various incarnations. Most should just pick the [bUnit](https://www.nuget.org/packages/bunit/) package:

| Name | Description | NuGet Download Link |
| ----- | ----- | ---- |
| [bUnit](https://www.nuget.org/packages/bunit/) | Includes the bUnit.core and bUnit.web packages. | [![Nuget](https://img.shields.io/nuget/dt/bunit?logo=nuget&style=flat-square)](https://www.nuget.org/packages/bunit/) |
| [bUnit.core](https://www.nuget.org/packages/bunit.core/) | Core library that enables rendering a Blazor component in a test context. | [![Nuget](https://img.shields.io/nuget/dt/bunit.core?logo=nuget&style=flat-square)](https://www.nuget.org/packages/bunit.core/) |
| [bUnit.web](https://www.nuget.org/packages/bunit.web/) | Adds support for testing Blazor components for the web. This includes bUnit.core. | [![Nuget](https://img.shields.io/nuget/dt/bunit.web?logo=nuget&style=flat-square)](https://www.nuget.org/packages/bunit.web/) |
| [bUnit.template](https://www.nuget.org/packages/bunit.template/) | Template, which currently creates xUnit-based bUnit test projects only. | [![Nuget](https://img.shields.io/nuget/dt/bunit.template?logo=nuget&style=flat-square)](https://www.nuget.org/packages/bunit.template/) |
| [bUnit.generators](https://www.nuget.org/packages/bunit.generators/)|Source code generators to minimize code setup in various situations.|[![Nuget](https://img.shields.io/nuget/dt/bunit.generators?logo=nuget&style=flat-square)](https://www.nuget.org/packages/bunit.generators/)|
| [bUnit.web.query](https://www.nuget.org/packages/bunit.web.query/)|bUnit implementation of testing-library.com's query APIs.|[![Nuget](https://img.shields.io/nuget/dt/bunit.web.query?logo=nuget&style=flat-square)](https://www.nuget.org/packages/bunit.web.query/)|

To get started, head to the [getting started documentation](https://bunit.dev/docs/getting-started) to learn more.

## Sponsors

A huge thank you to the [sponsors of my work with bUnit](https://github.com/sponsors/egil). The higher tier sponsors are:

<table border="0">
	<tr>
		<td align="center" width="120">
			<a href="https://github.com/Progress-Telerik">
				<img src="https://avatars.githubusercontent.com/u/57092419?s=460" alt="@Progress-Telerik" class="avatar" width="72" height="72" />
				<br />
				Progress Telerik
			</a>
		</td>
		<td align="center" width="120">
			<a href="https://github.com/syncfusion">
				<img class="avatar" src="https://avatars.githubusercontent.com/u/1699795?s=460" width="72" height="72" alt="@syncfusion" />
				<br />
				Syncfusion
			</a>
		</td>
	</tr>
</table>

## Contributors

Shout outs and a big thank you [to all the contributors](https://github.com/bUnit-dev/bUnit/graphs/contributors) to the library, including those that raise issues, provide input to issues, and those who send pull requests. Thank you!

These good people have contributed code or documentation to bUnit:

<a href="https://github.com/bUnit-dev/bUnit/graphs/contributors">
  <img src="https://contrib.rocks/image?repo=bUnit-dev/bUnit" />
</a>

## Code of conduct

This project has adopted the code of conduct defined by the Contributor Covenant to clarify expected behavior in our community.
For more information see the [.NET Foundation Code of Conduct](https://dotnetfoundation.org/code-of-conduct).

## .NET Foundation

This project is supported by the [.NET Foundation](https://dotnetfoundation.org).
