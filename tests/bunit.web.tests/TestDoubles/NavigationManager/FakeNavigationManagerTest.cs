using System;
using System.Diagnostics.CodeAnalysis;
using Bunit.TestAssets.SampleComponents;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit;

namespace Bunit.TestDoubles
{
	public class FakeNavigationManagerTest : TestContext
	{
		private NavigationManager CreateFakeNavigationMananger()
			=> Services.GetRequiredService<NavigationManager>();

		[Fact(DisplayName = "TestContext.Services has NavigationManager registered by default as FakeNavigationManager")]
		public void Test001()
		{
			Services.GetService<NavigationManager>()
				.ShouldBeOfType<FakeNavigationManager>();
		}

		[Fact(DisplayName = "FakeNavigationManager.BaseUrl is set to http://localhost/")]
		public void Test002()
		{
			var sut = CreateFakeNavigationMananger();

			sut.BaseUri.ShouldBe("http://localhost/");
		}

		[Theory(DisplayName = "NavigateTo with relative URI converts it to absolute and sets the Uri property ")]
		[InlineData("")]
		[InlineData("/")]
		[InlineData("/foo")]
		public void Test003(string uri)
		{
			var sut = CreateFakeNavigationMananger();
			var expectedUri = new Uri(new Uri(sut.BaseUri, UriKind.Absolute), new Uri(uri, UriKind.Relative));

			sut.NavigateTo(uri);

			sut.Uri.ShouldBe(expectedUri.ToString());
		}

		[Theory(DisplayName = "NavigateTo with absolute URI sets the Uri property ")]
		[InlineData("http://localhost")]
		[InlineData("http://localhost/")]
		[InlineData("http://localhost/foo")]
		public void Test004(string uri)
		{
			var sut = CreateFakeNavigationMananger();
			var expectedUri = new Uri(uri, UriKind.Absolute);

			sut.NavigateTo(uri);

			sut.Uri.ShouldBe(expectedUri.ToString());
		}

		[Fact(DisplayName = "NavigateTo raises the NotifyLocationChanged")]
		[SuppressMessage("Major Bug", "S2259:Null pointers should not be dereferenced", Justification = "BUG in analyzer - 'actualLocationChange' is NOT null on the execution path.")]
		public void Test005()
		{
			// arrange
			LocationChangedEventArgs actualLocationChange = default;
			var navigationUri = "foo";
			var sut = CreateFakeNavigationMananger();
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
			var sut = CreateFakeNavigationMananger();
			var cut = RenderComponent<PrintCurrentUrl>();

			sut.NavigateTo("foo");

			cut.Markup.ShouldBe($"{sut.BaseUri}foo");
		}

		[Fact(DisplayName = "Uri should not be unescaped")]
		public void Test007()
		{
			var sut = CreateFakeNavigationMananger();

			sut.NavigateTo("/with%20whitespace");

			sut.Uri.ShouldEndWith("with%20whitespace");
		}
	}
}
