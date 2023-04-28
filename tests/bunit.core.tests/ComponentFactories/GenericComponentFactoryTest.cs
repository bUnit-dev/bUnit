namespace Bunit.ComponentFactories;

public class GenericComponentFactoryTest : TestContext
{
	[Fact(DisplayName = "Add throws when factories is null")]
	public void Test001()
		=> Should.Throw<ArgumentNullException>(() => ComponentFactoryCollectionExtensions.Add<Simple1, FakeSimple1>(factories: default));

	[Fact(DisplayName = "Add<TComponent, TReplacementComponent> replaces components of type TComponent with TReplacementComponent")]
	public void Test002()
	{
		ComponentFactories.Add<Simple1, FakeSimple1>();

		var cut = RenderComponent<RefToSimple1Child>();

		cut.MarkupMatches(@"<div id=""ref-status"">Has ref = True</div>");
	}

	private sealed class FakeSimple1 : Simple1
	{
		protected override void OnInitialized() { }
		protected override Task OnInitializedAsync() => Task.CompletedTask;
		public override Task SetParametersAsync(ParameterView parameters) => Task.CompletedTask;
		protected override void OnParametersSet() { }
		protected override Task OnParametersSetAsync() => Task.CompletedTask;
		protected override void BuildRenderTree(RenderTreeBuilder builder) { }
		protected override bool ShouldRender() => false;
		protected override void OnAfterRender(bool firstRender) { }
		protected override Task OnAfterRenderAsync(bool firstRender) => Task.CompletedTask;
	}
}
