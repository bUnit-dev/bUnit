#if NET9_0_OR_GREATER
using Bunit.TestAssets.RenderModes;

namespace Bunit.Rendering;

public class RendererInfoTests : TestContext
{
	[Fact(DisplayName = "TestRenderer provides RendererInfo")]
	public void Test001()
	{
		Renderer.SetRendererInfo(new RendererInfo("Server", true));
		var cut = RenderComponent<RendererInfoComponent>();

		cut.MarkupMatches("""
		                  <p>Is interactive: True</p>
		                  <p>Rendermode: Server</p>
		                  """);
	}

	[Fact(DisplayName = "Renderer throws exception if RendererInfo is not specified")]
	public void Test002()
	{
		Action act = () => RenderComponent<RendererInfoComponent>();

		act.ShouldThrow<MissingRendererInfoException>();
	}
}
#endif