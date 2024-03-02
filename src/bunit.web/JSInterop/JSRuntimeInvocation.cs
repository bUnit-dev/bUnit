namespace Bunit;

/// <summary>
/// Represents an invocation of JavaScript via the JSRuntime Mock.
/// </summary>
[Serializable]
public readonly struct JSRuntimeInvocation : IEquatable<JSRuntimeInvocation>
{
	/// <summary>
	/// Gets the identifier used in the invocation.
	/// </summary>
	public string Identifier { get; }

	/// <summary>
	/// Gets the cancellation token used in the invocation, if any.
	/// </summary>
	public CancellationToken? CancellationToken { get; }

	/// <summary>
	/// Gets the arguments used in the invocation.
	/// </summary>
	public IReadOnlyList<object?> Arguments { get; }

	/// <summary>
	/// Gets whether the invocation has a <c>void</c> return type.
	/// </summary>
	public bool IsVoidResultInvocation { get; }

	/// <summary>
	/// Gets the result type of the invocation. If <see cref="IsVoidResultInvocation"/> then
	/// this will be of type <see cref="object"/>.
	/// </summary>
	public Type ResultType { get; }

	/// <summary>
	/// Gets the name of the method that initiated the invocation, e.g. <c>InvokeAsync</c> or <c>Invoke</c>.
	/// </summary>
	public string InvocationMethodName { get; }

	/// <summary>
	/// Initializes a new instance of the <see cref="JSRuntimeInvocation"/> struct.
	/// </summary>
	public JSRuntimeInvocation(string identifier, Type resultType, string invocationMethodName)
		: this(identifier, null, Array.Empty<object?>(), resultType, invocationMethodName)
	{ }

	/// <summary>
	/// Initializes a new instance of the <see cref="JSRuntimeInvocation"/> struct.
	/// </summary>
	public JSRuntimeInvocation(string identifier, object?[] args, Type resultType, string invocationMethodName)
		: this(identifier, null, args, resultType, invocationMethodName)
	{ }

	/// <summary>
	/// Initializes a new instance of the <see cref="JSRuntimeInvocation"/> struct.
	/// </summary>
	public JSRuntimeInvocation(
		string identifier,
		CancellationToken? cancellationToken,
		object?[]? args,
		Type resultType,
		string invocationMethodName)
	{
		Identifier = identifier;
		CancellationToken = cancellationToken;
		Arguments = args ?? Array.Empty<object?>();
		ResultType = resultType;
		InvocationMethodName = invocationMethodName;
#if !NET6_0_OR_GREATER
			IsVoidResultInvocation = resultType == typeof(object);
#else
		IsVoidResultInvocation = resultType == typeof(Microsoft.JSInterop.Infrastructure.IJSVoidResult);
#endif
	}

	/// <inheritdoc/>
	public bool Equals(JSRuntimeInvocation other)
		=> Identifier.Equals(other.Identifier, StringComparison.Ordinal)
		   && CancellationToken == other.CancellationToken
		   && ArgumentsEqual(Arguments, other.Arguments)
		   && ResultType == other.ResultType
		   && InvocationMethodName.Equals(other.InvocationMethodName, StringComparison.Ordinal);

	/// <inheritdoc/>
	public override bool Equals(object? obj) => obj is JSRuntimeInvocation other && Equals(other);

	/// <inheritdoc/>
	public override int GetHashCode()
	{
		var hash = default(HashCode);
		hash.Add(Identifier);
		hash.Add(CancellationToken);
		hash.Add(ResultType);
		hash.Add(InvocationMethodName);

		for (var i = 0; i < Arguments.Count; i++)
		{
			hash.Add(Arguments[i]);
		}

		return hash.ToHashCode();
	}

	/// <summary>
	/// Verify whether <paramref name="left"/> and <paramref name="right"/>
	/// <see cref="JSRuntimeInvocation"/> is equal.
	/// </summary>
	public static bool operator ==(JSRuntimeInvocation left, JSRuntimeInvocation right) => left.Equals(right);

	/// <summary>
	/// Verify whether <paramref name="left"/> and <paramref name="right"/>
	/// <see cref="JSRuntimeInvocation"/> is not equal.
	/// </summary>
	public static bool operator !=(JSRuntimeInvocation left, JSRuntimeInvocation right) => !(left == right);

	private static bool ArgumentsEqual(IReadOnlyList<object?> left, IReadOnlyList<object?> right)
	{
		if (left.Count != right.Count)
			return false;

		for (var i = 0; i < left.Count; i++)
		{
			var l = left[i];
			var r = right[i];

			if (l is null)
			{
				if (r is object)
					return false;
			}
			else
			{
				if (!l.Equals(right[i]))
					return false;
			}
		}

		return true;
	}
}
