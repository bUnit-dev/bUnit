using System;
using System.Threading;
using AngleSharp.Dom;
using Bunit.Mocking.JSInterop;
using Bunit.SampleComponents;
using Bunit.SampleComponents.Data;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace Bunit.Rendering
{
	public class RenderWaitingHelperExtensionsTest : ComponentTestFixture
	{
		ITestOutputHelper _testOutput;
		public RenderWaitingHelperExtensionsTest(ITestOutputHelper testOutput)
		{
			Services.AddXunitLogger(testOutput);
			_testOutput = testOutput;
		}

		[Fact(DisplayName = "Nodes should return new instance when " +
						   "async operation during OnInit causes component to re-render")]
		[Obsolete("Calls to WaitForNextRender is obsolete, but still needs tests")]
		public void Test003()
		{
			var testData = new AsyncNameDep();
			Services.AddSingleton<IAsyncTestDep>(testData);
			var cut = RenderComponent<SimpleWithAyncDeps>();
			var initialValue = cut.Nodes.QuerySelector("p").TextContent;
			var expectedValue = "Steve Sanderson";

			WaitForNextRender(() => testData.SetResult(expectedValue));

			var steveValue = cut.Nodes.QuerySelector("p").TextContent;
			steveValue.ShouldNotBe(initialValue);
			steveValue.ShouldBe(expectedValue);
		}

		[Fact(DisplayName = "Nodes should return new instance when " +
					"async operation/StateHasChanged during OnAfterRender causes component to re-render")]
		[Obsolete("Calls to WaitForNextRender is obsolete, but still needs tests")]
		public void Test004()
		{
			var invocation = Services.AddMockJsRuntime().Setup<string>("getdata");
			var cut = RenderComponent<SimpleWithJsRuntimeDep>();
			var initialValue = cut.Nodes.QuerySelector("p").OuterHtml;

			WaitForNextRender(() => invocation.SetResult("NEW DATA"));

			var steveValue = cut.Nodes.QuerySelector("p").OuterHtml;
			steveValue.ShouldNotBe(initialValue);
		}

		[Fact(DisplayName = "Nodes on a components with child component returns " +
							"new instance when the child component has changes")]
		[Obsolete("Calls to WaitForNextRender is obsolete, but still needs tests")]
		public void Test005()
		{
			var invocation = Services.AddMockJsRuntime().Setup<string>("getdata");
			var notcut = RenderComponent<Wrapper>(ChildContent<Simple1>());
			var cut = RenderComponent<Wrapper>(ChildContent<SimpleWithJsRuntimeDep>());
			var initialValue = cut.Nodes;

			WaitForNextRender(() => invocation.SetResult("NEW DATA"), TimeSpan.FromSeconds(2));

			Assert.NotSame(initialValue, cut.Nodes);
		}

		[Fact(DisplayName = "WaitForRender throws WaitForRenderFailedException when a render does not happen within the timeout period")]
		[Obsolete("Calls to WaitForNextRender is obsolete, but still needs tests")]
		public void Test006()
		{
			const string expectedMessage = "No render happened before the timeout period passed.";
			var cut = RenderComponent<Simple1>();

			var expected = Should.Throw<WaitForRenderFailedException>(() =>
				WaitForNextRender(timeout: TimeSpan.FromMilliseconds(10))
			);

			expected.Message.ShouldBe(expectedMessage);
		}

		[Fact(DisplayName = "WaitForAssertion can wait for multiple renders and changes to occur")]
		public void Test110()
		{
			// Initial state is stopped
			var cut = RenderComponent<TwoRendersTwoChanges>();
			var stateElement = cut.Find("#state");
			stateElement.TextContent.ShouldBe("Stopped");

			// Clicking 'tick' changes the state, and starts a task
			cut.Find("#tick").Click();
			cut.Find("#state").TextContent.ShouldBe("Started");

			// Clicking 'tock' completes the task, which updates the state
			// This click causes two renders, thus something is needed to await here.
			cut.Find("#tock").Click();
			cut.WaitForAssertion(
				() => cut.Find("#state").TextContent.ShouldBe("Stopped")
			);
		}

		[Fact(DisplayName = "WaitForAssertion throws verification exception after timeout")]
		public void Test011()
		{
			const string expectedMessage = "The assertion did not pass within the timeout period.";
			var cut = RenderComponent<Simple1>();

			var expected = Should.Throw<WaitForAssertionFailedException>(() =>
			  cut.WaitForAssertion(() => cut.Markup.ShouldBeEmpty(), TimeSpan.FromMilliseconds(10))
			);
			expected.Message.ShouldBe(expectedMessage);
			expected.InnerException.ShouldBeOfType<ShouldAssertException>();
		}

		[Fact(DisplayName = "WaitForState throws WaitForRenderFailedException exception after timeout")]
		public void Test012()
		{
			const string expectedMessage = "The state predicate did not pass before the timeout period passed.";
			var cut = RenderComponent<Simple1>();

			var expected = Should.Throw<WaitForStateFailedException>(() =>
				cut.WaitForState(() => string.IsNullOrEmpty(cut.Markup), TimeSpan.FromMilliseconds(100))
			);

			expected.Message.ShouldBe(expectedMessage);
			expected.InnerException.ShouldBeOfType<TimeoutException>()
				.Message.ShouldBe(expectedMessage);
		}

		[Fact(DisplayName = "WaitForState throws WaitForRenderFailedException exception if statePredicate throws on a later render")]
		public void Test013()
		{
			const string expectedMessage = "The state predicate throw an unhandled exception.";
			const string expectedInnerMessage = "INNER MESSAGE";
			var cut = RenderComponent<TwoRendersTwoChanges>();
			cut.Find("#tick").Click();
			cut.Find("#tock").Click();

			var expected = Should.Throw<WaitForStateFailedException>(() =>
				cut.WaitForState(() =>
				{
					if (cut.Find("#state").TextContent == "Stopped")
						throw new InvalidOperationException(expectedInnerMessage);
					return false;
				})
			);

			expected.Message.ShouldBe(expectedMessage);
			expected.InnerException.ShouldBeOfType<InvalidOperationException>()
				.Message.ShouldBe(expectedInnerMessage);
		}

		[Fact(DisplayName = "WaitForState can wait for multiple renders and changes to occur")]
		public void Test100()
		{
			_testOutput.WriteLine($"INIT TEST100: {Thread.GetCurrentProcessorId()} - {Thread.CurrentThread.ManagedThreadId}");
			// Initial state is stopped
			var cut = RenderComponent<TwoRendersTwoChanges>();
			var stateElement = cut.Find("#state");
			stateElement.TextContent.ShouldBe("Stopped");

			// Clicking 'tick' changes the state, and starts a task
			cut.Find("#tick").Click();
			cut.Find("#state").TextContent.ShouldBe("Started");

			// Clicking 'tock' completes the task, which updates the state
			// This click causes two renders, thus something is needed to await here.
			cut.Find("#tock").Click();
			_testOutput.WriteLine($"BEFORE WAIT FOR STATE TEST100: {Thread.GetCurrentProcessorId()} - {Thread.CurrentThread.ManagedThreadId}");
			cut.WaitForState(() => cut.Find("#state").TextContent == "Stopped");
			_testOutput.WriteLine($"AFTER WAIT FOR STATE TEST100: {Thread.GetCurrentProcessorId()} - {Thread.CurrentThread.ManagedThreadId}");
			cut.Find("#state").TextContent.ShouldBe("Stopped");
			_testOutput.WriteLine($"END TEST100: {Thread.GetCurrentProcessorId()} - {Thread.CurrentThread.ManagedThreadId}");
		}
	}
}
