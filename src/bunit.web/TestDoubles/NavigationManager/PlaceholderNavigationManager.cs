using Microsoft.AspNetCore.Components;

namespace Bunit.TestDoubles
{
	/// <summary>
	/// This NavigationManager is used to provide users with helpful exceptions if they fail to provide a mock when required.
	/// </summary>
	internal class PlaceholderNavigationManager : NavigationManager
	{
		/// <summary>
		/// Will throw exception to prompt user
		/// </summary>
		/// <param name="uri"></param>
		/// <param name="forceLoad"></param>
		/// <exception cref="MissingMockNavigationManagerException"></exception>
		protected override void NavigateToCore(string uri, bool forceLoad)
		{
			throw new MissingMockNavigationManagerException(uri, forceLoad);
		}

		/// <summary>
		/// Will initialize the navigation manager with a hard coded
		/// value of http://localhost:5000/
		/// </summary>
		protected override void EnsureInitialized()
		{
			Initialize("http://localhost/", "http://localhost/");
		}
	}
}
