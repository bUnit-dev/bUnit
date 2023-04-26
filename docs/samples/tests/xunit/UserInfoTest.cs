using Bunit.TestDoubles;
using Xunit;

namespace Bunit.Docs.Samples;

public class UserInfoTest : TestContext
{
  [Fact(DisplayName = "UserInfo with unauthenticated user")]
  public void Test001()
  {
    // Arrange
    this.AddTestAuthorization();

    // Act
    var cut = RenderComponent<UserInfo>();

    // Assert
    cut.MarkupMatches(@"<h1>Please log in!</h1>
                          <p>State: Not authorized</p>");
  }

  [Fact(DisplayName = "UserInfo while authorizing user")]
  public void Test004()
  {
    // Arrange
    var authContext = this.AddTestAuthorization();
    authContext.SetAuthorizing();

    // Act
    var cut = RenderComponent<UserInfo>();

    // Assert
    cut.MarkupMatches(@"<h1>Please log in!</h1>
                          <p>State: Authorizing</p>");
  }

  [Fact(DisplayName = "UserInfo with authenticated but unauthorized user")]
  public void Test002()
  {
    // Arrange
    var authContext = this.AddTestAuthorization();
    authContext.SetAuthorized("TEST USER", AuthorizationState.Unauthorized);

    // Act
    var cut = RenderComponent<UserInfo>();

    // Assert
    cut.MarkupMatches(@"<h1>Welcome TEST USER</h1>
                          <p>State: Not authorized</p>");
  }

  [Fact(DisplayName = "UserInfo with authenticated and authorized user")]
  public void Test003()
  {
    // Arrange
    var authContext = this.AddTestAuthorization();
    authContext.SetAuthorized("TEST USER");

    // Act
    var cut = RenderComponent<UserInfo>();

    // Assert
    cut.MarkupMatches(@"<h1>Welcome TEST USER</h1>
                          <p>State: Authorized</p>");
  }
}