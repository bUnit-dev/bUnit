using System;
using System.Threading.Tasks;

namespace SampleApp.Data
{
    public interface IWeatherForecastService
    {
        Task<WeatherForecast[]> GetForecastAsync(DateTime startDate);
    }
}