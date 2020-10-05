using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;

namespace Bunit.TestDoubles
{
	/// <summary>
	/// A fake Principal implementation that represents an authenticated user.
	/// </summary>
	internal class FakePrincipal : IPrincipal
	{
		private IEnumerable<string> _roles = Array.Empty<string>();

		/// <summary>
		/// Gets or sets the identity for this authenticated principal.
		/// </summary>
		public IIdentity? Identity { get; set; }

		/// <summary>
		/// Gets or sets the set of roles this user is authorized for.
		/// </summary>
		public IEnumerable<string> Roles
		{
			get => _roles;
			set => _roles = value ?? throw new ArgumentNullException(nameof(value));
		}

		/// <summary>
		/// Default non-authenticated principal returns false for IsInRole check.
		/// </summary>
		/// <param name="role">Role name</param>
		/// <returns>Returns that this principal is not in any role.</returns>
		public bool IsInRole(string role) => Roles.Any(p => p.Equals(role, StringComparison.Ordinal));
	}
}
