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
		/// <summary>
		/// Constructor that intializes the authorization state of the service. 
		/// </summary>
		/// <param name="state"></param>
		public FakeAuthorizationService(AuthorizationState state = AuthorizationState.Authorized)
		{
			CurrentState = state;
		}

		/// <summary>
		/// Gets or sets the current authorization state for this service.
		/// </summary>
		public AuthorizationState CurrentState { get; private set; } = AuthorizationState.Authorized;

		/// <summary>
		/// Gets a list of the AuthorizeAsync calls that were made to this service.
		/// </summary>
		public List<(ClaimsPrincipal user, object resource, IEnumerable<IAuthorizationRequirement> requirements)> AuthorizeCalls { get; }
			= new List<(ClaimsPrincipal user, object resource, IEnumerable<IAuthorizationRequirement> requirements)>();

		/// <summary>
		/// Method to explicitly set the authorization state.
		/// </summary>
		/// <param name="state">New state to set.</param>
		public void SetAuthorizationState(AuthorizationState state)
		{
			CurrentState = state;
		}

		/// <summary>
		/// Authorizes the Principal user based solely on the valuy of the NextResult property.
		/// </summary>
		/// <param name="user">Principal user to authorize.</param>
		/// <param name="resource">Resource being authorized.</param>
		/// <param name="requirements">Authorization requirements.</param>
		/// <returns>Result of authorize request.</returns>
		public Task<AuthorizationResult> AuthorizeAsync(ClaimsPrincipal user, object resource, IEnumerable<IAuthorizationRequirement> requirements)
		{
			AuthorizeCalls.Add((user, resource, requirements));

			// The FakeAuthorizationService doesn't apply any authorization requirements.
			// It just returns whatever the user specified in the NextResult property.
			var result = (CurrentState == AuthorizationState.Authorized) ? AuthorizationResult.Success() : AuthorizationResult.Failed();
			return Task.FromResult(result);
		}

		/// <summary>
		/// Authorizes the Principal user based solely on the valuy of the NextResult property.
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
