using System;
using System.Runtime.Serialization;

namespace Bunit.Extensions.WaitForHelpers
{
	/// <summary>
	/// Represents an exception thrown when the <see cref="WaitForHelper"/> does not complete successfully.
	/// </summary>
	[Serializable]
	public sealed class WaitForFailedException : Exception
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="WaitForFailedException"/> class.
		/// </summary>
		public WaitForFailedException(string? errorMessage, Exception? innerException = null)
			: base(errorMessage ?? string.Empty, innerException)
		{
		}

		private WaitForFailedException(SerializationInfo info, StreamingContext context)
			: base(info, context) { }
	}
}
