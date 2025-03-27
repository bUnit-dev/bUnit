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


	[Fact]
	public async Task ShouldSupportSelected()
	{
		var cut = Render<Wrapper>(ps => ps.AddChildContent(
			@"
			<select>
			  <option>Hi</option>
			  <option selected>Hello</option>
			</select>
			<div>
			  <div role=""option"" aria-selected=""true"">Hi</div>
			  <div role=""option"" aria-selected=""false"">Hello</div>
			</div>
			"));

		// Test finding options that are selected
		var selectedOptions = cut.FindAllByRole(AriaRole.Option, new ByRoleOptions { Selected = true }).ToArray();
		selectedOptions.Length.Should().Be(2);
		selectedOptions[0].MarkupMatches("<option selected>Hello</option>");
		selectedOptions[1].MarkupMatches("<div role=\"option\" aria-selected=\"true\">Hi</div>");

		// Test finding options that are not selected
		var unselectedOptions = cut.FindAllByRole(AriaRole.Option, new ByRoleOptions { Selected = false }).ToArray();
		unselectedOptions.Length.Should().Be(2);
		unselectedOptions[0].MarkupMatches("<option>Hi</option>");
		unselectedOptions[1].MarkupMatches("<div role=\"option\" aria-selected=\"false\">Hello</div>");
	}
}
