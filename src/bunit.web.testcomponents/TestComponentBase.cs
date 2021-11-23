using Xunit;

namespace Bunit;

/// <summary>
/// Base test class/test runner, that runs Fixtures defined in razor files.
/// </summary>
[SuppressMessage("Design", "CA1033:Interface methods should be callable by child types", Justification = "These interface methods should not be called by inheriting types.")]
public abstract class TestComponentBase : IComponent
{
	/// <summary>
	/// Renders the component to the supplied <see cref="RenderTreeBuilder"/>.
	/// </summary>
	/// <param name="builder">The builder to use for rendering.</param>
	protected virtual void BuildRenderTree(RenderTreeBuilder builder) { }

	/// <summary>
	/// Called by the XUnit test runner. Finds all Fixture components
	/// in the file and runs their associated tests.
	/// </summary>
	[SuppressMessage("Blocker Code Smell", "S2699:Tests should include assertions", Justification = "This is a placeholder for the sub classes to override, they should have assertions.")]
	[RazorTest]
	public virtual void RazorTests() { }

	void IComponent.Attach(RenderHandle renderHandle) => renderHandle.Render(BuildRenderTree);

	Task IComponent.SetParametersAsync(ParameterView parameters) => Task.CompletedTask;
}
