using System;
using System.Collections.Generic;
using Bunit.Extensions;
using Bunit.Rendering;
using Microsoft.AspNetCore.Components;

namespace Bunit
{
	/// <summary>
	/// A test context is a factory that makes it possible to create components under tests.
	/// </summary>
	public class TestContext : TestContextBase
	{
		/// <summary>
		/// Gets the <see cref="RootRenderTree"/> that all components rendered with the
		/// <c>RenderComponent&lt;TComponent&gt;()</c> methods, are rendered inside.
		/// </summary>
		/// <remarks>
		/// Use this to add default layout- or root-components which a component under test
		/// should be rendered under.
		/// </remarks>
		public RootRenderTree RenderTree { get; } = new RootRenderTree();

		/// <summary>
		/// Creates a new instance of the <see cref="TestContext"/> class.
		/// </summary>
		public TestContext() => Services.AddDefaultTestContextServices();

		/// <summary>
		/// Instantiates and performs a first render of a component of type <typeparamref name="TComponent"/>.
		/// </summary>
		/// <typeparam name="TComponent">Type of the component to render</typeparam>
		/// <param name="parameters">Parameters to pass to the component when it is rendered</param>
		/// <returns>The rendered <typeparamref name="TComponent"/></returns>
		public virtual IRenderedComponent<TComponent> RenderComponent<TComponent>(params ComponentParameter[] parameters) where TComponent : IComponent
		{
			return RenderComponent<TComponent>(new ComponentParameterCollection { parameters }.ToRenderFragment<TComponent>());
		}

		/// <summary>
		/// Instantiates and performs a first render of a component of type <typeparamref name="TComponent"/>.
		/// </summary>
		/// <typeparam name="TComponent">Type of the component to render</typeparam>
		/// <param name="parameterBuilder">The ComponentParameterBuilder action to add type safe parameters to pass to the component when it is rendered</param>
		/// <returns>The rendered <typeparamref name="TComponent"/></returns>
		public virtual IRenderedComponent<TComponent> RenderComponent<TComponent>(Action<ComponentParameterCollectionBuilder<TComponent>> parameterBuilder) where TComponent : IComponent
		{
			return RenderComponent<TComponent>(
				new ComponentParameterCollectionBuilder<TComponent>(parameterBuilder)
				.Build()
				.ToRenderFragment<TComponent>()
			);
		}

		private IRenderedComponent<TComponent> RenderComponent<TComponent>(RenderFragment renderFragment) where TComponent : IComponent
		{
			// Wrap TComponent in any layout components added to the test context.
			// If one of the layout components is the same type as TComponent,
			// make sure to return the rendered component, not the layout component.			
			var resultBase = Renderer.RenderFragment(RenderTree.Wrap(renderFragment));

			// This ensures that the correct component is returned, in case an added layout component
			// is of type TComponent.
			var renderTreeTComponentCount = RenderTree.GetCountOf<TComponent>();
			var result = renderTreeTComponentCount > 0
				? Renderer.FindComponents<TComponent>(resultBase)[renderTreeTComponentCount]
				: Renderer.FindComponent<TComponent>(resultBase);

			return (IRenderedComponent<TComponent>)result;
		}
	}
}
