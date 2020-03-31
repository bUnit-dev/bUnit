using System;
using Bunit.Rendering;
using Microsoft.AspNetCore.Components;

namespace Bunit
{
	/// <summary>
	/// A test context is a factory that makes it possible to create components under tests.
	/// </summary>
	public interface ITestContext : IDisposable
	{
		/// <summary>
		/// Gets the service collection and service provider that is used when a 
		/// component is rendered by the test context.
		/// </summary>
		TestServiceProvider Services { get; }

		/// <summary>
		/// Gets an <see cref="IObservable{RenderEvent}"/> which will provide subscribers with <see cref="RenderEvent"/>s from the
		/// related to this <see cref="ITestContext"/>.
		/// </summary>
		public IObservable<RenderEvent> RenderEvents { get; }

		/// <summary>
		/// Instantiates and performs a first render of a component of type <typeparamref name="TComponent"/>.
		/// </summary>
		/// <typeparam name="TComponent">Type of the component to render</typeparam>
		/// <param name="parameters">Parameters to pass to the component when it is rendered</param>
		/// <returns>The rendered <typeparamref name="TComponent"/></returns>
		IRenderedComponent<TComponent> RenderComponent<TComponent>(params ComponentParameter[] parameters) where TComponent : IComponent;
	}
}
