#if NET5_0_OR_GREATER

using Bunit.Rendering;
using Bunit.TestAssets.SampleComponents.DisposeComponents;
#pragma warning disable CA2252

namespace Bunit;

public partial class TestContextBaseTest : TestContext
{
	[Fact(DisplayName = "ComponentFactories CanCreate() method are checked during component instantiation")]
	public void Test0001()
	{
		var mock = CreateMockComponentFactory(canCreate: _ => false, create: _ => null);
		ComponentFactories.Add(mock.Object);

		RenderComponent<Simple1>();

		mock.Verify(x => x.CanCreate(typeof(Simple1)), Times.Once);
		mock.Verify(x => x.Create(It.IsAny<Type>()), Times.Never);
	}

	[Fact(DisplayName = "ComponentFactories Create() method is called when their CanCreate() method returns true")]
	public void Test0002()
	{
		var mock = CreateMockComponentFactory(canCreate: _ => true, create: _ => new Simple1());
		ComponentFactories.Add(mock.Object);

		RenderComponent<Simple1>();

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

		RenderComponent<Simple1>();

		firstMock.Verify(x => x.CanCreate(It.IsAny<Type>()), Times.Never);
		firstMock.Verify(x => x.Create(It.IsAny<Type>()), Times.Never);
		secondMock.Verify(x => x.CanCreate(typeof(Simple1)), Times.Once);
		secondMock.Verify(x => x.Create(typeof(Simple1)), Times.Once);
	}

	[Fact(DisplayName = "DisposeComponents captures exceptions from DisposeAsync in Renderer.UnhandledException")]
	[Trait("Category", "sync")]
	public async Task Test201_Sync()
	{
		RenderComponent<AsyncThrowExceptionComponent>();

		DisposeComponents();

		var exception = await Renderer.UnhandledException;
		exception.ShouldBeOfType<NotSupportedException>();
	}

	[Fact(DisplayName = "DisposeComponents captures exceptions from DisposeAsync in Renderer.UnhandledException")]
	[Trait("Category", "async")]
	public async Task Test201()
	{
		RenderComponent<AsyncThrowExceptionComponent>();

		await DisposeComponentsAsync();

		var exception = await Renderer.UnhandledException;
		exception.ShouldBeOfType<NotSupportedException>();
	}

	[Fact(DisplayName = "DisposeComponents calls DisposeAsync on rendered components")]
	[Trait("Category", "sync")]
	public async Task Test202_Sync()
	{
		var cut = RenderComponent<AsyncDisposableComponent>();
		var wasDisposedTask = cut.Instance.DisposedTask;

		DisposeComponents();

		await wasDisposedTask.ShouldCompleteWithin(TimeSpan.FromMilliseconds(100));
	}

	[Fact(DisplayName = "DisposeComponents calls DisposeAsync on rendered components")]
	[Trait("Category", "async")]
	public async Task Test202()
	{
		var cut = RenderComponent<AsyncDisposableComponent>();
		var wasDisposedTask = cut.Instance.DisposedTask;

		await DisposeComponentsAsync();

		await wasDisposedTask.ShouldCompleteWithin(TimeSpan.FromMilliseconds(100));
	}

	[Fact(DisplayName = "DisposeComponents should dispose components added via ComponentFactory")]
	[Trait("Category", "sync")]
	public void Test203_Sync()
	{
		ComponentFactories.Add<ChildDispose, MyChildDisposeStub>();
		var cut = RenderComponent<ParentDispose>(ps => ps.Add(p => p.CallStack, new List<string>()));
		var instance = cut.FindComponent<MyChildDisposeStub>().Instance;

		DisposeComponents();

		instance.WasDisposed.ShouldBeTrue();
	}

	[Fact(DisplayName = "DisposeComponents should dispose components added via ComponentFactory")]
	[Trait("Category", "sync")]
	public async Task Test203()
	{
		ComponentFactories.Add<ChildDispose, MyChildDisposeStub>();
		var cut = RenderComponent<ParentDispose>(ps => ps.Add(p => p.CallStack, new List<string>()));
		var instance = cut.FindComponent<MyChildDisposeStub>().Instance;

		await DisposeComponentsAsync();

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
}
#endif
