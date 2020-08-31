using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.RenderTree;

namespace Bunit.Rendering
{
	/// <inheritdoc/>
	internal class RenderedComponent<TComponent> : RenderedFragment, IRenderedComponent<TComponent> where TComponent : IComponent
	{
		private TComponent _instance = default!;

		/// <inheritdoc/>
		public TComponent Instance
		{
			get
			{
				EnsureComponentNotDisposed();
				return _instance ?? throw new InvalidOperationException("Component has not rendered yet...");
			}
		}

		internal RenderedComponent(int componentId, IServiceProvider services) : base(componentId, services) { }

		internal RenderedComponent(int componentId, TComponent instance, RenderTreeFrameCollection componentFrames, IServiceProvider services) : base(componentId, services)
		{
			_instance = instance;
			RenderCount++;
			UpdateMarkup(componentFrames);
		}

		protected override void OnRender(RenderEvent renderEvent)
		{
			// checks if this is the first render, and if it is
			// tries to find the TCompoent in the render event			
			if (_instance is null)
			{
				SetComponentAndID(renderEvent);
			}
		}

		private void SetComponentAndID(RenderEvent renderEvent)
		{			
			if (TryFindComponent(renderEvent.Frames, ComponentId, out var id, out var component))
			{
				_instance = component;
				ComponentId = id;
			}
			else
			{
				throw new InvalidOperationException("Component instance not found at expected position in render tree.");
			}
		}

		private bool TryFindComponent(RenderTreeFrameCollection framesCollection, int parentComponentId, out int componentId, out TComponent component)
		{
			var result = false;
			componentId = -1;
			component = default!;

			var frames = framesCollection[parentComponentId];

			for (var i = 0; i < frames.Count; i++)
			{
				ref var frame = ref frames.Array[i];
				if (frame.FrameType == RenderTreeFrameType.Component)
				{
					if (frame.Component is TComponent c)
					{
						componentId = frame.ComponentId;
						component = c;
						result = true;
						break;
					}

					if (TryFindComponent(framesCollection, frame.ComponentId, out componentId, out component))
					{
						result = true;
						break;
					}
				}
			}

			return result;
		}
	}
}
