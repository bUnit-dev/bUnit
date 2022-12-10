using System.Text.Json;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace Bunit.TestDoubles;

using static Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers;

public class FakeNavigationManagerTest : TestContext
{
	private FakeNavigationManager CreateFakeNavigationManager()
		=> Services.GetRequiredService<FakeNavigationManager>();

	[UIFact(DisplayName = "TestContext.Services has NavigationManager registered by default as FakeNavigationManager")]
	public void Test001()
	{
		var nm = Services.GetService<NavigationManager>();
		var fnm = Services.GetService<FakeNavigationManager>();

		nm.ShouldNotBeNull();
		fnm.ShouldNotBeNull();
		nm.ShouldBe(fnm);
	}

	[UIFact(DisplayName = "FakeNavigationManager.BaseUrl is set to http://localhost/")]
	public void Test002()
	{
		var sut = CreateFakeNavigationManager();

		sut.BaseUri.ShouldBe("http://localhost/");
	}

	[UITheory(DisplayName = "NavigateTo with relative URI converts it to absolute and sets the Uri property ")]
	[InlineData("")]
	[InlineData("/")]
	[InlineData("/foo")]
	public void Test003(string uri)
	{
		var sut = CreateFakeNavigationManager();
		var expectedUri = new Uri(new Uri(sut.BaseUri, UriKind.Absolute), new Uri(uri, UriKind.Relative));

		sut.NavigateTo(uri);

		sut.Uri.ShouldBe(expectedUri.ToString());
	}

	[UITheory(DisplayName = "NavigateTo with absolute URI sets the Uri property")]
	[InlineData("http://localhost")]
	[InlineData("http://localhost/")]
	[InlineData("http://localhost/foo")]
	public void Test004(string uri)
	{
		var sut = CreateFakeNavigationManager();
		var expectedUri = new Uri(uri, UriKind.Absolute);

		sut.NavigateTo(uri);

		sut.Uri.ShouldBe(expectedUri.OriginalString);
	}

	[UIFact(DisplayName = "NavigateTo raises the NotifyLocationChanged")]
	public void Test005()
	{
		// arrange
		LocationChangedEventArgs actualLocationChange = default;
		var navigationUri = "foo";
		var sut = CreateFakeNavigationManager();
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

	[UIFact(DisplayName = "LocationChanged is raised on the test renderer's dispatcher")]
	public void Test006()
	{
		var sut = CreateFakeNavigationManager();
		var cut = RenderComponent<PrintCurrentUrl>();

		sut.NavigateTo("foo");

		cut.Find("p").MarkupMatches($"<p>{sut.BaseUri}foo</p>");
	}

	[UIFact(DisplayName = "Uri should not be unescaped")]
	public void Test007()
	{
		var sut = CreateFakeNavigationManager();

		sut.NavigateTo("/with%20whitespace");

		sut.Uri.ShouldEndWith("with%20whitespace");
	}

	[UITheory(DisplayName = "NavigateTo(uri, forceLoad, replaceHistoryEntry) is saved in history")]
	[InlineData("/uri", false, false)]
	[InlineData("/uri", true, false)]
	[InlineData("/uri", false, true)]
	public void Test200(string uri, bool forceLoad, bool replaceHistoryEntry)
	{
		var sut = CreateFakeNavigationManager();

		sut.NavigateTo(uri, forceLoad, replaceHistoryEntry);

#if NET6_0
		sut.History.ShouldHaveSingleItem()
			.ShouldBeEquivalentTo(new NavigationHistory(uri,
				new NavigationOptions { ForceLoad = forceLoad, ReplaceHistoryEntry = replaceHistoryEntry }));
#else
		var navigationOptions = new NavigationOptions { ForceLoad = forceLoad, ReplaceHistoryEntry =
 replaceHistoryEntry };
		sut.History.ShouldHaveSingleItem()
			.ShouldBeEquivalentTo(new NavigationHistory(uri, navigationOptions, NavigationState.Succeeded));
#endif
	}

	[UIFact(DisplayName = "NavigateTo with replaceHistoryEntry true replaces previous history entry")]
	public void Test201()
	{
		var sut = CreateFakeNavigationManager();

		sut.NavigateTo("/firstUrl");
		sut.NavigateTo("/secondUrl", new NavigationOptions { ReplaceHistoryEntry = true });

#if NET6_0
		sut.History.ShouldHaveSingleItem()
			.ShouldBeEquivalentTo(new NavigationHistory("/secondUrl",
				new NavigationOptions { ReplaceHistoryEntry = true }));
#else
		sut.History.ShouldHaveSingleItem()
			.ShouldBeEquivalentTo(new NavigationHistory("/secondUrl",  new NavigationOptions { ReplaceHistoryEntry =
 true }, NavigationState.Succeeded));
#endif
	}

	[UIFact(DisplayName = "Navigate to an external url should set BaseUri")]
	public void Test008()
	{
		const string externalUri = "https://bunit.dev/docs/getting-started/index.html";
		var sut = CreateFakeNavigationManager();

		sut.NavigateTo(externalUri);

		sut.BaseUri.ShouldBe("https://bunit.dev/");
		sut.Uri.ShouldBe(externalUri);
	}

	[UIFact(DisplayName = "Navigate to external url should not invoke LocationChanged event")]
	public void Test009()
	{
		var locationChangedInvoked = false;
		const string externalUri = "https://bunit.dev/docs/getting-started/index.html";
		var sut = CreateFakeNavigationManager();
		sut.LocationChanged += (s, e) => locationChangedInvoked = true;

		sut.NavigateTo(externalUri);

		locationChangedInvoked.ShouldBeFalse();
	}

#if NET7_0_OR_GREATER
	[UIFact(DisplayName = "When component provides NavigationLock, FakeNavigationManager should intercept calls")]
	public void Test010()
	{
		var fakeNavigationManager = CreateFakeNavigationManager();
		var cut = RenderComponent<InterceptNavigateToCounterComponent>();

		cut.Find("button").Click();

		cut.Instance.NavigationIntercepted.ShouldBeTrue();
		fakeNavigationManager.History.Single().State.ShouldBe(NavigationState.Prevented);
	}

	[UIFact(DisplayName = "Intercepting external url's should work")]
	public void Test011()
	{
		var fakeNavigationManager = CreateFakeNavigationManager();
		var cut = RenderComponent<GotoExternalResourceComponent>();

		cut.Find("button").Click();

		fakeNavigationManager.History.ShouldNotBeEmpty();
	}

	[UIFact(DisplayName = "Exception while intercepting is set on FakeNaviationManager")]
	public void Test012()
	{
		var fakeNavigationManager = CreateFakeNavigationManager();
		var cut = RenderComponent<ThrowsExceptionInInterceptNavigationComponent>();

		cut.Find("button").Click();

		var entry = fakeNavigationManager.History.Single();
		entry.Exception.ShouldBeOfType<NotSupportedException>();
		entry.State.ShouldBe(NavigationState.Faulted);
	}

	[UIFact(DisplayName = "StateFromJson deserialize InteractiveRequestOptions")]
	public void Test013()
	{
		var fakeNavigationManager = CreateFakeNavigationManager();
		var requestOptions = new InteractiveRequestOptions
		{
			ReturnUrl = "return", Interaction = InteractionType.SignIn,
		};
		requestOptions.TryAddAdditionalParameter("library", "bunit");

		fakeNavigationManager.NavigateToLogin("/some-url", requestOptions);

		var options = fakeNavigationManager.History.Last().StateFromJson<InteractiveRequestOptions>();
		options.ShouldNotBeNull();
		options.Interaction.ShouldBe(InteractionType.SignIn);
		options.ReturnUrl.ShouldBe("return");
		options.TryGetAdditionalParameter("library", out string libraryName).ShouldBeTrue();
		libraryName.ShouldBe("bunit");
	}

	[UIFact(DisplayName = "Given no content in state then StateFromJson throws")]
	public void Test014()
	{
		var fakeNavigationManager = CreateFakeNavigationManager();
		fakeNavigationManager.NavigateTo("/some-url");

		Should.Throw<InvalidOperationException>(
			() => fakeNavigationManager.History.Last().StateFromJson<InteractiveRequestOptions>());
	}

	[UIFact(DisplayName = "StateFromJson with invalid json throws")]
	public void Test015()
	{
		var fakeNavigationManager = CreateFakeNavigationManager();

		fakeNavigationManager.NavigateTo("/login", new NavigationOptions { HistoryEntryState = "<invalidjson>" });

		Should.Throw<JsonException>(
			() => fakeNavigationManager.History.Last().StateFromJson<InteractiveRequestOptions>());
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
#endif
}

