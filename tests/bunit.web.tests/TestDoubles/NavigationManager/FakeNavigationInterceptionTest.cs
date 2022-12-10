namespace Bunit.TestDoubles;

public class FakeNavigationInterceptionTest
{
	[UIFact(DisplayName = "EnableNavigationInterceptionAsync returns completed task")]
	public void Test001()
	{
		new FakeNavigationInterception()
			.EnableNavigationInterceptionAsync()
			.IsCompletedSuccessfully
			.ShouldBeTrue();
	}

	[UIFact(DisplayName = "FakeNavigationInterception is registered as the default INavigationInterception")]
	public void Test002()
	{
		using var ctx = new TestContext();
		ctx.Services.GetService<INavigationInterception>()
			.ShouldBeOfType<FakeNavigationInterception>();
	}
}
