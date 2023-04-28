namespace Bunit.Docs.Samples;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Bunit;

public abstract class BunitTestContext : TestContextWrapper
{
  [TestInitialize]
  public void Setup() => TestContext = new Bunit.TestContext();

  [TestCleanup]
  public void TearDown() => TestContext?.Dispose();
}