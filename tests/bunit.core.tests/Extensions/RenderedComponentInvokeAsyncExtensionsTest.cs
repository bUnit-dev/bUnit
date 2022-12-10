namespace Bunit.Extensions;

public class RenderedComponentInvokeAsyncExtensionsTest : TestContext
{
	[UIFact(DisplayName = "Dispatcher awaits Task-returning callback")]
	public async Task Test003()
	{
		// Arrange
		var cut = RenderComponent<Simple1>();
		bool delegateFinished = false;

		async Task Callback()
		{
			await Task.Delay(10);
			delegateFinished = true;
		}

		// Act
		await cut.InvokeAsync(Callback);

		// Assert
		delegateFinished.ShouldBeTrue();
	}

	[UIFact(DisplayName = "Dispatcher does not await void-returning callback")]
	public async Task Test004()
	{
		// Arrange
		var cut = RenderComponent<Simple1>();
		bool delegateFinished = false;

		async void Callback()
		{
			await Task.Delay(10);
			delegateFinished = true;
		}

		// Act
		await cut.InvokeAsync(Callback);

		// Assert
		delegateFinished.ShouldBeFalse();
	}
}
