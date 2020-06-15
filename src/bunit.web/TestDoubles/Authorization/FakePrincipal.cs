using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;

namespace Bunit.TestDoubles.Authorization
{
	/// <summary>
	/// A fake Principal implementation that represents an authenticated user.
	/// </summary>
	class FakePrincipal : IPrincipal
	{
		/// <summary>
		/// Gets or sets the identity for this authenticated principal.
		/// </summary>
		public IIdentity? Identity { get; set; }

		/// <summary>
		/// Gets or sets the set of roles this user is authorized for.
		/// </summary>
		public IList<string>? Roles { get; set; }

		/// <summary>
		/// Default non-authenticated principal returns false for IsInRole check.
		/// </summary>
		/// <param name="role">Role name</param>
		/// <returns>Returns that this principal is not in any role.</returns>
		public bool IsInRole(string role)
		{
			if (this.Roles == null)
			{
				return false;
			}

			return this.Roles.Any(p => p == role);
		}
	}
}
