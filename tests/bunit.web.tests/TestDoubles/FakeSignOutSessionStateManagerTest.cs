namespace Bunit.TestDoubles;

public class FakeSignOutSessionStateManagerTest : TestContext
{
	[Theory, AutoData]
	public void ShouldSignOut(string randomUserName)
	{
		this.AddTestAuthorization().SetAuthorized(randomUserName);
		var cut = RenderComponent<SignOutSessionManagerLoginDisplay>();

		cut.Find("button").Click();

		Services.GetService<FakeSignOutSessionStateManager>()!
			.IsSignedOut
			.ShouldBeTrue();
	}
}
