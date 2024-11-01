#if NET9_0_OR_GREATER
using Bunit.TestAssets.RenderModes;

namespace Bunit.Rendering;

public class RenderModeTests : TestContext
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

	[Fact(DisplayName = "Renderer should set the RenderModeAttribute on the component")]
	public void Test003()
	{
		var cut = RenderComponent<ComponentWithServerRenderMode>();
		
		cut.MarkupMatches("<div>Assigned render mode: InteractiveServerRenderMode</div>");
	}
	
	[Fact(DisplayName = "The AssignedRenderMode is based on the RenderModeAttribute in the component hierarchy where parent component has no RenderMode")]
	public void Test004()
	{
		var cut = RenderComponent<ComponentWithoutRenderMode>(
			c => c.AddChildContent<ComponentWithWebAssemblyRenderMode>());

		cut.MarkupMatches("""
		                  <div>Parent assigned render mode: </div>
		                  <div>Assigned render mode: InteractiveWebAssemblyRenderMode</div>
		                  """);
	}

	[Fact(DisplayName = "Parent and child render mode is specified")]
	public void Test005()
	{
		var cut = RenderComponent<ComponentWithWebAssemblyRenderMode>(
			c => c.AddChildContent<ComponentWithServerRenderMode>());

		cut.MarkupMatches("""
		                  <div>Parent assigned render mode: InteractiveWebAssemblyRenderMode</div>
		                  <div>Assigned render mode: InteractiveServerRenderMode</div>
		                  """);
	}

	[Fact(DisplayName = "Parent and child render mode is not specified")]
	public void Test006()
	{		
		var cut = RenderComponent<ComponentWithoutRenderMode>(
			c => c.AddChildContent<ComponentWithoutRenderMode>());

		cut.MarkupMatches("""
		                  <div>Parent assigned render mode: </div>
		                  <div>Assigned render mode: </div>
		                  """);
		
	}
}
#endif