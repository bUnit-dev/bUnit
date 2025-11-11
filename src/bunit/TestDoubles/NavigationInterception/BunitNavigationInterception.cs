using Microsoft.AspNetCore.Components.Routing;

namespace Bunit.TestDoubles;

internal sealed class BunitNavigationInterception : INavigationInterception
{
	public Task EnableNavigationInterceptionAsync() => Task.CompletedTask;
}
