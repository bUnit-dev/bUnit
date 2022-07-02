using System.Diagnostics;
using System.Runtime.CompilerServices;
using AngleSharp.Dom;
using AngleSharp.Html.Parser;
using Bunit.RenderingV2.ComponentTree;
using Microsoft.Extensions.Logging;

namespace Bunit.RenderingV2;

public partial class BunitRenderer : Renderer
{	
	private readonly ILogger logger;
	private readonly ComponentTreeManager componentTreeManager;
	private TaskCompletionSource<Exception> unhandledExceptionTsc = new();
	private Exception? capturedUnhandledException;

	public int RenderCount { get; private set; }

	public override Dispatcher Dispatcher { get; } = Dispatcher.CreateDefault();

	public Task<Exception> UnhandledException => unhandledExceptionTsc.Task;

	public BunitRenderer(IServiceProvider serviceProvider, ILoggerFactory loggerFactory)
		: base(serviceProvider, loggerFactory)
	{
		logger = loggerFactory.CreateLogger<BunitRenderer>();
		componentTreeManager = new(this);
	}

	/// <summary>
	/// Renders the <paramref name="renderFragment"/> inside a <see cref="RootComponent"/>.
	/// This returns as soon all components in the <paramref name="renderFragment"/>
	/// have finished their first render cycle. It does not await any async life cycle methods
	/// in the components. To await life cycle methods, use <see cref="RenderAsync(RenderFragment)"/>.
	/// </summary>
	public IRenderedComponent<RootComponent> Render(RenderFragment renderFragment)
	{
		try
		{
			var renderTask = Dispatcher.InvokeAsync(() =>
			{
				var rc = InitializeRenderedRootComponent(renderFragment);
				_ = RenderRootComponentAsync(rc.ComponentId);
				return rc;
			});
			Debug.Assert(renderTask.IsCompletedSuccessfully, "There should not be any asynchronous code in the inside the lambda passed to Dispatcher.InvokeAsync.");
			return renderTask.Result;
		}
		catch (Exception ex)
		{
			HandleException(ex);
			throw;
		}
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
				var rc = InitializeRenderedRootComponent(renderFragment);
				await RenderRootComponentAsync(rc.ComponentId).ConfigureAwait(false);
				return rc;
			}).ConfigureAwait(false);
		}
		catch (Exception ex)
		{
			HandleException(ex);
			throw;
		}
	}

	private IRenderedComponent<RootComponent> InitializeRenderedRootComponent(RenderFragment renderFragment)
	{
		var component = new RootComponent(renderFragment);
		var componentId = AssignRootComponentId(component);
		var treeRoot = componentTreeManager.CreateTreeRoot(componentId, component);
		return new RenderedComponentV2<RootComponent>(treeRoot);
	}

	protected override Task UpdateDisplayAsync(in RenderBatch renderBatch)
	{
		RenderCount++;
		componentTreeManager.UpdateComponentTrees(in renderBatch);
		return Task.CompletedTask;
	}

	public override Task DispatchEventAsync(ulong eventHandlerId, EventFieldInfo? fieldInfo, EventArgs eventArgs)
	{
		return Dispatcher.InvokeAsync(() => base.DispatchEventAsync(eventHandlerId, fieldInfo, eventArgs));
	}

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

	protected override void Dispose(bool disposing)
	{
		base.Dispose(disposing);
		componentTreeManager.Dispose();
	}
}
