using System;

namespace Bunit.Asserting
{
	/// <summary>
	/// Add this attribute to assertion methods to indicate to
	/// 3rd party analyzers that the method is an assertion method.
	/// See more here: https://jira.sonarsource.com/browse/RSPEC-3413
	/// </summary>
	[AttributeUsage(AttributeTargets.Method)]
	public sealed class AssertionMethodAttribute : Attribute { }
}
