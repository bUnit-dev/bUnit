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

	[Fact(DisplayName = "logs a different error if inaccessible roles should be included")]
	public async Task Test004Async()
	{
		var cut = Render<Wrapper>(ps => ps.AddChildContent(
			"""
			<div />
			"""));

		var exception = Should.Throw<RoleNotFoundException>(() => cut.FindByRole(AriaRole.Article, new() { Hidden = true }));
		await Verify(exception.Message);
	}
	
	[Fact(DisplayName = "by default excludes elements that have the html hidden attribute or any of their parents")]
	public void Test005()
	{
		var cut = Render<Wrapper>(ps => ps.AddChildContent(
			"""
			<div hidden><ul /></div>
			"""));

		Should.Throw<RoleNotFoundException>(() => cut.FindByRole(AriaRole.List));
	}

	[Fact(DisplayName = "by default excludes elements which have display: none or any of their parents")]
	public void Test006()
	{
		var cut = Render<Wrapper>(ps => ps.AddChildContent(
			"""
			<div style="display: none;"><ul style="display: block;" /></div>
			"""));

		Should.Throw<RoleNotFoundException>(() => cut.FindByRole(AriaRole.List));
	}

	[Fact(DisplayName = "by default excludes elements which have visibility hidden")]
	public void Test007()
	{
		var cut = Render<Wrapper>(ps => ps.AddChildContent(
			"""
			<div><ul style="visibility: hidden;" /></div>
			"""));

		Should.Throw<RoleNotFoundException>(() => cut.FindByRole(AriaRole.List));
	}

	[Fact(DisplayName = "by default excludes elements which have aria-hidden=\"true\" or any of their parents")]
	public void Test008()
	{
		var cut = Render<Wrapper>(ps => ps.AddChildContent(
			"""
			<div aria-hidden="true"><ul aria-hidden="false" /></div>
			"""));

		Should.Throw<RoleNotFoundException>(() => cut.FindByRole(AriaRole.List));
	}

	[Fact(DisplayName = "considers the computed visibility style not the parent")]
	public void Test009()
	{
		var cut = Render<Wrapper>(ps => ps.AddChildContent(
			"""
			<div style="visibility: hidden;"><main style="visibility: visible;"><ul /></main></div>
			"""));

		var element = cut.FindByRole(AriaRole.List);
		element.ShouldNotBeNull();
	}

	[Fact(DisplayName = "can include inaccessible roles")]
	public void Test010()
	{
		var cut = Render<Wrapper>(ps => ps.AddChildContent(
			"""
			<div hidden><ul /></div>
			"""));

		var element = cut.FindByRole(AriaRole.List, new() { Hidden = true });
		element.ShouldNotBeNull();
	}

	[Fact(DisplayName = "can be filtered by accessible name")]
	public void Test011()
	{
		var cut = Render<Wrapper>(ps => ps.AddChildContent(
			"""
			<div>
			  <h1>Order</h1>
			  <h2>Delivery Address</h2>
			  <form aria-label="Delivery Address">
				<label>
				  <div>Street</div>
				  <input type="text" />
				</label>
				<input type="submit" />
			  </form>
			  <h2>Invoice Address</h2>
			  <form aria-label="Invoice Address">
				<label>
				  <div>Street</div>
				  <input type="text" />
				</label>
				<input type="submit" />
			  </form>
			</div>
			"""));

		var deliveryForm = cut.FindByRole(AriaRole.Form, new() { Name = "Delivery Address" });
		deliveryForm.ShouldNotBeNull();

		var button = deliveryForm.QuerySelector("input[type=submit]");
		button.ShouldNotBeNull();

		var invoiceForm = cut.FindByRole(AriaRole.Form, new() { Name = "Delivery Address" });
		invoiceForm.ShouldNotBeNull();

		var textbox = invoiceForm.QuerySelector("input[type=text]");
		textbox.ShouldNotBeNull();
	}

	[Fact(DisplayName = "accessible name comparison is case sensitive")]
	public void Test012()
	{
		var cut = Render<Wrapper>(ps => ps.AddChildContent(
			"""
			<h1>Sign <em>up</em></h1>
			"""));

		Should.Throw<RoleNotFoundException>(() => 
			cut.FindByRole(AriaRole.Heading, new() { Name = "something that does not match" }));
	}

	[Fact(DisplayName = "accessible name filter implements regex matching")]
	public void Test013()
	{
		var cut = Render<Wrapper>(ps => ps.AddChildContent(
			"""
			<h1>Sign <em>up</em></h1><h2>Details</h2><h2>Your Signature</h2>
			"""));

		// Using partial match (equivalent to regex subset)
		var heading1 = cut.FindByRole(AriaRole.Heading, new() { Name = "Sign", Exact = false });
		heading1.ShouldNotBeNull();

		// Using case-insensitive match (equivalent to regex with i flag)
		var heading2 = cut.FindByRole(AriaRole.Heading, new() { Name = "sign", Exact = false });
		heading2.ShouldNotBeNull();

		// Using attributes to match specific heading (equivalent to function matcher)
		var heading3 = cut.FindByRole(AriaRole.Heading, new() { 
			Name = "Your Signature",
			Attributes = new Dictionary<string, string> { { "tagName", "H2" } }
		});
		heading3.ShouldNotBeNull();
	}

	[Fact(DisplayName = "does not include the container in the queryable roles")]
	public void Test014()
	{
		var cut = Render<Wrapper>(ps => ps.AddChildContent("<li role='listitem' />"));

		Should.Throw<RoleNotFoundException>(() => cut.FindByRole(AriaRole.List));
		var listItem = cut.FindByRole(AriaRole.Listitem);
		listItem.ShouldNotBeNull();
	}

	[Fact(DisplayName = "explicit role is most specific")]
	public void Test015()
	{
		var cut = Render<Wrapper>(ps => ps.AddChildContent(
			"""
			<button role="tab" aria-label="my-tab" />
			"""));

		Should.Throw<RoleNotFoundException>(() => cut.FindByRole(AriaRole.Button));
	}

	[Fact(DisplayName = "should find the input using type property instead of attribute")]
	public void Test016()
	{
		var cut = Render<Wrapper>(ps => ps.AddChildContent(
			"""
			<input type="124">
			"""));

		var element = cut.FindByRole(AriaRole.TextBox);
		element.ShouldNotBeNull();
	}

	[Fact(DisplayName = "can be filtered by accessible description")]
	public void Test017()
	{
		var targetedNotificationMessage = "Your session is about to expire!";
		var cut = Render<Wrapper>(ps => ps.AddChildContent(
			$"""
			<ul>
			  <li role="alertdialog" aria-describedby="notification-id-1">
				<div><button>Close</button></div>
				<div id="notification-id-1">You have unread emails</div>
			  </li>
			  <li role="alertdialog" aria-describedby="notification-id-2">
				<div><button>Close</button></div>
				<div id="notification-id-2">{targetedNotificationMessage}</div>
			  </li>
			</ul>
			"""));

		var notification = cut.FindByRole(AriaRole.AlertDialog, new() {
			Description = targetedNotificationMessage
		});

		notification.ShouldNotBeNull();
		notification.TextContent.ShouldContain(targetedNotificationMessage);

		var button = notification.QuerySelector("button");
		button.ShouldNotBeNull();
		button.TextContent.ShouldBe("Close");
	}
}
