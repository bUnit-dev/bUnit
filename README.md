[![GitHub tag (latest SemVer pre-release)](https://img.shields.io/github/v/tag/egil/bunit?include_prereleases&logo=github&style=flat-square)](https://github.com/egil/bunit/releases)
[![Nuget](https://img.shields.io/nuget/dt/bunit?logo=nuget&style=flat-square)](https://www.nuget.org/packages/bunit/)
[![Issues Open](https://img.shields.io/github/issues/egil/bunit.svg?style=flat-square&logo=github)](https://github.com/egil/bunit/issues)
[![Gitter](https://img.shields.io/gitter/room/egil/bunit?logo=gitter&style=flat-square)](https://gitter.im/egil/bunit?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge)

# bUnit - a testing library for Blazor components

<a href="https://www.telerik.com/campaigns/blazor/free-trial-1?utm_source=egilhansen&utm_medium=cpm&utm_campaign=blazor-trial-readme-sponsored-message"><img align="right" width="200" src="https://raw.githubusercontent.com/egil/bUnit/main/docs/site/sponsors/telerik-ad-github.svg"  /></a>

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
| [bUnit.template](https://www.nuget.org/packages/bunit.template/) | Template, which currently creates xUnit-based bUnit test projects only | [![Nuget](https://img.shields.io/nuget/dt/bunit.template?logo=nuget&style=flat-square)](https://www.nuget.org/packages/bunit.template/) | 

To get started, head to the [getting started documentation](https://bunit.dev/docs/getting-started) to learn more.

## Sponsors

A huge thank you to the [sponsors of my work with bUnit](https://github.com/sponsors/egil). The higher tier sponsors are:

<table border="0">
	<tr>
		<td align="center" width="120">
			<a href="https://github.com/syncfusion"><img class="avatar" src="https://avatars.githubusercontent.com/u/1699795?s=460&amp;v=4" width="72" height="72" alt="@syncfusion"></a><br><a href="https://github.com/syncfusion">Syncfusion</a>
		</td>		
		<td align="center" width="120">
			<a src="https://github.com/Progress-Telerik"><img src="https://avatars.githubusercontent.com/u/57092419?s=460&amp;v=4" alt="@Progress-Telerik" class="avatar" width="72" height="72"></a><br><a src="https://github.com/Progress-Telerik">Progress-Telerik</a>
		</td>
		<td align="center" width="120">
			<a src="https://github.com/hassanhabib"><img src="https://avatars.githubusercontent.com/u/1453985?s=460&amp;v=4" alt="Hassan Rezk Habib (@hassanhabib)" width="72" height="72" class="avatar"></a><br><a src="https://github.com/hassanhabib">Hassan Rezk Habib</a>
		</td>
	</tr>
</table>

## Contributors

Shout outs and a big thank you [to all the contributors](https://github.com/egil/bunit/graphs/contributors) to the library, including those that raise issues, provide input to issues, and those who send pull requests. Thank you!
