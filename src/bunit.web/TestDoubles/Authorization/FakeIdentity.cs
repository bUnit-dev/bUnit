using System.Security.Principal;
using Bunit.Extensions;

namespace Bunit.TestDoubles.Authorization
{
	/// <summary>
	/// Identity class to use in tests where you specify a user identity.
	/// </summary>
	internal class FakeIdentity : IIdentity
	{
		private string _authType = "Test";
		private string _name = string.Empty;

		/// <summary>
		/// Gets the test authentication type.
		/// </summary>
		public string AuthenticationType
		{
			get => _authType;
			set => _authType = value.VerifyRequiredValue();
		}

		/// <summary>
		/// Gets whether the identity is set to authenticated.
		/// </summary>
		public bool IsAuthenticated => true;

		/// <summary>
		/// Gets or sets the name of the Identity user (maps to the PrincipalUser.Name).
		/// </summary>
		public string Name
		{
			get => _name;
			set => _name = value.VerifyRequiredValue();
		}
	}
}
