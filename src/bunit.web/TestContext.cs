using System;
using Bunit.Extensions;
using Bunit.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;

namespace Bunit
{
	/// <summary>
	/// A test context is a factory that makes it possible to create components under tests.
	/// </summary>
	public class TestContext : TestContextBase
	{
		/// <summary>
		/// Gets bUnits JSInterop, that allows setting up handlers for <see cref="IJSRuntime.InvokeAsync{TValue}(string, object[])"/> invocations
		/// that components under tests will issue during testing. It also makes it possible to verify that the invocations has happened as expected.
		/// </summary>
		public BunitJSInterop JSInterop { get; } = new BunitJSInterop();

		/// <summary>
		/// Creates a new instance of the <see cref="TestContext"/> class.
		/// </summary>
		public TestContext()
		{
			JSInterop.AddBuiltInJSRuntimeInvocationHandlers();
			Services.AddSingleton<IJSRuntime>(JSInterop.JSRuntime);
			Services.AddDefaultTestContextServices();
		}

		/// <summary>
		/// Instantiates and performs a first render of a component of type <typeparamref name="TComponent"/>.
		/// </summary>
		/// <typeparam name="TComponent">Type of the component to render</typeparam>
		/// <param name="parameters">Parameters to pass to the component when it is rendered</param>
		/// <returns>The rendered <typeparamref name="TComponent"/></returns>
		public virtual IRenderedComponent<TComponent> RenderComponent<TComponent>(params ComponentParameter[] parameters) where TComponent : IComponent
		{
			var renderFragment = new ComponentParameterCollection { parameters }.ToRenderFragment<TComponent>();
			return (IRenderedComponent<TComponent>)RenderComponent<TComponent>(renderFragment);
		}

		/// <summary>
		/// Instantiates and performs a first render of a component of type <typeparamref name="TComponent"/>.
		/// </summary>
		/// <typeparam name="TComponent">Type of the component to render</typeparam>
		/// <param name="parameterBuilder">The ComponentParameterBuilder action to add type safe parameters to pass to the component when it is rendered</param>
		/// <returns>The rendered <typeparamref name="TComponent"/></returns>
		public virtual IRenderedComponent<TComponent> RenderComponent<TComponent>(Action<ComponentParameterCollectionBuilder<TComponent>> parameterBuilder) where TComponent : IComponent
		{
			var renderFragment = new ComponentParameterCollectionBuilder<TComponent>(parameterBuilder).Build().ToRenderFragment<TComponent>();
			return (IRenderedComponent<TComponent>)RenderComponent<TComponent>(renderFragment);
		}
	}
}
