---
uid: snapshottest-details
title: SnapshotTest Details
---

# `<SnapshotTest>` Details

bUnit's support for snapshot testing comes with the <xref:Bunit.SnapshotTest> component. In snapshot testing, you declare your input (e.g. one or more component under test) and the expected output, and the library will automatically tell you if they do not match.

> [!NOTE] 
> One notable snapshot testing feature is missing now; the ability to auto-generate the expected output initially, when it is not specified. If you want to contribute to this, take a look at [issue #3 on GitHub](https://github.com/egil/bunit/issues/3).

> [!WARNING]
> Razor tests, where <xref:Bunit.SnapshotTest> components are used, are currently only compatible with using xUnit as the general purpose testing framework.

## Parameters

All the parameters the <xref:Bunit.SnapshotTest> component support is shown in the listing below:

[!code-cshtml[](../../../samples/tests/razor/AllSnapshotTestParameters.razor)]

Let us go over each of these:

1. **<xref:Bunit.RazorTesting.FixtureBase`1.Setup>** and **<xref:Bunit.RazorTesting.FixtureBase`1.SetupAsync>:**  
   The `Setup` and `SetupAsync` methods can be used to register any services that should be injected into the components declared inside the `<TestInput>` and `<ExpectedOutput>`, and you can use both `Setup` and `SetupAsync`, if needed. If both are provided, `Setup` is called first.   
2. **<xref:Bunit.RazorTesting.RazorTestBase.Description>:**   
   If a description is provided, it will be displayed by the test runner when the test runs, and in Visual Studio's Test Explorer. If no description is provided, "SnapshotTest #NUM" is used, where NUM is the position the test has in the file it is declared in.
3. **<xref:Bunit.RazorTesting.RazorTestBase.Skip>:**  
   If the skip parameter is provided, the test is skipped, and the text entered in the skip parameter is passed to the test runner as the reason to skip the test.
4. **<xref:Bunit.RazorTesting.RazorTestBase.Timeout>:**  
   If provided, the test runner will terminate the test after the specified amount of time, if it has not completed already.
5. **<xref:Bunit.SnapshotTest.TestInput> child component:**  
   Inside the `<TestInput>` child component is where you put all Razor and HTML markup, that constitute the test input or component under test.
6. **<xref:Bunit.SnapshotTest.ExpectedOutput> child component:**  
   Inside the `<ExpectedOutput>` child component is where you put all Razor and HTML markup that represents what the rendered result of `<TestInput>` of should be. 

## What Happens When The Test Runs?

When a <xref:Bunit.SnapshotTest> runs, this happens:

1. It will first call the setup methods.
2. Then it will render the `<TestInput>` and `<ExpectedOutput>` child components.
3. Finally, it will compare the rendered markup from the `<TestInput>` and `<ExpectedOutput>` child components, using the semantic HTML comparer built into bUnit.

The semantic comparison in bUnit allows you to customize the it through _"comparison modifiers"_ in the `<ExpectedOutput>` markup. For example, if you want to tell the semantic comparer to ignore the case of the text content inside an element, you can add the `diff:ignoreCase` attribute to the element inside `<ExpectedOutput>`. 

To learn more about semantic comparison modifiers, go to the <xref:semantic-html-comparison> page.