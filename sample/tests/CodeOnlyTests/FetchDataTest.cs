using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Egil.RazorComponents.Testing.Library.SampleApp.Data;
using Egil.RazorComponents.Testing.Library.SampleApp.Components;
using Xunit;
using Egil.RazorComponents.Testing.Library.SampleApp.Pages;
using Shouldly;

namespace Egil.RazorComponents.Testing.Library.SampleApp.CodeOnlyTests
{
    public class FetchDataTest : ComponentFixtureBase
    {
        class MockForecastService : IWeatherForecastService
        {
            public TaskCompletionSource<WeatherForecast[]> Task { get; } = new TaskCompletionSource<WeatherForecast[]>();

            public Task<WeatherForecast[]> GetForecastAsync(DateTime startDate) => Task.Task;
        }

        [Fact]
        public void InitialLoadingHtmlRendersCorrectly()
        {
            TestHost.AddService<IWeatherForecastService, MockForecastService>();
            var cut = TestHost.AddComponent<FetchData>();
            var initialExpectedHtml = @"<h1>Weather forecast</h1>
                                        <p>This component demonstrates fetching data from a service.</p>
                                        <p><em>Loading...</em></p>";

            cut.ShouldBe(initialExpectedHtml);
        }

        [Fact]
        public void AfterDataLoadsItIsDisplayedInAForecastTable()
        {
            // setup mock
            var mockForecastService = new MockForecastService();
            TestHost.AddService<IWeatherForecastService>(mockForecastService);

            // arrange
            var forecasts = new[] { new WeatherForecast { Date = DateTime.Now, Summary = "Testy", TemperatureC = 42 } };
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

        [Fact]
        public void AfterDataLoadsItIsDisplayedInAForecastTableChangeTracking()
        {
            // setup mock
            var mockForecastService = new MockForecastService();
            TestHost.AddService<IWeatherForecastService>(mockForecastService);

            // arrange
            var forecasts = new[] { new WeatherForecast { Date = DateTime.Now, Summary = "Testy", TemperatureC = 42 } };
            var cut = TestHost.AddComponent<FetchData>();
            var expectedDataTable = TestHost.AddComponent<ForecastDataTable>((nameof(ForecastDataTable.Forecasts), forecasts));

            // act
            TestHost.WaitForNextRender(() => mockForecastService.Task.SetResult(forecasts));

            // assert
            cut.GetChangesSinceFirstRender().ShouldHaveChanges(
                diff => diff.ShouldBeRemoval("<p><em>Loading...</em></p>"),
                diff => diff.ShouldBeAddition(expectedDataTable)
            );
        }
    }
}
