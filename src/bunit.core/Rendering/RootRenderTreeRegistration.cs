using System;
using Microsoft.AspNetCore.Components;

namespace Bunit.Rendering
{
	/// <summary>
	/// Represents an added component with parameters in an <see cref="RootRenderTree"/>.
	/// </summary>
	public sealed class RootRenderTreeRegistration
	{
		/// <summary>
		/// Gets the type of component registered.
		/// </summary>
		public Type ComponentType { get; }

		/// <summary>
		/// Gets the render fragment builder that renders the component of type <see cref="ComponentType"/>
		/// with the specified parameters and the provided <see cref="RenderFragment"/> passed to its
		/// ChildContent or Body parameter.
		/// </summary>
		public RenderFragment<RenderFragment> RenderFragmentBuilder { get; }

		/// <summary>
		/// Initializes a new instance of the <see cref="RootRenderTreeRegistration"/> class.
		/// </summary>
		internal RootRenderTreeRegistration(Type componentType, RenderFragment<RenderFragment> renderFragmentBuilder)
		{
			ComponentType = componentType;
			RenderFragmentBuilder = renderFragmentBuilder;
		}
	}
}
