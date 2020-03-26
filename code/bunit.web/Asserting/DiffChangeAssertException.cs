using System;
using AngleSharp.Diffing.Core;
using Bunit.Asserting;

namespace Bunit
{
	/// <summary>
	/// Represents an diff change assertion that has failed.
	/// </summary>
	public class DiffChangeAssertException : ActualExpectedAssertException
	{
		/// <summary>
		/// Creates an instance of the <see cref="DiffChangeAssertException"/>.
		/// </summary>
		/// <param name="actual">The actual change</param>
		/// <param name="expected">The expected change</param>
		/// <param name="message">An error message explaining the context of the assertion</param>
		public DiffChangeAssertException(DiffResult actual, DiffResult expected, string message) : base(PrintDiffResult(actual), PrintDiffResult(expected), "Actual change", "Expected change", message)
		{
		}

		private static string PrintDiffResult(DiffResult result) => result switch
		{
			DiffResult.Different => "Changed",
			DiffResult.Missing => "Removed",
			DiffResult.Unexpected => "Added",
			_ => throw new InvalidOperationException("Unknown DiffResult type")
		};
	}
}
