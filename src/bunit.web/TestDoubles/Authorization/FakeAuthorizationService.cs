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
		/// Gets to sets the next authorization result. This can be set to the appropriate state
		/// in your tests, so that you will get an authorized or not-authorized response.
		/// </summary>
		public AuthorizationResult NextResult { get; set; }
			= AuthorizationResult.Success();

		/// <summary>
		/// Gets a list of the AuthorizeAsync calls that were made to this service.
		/// </summary>
		public List<(ClaimsPrincipal user, object resource, IEnumerable<IAuthorizationRequirement> requirements)> AuthorizeCalls { get; }
			= new List<(ClaimsPrincipal user, object resource, IEnumerable<IAuthorizationRequirement> requirements)>();

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
			return Task.FromResult(NextResult);
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
