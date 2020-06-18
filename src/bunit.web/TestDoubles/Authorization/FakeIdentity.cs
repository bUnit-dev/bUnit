using System.Security.Principal;

namespace Bunit.TestDoubles.Authorization
{
	/// <summary>
	/// Identity class to use in tests where you specify a user identity.
	/// </summary>
	internal class FakeIdentity : IIdentity
	{
		/// <summary>
		/// Gets the test authentication type.
		/// </summary>
		public string AuthenticationType { get; set; } = "Test";

		/// <summary>
		/// Gets whether the identity is set to authenticated.
		/// </summary>
		public bool IsAuthenticated => true;

		/// <summary>
		/// Gets or sets the name of the Identity user (maps to the PrincipalUser.Name).
		/// </summary>
		public string Name { get; set; } = string.Empty;
	}
}
