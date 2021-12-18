using Bunit.RazorTesting;

namespace Bunit;

/// <summary>
/// Represents a component that can be added inside a <see cref="RazorTestBase"/>,
/// where a component under test can be defined as the child content.
/// </summary>
public class ComponentUnderTest : FragmentBase
{
	/// <inheritdoc />
	public override Task SetParametersAsync(ParameterView parameters) => base.SetParametersAsync(parameters);
}
