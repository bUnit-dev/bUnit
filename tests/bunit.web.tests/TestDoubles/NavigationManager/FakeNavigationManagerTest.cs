namespace Bunit.TestDoubles
{
	public class FakeNavigationManagerTest : TestContext
	{
		private FakeNavigationManager CreateFakeNavigationManager()
			=> Services.GetRequiredService<FakeNavigationManager>();

		[Fact(DisplayName = "TestContext.Services has NavigationManager registered by default as FakeNavigationManager")]
		public void Test001()
		{
			var nm = Services.GetService<NavigationManager>();
			var fnm = Services.GetService<FakeNavigationManager>();

			nm.ShouldNotBeNull();
			fnm.ShouldNotBeNull();
			nm.ShouldBe(fnm);
		}

		[Fact(DisplayName = "FakeNavigationManager.BaseUrl is set to http://localhost/")]
		public void Test002()
		{
			var sut = CreateFakeNavigationManager();

			sut.BaseUri.ShouldBe("http://localhost/");
		}

		[Theory(DisplayName = "NavigateTo with relative URI converts it to absolute and sets the Uri property ")]
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

		[Theory(DisplayName = "NavigateTo with absolute URI sets the Uri property")]
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

		[Fact(DisplayName = "NavigateTo raises the NotifyLocationChanged")]
		[SuppressMessage("Major Bug", "S2259:Null pointers should not be dereferenced", Justification = "BUG in analyzer - 'actualLocationChange' is NOT null on the execution path.")]
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

		[Fact(DisplayName = "LocationChanged is raised on the test renderer's dispatcher")]
		public void Test006()
		{
			var sut = CreateFakeNavigationManager();
			var cut = RenderComponent<PrintCurrentUrl>();

			sut.NavigateTo("foo");

			cut.Find("p").MarkupMatches($"<p>{sut.BaseUri}foo</p>");
		}

		[Fact(DisplayName = "Uri should not be unescaped")]
		public void Test007()
		{
			var sut = CreateFakeNavigationManager();

			sut.NavigateTo("/with%20whitespace");

			sut.Uri.ShouldEndWith("with%20whitespace");
		}

#if !NET6_0_OR_GREATER
		[Theory(DisplayName = "NavigateTo(uri, forceLoad) is saved in history")]
		[InlineData("/uri", false)]
		[InlineData("/anotherUri", true)]
		public void Test100(string uri, bool forceLoad)
		{
			var sut = CreateFakeNavigationManager();

			sut.NavigateTo(uri, forceLoad);

			sut.History.ShouldHaveSingleItem()
				.ShouldBeEquivalentTo(new NavigationHistory(uri, new NavigationOptions(forceLoad)));
		}
#endif
#if NET6_0_OR_GREATER
		[Theory(DisplayName = "NavigateTo(uri, forceLoad, replaceHistoryEntry) is saved in history")]
		[InlineData("/uri", false, false)]
		[InlineData("/uri", true, false)]
		[InlineData("/uri", false, true)]
		public void Test200(string uri, bool forceLoad, bool replaceHistoryEntry)
		{
			var sut = CreateFakeNavigationManager();

			sut.NavigateTo(uri, forceLoad, replaceHistoryEntry);

			sut.History.ShouldHaveSingleItem()
				.ShouldBeEquivalentTo(new NavigationHistory(uri, new NavigationOptions { ForceLoad = forceLoad, ReplaceHistoryEntry = replaceHistoryEntry }));
		}

		[Fact(DisplayName = "NavigateTo with replaceHistoryEntry true replaces previous history entry")]
		public void Test201()
		{
			var sut = CreateFakeNavigationManager();

			sut.NavigateTo("/firstUrl");
			sut.NavigateTo("/secondUrl", new NavigationOptions { ReplaceHistoryEntry = true });

			sut.History.ShouldHaveSingleItem()
				.ShouldBeEquivalentTo(new NavigationHistory("/secondUrl", new NavigationOptions { ReplaceHistoryEntry = true }));
		}
#endif

		[Fact(DisplayName = "Navigate to an external url should set BaseUri")]
		public void Test008()
		{
			const string externalUri = "https://bunit.dev/docs/getting-started/index.html";
			var sut = CreateFakeNavigationManager();

			sut.NavigateTo(externalUri);

			sut.BaseUri.ShouldBe("https://bunit.dev/");
			sut.Uri.ShouldBe(externalUri);
		}

		[Fact(DisplayName = "Navigate to external url should not invoke LocationChanged event")]
		public void Test009()
		{
			var locationChangedInvoked = false;
			const string externalUri = "https://bunit.dev/docs/getting-started/index.html";
			var sut = CreateFakeNavigationManager();
			sut.LocationChanged += (s, e) => locationChangedInvoked = true;

			sut.NavigateTo(externalUri);

			locationChangedInvoked.ShouldBeFalse();
		}
	}

	[Fact(DisplayName = "LocationChanged is raised on the test renderer's dispatcher")]
	public void Test006()
	{
		var sut = CreateFakeNavigationManager();
		var cut = RenderComponent<PrintCurrentUrl>();

		sut.NavigateTo("foo");

		cut.Find("p").MarkupMatches($"<p>{sut.BaseUri}foo</p>");
	}

	[Fact(DisplayName = "Uri should not be unescaped")]
	public void Test007()
	{
		var sut = CreateFakeNavigationManager();

		sut.NavigateTo("/with%20whitespace");

		sut.Uri.ShouldEndWith("with%20whitespace");
	}

	[Theory(DisplayName = "NavigateTo(uri, forceLoad, replaceHistoryEntry) is saved in history")]
	[InlineData("/uri", false, false)]
	[InlineData("/uri", true, false)]
	[InlineData("/uri", false, true)]
	public void Test200(string uri, bool forceLoad, bool replaceHistoryEntry)
	{
		var sut = CreateFakeNavigationManager();

		sut.NavigateTo(uri, forceLoad, replaceHistoryEntry);

		sut.History.ShouldHaveSingleItem()
			.ShouldBeEquivalentTo(new NavigationHistory(uri, new NavigationOptions() { ForceLoad = forceLoad, ReplaceHistoryEntry = replaceHistoryEntry }));
	}

	[Fact(DisplayName = "NavigateTo with replaceHistoryEntry true replaces previous history entry")]
	public void Test201()
	{
		var sut = CreateFakeNavigationManager();

		sut.NavigateTo("/firstUrl");
		sut.NavigateTo("/secondUrl", new NavigationOptions { ReplaceHistoryEntry = true });

		sut.History.ShouldHaveSingleItem()
			.ShouldBeEquivalentTo(new NavigationHistory("/secondUrl", new NavigationOptions() { ReplaceHistoryEntry = true }));
	}

	[Fact(DisplayName = "Navigate to an external url should set BaseUri")]
	public void Test008()
	{
		const string externalUri = "https://bunit.dev/docs/getting-started/index.html";
		var sut = CreateFakeNavigationManager();

		sut.NavigateTo(externalUri);

		sut.BaseUri.ShouldBe("https://bunit.dev/");
		sut.Uri.ShouldBe(externalUri);
	}

	[Fact(DisplayName = "Navigate to external url should not invoke LocationChanged event")]
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
	[Fact(DisplayName = "When component provides NavigationLock, FakeNavigationManager should intercept calls")]
	public void Test010()
	{
		var fakeNavigationManager = CreateFakeNavigationManager();
		var cut = RenderComponent<InterceptNavigateToCounterComponent>();

		cut.Find("button").Click();

		cut.Instance.NavigationIntercepted.ShouldBeTrue();
		cut.WaitForAssertion(() => fakeNavigationManager.History.ShouldBeEmpty());
	}

	[Fact(DisplayName = "Intercepting external url's should work")]
	public void Test011()
	{
		var fakeNavigationManager = CreateFakeNavigationManager();
		var cut = RenderComponent<GotoExternalResourceComponent>();

		cut.Find("button").Click();

		fakeNavigationManager.History.ShouldNotBeEmpty();
	}

	private class InterceptNavigateToCounterComponent : ComponentBase
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
			builder.AddAttribute(5, "OnBeforeInternalNavigation", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck(EventCallback.Factory.Create<LocationChangingContext>(this,
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

	public class GotoExternalResourceComponent : ComponentBase
	{
#pragma warning disable 1998
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
			builder.AddAttribute(5, "ConfirmExternalNavigation", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<Boolean>(
				true
			));
			builder.CloseComponent();
		}

		[Inject] private NavigationManager NavigationManager { get; set; } = default!;
	}
#endif
}
