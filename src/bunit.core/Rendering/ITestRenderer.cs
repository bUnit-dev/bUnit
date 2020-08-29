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

		IRenderedFragmentBase RenderFragment(RenderFragment renderFragment);

		IRenderedComponentBase<TComponent> RenderComponent<TComponent>(IEnumerable<ComponentParameter> componentParameters)
			where TComponent : IComponent;

		IRenderedComponentBase<TComponent> FindComponent<TComponent>(IRenderedFragmentBase parentComponent)
			where TComponent : IComponent;

		IReadOnlyList<IRenderedComponentBase<TComponent>> FindComponents<TComponent>(IRenderedFragmentBase parentComponent)
			where TComponent : IComponent;
	}
}
