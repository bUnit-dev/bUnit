using System;
using System.Threading.Tasks;
using Bunit.SampleApp.Data;

namespace Bunit.SampleApp
{
    internal class MockForecastService : IWeatherForecastService
    {
        public TaskCompletionSource<WeatherForecast[]> Task { get; } = new TaskCompletionSource<WeatherForecast[]>();

        public Task<WeatherForecast[]> GetForecastAsync(DateTime startDate) => Task.Task;
    }
}
