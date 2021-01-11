using System;

namespace Bunit.Asserting
{
	/// <summary>
	/// Add this attribute to assertion methods to indicate to
	/// 3rd party analyzers that the method is an assertion method.
	/// See more here: https://rules.sonarsource.com/csharp/RSPEC-2699.
	/// </summary>
	[AttributeUsage(AttributeTargets.Method)]
	public sealed class AssertionMethodAttribute : Attribute { }
}
