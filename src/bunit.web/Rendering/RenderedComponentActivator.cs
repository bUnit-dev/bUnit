using System;

using Microsoft.AspNetCore.Components;

namespace Bunit.Rendering
{
	/// <summary>
	/// Represents a rendered component activator for bUnit.web.
	/// </summary>
	public sealed class RenderedComponentActivator : IRenderedComponentActivator
	{
		private readonly IServiceProvider _services;

		/// <summary>
		/// Creates an instance of the activator.
		/// </summary>
		public RenderedComponentActivator(IServiceProvider services)
		{
			_services = services;
		}

		/// <inheritdoc/>
		public IRenderedFragmentBase CreateRenderedFragment(int componentId)
			=> new RenderedFragment(componentId, _services);

		/// <inheritdoc/>
		public IRenderedComponentBase<TComponent> CreateRenderedComponent<TComponent>(int componentId) where TComponent : IComponent
			=> new RenderedComponent<TComponent>(componentId, _services);

		/// <inheritdoc/>
		public IRenderedComponentBase<TComponent> CreateRenderedComponent<TComponent>(int componentId, TComponent component, RenderTreeFrameCollection componentFrames)
			where TComponent : IComponent
			=> new RenderedComponent<TComponent>(componentId, component, componentFrames, _services);
	}
}
