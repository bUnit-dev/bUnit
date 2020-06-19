using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Bunit.TestDoubles.Authorization
{
	/// <summary>
	/// Test class that provides an implementation of IAuthorizationService that
	/// assists user in testing components by allowing them to set the authorization
	/// expected authorization response.
	/// </summary>
	public class FakeAuthorizationService : IAuthorizationService
	{
		private readonly List<(ClaimsPrincipal user, object resource, IEnumerable<IAuthorizationRequirement> requirements)> _authorizeCalls = new List<(ClaimsPrincipal user, object resource, IEnumerable<IAuthorizationRequirement> requirements)>();
		private AuthorizationState _currentState = AuthorizationState.Authorized;

		/// <summary>
		/// Constructor that initializes the authorization state of the service. 
		/// </summary>
		/// <param name="state"></param>
		public FakeAuthorizationService(AuthorizationState state = AuthorizationState.Authorized)
		{
			_currentState = state;
		}

		/// <summary>
		/// Gets a list of the AuthorizeAsync calls that were made to this service.
		/// </summary>
		public IReadOnlyList<(ClaimsPrincipal user, object resource, IEnumerable<IAuthorizationRequirement> requirements)> AuthorizeCalls => _authorizeCalls;

		/// <summary>
		/// Method to explicitly set the authorization state.
		/// </summary>
		/// <param name="state">New state to set.</param>
		public void SetAuthorizationState(AuthorizationState state)
		{
			_currentState = state;
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
			_authorizeCalls.Add((user, resource, requirements));

			// The FakeAuthorizationService doesn't apply any authorization requirements.
			// It just returns an AuthorizationResult based on the current authorization state.
			var result = (_currentState == AuthorizationState.Authorized) ? AuthorizationResult.Success() : AuthorizationResult.Failed();
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
	}
}
