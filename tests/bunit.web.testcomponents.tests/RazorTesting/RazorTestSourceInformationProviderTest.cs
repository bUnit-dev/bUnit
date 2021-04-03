using System;
using System.IO;
using Bunit.Rendering;
using Bunit.SampleComponents;
using Moq;
using Shouldly;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Bunit.RazorTesting
{
	public sealed class RazorTestSourceInformationProviderTest : IDisposable
	{
		private readonly TestComponentRenderer renderer = new();
		private readonly IMessageSink messageBus = Mock.Of<IMessageSink>();

		private RazorTestBase GetTest(Type testComponent, int testIndex)
		{
			var tests = renderer.GetRazorTestsFromComponent(testComponent);
			return tests[testIndex - 1];
		}

		// Ignored because file doesnt seem to be compile on Linux [InlineData(typeof(MixedCaseComponent), 1, 2)]
		[Theory(DisplayName = "Can find source info")]
		[InlineData(typeof(ComponentWithoutMethods), 1, 2)]
		[InlineData(typeof(ComponentWithMethod), 1, 2)]
		[InlineData(typeof(ComponentWithTwoTests), 1, 3)]
		[InlineData(typeof(ComponentWithTwoTests), 2, 8)]
		[InlineData(typeof(TestCasesWithWeirdLineBreaks), 1, 2)]
		[InlineData(typeof(TestCasesWithWeirdLineBreaks), 2, 7)]
		public void Test001(Type target, int testNumber, int expectedLineNumber)
		{
			using var sut = new RazorTestSourceInformationProvider(messageBus);

			var sourceInfo = sut.GetSourceInformation(target, GetTest(target, testNumber), testNumber);

			sourceInfo.ShouldNotBeNull();
			sourceInfo?.FileName.ShouldEndWith($"SampleComponents{Path.DirectorySeparatorChar}{target.Name}.razor", Case.Insensitive);
			sourceInfo?.LineNumber.ShouldBe(expectedLineNumber);
		}

		public void Dispose() => renderer.Dispose();
	}
}
