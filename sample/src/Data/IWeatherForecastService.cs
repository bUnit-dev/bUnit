using System;
using System.Threading.Tasks;

namespace Egil.RazorComponents.Testing.SampleApp.Data
{
    public interface IWeatherForecastService
    {
        Task<WeatherForecast[]> GetForecastAsync(DateTime startDate);
    }
}