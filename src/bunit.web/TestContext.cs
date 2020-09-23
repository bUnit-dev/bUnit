using System;
using Bunit.Extensions;
using Bunit.Rendering;
using Microsoft.AspNetCore.Components;

namespace Bunit
{
	/// <summary>
	/// A test context is a factory that makes it possible to create components under tests.
	/// </summary>
	public class TestContext : TestContextBase, ITestContext
	{
		/// <summary>
		/// Creates a new instance of the <see cref="TestContext"/> class.
		/// </summary>
		public TestContext()
		{
			Services.AddDefaultTestContextServices();
		}

		/// <summary>
		/// Instantiates and performs a first render of a component of type <typeparamref name="TComponent"/>.
		/// </summary>
		/// <typeparam name="TComponent">Type of the component to render</typeparam>
		/// <param name="parameters">Parameters to pass to the component when it is rendered</param>
		/// <returns>The rendered <typeparamref name="TComponent"/></returns>
		public IRenderedComponent<TComponent> RenderComponent<TComponent>(params ComponentParameter[] parameters) where TComponent : IComponent
			=> TestRendererExtensions.RenderComponent<TComponent>(Renderer, parameters);

		/// <summary>
		/// Instantiates and performs a first render of a component of type <typeparamref name="TComponent"/>.
		/// </summary>
		/// <typeparam name="TComponent">Type of the component to render</typeparam>
		/// <param name="parameterBuilder">The ComponentParameterBuilder action to add type safe parameters to pass to the component when it is rendered</param>
		/// <returns>The rendered <typeparamref name="TComponent"/></returns>
		public virtual IRenderedComponent<TComponent> RenderComponent<TComponent>(Action<ComponentParameterCollectionBuilder<TComponent>> parameterBuilder) where TComponent : IComponent
			=> TestRendererExtensions.RenderComponent<TComponent>(Renderer, parameterBuilder);
	}
}
