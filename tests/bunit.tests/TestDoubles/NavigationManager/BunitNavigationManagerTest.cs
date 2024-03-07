using System.Text.Json;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace Bunit.TestDoubles;

using static Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers;

public class BunitNavigationManagerTest : TestContext
{
	private BunitNavigationManager CreateNavigationManager()
		=> Services.GetRequiredService<BunitNavigationManager>();

	[Fact(DisplayName = "TestContext.Services has NavigationManager registered by default as BunitNavigationManager")]
	public void Test001()
	{
		var nm = Services.GetService<NavigationManager>();
		var fnm = Services.GetService<BunitNavigationManager>();

		nm.ShouldNotBeNull();
		fnm.ShouldNotBeNull();
		nm.ShouldBe(fnm);
	}

	[Fact(DisplayName = "BunitNavigationManager.BaseUrl is set to http://localhost/")]
	public void Test002()
	{
		var sut = CreateNavigationManager();

		sut.BaseUri.ShouldBe("http://localhost/");
	}

	[Theory(DisplayName = "NavigateTo with relative URI converts it to absolute and sets the Uri property ")]
	[InlineData("")]
	[InlineData("/")]
	[InlineData("/foo")]
	[InlineData("/#Storstädning")]
	[InlineData("/#åäö")]
	public void Test003(string uri)
	{
		var sut = CreateNavigationManager();
		var expectedUri = new Uri(new Uri(sut.BaseUri, UriKind.Absolute), new Uri(uri, UriKind.Relative));

		sut.NavigateTo(uri);

		sut.Uri.ShouldBe(expectedUri.ToString());
	}

	[Theory(DisplayName = "NavigateTo with absolute URI sets the Uri property")]
	[InlineData("http://localhost")]
	[InlineData("http://localhost/")]
	[InlineData("http://localhost/foo")]
	public void Test004(string uri)
	{
		var sut = CreateNavigationManager();
		var expectedUri = new Uri(uri, UriKind.Absolute);

		sut.NavigateTo(uri);

		sut.Uri.ShouldBe(expectedUri.OriginalString);
	}

	[Fact(DisplayName = "NavigateTo raises the NotifyLocationChanged")]
	[SuppressMessage("Major Bug", "S2259:Null pointers should not be dereferenced",
		Justification = "BUG in analyzer - 'actualLocationChange' is NOT null on the execution path.")]
	public void Test005()
	{
		// arrange
		LocationChangedEventArgs actualLocationChange = default;
		var navigationUri = "foo";
		var sut = CreateNavigationManager();
		sut.LocationChanged += Sut_LocationChanged;

		// act
		sut.NavigateTo(navigationUri);

		// assert
		actualLocationChange.Location.ShouldBe($"{sut.BaseUri}{navigationUri}");
		actualLocationChange.IsNavigationIntercepted.ShouldBeFalse();

		// test helpers
		void Sut_LocationChanged(object? sender, LocationChangedEventArgs e)
		{
			actualLocationChange = e;
		}
	}

	[Fact(DisplayName = "LocationChanged is raised on the test renderer's dispatcher")]
	public void Test006()
	{
		var sut = CreateNavigationManager();
		var cut = Render<PrintCurrentUrl>();

		sut.NavigateTo("foo");

		cut.Find("p").MarkupMatches($"<p>{sut.BaseUri}foo</p>");
	}

	[Fact(DisplayName = "Uri should not be unescaped")]
	public void Test007()
	{
		var sut = CreateNavigationManager();

		sut.NavigateTo("/with%20whitespace");

		sut.Uri.ShouldEndWith("with%20whitespace");
	}

	[Theory(DisplayName = "NavigateTo(uri, forceLoad, replaceHistoryEntry) is saved in history")]
	[InlineData("/uri", false, false)]
	[InlineData("/uri", true, false)]
	[InlineData("/uri", false, true)]
	public void Test200(string uri, bool forceLoad, bool replaceHistoryEntry)
	{
		var sut = CreateNavigationManager();

		sut.NavigateTo(uri, forceLoad, replaceHistoryEntry);

		var navigationOptions = new NavigationOptions { ForceLoad = forceLoad, ReplaceHistoryEntry =
 replaceHistoryEntry };
		sut.History.ShouldHaveSingleItem()
			.ShouldBeEquivalentTo(new NavigationHistory(uri, navigationOptions, NavigationState.Succeeded));
	}

	[Fact(DisplayName = "NavigateTo with replaceHistoryEntry true replaces previous history entry")]
	public void Test201()
	{
		var sut = CreateNavigationManager();

		sut.NavigateTo("/firstUrl");
		sut.NavigateTo("/secondUrl", new NavigationOptions { ReplaceHistoryEntry = true });

		sut.History.ShouldHaveSingleItem()
			.ShouldBeEquivalentTo(new NavigationHistory("/secondUrl", new NavigationOptions
			{
				ReplaceHistoryEntry =
					true
			}, NavigationState.Succeeded));
	}

	[Fact(DisplayName = "Navigate to an external url should set BaseUri")]
	public void Test008()
	{
		const string externalUri = "https://bunit.dev/docs/getting-started/index.html";
		var sut = CreateNavigationManager();

		sut.NavigateTo(externalUri);

		sut.BaseUri.ShouldBe("https://bunit.dev/");
		sut.Uri.ShouldBe(externalUri);
	}

	[Fact(DisplayName = "Navigate to external url should not invoke LocationChanged event")]
	public void Test009()
	{
		var locationChangedInvoked = false;
		const string externalUri = "https://bunit.dev/docs/getting-started/index.html";
		var sut = CreateNavigationManager();
		sut.LocationChanged += (s, e) => locationChangedInvoked = true;

		sut.NavigateTo(externalUri);

		locationChangedInvoked.ShouldBeFalse();
	}

	[Fact(DisplayName = "When component provides NavigationLock, BunitNavigationManager should intercept calls")]
	public void Test010()
	{
		var navigationManager = CreateNavigationManager();
		var cut = Render<InterceptNavigateToCounterComponent>();

		cut.Find("button").Click();

		cut.Instance.NavigationIntercepted.ShouldBeTrue();
		navigationManager.History.Single().State.ShouldBe(NavigationState.Prevented);
	}

	[Fact(DisplayName = "Intercepting external url's should work")]
	public void Test011()
	{
		var navigationManager = CreateNavigationManager();
		var cut = Render<GotoExternalResourceComponent>();

		cut.Find("button").Click();

		navigationManager.History.ShouldNotBeEmpty();
	}

	[Fact(DisplayName = "Exception while intercepting is set on BunitNaviationManager")]
	public void Test012()
	{
		var navigationManager = CreateNavigationManager();
		var cut = Render<ThrowsExceptionInInterceptNavigationComponent>();

		cut.Find("button").Click();

		var entry = navigationManager.History.Single();
		entry.Exception.ShouldBeOfType<NotSupportedException>();
		entry.State.ShouldBe(NavigationState.Faulted);
	}

	[Fact(DisplayName = "StateFromJson deserialize InteractiveRequestOptions")]
	public void Test013()
	{
		var navigationManager = CreateNavigationManager();
		var requestOptions = new InteractiveRequestOptions
		{
			ReturnUrl = "return", Interaction = InteractionType.SignIn,
		};
		requestOptions.TryAddAdditionalParameter("library", "bunit");

		navigationManager.NavigateToLogin("/some-url", requestOptions);

		var options = navigationManager.History.Last().StateFromJson<InteractiveRequestOptions>();
		options.ShouldNotBeNull();
		options.Interaction.ShouldBe(InteractionType.SignIn);
		options.ReturnUrl.ShouldBe("return");
		options.TryGetAdditionalParameter("library", out string libraryName).ShouldBeTrue();
		libraryName.ShouldBe("bunit");
	}

	[Fact(DisplayName = "Given no content in state then StateFromJson throws")]
	public void Test014()
	{
		var navigationManager = CreateNavigationManager();
		navigationManager.NavigateTo("/some-url");

		Should.Throw<InvalidOperationException>(
			() => navigationManager.History.Last().StateFromJson<InteractiveRequestOptions>());
	}

	[Fact(DisplayName = "StateFromJson with invalid json throws")]
	public void Test015()
	{
		var navigationManager = CreateNavigationManager();

		navigationManager.NavigateTo("/login", new NavigationOptions { HistoryEntryState = "<invalidjson>" });

		Should.Throw<JsonException>(
			() => navigationManager.History.Last().StateFromJson<InteractiveRequestOptions>());
	}

	[Fact(DisplayName = "Navigate to url with state should set that state on the NavigationManager")]
	public void Test016()
	{
		const string State = "State";
		var sut = CreateNavigationManager();

		sut.NavigateTo("/internal", new NavigationOptions { HistoryEntryState = State });

		sut.HistoryEntryState.ShouldBe(State);
	}

	[Fact(DisplayName = "Navigate to url with force reload resets state")]
	public void Test017()
	{
		const string State = "State";
		var sut = CreateNavigationManager();

		sut.NavigateTo("/internal", new NavigationOptions { HistoryEntryState = State });
		sut.NavigateTo("/internal", new NavigationOptions { HistoryEntryState = State, ForceLoad = true});

		sut.HistoryEntryState.ShouldBe(null);
	}

	private sealed class InterceptNavigateToCounterComponent : ComponentBase
	{
		protected override void BuildRenderTree(RenderTreeBuilder builder)
		{
			builder.OpenElement(0, "button");
			builder.AddAttribute(1, "onclick", EventCallback.Factory.Create<MouseEventArgs>(this,
				() => NavigationManager.NavigateTo("/counter")
			));
			builder.AddContent(2, "Goto counter");
			builder.CloseElement();
			builder.AddMarkupContent(3, "\n\n");
			builder.OpenComponent<NavigationLock>(4);
			builder.AddAttribute(5, "OnBeforeInternalNavigation", TypeCheck(
				EventCallback.Factory.Create<LocationChangingContext>(this,
					InterceptNavigation
				)));
			builder.CloseComponent();
		}

		public bool NavigationIntercepted { get; set; }

		private void InterceptNavigation(LocationChangingContext context)
		{
			context.PreventNavigation();
			NavigationIntercepted = true;
		}

		[Inject] private NavigationManager NavigationManager { get; set; } = default!;
	}

	private sealed class GotoExternalResourceComponent : ComponentBase
	{
		protected override void BuildRenderTree(RenderTreeBuilder builder)
		{
			builder.OpenElement(0, "button");
			builder.AddAttribute(1, "onclick", EventCallback.Factory.Create<MouseEventArgs>(this,
				() => NavigationManager.NavigateTo("https://bunit.dev")
			));
			builder.AddContent(2, "bunit");
			builder.CloseElement();
			builder.AddMarkupContent(3, "\n");
			builder.OpenComponent<NavigationLock>(4);
			builder.AddAttribute(5, "ConfirmExternalNavigation", TypeCheck(true));
			builder.CloseComponent();
		}

		[Inject] private NavigationManager NavigationManager { get; set; } = default!;
	}

	private sealed class ThrowsExceptionInInterceptNavigationComponent : ComponentBase
	{
		protected override void BuildRenderTree(RenderTreeBuilder builder)
		{
			builder.OpenElement(0, "button");
			builder.AddAttribute(1, "onclick", EventCallback.Factory.Create<MouseEventArgs>(this,
				() => NavigationManager.NavigateTo("/counter")
			));
			builder.AddContent(2, "Goto counter");
			builder.CloseElement();
			builder.AddMarkupContent(3, "\n\n");
			builder.OpenComponent<NavigationLock>(4);
			builder.AddAttribute(5, "OnBeforeInternalNavigation", TypeCheck(
				EventCallback.Factory.Create<LocationChangingContext>(this,
					InterceptNavigation
				)));
			builder.CloseComponent();
		}

		private void InterceptNavigation(LocationChangingContext context)
		{
			throw new NotSupportedException("Don't intercept");
		}

		[Inject] private NavigationManager NavigationManager { get; set; } = default!;
	}
}

