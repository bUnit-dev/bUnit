namespace Bunit.TestDoubles.Components;

public class ComponentDoubleBaseTest : TestContext
{
	private sealed class ComponentDouble<TComponent> : ComponentDoubleBase<TComponent>
		where TComponent : IComponent
	{ }

	[Theory(DisplayName = "Double captures unmatched parameters")]
	[AutoData]
	public void Test022(string attrName, string attrValue)
	{
		var cut = Render<ComponentDouble<AllTypesOfParams<string>>>(ps => ps
			.AddUnmatched(attrName, attrValue));

		cut.AccessInstance(c => c.Parameters[attrName].ShouldBe(attrValue));
	}
}
