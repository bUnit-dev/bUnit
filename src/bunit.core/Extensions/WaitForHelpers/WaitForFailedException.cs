using System;

namespace Bunit.Extensions.WaitForHelpers
{
	/// <summary>
	/// Represents an exception thrown when the <see cref="WaitForHelper"/> does not complete successfully.
	/// </summary>
	public class WaitForFailedException : Exception
	{
		/// <summary>
		/// Creates an instance of the <see cref="WaitForFailedException"/>.
		/// </summary>
		public WaitForFailedException(string? errorMessage, Exception? innerException = null) : base(errorMessage ?? string.Empty, innerException)
		{
		}
	}
}
