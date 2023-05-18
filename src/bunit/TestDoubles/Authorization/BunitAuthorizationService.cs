using System.Security.Claims;
using Bunit.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace Bunit.TestDoubles;

/// <summary>
/// Test class that provides an implementation of IAuthorizationService that
/// assists user in testing components by allowing them to set the authorization
/// expected authorization response.
/// </summary>
public class BunitAuthorizationService : IAuthorizationService
{
	private AuthorizationState currentState;
	private IEnumerable<string>? supportedPolicies;
	private IEnumerable<string> supportedRoles = Array.Empty<string>();

	/// <summary>
	/// Initializes a new instance of the <see cref="BunitAuthorizationService"/> class.
	/// </summary>
	/// <param name="state">Initial authorization state.</param>
	public BunitAuthorizationService(AuthorizationState state = AuthorizationState.Authorized)
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
		ArgumentNullException.ThrowIfNull(requirements);

		AuthorizationResult result;

		var requirementsArray = requirements.ToArray();
		if (requirementsArray.All(p => p is DenyAnonymousAuthorizationRequirement))
		{
			result = currentState == AuthorizationState.Authorized
				? AuthorizationResult.Success()
				: AuthorizationResult.Failed();
		}
		else if (requirementsArray.All(p => p is RolesAuthorizationRequirement))
		{
			result = VerifyRequiredRoles(requirementsArray);
		}
		else if (supportedPolicies is not null)
		{
			result = VerifyRequiredPolicies(requirementsArray);
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

	private AuthorizationResult VerifyRequiredPolicies(IReadOnlyCollection<IAuthorizationRequirement> requirements)
	{
		if (supportedPolicies.IsNullOrEmpty() || requirements.IsNullOrEmpty())
		{
			return AuthorizationResult.Failed();
		}

		return requirements.OfType<TestPolicyRequirement>().Any(req => supportedPolicies.Contains(req.PolicyName, StringComparer.Ordinal))
			? AuthorizationResult.Success()
			: AuthorizationResult.Failed();
	}

	private AuthorizationResult VerifyRequiredRoles(IReadOnlyCollection<IAuthorizationRequirement> requirements)
	{
		var result = AuthorizationResult.Failed();
		foreach (var req in requirements.OfType<RolesAuthorizationRequirement>())
		{
			var rolesFound = req.AllowedRoles.Intersect(supportedRoles, StringComparer.Ordinal);
			if (rolesFound.Any())
			{
				result = AuthorizationResult.Success();
				break;
			}
		}

		return result;
	}
}
