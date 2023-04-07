using System;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Bunit.Docs.Samples;

public class DisposeComponentsTest : TestContext
{
  [Fact]
  public void DisposeElements()
  {
    var calledTimes = 0;
    var cut = RenderComponent<DisposableComponent>(parameters => parameters
      .Add(p => p.LocationChangedCallback, url => calledTimes++)
    );

    DisposeComponents();

    Services.GetRequiredService<NavigationManager>().NavigateTo("newurl");

    Assert.Equal(0, calledTimes);
  }

  [Fact]
  public void ShouldCatchExceptionInDispose()
  {
    RenderComponent<ExceptionInDisposeComponent>();

    var act = DisposeComponents;

    Assert.Throws<NotSupportedException>(act);
  }

#if NET5_0_OR_GREATER
  [Fact]
  public void ShouldCatchExceptionInDisposeAsync()
  {
    RenderComponent<ExceptionInDisposeAsyncComponent>();

    DisposeComponents();
    var exception = Renderer.UnhandledException.Result;
    Assert.IsType<NotSupportedException>(exception);
  }
#endif
}