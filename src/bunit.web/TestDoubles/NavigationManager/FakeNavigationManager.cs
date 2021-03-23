using System;
using System.Diagnostics.CodeAnalysis;
using Bunit.Rendering;
using Microsoft.AspNetCore.Components;

namespace Bunit.TestDoubles
{
	/// <summary>
	/// Represents a fake <see cref="NavigationManager"/> that captures calls to
	/// <see cref="NavigationManager.NavigateTo(string, bool)"/> for testing purposes.
	/// </summary>
	public sealed class FakeNavigationManager : NavigationManager
	{
		private readonly ITestRenderer renderer;

		/// <summary>
		/// Initializes a new instance of the <see cref="FakeNavigationManager"/> class.
		/// </summary>
		[SuppressMessage("Minor Code Smell", "S1075:URIs should not be hardcoded", Justification = "By design. Fake navigation manager defaults to local host as base URI.")]
		public FakeNavigationManager(ITestRenderer renderer)
		{
			this.renderer = renderer ?? throw new ArgumentNullException(nameof(renderer));
			Initialize("http://localhost/", "http://localhost/");
		}

		/// <inheritdoc/>
		protected override void NavigateToCore(string uri, bool forceLoad)
		{
			Uri = ToAbsoluteUri(uri).ToString();

			renderer.Dispatcher.InvokeAsync(
				() => NotifyLocationChanged(isInterceptedLink: false));
		}
	}
}
