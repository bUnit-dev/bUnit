namespace Bunit;

public class RenderedComponentRenderExtensionsTest : TestContext
{
	[Fact(DisplayName = "SetParametersAndRender rethrows exceptions from SetParameterAsync")]
	public async Task Test001()
	{
		var cut = await RenderComponent<ThrowsOnParameterSet>();

		var exception = Should.Throw<InvalidOperationException>(
			() => cut.SetParametersAndRender(ps => ps.Add(qt => qt.Value, "something")));
		exception.InnerException.ShouldBeOfType<ArgumentOutOfRangeException>();
	}
}
