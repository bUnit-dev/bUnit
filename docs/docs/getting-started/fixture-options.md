---
uid: fixture-options
title: Fixture options in Razor Tests
---

# `<Fixture>` options in Razor Tests

bUnit's <xref:Bunit.Fixture> component provides different parameters you can set on it, that changes the behavior of the test. It also allows you to both set up a component under test, and additional fragments, that can be used in the test.


## Parameters

All the parameters the `<Fixture>` support is shown in the listing below:

[!code-html[](../../samples/tests/razor/AllFixtureParameters.razor)]

**Setup and Test methods:**

Let us start by looking at the parameters that takes a method as input first. The methods are called in the order they are listed in below, if provided, and should be used to the following:

1. **<xref:Bunit.RazorTesting.FixtureBase`1.Setup>** and **<xref:Bunit.RazorTesting.FixtureBase`1.SetupAsync>:**  
   The `Setup` and `SetupAsync` method is called first, and you can provide both if needed. If both are provided, `Setup` is called first.   
  They allows you to configures e.g. the <xref:Bunit.ITestContext.Services> collection of the <xref:Bunit.Fixture> component before the component under test or any fragments are rendered.
2. **<xref:Bunit.RazorTesting.FixtureBase`1.Test>** or **<xref:Bunit.RazorTesting.FixtureBase`1.TestAsync>:**  
  The `Test` or `TestAsync` methods are called after the setup methods.   
  _One, and only one,_ of the test methods must be specified. Use the test method to access the component under test and any fragments defined in the fixture and interact and assert against them.
  
**Other parameters**

The other parameters affect how the test runs, and how it is displayed in e.g. Visual Studio's Test Explorer:

1. **<xref:Bunit.RazorTesting.RazorTestBase.Description>:**   
   If a description is provided, it will be displayed by the test runner when the test runs, and in Visual Studio's Test Explorer. If no description is provided, the name of the provided test method is used.
2. **<xref:Bunit.RazorTesting.RazorTestBase.Skip>:**  
   If the skip parameter is provided, the test is skipped and the text entered in the skip parameter is passed to the test runner as the reason to skip the test.
3. **<xref:Bunit.RazorTesting.RazorTestBase.Timeout>:**  
   If provided, the test runner will terminate the test after the specified amount of time.

## `<ComponentUnderTest>` and `<Fragment>`

## Getting `<ComponentUnderTest>` and `<Fragment>`

## Example