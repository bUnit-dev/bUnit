using Microsoft.AspNetCore.Components.Web;
using Xunit;
using Bunit;
using System;
using System.Threading.Tasks;

namespace Bunit.Docs.Samples
{
  public class AsyncDataTest
  {
    [Fact]
    public void LoadDataAsync()
    {
      // Arrange
      using var ctx = new TestContext();
      var textService = new TaskCompletionSource<string>();
      var cut = ctx.RenderComponent<AsyncData>(parameters => parameters
        .Add(p => p.TextService, textService.Task)
      );

      // Act - set the awaited result from the text service
      textService.SetResult("Hello World");

      // Wait for state before continuing test
      cut.WaitForState(() => cut.Find("p").TextContent == "Hello World");

      // Assert - verify result has been set
      cut.MarkupMatches("<p>Hello World</p>");
    }

    [Fact]
    public void LoadDataAsyncWithTimeout()
    {
      // Arrange
      using var ctx = new TestContext();
      var textService = new TaskCompletionSource<string>();
      var cut = ctx.RenderComponent<AsyncData>(parameters => parameters
        .Add(p => p.TextService, textService.Task)
      );

      // Act - set the awaited result from the text service
      textService.SetResult("Long time");

      // Wait for state before continuing test
      cut.WaitForState(() => cut.Find("p").TextContent == "Long time", TimeSpan.FromSeconds(2));

      // Assert - verify result has been set
      cut.MarkupMatches("<p>Long time</p>");
    }

    [Fact]
    public void LoadDataAsyncAssertion()
    {
      // Arrange
      using var ctx = new TestContext();
      var textService = new TaskCompletionSource<string>();
      var cut = ctx.RenderComponent<AsyncData>(parameters => parameters
        .Add(p => p.TextService, textService.Task)
      );

      // Act - set the awaited result from the text service
      textService.SetResult("Hello World");

      // Wait for assertion to pass
      cut.WaitForAssertion(() => cut.MarkupMatches("<p>Hello World</p>"));
      cut.WaitForAssertion(() => cut.MarkupMatches("<p>Hello World</p>"), TimeSpan.FromSeconds(2));
    }
  }
}
