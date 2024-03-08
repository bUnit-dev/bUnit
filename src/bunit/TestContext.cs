namespace Bunit;

/// <inheritdoc />
[Obsolete($"Use {nameof(BunitContext)} instead. BunitContext will be removed in a future release.", false, UrlFormat = "https://bunit.dev/docs/migration")]
public class TestContext : BunitContext
{
}
