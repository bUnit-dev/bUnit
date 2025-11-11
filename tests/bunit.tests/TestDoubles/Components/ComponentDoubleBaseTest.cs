namespace Bunit.TestDoubles.Components;

public class ComponentDoubleBaseTest : BunitContext
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

		cut.Instance.Parameters[attrName].ShouldBe(attrValue);
	}
}
