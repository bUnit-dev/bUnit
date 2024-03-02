using Bunit.TestAssets.SampleComponents.DisposeComponents;

namespace Bunit;

public partial class TestContextBaseTest : TestContext
{
	[Fact(DisplayName = "ComponentFactories CanCreate() method are checked during component instantiation")]
	public void Test0001()
	{
		var mock = CreateMockComponentFactory(canCreate: _ => false, create: _ => null);
		ComponentFactories.Add(mock);

		RenderComponent<Simple1>();

		mock.Received(1).CanCreate(typeof(Simple1));
		mock.DidNotReceive().Create(Arg.Any<Type>());
	}

	[Fact(DisplayName = "ComponentFactories Create() method is called when their CanCreate() method returns true")]
	public void Test0002()
	{
		var mock = CreateMockComponentFactory(canCreate: _ => true, create: _ => new Simple1());
		ComponentFactories.Add(mock);

		RenderComponent<Simple1>();

		mock.Received(1).CanCreate(typeof(Simple1));
		mock.Received(1).Create(typeof(Simple1));
	}

	[Fact(DisplayName = "ComponentFactories is used in last added order")]
	public void Test0003()
	{
		var firstMock = CreateMockComponentFactory(canCreate: _ => true, create: _ => new Simple1());
		var secondMock = CreateMockComponentFactory(canCreate: _ => true, create: _ => new Simple1());
		ComponentFactories.Add(firstMock);
		ComponentFactories.Add(secondMock);

		RenderComponent<Simple1>();

		firstMock.DidNotReceive().CanCreate(Arg.Any<Type>());
		firstMock.DidNotReceive().Create(Arg.Any<Type>());
		secondMock.Received(1).CanCreate(typeof(Simple1));
		secondMock.Received(1).Create(typeof(Simple1));
	}

	[Fact(DisplayName = "DisposeComponents captures exceptions from DisposeAsync in Renderer.UnhandledException")]
	public async Task Test201()
	{
		var tcs = new TaskCompletionSource();
		var expected = new NotSupportedException();
		RenderComponent<AsyncThrowExceptionComponent>(
			ps => ps.Add(p => p.DisposedTask, tcs.Task));

		DisposeComponents();

		tcs.SetException(expected);
		var actual = await Renderer.UnhandledException;
		actual.ShouldBeSameAs(expected);
	}

	[Fact(DisplayName = "DisposeComponents calls DisposeAsync on rendered components")]
	public async Task Test202()
	{
		var cut = RenderComponent<AsyncDisposableComponent>();
		var wasDisposedTask = cut.Instance.DisposedTask;

		DisposeComponents();

		await wasDisposedTask.ShouldCompleteWithin(TimeSpan.FromSeconds(1));
	}

	[Fact(DisplayName = "DisposeComponents should dispose components added via ComponentFactory")]
	public void Test203()
	{
		ComponentFactories.Add<ChildDispose, MyChildDisposeStub>();
		var cut = RenderComponent<ParentDispose>(ps => ps.Add(p => p.CallStack, new List<string>()));
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
		[Parameter]
		public Task DisposedTask { get; set; }

		public async ValueTask DisposeAsync()
		{
			await DisposedTask;
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

	private static IComponentFactory CreateMockComponentFactory(Func<Type, bool> canCreate, Func<Type, IComponent> create)
	{
		var result = Substitute.For<IComponentFactory>();
		result.CanCreate(Arg.Any<Type>()).Returns(call => canCreate((Type)call[0]));
		result.Create(Arg.Any<Type>()).Returns(call => create((Type)call[0]));
		return result;
	}
}
