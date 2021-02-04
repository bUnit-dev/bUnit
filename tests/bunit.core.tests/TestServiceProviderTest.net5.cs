#if NET5_0
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit;

namespace Bunit
{
	public partial class TestServiceProviderTest
	{
		[Fact(DisplayName = "Can correctly dispose of async disposable service")]
		[SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope", Justification = "Point of test is to verify explicit call to Dispose doesn't throw.")]
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
