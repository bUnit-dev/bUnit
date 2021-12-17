using Bunit.Rendering;

namespace Bunit.TestDoubles;

/// <summary>
/// Represents a fake <see cref="NavigationManager"/> that captures calls to
/// <see cref="NavigationManager.NavigateTo(string, bool)"/> for testing purposes.
/// </summary>
public sealed class FakeNavigationManager : NavigationManager
{
	private readonly ITestRenderer renderer;
	private readonly Stack<NavigationHistory> history = new();

	/// <summary>
	/// The navigation history captured by the <see cref="FakeNavigationManager"/>.
	/// This is a stack based collection, so the first element is the latest/current navigation target.
	/// </summary>
	/// <remarks>
	/// The initial Uri is not added to the history.
	/// </remarks>
	public IReadOnlyCollection<NavigationHistory> History => history;

	/// <summary>
	/// Initializes a new instance of the <see cref="FakeNavigationManager"/> class.
	/// </summary>
	[SuppressMessage("Minor Code Smell", "S1075:URIs should not be hardcoded", Justification = "By design. Fake navigation manager defaults to local host as base URI.")]
	public FakeNavigationManager(ITestRenderer renderer)
	{
		this.renderer = renderer ?? throw new ArgumentNullException(nameof(renderer));
		Initialize("http://localhost/", "http://localhost/");
	}

#if !NET6_0_OR_GREATER
	/// <inheritdoc/>
	protected override void NavigateToCore(string uri, bool forceLoad)
	{
		Uri = ToAbsoluteUri(uri).OriginalString;
		history.Push(new NavigationHistory(uri, new NavigationOptions(forceLoad)));

		renderer.Dispatcher.InvokeAsync(() =>
		{
			Uri = ToAbsoluteUri(uri).OriginalString;
			NotifyLocationChanged(isInterceptedLink: false);
		});
	}
#endif

#if NET6_0_OR_GREATER

		/// <inheritdoc/>
		protected override void NavigateToCore(string uri, NavigationOptions options)
		{
			Uri = ToAbsoluteUri(uri).OriginalString;

			if (options.ReplaceHistoryEntry && history.Count > 0)
				history.Pop();

			history.Push(new NavigationHistory(uri, options));

			renderer.Dispatcher.InvokeAsync(() =>
			{
				Uri = ToAbsoluteUri(uri).OriginalString;
				NotifyLocationChanged(isInterceptedLink: false);
			});
		}
#endif
}
