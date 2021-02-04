using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Shouldly;
using Xunit;
using static Bunit.ComponentParameterFactory;

namespace Bunit
{
	public class ComponentParameterFactoryTest
	{
		private const string NAME = nameof(NAME);
		private const string EXPECTED = nameof(EXPECTED);
		private static readonly TestContext Context = new();

		private static IRenderedFragment RenderWithRenderFragment(RenderFragment renderFragment)
		{
			return (IRenderedFragment)Context.Renderer.RenderFragment(renderFragment);
		}

		private string? Actual { get; set; }

		[Fact(DisplayName = "EventCallback(Action) creates parameter with provided name and callback")]
		public async Task Test001()
		{
			Action action = () => Actual = EXPECTED;

			var cp = EventCallback(NAME, action);

			await VerifyEventCallbackParameter(cp);
		}

		[Fact(DisplayName = "EventCallback(Action<object>) creates parameter with provided name and callback")]
		public async Task Test002()
		{
			Action<object> action = args => Actual = EXPECTED;

			var cp = EventCallback(NAME, action);

			await VerifyEventCallbackParameter(cp);
		}

		[Fact(DisplayName = "EventCallback(Func<Task>) creates parameter with provided name and callback")]
		public async Task Test003()
		{
			Func<Task> action = () => { Actual = EXPECTED; return Task.CompletedTask; };

			var cp = EventCallback(NAME, action);

			await VerifyEventCallbackParameter(cp);
		}

		[Fact(DisplayName = "EventCallback(Func<object, Task>) creates parameter with provided name and callback")]
		public async Task Test004()
		{
			Func<object, Task> action = args => { Actual = EXPECTED; return Task.CompletedTask; };

			var cp = EventCallback(NAME, action);

			await VerifyEventCallbackParameter(cp);
		}

		[Fact(DisplayName = "EventCallback<TValue>(Action) creates parameter with provided name and callback")]
		public async Task Test011()
		{
			Action action = () => Actual = EXPECTED;

			var cp = EventCallback<EventArgs>(NAME, action);

			await VerifyEventCallbackParameter<EventArgs>(cp);
		}

		[Fact(DisplayName = "EventCallback<TValue>(Action<TValue>) creates parameter with provided name and callback")]
		public async Task Test012()
		{
			Action<EventArgs> action = args => Actual = EXPECTED;

			var cp = EventCallback<EventArgs>(NAME, action);

			await VerifyEventCallbackParameter<EventArgs>(cp);
		}

		[Fact(DisplayName = "EventCallback(Func<Task>) creates parameter with provided name and callback")]
		public async Task Test013()
		{
			Func<Task> action = () => { Actual = EXPECTED; return Task.CompletedTask; };

			var cp = EventCallback<EventArgs>(NAME, action);

			await VerifyEventCallbackParameter<EventArgs>(cp);
		}

		[Fact(DisplayName = "EventCallback(Func<object, Task>) creates parameter with provided name and callback")]
		public async Task Test014()
		{
			Func<EventArgs, Task> action = args => { Actual = EXPECTED; return Task.CompletedTask; };

			var cp = EventCallback<EventArgs>(NAME, action);

			await VerifyEventCallbackParameter<EventArgs>(cp);
		}

		private async Task VerifyEventCallbackParameter(ComponentParameter cp)
		{
			cp.Name.ShouldBe(NAME);
			cp.IsCascadingValue.ShouldBeFalse();
			await cp.Value.ShouldBeOfType<EventCallback>().InvokeAsync(EventArgs.Empty);
			Actual.ShouldBe(EXPECTED);
		}

		private async Task VerifyEventCallbackParameter<TCallbackType>(ComponentParameter cp)
			where TCallbackType : new()
		{
			cp.Name.ShouldBe(NAME);
			cp.IsCascadingValue.ShouldBeFalse();
			await cp.Value.ShouldBeOfType<EventCallback<TCallbackType>>().InvokeAsync(new TCallbackType());
			Actual.ShouldBe(EXPECTED);
		}

		[Fact(DisplayName = "Parameter creates a parameter with provided name and value")]
		public void Test020()
		{
			var cp = Parameter(NAME, EXPECTED);

			cp.Name.ShouldBe(NAME);
			cp.Value.ShouldBe(EXPECTED);
			cp.IsCascadingValue.ShouldBeFalse();
		}

		[Fact(DisplayName = "Parameter creates a parameter with provided name and null value")]
		public void Test021()
		{
			var cp = Parameter(NAME, null);

			cp.Name.ShouldBe(NAME);
			cp.Value.ShouldBeNull();
			cp.IsCascadingValue.ShouldBeFalse();
		}

		[Fact(DisplayName = "CascadingValue(name, value) creates a named cascading value parameter with provided name and value")]
		public void Test030()
		{
			var cp = CascadingValue(NAME, EXPECTED);

			cp.Name.ShouldBe(NAME);
			cp.Value.ShouldBe(EXPECTED);
			cp.IsCascadingValue.ShouldBeTrue();
		}

		[Fact(DisplayName = "CascadingValue(name, value) creates a unnamed cascading value parameter with provided name and value")]
		public void Test031()
		{
			var cp = CascadingValue(EXPECTED);

			cp.Name.ShouldBeNull();
			cp.Value.ShouldBe(EXPECTED);
			cp.IsCascadingValue.ShouldBeTrue();
		}

		[Fact(DisplayName = "ChildContent(string markup) creates a parameter with a RenderFragment that renders the provided markup")]
		public void Test040()
		{
			var cp = ChildContent(EXPECTED);

			cp.Name.ShouldBe("ChildContent");
			cp.IsCascadingValue.ShouldBeFalse();
			var renderFragment = cp.Value.ShouldBeOfType<RenderFragment>();
			var renderedFragment = RenderWithRenderFragment(renderFragment);
			renderedFragment.Markup.ShouldBe(EXPECTED);
		}

		[Fact(DisplayName = "ChildContent<TComponent>() creates a parameter with a RenderFragment that renders a component of type TComponent")]
		public void Test041()
		{
			var cp = ChildContent<TestComponent>();

			cp.Name.ShouldBe("ChildContent");
			cp.IsCascadingValue.ShouldBeFalse();
			var renderFragment = cp.Value.ShouldBeOfType<RenderFragment>();
			var renderedFragment = RenderWithRenderFragment(renderFragment);
			renderedFragment.Markup.ShouldBe(nameof(TestComponent));
		}

		[Fact(DisplayName = "ChildContent<TComponent>(component parameters) creates a parameter with a RenderFragment that renders a component of type TComponent")]
		public void Test042()
		{
			var cp = ChildContent<TestComponent>((nameof(TestComponent.Input), EXPECTED));

			cp.Name.ShouldBe("ChildContent");
			cp.IsCascadingValue.ShouldBeFalse();
			var renderFragment = cp.Value.ShouldBeOfType<RenderFragment>();
			var renderedFragment = RenderWithRenderFragment(renderFragment);
			renderedFragment.Markup.ShouldBe(nameof(TestComponent) + EXPECTED);
		}

		[Fact(DisplayName = "ChildContent(RenderFragment) creates a parameter with a RenderFragment passed to ChildContent")]
		public void Test043()
		{
			var cp = ChildContent(b => b.AddMarkupContent(0, EXPECTED));

			cp.Name.ShouldBe("ChildContent");
			cp.IsCascadingValue.ShouldBeFalse();
			var renderFragment = cp.Value.ShouldBeOfType<RenderFragment>();
			var renderedFragment = RenderWithRenderFragment(renderFragment);
			renderedFragment.Markup.ShouldBe(EXPECTED);
		}

		[Fact(DisplayName = "RenderFragment(name, markup) creates a parameter with a RenderFragment that renders a component of type TComponent")]
		public void Test051()
		{
			var cp = RenderFragment(NAME, EXPECTED);

			cp.Name.ShouldBe(NAME);
			cp.IsCascadingValue.ShouldBeFalse();
			var renderFragment = cp.Value.ShouldBeOfType<RenderFragment>();
			var renderedFragment = RenderWithRenderFragment(renderFragment);
			renderedFragment.Markup.ShouldBe(EXPECTED);
		}

		[Fact(DisplayName = "RenderFragment<TComponent>(name, component parameters) creates a parameter with a RenderFragment that renders a component of type TComponent")]
		public void Test052()
		{
			var cp = RenderFragment<TestComponent>(NAME, (nameof(TestComponent.Input), EXPECTED));

			cp.Name.ShouldBe(NAME);
			cp.IsCascadingValue.ShouldBeFalse();
			var renderFragment = cp.Value.ShouldBeOfType<RenderFragment>();
			var renderedFragment = RenderWithRenderFragment(renderFragment);
			renderedFragment.Markup.ShouldBe(nameof(TestComponent) + EXPECTED);
		}

		[Fact(DisplayName = "Template<TValue>(string, RenderFragment<TValue>) creates a parameter with a Template")]
		public void Test061()
		{
			var cp = Template<string>(NAME, s => b => b.AddMarkupContent(0, s));

			cp.Name.ShouldBe(NAME);
			cp.IsCascadingValue.ShouldBeFalse();
			var template = cp.Value.ShouldBeOfType<RenderFragment<string>>();
			var renderedFragment = RenderWithRenderFragment(template(EXPECTED));
			renderedFragment.Markup.ShouldBe(EXPECTED);
		}

		[Fact(DisplayName = "Template<TValue>(string, Func<TValue, string>) creates a parameter with a Template")]
		public void Test062()
		{
			var cp = Template<string>(NAME, s => s);

			cp.Name.ShouldBe(NAME);
			cp.IsCascadingValue.ShouldBeFalse();
			var template = cp.Value.ShouldBeOfType<RenderFragment<string>>();
			var renderedFragment = RenderWithRenderFragment(template(EXPECTED));
			renderedFragment.Markup.ShouldBe(EXPECTED);
		}

		[Fact(DisplayName = "Template<TValue>(string, Func<TValue, string>) creates a parameter with a Template")]
		public void Test063()
		{
			var cp = Template<TestComponent, string>(NAME, value => new ComponentParameter[]
			{
				(nameof(TestComponent.Input), value),
			});

			cp.Name.ShouldBe(NAME);
			cp.IsCascadingValue.ShouldBeFalse();
			var template = cp.Value.ShouldBeOfType<RenderFragment<string>>();
			var renderedFragment = RenderWithRenderFragment(template(EXPECTED));
			renderedFragment.Markup.ShouldBe(nameof(TestComponent) + EXPECTED);
		}

		private class TestComponent : ComponentBase
		{
			[Parameter] public string? Input { get; set; }
			[Parameter] public RenderFragment<string>? Template { get; set; }

			protected override void BuildRenderTree(RenderTreeBuilder builder)
			{
				builder.AddMarkupContent(0, nameof(TestComponent));
				builder.AddMarkupContent(1, Input);
			}
		}
	}
}
