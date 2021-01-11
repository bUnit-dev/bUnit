using System;
using System.Collections;
using System.Linq;
using Bunit.Rendering;
using Bunit.TestAssets.SampleComponents;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Shouldly;
using Xunit;

namespace Bunit
{
	public class RootRenderTreeTest : TestContext
	{
		[Fact(DisplayName = "Count returns number of component registrations added")]
		public void Test010()
		{
			RenderTree.Add<LayoutComponent>();

			RenderTree.Count.ShouldBe(1);
		}

		[Fact(DisplayName = "GetEnumerator enumerates registered components")]
		public void Test011()
		{
			RenderTree.Add<LayoutComponent>();
			RenderTree.Add<CascadingValue<string>>();

			RenderTree.ShouldAllBe(
				x => x.ComponentType.ShouldBe(typeof(LayoutComponent)),
				x => x.ComponentType.ShouldBe(typeof(CascadingValue<string>)));

			((IEnumerable)RenderTree).OfType<RootRenderTreeRegistration>().ShouldAllBe(
				x => x.ComponentType.ShouldBe(typeof(LayoutComponent)),
				x => x.ComponentType.ShouldBe(typeof(CascadingValue<string>)));
		}

		[Fact(DisplayName = "RenderTree.Add<T> throws when T doesn't have a ChildContent or Body parameter")]
		public void Test100()
		{
			Should.Throw<ArgumentException>(() =>
			{
				RenderTree.Add<Simple1>();
				RenderComponent<InnerComponent>();
			});
		}

		[Fact(DisplayName = "RenderTree.Add<T> adds T to render tree which CUT is rendered as child of")]
		public void Test110()
		{
			RenderTree.Add<LayoutComponent>();

			var cut = RenderComponent<InnerComponent>();

			cut.Markup.ShouldBe($"<div>LAYOUT VALUE</div>");
		}

		[Fact(DisplayName = "RenderTree.Add<T> allows passing parameters to render tree components")]
		public void Test111()
		{
			RenderTree.Add<LayoutComponent>(parameters => parameters.Add(p => p.Value, "ANOTHER VALUE"));

			var cut = RenderComponent<InnerComponent>();

			cut.Markup.ShouldBe($"<div>ANOTHER VALUE</div>");
		}

		[Fact(DisplayName = "RenderTree.Add<T> can be called multiple times")]
		public void Test112()
		{
			RenderTree.Add<CascadingValue<string>>(parameters => parameters.Add(p => p.Value, "VALUE"));
			RenderTree.Add<CascadingValue<int>>(parameters => parameters.Add(p => p.Value, 42));

			var cut = RenderComponent<MultipleParametersInnerComponent>();

			cut.Markup.ShouldBe($"<div>VALUE42</div>");
		}

		[Fact(DisplayName = "RenderComponent<T> finds correct component when T is also added to render tree")]
		public void Test113()
		{
			RenderTree.Add<CascadingValue<string>>(parameters => parameters.Add(p => p.Value, "VALUE"));
			RenderTree.Add<MultipleParametersInnerComponent>();
			RenderTree.Add<CascadingValue<int>>(parameters => parameters.Add(p => p.Value, 42));
			RenderTree.Add<MultipleParametersInnerComponent>();

			var cut = RenderComponent<MultipleParametersInnerComponent>();

			cut.Markup.ShouldBe($"<div>VALUE42</div>");
		}

		[Fact(DisplayName = "RenderComponent<T> finds correct component when T is also added to render tree")]
		public void Test113_2()
		{
			RenderTree.Add<CascadingValue<string>>(parameters => parameters.Add(p => p.Value, "VALUE"));
			RenderTree.Add<CascadingValue<int>>(parameters => parameters.Add(p => p.Value, 42));

			var cut = RenderComponent<CascadingValue<string>>(parameters => parameters
				.Add(p => p.Value, "FOO"));

			cut.Instance.Value.ShouldBe("FOO");
		}

		[Fact(DisplayName = "Multiple RenderTree.Add<T> calls are added to render tree in call order")]
		public void Test114()
		{
			RenderTree.Add<LayoutComponent>(parameters => parameters.Add(p => p.Value, "FOO"));
			RenderTree.Add<LayoutComponent>(parameters => parameters.Add(p => p.Value, "BAR"));

			var cut = RenderComponent<InnerComponent>();

			cut.Markup.ShouldBe($"<div>BAR</div>");
		}

		[Fact(DisplayName = "RenderTree.TryAdd<T> only adds T if it hasn't already been added")]
		public void Test120()
		{
			RenderTree.Add<LayoutComponent>(parameters => parameters.Add(p => p.Value, "FOO"));
			RenderTree.TryAdd<LayoutComponent>(parameters => parameters.Add(p => p.Value, "BAR"));

			var cut = RenderComponent<InnerComponent>();

			cut.Markup.ShouldBe($"<div>FOO</div>");
		}

		[Fact(DisplayName = "RenderTree.TryAdd<T> returns true if T was added")]
		public void Test121()
		{
			var result = RenderTree.TryAdd<LayoutComponent>();
			result.ShouldBeTrue();
		}

		[Fact(DisplayName = "RenderTree.TryAdd<T> returns false if T was previously added and not added again.")]
		public void Test122()
		{
			RenderTree.Add<LayoutComponent>();

			var result = RenderTree.TryAdd<LayoutComponent>();

			result.ShouldBeFalse();
		}

		[Fact(DisplayName = "RenderTree.TryAdd<T> distinguishes between different generic types of a generic component")]
		public void Test123()
		{
			RenderTree.TryAdd<CascadingValue<string>>();
			RenderTree.TryAdd<CascadingValue<int>>();

			RenderTree.ShouldAllBe(
				x => x.ComponentType.ShouldBe(typeof(CascadingValue<string>)),
				x => x.ComponentType.ShouldBe(typeof(CascadingValue<int>)));
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
			[CascadingParameter] public string LayoutValue { get; set; } = string.Empty;

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
