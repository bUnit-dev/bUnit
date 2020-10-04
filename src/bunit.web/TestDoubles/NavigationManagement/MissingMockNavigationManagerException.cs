﻿using System;

namespace Bunit.TestDoubles.NavigationManagement
{
	/// <summary>
	/// Exception use to indicate that a NavigationManager is required by a test
	/// but was not provided.
	/// </summary>
	public class MissingMockNavigationManagerException : Exception
	{
		private static string ExceptionMessage(string url, bool forceLoad) =>
			$"This test requires a NavigationManager to be supplied, because the component under test invokes the NavigationManager during the test. The url that was requested was '{url}' with a force reload value of '{forceLoad.ToString()}'.";

		/// <summary>
		/// Creates a new instance of the <see cref="MissingMockNavigationManagerException"/>
		/// with the arguments used in the invocation.
		/// </summary>
		/// <param name="url">Uri to navigate to</param>
		/// <param name="forceLoad">Whether to force load</param>
		public MissingMockNavigationManagerException(string url, bool forceLoad)
			: base (ExceptionMessage(url, forceLoad))
		{

		}

		/// <summary>
		/// Creates a new instance of the <see cref="MissingMockNavigationManagerException"/>
		/// with the arguments used in the invocation.
		/// </summary>
		/// <param name="url">Uri to navigate to</param>
		/// <param name="forceLoad">Whether to force load</param>
		public MissingMockNavigationManagerException(Uri url, bool forceLoad)
			: base (ExceptionMessage(url?.ToString() ?? string.Empty, forceLoad))
		{

		}
	}
}
