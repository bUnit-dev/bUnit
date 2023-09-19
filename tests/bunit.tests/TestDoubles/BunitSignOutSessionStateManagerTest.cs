namespace Bunit.TestDoubles;

public class BunitSignOutSessionStateManagerTest : TestContext
{
	[Theory, AutoData]
	public void ShouldSignOut(string randomUserName)
	{
		AddAuthorization().SetAuthorized(randomUserName);
		var cut = Render<SignOutSessionManagerLoginDisplay>();

		cut.Find("button").Click();

		Services.GetService<BunitSignOutSessionStateManager>()!
			.IsSignedOut
			.ShouldBeTrue();
	}

	[Fact]
	public async Task ShouldReturnSignOutStateOnValidateSignOutState()
	{
		var cut = new BunitSignOutSessionStateManager(Substitute.For<IJSRuntime>());
		await cut.SetSignOutState();

		var wasValidate = await cut.ValidateSignOutState();

		wasValidate.ShouldBeTrue();
		cut.IsSignedOut.ShouldBeFalse();
	}
}
