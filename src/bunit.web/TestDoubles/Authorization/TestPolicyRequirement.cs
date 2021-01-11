using Microsoft.AspNetCore.Authorization;

namespace Bunit.TestDoubles
{
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
