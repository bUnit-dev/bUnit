using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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
		private readonly CancellationTokenSource _cancellationTokenSource;
		private readonly IMessageSink _messageBus;
		private readonly ITestFrameworkDiscoveryOptions _options;
		private readonly IReflectionAttributeInfo _attribute;

		public RazorTestDiscovererTest()
		{
			_aggregator = new ExceptionAggregator();
			_cancellationTokenSource = new CancellationTokenSource();
			_options = TestFrameworkOptions.ForDiscovery();
			_messageBus = Mock.Of<IMessageSink>();
			_attribute = Mock.Of<IReflectionAttributeInfo>();
		}

		[Fact]
		public void MyTestMethod()
		{
			var discoverer = new RazorTestDiscoverer(_messageBus);
			var testMethod = Mocks.TestMethod(typeof(OneFixtureComponent), nameof(OneFixtureComponent.Test));

			var testCases = discoverer.Discover(_options, testMethod, _attribute);

			testCases.ShouldHaveSingleItem()
				.ShouldBeOfType<RazorTestCase>()
				.DisplayName.ShouldBe("DESCRIPTION");
		}


	}

	public static class Mocks
	{
		public static TestMethod TestMethod(Type type, string methodName, ITestCollection? collection = null)
		{
			var @class = TestClass(type, collection);
			var methodInfo = type.GetMethod(methodName);
			if (methodInfo == null)
				throw new Exception($"Unknown method: {type.FullName}.{methodName}");

			return new TestMethod(@class, Reflector.Wrap(methodInfo));
		}

		public static TestClass TestClass(Type type, ITestCollection? collection = null)
		{
			if (collection == null)
				collection = TestCollection(type.GetTypeInfo().Assembly);

			return new TestClass(collection, Reflector.Wrap(type));
		}

		public static TestCollection TestCollection(Assembly? assembly = null, ITypeInfo? definition = null, string? displayName = null)
		{
			if (assembly == null)
				assembly = typeof(Mocks).GetTypeInfo().Assembly;
			if (displayName == null)
				displayName = "Mock test collection for " + assembly.CodeBase;

			return new TestCollection(TestAssembly(assembly), definition, displayName);
		}
		public static TestAssembly TestAssembly(Assembly? assembly = null, string? configFileName = null)
		{
			return new TestAssembly(Reflector.Wrap(assembly ?? typeof(Mocks).GetTypeInfo().Assembly), configFileName);
		}
	}
}
