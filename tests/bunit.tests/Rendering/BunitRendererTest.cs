using Bunit.Extensions;
using Xunit.Abstractions;

namespace Bunit.Rendering;

public class BunitRendererTest : TestContext
{
	public BunitRendererTest(ITestOutputHelper outputHelper)
	{
		DefaultWaitTimeout = TimeSpan.FromSeconds(30);
		Services.AddXunitLogger(outputHelper);
	}

	[Fact(DisplayName = "RenderFragment re-throws exception from component")]
	public void Test004()
	{
		var sut = Services.GetRequiredService<BunitRenderer>();
		RenderFragment throwingFragment = b => { b.OpenComponent<ThrowsDuringSetParams>(0); b.CloseComponent(); };

		Should.Throw<InvalidOperationException>(() => sut.RenderFragment(throwingFragment))
			.Message.ShouldBe(ThrowsDuringSetParams.EXCEPTION.Message);
	}

	[Fact(DisplayName = "RenderComponent re-throws exception from component")]
	public void Test003()
	{
		var sut = Services.GetRequiredService<BunitRenderer>();

		Should.Throw<InvalidOperationException>(() => sut.Render<ThrowsDuringSetParams>())
			.Message.ShouldBe(ThrowsDuringSetParams.EXCEPTION.Message);
	}

	[Fact(DisplayName = "Can render fragment without children and no parameters")]
	public void Test001()
	{
		const string MARKUP = "<h1>hello world</h1>";
		var sut = Services.GetRequiredService<BunitRenderer>();

		var cut = sut.RenderFragment(builder => builder.AddMarkupContent(0, MARKUP));

		cut.RenderCount.ShouldBe(1);
		cut.Markup.ShouldBe(MARKUP);
	}

	[Fact(DisplayName = "Can render component without children and no parameters")]
	public void Test002()
	{
		var sut = Services.GetRequiredService<BunitRenderer>();

		var cut = sut.Render<NoChildNoParams>();

		cut.RenderCount.ShouldBe(1);
		cut.Markup.ShouldBe(NoChildNoParams.MARKUP);
		cut.AccessInstance(c => c.ShouldBeOfType<NoChildNoParams>());
	}

	[Fact(DisplayName = "Can render component with parameters")]
	public void Test005()
	{
		const string VALUE = "FOO BAR";
		var sut = Services.GetRequiredService<BunitRenderer>();

		var cut = sut.Render<HasParams>(ps => ps
			.Add(p => p.Value, VALUE));

		cut.AccessInstance(c => c.Value.ShouldBe(VALUE));
	}

	[Fact(DisplayName = "Can render component with child component")]
	public void Test006()
	{
		const string PARENT_VALUE = "PARENT";
		const string CHILD_VALUE = "CHILD";

		var sut = Services.GetRequiredService<BunitRenderer>();

		var cut = sut.Render<HasParams>(ps => ps
			.Add(p => p.Value, PARENT_VALUE)
			.AddChildContent<HasParams>(pps => pps.Add(p => p.Value, CHILD_VALUE)));

		cut.Markup.ShouldStartWith(PARENT_VALUE);
		cut.Markup.ShouldEndWith(CHILD_VALUE);
	}

	[Fact(DisplayName = "Rendered component gets RenderCount updated on re-render")]
	public void Test010()
	{
		var sut = Services.GetRequiredService<BunitRenderer>();

		var cut = sut.Render<RenderTrigger>();

		cut.RenderCount.ShouldBe(1);

		cut.AccessInstance(c => c.Trigger());

		cut.RenderCount.ShouldBe(2);
	}

	[Fact(DisplayName = "Rendered component gets Markup updated on re-render")]
	public void Test011()
	{
		// arrange
		const string EXPECTED = "NOW VALUE";
		var sut = Services.GetRequiredService<BunitRenderer>();
		var cut = sut.Render<RenderTrigger>();

		cut.RenderCount.ShouldBe(1);

		// act
		cut.AccessInstance(c => c.TriggerWithValue(EXPECTED));

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
		var sut = Services.GetRequiredService<BunitRenderer>();
		var cut = sut.Render<HasParams>(ps => ps
			.Add(p => p.Value, PARENT_VALUE)
			.AddChildContent<HasParams>(pps => pps.Add(p => p.Value, CHILD_VALUE)));

		// act
		var childCut = sut.FindComponent<HasParams>(cut);

		// assert
		childCut.Markup.ShouldBe(CHILD_VALUE);
		childCut.RenderCount.ShouldBe(1);
	}

	[Fact(DisplayName = "FindComponent throws if parentComponent parameter is null")]
	public void Test021()
	{
		var sut = Services.GetRequiredService<BunitRenderer>();

		Should.Throw<ArgumentNullException>(() => sut.FindComponent<HasParams>(null!));
	}

	[Fact(DisplayName = "FindComponent throws if component is not found")]
	public void Test022()
	{
		var sut = Services.GetRequiredService<BunitRenderer>();
		var cut = sut.Render<HasParams>();

		Should.Throw<ComponentNotFoundException>(() => sut.FindComponent<HasParams>(cut));
	}

	[Fact(DisplayName = "FindComponent returns same rendered component when called multiple times")]
	public void Test023()
	{
		var sut = Services.GetRequiredService<BunitRenderer>();
		var cut = sut.Render<HasParams>(ps => ps
			.AddChildContent<HasParams>());

		var child1 = sut.FindComponent<HasParams>(cut);
		var child2 = sut.FindComponent<HasParams>(cut);

		child1.ShouldBe(child2);
	}

	[Fact(DisplayName = "FindComponents returns all components nested inside another rendered component")]
	public void Test030()
	{
		// arrange
		const string GRAND_PARENT_VALUE = nameof(GRAND_PARENT_VALUE);
		const string PARENT_VALUE = nameof(PARENT_VALUE);
		const string CHILD_VALUE = nameof(CHILD_VALUE);

		var sut = Services.GetRequiredService<BunitRenderer>();
		var cut = sut.Render<HasParams>(ps => ps
			.Add(p => p.Value, GRAND_PARENT_VALUE)
			.AddChildContent<HasParams>(pps => pps
				.Add(p => p.Value, PARENT_VALUE)
				.AddChildContent<HasParams>(ppps => ppps
					.Add(p=>p.Value, CHILD_VALUE))));

		// act
		var childCuts = sut.FindComponents<HasParams>(cut)
			.OfType<IRenderedComponent<HasParams>>()
			.ToArray();

		// assert
		childCuts[0].Markup.ShouldBe(PARENT_VALUE + CHILD_VALUE);
		childCuts[0].RenderCount.ShouldBe(1);

		childCuts[1].Markup.ShouldBe(CHILD_VALUE);
		childCuts[1].RenderCount.ShouldBe(1);
	}

	[Fact(DisplayName = "FindComponents throws if parentComponent parameter is null")]
	public void Test031()
	{
		var sut = Services.GetRequiredService<BunitRenderer>();

		Should.Throw<ArgumentNullException>(() => sut.FindComponents<HasParams>(null!));
	}

	[Fact(DisplayName = "FindComponents returns same rendered components when called multiple times")]
	public void Test032()
	{
		// arrange
		var sut = Services.GetRequiredService<BunitRenderer>();
		var cut = sut.Render<HasParams>(ps => ps
			.AddChildContent<HasParams>(pps => pps
				.AddChildContent<HasParams>()));

		// act
		var childCuts1 = sut.FindComponents<HasParams>(cut);
		var childCuts2 = sut.FindComponents<HasParams>(cut);

		// assert
		childCuts1.ShouldBe(childCuts2);
	}

	[Fact(DisplayName = "Retrieved rendered child component with FindComponent gets updated on re-render")]
	public void Test040()
	{
		var sut = Services.GetRequiredService<BunitRenderer>();

		var parent = sut.Render<HasParams>(ps => ps
			.AddChildContent<RenderTrigger>());

		// act
		var cut = sut.FindComponent<RenderTrigger>(parent);

		cut.RenderCount.ShouldBe(1);

		cut.AccessInstance(c => c.TriggerWithValue("X"));

		cut.RenderCount.ShouldBe(2);
		cut.Markup.ShouldBe("X");
	}

	[Fact(DisplayName = "Retrieved rendered child component with FindComponents gets updated on re-render")]
	public void Test041()
	{
		var sut = Services.GetRequiredService<BunitRenderer>();

		var parent = sut.Render<HasParams>(ps => ps
			.AddChildContent<RenderTrigger>());

		// act
		var cut = sut.FindComponents<RenderTrigger>(parent).Single();

		cut.RenderCount.ShouldBe(1);

		cut.AccessInstance(c => c.TriggerWithValue("X"));

		cut.RenderCount.ShouldBe(2);
		cut.Markup.ShouldBe("X");
	}

	[Fact(DisplayName = "Rendered component updates on re-renders from child components with changes in render tree")]
	public void Test050()
	{
		// arrange
		var sut = Services.GetRequiredService<BunitRenderer>();

		var cut = sut.Render<HasParams>(ps => ps
			.AddChildContent<RenderTrigger>());
		var child = sut.FindComponent<RenderTrigger>(cut);

		// act
		child.AccessInstance(c => c.TriggerWithValue("X"));

		// assert
		cut.RenderCount.ShouldBe(2);
		cut.Markup.ShouldBe("X");
	}

	[Fact(DisplayName = "When component is disposed by renderer, getting Markup throws and IsDisposed returns true")]
	public void Test060()
	{
		// arrange
		var sut = Services.GetRequiredService<BunitRenderer>();

		var cut = sut.Render<ToggleChild>(ps => ps
			.AddChildContent<NoChildNoParams>());
		var child = sut.FindComponent<NoChildNoParams>(cut);

		// act
		cut.AccessInstance(c => c.DisposeChild());

		// assert
		child.IsDisposed.ShouldBeTrue();
		Should.Throw<ComponentDisposedException>(() => child.Markup);
	}

	[Fact(DisplayName = "Rendered component updates itself if a child's child is disposed")]
	public void Test061()
	{
		// arrange
		var sut = Services.GetRequiredService<BunitRenderer>();

		var cut = sut.Render<ToggleChild>(ps => ps
			.AddChildContent<ToggleChild>(pps => pps
				.AddChildContent<NoChildNoParams>()));
		var child = sut.FindComponent<ToggleChild>(cut);
		var childChild = sut.FindComponent<NoChildNoParams>(cut);

		// act
		child.AccessInstance(c => c.DisposeChild());

		// assert
		childChild.IsDisposed.ShouldBeTrue();
		cut.Markup.ShouldBe(string.Empty);
	}

	[Fact(DisplayName = "When test renderer is disposed, so is all rendered components")]
	public void Test070()
	{
		var sut = Services.GetRequiredService<BunitRenderer>();
		var cut = sut.Render<NoChildNoParams>();

		sut.Dispose();

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
	public async Task Test101()
	{
		var cut = Render<AsyncRenderOfSubComponentDuringInit>(parameters =>
			parameters.Add(p => p.EitherOr, Task.Delay(1)));

		await cut.WaitForAssertionAsync(() => cut.Find("h1").TextContent.ShouldBe("SECOND"));
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
	public void Test203()
	{
		// Arrange
		var planned = JSInterop.SetupVoid("foo");
		Render<AsyncAfterRenderThrows>();

		// Act
		planned.SetVoidResult(); // <-- After here the `OnAfterRenderAsync` progresses and throws an exception.

		// Assert
		planned.VerifyInvoke("foo");
		Renderer.UnhandledException.Result.ShouldBeOfType<InvalidOperationException>();
	}

	[Fact(DisplayName = "Can render components that have a RenderMode attribute")]
	public void Test204()
	{
		var cut = Render<RenderModeServerComponent>();

		cut.Find("h3").TextContent.ShouldBe("Hello from Server");
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
		cut.AccessInstance(c =>
		{
			c.InitilizedCount.ShouldBe(1);
			c.InitilizedAsyncCount.ShouldBe(1);
			c.ParametersSetCount.ShouldBe(2);
			c.ParametersSetAsyncCount.ShouldBe(2);
			c.AfterRenderCount.ShouldBe(2);
			c.AfterRenderAsyncCount.ShouldBe(2);
		});
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

	private sealed class ThrowsDuringSetParams : ComponentBase
	{
		public static readonly InvalidOperationException EXCEPTION = new("THROWS ON PURPOSE");

		public override Task SetParametersAsync(ParameterView parameters) => throw EXCEPTION;
	}

	private sealed class HasParams : ComponentBase
	{
		[Parameter] public string? Value { get; set; }
		[Parameter] public RenderFragment? ChildContent { get; set; }

		protected override void BuildRenderTree(RenderTreeBuilder builder)
		{
			builder.AddMarkupContent(0, Value);
			builder.AddContent(1, ChildContent);
		}
	}

	private sealed class RenderTrigger : ComponentBase
	{
		[Parameter] public string? Value { get; set; }

		public void Trigger() => InvokeAsync(StateHasChanged);

		public void TriggerWithValue(string value)
		{
			Value = value;
			InvokeAsync(StateHasChanged);
		}

		protected override void BuildRenderTree(RenderTreeBuilder builder)
		{
			builder.AddMarkupContent(0, Value);
		}
	}

	private sealed class ToggleChild : ComponentBase
	{
		private bool showing = true;

		[Parameter] public RenderFragment? ChildContent { get; set; }

		public void DisposeChild()
		{
			showing = false;
			InvokeAsync(StateHasChanged);
		}

		protected override void BuildRenderTree(RenderTreeBuilder builder)
		{
			if (showing)
				builder.AddContent(0, ChildContent);
		}
	}

	private sealed class SyncOperationThrows : ComponentBase
	{
		public bool AwaitDone { get; private set; }

		protected override void OnInitialized()
			=> throw new SyncOperationThrowsException();

		internal sealed class SyncOperationThrowsException : Exception { }
	}

	private sealed class AsyncOperationThrows : ComponentBase
	{
		[Parameter] public Task Awaitable { get; set; }

		protected override async Task OnInitializedAsync()
		{
			await Awaitable;
		}

		internal sealed class AsyncOperationThrowsException : Exception { }
	}

	private sealed class AsyncAfterRenderThrows : ComponentBase
	{
		[Inject] private IJSRuntime JSRuntime { get; set; }

		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			await JSRuntime.InvokeVoidAsync("foo");
			throw new InvalidOperationException();
		}
	}

	[RenderModeServer]
	private sealed class RenderModeServerComponent : ComponentBase
	{
		protected override void BuildRenderTree(RenderTreeBuilder builder)
		{
			builder.AddMarkupContent(0, "<h3>Hello from Server</h3>");
		}
	}
}
