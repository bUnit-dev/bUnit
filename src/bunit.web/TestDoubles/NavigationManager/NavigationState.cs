#if NET7_0_OR_GREATER
namespace Bunit.TestDoubles;

/// <summary>
/// Describes the possible enumerations when a navigation gets intercepted.
/// </summary>
public enum NavigationState
{
	/// <summary>
	/// The navigation was successfully executed.
	/// </summary>
	Succeeded,

	/// <summary>
	/// The navigation was prevented.
	/// </summary>
	Prevented,

	/// <summary>
	/// An exception was thrown
	/// </summary>
	Failed
}
#endif
