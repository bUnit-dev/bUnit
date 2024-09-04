---
uid: faking-persistentcomponentstate
title: Faking persistent component state
---

# Faking persistent component state

bUnit comes with fake version of the `PersistentComponentState` type in Blazor that makes it possible to test components that use the type.

## Using the fake `PersistentComponentState`

To use the fake `PersistentComponentState` in bUnit, call the `AddFakePersistentComponentState` extension method on `TestContext`:

```csharp
var fakeState = this.AddFakePersistentComponentState();
```

Calling `AddFakePersistentComponentState` returns a `FakePersistentComponentState` type, which has three methods; one to persist data, one to get persisted data, and one that triggers any "OnPersisting" callbacks added to the `PersistentComponentState`.

To add data to the `PersistentComponentState` before running a test, i.e. to verify that a component uses the persisted state, use the `Persist` method:

```csharp
var fakeState = this.AddFakePersistentComponentState();
var key = "STATE KEY";
var data = ...; // data to persist

fakeState.Persist(key, data);

// render component
```
To trigger a callback registered with the `PersistentComponentState.RegisterOnPersisting` method, use the `TriggerOnPersisting` method on `FakePersistentComponentState`:

```csharp
var fakeState = this.AddFakePersistentComponentState();

// render component

fakeState.TriggerOnPersisting();
```

To check if data has been persisted, use the `TryTake` method:

```csharp
var fakeState = this.AddFakePersistentComponentState();
var key = "STATE KEY";

// render component, call TriggerOnPersisting

bool foundState = fakeState.TryTake<string>(key, out var data);
```

The following section has a complete example.

## Example - testing the `<FetchData>` component

In this example, lets test the following `<FetchData>` component that use `PersistentComponentState`:

```cshtml
@page "/fetchdata"
@inject PersistentComponentState State

@foreach (var f in Forecasts)
{
  <p>@f.Temperature</p>
}

@code {
  private WeatherForecast[] Forecasts { get; set; }

  protected override async Task OnInitializedAsync()
  {
    State.RegisterOnPersisting(PersistForecasts);
    
    if (!State.TryTakeFromJson<WeatherForecast[]>("weather-data", out var forecasts))
    {
        forecasts = await CreateForecastsAsync();
    }
    
    Forecasts = forecasts;
  }

  private Task PersistForecasts()
  {
    State.PersistAsJson("weather-data", Forecasts);
    return Task.CompletedTask;
  }

  // Emulates calling an API or database to get forecasts
  private Task<WeatherForecast[]> CreateForecastsAsync() =>
    new WeatherForecast[] { ... };
}
```

To test that the `<FetchData>` component uses persisted weather data instead of downloading (generating) it again with the `CreateForecastsAsync` method, use the `Persist` method on the `FakePersistentComponentState` type:

```csharp
// Arrange
var fakeState = this.AddFakePersistentComponentState();

// Persist a single weather forecast with a temperature of 42
fakeState.Persist("weather-data", new [] { new WeatherForecast { Temperature = 42 } });

// Act
var cut = RenderComponent<FetchData>();

// Assert - verify that the persisted forecast was rendered out
cut.MarkupMatches("<p>42</p>");
```    

To test that the `<FetchData>` component correctly persists weather data when its `OnPersisting` callback is triggered, do the following:

```csharp
// Arrange
var fakeState = this.AddFakePersistentComponentState();
var cut = RenderComponent<FetchData>();

// Act - trigger the FetchData components PersistForecasts method
fakeState.TriggerOnPersisting();

// Assert that state was saved and there is a non-empty forecast array returned
var didSaveState = fakeState.TryTake<WeatherForecast[]>("weather-data", out var savedState);
Assert.IsTrue(didSaveState);
Assert.NotEmpty(savedState);
```   

