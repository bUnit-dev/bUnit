using BenchmarkDotNet.Attributes;
using Bunit.Rendering;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;

namespace Bunit;

public abstract class BenchmarkBase
{
    private static readonly ComponentParameterCollection EmptyParameter = new();
    private readonly ServiceCollection services = new();
    
    protected TestRenderer Renderer { get; private set; } = default!;

    [GlobalSetup]
    public void Setup()
    {
        RegisterServices(services);
        
        var serviceProvider = services.BuildServiceProvider();
        Renderer = new TestRenderer(
            new RenderedComponentActivator(serviceProvider),
            new TestServiceProvider(services),
            new NullLoggerFactory());
    }

    [GlobalCleanup]
    public void Cleanup()
    {
        InternalCleanup();
        Renderer.Dispose();
    }

    protected IRenderedComponentBase<TComponent> RenderComponent<TComponent>()
        where TComponent : IComponent =>
        Renderer.RenderComponent<TComponent>(EmptyParameter);

    protected virtual void InternalCleanup()
    {
    }

    protected virtual void RegisterServices(IServiceCollection serviceCollection)
    {
        services.AddSingleton<BunitHtmlParser>();
    }
}