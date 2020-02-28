using System;
using System.Threading.Tasks;

namespace Bunit.SampleApp.Data
{
    public interface IWeatherForecastService
    {
        Task<WeatherForecast[]> GetForecastAsync(DateTime startDate);
    }
}