using System;
using Microsoft.Extensions.Logging;

namespace Bunit.TestDoubles.Logging
{
	/// <summary>
	/// This LogFactory is used to provide users with helpful exceptions if they fail to provide a mock when required.
	/// </summary>
	public class PlaceholderLogFactory : ILoggerFactory
	{
		/// <summary>
		///
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);

		}

		/// <summary>
		///
		/// </summary>
		/// <param name="disposing"></param>
		protected virtual void Dispose(bool disposing)
		{
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="categoryName"></param>
		/// <returns></returns>
		/// <exception cref="MissingMockLoggerFactoryException"></exception>
		public ILogger CreateLogger(string categoryName)
		{
			throw new MissingMockLoggerFactoryException(nameof(CreateLogger), categoryName);
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="provider"></param>
		/// <exception cref="MissingMockLoggerFactoryException"></exception>
		public void AddProvider(ILoggerProvider provider)
		{
			throw new MissingMockLoggerFactoryException(nameof(AddProvider), provider);
		}
	}
}
