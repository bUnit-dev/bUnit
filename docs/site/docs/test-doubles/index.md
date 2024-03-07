---
uid: test-doubles
title: Mocking and faking component dependencies
---

# Mocking or faking component dependencies

Mocking or faking a component under test's dependencies (services) can be the difference between being able to write a stable test that is easy to understand, and the opposite. bUnit does not have any particular preferences when it comes to mocking frameworks; all the usual suspects will work with bUnit. For example, Moq, JustMock and NSubstitute all work well with bUnit, so pick the one you are the most comfortable with and use it.

bUnit does, however, come with a few specially crafted test doubles for some of Blazor’s built-in services. These are designed to make it easy to write tests of components that uses these services. More are planned for the future too.

The built-in test doubles are described on the following pages:

- <xref:bunit-auth>
- <xref:emulating-ijsruntime>
- <xref:mocking-httpclient>
- <xref:bunit-persistentcomponentstate>
- <xref:bunit-navigation-manager>
- <xref:bunit-webassemblyhostenvironment>
- <xref:input-file>