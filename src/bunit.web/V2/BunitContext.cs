using Bunit.V2.Rendering;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Bunit.V2;

[SuppressMessage("Major Code Smell", "S3881:\"IDisposable\" should be implemented correctly", Justification = "<Pending>")]
public class BunitContext : IDisposable
{
	private readonly ILoggerFactory loggerFactory;
	private BunitRenderer? bunitRenderer;

	protected ILogger Logger { get; }

	public TestServiceProvider Services { get; } = new();

	public BunitContext(ILoggerFactory? loggerFactory = null)
	{
		this.loggerFactory = loggerFactory ?? NullLoggerFactory.Instance;
		Services.AddSingleton<ILoggerFactory>(this.loggerFactory);
		Services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));
		Logger = this.loggerFactory.CreateLogger<BunitContext>();
	}

	public Task<RenderedFragment> RenderAsync<TComponent>(Action<ComponentParameterCollectionBuilder<TComponent>>? parameterBuilder = null)
		where TComponent : IComponent
	{
		var renderFragment = new ComponentParameterCollectionBuilder<TComponent>(parameterBuilder)
			.Build()
			.ToRenderFragment<TComponent>();

		bunitRenderer = new BunitRenderer(Services, loggerFactory);

		return bunitRenderer.RenderAsync(renderFragment);
	}

	public void Dispose()
		=> bunitRenderer?.Dispose();
}
