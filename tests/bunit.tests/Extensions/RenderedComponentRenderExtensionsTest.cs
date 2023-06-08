namespace Bunit;

public class RenderedComponentRenderExtensionsTest : TestContext
{
	[Fact(DisplayName = "SetParametersAndRender rethrows exceptions from SetParameterAsync")]
	public void Test001()
	{
		var cut = Render<ThrowsOnParameterSet>();

		var exception = Should.Throw<InvalidOperationException>(
			() => cut.Render(ps => ps.Add(qt => qt.Value, "something")));
		exception.InnerException.ShouldBeOfType<ArgumentOutOfRangeException>();
	}
}
