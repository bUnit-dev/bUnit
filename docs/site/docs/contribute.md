---
uid: contribute
title: Contribute
---

# How to Contribute

One of the easiest ways to contribute is to participate in discussions on GitHub issues. You can also contribute by submitting pull requests with code changes.

## General Feedback and Discussions
Start a discussion on the [bUnit discussion list](https://github.com/egil/bUnit/discussions).

## Bugs and feature requests
For bugs or feature requests, log a new issue on the [issues list](https://github.com/egil/bunit/issues). Be sure to use the right template.

## Contributing Code and Content

bUnit accepts fixes and features. Here is what you should do when writing code for bUnit:

- Follow the coding conventions used throughout the bUnit project. In general, they align with the AspNetCore teams [coding guidelines](https://github.com/dotnet/aspnetcore/wiki/Engineering-guidelines#coding-guidelines).
- Add, remove, or delete unit tests to cover your changes. Make sure tests are specific to the changes you are making. Tests need to be provided for every bug/feature that is completed.
- All code changes should be done on the `DEV` branch, and pull requests should target it.
- All updates to the documentation located under `./docs/` should be done on the `main` branch **if** they are general in nature and not tied to a specific version. Changes to the documentation related to changes on the `DEV` branch should be submitted to the `DEV` branch.
- Any code or documentation you share with the bUnit projects should fall under the projects license agreement.

Here are some resources to help you get started on how to contribute code or new content:

* ["Help wanted" issues](https://github.com/egil/bunit/labels/help%20wanted) - these issues are up for grabs if you want to create a fix. To do this, simply comment on the issue you want to fix.
* ["Good first issue" issues](https://github.com/egil/bunit/labels/good%20first%20issue) - these are good for newcomers. Good first issues are small, usually require just a few hours of work, and do not require a deep technical knowledge of bUnit. This is a good place to start if you want to become familiar with bUnit’s inner workings and maybe take on bigger issues later.

### Identifying the Scale of a Contribution

If you would like to contribute to bUnit, first identify the scale of what you would like to contribute. If it is small (grammar/spelling or a bug fix), feel free to start working on a fix. If you are submitting a feature or substantial code contribution, please discuss it with us first. 

You might also read these two blogs posts on contributing code: [Open Source Contribution Etiquette](http://tirania.org/blog/archive/2010/Dec-31.html) by Miguel de Icaza and [Don't "Push" Your Pull Requests](https://www.igvita.com/2011/12/19/dont-push-your-pull-requests/) by Ilya Grigorik. These blog posts highlight good open source collaboration etiquette and help align expectations between you and us.

All code submissions will be rigorously reviewed and tested, and only those that meet a high bar for both quality and design/roadmap appropriateness will be merged into the source.

### Submitting a Pull Request

If you don't know what a pull request is, read this article: https://help.github.com/articles/using-pull-requests. Make sure the repository can build and all tests pass. It is also a good idea to familiarize yourself with the project workflow and our coding conventions.

## Code of Conduct

See <xref:code-of-conduct>