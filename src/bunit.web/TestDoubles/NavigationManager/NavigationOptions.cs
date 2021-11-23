#if !NET6_0_OR_GREATER
using System;

namespace Bunit.TestDoubles;

/// <summary>
/// Additional options for navigating to another URI.
/// </summary>	
public sealed class NavigationOptions : IEquatable<NavigationOptions>
{
	/// <summary>
	/// If true, bypasses client-side routing and forces the browser to load the new
	/// page from the server, whether or not the URI would normally be handled by the
	/// client-side router.
	/// </summary>
	public bool ForceLoad { get; }

	/// <summary>
	/// Initializes a new instance of the <see cref="NavigationOptions"/> class.
	/// </summary>
	public NavigationOptions(bool forceLoad) => ForceLoad = forceLoad;

	/// <inheritdoc/>
	public bool Equals(NavigationOptions? other)
		=> other is not null && ForceLoad == other.ForceLoad;

	/// <inheritdoc/>
	public override bool Equals(object? obj)
		=> obj is NavigationOptions other && Equals(other);

	/// <inheritdoc/>
	public override int GetHashCode() => ForceLoad.GetHashCode();
}
#endif
