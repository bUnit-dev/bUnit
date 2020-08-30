---
uid: test-doubles
title: Mocking and Faking Component Dependencies
---

# Mocking Component Dependencies

Mocking a component under tests dependencies (services) can be the differece between being able to write an easy to understand and stable test and the opposite. bUnit does not have any particular preferences when it comes to mocking frameworks, all the usual suspects will work with bUnit, e.g. Moq, JustMock and NSubstitute all work well with bUnit, so pick the one you are the most comfortable with and use it.

bUnit does however come with a few specially crafted mock helpers for some of Blazors built-in services, which are specifically designed to make it easy and clean to mock these, and more are planned in the future.

The built-in mock helpers are described on the following pages:

- <xref:faking-auth>
- <xref:mocking-ijsruntime>
