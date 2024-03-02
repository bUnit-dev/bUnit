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

	[Fact]
	public async Task ShouldReturnSignOutStateOnValidateSignOutState()
	{
		var cut = new FakeSignOutSessionStateManager(Substitute.For<IJSRuntime>());
		await cut.SetSignOutState();

		var wasValidate = await cut.ValidateSignOutState();

		wasValidate.ShouldBeTrue();
		cut.IsSignedOut.ShouldBeFalse();
	}
}
