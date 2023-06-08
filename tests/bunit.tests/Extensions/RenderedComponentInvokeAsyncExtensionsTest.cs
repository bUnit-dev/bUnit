namespace Bunit.Extensions;

public class RenderedComponentInvokeAsyncExtensionsTest : TestContext
{
	[Fact(DisplayName = "Dispatcher awaits Task-returning callback")]
	public async Task Test003()
	{
		// Arrange
		var cut = Render<Simple1>();
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

	[Fact(DisplayName = "Dispatcher does not await void-returning callback")]
	public async Task Test004()
	{
		// Arrange
		var cut = Render<Simple1>();
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
