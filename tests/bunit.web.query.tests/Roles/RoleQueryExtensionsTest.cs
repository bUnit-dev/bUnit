
namespace Bunit.Roles;

public class RoleQueryExtensionsTest : BunitContext
{
	[Fact(DisplayName = "by default logs accessible roles when it fails")]
	public async Task Test001Async()
	{
		var cut = Render<Wrapper>(ps => ps.AddChildContent(
			"""
			<h1>Hi</h1>		
			"""));

		var exception = Should.Throw<RoleNotFoundException>(() => cut.FindByRole(AriaRole.Article));
		await Verify(exception.Message);
	}

	[Fact(DisplayName = "when hidden: true logs available roles when it fails")]
	public async Task Test002Async()
	{
		var cut = Render<Wrapper>(ps => ps.AddChildContent(
			"""
			<div hidden>
				<h1>Hi</h1>
			</div>
			"""));

		var exception = Should.Throw<RoleNotFoundException>(() => cut.FindByRole(AriaRole.Article, new() { Hidden = true }));
		await Verify(exception.Message);
	}

	[Fact(DisplayName = "logs error when there are no accessible roles")]
	public async Task Test003Async()
	{
		var cut = Render<Wrapper>(ps => ps.AddChildContent(
			"""
			<div />
			"""));

		var exception = Should.Throw<RoleNotFoundException>(() => cut.FindByRole(AriaRole.Article));
		await Verify(exception.Message);
	}
}
