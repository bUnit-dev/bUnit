using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Runtime.ExceptionServices;

namespace Bunit.Rendering.Internal;

public class BunitRendererV2 : Renderer
{
	private readonly object renderTreeUpdateLock = new();
	private readonly SynchronizationContext? usersSyncContext = SynchronizationContext.Current;
	private readonly ILogger<BunitRendererV2> logger;
	private readonly TestServiceProvider services;
	private TaskCompletionSource<Exception> unhandledExceptionTsc = new(TaskCreationOptions.RunContinuationsAsynchronously);
	private Exception? capturedUnhandledException;
	private bool isDisposed;

	/// <summary>
	/// Gets a <see cref="Task{Exception}"/>, which completes when an unhandled exception
	/// is thrown during the rendering of a component, that is caught by the renderer.
	/// </summary>
	public Task<Exception> UnhandledException => unhandledExceptionTsc.Task;

	/// <inheritdoc/>
	public override Dispatcher Dispatcher { get; } = Dispatcher.CreateDefault();

	/// <summary>
	/// Gets the number of render cycles that has been performed.
	/// </summary>
	internal int RenderCount { get; private set; }

	/// <summary>
	/// Initializes a new instance of the <see cref="BunitRenderer"/> class.
	/// </summary>
	public BunitRendererV2(TestServiceProvider services, ILoggerFactory loggerFactory)
		: base(services, loggerFactory)
	{
		logger = loggerFactory.CreateLogger<BunitRendererV2>();
		ElementReferenceContext = new WebElementReferenceContext(services.GetRequiredService<IJSRuntime>());
		this.services = services;
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="BunitRenderer"/> class.
	/// </summary>
	public BunitRendererV2(TestServiceProvider services, ILoggerFactory loggerFactory, IComponentActivator componentActivator)
		: base(services, loggerFactory, componentActivator)
	{
		logger = loggerFactory.CreateLogger<BunitRendererV2>();
		ElementReferenceContext = new WebElementReferenceContext(services.GetRequiredService<IJSRuntime>());
		this.services = services;
	}

	/// <summary>
	/// Renders the <paramref name="renderFragment"/>.
	/// </summary>
	/// <param name="renderFragment">The <see cref="Microsoft.AspNetCore.Components.RenderFragment"/> to render.</param>
	/// <returns>A <see cref="IRenderedFragment"/> that provides access to the rendered <paramref name="renderFragment"/>.</returns>
	public IRenderedFragment RenderFragment(RenderFragment renderFragment)
	{
		ObjectDisposedException.ThrowIf(isDisposed, this);

		lock (renderTreeUpdateLock)
		{
			ResetUnhandledException();
			var rootComponentId = -1;

			_ = Dispatcher.InvokeAsync(() =>
			{
				var component = new BunitRootComponent(renderFragment);
				rootComponentId = AssignRootComponentId(component);
				RenderRootComponentAsync(rootComponentId);
			});

			Debug.Assert(rootComponentId >= 0, "ID should have been assigned by now!");

			AssertNoUnhandledExceptions();

			return GetComponentState(rootComponentId).GetRenderedComponent<BunitRootComponent>();
		}
	}

	/// <summary>
	/// Notifies the renderer that an event has occurred.
	/// </summary>
	/// <param name="eventHandlerId">The <see cref="RenderTreeFrame.AttributeEventHandlerId"/> value from the original event attribute.</param>
	/// <param name="fieldInfo">Information that the renderer can use to update the state of the existing render tree to match the UI.</param>
	/// <param name="eventArgs">Arguments to be passed to the event handler.</param>
	/// <returns>A <see cref="Task"/> which will complete once all asynchronous processing related to the event has completed.</returns>
	public override Task DispatchEventAsync(
		ulong eventHandlerId,
		EventFieldInfo? fieldInfo,
		EventArgs eventArgs) => DispatchEventAsync(eventHandlerId, fieldInfo, eventArgs, ignoreUnknownEventHandlers: false);

	/// <summary>
	/// Notifies the renderer that an event has occurred.
	/// </summary>
	/// <param name="eventHandlerId">The <see cref="RenderTreeFrame.AttributeEventHandlerId"/> value from the original event attribute.</param>
	/// <param name="fieldInfo">Information that the renderer can use to update the state of the existing render tree to match the UI.</param>
	/// <param name="eventArgs">Arguments to be passed to the event handler.</param>
	/// <param name="ignoreUnknownEventHandlers">Set to true to ignore the <see cref="UnknownEventHandlerIdException"/>.</param>
	/// <returns>A <see cref="Task"/> which will complete once all asynchronous processing related to the event has completed.</returns>
	public new Task DispatchEventAsync(
		ulong eventHandlerId,
		EventFieldInfo? fieldInfo,
		EventArgs eventArgs,
		bool ignoreUnknownEventHandlers)
	{
		ObjectDisposedException.ThrowIf(isDisposed, this);
		ArgumentNullException.ThrowIfNull(fieldInfo);

		// Calling base.DispatchEventAsync updates the render tree
		// if the event contains associated data.
		lock (renderTreeUpdateLock)
		{
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
	}

	/// <summary>
	/// Performs a depth-first search for the first <typeparamref name="TComponent"/> child component of the <paramref name="parentComponent"/>.
	/// </summary>
	/// <typeparam name="TComponent">Type of component to find.</typeparam>
	/// <param name="parentComponent">Parent component to search.</param>
	public IRenderedComponent<TComponent> FindComponent<TComponent>(IRenderedFragment parentComponent)
		where TComponent : IComponent
		=> throw new NotImplementedException();

	/// <summary>
	/// Performs a depth-first search for all <typeparamref name="TComponent"/> child components of the <paramref name="parentComponent"/>.
	/// </summary>
	/// <typeparam name="TComponent">Type of components to find.</typeparam>
	/// <param name="parentComponent">Parent component to search.</param>
	public IReadOnlyList<IRenderedComponent<TComponent>> FindComponents<TComponent>(IRenderedFragment parentComponent)
		where TComponent : IComponent
		=> throw new NotImplementedException();

	/// <inheritdoc/>
	/// <remarks>This method returns a <see cref="BunitComponentState"/>.</remarks>
	protected override ComponentState CreateComponentState(int componentId, IComponent component, ComponentState? parentComponentState)
		=> parentComponentState is null
		? new BunitRootComponentState(this, componentId, (BunitRootComponent)component)
		: new BunitChildComponentState(this, componentId, component, (BunitComponentState)parentComponentState);

	/// <inheritdoc/>
	protected override void ProcessPendingRender()
	{
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
		RenderCount++;

		for (int i = 0; i < renderBatch.UpdatedComponents.Count; i++)
		{
			ref var diff = ref renderBatch.UpdatedComponents.Array[i];
			GetComponentState(diff.ComponentId).UpdateMarkup(RenderCount);
		}

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
			}, null);
		}
		else
		{
			// TODO
		}

		return Task.CompletedTask;


	}

	/// <inheritdoc/>
	protected override IComponent ResolveComponentForRenderMode(Type componentType, int? parentComponentId,
		IComponentActivator componentActivator, IComponentRenderMode componentTypeRenderMode)

	{
		ArgumentNullException.ThrowIfNull(componentActivator);
		return componentActivator.CreateInstance(componentType);
	}

	/// <inheritdoc/>
	protected override void HandleException(Exception exception)
	{
		if (exception is null)
			return;

		capturedUnhandledException = exception;

		if (!unhandledExceptionTsc.TrySetResult(capturedUnhandledException))
		{
			unhandledExceptionTsc = new TaskCompletionSource<Exception>(TaskCreationOptions.RunContinuationsAsynchronously);
			unhandledExceptionTsc.SetResult(capturedUnhandledException);
		}
	}

	internal new BunitComponentState GetComponentState(int componentId)
	{
		var result = base.GetComponentState(componentId) as BunitComponentState;
		Debug.Assert(result is not null, "ComponentState should be of type BunitComponentState");
		return result;
	}

	internal new ArrayRange<RenderTreeFrame> GetCurrentRenderTreeFrames(int componentId)
		=> base.GetCurrentRenderTreeFrames(componentId);

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
