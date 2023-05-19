using Bunit.TestDoubles;
using System;
using Xunit;

namespace Bunit.Docs.Samples;

public class InjectAuthServiceTest : TestContext
{
  [Fact(DisplayName = "Use AuthenticationStateProvider service with authenticated and authorized user")]
  public void Test001()
  {
    // arrange
    var authContext = AddAuthorization();
    authContext.SetAuthorized("TestUserName", AuthorizationState.Authorized);

    // act
    var cut = RenderComponent<InjectAuthService>();

    // assert
    Assert.Contains("<p>User: TestUserName</p>", cut.Markup, StringComparison.InvariantCulture);
  }

  [Fact(DisplayName = "Use AuthenticationStateProvider service with unauthenticated and unauthorized user")]
  public void Test002()
  {
    // arrange
    var authContext = AddAuthorization();

    // act
    var cut = RenderComponent<InjectAuthService>();

    // assert
    Assert.DoesNotContain("User:", cut.Markup, StringComparison.InvariantCulture);
  }
}