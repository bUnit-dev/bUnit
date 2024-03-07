using Bunit.Extensions;
using Bunit.TestAssets.RenderModes;

namespace Bunit.Rendering;

public class BunitRendererTests : TestContext
{
	public BunitRendererTests(ITestOutputHelper outputHelper)
	{
		DefaultWaitTimeout = TimeSpan.FromSeconds(30);
		Services.AddXunitLogger(outputHelper);
	}

	[Fact(DisplayName = "RenderFragment re-throws exception from component")]
	public void Test004()
	{
		RenderFragment throwingFragment = b => { b.OpenComponent<ThrowsDuringSetParams>(0); b.CloseComponent(); };

		Should.Throw<InvalidOperationException>(() => Renderer.RenderFragment(throwingFragment))
			.Message.ShouldBe(ThrowsDuringSetParams.EXCEPTION.Message);
	}

	[Fact(DisplayName = "Render re-throws exception from component")]
	public void Test003()
	{
		Should.Throw<InvalidOperationException>(() => Renderer.Render<ThrowsDuringSetParams>())
			.Message.ShouldBe(ThrowsDuringSetParams.EXCEPTION.Message);
	}

	[Fact(DisplayName = "Can render fragment without children and no parameters")]
	public void Test001()
	{
		const string MARKUP = "<h1>hello world</h1>";

		var cut = Renderer.RenderFragment(builder => builder.AddMarkupContent(0, MARKUP));

		cut.RenderCount.ShouldBe(1);
		cut.Markup.ShouldBe(MARKUP);
	}

	[Fact(DisplayName = "Can render component without children and no parameters")]
	public void Test002()
	{
		var cut = Renderer.Render<NoChildNoParams>();

		cut.RenderCount.ShouldBe(1);
		cut.Markup.ShouldBe(NoChildNoParams.MARKUP);
		cut.Instance.ShouldBeOfType<NoChildNoParams>();
	}

	[Fact(DisplayName = "Can render component with parameters")]
	public void Test005()
	{
		const string VALUE = "FOO BAR";

		var cut = Renderer.Render<HasParams>(ps => ps.Add(p => p.Value, VALUE));

		cut.Instance.Value.ShouldBe(VALUE);
	}

	[Fact(DisplayName = "Can render component with child component")]
	public void Test006()
	{
		const string PARENT_VALUE = "PARENT";
		const string CHILD_VALUE = "CHILD";

		var cut = Renderer.Render<HasParams>(ps => ps
			.Add(p => p.Value, PARENT_VALUE)
			.AddChildContent<HasParams>(pps => pps.Add(p => p.Value, CHILD_VALUE)));

		cut.Markup.ShouldStartWith(PARENT_VALUE);
		cut.Markup.ShouldEndWith(CHILD_VALUE);
	}

	[Fact(DisplayName = "Rendered component gets RenderCount updated on re-render")]
	public async Task Test010()
	{
		var cut = Renderer.Render<RenderTrigger>();

		cut.RenderCount.ShouldBe(1);

		await cut.Instance.Trigger();

		cut.RenderCount.ShouldBe(2);
	}

	[Fact(DisplayName = "Rendered component gets Markup updated on re-render")]
	public async Task Test011()
	{
		// arrange
		const string EXPECTED = "NOW VALUE";
		var cut = Renderer.Render<RenderTrigger>();

		cut.RenderCount.ShouldBe(1);

		// act
		await cut.Instance.TriggerWithValue(EXPECTED);

		// assert
		cut.RenderCount.ShouldBe(2);
		cut.Markup.ShouldBe(EXPECTED);
	}

	[Fact(DisplayName = "FindComponent returns first component nested inside another rendered component")]
	public void Test020()
	{
		// arrange
		const string PARENT_VALUE = "PARENT";
		const string CHILD_VALUE = "CHILD";

		var cut = Renderer.Render<HasParams>(ps => ps
			.Add(p => p.Value, PARENT_VALUE)
			.AddChildContent<HasParams>(pps => pps.Add(p => p.Value, CHILD_VALUE)));

		// act
		var childCut = Renderer.FindComponent<HasParams>(cut);

		// assert
		childCut.Markup.ShouldBe(CHILD_VALUE);
		childCut.RenderCount.ShouldBe(1);
	}

	[Fact(DisplayName = "FindComponent throws if parentComponent parameter is null")]
	public void Test021()
	{
		Should.Throw<ArgumentNullException>(() => Renderer.FindComponent<HasParams>(null!));
	}

	[Fact(DisplayName = "FindComponent throws if component is not found")]
	public void Test022()
	{
		var cut = Renderer.Render<HasParams>();

		Should.Throw<ComponentNotFoundException>(() => Renderer.FindComponent<HasParams>(cut));
	}

	[Fact(DisplayName = "FindComponent returns same rendered component when called multiple times")]
	public void Test023()
	{
		var cut = Renderer.Render<HasParams>(ps => ps
			.AddChildContent<HasParams>());

		var child1 = Renderer.FindComponent<HasParams>(cut);
		var child2 = Renderer.FindComponent<HasParams>(cut);

		child1.ShouldBe(child2);
	}

	[Fact(DisplayName = "FindComponents returns all components nested inside another rendered component")]
	public void Test030()
	{
		// arrange
		const string GRAND_PARENT_VALUE = nameof(GRAND_PARENT_VALUE);
		const string PARENT_VALUE = nameof(PARENT_VALUE);
		const string CHILD_VALUE = nameof(CHILD_VALUE);

		var cut = Renderer.Render<HasParams>(ps => ps
			.Add(p => p.Value, GRAND_PARENT_VALUE)
			.AddChildContent<HasParams>(pps => pps
				.Add(p => p.Value, PARENT_VALUE)
				.AddChildContent<HasParams>(ppps => ppps
					.Add(p=>p.Value, CHILD_VALUE))));

		// act
		var childCuts = Renderer.FindComponents<HasParams>(cut)
			.OfType<RenderedComponent<HasParams>>()
			.ToList();

		// assert
		childCuts[0].Markup.ShouldBe(PARENT_VALUE + CHILD_VALUE);
		childCuts[0].RenderCount.ShouldBe(1);

		childCuts[1].Markup.ShouldBe(CHILD_VALUE);
		childCuts[1].RenderCount.ShouldBe(1);
	}

	[Fact(DisplayName = "FindComponents throws if parentComponent parameter is null")]
	public void Test031()
	{
		Should.Throw<ArgumentNullException>(() => Renderer.FindComponents<HasParams>(null!));
	}

	[Fact(DisplayName = "FindComponents returns same rendered components when called multiple times")]
	public void Test032()
	{
		// arrange
		var cut = Renderer.Render<HasParams>(ps => ps
			.AddChildContent<HasParams>(pps => pps
				.AddChildContent<HasParams>()));

		// act
		var childCuts1 = Renderer.FindComponents<HasParams>(cut);
		var childCuts2 = Renderer.FindComponents<HasParams>(cut);

		// assert
		childCuts1.ShouldBe(childCuts2);
	}

	[Fact(DisplayName = "Retrieved rendered child component with FindComponent gets updated on re-render")]
	public async Task Test040()
	{
		var parent = Renderer.Render<HasParams>(ps => ps
			.AddChildContent<RenderTrigger>());

		// act
		var cut = Renderer.FindComponent<RenderTrigger>(parent);

		cut.RenderCount.ShouldBe(1);

		await cut.Instance.TriggerWithValue("X");

		cut.RenderCount.ShouldBe(2);
		cut.Markup.ShouldBe("X");
	}

	[Fact(DisplayName = "Retrieved rendered child component with FindComponents gets updated on re-render")]
	public async Task Test041()
	{
		var parent = Renderer.Render<HasParams>(ps => ps
			.AddChildContent<RenderTrigger>());

		// act
		var cut = Renderer.FindComponents<RenderTrigger>(parent).Single();

		cut.RenderCount.ShouldBe(1);

		await cut.Instance.TriggerWithValue("X");

		cut.RenderCount.ShouldBe(2);
		cut.Markup.ShouldBe("X");
	}

	[Fact(DisplayName = "Rendered component updates on re-renders from child components with changes in render tree")]
	public async Task Test050()
	{
		// arrange
		var cut = Renderer.Render<HasParams>(ps => ps
			.AddChildContent<RenderTrigger>());
		var child = Renderer.FindComponent<RenderTrigger>(cut);

		// act
		await child.Instance.TriggerWithValue("X");

		// assert
		cut.RenderCount.ShouldBe(2);
		cut.Markup.ShouldBe("X");
	}

	[Fact(DisplayName = "When component is disposed by renderer, getting Markup throws and IsDisposed returns true")]
	public async Task Test060()
	{
		// arrange
		var cut = Renderer.Render<ToggleChild>(ps => ps
			.AddChildContent<NoChildNoParams>());
		var child = Renderer.FindComponent<NoChildNoParams>(cut);

		// act
		await cut.Instance.DisposeChild();

		// assert
		child.IsDisposed.ShouldBeTrue();
		Should.Throw<ComponentDisposedException>(() => child.Markup);
	}

	[Fact(DisplayName = "Rendered component updates itself if a child's child is disposed")]
	public async Task Test061()
	{
		// arrange
		var cut = Renderer.Render<ToggleChild>(ps => ps
			.AddChildContent<ToggleChild>(pps => pps
				.AddChildContent<NoChildNoParams>()));
		var child = Renderer.FindComponent<ToggleChild>(cut);
		var childChild = Renderer.FindComponent<NoChildNoParams>(cut);

		// act
		await child.Instance.DisposeChild();

		// assert
		childChild.IsDisposed.ShouldBeTrue();
		cut.Markup.ShouldBe(string.Empty);
	}

	[Fact(DisplayName = "When test renderer is disposed, so is all rendered components")]
	public void Test070()
	{
		var cut = Renderer.Render<NoChildNoParams>();

		Renderer.Dispose();

		cut.IsDisposed.ShouldBeTrue();
	}

	[Fact(DisplayName = "Can render component that awaits uncompleted task in OnInitializedAsync")]
	public void Test100()
	{
		var tcs = new TaskCompletionSource<object>();

		var cut = Render<AsyncRenderOfSubComponentDuringInit>(parameters =>
			parameters.Add(p => p.EitherOr, tcs.Task));

		cut.Find("h1").TextContent.ShouldBe("FIRST");
	}

	[Fact(DisplayName = "Can render component that awaits yielding task in OnInitializedAsync")]
	[Trait("Category", "async")]
	public async Task Test101()
	{
		var cut = Render<AsyncRenderOfSubComponentDuringInit>(parameters =>
			parameters.Add(p => p.EitherOr, Task.Delay(1)));

		await cut.WaitForAssertionAsync(() => cut.Find("h1").TextContent.ShouldBe("SECOND"));
	}

	[Fact(DisplayName = "Can render component that awaits yielding task in OnInitializedAsync")]
	[Trait("Category", "sync")]
	public void Test101_Sync()
	{
		var cut = Render<AsyncRenderOfSubComponentDuringInit>(parameters =>
			parameters.Add(p => p.EitherOr, Task.Delay(1)));

		cut.WaitForAssertion(() => cut.Find("h1").TextContent.ShouldBe("SECOND"));
	}

	[Fact(DisplayName = "Can render component that awaits completed task in OnInitializedAsync")]
	public void Test102()
	{
		var cut = Render<AsyncRenderOfSubComponentDuringInit>(parameters =>
			parameters.Add(p => p.EitherOr, Task.CompletedTask));

		cut.Find("h1").TextContent.ShouldBe("SECOND");
	}

	[Fact(DisplayName = "UnhandledException has a reference to the exception thrown by component synchronously")]
	public async Task Test200()
	{
		var syncException = Should.Throw<SyncOperationThrows.SyncOperationThrowsException>(
			() => Render<SyncOperationThrows>());

		var capturedException = await Renderer.UnhandledException;
		capturedException.ShouldBe(syncException);
	}

	[Fact(DisplayName = "UnhandledException has a reference to the exception thrown by an async operation in a component")]
	public async Task Test201()
	{
		var tsc = new TaskCompletionSource<object>();
		var expectedException = new AsyncOperationThrows.AsyncOperationThrowsException();
		Render<AsyncOperationThrows>(ps => ps.Add(p => p.Awaitable, tsc.Task));

		tsc.SetException(expectedException);

		var actualException = await Renderer.UnhandledException;
		actualException.ShouldBe(expectedException);
	}

	[Fact(DisplayName = "UnhandledException has a reference to latest unhandled exception thrown by a component")]
	public async Task Test202()
	{
		var tsc1 = new TaskCompletionSource<object>();
		Render<AsyncOperationThrows>(ps => ps.Add(p => p.Awaitable, tsc1.Task));
		tsc1.SetException(new AsyncOperationThrows.AsyncOperationThrowsException());

		var firstExceptionReported = await Renderer.UnhandledException;

		var secondException = new AsyncOperationThrows.AsyncOperationThrowsException();
		var tsc2 = new TaskCompletionSource<object>();
		Render<AsyncOperationThrows>(ps => ps.Add(p => p.Awaitable, tsc2.Task));
		tsc2.SetException(secondException);

		var secondExceptionReported = await Renderer.UnhandledException;
		secondExceptionReported.ShouldBe(secondException);
		firstExceptionReported.ShouldNotBe(secondException);
	}

	[Fact(DisplayName = "UnhandledException has a reference to latest unhandled exception thrown by a component during OnAfterRenderAsync")]
	public async Task Test203()
	{
		// Arrange
		var planned = JSInterop.SetupVoid("foo");
		Render<AsyncAfterRenderThrows>();

		// Act
		planned.SetVoidResult(); // <-- After here the `OnAfterRenderAsync` progresses and throws an exception.

		// Assert
		planned.VerifyInvoke("foo");
		(await Renderer.UnhandledException).ShouldBeOfType<InvalidOperationException>();
	}

	[Fact(DisplayName = "Multiple calls to StateHasChanged from OnParametersSet with Render")]
	public void Test205()
	{
		var cut = Render<MultipleStateHasChangedInOnParametersSet>();
		cut.RenderCount.ShouldBe(1);

		cut.Render();
		cut.RenderCount.ShouldBe(2);
	}

	[Fact(DisplayName = "Multiple calls to StateHasChanged from OnParametersSet with event dispatch render trigger")]
	public void Test206()
	{
		var cut = Render<TriggerChildContentRerenderViaClick>();
		var child = cut.FindComponent<MultipleStateHasChangedInOnParametersSet>();
		child.RenderCount.ShouldBe(1);

		cut.Find("button").Click();

		child.RenderCount.ShouldBe(2);
	}

	[Fact(DisplayName = "Multiple calls to StateHasChanged from OnParametersSet with Render")]
	public void Test207()
	{
		var cut = Render<LifeCycleMethodInvokeCounter>();
		cut.RenderCount.ShouldBe(1);

		cut.Render();

		cut.RenderCount.ShouldBe(2);
		cut.Instance.InitilizedCount.ShouldBe(1);
		cut.Instance.InitilizedAsyncCount.ShouldBe(1);
		cut.Instance.ParametersSetCount.ShouldBe(2);
		cut.Instance.ParametersSetAsyncCount.ShouldBe(2);
		cut.Instance.AfterRenderCount.ShouldBe(2);
		cut.Instance.AfterRenderAsyncCount.ShouldBe(2);
	}

	[Fact(DisplayName = "Can render components that have a RenderMode InteractiveAuto")]
	public void Test209()
	{
		var cut = Render<InteractiveAutoComponent>();

		cut.Find("h1").TextContent.ShouldBe($"Hello world {RenderMode.InteractiveAuto}");
	}

	[Fact(DisplayName = "Can render components that have a RenderMode InteractiveServer")]
	public void Test210()
	{
		var cut = Render<InteractiveServerComponent>();

		cut.Find("h1").TextContent.ShouldBe($"Hello world {RenderMode.InteractiveServer}");
	}

	[Fact(DisplayName = "Can render components that have a RenderMode InteractiveWebAssembly")]
	public void Test211()
	{
		var cut = Render<InteractiveWebAssemblyComponent>();

		cut.Find("h1").TextContent.ShouldBe($"Hello world {RenderMode.InteractiveWebAssembly}");
	}

	internal sealed class LifeCycleMethodInvokeCounter : ComponentBase
	{
		public int InitilizedCount { get; private set; }

		public int InitilizedAsyncCount { get; private set; }

		public int ParametersSetCount { get; private set; }
		public int ParametersSetAsyncCount { get; private set; }
		public int AfterRenderCount { get; private set; }
		public int AfterRenderAsyncCount { get; private set; }

		protected override void OnInitialized()
			=> InitilizedCount++;

		protected override Task OnInitializedAsync()
		{
			InitilizedAsyncCount++;
			return Task.CompletedTask;
		}

		protected override void OnParametersSet()
			=> ParametersSetCount++;

		protected override Task OnParametersSetAsync()
		{
			ParametersSetAsyncCount++;
			return Task.CompletedTask;
		}

		protected override void OnAfterRender(bool firstRender)
			=> AfterRenderCount++;

		protected override Task OnAfterRenderAsync(bool firstRender)
		{
			AfterRenderAsyncCount++;
			return Task.CompletedTask;
		}
	}

	internal sealed class NoChildNoParams : ComponentBase
	{
		public const string MARKUP = "hello world";
		protected override void BuildRenderTree(RenderTreeBuilder builder) => builder.AddMarkupContent(0, MARKUP);
	}

	internal sealed class ThrowsDuringSetParams : ComponentBase
	{
		public static readonly InvalidOperationException EXCEPTION = new("THROWS ON PURPOSE");

		public override Task SetParametersAsync(ParameterView parameters) => throw EXCEPTION;
	}

	internal sealed class HasParams : ComponentBase
	{
		[Parameter] public string? Value { get; set; }
		[Parameter] public RenderFragment? ChildContent { get; set; }

		protected override void BuildRenderTree(RenderTreeBuilder builder)
		{
			builder.AddMarkupContent(0, Value);
			builder.AddContent(1, ChildContent);
		}
	}

	internal sealed class RenderTrigger : ComponentBase
	{
		[Parameter] public string? Value { get; set; }

		public Task Trigger() => InvokeAsync(StateHasChanged);

		public Task TriggerWithValue(string value)
		{
			Value = value;
			return InvokeAsync(StateHasChanged);
		}

		protected override void BuildRenderTree(RenderTreeBuilder builder)
		{
			builder.AddMarkupContent(0, Value);
		}
	}

	internal sealed class ToggleChild : ComponentBase
	{
		private bool showing = true;

		[Parameter] public RenderFragment? ChildContent { get; set; }

		public Task DisposeChild()
		{
			showing = false;
			return InvokeAsync(StateHasChanged);
		}

		protected override void BuildRenderTree(RenderTreeBuilder builder)
		{
			if (showing)
				builder.AddContent(0, ChildContent);
		}
	}

	internal sealed class SyncOperationThrows : ComponentBase
	{
		public bool AwaitDone { get; private set; }

		protected override void OnInitialized()
			=> throw new SyncOperationThrowsException();

		internal sealed class SyncOperationThrowsException : Exception { }
	}

	internal sealed class AsyncOperationThrows : ComponentBase
	{
		[Parameter] public Task Awaitable { get; set; }

		protected override async Task OnInitializedAsync()
		{
			await Awaitable;
		}

		internal sealed class AsyncOperationThrowsException : Exception { }
	}

	internal sealed class AsyncAfterRenderThrows : ComponentBase
	{
		[Inject] private IJSRuntime JSRuntime { get; set; }

		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			await JSRuntime.InvokeVoidAsync("foo");
			throw new InvalidOperationException();
		}
	}
}
