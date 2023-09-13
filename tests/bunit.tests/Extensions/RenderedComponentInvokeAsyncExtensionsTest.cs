namespace Bunit.Extensions;

public class RenderedComponentInvokeAsyncExtensionsTest : TestContext
{
	[Fact(DisplayName = "Dispatcher does not await void-returning callback")]
	public async Task Test004()
	{
		// Arrange
		var cut = Render<Simple1>();
		bool delegateFinished = false;

		async void Callback()
		{
			await Task.Delay(20).ConfigureAwait(ConfigureAwaitOptions.ForceYielding);
			delegateFinished = true;
		}

		// Act
		await cut.InvokeAsync(Callback);

		// Assert
		delegateFinished.ShouldBeFalse();
	}
}
