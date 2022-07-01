namespace Bunit.RenderingV2;

internal static class TestRendererV2Extensions
{
	public static IRenderedComponent<IComponent> Render<TComponent>(this BunitRenderer renderer)
		where TComponent : IComponent
		=> renderer.Render(builder =>
		{
			builder.OpenComponent<TComponent>(0);
			builder.CloseComponent();
		});

	public static async Task<IRenderedComponent<IComponent>> RenderAsync<TComponent>(this BunitRenderer renderer)
		where TComponent : IComponent
	{
		var rc = await renderer.RenderAsync(builder =>
			{
				builder.OpenComponent<TComponent>(0);
				builder.CloseComponent();
			}).ConfigureAwait(false);

		return rc;
	}
}
