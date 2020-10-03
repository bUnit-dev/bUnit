using Microsoft.Extensions.Logging;

namespace Bunit.TestDoubles.Logging
{
	/// <summary>
	/// This LogFactory is used to provide users with helpful exceptions if they fail to provide a mock when required.
	/// </summary>
	public class PlaceholderLogFactory : ILoggerFactory
	{
		public void Dispose()
		{
		}

		public ILogger CreateLogger(string categoryName)
		{
			throw new MissingMockLoggerFactoryException(nameof(CreateLogger), categoryName);
		}

		public void AddProvider(ILoggerProvider provider)
		{
			throw new MissingMockLoggerFactoryException(nameof(AddProvider), provider);
		}
	}
}
