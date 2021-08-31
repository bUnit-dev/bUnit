namespace Bunit
{
	/// <summary>
	/// Represents a invocation matcher / predicate, that is used to determine
	/// if a <see cref="JSRuntimeInvocationHandler{TResult}"/> matches a specific
	/// <see cref="JSRuntimeInvocation"/>.
	/// </summary>
	/// <param name="invocation">The invocation to match against.</param>
	/// <returns>True if the <see cref="JSRuntimeInvocationHandler{TResult}"/> can handle the invocation, false otherwise.</returns>
	public delegate bool InvocationMatcher(JSRuntimeInvocation invocation);
}
