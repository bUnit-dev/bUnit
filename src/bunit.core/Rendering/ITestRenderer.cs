using System;
using System.Threading.Tasks;
using Bunit.Rendering.RenderEvents;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.RenderTree;

namespace Bunit.Rendering
{
	public interface ITestRenderer
	{
		Dispatcher Dispatcher { get; }
		IObservable<RenderEvent> RenderEvents { get; }

		Task DispatchEventAsync(ulong eventHandlerId, EventFieldInfo fieldInfo, EventArgs eventArgs);
		(int ComponentId, TComponent Component) FindComponent<TComponent>(int parentComponentId);
		System.Collections.Generic.IReadOnlyList<(int ComponentId, TComponent Component)> FindComponents<TComponent>(int parentComponentId);
		ArrayRange<RenderTreeFrame> GetCurrentRenderTreeFrames(int componentId);
		void InvokeAsync(Action callback);
		Task<(int ComponentId, TComponent Component)> RenderComponent<TComponent>(params ComponentParameter[] parameters) where TComponent : IComponent;
		Task<int> RenderFragment(RenderFragment renderFragment);
	}
}