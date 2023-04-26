using System.Text.Json;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

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
	/// Gets the <see cref="NavigationState"/> associated with this history entry.
	/// </summary>
	public NavigationState State { get; }

	/// <summary>
	/// Gets the exception thrown from the <see cref="NavigationLock.OnBeforeInternalNavigation"/> handler, if any.
	/// </summary>
	/// <remarks>
	/// Will not be null when <see cref="State"/> is <see cref="NavigationState.Faulted"/>.
	/// </remarks>
	public Exception? Exception { get; }

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

	/// <summary>
	/// Initializes a new instance of the <see cref="NavigationHistory"/> class.
	/// </summary>
	/// <param name="uri"></param>
	/// <param name="options"></param>
	/// <param name="navigationState"></param>
	/// <param name="exception"></param>
	[SuppressMessage("Design", "CA1054:URI-like parameters should not be strings", Justification = "Using string to align with NavigationManager")]
	public NavigationHistory(
		[StringSyntax(StringSyntaxAttribute.Uri)]string uri,
		NavigationOptions options,
		NavigationState navigationState,
		Exception? exception = null)
	{
		Uri = uri;
		Options = options;
		State = navigationState;
		Exception = exception;
	}

	/// <summary>
	/// Deserialize the content of <see cref="Options"/>.<see cref="NavigationOptions.HistoryEntryState"/>
	/// into <typeparamref name="T"/> if it is not null.
	/// </summary>
	/// <typeparam name="T">The type to deserialize the content of <see cref="Options"/>.<see cref="NavigationOptions.HistoryEntryState"/> to.</typeparam>
	/// <param name="options">The <see cref="JsonSerializerOptions" /> used when deserializing. If not provided, <see cref="JsonSerializerOptions.Default"/> is used.</param>
	/// <returns>The target type of the JSON value.</returns>
	/// <exception cref="InvalidOperationException">When <see cref="Options"/>.<see cref="NavigationOptions.HistoryEntryState"/> is null.</exception>
	public T? StateFromJson<T>(JsonSerializerOptions? options = null)
	{
		if (Options.HistoryEntryState is null)
		{
			throw new InvalidOperationException($"No {nameof(Options.HistoryEntryState)} has been set.");
		}

		return JsonSerializer.Deserialize<T>(
			Options.HistoryEntryState,
			options ?? JsonSerializerOptions.Default);
	}

	/// <inheritdoc/>
	public bool Equals(NavigationHistory? other)
	=> other is not null
	&& string.Equals(Uri, other.Uri, StringComparison.Ordinal)
	&& Options.ForceLoad == other.Options.ForceLoad
	&& Options.ReplaceHistoryEntry == other.Options.ReplaceHistoryEntry
	&& State == other.State
	&& Exception == other.Exception;

	/// <inheritdoc/>
	public override bool Equals(object? obj) => obj is NavigationHistory other && Equals(other);

	/// <inheritdoc/>
	public override int GetHashCode() => HashCode.Combine(Uri, Options);
}
