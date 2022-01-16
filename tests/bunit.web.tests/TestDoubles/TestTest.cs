namespace Bunit.TestDoubles;

public class TEstTest : TestContext
{
	[Fact]
	public void ShouldDo()
	{
		this.AddTestAuthorization().SetAuthorized("egil");
		var cut = RenderComponent<SignOutSessionManagerLoginDisplay>();

		cut.Find("button").Click();

		var wasCalled =  Services.GetService<FakeSignOutSessionStateManager>()!.SignOutStateWasCalled;
		wasCalled.ShouldBeTrue();
	}
}
