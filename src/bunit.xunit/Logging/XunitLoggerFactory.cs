using Microsoft.Extensions.Logging;

namespace Bunit.Xunit.Logging
{
	/// <summary>
	/// Represents a xUnit logger factory.
	/// </summary>
	public class XunitLoggerFactory : LoggerFactory
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="XunitLoggerFactory"/> class.
		/// </summary>
		public XunitLoggerFactory(ILoggerProvider xunitLoggerProvider)
		{
			AddProvider(xunitLoggerProvider);
		}
	}
}
