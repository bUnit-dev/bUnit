---
uid: test-doubles
title: Mocking and Faking Component Dependencies
---

# Mocking Component Dependencies

Mocking a component under tests dependencies (services) can be the difference between being able to write a stable test that is easy to understand, and the opposite. bUnit does not have any particular preferences when it comes to mocking frameworks; all the usual suspects will work with bUnit. For example, Moq, JustMock and NSubstitute all work well with bUnit, so pick the one you are the most comfortable with and use it.

bUnit does, however, come with a few specially crafted mock helpers for some of Blazor’s built-in services. These are designed to make it easy and clean to mock them. More are planned for the future too.

The built-in mock helpers are described on the following pages:

- <xref:faking-auth>
- <xref:mocking-ijsruntime>
