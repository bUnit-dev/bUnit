using Bunit.TestDoubles;
using System.Security.Claims;
using System.Globalization;
using Xunit;

namespace Bunit.Docs.Samples;

public class UserRightsTest : BunitContext
{
  [Fact(DisplayName = "No roles or policies")]
  public void Test001()
  {
    // Arrange
    var authContext = AddAuthorization();
    authContext.SetAuthorized("TEST USER");

    // Act
    var cut = Render<UserRights>();

    // Assert
    cut.MarkupMatches(@"<h1>Hi TEST USER, you have these claims and rights:</h1>
                          <ul></ul>");
  }

  [Fact(DisplayName = "Superuser role")]
  public void Test002()
  {
    // Arrange
    var authContext = AddAuthorization();
    authContext.SetAuthorized("TEST USER");
    authContext.SetRoles("superuser");

    // Act
    var cut = Render<UserRights>();

    // Assert
    cut.MarkupMatches(@"<h1>Hi TEST USER, you have these claims and rights:</h1>
                          <ul>
                            <li>You have the role SUPER USER</li>
                          </ul>");
  }

  [Fact(DisplayName = "Superuser and admin role")]
  public void Test003()
  {
    // Arrange
    var authContext = AddAuthorization();
    authContext.SetAuthorized("TEST USER");
    authContext.SetRoles("admin", "superuser");

    // Act
    var cut = Render<UserRights>();

    // Assert
    cut.MarkupMatches(@"<h1>Hi TEST USER, you have these claims and rights:</h1>
                          <ul>
                            <li>You have the role SUPER USER</li>
                            <li>You have the role ADMIN</li>
                          </ul>");
  }

  [Fact(DisplayName = "content-editor policy")]
  public void Test004()
  {
    // Arrange
    var authContext = AddAuthorization();
    authContext.SetAuthorized("TEST USER");
    authContext.SetPolicies("content-editor");

    // Act
    var cut = Render<UserRights>();

    // Assert
    cut.MarkupMatches(@"<h1>Hi TEST USER, you have these claims and rights:</h1>
                          <ul>
                            <li>You are a CONTENT EDITOR</li>
                          </ul>");
  }

  [Fact(DisplayName = "multiple content-editor policy")]
  public void Test0041()
  {
    // Arrange
    var authContext = AddAuthorization();
    authContext.SetAuthorized("TEST USER");
    authContext.SetPolicies("content-editor", "approver");

    // Act
    var cut = Render<UserRights>();

    // Assert
    cut.MarkupMatches(@"<h1>Hi TEST USER, you have these claims and rights:</h1>
                          <ul>
                            <li>You are a CONTENT EDITOR</li>
                          </ul>");
  }

  [Fact(DisplayName = "multiple claims")]
  public void Test006()
  {
    // Arrange
    var authContext = AddAuthorization();
    authContext.SetAuthorized("TEST USER");
    authContext.SetClaims(
      new Claim(ClaimTypes.Email, "test@example.com"),
      new Claim(ClaimTypes.DateOfBirth, "01-01-1970")
    );

    // Act
    var cut = Render<UserRights>();

    // Assert
    cut.MarkupMatches(@"<h1>Hi TEST USER, you have these claims and rights:</h1>
                          <ul>
                            <li>Emailaddress: test@example.com</li>
                            <li>Dateofbirth: 01-01-1970</li>
                          </ul>");
  }

  [Fact(DisplayName = "All roles and policies")]
  public void Test005()
  {
    // Arrange
    var authContext = AddAuthorization();
    authContext.SetAuthorized("TEST USER");
    authContext.SetRoles("admin", "superuser");
    authContext.SetPolicies("content-editor");
    authContext.SetClaims(new Claim(ClaimTypes.Email, "test@example.com"));

    // Act
    var cut = Render<UserRights>();

    // Assert
    cut.MarkupMatches(@"<h1>Hi TEST USER, you have these claims and rights:</h1>
                          <ul>
                            <li>Emailaddress: test@example.com</li>
                            <li>You have the role SUPER USER</li>
                            <li>You have the role ADMIN</li>
                            <li>You are a CONTENT EDITOR</li>
                          </ul>");
  }

  [Fact(DisplayName = "Custom authentication type")]
  public void Test007()
  {
    // Arrange
    var authContext = AddAuthorization();
    authContext.SetAuthorized("TEST USER");
    authContext.SetAuthenticationType("custom-auth-type");

    // Act
    var cut = Render<UserRights>();

    // Assert
    cut.MarkupMatches(@"<h1>Hi TEST USER, you have these claims and rights:</h1>
                          <ul>
                            <li>You have the authentication type CUSTOM AUTH TYPE</li>
                          </ul>");
  }
}