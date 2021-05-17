#if NET5_0_OR_GREATER
using System;
using AutoFixture.Xunit2;
using Bunit.TestAssets.SampleComponents;
using Microsoft.AspNetCore.Components;
using Xunit;

namespace Bunit.Rendering
{
	public class ShallowRenderTest : TestContext
	{
		[Theory(DisplayName = "ShallowRender only renders first component in render fragment, stubs child component")]
		[AutoData]
		public void Test001(string text)
		{
			var renderFragment = CreateRenderFragment<WrapperDiv>(ps => ps
				.AddChildContent<PrintCascadingValue>()
				.AddChildContent($"<header>{text}</header>")
				.AddChildContent<Simple1>());

			var cut = ShallowRender(renderFragment);

			cut.MarkupMatches(@$"<div class='wrapper'>
								  <PrintCascadingValue></PrintCascadingValue>
								  <header>{text}</header>
							      <Simple1></Simple1>
								</div>");
		}

		[Fact(DisplayName = "ShallowRender only renders first root component in render fragment")]
		public void Test002()
		{
			RenderFragment renderFragment = builder =>
			{
				builder.OpenComponent<WrapperDiv>(0);
				builder.CloseComponent();
				builder.OpenComponent<Wrapper>(1);
				builder.CloseComponent();
			};

			var cut = ShallowRender(renderFragment);

			cut.MarkupMatches($"<div class='wrapper'></div>" +
							  $"<Wrapper />");
		}

		[Theory(DisplayName = "ShallowRender does render components added to RenderTree")]
		[AutoData]
		public void Test003(string cascadingValue)
		{
			RenderTree.Add<CascadingValue<string>>(ps => ps.Add(p => p.Value, cascadingValue));

			var cut = ShallowRender(CreateRenderFragment<PrintCascadingValue>());

			cut.MarkupMatches($"<p>{cascadingValue}</p>");
		}

		private static RenderFragment CreateRenderFragment<TComponent>(Action<ComponentParameterCollectionBuilder<TComponent>>? parameterBuilder = null)
			where TComponent : IComponent
		{
			return new ComponentParameterCollectionBuilder<TComponent>(parameterBuilder)
				.Build()
				.ToRenderFragment<TComponent>();
		}
	}
}
#endif
