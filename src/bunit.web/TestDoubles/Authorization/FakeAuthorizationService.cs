using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace Bunit.TestDoubles.Authorization
{
	/// <summary>
	/// Test class that provides an implementation of IAuthorizationService that
	/// assists user in testing components by allowing them to set the authorization
	/// expected authorization response.
	/// </summary>
	public class FakeAuthorizationService : IAuthorizationService
	{
		private AuthorizationState _currentState = AuthorizationState.Authorized;
		private IEnumerable<string>? _supportedPolicies;
		private IEnumerable<string> _supportedRoles = new List<string>();

		/// <summary>
		/// Constructor that initializes the authorization state of the service. 
		/// </summary>
		/// <param name="state">Initial authorization state.</param>
		public FakeAuthorizationService(AuthorizationState state = AuthorizationState.Authorized)
		{
			_currentState = state;
		}

		/// <summary>
		/// Method to explicitly set the authorization state.
		/// </summary>
		/// <param name="state">New state to set.</param>
		public void SetAuthorizationState(AuthorizationState state)
		{
			_currentState = state;
		}

		/// <summary>
		/// Sets the supported policies in the Authoriation service for current user.
		/// </summary>
		/// <param name="policies">List of supported policies.</param>
		public void SetPolicies(IEnumerable<string> policies)
		{
			_supportedPolicies = policies;
		}

		/// <summary>
		/// Sets the supported roles in the Authoriation service for current user.
		/// </summary>
		/// <param name="roles">List of supported roles.</param>
		public void SetRoles(IEnumerable<string> roles)
		{
			_supportedRoles = roles;
		}

		/// <summary>
		/// Authorizes the Principal user based solely on the current authorization state.
		/// </summary>
		/// <param name="user">Principal user to authorize.</param>
		/// <param name="resource">Resource being authorized.</param>
		/// <param name="requirements">Authorization requirements.</param>
		/// <returns>Result of authorize request.</returns>
		public Task<AuthorizationResult> AuthorizeAsync(ClaimsPrincipal user, object resource, IEnumerable<IAuthorizationRequirement> requirements)
		{
			_ = requirements ?? throw new ArgumentNullException(nameof(requirements));

			AuthorizationResult result;

			if (requirements.All(p => p is DenyAnonymousAuthorizationRequirement))
			{
				result = (_currentState == AuthorizationState.Authorized) ? AuthorizationResult.Success() : AuthorizationResult.Failed();
			}
			else if (requirements.All( p => p is RolesAuthorizationRequirement))
			{
				result = VerifyRequiredRoles(requirements);
			}
			else if (_supportedPolicies != null)
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
		public Task<AuthorizationResult> AuthorizeAsync(ClaimsPrincipal user, object resource, string policyName)
		{
			var requirements = new List<IAuthorizationRequirement>
			{
				new TestPolicyRequirement { PolicyName = policyName }
			};

			return AuthorizeAsync(user, resource, requirements);
		}

		private AuthorizationResult VerifyRequiredPolicies(IEnumerable<IAuthorizationRequirement> requirements)
		{
			AuthorizationResult result = AuthorizationResult.Failed();
			foreach (IAuthorizationRequirement req in requirements)
			{
				if (req is TestPolicyRequirement testReq)
				{
					if (_supportedPolicies.Contains(testReq.PolicyName))
					{
						result = AuthorizationResult.Success();
						break;
					}
				}
			}

			return result;
		}

		private AuthorizationResult VerifyRequiredRoles(IEnumerable<IAuthorizationRequirement> requirements)
		{
			AuthorizationResult result = AuthorizationResult.Failed();
			foreach (IAuthorizationRequirement req in requirements)
			{
				if (req is RolesAuthorizationRequirement testReq)
				{
					IEnumerable<string> rolesFound = testReq.AllowedRoles.Intersect(_supportedRoles);
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
