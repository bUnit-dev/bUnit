namespace Bunit;

/// <summary>
/// Represents an invocation of JavaScript via the JSRuntime Mock.
/// </summary>
public readonly record struct JSRuntimeInvocation
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
		IsVoidResultInvocation = resultType == typeof(Microsoft.JSInterop.Infrastructure.IJSVoidResult);
	}
}
