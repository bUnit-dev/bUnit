using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;

namespace Bunit.TestDoubles.Authorization
{
	/// <summary>
	/// A fake Principal implementation that represents an authenticated user.
	/// </summary>
	public class FakePrincipal : IPrincipal
	{
		/// <summary>
		/// Gets or sets the identity for this authenticated principal.
		/// </summary>
		public IIdentity? Identity { get; set; }

		/// <summary>
		/// Gets or sets the set of roles this user is authorized for.
		/// </summary>
		public IEnumerable<string>? Roles { get; set; }

		/// <summary>
		/// Default non-authenticated principal returns false for IsInRole check.
		/// </summary>
		/// <param name="role">Role name</param>
		/// <returns>Returns that this principal is not in any role.</returns>
		public bool IsInRole(string role)
		{
			if (Roles == null)
			{
				return false;
			}

			return Roles.Any(p => p == role);
		}

		/// <summary>
		/// Factory method to create a ClaimsPrincipal from a FakePrincipal and its data.
		/// </summary>
		/// <param name="userName">User name for principal identity.</param>
		/// <param name="roles">Roles for the user.</param>
		/// <returns>ClaimsPrincipal created from this data.</returns>
		public static ClaimsPrincipal CreatePrincipal(string userName, IList<string>? roles = null)
		{
			var principal = new ClaimsPrincipal(
				new FakePrincipal { Identity = new FakeIdentity { Name = userName }, Roles = roles });
			return principal;
		}
	}
}
