using System;

namespace Bunit.TestDoubles
{
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
#if !NET6_0_OR_GREATER
		public Bunit.TestDoubles.NavigationOptions Options { get; }
#endif
#if NET6_0_OR_GREATER
		public Microsoft.AspNetCore.Components.NavigationOptions Options { get; }
#endif

		/// <summary>
		/// Initializes a new instance of the <see cref="NavigationHistory"/> class.
		/// </summary>
		/// <param name="uri"></param>
		/// <param name="options"></param>
#if !NET6_0_OR_GREATER
		public NavigationHistory(string uri, Bunit.TestDoubles.NavigationOptions options)
		{
			Uri = uri;
			Options = options;
		}
#endif
#if NET6_0_OR_GREATER
		public NavigationHistory(string uri, Microsoft.AspNetCore.Components.NavigationOptions options)
		{
			Uri = uri;
			Options = options;
		}
#endif

		/// <inheritdoc/>
#if !NET6_0_OR_GREATER
		public bool Equals(NavigationHistory? other)
			=> other is not null && string.Equals(Uri, other.Uri, StringComparison.Ordinal) && Options.Equals(other.Options);
#endif
#if NET6_0_OR_GREATER
		public bool Equals(NavigationHistory? other)
			=> other is not null
			&& string.Equals(Uri, other.Uri, StringComparison.Ordinal)
			&& Options.ForceLoad == other.Options.ForceLoad
			&& Options.ReplaceHistoryEntry == other.Options.ReplaceHistoryEntry;
#endif

		/// <inheritdoc/>
		public override bool Equals(object? obj) => obj is NavigationHistory other && Equals(other);

		/// <inheritdoc/>
		public override int GetHashCode() => HashCode.Combine(Uri, Options);
	}
}
