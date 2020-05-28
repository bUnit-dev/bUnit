using System;
using Bunit;
using Bunit.Rendering;
using Microsoft.AspNetCore.Components;
using NUnit.Framework;

namespace Bunit.Docs.Samples
{
  public abstract class BunitTestContext : ITestContext, IDisposable
  {
    private Bunit.TestContext _context;

    public ITestRenderer Renderer => _context?.Renderer ?? throw new InvalidOperationException("NUnit has not started executing tests yet");

    public TestServiceProvider Services => _context?.Services ?? throw new InvalidOperationException("NUnit has not started executing tests yet");

    public void Dispose()
    {
      _context?.Dispose();
      _context = null;
    }

    [SetUp]
    public void Setup() => _context = new Bunit.TestContext();

    [TearDown]
    public void TearDown() => Dispose();

    public IRenderedComponent<TComponent> RenderComponent<TComponent>(params ComponentParameter[] parameters) where TComponent : IComponent
        => _context?.RenderComponent<TComponent>(parameters) ?? throw new InvalidOperationException("NUnit has not started executing tests yet");

    public IRenderedComponent<TComponent> RenderComponent<TComponent>(Action<ComponentParameterBuilder<TComponent>> parameterBuilder) where TComponent : IComponent
        => _context?.RenderComponent<TComponent>(parameterBuilder) ?? throw new InvalidOperationException("NUnit has not started executing tests yet");
  }
}