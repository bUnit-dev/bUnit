using System;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Bunit.Docs.Samples;

public class DisposeComponentsTest
{
  [Fact]
  public void DisposeElements()
  {
    using var ctx = new TestContext();
    var calledTimes = 0;
    var cut = ctx.RenderComponent<DisposableComponent>(parameters => parameters
      .Add(p => p.LocationChangedCallback, url => calledTimes++)
    );
    
    ctx.DisposeComponents();

    ctx.Services.GetRequiredService<NavigationManager>().NavigateTo("newurl");

    Assert.Equal(0, calledTimes);
  }

  [Fact]
  public void ShouldCatchExceptionInDispose()
  {
    using var ctx = new TestContext();
    ctx.RenderComponent<ExceptionInDisposeComponent>();

    var act = ctx.DisposeComponents;

    Assert.Throws<NotSupportedException>(act);
  }

#if NET5_0_OR_GREATER
  [Fact]
  public void ShouldCatchExceptionInDisposeAsync()
  {
    using var ctx = new TestContext();
    ctx.RenderComponent<ExceptionInDisposeAsyncComponent>();

    ctx.DisposeComponents();
    var exception = ctx.Renderer.UnhandledException.Result;
    Assert.IsType<NotSupportedException>(exception);
  }
#endif

  [Fact]
  public async Task DisposeElementsAsync()
  {
    using var ctx = new TestContext();
    var calledTimes = 0;
    var cut = ctx.RenderComponent<DisposableComponent>(parameters => parameters
      .Add(p => p.LocationChangedCallback, url => calledTimes++)
    );
    
    await ctx.DisposeComponentsAsync();

    ctx.Services.GetRequiredService<NavigationManager>().NavigateTo("newurl");

    Assert.Equal(0, calledTimes);
  }
}