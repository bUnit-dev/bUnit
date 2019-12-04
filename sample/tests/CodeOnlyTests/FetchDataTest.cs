using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Egil.RazorComponents.Testing.Library.SampleApp.Data;
using Egil.RazorComponents.Testing.Library.SampleApp.Components;
using Xunit;
using Egil.RazorComponents.Testing.Library.SampleApp.Pages;

namespace Egil.RazorComponents.Testing.Library.SampleApp.CodeOnlyTests
{
    public class FetchDataTest : ComponentFixtureBase
    {
        class MockForecastService : IWeatherForecastService
        {
            public TaskCompletionSource<WeatherForecast[]> Task { get; } = new TaskCompletionSource<WeatherForecast[]>();

            public Task<WeatherForecast[]> GetForecastAsync(DateTime startDate) => Task.Task;
        }

        MockForecastService mockForecastService;

        public FetchDataTest()
        {
            mockForecastService = new MockForecastService();
            TestHost.AddService<IWeatherForecastService>(mockForecastService);
        }

        [Fact]
        public void InitialLoadingHtmlRendersCorrectly()
        {
            var cut = TestHost.AddComponent<FetchData>();
            var initialExpectedHtml = @"<h1>Weather forecast</h1>
                                        <p>This component demonstrates fetching data from a service.</p>
                                        <p><em>Loading...</em></p>";

            cut.ShouldBe(initialExpectedHtml);
        }

        [Fact]
        public void AfterDataLoadsItIsDisplayedInAForecastTable()
        {
            // arrange
            var forecasts = new[]
            {
                new WeatherForecast{ Date = DateTime.Now, Summary = "Testy", TemperatureC = 42},
            };
            var cut = TestHost.AddComponent<FetchData>();
            var initialHtml = cut.GetMarkup();
            var expectedDataTable = TestHost.AddComponent<ForecastDataTable>((nameof(ForecastDataTable.Forecasts), forecasts));

            // act
            TestHost.WaitForNextRender(() => mockForecastService.Task.SetResult(forecasts));

            // assert
            cut.CompareTo(initialHtml)
                .ShouldHaveChanges(
                    diff => diff.ShouldBeRemoval("<p><em>Loading...</em></p>"),
                    diff => diff.ShouldBeAddition(expectedDataTable)
                );
        }
    }
}
