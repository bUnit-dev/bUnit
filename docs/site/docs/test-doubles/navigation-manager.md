---
uid: bunit-navigation-manager
title: Adding NavigationManager
---

# Adding `NavigationManager`

bUnit has its own version of Blazor's `NavigationManager` built-in, which is added by default to bUnit's `TestContext.Services` service provider. That means nothing special is needed to test components that depend on `NavigationManager`, as it is already available by default.

## Verify `NavigationManager` interactions

Lets look at a few examples that show how to verify that a component correctly interacts with the `NavigationManager` in various ways.

In the examples, we'll use the following `<PrintCurrentUrl>` component:

```cshtml
@implements IDisposable
@inject NavigationManager NavMan

<button @onclick="() => NavMan.NavigateTo(GoToUrl)"></button>
<p>@url</p>

@code {
  private string url;

  [Parameter] public string GoToUrl { get; set; } = string.Empty;

  protected override void OnInitialized()
    => NavMan.LocationChanged += OnLocationChanged;

  public void Dispose()
    => NavMan.LocationChanged -= OnLocationChanged;

  private void OnLocationChanged(object? sender, LocationChangedEventArgs e)
  {
    url = e.Location;
    StateHasChanged();
  }
}
```

To verify that the `<PrintCurrentUrl>` component correctly listens to location changes, do the following:

```csharp
// Arrange
var navMan = Services.GetRequiredService<BunitNavigationManager>();
var cut = RenderComponent<PrintCurrentUrl>();

// Act - trigger a navigation change
navMan.NavigateTo("newUrl");

// Assert - inspects markup to verify the location change is reflected there
cut.Find("p").MarkupMatches($"<p>{navMan.BaseUri}newUrl</p>");
```

To verify that the `<PrintCurrentUrl>` component correctly calls `NavigateTo` when it's button is clicked, do the following:

```csharp
// Arrange
var cut = RenderComponent<PrintCurrentUrl>(parameters => parameters
  .Add(p => p.GoToUrl, "http://localhost/foo"));

// Act - trigger a location change by clicking the button
cut.Find("button").Click();

// Assert - inspect the navigation manager to see if its Uri has been updated.
var navMan = Services.GetRequiredService<BunitNavigationManager>();
Assert.Equal("http://localhost/foo", navMan.Uri);
```

If a component issues multiple `NavigateTo` calls, then it is possible to inspect the navigation history by accessing the <xref:Bunit.TestDoubles.BunitNavigationManager.History> property. It's a stack based structure, meaning the latest navigations will be first in the collection at index 0.

## Asserting that navigation was prevented with the `NavigationLock` component

The `NavigationLock` component, which was introduced with .NET 7, gives the possibility to intercept the navigation and can even prevent it. bUnit will always create a history entry for prevented or even failed interceptions. This gets reflected in the <xref:Bunit.TestDoubles.NavigationHistory> property.

A component can look like this:
```razor
@inject NavigationManager NavigationManager

<button @onclick="(() => NavigationManager.NavigateTo("/counter"))">Counter</button>

<NavigationLock OnBeforeInternalNavigation="InterceptNavigation"></NavigationLock>

@code {
  private void InterceptNavigation(LocationChangingContext context)
  {
    context.PreventNavigation();
  }
}
```

A typical test, which asserts that the navigation got prevented, would look like this:

```csharp
var navMan = Services.GetRequiredService<BunitNavigationManager>();
var cut = RenderComponent<InterceptComponent>();

cut.Find("button").Click();

// Assert that the navigation was prevented
var navigationHistory = navMan.History.Single();
Assert.Equal(NavigationState.Prevented, navigationHistory.NavigationState);
```

## Simulate preventing navigation from a `<a href>` with the `NavigationLock` component

As `<a href>` navigation is not natively supported in bUnit, the `NavigationManager` can be used to simulate the exact behavior.

```razor
<a href="/counter">Counter</a>

<NavigationLock OnBeforeInternalNavigation="InterceptNavigation"></NavigationLock>

@code {
  private void InterceptNavigation(LocationChangingContext context)
  {
    throw new Exception();
  }
}
```

The test utilizes the `NavigationManager` itself to achieve the same:

```csharp
var navMan = Services.GetRequiredService<BunitNavigationManager>();
var cut = RenderComponent<InterceptAHRefComponent>();

navMan.NavigateTo("/counter");

// Assert that the navigation was prevented
var navigationHistory = navMan.History.Single();
Assert.Equal(NavigationState.Faulted, navigationHistory.NavigationState);
Assert.NotNull(navigationHistory.Exception);
```

## Getting the result of `NavigationManager.NavigateToLogin`
[`NavigationManager.NavigateToLogin`](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.components.navigationmanager.navigateto?view=aspnetcore-7.0) is a function, which was introduced with .NET 7, which allows to login dynamically. The function can also retrieve an `InteractiveRequestOptions` object, which can hold additional parameter.

```csharp
InteractiveRequestOptions requestOptions = new()
{
    Interaction = InteractionType.SignIn,
    ReturnUrl = NavigationManager.Uri,
};
requestOptions.TryAddAdditionalParameter("prompt", "login");
NavigationManager.NavigateToLogin("authentication/login", requestOptions);
```

A test could look like this:
```csharp
var navigationManager = Services.GetRequiredService<BunitNavigationManager>();

ActionToTriggerTheNavigationManager();

// This helper method retrieves the InteractiveRequestOptions object
var requestOptions = navigationManager.History.Last().StateFromJson<InteractiveRequestOptions>();
Asser.NotNull(requestOptions);
Assert.Equal(requestOptions.Interaction, InteractionType.SignIn);
options.TryGetAdditionalParameter("prompt", out string prompt);
Assert.Equal(prompt, "login");
```