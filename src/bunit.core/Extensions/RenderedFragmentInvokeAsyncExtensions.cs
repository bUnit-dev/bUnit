using Bunit.Rendering;

namespace Bunit;

/// <summary>
/// InvokeAsync extensions methods on <see cref="IRenderedFragmentBase"/>.
/// </summary>
public static class RenderedFragmentInvokeAsyncExtensions
{
	/// <summary>
	/// Invokes the given <paramref name="workItem"/> in the context of the associated <see cref="ITestRenderer"/>.
	/// </summary>
	/// <param name="renderedFragment">The rendered fragment whose dispatcher to invoke with.</param>
	/// <param name="workItem">The work item to execute on the renderer's thread.</param>
	/// <returns>A <see cref="Task"/> that will be completed when the action has finished executing or is suspended by an asynchronous operation.</returns>
	public static Task InvokeAsync(this IRenderedFragmentBase renderedFragment, Action workItem)
	{
		if (renderedFragment is null)
			throw new ArgumentNullException(nameof(renderedFragment));

		return renderedFragment.Services.GetRequiredService<ITestRenderer>()
			.Dispatcher.InvokeAsync(workItem);
	}

	/// <summary>
	/// Invokes the given <paramref name="workItem"/> in the context of the associated <see cref="ITestRenderer"/>.
	/// </summary>
	/// <param name="renderedFragment">The rendered component whose dispatcher to invoke with.</param>
	/// <param name="workItem">The work item to execute on the renderer's thread.</param>
	/// <returns>A <see cref="Task"/> that will be completed when the action has finished executing.</returns>
	public static Task InvokeAsync(this IRenderedFragmentBase renderedFragment, Func<Task> workItem)
	{
		if (renderedFragment is null)
			throw new ArgumentNullException(nameof(renderedFragment));

		return renderedFragment.Services.GetRequiredService<ITestRenderer>()
			.Dispatcher.InvokeAsync(workItem);
	}
}
