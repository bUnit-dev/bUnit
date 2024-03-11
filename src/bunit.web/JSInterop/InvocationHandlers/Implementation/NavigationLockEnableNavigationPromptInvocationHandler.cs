namespace Bunit.JSInterop.InvocationHandlers.Implementation;

internal sealed class NavigationLockEnableNavigationPromptInvocationHandler : JSRuntimeInvocationHandler
{
	private const string Identifier = "Blazor._internal.NavigationLock.enableNavigationPrompt";

	internal NavigationLockEnableNavigationPromptInvocationHandler()
		: base(inv => inv.Identifier.Equals(Identifier, StringComparison.Ordinal), isCatchAllHandler: true)
	{
		SetVoidResult();
	}
}
