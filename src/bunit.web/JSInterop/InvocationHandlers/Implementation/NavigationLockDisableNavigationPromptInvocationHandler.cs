#if NET7_0_OR_GREATER
namespace Bunit.JSInterop.InvocationHandlers.Implementation;

internal sealed class NavigationLockDisableNavigationPromptInvocationHandler : JSRuntimeInvocationHandler
{
	private const string Identifier = "Blazor._internal.NavigationLock.disableNavigationPrompt";

	internal NavigationLockDisableNavigationPromptInvocationHandler()
		: base(inv => inv.Identifier.Equals(Identifier, StringComparison.Ordinal), isCatchAllHandler: true)
	{
		SetVoidResult();
	}
}
#endif
