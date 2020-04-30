using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bunit.Rendering;
using Bunit.SampleComponents;
using Moq;
using Shouldly;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Bunit.RazorTesting
{
	public class RazorTestSourceInformationProviderTest : IDisposable
	{
		private readonly TestComponentRenderer _renderer = new TestComponentRenderer();
		private readonly IMessageSink _messageBus = Mock.Of<IMessageSink>();

		private async Task<RazorTestBase> GetTest(Type testComponent, int testIndex)
		{
			var tests = await _renderer.GetRazorTestsFromComponent(testComponent);
			return tests[testIndex - 1];
		}

		[Theory(DisplayName = "Can find source info")]
		[InlineData(typeof(ComponentWithoutMethods), 1, 2)]
		[InlineData(typeof(ComponentWithMethod), 1, 2)]
		[InlineData(typeof(ComponentWithTwoTests), 1, 3)]
		[InlineData(typeof(ComponentWithTwoTests), 2, 8)]
		[InlineData(typeof(MixedCaseComponent), 1, 2)]
		[InlineData(typeof(TestCasesWithWeirdLineBreaks), 1, 2)]
		[InlineData(typeof(TestCasesWithWeirdLineBreaks), 2, 7)]
		public async Task Test001(Type target, int testNumber, int expectedLineNumber)
		{
			var sut = new RazorTestSourceInformationProvider(_messageBus);

			var sourceInfo = sut.GetSourceInformation(target, await GetTest(target, testNumber), testNumber);

			sourceInfo.ShouldNotBeNull();
			sourceInfo?.FileName.ShouldEndWith($"SampleComponents{Path.DirectorySeparatorChar}{target.Name}.razor", Case.Insensitive);
			sourceInfo?.LineNumber.ShouldBe(expectedLineNumber);
		}

		void IDisposable.Dispose() => _renderer.Dispose();
	}
}
