using System;
using Bunit.TestAssets.SampleComponents;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Shouldly;
using Xunit;

namespace Bunit
{
	public class TestContextAddLayoutComponentTest : TestContext
	{
		[Fact(DisplayName = "AddLayoutComponent<T> throws when T doesn't have a ChildContent or Body parameter")]
		public void Test100()
		{
			Should.Throw<ArgumentException>(() =>
			{
				AddLayoutComponent<Simple1>();
				RenderComponent<InnerComponent>();
			});
		}

		[Fact(DisplayName = "AddLayoutComponent<T> adds T as a layout component which CUT is rendered as child of")]
		public void Test110()
		{
			AddLayoutComponent<LayoutComponent>();

			var cut = RenderComponent<InnerComponent>();

			cut.Markup.ShouldBe($"<div>LAYOUT VALUE</div>");
		}

		[Fact(DisplayName = "AddLayoutComponent<T> allows passing parameters to layout components")]
		public void Test111()
		{
			AddLayoutComponent<LayoutComponent>(parameters => parameters.Add(p => p.Value, "ANOTHER VALUE"));

			var cut = RenderComponent<InnerComponent>();

			cut.Markup.ShouldBe($"<div>ANOTHER VALUE</div>");
		}

		[Fact(DisplayName = "AddLayoutComponent<T> can be called multiple times")]
		public void Test112()
		{
			AddLayoutComponent<CascadingValue<string>>(parameters => parameters.Add(p => p.Value, "VALUE"));
			AddLayoutComponent<CascadingValue<int>>(parameters => parameters.Add(p => p.Value, 42));

			var cut = RenderComponent<MultipleParametersInnerComponent>();

			cut.Markup.ShouldBe($"<div>VALUE42</div>");
		}

		[Fact(DisplayName = "RenderComponent<T> finds correct component when T is also added via AddLayoutComponent<T>")]
		public void Test113()
		{
			AddLayoutComponent<CascadingValue<string>>(parameters => parameters.Add(p => p.Value, "VALUE"));
			AddLayoutComponent<MultipleParametersInnerComponent>();
			AddLayoutComponent<CascadingValue<int>>(parameters => parameters.Add(p => p.Value, 42));
			AddLayoutComponent<MultipleParametersInnerComponent>();

			var cut = RenderComponent<MultipleParametersInnerComponent>();

			cut.Markup.ShouldBe($"<div>VALUE42</div>");
		}

		private class LayoutComponent : LayoutComponentBase
		{
			[Parameter] public string Value { get; set; } = "LAYOUT VALUE";
			[Parameter] public string? Name { get; set; }

			protected override void BuildRenderTree(RenderTreeBuilder builder)
			{
				builder.OpenComponent<CascadingValue<string>>(0);
				builder.AddAttribute(1, "Value", Value);
				if (Name is not null)
					builder.AddAttribute(2, "Name", Name);
				builder.AddAttribute(3, "ChildContent", Body);
				builder.CloseComponent();
			}
		}

		private class InnerComponent : ComponentBase
		{
			[CascadingParameter] public string? LayoutValue { get; set; }

			protected override void BuildRenderTree(RenderTreeBuilder builder)
			{
				builder.OpenElement(0, "div");
				builder.AddContent(1, LayoutValue);
				builder.CloseElement();
			}
		}

		private class MultipleParametersInnerComponent : ComponentBase
		{
			[CascadingParameter] public string StringValue { get; set; } = string.Empty;
			[CascadingParameter] public int IntValue { get; set; }
			[Parameter] public RenderFragment? ChildContent { get; set; }

			protected override void BuildRenderTree(RenderTreeBuilder builder)
			{
				builder.OpenElement(0, "div");
				builder.AddContent(1, StringValue);
				builder.AddContent(2, IntValue);
				builder.AddContent(3, ChildContent);
				builder.CloseElement();
			}
		}
	}
}
