#if NET5_0
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit;

namespace Bunit
{
	public partial class TestServiceProviderTest
	{
		[Fact(DisplayName = "Can correctly dispose of async disposable service")]
		public void Net5Test001()
		{
			var sut = new TestServiceProvider();
			sut.AddScoped<AsyncDisposableService>();
			sut.GetService<AsyncDisposableService>();

			Should.NotThrow(() => sut.Dispose());
		}

		private class AsyncDisposableService : IAsyncDisposable
		{
			public ValueTask DisposeAsync() => ValueTask.CompletedTask;
		}
	}
}
#endif
