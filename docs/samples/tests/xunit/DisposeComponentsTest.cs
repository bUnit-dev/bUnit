using System;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Bunit.Docs.Samples;

public class DisposeComponentsTest : BunitContext
{
  [Fact]
  public async Task DisposeElements()
  {
    var calledTimes = 0;
    var cut = Render<DisposableComponent>(parameters => parameters
      .Add(p => p.LocationChangedCallback, url => calledTimes++)
    );

    await DisposeComponentsAsync();

    Services.GetRequiredService<NavigationManager>().NavigateTo("newurl");

    Assert.Equal(0, calledTimes);
  }

  [Fact]
  public async Task ShouldCatchExceptionInDispose()
  {
    Render<ExceptionInDisposeComponent>();

    Func<Task> act = () => DisposeComponentsAsync();

    await Assert.ThrowsAsync<NotSupportedException>(act);
  }

  [Fact]
  public async Task ShouldCatchExceptionInDisposeAsync()
  {
    Render<ExceptionInDisposeAsyncComponent>();

    await DisposeComponentsAsync();
    var exception = Renderer.UnhandledException.Result;
    Assert.IsType<NotSupportedException>(exception);
  }

  [Fact]
  public async Task ShouldDisposeJSObject()
  {
    JSInterop.SetupVoid("dispose").SetVoidResult();
    Render<AsyncDisposableComponent>();

    await DisposeComponentsAsync();

    JSInterop.VerifyInvoke("dispose");
  }
}