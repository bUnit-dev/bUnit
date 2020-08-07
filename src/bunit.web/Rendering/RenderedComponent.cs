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
		private readonly TComponent _instance;

		/// <inheritdoc/>
		public TComponent Instance
		{
			get
			{
				EnsureComponentNotDisposed();
				return _instance;
			}
		}

		public RenderedComponent(IServiceProvider services, int componentId, TComponent component) : base(services, componentId)
		{
			_instance = component;
		}
	}
}
