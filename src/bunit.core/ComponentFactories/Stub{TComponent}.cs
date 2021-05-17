using System;
using Bunit.Rendering;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Bunit.ComponentFactories
{
	internal class Stub<TComponent> : ComponentBase
		where TComponent : IComponent
	{
		protected override void BuildRenderTree(RenderTreeBuilder builder)
		{
			builder.AddMarkupContent(0, $"<{typeof(TComponent).Name}></{typeof(TComponent).Name}>");
		}
	}
}
