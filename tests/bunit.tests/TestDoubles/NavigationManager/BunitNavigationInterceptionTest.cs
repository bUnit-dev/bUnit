namespace Bunit.TestDoubles;

public class BunitNavigationInterceptionTest
{
	[Fact(DisplayName = "EnableNavigationInterceptionAsync returns completed task")]
	public void Test001()
	{
		new BunitNavigationInterception()
			.EnableNavigationInterceptionAsync()
			.IsCompletedSuccessfully
			.ShouldBeTrue();
	}

	[Fact(DisplayName = "FakeNavigationInterception is registered as the default INavigationInterception")]
	public void Test002()
	{
		using var ctx = new BunitContext();
		ctx.Services.GetService<INavigationInterception>()
			.ShouldBeOfType<BunitNavigationInterception>();
	}
}
