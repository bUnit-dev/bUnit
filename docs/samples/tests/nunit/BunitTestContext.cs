namespace Bunit.Docs.Samples;

using Bunit;
using NUnit.Framework;

public abstract class BunitTestContext : TestContextWrapper
{
  [SetUp]
  public void Setup() => TestContext = new Bunit.TestContext();

  [TearDown]
  public void TearDown() => TestContext?.Dispose();
}