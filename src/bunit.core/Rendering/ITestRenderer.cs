using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.RenderTree;

namespace Bunit.Rendering
{
	/// <summary>
	/// Represents a generalized Blazor renderer for testing purposes.
	/// </summary>
	public interface ITestRenderer
	{
		/// <summary>
		/// Gets the <see cref="Dispatcher"/> associated with this <see cref="ITestRenderer"/>.
		/// </summary>
		Dispatcher Dispatcher { get; }

		/// <summary>
		/// Notifies the renderer that an event has occurred.
		/// </summary>
		/// <param name="eventHandlerId">The <see cref="RenderTreeFrame.AttributeEventHandlerId"/> value from the original event attribute.</param>
		/// <param name="fieldInfo">Information that the renderer can use to update the state of the existing render tree to match the UI.</param>
		/// <param name="eventArgs">Arguments to be passed to the event handler.</param>
		/// <returns>A <see cref="Task"/> which will complete once all asynchronous processing related to the event has completed.</returns>
		Task DispatchEventAsync(ulong eventHandlerId, EventFieldInfo fieldInfo, EventArgs eventArgs);

		/// <summary>
		/// Renders the <paramref name="renderFragment"/>.
		/// </summary>
		/// <param name="renderFragment">The <see cref="Microsoft.AspNetCore.Components.RenderFragment"/> to render.</param>
		/// <returns>A <see cref="IRenderedFragmentBase"/> that provides access to the rendered <paramref name="renderFragment"/>.</returns>
		IRenderedFragmentBase RenderFragment(RenderFragment renderFragment);

		/// <summary>
		/// Renders a <typeparamref name="TComponent"/> with the <paramref name="parameters"/> passed to it.
		/// </summary>
		/// <typeparam name = "TComponent" > The type of component to render.</typeparam>
		/// <param name="parameters">The parameters to pass to the component.</param>
		/// <returns>A <see cref="IRenderedComponentBase{TComponent}"/> that provides access to the rendered component.</returns>
		IRenderedComponentBase<TComponent> RenderComponent<TComponent>(ComponentParameterCollection parameters)
			where TComponent : IComponent;

		/// <summary>
		/// Performs a depth-first search for the first <typeparamref name="TComponent"/> child component of the <paramref name="parentComponent"/>.
		/// </summary>
		/// <typeparam name="TComponent">Type of component to find.</typeparam>
		/// <param name="parentComponent">Parent component to search.</param>
		IRenderedComponentBase<TComponent> FindComponent<TComponent>(IRenderedFragmentBase parentComponent)
			where TComponent : IComponent;

		/// <summary>
		/// Performs a depth-first search for all <typeparamref name="TComponent"/> child components of the <paramref name="parentComponent"/>.
		/// </summary>
		/// <typeparam name="TComponent">Type of components to find.</typeparam>
		/// <param name="parentComponent">Parent component to search.</param>
		IReadOnlyList<IRenderedComponentBase<TComponent>> FindComponents<TComponent>(IRenderedFragmentBase parentComponent)
			where TComponent : IComponent;
	}
}
