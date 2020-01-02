using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Egil.RazorComponents.Testing.Asserting;
using Egil.RazorComponents.Testing.SampleApp.Data;
using Egil.RazorComponents.Testing.SampleApp.Components;
using Xunit;
using Egil.RazorComponents.Testing.SampleApp.Pages;
using Shouldly;

namespace Egil.RazorComponents.Testing.SampleApp.CodeOnlyTests
{
    public class FetchDataTest : ComponentTestFixture
    {
        [Fact(DisplayName = "Fetch data component renders expected initial markup")]
        public void Test001()
        {
            // Arrange - add the mock forecast service
            Services.AddService<IWeatherForecastService, MockForecastService>();

            // Act - render the FetchData component
            var cut = RenderComponent<FetchData>();

            // Assert that it renders the initial loading message
            var initialExpectedHtml = @"<h1>Weather forecast</h1>
                                    <p>This component demonstrates fetching data from a service.</p>
                                    <p><em>Loading...</em></p>";
            cut.MarkupMatches(initialExpectedHtml);
        }

        [Fact(DisplayName = "After data loads it is displayed in a ForecastTable component")]
        public void Test002()
        {
            // Setup the mock forecast service
            var forecasts = new[] { new WeatherForecast { Date = DateTime.Now, Summary = "Testy", TemperatureC = 42 } };
            var mockForecastService = new MockForecastService();
            Services.AddService<IWeatherForecastService>(mockForecastService);

            // Arrange - render the FetchData component
            var cut = RenderComponent<FetchData>();

            // Act - pass the test forecasts to the component via the mock services
            WaitForNextRender(() => mockForecastService.Task.SetResult(forecasts));

            // Assert
            // Render an new instance of the ForecastDataTable, passing in the test data
            var expectedDataTable = RenderComponent<ForecastDataTable>((nameof(ForecastDataTable.Forecasts), forecasts));
            // Assert that the CUT has two changes, one removal of the loading message and one addition which matched the 
            // rendered HTML from the expectedDataTable.
            cut.GetChangesSinceFirstRender().ShouldHaveChanges(
                diff => diff.ShouldBeRemoval("<p><em>Loading...</em></p>"),
                diff => diff.ShouldBeAddition(expectedDataTable)
            );
        }
    }
}