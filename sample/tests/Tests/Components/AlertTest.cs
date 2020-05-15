using System;
using System.Threading.Tasks;
using Bunit.Mocking.JSInterop;
using SampleApp.Components;
using SampleApp.Data;
using Microsoft.AspNetCore.Authentication;
using Xunit;
using Bunit;

using static Bunit.ComponentParameterFactory;

namespace SampleApp.Tests.Components
{
    public class AlertTest2 : TestContext
    {
        MockJsRuntimeInvokeHandler MockJsRuntime { get; }

        public AlertTest2()
        {
            MockJsRuntime = Services.AddMockJsRuntime();
        }

        [Fact(DisplayName = "Given no parameters, " +
                            "Alert renders its basic markup, " +
                            "including dismiss button")]
        public void Test001()
        {
            // Arrange and Act
            var cut = RenderComponent<Alert>();

            // Assert
            cut.MarkupMatches(
                $@"<div role=""alert"" class=""alert alert-info alert-dismissible fade show"">
                      <div class=""alert-content""></div>
                      <button type=""button"" class=""close"" aria-label=""Close"">
                         <span aria-hidden=""true"">&times;</span>
                      </button>
                   </div>");
        }

        [Fact(DisplayName = "Given a component as Child Content, " +
                            "Alert renders it inside its element")]
        public void Test002()
        {
            // Arrange and act
            var content = ".NET Conf: Focus on Blazor is a free ...";
            var cut = RenderComponent<Alert>(
                ChildContent<Paragraph>(
                    (nameof(Paragraph.IsLast), true),
                    ChildContent(content)
                )
            );

            // Assert
            // Verify by looking at the text content.
            var actual = cut.Find(".alert-content > p").TextContent.Trim();
            Assert.Equal(content, actual);

            // Verify by creating the expected component
            var expected = RenderComponent<Paragraph>(
                (nameof(Paragraph.IsLast), true),
                ChildContent(content)
            );
            cut.Find(".alert-content > p").MarkupMatches(expected);

            // This verification is actually testing <Paragraph/>
            var expectedMarkup = $@"<p class=""mb-0"">{content}</p>";
            cut.Find(".alert-content > p").MarkupMatches(expectedMarkup);
        }

        [Fact(DisplayName = "Given Child Content as input, " +
                            "Alert renders it inside its element")]
        public void Test003()
        {
            // Arrange and act
            var content = ".NET Conf: Focus on Blazor is a free ...";
            var cut = RenderComponent<Alert>(
                ChildContent(content)
            );

            // Assert
            // Verify by looking at the content and handling whitespace
            var actual = cut.Find(".alert-content").TextContent.Trim();
            Assert.Equal(content, actual);

            // Verify using semantic comparison of all child nodes inside content
            cut.Find(".alert-content")
                .ChildNodes
                .MarkupMatches(content);
        }

        [Fact(DisplayName = "Given a Header as input, " +
                            "Alert renders the header text in the expected element")]
        public void Test004()
        {
            // Arrange
            var headerText = "It is time to focus on Blazor";

            // Act
            var cut = RenderComponent<Alert>(
                (nameof(Alert.Header), headerText)
            );

            // Assert
            string expected = cut.Find(".alert-heading").TextContent;
            Assert.Equal(headerText, expected);

            var expectedMarkup = $@"<h4 class=""alert-heading"">{headerText}</h4>";
            cut.Find(".alert-heading").MarkupMatches(expectedMarkup);
        }

        [Fact(DisplayName = "Given a Header and Localizer as input, " +
                            "Alert uses Localizer to localize Header text")]
        public void Test005()
        {
            // Arrange
            var headerKey = "alert-heading";
            var localizer = new Localizer() { CultureCode = "Yoda" };
            localizer.Add(headerKey, "Time to focus on Blazor it is.");

            // Act
            var cut = RenderComponent<Alert>(
                (nameof(Alert.Header), headerKey),
                CascadingValue(localizer)
            );

            // Assert
            var expected = cut.Find(".alert-heading").TextContent;
            Assert.Equal(localizer[headerKey], expected);

            var expectedMarkup = $@"<h4 class=""alert-heading"">{localizer[headerKey]}</h4>";
            cut.Find(".alert-heading").MarkupMatches(expectedMarkup);
        }

        [Fact(DisplayName = "When dismiss button is clicked, " +
                            "Alert triggers expected callbacks")]
        public void Test006()
        {
            // Arrange
            DismissingEventArgs? dismissingEvent = default;
            Alert? dismissedAlert = default;
            var cut = RenderComponent<Alert>(
                EventCallback<DismissingEventArgs>(nameof(Alert.OnDismissing), arg => dismissingEvent = arg),
                EventCallback<Alert>(nameof(Alert.OnDismissed), alert => dismissedAlert = alert)
            );

            // Act
            cut.Find("button").Click();

            // Assert
            Assert.NotNull(dismissingEvent);
            Assert.Equal(cut.Instance, dismissedAlert);
        }

        [Fact(DisplayName = "Full uncanceled Dismiss work flow executes as expected")]
        public void Test()
        {
            // Arrange
            var mockJsRuntime = Services.AddMockJsRuntime();
            var plannedInvocation = mockJsRuntime.Setup<object>("window.transitionFinished");

            DismissingEventArgs? dismissingEvent = default;
            Alert? dismissedAlert = default;

            var cut = RenderComponent<Alert>(
                EventCallback<DismissingEventArgs>(nameof(Alert.OnDismissing), arg => dismissingEvent = arg),
                EventCallback<Alert>(nameof(Alert.OnDismissed), alert => dismissedAlert = alert)
            );

            // Act
            cut.Find("button").Click();

            // Assert
            Assert.DoesNotContain("show", cut.Find(".alert").ClassList);
            Assert.NotNull(dismissingEvent);

            // Act
            plannedInvocation.SetResult(default!);

            // Assert
            cut.WaitForAssertion(() =>
            {
                cut.MarkupMatches(string.Empty);
                Assert.NotNull(dismissedAlert);
            });
        }

        [Fact(DisplayName = "When dismiss button is clicked, " +
                            "Alert trigger css animation " +
                            "and finally removes all markup")]
        public void Test007()
        {
            // Arrange            
            var plannedInvocation = MockJsRuntime.Setup<object>("window.transitionFinished");
            var cut = RenderComponent<Alert>();

            // Act - click the button
            cut.Find("button").Click();

            // Assert that css transition has started
            Assert.DoesNotContain("show", cut.Find(".alert").ClassList);

            // Act - complete 
            plannedInvocation.SetResult(default!);

            // Assert that all markup is gone            
            cut.WaitForAssertion(() => cut.MarkupMatches(string.Empty));
        }

        [Fact(DisplayName = "Alert can be dismissed via Dismiss() method")]
        public async Task Test008()
        {
            // Arrange            
            var cut = RenderComponent<Alert>();

            // Act
            await cut.Instance.Dismiss();

            // Assert that all markup is gone            
            cut.MarkupMatches(string.Empty);
            Assert.False(cut.Instance.IsVisible);
        }

        [Fact(DisplayName = "Alert renders correctly when all input is provided")]
        public void Test009()
        {
            // Arrange
            var content = "NET Conf: Focus on Blazor is a free ...";
            var headerKey = "alert-heading";
            var localizer = new Localizer() { CultureCode = "Yoda" };
            localizer.Add(headerKey, "Time to focus on Blazor it is.");

            // Act
            var cut = RenderComponent<Alert>(
                (nameof(Alert.Header), headerKey),
                CascadingValue(localizer),
                EventCallback<DismissingEventArgs>(nameof(Alert.OnDismissing), arg => { }),
                EventCallback<Alert>(nameof(Alert.OnDismissed), arg => { }),
                ChildContent(content)
            );

            // Assert
            var expected = $@"<div role=""alert"" class=""alert alert-info alert-dismissible fade show"">
                                 <h4 class=""alert-heading"">{localizer[headerKey]}</h4>
                                 <div class=""alert-content"">{content}</div>
                                 <button type=""button"" class=""close"" aria-label=""Close"">
                                    <span aria-hidden=""true"">&times;</span>
                                 </button>
                              </div>";

            cut.MarkupMatches(expected);
        }
    }
}
