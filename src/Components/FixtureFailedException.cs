using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Egil.RazorComponents.Testing;
using System.Diagnostics.CodeAnalysis;

namespace Xunit.Sdk
{
    /// <summary>
    /// Exception thrown when an <see cref="Fixture"/> in a Razor based test fails.
    /// </summary>
    [SuppressMessage("Design", "CA1032:Implement standard exception constructors", Justification = "Do not need them")]
    public class FixtureFailedException : XunitException
    {
        /// <summary>
        /// Creates an instance of the <see cref="FixtureFailedException"/> class.
        /// </summary>
        public FixtureFailedException(string message, Exception assertFailureException)
            : base(message, assertFailureException)
        {
        }
    }
}
