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

	protected BunitRenderer Renderer { get; private set; } = default!;

	[GlobalSetup]
	public void Setup()
	{
		RegisterServices(services);

		var serviceProvider = services.BuildServiceProvider();
		Renderer = serviceProvider.GetRequiredService<BunitRenderer>();
	}

	[GlobalCleanup]
	public void Cleanup()
	{
		InternalCleanup();
		Renderer.Dispose();
	}

	protected IRenderedComponent<TComponent> RenderComponent<TComponent>()
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