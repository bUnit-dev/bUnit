using Microsoft.Extensions.Logging;

namespace Bunit.Logging
{
    /// <summary>
    /// Represents a xUnit logger factory
    /// </summary>
    public class XunitLoggerFactory : LoggerFactory
    {
        /// <inheritdoc/>
        public XunitLoggerFactory(XunitLoggerProvider xunitLoggerProvider)
        {
            AddProvider(xunitLoggerProvider);
        }
    }
}
