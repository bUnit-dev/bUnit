namespace Bunit.Roles;

public class RoleQueryExtensionsTest : BunitContext
{
	[Fact]
	public async Task ShouldDetectRoles()
	{
		var cut = Render<Wrapper>(ps => ps.AddChildContent(
			@"
          <button>Hello</button>
          <select multiple="""" size=""2""></select>
          <select></select>
          <h3>Heading</h3>
          <details><summary>Hello</summary></details>
          <div role=""dialog"">I am a dialog</div>
        "));

		cut.FindByRole(AriaRole.Button).MarkupMatches("<button>Hello</button>");
		cut.FindByRole(AriaRole.Listbox).MarkupMatches("<select multiple size=\"2\"></select>");
		cut.FindByRole(AriaRole.Combobox).MarkupMatches("<select></select>");
		cut.FindByRole(AriaRole.Heading).MarkupMatches("<h3>Heading</h3>");
		cut.FindByRole(AriaRole.Group).MarkupMatches("<details><summary>Hello</summary></details>");
		cut.FindByRole(AriaRole.Dialog).MarkupMatches("<div role=\"dialog\">I am a dialog</div>");
		Should.Throw<InvalidOperationException>(() => cut.FindByRole(AriaRole.Menuitem));
	}
}
