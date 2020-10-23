using System;
using System.Collections.Generic;
using Bunit.Extensions;
using Microsoft.AspNetCore.Components;

namespace Bunit
{
	/// <summary>
	/// A test context is a factory that makes it possible to create components under tests.
	/// </summary>
	public class TestContext : TestContextBase, ITestContext
	{
		private readonly List<(Type LayoutType, RenderFragment<RenderFragment> LayoutFragment)> _layouts = new();

		/// <summary>
		/// Creates a new instance of the <see cref="TestContext"/> class.
		/// </summary>
		public TestContext() => Services.AddDefaultTestContextServices();

		/// <summary>
		/// Adds a "layout" component to the test context, which components rendered using one of the 
		/// <c>RenderComponent&lt;TComponent&gt;()</c>> methods will be rendered inside. This method can
		/// be called multiple times, with each invocation adding a <typeparamref name="TComponent"/>
		/// to the "layout" render tree. The <typeparamref name="TComponent"/> must have a <c>ChildContent</c>
		/// or <c>Body</c> parameter.
		/// </summary>
		/// <typeparam name="TComponent">The type of the component to use as a layout component.</typeparam>
		/// <param name="parameterBuilder">An optional parameter builder, used to pass parameters to <typeparamref name="TComponent"/>.</param>
		public virtual void AddLayoutComponent<TComponent>(Action<ComponentParameterCollectionBuilder<TComponent>>? parameterBuilder = null) where TComponent : IComponent
		{
			RenderFragment<RenderFragment> layoutFragment = rc =>
			{
				var builder = new ComponentParameterCollectionBuilder<TComponent>();
				parameterBuilder?.Invoke(builder);

				var added = builder.TryAdd("ChildContent", rc) || builder.TryAdd("Body", rc);
				if (!added)
					throw new ArgumentException($"The {typeof(TComponent)} does not have a ChildContent or Body parameter. Only components with one of these parameters can be used as layout components.");

				return builder.Build().ToRenderFragment<TComponent>();
			};

			_layouts.Add((typeof(TComponent), layoutFragment));
		}

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
			// make sure to return the rendered component, not the layout compnent.
			var rcType = typeof(TComponent);
			var rcInLayoutCount = 0;
			var wrappedRenderFragment = renderFragment;

			for (int i = 0; i < _layouts.Count; i++)
			{
				if (rcType == _layouts[i].LayoutType)
					rcInLayoutCount++;

				wrappedRenderFragment = _layouts[i].LayoutFragment(wrappedRenderFragment);
			}

			var resultBase = Renderer.RenderFragment(wrappedRenderFragment);

			// This ensures that the correct component is returned, in case an added layout component
			// is of type TComponent.
			var result = rcInLayoutCount > 0
				? Renderer.FindComponents<TComponent>(resultBase)[rcInLayoutCount]
				: Renderer.FindComponent<TComponent>(resultBase);

			return (IRenderedComponent<TComponent>)result;
		}
	}
}
