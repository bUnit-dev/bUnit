namespace Bunit.TestDoubles;

public partial class RouterTests : BunitContext
{
	[Fact]
	public void NavigatingToRouteWithPageAttributeShouldSetParameters()
	{
		var cut = Render<ComponentWithPageAttribute>();
		var navigationManager = cut.Services.GetRequiredService<Microsoft.AspNetCore.Components.NavigationManager>();

		navigationManager.NavigateTo("/page/1/test");

		cut.Find("p").TextContent.ShouldBe("1 / test");
	}

	[Fact]
	public void ShouldParseMultiplePageAttributes()
	{
		var cut = Render<ComponentWithMultiplePageAttributes>();
		var navigationManager = cut.Services.GetRequiredService<Microsoft.AspNetCore.Components.NavigationManager>();

		navigationManager.NavigateTo("/page/1");

		cut.Find("p").TextContent.ShouldBe("1");
	}

	[Fact]
	public void WhenParameterIsSetNavigatingShouldNotResetNonPageAttributeParameters()
	{
		var cut = Render<ComponentWithOtherParameters>(p => p.Add(ps => ps.OtherNumber, 2));
		var navigationManager = cut.Services.GetRequiredService<Microsoft.AspNetCore.Components.NavigationManager>();

		navigationManager.NavigateTo("/page/1");

		cut.Find("p").TextContent.ShouldBe("1/2");
	}

	[Fact]
	public void GivenACatchAllRouteShouldSetParameter()
	{
		var cut = Render<ComponentWithCatchAllRoute>();
		var navigationManager = cut.Services.GetRequiredService<Microsoft.AspNetCore.Components.NavigationManager>();

		navigationManager.NavigateTo("/page/1/2/3");

		cut.Find("p").TextContent.ShouldBe("1/2/3");
	}

	[Fact]
	public void ShouldInvokeParameterLifeCycleEvents()
	{
		var cut = Render<ComponentWithCustomOnParametersSetAsyncsCall>(
			p => p.Add(ps => ps.IncrementOnParametersSet, 10));
		var navigationManager = cut.Services.GetRequiredService<Microsoft.AspNetCore.Components.NavigationManager>();

		navigationManager.NavigateTo("/page/1");

		cut.Find("p").TextContent.ShouldBe("11");
	}

	[Theory]
	[InlineData("/page/1", "1")]
	[InlineData("/page", "")]
	public void ShouldSetOptionalParameter(string uri, string expectedTextContent)
	{
		var cut = Render<ComponentWithOptionalParameter>();
		var navigationManager = cut.Services.GetRequiredService<Microsoft.AspNetCore.Components.NavigationManager>();

		navigationManager.NavigateTo(uri);

		cut.Find("p").TextContent.ShouldBe(expectedTextContent);
	}

	[Fact]
	public void ComponentThatNavigatesToSelfOnClickShouldBeUpdated()
	{
		var cut = Render<ComponentThatNavigatesToSelfOnButtonClick>();

		cut.Find("button").Click();

		cut.Find("p").TextContent.ShouldBe("1");
	}

	[Fact]
	public void ComponentThatInterceptsNavigationShouldNotBeUpdated()
	{
		var cut = Render<ComponentThatNavigatesToSelfOnButtonClickIntercepted>();

		cut.Find("button").Click();

		cut.Find("p").TextContent.ShouldBe("0");
	}
}