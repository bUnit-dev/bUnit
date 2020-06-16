using System;
using System.Linq;
using System.Threading.Tasks;

namespace Bunit.Docs.Samples
{
  public interface IWeatherForecastService
  {
    Task<WeatherForecast[]> GetForecastAsync(DateTime startDate);
  }

  public class WeatherForecast
  {
    public DateTime Date { get; set; }

    public int TemperatureC { get; set; }

    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

    public string Summary { get; set; }
  }

  public class WeatherForecastService : IWeatherForecastService
  {
    public Task<WeatherForecast[]> GetForecastAsync(DateTime startDate)
      => Task.FromResult(Array.Empty<WeatherForecast>());
  }
}