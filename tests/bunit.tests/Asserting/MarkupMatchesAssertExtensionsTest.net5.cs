namespace Bunit.Asserting;

public partial class MarkupMatchesAssertExtensionsTest : TestContext
{
	[Fact(DisplayName = "MarkupMatches correctly ignores scoped CSS attributes")]
	public void Test_net5_001()
	{
		var cut = RenderComponent<ScopedCssElements>();

		cut.MarkupMatches("<h1>Hello Pink World!</h1>");
	}
}
