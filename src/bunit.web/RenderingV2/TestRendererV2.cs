using Microsoft.Extensions.Logging;

namespace Bunit.RenderingV2;

public partial class TestRendererV2 : Renderer
{
	private readonly ILogger logger;
	private readonly Dictionary<int, IRenderedComponent<IComponent>> renderedComponents = new();
	private TaskCompletionSource<Exception> unhandledExceptionTsc = new();
	private Exception? capturedUnhandledException;

	public override Dispatcher Dispatcher { get; } = Dispatcher.CreateDefault();

	public Task<Exception> UnhandledException => unhandledExceptionTsc.Task;

	public TestRendererV2(IServiceProvider serviceProvider, ILoggerFactory loggerFactory)
		: base(serviceProvider, loggerFactory)
	{
		logger = loggerFactory.CreateLogger<TestRendererV2>();
	}

	public IRenderedComponent<RootComponent> Render<TComponent>(Action<ComponentParameterCollectionBuilder<TComponent>> renderFragmentBuilder)
		where TComponent : IComponent
	{
		var builder = new ComponentParameterCollectionBuilder<TComponent>();
		renderFragmentBuilder(builder);
		return Render(builder.Build().ToRenderFragment<TComponent>());
	}

	public IRenderedComponent<RootComponent> Render(RenderFragment renderFragment)
	{
		try
		{
			return Dispatcher.InvokeAsync(() =>
			{
				var component = new RootComponent(renderFragment);
				var componentId = AssignRootComponentId(component);
				var rc = new RenderedComponentV2<RootComponent>(component);
				renderedComponents[componentId] = rc;

				// Do not await async life cycle methods of components in render fragment
				_ = RenderRootComponentAsync(componentId);

				return rc;
			}).Result;
		}
		catch (Exception ex)
		{
			HandleException(ex);
			throw;
		}
	}

	/// <summary>
	/// Creates a <see cref="RenderFragment"/> with the <paramref name="renderFragmentBuilder"/> and
	/// renders it inside a <see cref="RootComponent"/>.
	/// The returned task completes when all components in the render Fragment
	/// have finished rendering completely. That includes waiting for any async operations
	/// in life-cycle methods of the components in the render tree.
	/// </summary>
	public Task<IRenderedComponent<RootComponent>> RenderAsync<TComponent>(Action<ComponentParameterCollectionBuilder<TComponent>> renderFragmentBuilder)
		where TComponent : IComponent
	{
		var builder = new ComponentParameterCollectionBuilder<TComponent>();
		renderFragmentBuilder(builder);
		return RenderAsync(builder.Build().ToRenderFragment<TComponent>());
	}

	/// <summary>
	/// Renders the <paramref name="renderFragment"/> inside a <see cref="RootComponent"/>.
	/// The returned task completes when all components in the <paramref name="renderFragment"/>
	/// have finished rendering completely. That includes waiting for any async operations
	/// in life-cycle methods of the components in the render tree.
	/// </summary>
	public async Task<IRenderedComponent<RootComponent>> RenderAsync(RenderFragment renderFragment)
	{
		try
		{
			return await Dispatcher.InvokeAsync(async () =>
			{
				var component = new RootComponent(renderFragment);
				var componentId = AssignRootComponentId(component);
				var rc = new RenderedComponentV2<RootComponent>(component);
				renderedComponents[componentId] = rc;
				await RenderRootComponentAsync(componentId).ConfigureAwait(false);
				return rc;
			}).ConfigureAwait(false);
		}
		catch (Exception ex)
		{
			HandleException(ex);
			throw;
		}
	}

	protected override Task UpdateDisplayAsync(in RenderBatch renderBatch)
		=> Task.CompletedTask;

	/// <inheritdoc/>
	protected override void HandleException(Exception exception)
	{
		if (exception is null)
			return;

		LogUnhandledException(logger, exception);

		capturedUnhandledException = exception;

		if (!unhandledExceptionTsc.TrySetResult(capturedUnhandledException))
		{
			unhandledExceptionTsc = new TaskCompletionSource<Exception>();
			unhandledExceptionTsc.SetResult(capturedUnhandledException);
		}
	}
}
