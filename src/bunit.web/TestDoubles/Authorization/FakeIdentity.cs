using System;
using System.Security.Principal;

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
			set
			{
				if (string.IsNullOrEmpty(value))
					throw new ArgumentNullException(nameof(value));
				_authType = value;
			}
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
			set
			{
				if (string.IsNullOrEmpty(value))
					throw new ArgumentNullException(nameof(value));
				_name = value;
			}
		}
	}
}
