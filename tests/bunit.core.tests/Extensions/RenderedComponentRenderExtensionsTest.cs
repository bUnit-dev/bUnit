namespace Bunit;

public class RenderedComponentRenderExtensionsTest : TestContext
{
	[UIFact(DisplayName = "SetParametersAndRender rethrows exceptions from SetParameterAsync")]
	public void Test001()
	{
		var cut = RenderComponent<ThrowsOnParameterSet>();

		var exception = Should.Throw<InvalidOperationException>(
			() => cut.SetParametersAndRender(ps => ps.Add(qt => qt.Value, "something")));
		exception.InnerException.ShouldBeOfType<ArgumentOutOfRangeException>();
	}
}
