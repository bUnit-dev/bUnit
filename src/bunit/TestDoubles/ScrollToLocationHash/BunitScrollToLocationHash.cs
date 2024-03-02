using Microsoft.AspNetCore.Components.Routing;

namespace Bunit.TestDoubles;

internal sealed class BunitScrollToLocationHash : IScrollToLocationHash
{
	public Task RefreshScrollPositionForHash(string locationAbsolute) => Task.CompletedTask;
}
