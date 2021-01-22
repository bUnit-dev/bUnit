---
uid: test-doubles
title: Mocking and Faking Component Dependencies
---

# Mocking or Faking Component Dependencies

Mocking or faking a component under test's dependencies (services) can be the difference between being able to write a stable test that is easy to understand, and the opposite. bUnit does not have any particular preferences when it comes to mocking frameworks; all the usual suspects will work with bUnit. For example, Moq, JustMock and NSubstitute all work well with bUnit, so pick the one you are the most comfortable with and use it.

bUnit does, however, come with a few specially crafted test doubles for some of Blazor’s built-in services. These are designed to make it easy to write tests of components that uses these services. More are planned for the future too.

The built-in test doubles are described on the following pages:

- <xref:faking-auth>
- <xref:emulating-ijsruntime>
<!--stackedit_data:
eyJoaXN0b3J5IjpbMTA3NDM0Mjc2XX0=
-->