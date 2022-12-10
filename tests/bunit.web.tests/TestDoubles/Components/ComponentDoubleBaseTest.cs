namespace Bunit.TestDoubles.Components;

public class ComponentDoubleBaseTest : TestContext
{
	private sealed class ComponentDouble<TComponent> : ComponentDoubleBase<TComponent>
		where TComponent : IComponent
	{ }

	[UITheory(DisplayName = "Double captures unmatched parameters")]
	[AutoData]
	public void Test022(string attrName, string attrValue)
	{
		var cut = RenderComponent<ComponentDouble<AllTypesOfParams<string>>>((attrName, attrValue));

		cut.Instance.Parameters[attrName].ShouldBe(attrValue);
	}
}
