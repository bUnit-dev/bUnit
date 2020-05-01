using System;
using System.Linq;
using System.Threading.Tasks;
using Bunit.Rendering;
using Bunit.Rendering.RenderEvents;
using Bunit.TestAssets.SampleComponents;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using Shouldly;
using Xunit;

namespace Bunit
{
	public class TestRendererTest
	{
		private static readonly ServiceProvider ServiceProvider = new ServiceCollection().BuildServiceProvider();

		[Fact(DisplayName = "Renderer pushes render events to subscribers when renders occur")]
		public async Task Test001()
		{
			// arrange
			var sut = new TestRenderer(ServiceProvider, NullLoggerFactory.Instance);
			var res = new ConcurrentRenderEventSubscriber(sut.RenderEvents);

			// act
			var cut = await sut.RenderComponent<Simple1>(Array.Empty<ComponentParameter>());

			// assert
			res.RenderCount.ShouldBe(1);

			// act - trigger another render by setting the components parameters again
			await sut.InvokeAsync(() => cut.Component.SetParametersAsync(ParameterView.Empty));

			// assert
			res.RenderCount.ShouldBe(2);
		}
	}
}
