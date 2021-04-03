using System;
using System.Linq;
using Bunit.SampleComponents;
using Moq;
using Shouldly;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Bunit.RazorTesting
{
	public class RazorTestDiscovererTest
	{
		private readonly IMessageSink messageBus;
		private readonly ITestFrameworkDiscoveryOptions options;
		private readonly IReflectionAttributeInfo attribute;

		public RazorTestDiscovererTest()
		{
			options = TestFrameworkOptions.ForDiscovery();
			messageBus = Mock.Of<IMessageSink>();
			attribute = Mock.Of<IReflectionAttributeInfo>();
		}

		[Fact(DisplayName = "Can find single razor test in test component")]
		public void Test001()
		{
			using var discoverer = new RazorTestDiscoverer(messageBus);
			var testMethod = Mocks.TestMethod(typeof(OneFixtureComponent), nameof(TestComponentBase.RazorTests));

			var testCases = discoverer.Discover(options, testMethod, attribute);

			testCases.ShouldAllBe(x => x.DisplayName.ShouldBe("FIXTURE 1"));
		}

		[Fact(DisplayName = "Can find two razor test in test component")]
		public void Test002()
		{
			using var discoverer = new RazorTestDiscoverer(messageBus);
			var testMethod = Mocks.TestMethod(typeof(TwoFixtureComponent), nameof(TestComponentBase.RazorTests));

			var testCases = discoverer.Discover(options, testMethod, attribute);

			testCases.ShouldAllBe(
				x => x.DisplayName.ShouldBe("FIXTURE 1"),
				x => x.DisplayName.ShouldBe("FIXTURE 2"));
		}

		[Fact(DisplayName = "Can find zero razor test in test component")]
		public void Test003()
		{
			using var discoverer = new RazorTestDiscoverer(messageBus);
			var testMethod = Mocks.TestMethod(typeof(ZeroFixtureComponent), nameof(TestComponentBase.RazorTests));

			var testCases = discoverer.Discover(options, testMethod, attribute);

			testCases.ShouldBeEmpty();
		}

		[Fact(DisplayName = "If no description is provided, the name of the test method is used")]
		public void Test004()
		{
			using var discoverer = new RazorTestDiscoverer(messageBus);
			var testMethod = Mocks.TestMethod(typeof(FixturesWithoutDescription), nameof(TestComponentBase.RazorTests));

			var testCases = discoverer.Discover(options, testMethod, attribute);

			testCases.ShouldAllBe(
				x => x.DisplayName.ShouldBe(nameof(FixturesWithoutDescription.SyncTest)),
				x => x.DisplayName.ShouldBe(nameof(FixturesWithoutDescription.AsyncTest)));
		}

		[Fact(DisplayName = "Timeout is set correctly in test case")]
		public void Test005()
		{
			using var discoverer = new RazorTestDiscoverer(messageBus);
			var testMethod = Mocks.TestMethod(typeof(TimeoutRazorComponent), nameof(TestComponentBase.RazorTests));

			var testCases = discoverer.Discover(options, testMethod, attribute);

			var actualTimeout = testCases.Single().Timeout;

			TimeSpan.FromMilliseconds(actualTimeout).ShouldBe(TimeoutRazorComponent.TIMEOUT);
		}
	}
}
