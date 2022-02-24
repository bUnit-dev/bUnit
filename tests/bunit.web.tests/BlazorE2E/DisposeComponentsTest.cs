using Bunit.TestAssets.SampleComponents.DisposeComponents;

namespace Bunit.BlazorE2E;

public class DisposeComponentsTest : TestContext
{
	[Fact]
	public void ShouldDisposeComponents()
	{
		RenderComponent<ParentDispose>();

		DisposeComponents();

		ParentDispose.CallStack.Count.ShouldBe(2);
		ParentDispose.CallStack[0].ShouldBe("ParentDispose");
		ParentDispose.CallStack[1].ShouldBe("ChildDispose");
	}

	[Fact]
	public void MultipleRenderComponentsShouldBeDisposed()
	{
		const string message = "Multi";
		RenderComponent<ChildDispose>(p => p.Add(c => c.MessageToAdd, message));
		RenderComponent<ChildDispose>(p => p.Add(c => c.MessageToAdd, message));

		DisposeComponents();

		ParentDispose.CallStack.Count(m => string.Equals(m, message, StringComparison.Ordinal)).ShouldBe(2);
	}

	[Fact]
	public void ExceptionThrownInDisposeShouldBeCatchable()
	{
		RenderComponent<ThrowExceptionComponent>();

		var act = DisposeComponents;

		act.ShouldThrow<NotSupportedException>();
	}

#if NET5_0_OR_GREATER
	[Fact]
	public void ExceptionThrownInDisposeAsyncShouldBeCatchable()
	{
		RenderComponent<AsyncThrowExceptionComponent>();

		DisposeComponents();

		var exception = Renderer.UnhandledException.Result;
		exception.ShouldNotBeNull();
		exception.ShouldBeOfType<NotSupportedException>();
	}

	[Fact]
	public void ShouldDisposeAsyncComponents()
	{
		var cut = RenderComponent<AsyncDisposableCompoent>();

		DisposeComponents();

		cut.WaitForAssertion(() => AsyncDisposableCompoent.WasDisposed.ShouldBeTrue(), TimeSpan.FromSeconds(1));
	}
#endif

	[Fact]
	public void ShouldDisposeRenderFragment()
	{
		Render(builder =>
		{
			builder.OpenComponent<ChildDispose>(0);
			builder.AddAttribute(1, "MessageToAdd", "RenderFragment");
			builder.CloseComponent();
		});

		DisposeComponents();

		ParentDispose.CallStack.Count(m => string.Equals(m, "RenderFragment", StringComparison.Ordinal)).ShouldBe(1);
	}


#if NET5_0_OR_GREATER
	[Fact]
	public void ComponentsAddedViaComponentFactoryShouldBeDisposed()
	{
		ComponentFactories.Add<ChildDispose, MyChildDisposeStub>();
		RenderComponent<ParentDispose>();

		DisposeComponents();

		MyChildDisposeStub.WasDisposed.ShouldBeTrue();
	}

	private sealed class MyChildDisposeStub : ComponentBase, IDisposable
	{
		public static bool WasDisposed;

		public void Dispose()
		{
			WasDisposed = true;
		}
	}
#endif

	private sealed class ThrowExceptionComponent : ComponentBase, IDisposable
	{
		public void Dispose()
		{
#pragma warning disable S3877
			throw new NotSupportedException();
#pragma warning restore S3877
		}
	}

	private sealed class AsyncThrowExceptionComponent : ComponentBase, IAsyncDisposable
	{
		public async ValueTask DisposeAsync()
		{
			await Task.Delay(10);
			throw new NotSupportedException();
		}
	}

	private sealed class AsyncDisposableCompoent : ComponentBase, IAsyncDisposable
	{
		public static bool WasDisposed;

		public async ValueTask DisposeAsync()
		{
			await Task.Delay(10);
			WasDisposed = true;
		}
	}
}