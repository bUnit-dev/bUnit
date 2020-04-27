using System;
using System.Diagnostics.CodeAnalysis;

namespace Bunit.RazorTesting
{
	/// <summary>
	/// Exception thrown when an <see cref="RazorTestBase"/> in a Razor based test fails.
	/// </summary>
	[SuppressMessage("Design", "CA1032:Implement standard exception constructors", Justification = "Do not need them")]
	public class RazorTestFailedException : Exception
	{
		/// <summary>
		/// Creates an instance of the <see cref="RazorTestFailedException"/> class.
		/// </summary>
		public RazorTestFailedException(string message, Exception assertFailureException)
			: base(message, assertFailureException)
		{
		}
	}
}
