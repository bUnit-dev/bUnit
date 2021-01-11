using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Bunit.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace Bunit.TestDoubles
{
	/// <summary>
	/// Test class that provides an implementation of IAuthorizationService that
	/// assists user in testing components by allowing them to set the authorization
	/// expected authorization response.
	/// </summary>
	public class FakeAuthorizationService : IAuthorizationService
	{
		private AuthorizationState currentState;
		private IEnumerable<string>? supportedPolicies;
		private IEnumerable<string> supportedRoles = Array.Empty<string>();

		/// <summary>
		/// Initializes a new instance of the <see cref="FakeAuthorizationService"/> class.
		/// </summary>
		/// <param name="state">Initial authorization state.</param>
		public FakeAuthorizationService(AuthorizationState state = AuthorizationState.Authorized)
		{
			currentState = state;
		}

		/// <summary>
		/// Method to explicitly set the authorization state.
		/// </summary>
		/// <param name="state">New state to set.</param>
		public void SetAuthorizationState(AuthorizationState state)
		{
			currentState = state;
		}

		/// <summary>
		/// Sets the supported policies in the Authorization service for current user.
		/// </summary>
		/// <param name="policies">List of supported policies.</param>
		public void SetPolicies(IEnumerable<string> policies)
		{
			supportedPolicies = policies;
		}

		/// <summary>
		/// Sets the supported roles in the Authorization service for current user.
		/// </summary>
		/// <param name="roles">List of supported roles.</param>
		public void SetRoles(IEnumerable<string> roles)
		{
			supportedRoles = roles ?? Array.Empty<string>();
		}

		/// <summary>
		/// Authorizes the Principal user based solely on the current authorization state.
		/// </summary>
		/// <param name="user">Principal user to authorize.</param>
		/// <param name="resource">Resource being authorized.</param>
		/// <param name="requirements">Authorization requirements.</param>
		/// <returns>Result of authorize request.</returns>
		public Task<AuthorizationResult> AuthorizeAsync(ClaimsPrincipal user, object? resource, IEnumerable<IAuthorizationRequirement> requirements)
		{
			if (requirements is null)
				throw new ArgumentNullException(nameof(requirements));

			AuthorizationResult result;

			if (requirements.All(p => p is DenyAnonymousAuthorizationRequirement))
			{
				result = (currentState == AuthorizationState.Authorized) ? AuthorizationResult.Success() : AuthorizationResult.Failed();
			}
			else if (requirements.All(p => p is RolesAuthorizationRequirement))
			{
				result = VerifyRequiredRoles(requirements);
			}
			else if (supportedPolicies is not null)
			{
				result = VerifyRequiredPolicies(requirements);
			}
			else
			{
				result = AuthorizationResult.Failed();
			}

			return Task.FromResult(result);
		}

		/// <summary>
		/// Authorizes the Principal user based solely on the current authorization state.
		/// </summary>
		/// <param name="user">Principal user to authorize.</param>
		/// <param name="resource">Resource being authorized.</param>
		/// <param name="policyName">Policy to use for authorization.</param>
		/// <returns>Result of authorize request.</returns>
		public Task<AuthorizationResult> AuthorizeAsync(ClaimsPrincipal user, object? resource, string policyName)
		{
			var requirements = new List<IAuthorizationRequirement>
			{
				new TestPolicyRequirement { PolicyName = policyName },
			};

			return AuthorizeAsync(user, resource, requirements);
		}

		private AuthorizationResult VerifyRequiredPolicies(IEnumerable<IAuthorizationRequirement> requirements)
		{
			if (supportedPolicies.IsNullOrEmpty() || requirements.IsNullOrEmpty())
			{
				return AuthorizationResult.Failed();
			}

			foreach (IAuthorizationRequirement req in requirements)
			{
				if (req is TestPolicyRequirement testReq && supportedPolicies.Contains(testReq.PolicyName, StringComparer.Ordinal))
					return AuthorizationResult.Success();
			}

			return AuthorizationResult.Failed();
		}

		private AuthorizationResult VerifyRequiredRoles(IEnumerable<IAuthorizationRequirement> requirements)
		{
			AuthorizationResult result = AuthorizationResult.Failed();
			foreach (IAuthorizationRequirement req in requirements)
			{
				if (req is RolesAuthorizationRequirement testReq)
				{
					IEnumerable<string> rolesFound = testReq.AllowedRoles.Intersect(supportedRoles, StringComparer.Ordinal);
					if (rolesFound.Any())
					{
						result = AuthorizationResult.Success();
						break;
					}
				}
			}

			return result;
		}
	}
}
