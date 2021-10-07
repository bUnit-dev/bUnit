---
uid: fake-navigation-manager
title: Faking NavigationManager
---

# Faking `NavigationManager`

bUnit has a fake version of Blazor's `NavigationManager` built-in, which is added by default to bUnit's `TestContext.Services` service provider. That means nothing special is needed to test components that depend on `NavigationManager`, as it is already available by default.

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
using var ctx = new TestContext();
var navMan = ctx.Services.GetRequiredService<FakeNavigationManager>();
var cut = ctx.RenderComponent<PrintCurrentUrl>();

// Act - trigger a navigation change
navMan.NavigateTo("newUrl");

// Assert - inspects markup to verify the location change is reflected there
cut.Find("p").MarkupMatches($"<p>{navMan.BaseUri}newUrl</p>");
```

To verify that the `<PrintCurrentUrl>` component correctly calls `NavigateTo` when it's button is clicked, do the following:

```csharp
// Arrange
using var ctx = new TestContext();
var cut = ctx.RenderComponent<PrintCurrentUrl>(parameters => parameters
  .Add(p => p.GoToUrl, "http://localhost/foo"));

// Act - trigger a location change by clicking the button
cut.Find("button").Click();

// Assert - inspect the navigation manager to see if its Uri has been updated.
var navMan = ctx.Services.GetRequiredService<FakeNavigationManager>();
Assert.Equal("http://localhost/foo", navMan.Uri);
```

If a component issues multiple `NavigateTo` calls, then it is possible to inspect the navigation history by accessing the <xref:Bunit.TestDoubles.FakeNavigationManager.History> property. It's a stack based structure, meaning the latest navigations will be first in the collection at index 0.
