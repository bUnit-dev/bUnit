using System.Linq.Expressions;

namespace Bunit.TestAssets.SampleComponents;

public class FullBind : ComponentBase
{
	[Parameter] public string Foo { get; set; } = string.Empty;
	[Parameter] public EventCallback<string> FooChanged { get; set; }
	[Parameter] public Expression<Func<string>> FooExpression { get; set; }
}
