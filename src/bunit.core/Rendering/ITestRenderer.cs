using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.RenderTree;

namespace Bunit.Rendering
{
	/// <summary>
	/// Represents a generalized Blazor renderer for testing purposes.
	/// </summary>
	public interface ITestRenderer : IRenderEventProducer
	{
		/// <summary>
		/// Gets the <see cref="Dispatcher"/> associated with this <see cref="ITestRenderer"/>.
		/// </summary>
		Dispatcher Dispatcher { get; }

		///// <summary>
		///// Invokes the given <paramref name="callback"/> in the context of this <see cref="ITestRenderer"/>.
		///// </summary>
		///// <param name="callback"></param>
		///// <returns>A <see cref="Task"/> that will be completed when the action has finished executing.</returns>
		//Task InvokeAsync(Action callback);

		/// <summary>
		/// Instantiates and renders the component of type <typeparamref name="TComponent"/>.
		/// </summary>
		/// <typeparam name="TComponent">Type of component to render.</typeparam>
		/// <param name="parameters">Parameters to pass to the component during first render.</param>
		/// <returns>The component and its assigned id.</returns>
		(int ComponentId, TComponent Component) RenderComponent<TComponent>(IEnumerable<ComponentParameter> parameters) where TComponent : IComponent;

		/// <summary>
		/// Renders the provided <paramref name="renderFragment"/> inside a wrapper and returns
		/// the wrappers component id.
		/// </summary>
		/// <param name="renderFragment"><see cref="Microsoft.AspNetCore.Components.RenderFragment"/> to render.</param>
		/// <returns>The id of the wrapper component which the <paramref name="renderFragment"/> is rendered inside.</returns>
		int RenderFragment(RenderFragment renderFragment);

		/// <summary>
		/// Notifies the renderer that an event has occurred.
		/// </summary>
		/// <param name="eventHandlerId">The <see cref="RenderTreeFrame.AttributeEventHandlerId"/> value from the original event attribute.</param>
		/// <param name="fieldInfo">Information that the renderer can use to update the state of the existing render tree to match the UI.</param>
		/// <param name="eventArgs">Arguments to be passed to the event handler.</param>
		/// <returns>A <see cref="Task"/> which will complete once all asynchronous processing related to the event has completed.</returns>
		Task DispatchEventAsync(ulong eventHandlerId, EventFieldInfo fieldInfo, EventArgs eventArgs);

		/// <summary>
		/// Performs a depth-first search for a <typeparamref name="TComponent"/> child component of the component with the <paramref name="parentComponentId"/>.
		/// </summary>
		/// <typeparam name="TComponent">Type of component to look for.</typeparam>
		/// <param name="parentComponentId">The id of the parent component.</param>
		/// <returns>The first matching child component.</returns>
		(int ComponentId, TComponent Component) FindComponent<TComponent>(int parentComponentId);

		/// <summary>
		/// Performs a depth-first search for all <typeparamref name="TComponent"/> child components of the component with the <paramref name="parentComponentId"/>.
		/// </summary>
		/// <typeparam name="TComponent">Type of components to look for.</typeparam>
		/// <param name="parentComponentId">The id of the parent component.</param>
		/// <returns>The matching child components.</returns>
		IReadOnlyList<(int ComponentId, TComponent Component)> FindComponents<TComponent>(int parentComponentId);

		/// <summary>
		/// Gets the current render tree for a given component.
		/// </summary>
		/// <param name="componentId">The id for the component.</param>
		/// <returns>The <see cref="RenderTreeBuilder"/> representing the current render tree.</returns>
		ArrayRange<RenderTreeFrame> GetCurrentRenderTreeFrames(int componentId);
	}
}
