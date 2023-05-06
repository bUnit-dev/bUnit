using Microsoft.AspNetCore.Components.Routing;

namespace Bunit.TestDoubles;

internal sealed class FakeNavigationInterception : INavigationInterception
{
	public Task EnableNavigationInterceptionAsync() => Task.CompletedTask;
}
