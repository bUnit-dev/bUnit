using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Bunit.TestDoubles
{
	/// <summary>
	/// Test class that provides an implementation of IAuthorizationPolicyProvider that
	/// assists user in testing components that use authentication and authorization.
	/// </summary>
	public class FakeAuthorizationPolicyProvider : IAuthorizationPolicyProvider
	{
		private readonly AuthorizationOptions options = new();
		private string policySchemeName = "TestScheme";

		/// <summary>
		/// Gets the default authorization policy.
		/// </summary>
		/// <returns>Default policy.</returns>
		public Task<AuthorizationPolicy> GetDefaultPolicyAsync()
			=> Task.FromResult(options.DefaultPolicy);

		/// <summary>
		/// Gets the fallback authorization policy.
		/// </summary>
		/// <returns>Fallback policy.</returns>
		public Task<AuthorizationPolicy?> GetFallbackPolicyAsync()
			=> Task.FromResult<AuthorizationPolicy?>(options.FallbackPolicy);

		/// <summary>
		/// Get the current policy, which in this implementation just returns a test policy.
		/// </summary>
		/// <param name="policyName">Policy name.</param>
		/// <returns>Test policy for the specified name.</returns>
		public Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
		{
			var authPolicy = new AuthorizationPolicy(
				new[]
				{
					new TestPolicyRequirement { PolicyName = policyName },
				},
				new[] { $"{policySchemeName}:{policyName}" });

			return Task.FromResult<AuthorizationPolicy?>(authPolicy);
		}

		/// <summary>
		/// Sets the currently supported policies for this provider.
		/// </summary>
		/// <param name="policySchemeName">Policy scheme name to use.</param>
		public void SetPolicyScheme(string policySchemeName)
		{
			if (string.IsNullOrEmpty(policySchemeName))
				throw new ArgumentNullException(nameof(policySchemeName));

			this.policySchemeName = policySchemeName;
		}
	}
}
