namespace Bunit.Roles;

public class RoleQueryExtensionsTest : BunitContext
{
	[Fact(DisplayName = "by default logs accessible roles when it fails")]
	public void Test001()
	{
		var cut = Render<Wrapper>(ps => ps.AddChildContent(
			"""
			<h1>Hi</h1>		
			"""));

		var exception = Should.Throw<RoleNotFoundException>(() => cut.FindByRole(AriaRole.Article));
		exception.Message.ShouldContain("Unable to find element with role 'article'");
		exception.Message.ShouldContain("Available roles: heading");
	}
}
