using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bunit.Docs.Samples
{
    public abstract class BunitTestContext : TestContextWrapper
    {
        [TestInitialize]
        public void Setup() => TestContext = new Bunit.TestContext();

        [TestCleanup]
        public void TearDown() => TestContext?.Dispose();
    }
}