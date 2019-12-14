using System;
using System.Threading.Tasks;
using Egil.RazorComponents.Testing.SampleApp.Data;

namespace Egil.RazorComponents.Testing.SampleApp
{
internal class MockForecastService : IWeatherForecastService
{
    public TaskCompletionSource<WeatherForecast[]> Task { get; } = new TaskCompletionSource<WeatherForecast[]>();

    public Task<WeatherForecast[]> GetForecastAsync(DateTime startDate) => Task.Task;
}
}
