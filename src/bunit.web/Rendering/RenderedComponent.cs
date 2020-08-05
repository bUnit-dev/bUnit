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
		/// <inheritdoc/>
		public TComponent Instance { get; }

		public RenderedComponent(IServiceProvider services, int componentId, TComponent component) : base(services, componentId)
		{
			Instance = component;
		}		
	}
}
