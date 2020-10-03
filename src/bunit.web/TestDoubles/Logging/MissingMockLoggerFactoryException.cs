using System;
using System.Collections.Generic;

namespace Bunit.TestDoubles.Logging
{
	/// <summary>
	/// Exception use to indicate that a Logger is required by a test
	/// but was not provided.
	/// </summary>
	public class MissingMockLoggerFactoryException : Exception
	{
		/// <summary>
		/// The method that was called by the logger factory
		/// </summary>
		public string MethodName { get; }

		/// <summary>
		/// The parameters passed into the logger factory
		/// </summary>
		public IReadOnlyList<object?> Arguments { get; }

		/// <summary>
		/// Creates a new instance of <see cref="MissingMockLoggerFactoryException"/>
		/// with the arguments used in the invocation.
		/// </summary>
		/// <param name="methodName"></param>
		/// <param name="arguments"></param>
		public MissingMockLoggerFactoryException(string methodName, params object?[]? arguments)
			: base ($"This test requires a LoggerFactory to be supplied, because the component under test invokes the LoggerFactory during the test. The method invocation was '{methodName}', the arguments are contained within the '{nameof(Arguments)}' attribute of this exception. Guidance on mocking the LoggerFactory is available in the testing library's Wiki.")
		{
			MethodName = methodName;
			Arguments = arguments ?? Array.Empty<object?>();
		}
	}
}
