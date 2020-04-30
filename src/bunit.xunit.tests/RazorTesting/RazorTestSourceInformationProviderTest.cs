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
			return tests[testIndex];
		}

		[Theory(DisplayName = "Can find source info in test component with one or more tests")]
		[InlineData(typeof(ComponentWithoutMethods), 0, 2)]
		[InlineData(typeof(ComponentWithMethod), 0, 2)]
		[InlineData(typeof(ComponentWithTwoTests), 0, 3)]
		[InlineData(typeof(ComponentWithTwoTests), 1, 8)]
		[InlineData(typeof(MixedCaseComponent), 0, 1)]
		public async Task Test001(Type target, int testNumber, int expectedLineNumber)
		{
			var sut = new RazorTestSourceInformationProvider(_messageBus);

			var sourceInfo = sut.GetSourceInformation(target, await GetTest(target, testNumber), testNumber);

			sourceInfo.ShouldNotBeNull();
			sourceInfo?.FileName.ShouldEndWith($"SampleComponents{Path.DirectorySeparatorChar}{target.Name}.razor", Case.Insensitive);
			//sourceInfo?.LineNumber.ShouldBe(expectedLineNumber);
		}

		void IDisposable.Dispose() => _renderer.Dispose();
	}
}
