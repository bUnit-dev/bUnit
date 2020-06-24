using Bunit.TestDoubles.Authorization;
using Xunit;

namespace Bunit.Docs.Samples
{
  public class UserInfoTest
  {
    [Fact(DisplayName = "UserInfo with unauthenticated user")]
    public void Test001()
    {
      // Arrange
      using var ctx = new TestContext();
      ctx.Services.AddTestAuthorization();

      // Act
      var cut = ctx.RenderComponent<UserInfo>();

      // Assert
      cut.MarkupMatches(@"<p>State: Not authorized</p>
                          <h1>Please log in!</h1>");
    }

    [Fact(DisplayName = "UserInfo with authenticated but unauthorized user")]
    public void Test002()
    {
      // Arrange
      using var ctx = new TestContext();
      var authContext = ctx.Services.AddTestAuthorization();
      authContext.SetAuthorized("TEST USER", AuthorizationState.Unauthorized);

      // Act
      var cut = ctx.RenderComponent<UserInfo>();

      // Assert
      cut.MarkupMatches(@"<h1>Welcome TEST USER</h1>
                          <p>State: Not authorized</p>");
    }    

    [Fact(DisplayName = "UserInfo with authenticated and authorized user")]
    public void Test003()
    {
      // Arrange
      using var ctx = new TestContext();
      var authContext = ctx.Services.AddTestAuthorization();
      authContext.SetAuthorized("TEST USER");

      // Act
      var cut = ctx.RenderComponent<UserInfo>();

      // Assert
      cut.MarkupMatches(@"<h1>Welcome TEST USER</h1>
                          <p>State: Authorized</p>");
    } 

    [Fact(DisplayName = "UserInfo while authorizing user")]
    public void Test004()
    {
      // Arrange
      using var ctx = new TestContext();
      var authContext = ctx.Services.AddTestAuthorization();
      authContext.SetNotAuthorized();

      // Act
      var cut = ctx.RenderComponent<UserInfo>();
      authContext.SetAuthorizing();

      // Assert
      cut.MarkupMatches(@"<p>State: Authorizing</p>
                          <h1>Please log in!</h1>");
    }    
  }
}
