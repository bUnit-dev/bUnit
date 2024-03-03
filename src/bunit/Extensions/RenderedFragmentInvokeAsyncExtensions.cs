using Bunit.Rendering;

namespace Bunit;

/// <summary>
/// InvokeAsync extensions methods on <see cref="IRenderedFragment"/>.
/// </summary>
public static class RenderedFragmentInvokeAsyncExtensions
{
	/// <summary>
	/// Invokes the given <paramref name="workItem"/> in the context of the associated <see cref="ITestRenderer"/>.
	/// </summary>
	/// <param name="renderedFragment">The rendered fragment whose dispatcher to invoke with.</param>
	/// <param name="workItem">The work item to execute on the renderer's thread.</param>
	/// <returns>A <see cref="Task"/> that will be completed when the action has finished executing or is suspended by an asynchronous operation.</returns>
	public static Task InvokeAsync(this IRenderedFragment renderedFragment, Action workItem)
	{
		ArgumentNullException.ThrowIfNull(renderedFragment);

		return renderedFragment
			.Services
			.GetRequiredService<TestContext>()
			.Renderer
			.Dispatcher
			.InvokeAsync(workItem);
	}

	/// <summary>
	/// Invokes the given <paramref name="workItem"/> in the context of the associated <see cref="ITestRenderer"/>.
	/// </summary>
	/// <param name="renderedFragment">The rendered component whose dispatcher to invoke with.</param>
	/// <param name="workItem">The work item to execute on the renderer's thread.</param>
	/// <returns>A <see cref="Task"/> that will be completed when the action has finished executing.</returns>
	public static Task InvokeAsync(this IRenderedFragment renderedFragment, Func<Task> workItem)
	{
		ArgumentNullException.ThrowIfNull(renderedFragment);

		return renderedFragment
			.Services
			.GetRequiredService<TestContext>()
			.Renderer
			.Dispatcher
			.InvokeAsync(workItem);
	}

	/// <summary>
	/// Invokes the given <paramref name="workItem"/> in the context of the associated <see cref="ITestRenderer"/>.
	/// </summary>
	/// <param name="renderedFragment">The rendered component whose dispatcher to invoke with.</param>
	/// <param name="workItem">The work item to execute on the renderer's thread.</param>
	/// <returns>A <see cref="Task"/> that will be completed when the action has finished executing, with the return value from <paramref name="workItem"/>.</returns>
	public static Task<T> InvokeAsync<T>(this IRenderedFragment renderedFragment, Func<T> workItem)
	{
		ArgumentNullException.ThrowIfNull(renderedFragment);

		return renderedFragment
			.Services
			.GetRequiredService<TestContext>()
			.Renderer
			.Dispatcher
			.InvokeAsync(workItem);
	}

	/// <summary>
	/// Invokes the given <paramref name="workItem"/> in the context of the associated <see cref="ITestRenderer"/>.
	/// </summary>
	/// <param name="renderedFragment">The rendered component whose dispatcher to invoke with.</param>
	/// <param name="workItem">The work item to execute on the renderer's thread.</param>
	/// <returns>A <see cref="Task"/> that will be completed when the action has finished executing, with the return value from <paramref name="workItem"/>.</returns>
	public static Task<T> InvokeAsync<T>(this IRenderedFragment renderedFragment, Func<Task<T>> workItem)
	{
		ArgumentNullException.ThrowIfNull(renderedFragment);

		return renderedFragment
			.Services
			.GetRequiredService<TestContext>()
			.Renderer
			.Dispatcher
			.InvokeAsync(workItem);
	}
}
