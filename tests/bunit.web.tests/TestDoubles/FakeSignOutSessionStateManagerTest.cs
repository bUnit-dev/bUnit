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

	[UIFact]
	public void ShouldReturnSignOutStateOnValidateSignOutState()
	{
		var cut = new FakeSignOutSessionStateManager(Mock.Of<IJSRuntime>());
		cut.SetSignOutState();

		var wasValidate = cut.ValidateSignOutState().Result;

		wasValidate.ShouldBeTrue();
		cut.IsSignedOut.ShouldBeFalse();
	}
}
