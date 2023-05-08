using Microsoft.AspNetCore.Components.Web;
using Xunit;
using Bunit;
using System;
using System.Threading.Tasks;

namespace Bunit.Docs.Samples;

public class AsyncDataTest : TestContext
{
  [Fact]
  public async Task LoadDataAsync()
  {
    // Arrange
    var textService = new TaskCompletionSource<string>();
    var cut = RenderComponent<AsyncData>(parameters => parameters
      .Add(p => p.TextService, textService.Task)
    );

    // Act - set the awaited result from the text service
    textService.SetResult("Hello World");

    // Wait for state before continuing test
    await cut.WaitForStateAsync(() => cut.Find("p").TextContent == "Hello World");

    // Assert - verify result has been set
    cut.MarkupMatches("<p>Hello World</p>");
  }

  [Fact]
  public async Task LoadDataAsyncWithTimeout()
  {
    // Arrange
    var textService = new TaskCompletionSource<string>();
    var cut = RenderComponent<AsyncData>(parameters => parameters
      .Add(p => p.TextService, textService.Task)
    );

    // Act - set the awaited result from the text service
    textService.SetResult("Long time");

    // Wait for state before continuing test
    await cut.WaitForStateAsync(() => cut.Find("p").TextContent == "Long time", TimeSpan.FromSeconds(2));

    // Assert - verify result has been set
    cut.MarkupMatches("<p>Long time</p>");
  }

  [Fact]
  public async Task LoadDataAsyncAssertion()
  {
    // Arrange
    var textService = new TaskCompletionSource<string>();
    var cut = RenderComponent<AsyncData>(parameters => parameters
      .Add(p => p.TextService, textService.Task)
    );

    // Act - set the awaited result from the text service
    textService.SetResult("Hello World");

    // Wait for assertion to pass
    await cut.WaitForAssertionAsync(() => cut.MarkupMatches("<p>Hello World</p>"));
    await cut.WaitForAssertionAsync(() => cut.MarkupMatches("<p>Hello World</p>"), TimeSpan.FromSeconds(2));
  }
}