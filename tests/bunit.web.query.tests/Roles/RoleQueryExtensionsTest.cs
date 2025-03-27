namespace Bunit.Roles;

public class RoleQueryExtensionsTest : BunitContext
{
	[Fact(DisplayName = "by default logs accessible roles when it fails")]
	public async Task Test001()
	{
		var cut = Render<Wrapper>(ps => ps.AddChildContent(
			"""
			<h1>Hi</h1>		
			"""));

		Should.Throw<ElementNotFoundException>(() => cut.FindByRole(AriaRole.Article));
	}
}
