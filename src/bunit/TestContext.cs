namespace Bunit;

/// <inheritdoc />
[Obsolete($"Use {nameof(BunitContext)} instead. TestContext will be removed in a future release.", false, UrlFormat = "https://bunit.dev/docs/migrations")]
[RemovedInFutureVersion("Obsolete in v2, removed in future version")]
public class TestContext : BunitContext
{
}
