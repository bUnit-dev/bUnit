using Bunit.Rendering;

namespace Bunit;

/// <summary>
/// InvokeAsync extensions methods on <see cref="IRenderedComponent{TComponent}"/>.
/// </summary>
public static class RenderedComponentInvokeAsyncExtensions
{
	/// <summary>
	/// Invokes the given <paramref name="workItem"/> in the context of the associated <see cref="BunitRenderer"/>.
	/// </summary>
	/// <param name="renderedComponent">The rendered fragment whose dispatcher to invoke with.</param>
	/// <param name="workItem">The work item to execute on the renderer's thread.</param>
	/// <returns>A <see cref="Task"/> that will be completed when the action has finished executing or is suspended by an asynchronous operation.</returns>
	public static Task InvokeAsync<TComponent>(this IRenderedComponent<TComponent> renderedComponent, Action workItem)
		where TComponent : IComponent
	{
		ArgumentNullException.ThrowIfNull(renderedComponent);

		return renderedComponent
			.Services
			.GetRequiredService<BunitContext>()
			.Renderer
			.Dispatcher
			.InvokeAsync(workItem);
	}

	/// <summary>
	/// Invokes the given <paramref name="workItem"/> in the context of the associated <see cref="BunitRenderer"/>.
	/// </summary>
	/// <param name="renderedComponent">The rendered component whose dispatcher to invoke with.</param>
	/// <param name="workItem">The work item to execute on the renderer's thread.</param>
	/// <returns>A <see cref="Task"/> that will be completed when the action has finished executing.</returns>
	public static Task InvokeAsync<TComponent>(this IRenderedComponent<TComponent> renderedComponent, Func<Task> workItem)
		where TComponent : IComponent
	{
		ArgumentNullException.ThrowIfNull(renderedComponent);

		return renderedComponent
			.Services
			.GetRequiredService<BunitContext>()
			.Renderer
			.Dispatcher
			.InvokeAsync(workItem);
	}

	/// <summary>
	/// Invokes the given <paramref name="workItem"/> in the context of the associated <see cref="BunitRenderer"/>.
	/// </summary>
	/// <param name="renderedComponent">The rendered component whose dispatcher to invoke with.</param>
	/// <param name="workItem">The work item to execute on the renderer's thread.</param>
	/// <returns>A <see cref="Task"/> that will be completed when the action has finished executing, with the return value from <paramref name="workItem"/>.</returns>
	public static Task<T> InvokeAsync<TComponent, T>(this IRenderedComponent<TComponent> renderedComponent, Func<T> workItem)
		where TComponent : IComponent
	{
		ArgumentNullException.ThrowIfNull(renderedComponent);

		return renderedComponent
			.Services
			.GetRequiredService<BunitContext>()
			.Renderer
			.Dispatcher
			.InvokeAsync(workItem);
	}

	/// <summary>
	/// Invokes the given <paramref name="workItem"/> in the context of the associated <see cref="BunitRenderer"/>.
	/// </summary>
	/// <param name="renderedComponent">The rendered component whose dispatcher to invoke with.</param>
	/// <param name="workItem">The work item to execute on the renderer's thread.</param>
	/// <returns>A <see cref="Task"/> that will be completed when the action has finished executing, with the return value from <paramref name="workItem"/>.</returns>
	public static Task<T> InvokeAsync<TComponent, T>(this IRenderedComponent<TComponent> renderedComponent, Func<Task<T>> workItem)
		where TComponent : IComponent
	{
		ArgumentNullException.ThrowIfNull(renderedComponent);

		return renderedComponent
			.Services
			.GetRequiredService<BunitContext>()
			.Renderer
			.Dispatcher
			.InvokeAsync(workItem);
	}
}
