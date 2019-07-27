# Razor Component Testing Library
Prototype testing library that renders Razor Components as HTML and compare the result to an 
expected result, using the [XMLDiff](https://www.xmlunit.org/) library and [Shouldly](https://github.com/shouldly/shouldly) for writing out error messages.

**NOTE: Only tested with Preview-6**

## Examples
The following examples uses XUnit and Shouldly for testing. 

To test the following component:

```razor
@code {
    [Parameter] public RenderFragment? ChildContent { get; set; }
}
@if (!(ChildContent is null))
{
    <span class="sr-only">@ChildContent</span>
}
```

Write the following test fixture:

```csharp
using Egil.RazorComponents.Testing;
using Xunit;

public class SrOnlyTest : RazorComponentFixture
{
    [Fact(DisplayName = "SrOnly does not render anything when ChildContent is null")]
    public void MyTestMethod()
    {
        var expectedHtml = string.Empty;

        var result = Component<SrOnly>().Render();

        result.ShouldBe(expectedHtml);
    }

    [Fact(DisplayName = "SrOnly renders if ChildContent is not null")]
    public void SrOnlyRenderCorrectlysIfChildContentIsNotNull()
    {
        var content = "CONTENT";
        var expectedHtml = $@"<span class=""sr-only"">{content}</span>";            

        var result = Component<SrOnly>().WithChildContent(content).Render();

        result.ShouldBe(expectedHtml);
    }

    [Fact(DisplayName = "Fail example: SrOnly renders if ChildContent is not null")]
    public void MyTestMethod3()
    {
        var content = "CONTENT";
        //var expectedHtml = $@"<span class=""sr-only"">{content}</span>";            
        var expectedHtml = "<div></div>";

        var result = Component<SrOnly>().WithChildContent(content).Render();

        result.ShouldBe(expectedHtml);
    }
}
```

The thrid test will fail with the following helpful error message:

```text
 Fail example: SrOnly renders if ChildContent is not null
   Source: SrOnlyTest.cs line: 30
   Duration: 122 ms

  Message: 
    Shouldly.ShouldAssertException : diffResult.HasDifferences()
        should be
    False
        but was
    True
    
    Additional Info:
        should be
    
    <div></div>
    
    	but was
    
    <span class="sr-only">CONTENT</span>
    
    	with the following differences:
    
    - Expected child nodelist length '0' but was '1' - comparing <div...> at /ROOT[1]/div[1] to <span...> at /ROOT[1]/span[1] (DIFFERENT)
    - Expected element tag name 'div' but was 'span' - comparing <div...> at /ROOT[1]/div[1] to <span...> at /ROOT[1]/span[1] (DIFFERENT)
    - Expected number of attributes '0' but was '1' - comparing <div...> at /ROOT[1]/div[1] to <span...> at /ROOT[1]/span[1] (DIFFERENT)
    - Expected attribute name '/ROOT[1]/div[1]' - comparing <div...> at /ROOT[1]/div[1] to <span...> at /ROOT[1]/span[1]/@class (DIFFERENT)
    - Expected child '' but was '#text' - comparing <NULL> to <span ...>CONTENT</span> at /ROOT[1]/span[1]/text()[1] (DIFFERENT)
    
  Stack Trace: 
    at ShouldlyRazorComponentTestExtensions.ShouldBe(ComponentRenderedText componentRenderedText, String expectedHtml) in ShouldlyRazorComponentTestExtensions.cs line: 23
    at SrOnlyTest.MyTestMethod3() in SrOnlyTest.cs line: 38
```

### Testing an Bootstrap Alert component
The following testing code tests an Bootstrap Alert component (not included at the moment).
It will hopefully serve as an example of the other things the library can do at the momemt.

```csharp
public class AlertTest : RazorComponentFixture
{
    [Fact(DisplayName = "Alert render with no parameters")]
    public void MyTestMethod()
    {
        var expectedHtml = $@"<div class=""alert fade show"" role=""alert""></div>";

        var result = Component<Alert>().Render();

        result.ShouldBe(expectedHtml);
    }

    [Fact(DisplayName = "Alert adds color when specified")]
    public void MyTestMethod2()
    {
        var expectedHtml = $@"<div class=""alert fade show alert-primary"" role=""alert""></div>";

        var component = Component<Alert>().WithParams(("Color", "primary"));

        var result = component.Render();

        result.ShouldBe(expectedHtml);
    }

    [Fact(DisplayName = "Providing a role overrides default role value")]
    public void MyTestMethod3()
    {
        var role = "ALERT";
        var expectedHtml = $@"<div class=""alert fade show"" role=""{role}""></div>";

        var result = Component<Alert>().WithParams(("Role", role)).Render();

        result.ShouldBe(expectedHtml);
    }

    [Fact(DisplayName = "Setting Dismisasable to true renderes dismiss button")]
    public void MyTestMethod4()
    {
        var expectedHtml = $@"<div class=""alert fade show alert-dismissible"" role=""alert"">
                                <button type=""button"" class=""close"" aria-label=""Close"">
                                    <span aria-hidden=""true"">&amp;times;</span>
                                </button>
                                </div>";

        var result = Component<Alert>().WithParams(("Dismissable", true)).Render();

        result.ShouldBe(expectedHtml);
    }

    [Fact(DisplayName = "Setting DismissAriaLabel and DismissText overrides defaults")]
    public void MyTestMethod42()
    {
        var dismissAriaLabel = "DISMISSARIALABEL";
        var dismissText = "DISMISSTEXT";
        var expectedHtml = $@"<div class=""alert fade show alert-dismissible"" role=""alert"">
                                <button type=""button"" class=""close"" aria-label=""{dismissAriaLabel}"">
                                    <span aria-hidden=""true"">{dismissText}</span>
                                </button>
                                </div>";

        var result = Component<Alert>().WithParams(
            ("Dismissable", true),
            ("DismissAriaLabel", dismissAriaLabel),
            ("DismissText", dismissText)
        ).Render();

        result.ShouldBe(expectedHtml);
    }

    [Fact(DisplayName = "Nested A components have their DefaultCssClass set to alert-link")]
    public void MyTestMethod5()
    {
        var expectedHtml = $@"<div class=""alert fade show"" role=""alert"">
                                <a class=""alert-link""></a>
                                </div>";

        var result = Component<Alert>().WithChildContent(Fragment<A>()).Render();

        result.ShouldBe(expectedHtml);
    }

    [Fact(DisplayName = "Nested Heading components have their DefaultCssClass set to alert-heading")]
    public void MyTestMethod6()
    {
        var expectedHtml = $@"<div class=""alert fade show"" role=""alert"">
                                <h1 class=""alert-heading""></h1>
                                </div>";

        var result = Component<Alert>().WithChildContent(Fragment<H1>()).Render();

        result.ShouldBe(expectedHtml);
    }
}
```

Ill add more examples later...
