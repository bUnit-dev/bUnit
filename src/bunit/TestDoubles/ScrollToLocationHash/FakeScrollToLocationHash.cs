using Microsoft.AspNetCore.Components.Routing;

namespace Bunit.TestDoubles;

internal sealed class FakeScrollToLocationHash : IScrollToLocationHash
{
	public Task RefreshScrollPositionForHash(string locationAbsolute) => Task.CompletedTask;
}
