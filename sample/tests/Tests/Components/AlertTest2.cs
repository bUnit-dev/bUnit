using Egil.RazorComponents.Testing.Asserting;
using Egil.RazorComponents.Testing.SampleApp.Components;
using Egil.RazorComponents.Testing.SampleApp.Data;
using Xunit;

namespace Egil.RazorComponents.Testing.SampleApp.Tests.Components
{
    public class AlertTest2 : ComponentTestFixture
    {
        MockJsRuntimeInvokeHandler MockJsRuntime { get; }

        public AlertTest2()
        {
            MockJsRuntime = Services.AddMockJsRuntime();
        }

        [Fact(DisplayName = "Given no parameters, Alert produces no markup")]
        public void Test001()
        {
            // Arrange
            var cut = RenderComponent<Alert>();
            
            // Assert
            Assert.Equal(string.Empty, cut.GetMarkup()); // option 1 using classic assert
            cut.MarkupMatches(string.Empty); // option 2 using semantic comparison
        }

        [Fact(DisplayName = "Given Child Content as input, " +
                            "Alert renders it inside its element")]
        public void Test002()
        {
            // Arrange
            var content = ".NET Conf: Focus on Blazor is a free ...";
            var cut = RenderComponent<Alert>(
                //ChildContent(content),
                ChildContent<Paragraph>(
                    (nameof(Paragraph.IsLast), true),
                    ChildContent(content)
                )
            );

            // Assert
            Assert.Equal(content, cut.Find("p").TextContent.Trim());
            cut.Find("p").MarkupMatches(content);
        }

        [Fact(DisplayName = "Given a Header as input, " +
                            "Alert renders the header text in the expected element")]
        public void Test003()
        {
            // Arrange
            var headerText = "It is time to focus on Blazor";
            var cut = RenderComponent<Alert>(
                ("Header", headerText), // Good
                (nameof(Alert.Header), headerText) // Better - refactoring friendly
            );

            // Act
            // todo

            // Assert
            // todo
        }

        [Fact(DisplayName = "Given a Header and Localizer as input, " +
                            "Alert uses Localizer to localize Header text")]
        public void Test004()
        {
            // Arrange
            var headerKey = "alert-heading";
            var localizer = new Localizer() { CultureCode = "Yoda" };
            localizer.Add(headerKey, "Time to focus on Blazor it is.");

            var cut = RenderComponent<Alert>(
                CascadingValue(localizer),
                CascadingValue("name", localizer), // if named cascading value
                (nameof(Alert.Header), headerKey) // Better - refactoring friendly
            );

            // Act
            // todo

            // Assert
            // todo
        }

        [Fact(DisplayName = "Alert calls OnDismissing callback " +
                            "when dismiss button is clicked")]
        public void Test005()
        {
            // Arrange
            DismissingEventArgs? dismissingEvent = default;
            var cut = RenderComponent<Alert>(
                ChildContent("Some alert content..."),
                EventCallback<DismissingEventArgs>(
                    nameof(Alert.OnDismissing), arg => dismissingEvent = arg
                )
            );

            // Act
            // todo

            // Assert
            // todo
        }

        [Fact(DisplayName = "Alert renders correctly when all input is provided")]
        public void Test006()
        {
            // Arrange
            var content = "NET Conf: Focus on Blazor is a free ...";
            var headerKey = "alert-heading";
            var localizer = new Localizer() { CultureCode = "Yoda" };
            localizer.Add(headerKey, "Time to focus on Blazor it is.");

            DismissingEventArgs? dismissingEvent = default;
            Alert? dismissedAlert = default;

            var cut = RenderComponent<Alert>(
                (nameof(Alert.Header), headerKey),
                CascadingValue(localizer),
                EventCallback<DismissingEventArgs>(
                    nameof(Alert.OnDismissing), arg => dismissingEvent = arg
                ),
                EventCallback<Alert>(
                    nameof(Alert.OnDismissed), alert => dismissedAlert = alert
                ),
                ChildContent<Paragraph>(
                    (nameof(Paragraph.IsLast), true),
                    ChildContent(content)
                )
            );

            // Act
            // todo

            // Assert
            // todo
        }


    }
}
