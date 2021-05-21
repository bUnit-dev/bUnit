#if NET5_0_OR_GREATER
using System;
using AutoFixture.Xunit2;
using Bunit.ComponentFactories;
using Bunit.TestAssets.SampleComponents;
using Bunit.TestDoubles;
using Microsoft.AspNetCore.Components;
using Shouldly;
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

			var cut = ShallowRender(renderFragment, new() { AddDiffIgnore = false });			

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

			var cut = ShallowRender(renderFragment, new() { AddDiffIgnore = false });

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

		[Fact(DisplayName = "given shallow rendered component, " +
							"when a child component is added during a later render, " +
							"then child component is stubbed")]
		public void Test005()
		{
			var cut = ShallowRender(CreateRenderFragment<ToggleChildComponent>())
				.FindComponent<ToggleChildComponent>();

			cut.SetParametersAndRender(ps => ps.Add(p => p.ShowChild, true));

			cut.FindComponents<Simple1>().ShouldBeEmpty();
			cut.FindComponents<Stub<Simple1>>().ShouldHaveSingleItem();
		}

		[Theory(DisplayName = "ShallowRender does render cascading values components added as parameters")]
		[AutoData]
		public void Test006(string cascadingValue)
		{
			var renderFragment = CreateRenderFragment<CascadingValue<string>>(ps => ps
				.Add(p => p.Value, cascadingValue)
				.AddChildContent<PrintCascadingValue>());

			var cut = ShallowRender<PrintCascadingValue>(renderFragment);

			cut.MarkupMatches($"<p>{cascadingValue}</p>");
		}

		[Theory(DisplayName = "ShallowRenderComponent only renders first component in render fragment, stubs child component")]
		[AutoData]
		public void Test010(string text)
		{
			var cut = ShallowRenderComponent<WrapperDiv>(ps => ps
				.AddChildContent<PrintCascadingValue>()
				.AddChildContent($"<header>{text}</header>")
				.AddChildContent<Simple1>(), new() { AddDiffIgnore = false });

			cut.MarkupMatches(@$"<div class='wrapper'>
								  <PrintCascadingValue></PrintCascadingValue>
								  <header>{text}</header>
							      <Simple1></Simple1>
								</div>");
		}

		[Theory(DisplayName = "ShallowRenderComponent does render components added to RenderTree")]
		[AutoData]
		public void Test011(string cascadingValue)
		{
			RenderTree.Add<CascadingValue<string>>(ps => ps.Add(p => p.Value, cascadingValue));

			var cut = ShallowRenderComponent<PrintCascadingValue>();

			cut.MarkupMatches($"<p>{cascadingValue}</p>");
		}

		[Theory(DisplayName = "ShallowRenderComponent does render cascading values components added as parameters")]
		[AutoData]
		public void Test012(string cascadingValue)
		{
			var cut = ShallowRenderComponent<PrintCascadingValue>(ps => ps
				.Add(p => p.Value, cascadingValue));

			cut.MarkupMatches($"<p>{cascadingValue}</p>");
		}

		[Fact(DisplayName = "calling ShallowRender twice on same test context throws")]
		public void Test100()
		{
			ShallowRender(CreateRenderFragment<Wrapper>());

			Should.Throw<InvalidOperationException>(() => ShallowRender(CreateRenderFragment<Wrapper>()))
				.Message.ShouldStartWith("The test context has previously been used to perform a shallow render");
		}

		[Fact(DisplayName = "calling Render after ShallowRender on same test context throws")]
		public void Test101()
		{
			ShallowRender(CreateRenderFragment<Wrapper>());

			Should.Throw<InvalidOperationException>(() => Render(CreateRenderFragment<Wrapper>()))
				.Message.ShouldStartWith("The test context has previously been used to perform a shallow render");
		}

		[Fact(DisplayName = "calling RenderComponent after ShallowRender on same test context throws")]
		public void Test102()
		{
			ShallowRender(CreateRenderFragment<Wrapper>());

			Should.Throw<InvalidOperationException>(() => RenderComponent<Wrapper>())
				.Message.ShouldStartWith("The test context has previously been used to perform a shallow render");
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
