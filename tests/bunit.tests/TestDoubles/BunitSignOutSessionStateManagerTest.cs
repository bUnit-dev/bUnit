namespace Bunit.TestDoubles;

public class BunitSignOutSessionStateManagerTest : TestContext
{
	[Theory, AutoData]
	public void ShouldSignOut(string randomUserName)
	{
		AddAuthorization().SetAuthorized(randomUserName);
		var cut = RenderComponent<SignOutSessionManagerLoginDisplay>();

		cut.Find("button").Click();

		Services.GetService<BunitSignOutSessionStateManager>()!
			.IsSignedOut
			.ShouldBeTrue();
	}

	[Fact]
	public void ShouldReturnSignOutStateOnValidateSignOutState()
	{
		var cut = new BunitSignOutSessionStateManager(Mock.Of<IJSRuntime>());
		cut.SetSignOutState();

		var wasValidate = cut.ValidateSignOutState().Result;

		wasValidate.ShouldBeTrue();
		cut.IsSignedOut.ShouldBeFalse();
	}
}
