using System;
using System.Threading.Tasks;
using Bunit.Rendering;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;

namespace Bunit
{
	/// <summary>
	/// InvokeAsync extensions methods on <see cref="IRenderedComponentBase{TComponent}"/>.
	/// </summary>
	public static class RenderedComponentInvokeAsyncExtensions
	{
		/// <summary>
		/// Invokes the given <paramref name="workItem"/> in the context of the associated <see cref="ITestRenderer"/>.
		/// </summary>
		/// <param name="renderedComponent">The rendered component whose dispatcher to invoke with.</param>
		/// <param name="workItem">The work item to execute on the renderer's thread.</param>
		/// <typeparam name="TComponent">The component type of the <see cref="IRenderedComponentBase{TComponent}"/>.</typeparam>
		/// <returns>A <see cref="Task"/> that will be completed when the action has finished executing or is suspended by an asynchronous operation.</returns>
		public static Task InvokeAsync<TComponent>(this IRenderedComponentBase<TComponent> renderedComponent, Action workItem)
			where TComponent : IComponent
		{
			if (renderedComponent is null)
				throw new ArgumentNullException(nameof(renderedComponent));

			return renderedComponent.Services.GetRequiredService<ITestRenderer>()
				.Dispatcher.InvokeAsync(workItem);
		}

		/// <summary>
		/// Invokes the given <paramref name="workItem"/> in the context of the associated <see cref="ITestRenderer"/>.
		/// </summary>
		/// <param name="renderedComponent">The rendered component whose dispatcher to invoke with.</param>
		/// <param name="workItem">The work item to execute on the renderer's thread.</param>
		/// <typeparam name="TComponent">The component type of the <see cref="IRenderedComponentBase{TComponent}"/>.</typeparam>
		/// <returns>A <see cref="Task"/> that will be completed when the action has finished executing.</returns>
		public static Task InvokeAsync<TComponent>(this IRenderedComponentBase<TComponent> renderedComponent, Func<Task> workItem)
			where TComponent : IComponent
		{
			if (renderedComponent is null)
				throw new ArgumentNullException(nameof(renderedComponent));

			return renderedComponent.Services.GetRequiredService<ITestRenderer>()
				.Dispatcher.InvokeAsync(workItem);
		}
	}
}
