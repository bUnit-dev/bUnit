[![GitHub tag (latest SemVer pre-release)](https://img.shields.io/github/v/tag/egil/bunit?include_prereleases&logo=github&style=flat-square)](https://github.com/egil/bunit/releases)
[![Nuget](https://img.shields.io/nuget/dt/bunit?logo=nuget&style=flat-square)](https://www.nuget.org/packages/bunit/)
[![Issues Open](https://img.shields.io/github/issues/egil/bunit.svg?style=flat-square&logo=github)](https://github.com/egil/bunit/issues)
[![Gitter](https://img.shields.io/gitter/room/egil/bunit?logo=gitter&style=flat-square)](https://gitter.im/egil/bunit?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge)

# bUnit - a testing library for Blazor components

<a href="https://www.telerik.com/campaigns/blazor/free-trial-1?utm_source=egilhansen&utm_medium=cpm&utm_campaign=blazor-trial-readme-sponsored-message"><img align="right" width="200" src="https://raw.githubusercontent.com/egil/bUnit/main/docs/site/sponsors/telerik-ad-github.svg"  /></a>

**bUnit** is a testing library for Blazor Components. Its goal is to make it easy to write _comprehensive, stable unit tests_. With bUnit, you can:

- Setup and define components under tests using C# or Razor syntax
- Verify outcome using semantic HTML comparer
- Interact with and inspect components as well as triggering event handlers
- Pass parameters, cascading values and inject services into components under test
- Mock `IJsRuntime` and Blazors authentication and authorization
- Perform snapshot testing

bUnit builds on top of existing unit testing frameworks such as xUnit, NUnit, and MSTest, which runs the Blazor components tests just as any normal unit test. 

**Go to [bUnit.egilhansen.com](https://bunit.egilhansen.com) to learn more.**

### NuGet Downloads

bUnit is available on NuGet in various incarnations. If you are using xUnit as your general purpose testing framework, you can use `bunit`, which includes everything in one package. If you want to use NUnit or MStest, then pick `bunit.core` and `bunit.web`:

| Name | Description | NuGet Download Link |
| ----- | ----- | ---- |
| [bUnit.web](https://www.nuget.org/packages/bunit.web/) | Adds support for testing Blazor components for the web. This includes bUnit.core. | [![Nuget](https://img.shields.io/nuget/dt/bunit.web?logo=nuget&style=flat-square)](https://www.nuget.org/packages/bunit.web/) | 
| [bUnit.xUnit](https://www.nuget.org/packages/bunit.xunit/) | Adds additional support for using bUnit with xUnit, including support for Razor-based tests. | [![Nuget](https://img.shields.io/nuget/dt/bunit.xunit?logo=nuget&style=flat-square)](https://www.nuget.org/packages/bunit.xunit/) |
| [bUnit.core](https://www.nuget.org/packages/bunit.core/) | Core library that enables rendering a Blazor component in a test context. | [![Nuget](https://img.shields.io/nuget/dt/bunit.core?logo=nuget&style=flat-square)](https://www.nuget.org/packages/bunit.core/) | 
| [bUnit.template](https://www.nuget.org/packages/bunit.template/) | Template, which currently creates an xUnit based bUnit test projects only | [![Nuget](https://img.shields.io/nuget/dt/bunit.template?logo=nuget&style=flat-square)](https://www.nuget.org/packages/bunit.template/) | 

To get started, head to the [getting started documentation](https://bunit.egilhansen.com/docs/getting-started) to learn more.

## Sponsors

A huge thank you to the [sponsors of my work with bUnit](https://github.com/sponsors/egil). The higher tier sponsors are:

<table>
	<tr>
		<td align="center" width="120">
			<a src="https://github.com/Progress-Telerik"><img src="https://avatars3.githubusercontent.com/u/57092419?s=460&u=fd421a2b423c3cad85866976935df3d4bec2ace3&v=4" alt="@Progress-Telerik" class="avatar" width="72" height="72" /></a><br/><a href="https://github.com/Progress-Telerik">Progress-Telerik</a>
		</td>
		<td align="center" width="120">
			<a src="https://github.com/hassanhabib"><img src="https://avatars0.githubusercontent.com/u/1453985?s=460&v=4" alt="Hassan Rezk Habib (@hassanhabib)" width="72" height="72" class="avatar" /></a><br/><a href="https://github.com/hassanhabib">Hassan Rezk Habib (@hassanhabib)</a>
		</td>
	</tr>
</table>

## Milestones to v1.0.0

These are the current goals that should be reached before v1.0.0 is ready:

- **Stabilize the APIs**, such that they work equally well with both xUnit, NUnit, and MSTest as the underlying test framework. The general goals are to make it easy for developers to create the tests they need, and to fall into the pit of success.
- **Get the Razor-based testing to be stable**, e.g. make the discovery and running of tests defined in .razor files stable and efficient. This includes adding support for NUnit and MSTest as test runners.
- **Improve the documentation**. It’s a good idea to get an experienced technical editor to review the documentation, making sure it is clear and understandable. In addition to this, more ‘How to’ guides are planned in the [Update Docs](https://github.com/egil/bunit/issues?q=is%3Aopen+is%3Aissue+milestone%3A%22updated+docs%22) milestone.
- **Join the .NET Foundation.**. This project is too large for one person to be the owner and the sole maintainer, so the plan is to apply for membership as soon as possible - most likely close to or after v1.0.0 ships - and to get the required support and guidance to ensure the project long term.

In the post-v1.0.0 to v1.0.x time frame, focus will be on improving performance. 
In particular, it would be nice to reduce the spin-up time of about one second.

## Contributors

Shout outs and a big thank you [to all the contributors](https://github.com/egil/bunit/graphs/contributors) to the library, including those that raise issues, provide input to issues, and those who send pull requests. Thank you!
