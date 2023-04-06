using Bunit;
using NUnit.Framework;

namespace Bunit.Docs.Samples;

public abstract class BunitTestContext : TestContextWrapper
{
  [SetUp]
  public void Setup() => TestContext = new Bunit.TestContext();

  [TearDown]
  public void TearDown() => TestContext?.Dispose();
}