using System.Diagnostics;
using Bunit.Rendering;
using Microsoft.AspNetCore.Components.Routing;

namespace Bunit.TestDoubles;

using URI = Uri;

/// <summary>
/// Represents a fake <see cref="NavigationManager"/> that captures calls to
/// <see cref="NavigationManager.NavigateTo(string, bool)"/> for testing purposes.
/// </summary>
[DebuggerDisplay("Current Uri: {Uri}, History Count: {History.Count}")]
public sealed class BunitNavigationManager : NavigationManager
{
	private readonly BunitRenderer renderer;
	private readonly Stack<NavigationHistory> history = new();

	/// <summary>
	/// The navigation history captured by the <see cref="BunitNavigationManager"/>.
	/// This is a stack based collection, so the first element is the latest/current navigation target.
	/// </summary>
	/// <remarks>
	/// The initial Uri is not added to the history.
	/// </remarks>
	public IReadOnlyCollection<NavigationHistory> History => history;

	/// <summary>
	/// Initializes a new instance of the <see cref="BunitNavigationManager"/> class.
	/// </summary>
	public BunitNavigationManager(BunitRenderer renderer)
	{
		this.renderer = renderer ?? throw new ArgumentNullException(nameof(renderer));
		Initialize("http://localhost/", "http://localhost/");
	}

	/// <inheritdoc/>
	protected override void NavigateToCore(string uri, NavigationOptions options)
	{
		var absoluteUri = GetNewAbsoluteUri(uri);
		var changedBaseUri = HasDifferentBaseUri(absoluteUri);

		if (changedBaseUri)
		{
			BaseUri = GetBaseUri(absoluteUri);
		}

		Uri = ToAbsoluteUri(uri).OriginalString;

		if (options.ReplaceHistoryEntry && history.Count > 0)
			history.Pop();

		renderer.Dispatcher.InvokeAsync(async () =>
		{
			Uri = absoluteUri.OriginalString;

			var shouldContinueNavigation = false;
			try
			{
				shouldContinueNavigation = await NotifyLocationChangingAsync(uri, options.HistoryEntryState, isNavigationIntercepted: false).ConfigureAwait(false);
			}
			catch (Exception exception)
			{
				history.Push(new NavigationHistory(uri, options, NavigationState.Faulted, exception));
				return;
			}

			history.Push(new NavigationHistory(uri, options, shouldContinueNavigation ? NavigationState.Succeeded : NavigationState.Prevented));

			if (!shouldContinueNavigation)
			{
				return;
			}

			// Only notify of changes if user navigates within the same
			// base url (domain). Otherwise, the user navigated away
			// from the app, and Blazor's NavigationManager would
			// not notify of location changes.
			if (!changedBaseUri)
			{
				NotifyLocationChanged(isInterceptedLink: false);
			}
			else
			{
				BaseUri = GetBaseUri(absoluteUri);
			}
		});
	}

	/// <inheritdoc/>
	protected override void SetNavigationLockState(bool value) {}

	/// <inheritdoc/>
	protected override void HandleLocationChangingHandlerException(Exception ex, LocationChangingContext context)
		=> throw ex;

	private URI GetNewAbsoluteUri(string uri)
		=> new URI(uri, UriKind.RelativeOrAbsolute).IsAbsoluteUri
			? new URI(uri, UriKind.RelativeOrAbsolute) : ToAbsoluteUri(uri);

	private bool HasDifferentBaseUri(URI absoluteUri)
		=> URI.Compare(
			new URI(BaseUri, UriKind.Absolute),
			absoluteUri,
			UriComponents.SchemeAndServer,
			UriFormat.Unescaped,
			StringComparison.OrdinalIgnoreCase) != 0;

	private static string GetBaseUri(URI uri)
	{
		return uri.Scheme + "://" + uri.Authority + "/";
	}
}
