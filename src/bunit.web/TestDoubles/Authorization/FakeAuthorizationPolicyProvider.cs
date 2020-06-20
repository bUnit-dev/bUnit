using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Bunit.TestDoubles.Authorization
{
	/// <summary>
	/// Test class that provides an implementation of IAuthorizationPolicyProvider that
	/// assists user in testing components that use authentication and authorization 
	/// </summary>
	public class FakeAuthorizationPolicyProvider : IAuthorizationPolicyProvider
	{
		private readonly AuthorizationOptions _options = new AuthorizationOptions();
		private string _policySchemeName = "TestScheme";
		private IList<string> _supportedPolicies = new List<string>();

		/// <summary>
		/// Gets the default authorization policy.
		/// </summary>
		/// <returns>Default policy.</returns>
		public Task<AuthorizationPolicy> GetDefaultPolicyAsync()
			=> Task.FromResult(_options.DefaultPolicy);

		/// <summary>
		/// Gets the fallback authorization policy.
		/// </summary>
		/// <returns>Fallback policy.</returns>
		public Task<AuthorizationPolicy> GetFallbackPolicyAsync()
			=> Task.FromResult(_options.FallbackPolicy);

		/// <summary>
		/// Get the current policy, which in this implementation just returns a test policy.
		/// </summary>
		/// <param name="policyName">Policy name.</param>
		/// <returns>Test policy for the specified name.</returns>
		public Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
		{
			if (_supportedPolicies.Contains(policyName))
			{
				var authPolicy = new AuthorizationPolicy(new[]
				{
					new TestPolicyRequirement { PolicyName = policyName }
				},
				new[] { $"{_policySchemeName}:{policyName}" });

				return Task.FromResult(authPolicy);
			}

			return this.GetDefaultPolicyAsync();
		}

		/// <summary>
		/// Sets the currently supported policies for this provider.
		/// </summary>
		/// <param name="policySchemeName">Policy scheme name to use.</param>
		/// <param name="policies">List of policy names supported for current user.</param>
		public void SetPolicies(string policySchemeName, IEnumerable<string> policies)
		{
			if (string.IsNullOrEmpty(policySchemeName))
				throw new ArgumentNullException(nameof(policySchemeName));

			_policySchemeName = policySchemeName;
			_supportedPolicies = new List<string>(policies);
		}
	}

	/// <summary>
	/// Test requirement that supports the minimum requirement - just a policy name.
	/// </summary>
	public class TestPolicyRequirement : IAuthorizationRequirement
	{
		/// <summary>
		/// Gets or sets the policy name for this requirement.
		/// </summary>
		public string PolicyName { get; set; } = string.Empty;
	}
}
