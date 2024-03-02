using Microsoft.Extensions.Logging.Abstractions;

namespace Bunit.Rendering;

public partial class TestRendererTest : TestContext
{
	[Fact(DisplayName = "given a IComponentActivator, " +
						"when passed to constructor," +
						"then it used to create components")]
	public void Test1000()
	{
		var activatorMock = Substitute.For<IComponentActivator>();
		activatorMock.CreateInstance(typeof(Wrapper)).Returns(new Wrapper());
		using var renderer = new TestRenderer(
			Services.GetService<IRenderedComponentActivator>(),
			Services,
			NullLoggerFactory.Instance,
			activatorMock);

		renderer.RenderComponent<Wrapper>(new ComponentParameterCollection());

		activatorMock.Received(1).CreateInstance(typeof(Wrapper));
	}
}
