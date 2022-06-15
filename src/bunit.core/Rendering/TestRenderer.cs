using System.Runtime.ExceptionServices;
using Microsoft.Extensions.Logging;

namespace Bunit.Rendering;

/// <summary>
/// Represents a bUnit <see cref="ITestRenderer"/> used to render Blazor components and fragments during bUnit tests.
/// </summary>
public class TestRenderer : Renderer, ITestRenderer
{
	private readonly Dictionary<int, IRenderedFragmentBase> renderedComponents = new();
	private readonly List<RootComponent> rootComponents = new();
	private readonly ILogger<TestRenderer> logger;
	private readonly IRenderedComponentActivator activator;
	private TaskCompletionSource<Exception> unhandledExceptionTsc = new(TaskCreationOptions.RunContinuationsAsynchronously);
	private Exception? capturedUnhandledException;

	/// <inheritdoc/>
	public Task<Exception> UnhandledException => unhandledExceptionTsc.Task;

	/// <inheritdoc/>
	public override Dispatcher Dispatcher { get; } = Dispatcher.CreateDefault();

	/// <summary>
	/// Gets the number of render cycles that has been performed.
	/// </summary>
	internal int RenderCount { get; private set; }

	/// <summary>
	/// Initializes a new instance of the <see cref="TestRenderer"/> class.
	/// </summary>
	public TestRenderer(IRenderedComponentActivator renderedComponentActivator, TestServiceProvider services, ILoggerFactory loggerFactory)
		: base(services, loggerFactory)
	{
		logger = loggerFactory.CreateLogger<TestRenderer>();
		this.activator = renderedComponentActivator;
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="TestRenderer"/> class.
	/// </summary>
	public TestRenderer(IRenderedComponentActivator renderedComponentActivator, TestServiceProvider services, ILoggerFactory loggerFactory, IComponentActivator componentActivator)
		: base(services, loggerFactory, componentActivator)
	{
		logger = loggerFactory.CreateLogger<TestRenderer>();
		this.activator = renderedComponentActivator;
	}

	/// <inheritdoc/>
	public IRenderedFragmentBase RenderFragment(RenderFragment renderFragment)
		=> Render(renderFragment, id => activator.CreateRenderedFragment(id));

	/// <inheritdoc/>
	public IRenderedComponentBase<TComponent> RenderComponent<TComponent>(ComponentParameterCollection parameters)
		where TComponent : IComponent
	{
		if (parameters is null)
			throw new ArgumentNullException(nameof(parameters));

		var renderFragment = parameters.ToRenderFragment<TComponent>();
		return Render(renderFragment, id => activator.CreateRenderedComponent<TComponent>(id));
	}

	/// <inheritdoc/>
	public new Task DispatchEventAsync(
		ulong eventHandlerId,
		EventFieldInfo fieldInfo,
		EventArgs eventArgs) => DispatchEventAsync(eventHandlerId, fieldInfo, eventArgs, ignoreUnknownEventHandlers: false);

	/// <inheritdoc/>
	public Task DispatchEventAsync(
		ulong eventHandlerId,
		EventFieldInfo fieldInfo,
		EventArgs eventArgs,
		bool ignoreUnknownEventHandlers)
	{
		if (fieldInfo is null)
			throw new ArgumentNullException(nameof(fieldInfo));

		var result = Dispatcher.InvokeAsync(() =>
		{
			ResetUnhandledException();

			try
			{
				return base.DispatchEventAsync(eventHandlerId, fieldInfo, eventArgs);
			}
			catch (ArgumentException ex) when (string.Equals(ex.Message, $"There is no event handler associated with this event. EventId: '{eventHandlerId}'. (Parameter 'eventHandlerId')", StringComparison.Ordinal))
			{
				if (ignoreUnknownEventHandlers)
				{
					return Task.CompletedTask;
				}

				var betterExceptionMsg = new UnknownEventHandlerIdException(eventHandlerId, fieldInfo, ex);
				return Task.FromException(betterExceptionMsg);
			}
		});

		if (result.IsFaulted && result.Exception is not null)
		{
			HandleException(result.Exception);
		}

		AssertNoUnhandledExceptions();

		return result;
	}

	/// <inheritdoc/>
	public IRenderedComponentBase<TComponent> FindComponent<TComponent>(IRenderedFragmentBase parentComponent)
		where TComponent : IComponent
	{
		var foundComponents = FindComponents<TComponent>(parentComponent, 1);
		return foundComponents.Count == 1
			? foundComponents[0]
			: throw new ComponentNotFoundException(typeof(TComponent));
	}

	/// <inheritdoc/>
	public IReadOnlyList<IRenderedComponentBase<TComponent>> FindComponents<TComponent>(IRenderedFragmentBase parentComponent)
		where TComponent : IComponent
		=> FindComponents<TComponent>(parentComponent, int.MaxValue);


	/// <inheritdoc />
	public void DisposeComponents()
	{
		// The dispatcher will always return a completed task,
		// when dealing with an IAsyncDisposable.
		// Therefore checking for a completed task and awaiting it
		// will only work on IDisposable
		var disposeTask = Dispatcher.InvokeAsync(() =>
		{
			ResetUnhandledException();

			foreach (var root in rootComponents)
			{
				root.Detach();
			}
		});

		if (!disposeTask.IsCompleted)
		{
			disposeTask.GetAwaiter().GetResult();
		}

		rootComponents.Clear();
		AssertNoUnhandledExceptions();
	}

	/// <inheritdoc/>
	protected override Task UpdateDisplayAsync(in RenderBatch renderBatch)
	{
		logger.LogNewRenderBatchReceived();

		RenderCount++;
		
		var renderEvent = new RenderEvent(renderBatch, new RenderTreeFrameDictionary());

		// removes disposed components
		for (var i = 0; i < renderBatch.DisposedComponentIDs.Count; i++)
		{
			var id = renderBatch.DisposedComponentIDs.Array[i];

			logger.LogComponentDisposed(id);

			if (renderedComponents.TryGetValue(id, out var rc))
			{
				renderedComponents.Remove(id);
				rc.OnRender(renderEvent);
			}
		}

		// notify each rendered component about the render
		foreach (var (key, rc) in renderedComponents.ToArray())
		{
			logger.LogComponentRendered(rc.ComponentId);

			LoadRenderTreeFrames(rc.ComponentId, renderEvent.Frames);

			rc.OnRender(renderEvent);

			// RC can replace the instance of the component it is bound
			// to while processing the update event.
			if (key != rc.ComponentId)
			{
				renderedComponents.Remove(key);
				renderedComponents.Add(rc.ComponentId, rc);
			}
		}

		logger.LogChangedComponentsMarkupUpdated();

		return Task.CompletedTask;
	}

	/// <inheritdoc/>
	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			foreach (var rc in renderedComponents.Values)
			{
				rc.Dispose();
			}

			renderedComponents.Clear();
			unhandledExceptionTsc.TrySetCanceled();
		}

		base.Dispose(disposing);
	}

	private TResult Render<TResult>(RenderFragment renderFragment, Func<int, TResult> activator)
		where TResult : IRenderedFragmentBase
	{
		var renderTask = Dispatcher.InvokeAsync(() =>
		{
			ResetUnhandledException();

			var root = new RootComponent(renderFragment);
			var rootComponentId = AssignRootComponentId(root);
			var result = activator(rootComponentId);
			renderedComponents.Add(rootComponentId, result);
			rootComponents.Add(root);
			root.Render();
			return result;
		});

		TResult result;

		if (!renderTask.IsCompleted)
		{
			logger.LogAsyncInitialRender();
			result = renderTask.GetAwaiter().GetResult();
		}
		else
		{
			result = renderTask.Result;
		}

		logger.LogInitialRenderCompleted(result.ComponentId);

		AssertNoUnhandledExceptions();

		return result;
	}

	private IReadOnlyList<IRenderedComponentBase<TComponent>> FindComponents<TComponent>(IRenderedFragmentBase parentComponent, int resultLimit)
		where TComponent : IComponent
	{
		if (parentComponent is null)
			throw new ArgumentNullException(nameof(parentComponent));

		// Ensure FindComponents runs on the same thread as the renderer,
		// and that the renderer does not perform any renders while
		// FindComponents is traversing the current render tree.
		// Without this, the render tree could change while FindComponentsInternal
		// is traversing down the render tree, with indeterministic as a results.
		return Dispatcher.InvokeAsync(() =>
		{
			var result = new List<IRenderedComponentBase<TComponent>>();
			var framesCollection = new RenderTreeFrameDictionary();

			FindComponentsInRenderTree(parentComponent.ComponentId);

			return result;

			void FindComponentsInRenderTree(int componentId)
			{
				var frames = GetOrLoadRenderTreeFrame(framesCollection, componentId);

				for (var i = 0; i < frames.Count; i++)
				{
					ref var frame = ref frames.Array[i];
					if (frame.FrameType == RenderTreeFrameType.Component)
					{
						if (frame.Component is TComponent component)
						{
							result.Add(GetOrCreateRenderedComponent(framesCollection, frame.ComponentId, component));

							if (result.Count == resultLimit)
								return;
						}

						FindComponentsInRenderTree(frame.ComponentId);

						if (result.Count == resultLimit)
							return;
					}
				}
			}
		}).GetAwaiter().GetResult();
	}

	IRenderedComponentBase<TComponent> GetOrCreateRenderedComponent<TComponent>(RenderTreeFrameDictionary framesCollection, int componentId, TComponent component)
		where TComponent : IComponent
	{
		IRenderedComponentBase<TComponent> result;

		if (renderedComponents.TryGetValue(componentId, out var renderedComponent))
		{
			result = (IRenderedComponentBase<TComponent>)renderedComponent;
		}
		else
		{
			LoadRenderTreeFrames(componentId, framesCollection);
			result = activator.CreateRenderedComponent(componentId, component, framesCollection);
			renderedComponents.Add(result.ComponentId, result);
		}

		return result;
	}

	/// <summary>
	/// Populates the <paramref name="framesCollection"/> with <see cref="ArrayRange{RenderTreeFrame}"/>
	/// starting with the one that belongs to the component with ID <paramref name="componentId"/>.
	/// </summary>
	private void LoadRenderTreeFrames(int componentId, RenderTreeFrameDictionary framesCollection)
	{
		var frames = GetOrLoadRenderTreeFrame(framesCollection, componentId);

		for (var i = 0; i < frames.Count; i++)
		{
			ref var frame = ref frames.Array[i];
			if (frame.FrameType == RenderTreeFrameType.Component)
			{
				LoadRenderTreeFrames(frame.ComponentId, framesCollection);
			}
		}
	}

	/// <summary>
	/// Gets the <see cref="ArrayRange{RenderTreeFrame}"/> from the <paramref name="framesCollection"/>.
	/// If the <paramref name="framesCollection"/> does not contain the frames, they are loaded into it first.
	/// </summary>
	private ArrayRange<RenderTreeFrame> GetOrLoadRenderTreeFrame(RenderTreeFrameDictionary framesCollection, int componentId)
	{
		if (!framesCollection.Contains(componentId))
		{
			var frames = GetCurrentRenderTreeFrames(componentId);
			framesCollection.Add(componentId, frames);
		}

		return framesCollection[componentId];
	}

	/// <inheritdoc/>
	protected override void HandleException(Exception exception)
	{
		if (exception is null)
			return;

		logger.LogUnhandledException(exception);

		capturedUnhandledException = exception;

		if (!unhandledExceptionTsc.TrySetResult(capturedUnhandledException))
		{
			unhandledExceptionTsc = new TaskCompletionSource<Exception>(TaskCreationOptions.RunContinuationsAsynchronously);
			unhandledExceptionTsc.SetResult(capturedUnhandledException);
		}
	}

	private void ResetUnhandledException()
	{
		capturedUnhandledException = null;

		if (unhandledExceptionTsc.Task.IsCompleted)
			unhandledExceptionTsc = new TaskCompletionSource<Exception>(TaskCreationOptions.RunContinuationsAsynchronously);
	}

	private void AssertNoUnhandledExceptions()
	{
		if (capturedUnhandledException is Exception unhandled)
		{
			capturedUnhandledException = null;

			if (unhandled is AggregateException aggregateException && aggregateException.InnerExceptions.Count == 1)
			{
				ExceptionDispatchInfo.Capture(aggregateException.InnerExceptions[0]).Throw();
			}
			else
			{
				ExceptionDispatchInfo.Capture(unhandled).Throw();
			}
		}
	}
}
