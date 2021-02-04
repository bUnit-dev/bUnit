using System;
using System.Runtime.Serialization;

namespace Bunit.TestDoubles
{
	/// <summary>
	/// Exception use to indicate that a NavigationManager is required by a test
	/// but was not provided.
	/// </summary>
	[Serializable]
	public sealed class MissingMockNavigationManagerException : Exception
	{
		private static string ExceptionMessage(string url, bool forceLoad) =>
			$"This test requires a NavigationManager to be supplied, because the component under test invokes the NavigationManager during the test. " +
			$"The url that was requested was '{url}' with a force reload value of '{forceLoad}'.";

		/// <summary>
		/// Initializes a new instance of the <see cref="MissingMockNavigationManagerException"/> class
		/// with the arguments used in the invocation.
		/// </summary>
		/// <param name="url">Uri to navigate to.</param>
		/// <param name="forceLoad">Whether to force load.</param>
		public MissingMockNavigationManagerException(string url, bool forceLoad)
			: base(ExceptionMessage(url, forceLoad))
		{ }

		/// <summary>
		/// Initializes a new instance of the <see cref="MissingMockNavigationManagerException"/> class
		/// with the arguments used in the invocation.
		/// </summary>
		/// <param name="url">Uri to navigate to.</param>
		/// <param name="forceLoad">Whether to force load.</param>
		public MissingMockNavigationManagerException(Uri url, bool forceLoad)
			: base(ExceptionMessage(url?.ToString() ?? string.Empty, forceLoad))
		{ }

		private MissingMockNavigationManagerException(SerializationInfo serializationInfo, StreamingContext streamingContext)
			: base(serializationInfo, streamingContext) { }
	}
}
