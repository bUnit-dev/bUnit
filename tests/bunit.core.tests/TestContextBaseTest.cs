using Bunit.TestAssets.SampleComponents.DisposeComponents;

namespace Bunit;

public partial class TestContextBaseTest : TestContext
{
	[Fact(DisplayName = "DisposeComponents disposes rendered components in parent to child order")]
	public void Test101()
	{
		var callStack = new List<string>();
		RenderComponent<ParentDispose>(ps => ps.Add(p => p.CallStack, callStack));

		DisposeComponents();

		callStack.Count.ShouldBe(2);
		callStack[0].ShouldBe("ParentDispose");
		callStack[1].ShouldBe("ChildDispose");
	}

	[Fact(DisplayName = "DisposeComponents disposes multiple rendered components")]
	public void Test102()
	{
		var callStack = new List<string>();
		RenderComponent<ChildDispose>(ps => ps.Add(p => p.CallStack, callStack));
		RenderComponent<ChildDispose>(ps => ps.Add(p => p.CallStack, callStack));

		DisposeComponents();

		callStack.Count.ShouldBe(2);
	}

	[Fact(DisplayName = "DisposeComponents rethrows exceptions from Dispose methods in components")]
	public void Test103()
	{
		RenderComponent<ThrowExceptionComponent>();
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

	private sealed class ThrowExceptionComponent : ComponentBase, IDisposable
	{
		public void Dispose()
		{
#pragma warning disable S3877
			throw new NotSupportedException();
#pragma warning restore S3877
		}
	}
}
