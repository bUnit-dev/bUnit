using System;
using System.Text;
using System.Threading.Tasks;
using Bunit.Rendering;
using Microsoft.AspNetCore.Components;

namespace Bunit
{
	/// <summary>
	/// InvokeAsync extensions methods on <see cref="IRenderedComponentBase{TComponent}"/>.
	/// </summary>
	public static class RenderedComponentInvokeAsyncExtensions
	{
		/// <summary>
		/// Invokes the given <paramref name="callback"/> in the context of the associated <see cref="ITestRenderer"/>.
		/// </summary>
		/// <param name="renderedComponent">The rendered component whose dispatcher to invoke with.</param>
		/// <param name="callback"></param>
		/// <returns>A <see cref="Task"/> that will be completed when the action has finished executing or is suspended by an asynchronous operation.</returns>
		public static Task InvokeAsync<TComponent>(this IRenderedComponentBase<TComponent> renderedComponent, Action callback)
			where TComponent : IComponent
		{
			if (renderedComponent is null)
				throw new ArgumentNullException(nameof(renderedComponent));

			return renderedComponent.Renderer.Dispatcher.InvokeAsync(callback);
		}

		/// <summary>
		/// Invokes the given <paramref name="callback"/> in the context of the associated <see cref="ITestRenderer"/>.
		/// </summary>
		/// <param name="renderedComponent">The rendered component whose dispatcher to invoke with.</param>
		/// <param name="callback"></param>
		/// <returns>A <see cref="Task"/> that will be completed when the action has finished executing.</returns>
		public static Task InvokeAsync<TComponent>(this IRenderedComponentBase<TComponent> renderedComponent, Func<Task> callback)
			where TComponent : IComponent
		{
			if (renderedComponent is null)
				throw new ArgumentNullException(nameof(renderedComponent));

			return renderedComponent.Renderer.Dispatcher.InvokeAsync(callback);
		}
	}
}
