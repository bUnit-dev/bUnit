using AngleSharp.Common;
using Bunit.TestAssets.SampleComponents.DisposeComponents;
using Bunit.TestDoubles;

namespace Bunit;

public class TestContextTest : TestContext
{
	[Fact(DisplayName = "DisposeComponents disposes rendered components in parent to child order")]
	public void Test101()
	{
		var callStack = new List<string>();
		Render<ParentDispose>(ps => ps.Add(p => p.CallStack, callStack));

		DisposeComponents();

		callStack.Count.ShouldBe(2);
		callStack[0].ShouldBe("ParentDispose");
		callStack[1].ShouldBe("ChildDispose");
	}

	[Fact(DisplayName = "DisposeComponents disposes multiple rendered components")]
	public void Test102()
	{
		var callStack = new List<string>();
		Render<ChildDispose>(ps => ps.Add(p => p.CallStack, callStack));
		Render<ChildDispose>(ps => ps.Add(p => p.CallStack, callStack));

		DisposeComponents();

		callStack.Count.ShouldBe(2);
	}

	[Fact(DisplayName = "DisposeComponents rethrows exceptions from Dispose methods in components")]
	public void Test103()
	{
		Render<ThrowExceptionComponent>();
		var action = () => DisposeComponents();

		action.ShouldThrow<NotSupportedException>();
	}

	[Fact(DisplayName = "DisposeComponents disposes components nested in render fragments")]
	public void Test104()
	{
		var callStack = new List<string>();
		Render(DisposeFragments.ChildDisposeAsFragment(callStack));

		DisposeComponents();

		callStack.Count.ShouldBe(1);
	}

	[Fact(DisplayName = "ComponentFactories CanCreate() method are checked during component instantiation")]
	public void Test0001()
	{
		var mock = CreateMockComponentFactory(canCreate: _ => false, create: _ => null);
		ComponentFactories.Add(mock.Object);

		Render<Simple1>();

		mock.Verify(x => x.CanCreate(typeof(Simple1)), Times.Once);
		mock.Verify(x => x.Create(It.IsAny<Type>()), Times.Never);
	}

	[Fact(DisplayName = "ComponentFactories Create() method is called when their CanCreate() method returns true")]
	public void Test0002()
	{
		var mock = CreateMockComponentFactory(canCreate: _ => true, create: _ => new Simple1());
		ComponentFactories.Add(mock.Object);

		Render<Simple1>();

		mock.Verify(x => x.CanCreate(typeof(Simple1)), Times.Once);
		mock.Verify(x => x.Create(typeof(Simple1)), Times.Once);
	}

	[Fact(DisplayName = "ComponentFactories is used in last added order")]
	public void Test0003()
	{
		var firstMock = CreateMockComponentFactory(canCreate: _ => true, create: _ => new Simple1());
		var secondMock = CreateMockComponentFactory(canCreate: _ => true, create: _ => new Simple1());
		ComponentFactories.Add(firstMock.Object);
		ComponentFactories.Add(secondMock.Object);

		Render<Simple1>();

		firstMock.Verify(x => x.CanCreate(It.IsAny<Type>()), Times.Never);
		firstMock.Verify(x => x.Create(It.IsAny<Type>()), Times.Never);
		secondMock.Verify(x => x.CanCreate(typeof(Simple1)), Times.Once);
		secondMock.Verify(x => x.Create(typeof(Simple1)), Times.Once);
	}

	[Fact(DisplayName = "DisposeComponents captures exceptions from DisposeAsync in Renderer.UnhandledException")]
	public async Task Test201()
	{
		Render<AsyncThrowExceptionComponent>();

		DisposeComponents();

		var exception = await Renderer.UnhandledException;
		exception.ShouldBeOfType<NotSupportedException>();
	}

	[Fact(DisplayName = "DisposeComponents calls DisposeAsync on rendered components")]
	public async Task Test202()
	{
		var cut = Render<AsyncDisposableComponent>();
		var wasDisposedTask = cut.Instance.DisposedTask;

		DisposeComponents();

		await wasDisposedTask.ShouldCompleteWithin(TimeSpan.FromMilliseconds(100));
	}

	[Fact(DisplayName = "DisposeComponents should dispose components added via ComponentFactory")]
	public void Test203()
	{
		ComponentFactories.Add<ChildDispose, MyChildDisposeStub>();
		var cut = Render<ParentDispose>(ps => ps.Add(p => p.CallStack, new List<string>()));
		var instance = cut.FindComponent<MyChildDisposeStub>().Instance;

		DisposeComponents();

		instance.WasDisposed.ShouldBeTrue();
	}

	[Fact(DisplayName = "Can correctly resolve and dispose of scoped disposable service")]
	public void Net5Test001()
	{
		AsyncDisposableService asyncDisposable;
		using (var sut = new TestContext())
		{
			sut.Services.AddScoped<AsyncDisposableService>();
			asyncDisposable = sut.Services.GetService<AsyncDisposableService>();
		}
		asyncDisposable.IsDisposed.ShouldBeTrue();
	}

	[Fact(DisplayName = "Can correctly resolve and dispose of transient disposable service")]
	public void Net5Test002()
	{
		AsyncDisposableService asyncDisposable;
		using (var sut = new TestContext())
		{
			sut.Services.AddTransient<AsyncDisposableService>();
			asyncDisposable = sut.Services.GetService<AsyncDisposableService>();
		}
		asyncDisposable.IsDisposed.ShouldBeTrue();
	}

	[Fact(DisplayName = "Can correctly resolve and dispose of singleton disposable service")]
	public void Net5Test003()
	{
		AsyncDisposableService asyncDisposable;
		using (var sut = new TestContext())
		{
			sut.Services.AddSingleton<AsyncDisposableService>();
			asyncDisposable = sut.Services.GetService<AsyncDisposableService>();
		}
		asyncDisposable.IsDisposed.ShouldBeTrue();
	}

	[Fact(DisplayName = "The test service provider should register a placeholder HttpClient which throws exceptions")]
	public void Test024()
	{
		Should.Throw<MissingMockHttpClientException>(() => Render<SimpleWithHttpClient>());
	}

	[Fact(DisplayName = "The test service provider should register a placeholder IStringLocalizer which throws exceptions")]
	public void Test026()
	{
		Should.Throw<MissingMockStringLocalizationException>(() => Render<SimpleUsingLocalizer>());
	}

	[Fact(DisplayName = "Render() renders fragment inside RenderTree")]
	public void Test030()
	{
		RenderTree.Add<CascadingValue<string>>(ps => ps.Add(p => p.Value, "FOO"));
		var cut = Render(b =>
		{
			b.OpenComponent<ReceivesCascadingValue>(0);
			b.CloseComponent();
		});

		cut.FindComponent<ReceivesCascadingValue>()
			.Instance
			.Value
			.ShouldBe("FOO");
	}

	[Fact(DisplayName = "Render<TComponent>() renders fragment inside RenderTreee")]
	public void Test031()
	{
		RenderTree.Add<CascadingValue<string>>(ps => ps.Add(p => p.Value, "FOO"));
		var cut = Render<ReceivesCascadingValue>(b =>
		{
			b.OpenComponent<ReceivesCascadingValue>(0);
			b.CloseComponent();
		});

		cut.Instance
			.Value
			.ShouldBe("FOO");
	}

	[Fact(DisplayName = "RenderComponent<TComponent>(builder) renders TComponent inside RenderTreee")]
	public void Test032()
	{
		RenderTree.Add<CascadingValue<string>>(ps => ps.Add(p => p.Value, "FOO"));
		var cut = Render<ReceivesCascadingValue>(ps => ps.Add(p => p.Dummy, null));

		cut.Instance
			.Value
			.ShouldBe("FOO");
	}

	[Fact(DisplayName = "RenderComponent<TComponent>(factories) renders TComponent inside RenderTreee")]
	public void Test033()
	{
		RenderTree.Add<CascadingValue<string>>(ps => ps.Add(p => p.Value, "FOO"));
		var cut = Render<ReceivesCascadingValue>(ps => ps
			.Add(p => p.Dummy, null!));

		cut.Instance
			.Value
			.ShouldBe("FOO");
	}

	[Fact(DisplayName = "Can raise events from markup rendered with TestContext")]
	public void Test040()
	{
		Should.NotThrow(() => Render<ClickCounter>().Find("button").Click());
	}

	private sealed class AsyncDisposableService : IAsyncDisposable
	{
		public bool IsDisposed { get; private set; }

		public ValueTask DisposeAsync()
		{
			IsDisposed = true;
			return ValueTask.CompletedTask;
		}
	}

	private sealed class MyChildDisposeStub : ComponentBase, IDisposable
	{
		public bool WasDisposed { get; private set; }

		public void Dispose()
		{
			WasDisposed = true;
		}
	}

	private sealed class AsyncThrowExceptionComponent : ComponentBase, IAsyncDisposable
	{
		public async ValueTask DisposeAsync()
		{
			await Task.Delay(30);
			throw new NotSupportedException();
		}
	}

	private sealed class AsyncDisposableComponent : ComponentBase, IAsyncDisposable
	{
		private readonly TaskCompletionSource tsc = new();

		public Task DisposedTask => tsc.Task;

		public async ValueTask DisposeAsync()
		{
			await Task.Delay(10);
			tsc.SetResult();
		}
	}

	private static Mock<IComponentFactory> CreateMockComponentFactory(Func<Type, bool> canCreate, Func<Type, IComponent> create)
	{
		var result = new Mock<IComponentFactory>(MockBehavior.Strict);
		result.Setup(x => x.CanCreate(It.IsAny<Type>())).Returns(canCreate);
		result.Setup(x => x.Create(It.IsAny<Type>())).Returns(create);
		return result;
	}

	private sealed class ThrowExceptionComponent : ComponentBase, IDisposable
	{
		public void Dispose()
		{
#pragma warning disable S3877
			throw new NotSupportedException();
#pragma warning restore S3877
		}
	}

	private sealed class ReceivesCascadingValue : ComponentBase
	{
		[CascadingParameter] public string? Value { get; set; }

		[Parameter] public object? Dummy { get; set; }
	}
}
