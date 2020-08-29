using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;

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

		public RenderedComponent(int componentId, IServiceProvider services) : base(componentId, services) { }

		public RenderedComponent(int componentId, TComponent instance, RenderTreeFrameCollection componentFrames, IServiceProvider services) : base(componentId, services)
		{
			_instance = instance;
			RenderCount++;
			UpdateMarkup(componentFrames);
		}

		protected override void OnRender(RenderEvent renderEvent)
		{
			if (_instance is null)
			{
				SetComponentAndID(renderEvent);
			}
		}

		private void SetComponentAndID(RenderEvent renderEvent)
		{			
			if (renderEvent.Frames.TryFindComponent<TComponent>(ComponentId, out var id, out var component))
			{
				_instance = component;
				ComponentId = id;
			}
			else
			{
				throw new InvalidOperationException("Component instance not found at expected position in render tree.");
			}
		}
	}
}
