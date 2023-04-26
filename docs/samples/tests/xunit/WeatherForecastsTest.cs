using Xunit;
using Bunit;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using static Bunit.ComponentParameterFactory;

namespace Bunit.Docs.Samples;

public class WeatherForecastsTest : TestContext
{
  [Fact]
  public void ServicesIsInjectedCorrectly()
  {
    // Register services
    Services.AddSingleton<IWeatherForecastService>(new WeatherForecastService());

    // RenderComponent will inject the service in the WeatherForecasts component
    // when it is instantiated and rendered.
    var cut = RenderComponent<WeatherForecasts>();

    // Assert that service is injected
    Assert.NotNull(cut.Instance.Forecasts);
  }
}