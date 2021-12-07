#if NET5_0_OR_GREATER
namespace Bunit;

public partial class TestServiceProviderTest
{
	[Fact(DisplayName = "Can correctly resolve and dispose of scoped disposable service")]
	public void Net5Test001()
	{
		var sut = new TestServiceProvider();
		sut.AddScoped<AsyncDisposableService>();
		var asyncDisposable = sut.GetService<AsyncDisposableService>();

		sut.Dispose();

		asyncDisposable.IsDisposed.ShouldBeTrue();
	}

	[Fact(DisplayName = "Can correctly resolve and dispose of transient disposable service")]
	public void Net5Test002()
	{
		var sut = new TestServiceProvider();
		sut.AddTransient<AsyncDisposableService>();
		var asyncDisposable = sut.GetService<AsyncDisposableService>();

		sut.Dispose();

		asyncDisposable.IsDisposed.ShouldBeTrue();
	}

	[Fact(DisplayName = "Can correctly resolve and dispose of singleton disposable service")]
	public void Net5Test003()
	{
		var sut = new TestServiceProvider();
		sut.AddSingleton<AsyncDisposableService>();
		var asyncDisposable = sut.GetService<AsyncDisposableService>();

		sut.Dispose();

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
}
#endif
