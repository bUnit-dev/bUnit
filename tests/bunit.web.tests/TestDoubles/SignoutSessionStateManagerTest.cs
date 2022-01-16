namespace Bunit.TestDoubles;

public class SignoutSessionStateManagerTest : TestContext
{
	[Fact]
	public void ShouldSignOut()
	{
		this.AddTestAuthorization().SetAuthorized("egil");
		var cut = RenderComponent<SignOutSessionManagerLoginDisplay>();

		cut.Find("button").Click();

		var wasCalled =  Services.GetService<FakeSignOutSessionStateManager>()!.SignOutStateWasCalled;
		wasCalled.ShouldBeTrue();
	}
}
