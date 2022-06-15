namespace Bunit.TestDoubles;

/// <summary>
/// Represents a navigation to a <seealso cref="Uri"/> with a set of specific navigation <seealso cref="Options"/>.
/// </summary>
public sealed class NavigationHistory : IEquatable<NavigationHistory>
{
	/// <summary>
	/// Gets the <see cref="Uri"/> that was navigated to.
	/// </summary>
	public string Uri { get; }

	/// <summary>
	/// Gets the options that was specified when the <see name="Uri"/> was navigated to.
	/// </summary>
	public Microsoft.AspNetCore.Components.NavigationOptions Options { get; }

	/// <summary>
	/// Initializes a new instance of the <see cref="NavigationHistory"/> class.
	/// </summary>
	/// <param name="uri"></param>
	/// <param name="options"></param>
	public NavigationHistory(string uri, Microsoft.AspNetCore.Components.NavigationOptions options)
	{
		Uri = uri;
		Options = options;
	}

	/// <inheritdoc/>
	public bool Equals(NavigationHistory? other)
		=> other is not null
		&& string.Equals(Uri, other.Uri, StringComparison.Ordinal)
		&& Options.ForceLoad == other.Options.ForceLoad
		&& Options.ReplaceHistoryEntry == other.Options.ReplaceHistoryEntry;

	/// <inheritdoc/>
	public override bool Equals(object? obj) => obj is NavigationHistory other && Equals(other);

	/// <inheritdoc/>
	public override int GetHashCode() => HashCode.Combine(Uri, Options);
}
