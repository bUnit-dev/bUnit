using Microsoft.Extensions.Logging;
using System.Reflection;
using System.Runtime.ExceptionServices;

namespace Bunit.Rendering;

/// <summary>
/// Represents a bUnit <see cref="ITestRenderer"/> used to render Blazor components and fragments during bUnit tests.
/// </summary>
[SuppressMessage("Major Code Smell", "S3011:Reflection should not be used to increase accessibility of classes, methods, or fields", Justification = "Blazors internal API has been stable for a while now.")]
public class TestRenderer : Renderer, ITestRenderer
{
	private static readonly Type RendererType = typeof(Renderer);
	private static readonly FieldInfo IsBatchInProgressField = RendererType.GetField("_isBatchInProgress", BindingFlags.Instance | BindingFlags.NonPublic)!;
	private static readonly MethodInfo GetRequiredComponentStateMethod = RendererType.GetMethod("GetRequiredComponentState", BindingFlags.Instance | BindingFlags.NonPublic)!;

	private readonly object renderTreeUpdateLock = new();
	private readonly SynchronizationContext? usersSyncContext = SynchronizationContext.Current;
	private readonly Dictionary<int, IRenderedFragmentBase> renderedComponents = new();
	private readonly List<RootComponent> rootComponents = new();
	private readonly ILogger<TestRenderer> logger;
	private readonly IRenderedComponentActivator activator;
	private bool disposed;
	private TaskCompletionSource<Exception> unhandledExceptionTsc = new(TaskCreationOptions.RunContinuationsAsynchronously);
	private Exception? capturedUnhandledException;

	private bool IsBatchInProgress
	{
#pragma warning disable S1144 // Unused private types or members should be removed
		get => (bool)(IsBatchInProgressField.GetValue(this) ?? false);
#pragma warning restore S1144 // Unused private types or members should be removed
		set => IsBatchInProgressField.SetValue(this, value);
	}

	/// <inheritdoc/>
	public Task<Exception> UnhandledException => unhandledExceptionTsc.Task;

	/// <inheritdoc/>
	public override Dispatcher Dispatcher { get; } = Dispatcher.CreateDefault();

	/// <summary>
	/// Gets the number of render cycles that has been performed.
	/// </summary>
	internal int RenderCount { get; private set; }

#if NETSTANDARD
	/// <summary>
	/// Initializes a new instance of the <see cref="TestRenderer"/> class.
	/// </summary>
	public TestRenderer(IRenderedComponentActivator renderedComponentActivator, TestServiceProvider services, ILoggerFactory loggerFactory)
		: base(services, loggerFactory)
	{
		logger = loggerFactory.CreateLogger<TestRenderer>();
		this.activator = renderedComponentActivator;
	}
#elif NET5_0_OR_GREATER
	/// <summary>
	/// Initializes a new instance of the <see cref="TestRenderer"/> class.
	/// </summary>
	public TestRenderer(IRenderedComponentActivator renderedComponentActivator, TestServiceProvider services, ILoggerFactory loggerFactory)
		: base(services, loggerFactory, new BunitComponentActivator(services.GetRequiredService<ComponentFactoryCollection>(), null))
	{
		logger = loggerFactory.CreateLogger<TestRenderer>();
		this.activator = renderedComponentActivator;
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="TestRenderer"/> class.
	/// </summary>
	public TestRenderer(IRenderedComponentActivator renderedComponentActivator, TestServiceProvider services, ILoggerFactory loggerFactory, IComponentActivator componentActivator)
		: base(services, loggerFactory, new BunitComponentActivator(services.GetRequiredService<ComponentFactoryCollection>(), componentActivator))
	{
		logger = loggerFactory.CreateLogger<TestRenderer>();
		this.activator = renderedComponentActivator;
	}
#endif

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

		if (disposed)
			throw new ObjectDisposedException(nameof(TestRenderer));

		// Calling base.DispatchEventAsync updates the render tree
		// if the event contains associated data.
		lock (renderTreeUpdateLock)
		{
			var result = Dispatcher.InvokeAsync(() =>
			{
				ResetUnhandledException();

				try
				{
					logger.LogDispatchingEvent(eventHandlerId, fieldInfo, eventArgs);
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
		if (disposed)
			throw new ObjectDisposedException(nameof(TestRenderer));

		lock (renderTreeUpdateLock)
		{
			// The dispatcher will always return a completed task,
			// when dealing with an IAsyncDisposable.
			// Therefore checking for a completed task and awaiting it
			// will only work on IDisposable
			Dispatcher.InvokeAsync(() =>
			{
				ResetUnhandledException();

				foreach (var root in rootComponents)
				{
					root.Detach();
				}
			});

			rootComponents.Clear();
			AssertNoUnhandledExceptions();
		}
	}

	/// <inheritdoc/>
	internal void SetDirectParameters(IRenderedFragmentBase renderedComponent, ParameterView parameters)
	{
		// Calling SetDirectParameters updates the render tree
		// if the event contains associated data.
		lock (renderTreeUpdateLock)
		{
			var result = Dispatcher.InvokeAsync(() =>
			{
				try
				{
					IsBatchInProgress = true;

					var componentState = GetRequiredComponentStateMethod.Invoke(this, new object[] { renderedComponent.ComponentId })!;
					var setDirectParametersMethod = componentState.GetType().GetMethod("SetDirectParameters", BindingFlags.Public | BindingFlags.Instance)!;
					setDirectParametersMethod.Invoke(componentState, new object[] { parameters });
				}
				catch (TargetInvocationException ex) when (ex.InnerException is not null)
				{
					throw ex.InnerException;
				}
				finally
				{
					IsBatchInProgress = false;
				}

				ProcessPendingRender();
			});

			if (result.IsFaulted && result.Exception is not null)
			{
				HandleException(result.Exception);
			}

			AssertNoUnhandledExceptions();
		}
	}

	/// <inheritdoc/>
	protected override void ProcessPendingRender()
	{
		if (disposed)
		{
			logger.LogRenderCycleActiveAfterDispose();
			return;
		}

		// Blocks updates to the renderers internal render tree
		// while the render tree is being read elsewhere.
		// base.ProcessPendingRender calls UpdateDisplayAsync,
		// so there is no need to lock in that method.
		lock (renderTreeUpdateLock)
		{
			base.ProcessPendingRender();
		}
	}

	/// <inheritdoc/>
	protected override Task UpdateDisplayAsync(in RenderBatch renderBatch)
	{
		if (disposed)
		{
			return Task.CompletedTask;
		}

		var renderEvent = new RenderEvent();

		PrepareRenderEvent(renderBatch);
		InvokeApplyRenderEvent();

		return Task.CompletedTask;

		void PrepareRenderEvent(in RenderBatch renderBatch)
		{
			for (var i = 0; i < renderBatch.DisposedComponentIDs.Count; i++)
			{
				var id = renderBatch.DisposedComponentIDs.Array[i];
				renderEvent.SetDisposed(id);
			}

			for (int i = 0; i < renderBatch.UpdatedComponents.Count; i++)
			{
				ref var update = ref renderBatch.UpdatedComponents.Array[i];
				renderEvent.SetUpdated(update.ComponentId, update.Edits.Count > 0);
			}

			foreach (var (key, rc) in renderedComponents)
			{
				LoadChangesIntoRenderEvent(rc.ComponentId);
			}
		}

		void LoadChangesIntoRenderEvent(int componentId)
		{
			var status = renderEvent.GetStatus(componentId);
			if (status.FramesLoaded || status.Disposed)
			{
				return;
			}

			var frames = GetCurrentRenderTreeFrames(componentId);
			renderEvent.AddFrames(componentId, frames);

			for (var i = 0; i < frames.Count; i++)
			{
				ref var frame = ref frames.Array[i];
				if (frame.FrameType == RenderTreeFrameType.Component)
				{
					// If a child component of the current components has been
					// disposed, there is no reason to load the disposed components
					// render tree frames. This can also cause a stack overflow if
					// the current component was previously a child of the disposed
					// component (is that possible?)
					var childStatus = renderEvent.GetStatus(frame.ComponentId);
					if (childStatus.Disposed)
					{
						logger.LogDisposedChildInRenderTreeFrame(componentId, frame.ComponentId);
					}
					// The assumption is that a component cannot be in multiple places at
					// once. However, in case this is not a correct assumption, this
					// ensures that a child components frames are only loaded once.
					else if (!renderEvent.GetStatus(frame.ComponentId).FramesLoaded)
					{
						LoadChangesIntoRenderEvent(frame.ComponentId);
					}

					if (childStatus.Rendered || childStatus.Changed || childStatus.Disposed)
					{
						status.Rendered = status.Rendered || childStatus.Rendered;

						// The current component should also be marked as changed if the child component is
						// either changed or disposed, as there is a good chance that the child component
						// contained markup which is no longer visible.
						status.Changed = status.Changed || childStatus.Changed || childStatus.Disposed;
					}
				}
			}
		}

		void InvokeApplyRenderEvent()
		{
			if (usersSyncContext is not null && usersSyncContext != SynchronizationContext.Current)
			{
				// The users' sync context, typically one established by
				// xUnit or another testing framework is used to update any
				// rendered fragments/dom trees and trigger WaitForX handlers.
				// This ensures that changes to DOM observed inside a WaitForX handler
				// will also be visible outside a WaitForX handler, since
				// they will be running in the same sync context. The theory is that 
				// this should mitigate the issues where Blazor's dispatcher/thread is used 
				// to verify an assertion inside a WaitForX handler, and another thread is 
				// used again to access the DOM/repeat the assertion, where the change
				// may not be visible yet (another theory about why that may happen is different 
				// CPU cache updates not happening immediately).
				//
				// There is no guarantee a caller/test framework has set a sync context.
				usersSyncContext.Send(static (state) =>
				{
					var (renderEvent, renderer) = ((RenderEvent, TestRenderer))state!;
					renderer.ApplyRenderEvent(renderEvent);
				}, (renderEvent, this));
			}
			else
			{
				ApplyRenderEvent(renderEvent);
			}
		}
	}

	private void ApplyRenderEvent(RenderEvent renderEvent)
	{
		foreach (var (componentId, status) in renderEvent.Statuses)
		{
			if (status.UpdatesApplied || !renderedComponents.TryGetValue(componentId, out var rc))
			{
				continue;
			}

			if (status.Disposed)
			{
				renderedComponents.Remove(componentId);
				rc.OnRender(renderEvent);
				renderEvent.SetUpdatedApplied(componentId);
				logger.LogComponentDisposed(componentId);
				continue;
			}

			if (status.UpdateNeeded)
			{
				rc.OnRender(renderEvent);
				renderEvent.SetUpdatedApplied(componentId);

				// RC can replace the instance of the component it is bound
				// to while processing the update event, e.g. during the
				// initial render of a component.
				if (componentId != rc.ComponentId)
				{
					renderedComponents.Remove(componentId);
					renderedComponents.Add(rc.ComponentId, rc);
					renderEvent.SetUpdatedApplied(rc.ComponentId);
				}

				logger.LogComponentRendered(rc.ComponentId);
			}
		}
	}

	/// <inheritdoc/>
	protected override void Dispose(bool disposing)
	{
		if (disposed)
			return;

		disposed = true;

		lock (renderTreeUpdateLock)
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
	}

	private TResult Render<TResult>(RenderFragment renderFragment, Func<int, TResult> activator)
		where TResult : IRenderedFragmentBase
	{
		if (disposed)
			throw new ObjectDisposedException(nameof(TestRenderer));

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

		if (disposed)
			throw new ObjectDisposedException(nameof(TestRenderer));

		var result = new List<IRenderedComponentBase<TComponent>>();
		var framesCollection = new RenderTreeFrameDictionary();

		// Blocks the renderer from changing the render tree
		// while this method searches through it.
		lock (renderTreeUpdateLock)
		{
			FindComponentsInRenderTree(parentComponent.ComponentId);
		}

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
	}

	private IRenderedComponentBase<TComponent> GetOrCreateRenderedComponent<TComponent>(RenderTreeFrameDictionary framesCollection, int componentId, TComponent component)
		where TComponent : IComponent
	{
		if (renderedComponents.TryGetValue(componentId, out var renderedComponent))
		{
			return (IRenderedComponentBase<TComponent>)renderedComponent;
		}

		LoadRenderTreeFrames(componentId, framesCollection);
		var result = activator.CreateRenderedComponent(componentId, component, framesCollection);
		renderedComponents.Add(result.ComponentId, result);

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

			if (frame.FrameType == RenderTreeFrameType.Component && !framesCollection.Contains(frame.ComponentId))
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
		if (exception is null || disposed)
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
		// Ensure we are not throwing an exception while a render is ongoing.
		// This could lead to the renderer being disposed which could lead to
		// tests failing that should not be failing.
		lock (renderTreeUpdateLock)
		{
			if (capturedUnhandledException is Exception unhandled && !disposed)
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
}
