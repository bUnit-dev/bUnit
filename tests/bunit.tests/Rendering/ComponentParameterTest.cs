namespace Bunit.Rendering;

public class ComponentParameterTest
{
	[Fact(DisplayName = "Creating a cascading value with null throws")]
	public void Test001()
	{
		Should.Throw<ArgumentNullException>(() => ComponentParameter.CreateCascadingValue(null, null!));
		Should.Throw<ArgumentNullException>(() => (ComponentParameter)(null, null, true));
	}

	[Fact(DisplayName = "Creating a regular parameter without a name throws")]
	public void Test002()
	{
		Should.Throw<ArgumentNullException>(() => ComponentParameter.CreateParameter(null!, null));
		Should.Throw<ArgumentNullException>(() => (ComponentParameter)(null, null, false));
	}
}
