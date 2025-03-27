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
		exception.Message.ShouldContain("heading");
	}

	[Fact(DisplayName = "when hidden: true logs available roles when it fails")]
	public void Test002()
	{
		var cut = Render<Wrapper>(ps => ps.AddChildContent(
			"""
			<div hidden>
				<h1>Hi</h1>
			</div>
			"""));

		var exception = Should.Throw<RoleNotFoundException>(() => cut.FindByRole(AriaRole.Article, new() { Hidden = true }));
		exception.Message.ShouldContain("Unable to find element with role 'article'");
		exception.Message.ShouldContain("heading");
		exception.Message.ShouldContain("Name \"Hi\":");
		exception.Message.ShouldContain("<h1 />");
		exception.Message.ShouldContain("Ignored nodes: comments, script, style");
	}
}
