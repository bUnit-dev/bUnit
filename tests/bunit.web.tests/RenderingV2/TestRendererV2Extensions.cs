namespace Bunit.RenderingV2;

internal static class TestRendererV2Extensions
{
	public static IRenderedComponent<IComponent> Render<TComponent>(this TestRendererV2 renderer)
		where TComponent : IComponent
		=> renderer.Render(builder =>
		{
			builder.OpenComponent<TComponent>(0);
			builder.CloseComponent();
		});		
}
