#if NET5_0_OR_GREATER
namespace Bunit;

/// <summary>
/// TODO.
/// </summary>
[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class StubAttribute : Attribute
{
	/// <summary>
	/// TODO.
	/// </summary>
	public Type TargetType { get; }

	/// <summary>
	/// TODO.
	/// </summary>
	/// <param name="targetType"></param>
	public StubAttribute(Type targetType)
	{
		TargetType = targetType;
	}
}
#endif
