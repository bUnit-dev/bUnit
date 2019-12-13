using System;
using System.Threading.Tasks;
using Egil.RazorComponents.Testing.Library.SampleApp.Data;

namespace Egil.RazorComponents.Testing.Library.SampleApp
{
    internal class MockForecastService : IWeatherForecastService
    {
        public TaskCompletionSource<WeatherForecast[]> Task { get; } = new TaskCompletionSource<WeatherForecast[]>();

        public Task<WeatherForecast[]> GetForecastAsync(DateTime startDate) => Task.Task;
    }
}
