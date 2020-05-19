using System;
using System.Threading.Tasks;
using SampleApp.Data;

namespace SampleApp
{
    internal class MockForecastService : IWeatherForecastService
    {
        public TaskCompletionSource<WeatherForecast[]> Task { get; } = new TaskCompletionSource<WeatherForecast[]>();

        public Task<WeatherForecast[]> GetForecastAsync(DateTime startDate) => Task.Task;
    }
}
