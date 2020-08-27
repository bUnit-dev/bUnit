[![GitHub Workflow Status](https://img.shields.io/github/workflow/status/egil/bunit/RELEASE?logo=github&style=flat-square)](https://github.com/egil/bunit/actions?query=workflow%3ARELEASE)
[![GitHub tag (latest SemVer pre-release)](https://img.shields.io/github/v/tag/egil/bunit?include_prereleases&logo=github&style=flat-square)](https://github.com/egil/bunit/releases)
[![Nuget](https://img.shields.io/nuget/dt/bunit?logo=nuget&style=flat-square)](https://www.nuget.org/packages/bunit/)
[![Issues Open](https://img.shields.io/github/issues/egil/bunit.svg?style=flat-square&logo=github)](https://github.com/egil/bunit/issues)
[![Gitter](https://img.shields.io/gitter/room/egil/bunit?logo=gitter&style=flat-square)](https://gitter.im/egil/bunit?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge)

# bUnit - a testing library for Blazor components

**bUnit** is a testing library for Blazor Components. Its goal is to make it easy to write _comprehensive, stable unit tests_. You can:

- Setup and define components under tests in C# or Razor syntax
- Verify outcome using semantic HTML comparer
- Interact with and inspect components
- Trigger event handlers
- Provide cascading values
- Inject services
- Mock `IJsRuntime`
- Perform snapshot testing

bUnit builds on top of existing unit testing frameworks such as xUnit, NUnit, and MSTest, which runs the Blazor components tests, just as any normal unit test. 

**Go to [bUnit.egilhansen.com](https://bunit.egilhansen.com) to learn more.**

### NuGet downloads

bUnit is available on NuGet in various incarnations. If you are using xUnit as your general purpose testing framework, you can use `bunit`, which includes everything in one package. If you want to use NUnit or MStest, then pick `bunit.core` and `bunit.web`:

| Name | Type | NuGet Download Link |
| ----- | ----- | ---- |
| bUnit | Library, includes core, web, and xUnit | [![Nuget](https://img.shields.io/nuget/dt/bunit?logo=nuget&style=flat-square)](https://www.nuget.org/packages/bunit/) | 
| bUnit.core | Library, only core | [![Nuget](https://img.shields.io/nuget/dt/bunit.core?logo=nuget&style=flat-square)](https://www.nuget.org/packages/bunit.core/) | 
| bUnit.web | Library, web and core | [![Nuget](https://img.shields.io/nuget/dt/bunit.web?logo=nuget&style=flat-square)](https://www.nuget.org/packages/bunit.web/) | 
| bUnit.xUnit |Library, xUnit and core | [![Nuget](https://img.shields.io/nuget/dt/bunit.xunit?logo=nuget&style=flat-square)](https://www.nuget.org/packages/bunit.xunit/) | 
| bUnit.template | Template, which currently creates an xUnit based bUnit test projects only | [![Nuget](https://img.shields.io/nuget/dt/bunit.template?logo=nuget&style=flat-square)](https://www.nuget.org/packages/bunit.template/) | 

To get started, head to the [getting started documentation](https://bunit.egilhansen.com/docs/getting-started) to learn more.

### Sponsors

A hugh thank you to the [sponsors of my work with bUnit](https://github.com/sponsors/egil). The higher tire sponsors include:

- [Hassan Rezk Habib (@hassanhabib)](https://github.com/hassanhabib)

## Milestones to v1.0.0

These are the current goals that should be reached before v1.0.0 is ready:

- **Stabilize the APIs**, such that they work equally well with both xUnit, NUnit, and MSTest as the underlying test framework. The general goals is to make it easy and obvious for developers to create the tests they needed, and fall into the pit of success.
- **Get the Razor-based testing to stable**, e.g. make the discovery and running of tests defined in .razor files stable and efficient. This includes adding support for NUnit and MSTest as test runners.
- **Improve the documentation**. Currently there are a list of "How to" guides planned in the [Update Docs](https://github.com/egil/bunit/issues?q=is%3Aopen+is%3Aissue+milestone%3A%22updated+docs%22) milestone.
- **Join the .NET Foundation.**. This project is too large for one person to be the owner and be the sole maintainer of, so the plan is to apply for membership as soon as possible, most likely close to or after v1.0.0 ships, and get the needed support and guidance to ensure the project long term.

In the post v1.0.0 to v1.0.x time frame, focus will be on improving performance. Especially the spin-up time of about one second would be nice to get reduced.

## Contributors

Shout outs and a big thank you [to all the contributors](https://github.com/egil/bunit/graphs/contributors) to the library, both those that raise issues, provide input to issues, and those who send pull requests. Thank you!
