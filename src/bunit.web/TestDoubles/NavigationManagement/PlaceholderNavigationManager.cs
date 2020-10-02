using Microsoft.AspNetCore.Components;

namespace Bunit.TestDoubles.NavigationManagement
{
	/// <summary>
	/// This NavigationManager is used to provide users with helpful exceptions if they fail to provide a mock when required.
	/// </summary>
	public class PlaceholderNavigationManager : NavigationManager
	{
		protected override void NavigateToCore(string uri, bool forceLoad)
		{
			throw new MissingMockNavigationManagerException(uri, forceLoad);
		}

		protected override void EnsureInitialized()
		{
			Initialize("http://localhost:5000/", "http://localhost:5000/");
		}
	}
}
