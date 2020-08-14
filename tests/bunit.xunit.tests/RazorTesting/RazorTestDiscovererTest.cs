using System;
using System.Linq;
using System.Threading;

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
		private readonly ExceptionAggregator _aggregator;
		private readonly IMessageSink _messageBus;
		private readonly ITestFrameworkDiscoveryOptions _options;
		private readonly IReflectionAttributeInfo _attribute;

		public RazorTestDiscovererTest()
		{
			_aggregator = new ExceptionAggregator();
			_options = TestFrameworkOptions.ForDiscovery();
			_messageBus = Mock.Of<IMessageSink>();
			_attribute = Mock.Of<IReflectionAttributeInfo>();
		}

		[Fact(DisplayName = "Can find single razor test in test component")]
		public void Test001()
		{
			var discoverer = new RazorTestDiscoverer(_messageBus);
			var testMethod = Mocks.TestMethod(typeof(OneFixtureComponent), nameof(TestComponentBase.RazorTests));

			var testCases = discoverer.Discover(_options, testMethod, _attribute);

			testCases.ShouldAllBe(x => x.DisplayName.ShouldBe("FIXTURE 1"));
		}

		[Fact(DisplayName = "Can find two razor test in test component")]
		public void Test002()
		{
			var discoverer = new RazorTestDiscoverer(_messageBus);
			var testMethod = Mocks.TestMethod(typeof(TwoFixtureComponent), nameof(TestComponentBase.RazorTests));

			var testCases = discoverer.Discover(_options, testMethod, _attribute);

			testCases.ShouldAllBe(
				x => x.DisplayName.ShouldBe("FIXTURE 1"),
				x => x.DisplayName.ShouldBe("FIXTURE 2")
			);
		}

		[Fact(DisplayName = "Can find zero razor test in test component")]
		public void Test003()
		{
			var discoverer = new RazorTestDiscoverer(_messageBus);
			var testMethod = Mocks.TestMethod(typeof(ZeroFixtureComponent), nameof(TestComponentBase.RazorTests));

			var testCases = discoverer.Discover(_options, testMethod, _attribute);

			testCases.ShouldBeEmpty();
		}

		[Fact(DisplayName = "If no description is provided, the name of the test method is used")]
		public void Test004()
		{
			var discoverer = new RazorTestDiscoverer(_messageBus);
			var testMethod = Mocks.TestMethod(typeof(FixturesWithoutDescription), nameof(TestComponentBase.RazorTests));

			var testCases = discoverer.Discover(_options, testMethod, _attribute);

			testCases.ShouldAllBe(
				x => x.DisplayName.ShouldBe(nameof(FixturesWithoutDescription.SyncTest)),
				x => x.DisplayName.ShouldBe(nameof(FixturesWithoutDescription.AsyncTest))
			);
		}

		[Fact(DisplayName = "Timeout is set correctly in test case")]
		public void Test005()
		{
			var discoverer = new RazorTestDiscoverer(_messageBus);
			var testMethod = Mocks.TestMethod(typeof(TimeoutRazorComponent), nameof(TestComponentBase.RazorTests));

			var testCases = discoverer.Discover(_options, testMethod, _attribute);

			var actualTimeout = testCases.Single().Timeout;

			TimeSpan.FromMilliseconds(actualTimeout).ShouldBe(TimeoutRazorComponent.TIMEOUT);
		}
	}
}
