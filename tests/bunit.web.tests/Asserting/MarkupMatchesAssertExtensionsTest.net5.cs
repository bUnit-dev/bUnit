#if !NETCOREAPP3_1

using Bunit.TestAssets.SampleComponents;
using Xunit;

namespace Bunit.Asserting;

public partial class MarkupMatchesAssertExtensionsTest : TestContext
{
	[Fact(DisplayName = "MarkupMatches correctly ignores scoped CSS attributes")]
	public async Task Test_net5_001()
	{
		var cut = await RenderComponent<ScopedCssElements>();

		cut.MarkupMatches("<h1>Hello Pink World!</h1>");
	}
}
#endif
