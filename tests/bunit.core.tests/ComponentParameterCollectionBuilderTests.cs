using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Shouldly;
using Xunit;

namespace Bunit
{
	public class ComponentParameterCollectionBuilderTests : TestContext
	{
		private ComponentParameterCollectionBuilder<Params> Builder { get; } = new();

		private bool EventCallbackCalled { get; set; }

		private void VerifyParameter<T>(string expectedName, [AllowNull] T expectedInput)
		{
			Builder.Build()
				.ShouldHaveSingleItem()
				.ShouldBeParameter<object?>(
					name: expectedName,
					value: expectedInput,
					isCascadingValue: false);
		}

		private async Task VerifyEventCallbackAsync(string expectedName)
		{
			var actual = Builder.Build()
				.ShouldHaveSingleItem()
				.ShouldBeParameter<EventCallback>(name: expectedName, isCascadingValue: false);
			await actual.InvokeAsync(EventArgs.Empty);
			EventCallbackCalled.ShouldBeTrue();
		}

		private async Task VerifyEventCallbackAsync<T>(string expectedName)
			where T : new()
		{
			var actual = Builder.Build()
				.ShouldHaveSingleItem()
				.ShouldBeParameter<EventCallback<T>>(name: expectedName, isCascadingValue: false);
			await actual.InvokeAsync(new T());
			EventCallbackCalled.ShouldBeTrue();
		}

		private IRenderedFragment RenderWithRenderFragment(RenderFragment renderFragment)
		{
			return (IRenderedFragment)Renderer.RenderFragment(renderFragment);
		}

		private IRenderedComponent<TComponent> RenderWithRenderFragment<TComponent>(RenderFragment renderFragment)
			where TComponent : IComponent
		{
			var res = (IRenderedFragment)Renderer.RenderFragment(renderFragment);
			return res.FindComponent<TComponent>();
		}

		[Fact(DisplayName = "Null for parameter selector throws")]
		public void Test000()
		{
			Should.Throw<ArgumentNullException>(() => Builder.Add(default!, 42));
		}

		[Fact(DisplayName = "Selecting a non property with parameter selector throws")]
		public void Test0000()
		{
			Should.Throw<ArgumentException>(() => Builder.Add(x => x.Field, 42));
		}

		[Fact(DisplayName = "Selecting a non parameter property with parameter selector throws")]
		public void Test00000()
		{
			Should.Throw<ArgumentException>(() => Builder.Add(x => x.NonParamProp, new object()));
		}

		[Fact(DisplayName = "Value type with parameter selector")]
		public void Test001()
		{
			Builder.Add(x => x.ValueTypeParam, 42);
			VerifyParameter("ValueTypeParam", 42);
		}

		[Fact(DisplayName = "Null for struct? with parameter selector")]
		public void Test002()
		{
			Builder.Add(x => x.NullableValueTypeParam, null);
			VerifyParameter<int?>("NullableValueTypeParam", null);
		}

		[Fact(DisplayName = "Struct? with parameter selector")]
		public void Test003()
		{
			Builder.Add(x => x.NullableValueTypeParam, 1234);
			VerifyParameter("NullableValueTypeParam", 1234);
		}

		[Fact(DisplayName = "Object with parameter selector")]
		public void Test004()
		{
			var input = new object();
			Builder.Add(x => x.Param, input);
			VerifyParameter("Param", input);
		}

		[Fact(DisplayName = "Null for object with parameter selector")]
		public void Test005()
		{
			Builder.Add(x => x.Param, null);
			VerifyParameter<object>("Param", null);
		}

		[Fact(DisplayName = "EventCallback with parameter selector")]
		public void Test010()
		{
			var input = EventCallback.Empty;
			Builder.Add(x => x.EC, input);
			VerifyParameter("EC", input);
		}

		[Fact(DisplayName = "Null to EventCallback throws")]
		public void Test011()
		{
			Should.Throw<ArgumentNullException>(() => Builder.Add(x => x.EC, null!));
		}

		[Fact(DisplayName = "Null for EventCallback? with parameter selector")]
		public void Test011_2()
		{
			Builder.Add<EventCallback?>(x => x.NullableEC, null);
			VerifyParameter<EventCallback?>("NullableEC", null);
		}

		[Fact(DisplayName = "Action to EventCallback with parameter selector")]
		public async Task Test012()
		{
			Builder.Add(x => x.EC, () => { EventCallbackCalled = true; });
			await VerifyEventCallbackAsync("EC");
		}

		[Fact(DisplayName = "Action<object> to EventCallback with parameter selector")]
		public async Task Test013()
		{
			Builder.Add(x => x.EC, (x) => { EventCallbackCalled = true; });
			await VerifyEventCallbackAsync("EC");
		}

		[Fact(DisplayName = "Func<Task> to EventCallback with parameter selector")]
		public async Task Test014()
		{
			Builder.Add(x => x.EC, () => { EventCallbackCalled = true; return Task.CompletedTask; });
			await VerifyEventCallbackAsync("EC");
		}

		[Fact(DisplayName = "Action to EventCallback? with parameter selector")]
		public async Task Test015()
		{
			Builder.Add(x => x.NullableEC, () => { EventCallbackCalled = true; });
			await VerifyEventCallbackAsync("NullableEC");
		}

		[Fact(DisplayName = "Action<object> to EventCallback? with parameter selector")]
		public async Task Test016()
		{
			Builder.Add(x => x.NullableEC, (x) => { EventCallbackCalled = true; });
			await VerifyEventCallbackAsync("NullableEC");
		}

		[Fact(DisplayName = "Func<Task> to EventCallback? with parameter selector")]
		public async Task Test017()
		{
			Builder.Add(x => x.NullableEC, () => { EventCallbackCalled = true; return Task.CompletedTask; });
			await VerifyEventCallbackAsync("NullableEC");
		}

		[Fact(DisplayName = "EventCallback<T> with parameter selector")]
		public void Test018()
		{
			var input = EventCallback<EventArgs>.Empty;
			Builder.Add(x => x.ECWithArgs, input);
			VerifyParameter("ECWithArgs", input);
		}

		[Fact(DisplayName = "Action to EventCallback<T> with parameter selector")]
		public async Task Test019()
		{
			Builder.Add(x => x.ECWithArgs, () => { EventCallbackCalled = true; });
			await VerifyEventCallbackAsync<EventArgs>("ECWithArgs");
		}

		[Fact(DisplayName = "Action<object> to EventCallback<T> with parameter selector")]
		public async Task Test020()
		{
			Builder.Add(x => x.ECWithArgs, (x) => { EventCallbackCalled = true; });
			await VerifyEventCallbackAsync<EventArgs>("ECWithArgs");
		}

		[Fact(DisplayName = "Func<Task> to EventCallback<T> with parameter selector")]
		public async Task Test021()
		{
			Builder.Add(x => x.ECWithArgs, () => { EventCallbackCalled = true; return Task.CompletedTask; });
			await VerifyEventCallbackAsync<EventArgs>("ECWithArgs");
		}

		[Fact(DisplayName = "EventCallback<T> with parameter selector")]
		public void Test022()
		{
			var input = EventCallback<EventArgs>.Empty;
			Builder.Add(x => x.NullableECWithArgs, input);
			VerifyParameter("NullableECWithArgs", input);
		}

		[Fact(DisplayName = "Action to EventCallback<T> with parameter selector")]
		public async Task Test023()
		{
			Builder.Add(x => x.NullableECWithArgs, () => { EventCallbackCalled = true; });
			await VerifyEventCallbackAsync<EventArgs>("NullableECWithArgs");
		}

		[Fact(DisplayName = "Action<object> to EventCallback<T> with parameter selector")]
		public async Task Test024()
		{
			Builder.Add(x => x.NullableECWithArgs, (x) => { EventCallbackCalled = true; });
			await VerifyEventCallbackAsync<EventArgs>("NullableECWithArgs");
		}

		[Fact(DisplayName = "Func<Task> to EventCallback<T> with parameter selector")]
		public async Task Test025()
		{
			Builder.Add(x => x.NullableECWithArgs, () => { EventCallbackCalled = true; return Task.CompletedTask; });
			await VerifyEventCallbackAsync<EventArgs>("NullableECWithArgs");
		}

		[Fact(DisplayName = "ChildContent can be passed as RenderFragment")]
		public void Test030()
		{
			RenderFragment input = b => b.AddMarkupContent(0, string.Empty);
			Builder.AddChildContent(input);
			VerifyParameter<RenderFragment>("ChildContent", input);
		}

		[Fact(DisplayName = "Calling AddChildContent when TCompnent does not have a parameter named ChildContent throws")]
		public void Test031()
		{
			RenderFragment input = b => b.AddMarkupContent(0, string.Empty);
			Assert.Throws<ArgumentException>(() => new ComponentParameterCollectionBuilder<NoParams>().AddChildContent(input));
			Assert.Throws<ArgumentException>(() => new ComponentParameterCollectionBuilder<NonChildContentParameter>().AddChildContent(input));
		}

		[Fact(DisplayName = "ChildContent can be passed as a nested component parameter builder")]
		public void Test032()
		{
			Builder.AddChildContent<InhertedParams>(parameters => parameters.Add(p => p.ValueTypeParam, 42));

			var actual = Builder.Build().ShouldHaveSingleItem()
				.ShouldBeParameter<RenderFragment>("ChildContent", isCascadingValue: false);
			var actualComponent = RenderWithRenderFragment<InhertedParams>(actual);
			actualComponent.Instance.ValueTypeParam.ShouldBe(42);
		}

		[Fact(DisplayName = "ChildContent can be passed as a child component without parameters")]
		public void Test033()
		{
			Builder.AddChildContent<NoParams>();

			var actual = Builder.Build().ShouldHaveSingleItem()
				.ShouldBeParameter<RenderFragment>("ChildContent", isCascadingValue: false);
			var actualComponent = RenderWithRenderFragment<NoParams>(actual);
			actualComponent.Instance.ShouldBeOfType<NoParams>();
		}

		[Fact(DisplayName = "ChildContent can be passed as a markup string")]
		public void Test034()
		{
			var input = "<p>42</p>";
			Builder.AddChildContent(input);

			var actual = Builder.Build().ShouldHaveSingleItem()
				.ShouldBeParameter<RenderFragment>("ChildContent", isCascadingValue: false);
			var actualComponent = RenderWithRenderFragment(actual);
			actualComponent.Markup.ShouldBe(input);
		}

		[Fact(DisplayName = "RenderFragment can be passed as a nested component parameter builder")]
		public void Test040()
		{
			Builder.Add<InhertedParams>(x => x.OtherFragment, parameters => parameters.Add(p => p.ValueTypeParam, 42));

			var actual = Builder.Build().ShouldHaveSingleItem()
				.ShouldBeParameter<RenderFragment>("OtherFragment", isCascadingValue: false);
			var actualComponent = RenderWithRenderFragment<InhertedParams>(actual);
			actualComponent.Instance.ValueTypeParam.ShouldBe(42);
		}

		[Fact(DisplayName = "RenderFragment can be passed as a child component without parameters")]
		public void Test041()
		{
			Builder.Add<NoParams>(x => x.OtherFragment);

			var actual = Builder.Build().ShouldHaveSingleItem()
				.ShouldBeParameter<RenderFragment>("OtherFragment", isCascadingValue: false);
			var actualComponent = RenderWithRenderFragment<NoParams>(actual);
			actualComponent.Instance.ShouldBeOfType<NoParams>();
		}

		[Fact(DisplayName = "RenderFragment can be passed as a markup string")]
		public void Test042()
		{
			var input = "<p>42</p>";
			Builder.Add(x => x.OtherFragment, input);

			var actual = Builder.Build().ShouldHaveSingleItem()
				.ShouldBeParameter<RenderFragment>("OtherFragment", isCascadingValue: false);
			var actualComponent = RenderWithRenderFragment(actual);
			actualComponent.Markup.ShouldBe(input);
		}

		[Fact(DisplayName = "RenderFragment can be passed RenderFragment")]
		public void Test043()
		{
			RenderFragment input = b => b.AddMarkupContent(0, string.Empty);

			Builder.Add(x => x.OtherFragment, input);

			Builder.Build().ShouldHaveSingleItem()
				.ShouldBeParameter("OtherFragment", input, isCascadingValue: false);
		}

		[Fact(DisplayName = "RenderFragment can be passed multiple times")]
		public void Test044()
		{
			var input = "FOO";
			Builder.Add(x => x.OtherFragment, input);
			Builder.Add(x => x.OtherFragment, b => b.AddMarkupContent(0, input));

			Builder.Build().ShouldAllBe(VerifyTemplate, VerifyTemplate);

			void VerifyTemplate(ComponentParameter template)
			{
				var actual = template.ShouldBeParameter<RenderFragment>("OtherFragment", isCascadingValue: false);
				var actualComponent = RenderWithRenderFragment(actual);
				actualComponent.Markup.ShouldBe(input);
			}
		}

		[Fact(DisplayName = "RenderFragment<T>? can be passed as RenderFragment")]
		public void Test050()
		{
			RenderFragment<string> input = s => b => b.AddMarkupContent(0, s);

			Builder.Add(x => x.Template, input);

			Builder.Build().ShouldHaveSingleItem()
				.ShouldBeParameter("Template", input, isCascadingValue: false);
		}

		[Fact(DisplayName = "RenderFragment<T>? can be passed lambda builder")]
		public void Test051()
		{
			var input = "FOO";
			Builder.Add(x => x.Template, value => value);

			var actual = Builder.Build().ShouldHaveSingleItem()
				.ShouldBeParameter<RenderFragment<string>>("Template", isCascadingValue: false);
			var actualComponent = RenderWithRenderFragment(actual(input));
			actualComponent.Markup.ShouldBe(input);
		}

		[Fact(DisplayName = "RenderFragment<T>? can be passed as nested object builder")]
		public void Test052()
		{
			var input = "FOO";
			Builder.Add<InhertedParams, string>(
				x => x.Template,
				value => parameters => parameters.Add(p => p.Param, value));

			var actual = Builder.Build().ShouldHaveSingleItem()
				.ShouldBeParameter<RenderFragment<string>>("Template", isCascadingValue: false);

			var actualComponent = RenderWithRenderFragment<InhertedParams>(actual(input));
			actualComponent.Instance.Param.ShouldBe(input);
		}

		[Fact(DisplayName = "RenderFragment<T> can be passed multiple times")]
		public void Test053()
		{
			Builder.Add(x => x.Template, value => value);
			Builder.Add(x => x.Template, s => b => b.AddMarkupContent(0, s));

			Builder.Build().ShouldAllBe(VerifyTemplate, VerifyTemplate);

			void VerifyTemplate(ComponentParameter template)
			{
				var input = "FOO";
				var rf = template.ShouldBeParameter<RenderFragment<string>>("Template", isCascadingValue: false);
				var actualComponent = RenderWithRenderFragment(rf(input));
				actualComponent.Markup.ShouldBe(input);
			}
		}

		[Fact(DisplayName = "Cascading values can be passed using Add and parameter selector")]
		public void Test060()
		{
			Builder.Add(p => p.NullableCC, "FOO");
			Builder.Add(p => p.CC, 1);
			Builder.Add(p => p.NullableNamedCC, "BAR");
			Builder.Add(p => p.NamedCC, 2);
			Builder.Add(p => p.AnotherNamedCC, 3);
			Builder.Add(p => p.RFCC, "BAZ");

			Builder.Build().ShouldAllBe(
				x => x.ShouldBeParameter(null, "FOO", isCascadingValue: true),
				x => x.ShouldBeParameter(null, 1, isCascadingValue: true),
				x => x.ShouldBeParameter("NullableNamedCCNAME", "BAR", isCascadingValue: true),
				x => x.ShouldBeParameter("NamedCCNAME", 2, isCascadingValue: true),
				x => x.ShouldBeParameter("AnotherNamedCCNAME", 3, isCascadingValue: true),
				x =>
				{
					var rf = x.ShouldBeParameter<RenderFragment>(null, isCascadingValue: true);
					RenderWithRenderFragment(rf).Markup.ShouldBe("BAZ");
				});
		}

		[Fact(DisplayName = "AddCascadingValue can add unnamed cascading values")]
		public void Test061()
		{
			var input = "FOO";

			Builder.AddCascadingValue(input);

			Builder.Build().ShouldHaveSingleItem()
				.ShouldBeParameter(null, input, isCascadingValue: true);
		}

		[Fact(DisplayName = "AddCascadingValue can add named cascading values")]
		public void Test062()
		{
			var name = "NAME";
			var input = "FOO";

			Builder.AddCascadingValue(name, input);

			Builder.Build().ShouldHaveSingleItem()
				.ShouldBeParameter(name, input, isCascadingValue: true);
		}

		[Fact(DisplayName = "AddUnmatched can add unmatched empty value attributes as parameters")]
		public void Test070()
		{
			var name = "NAME";

			Builder.AddUnmatched(name);

			Builder.Build()
				.ShouldHaveSingleItem()
				.ShouldBeParameter<object>(name, null, isCascadingValue: false);
		}

		[Fact(DisplayName = "AddUnmatched can add unmatched value attributes as parameters")]
		public void Test071()
		{
			var name = "NAME";
			var value = "FOO";

			Builder.AddUnmatched(name, value);

			Builder.Build()
				.ShouldHaveSingleItem()
				.ShouldBeParameter(name, value, isCascadingValue: false);
		}

		[Theory(DisplayName = "AddUnmatched throws if name is null or whitespace")]
		[InlineData("")]
		[InlineData(" ")]
		public void Test072(string emptyName)
		{
			Should.Throw<ArgumentException>(() => Builder.AddUnmatched(emptyName));
		}

		[Fact(DisplayName = "AddUnmatched throws if component doesnt have an Parameter(CaptureUnmatchedValues = true)")]
		public void Test073()
		{
			var sut = new ComponentParameterCollectionBuilder<NoParams>();

			Should.Throw<ArgumentException>(() => sut.AddUnmatched("foo"));
		}

		[Fact(DisplayName = "Can select parameters inherited from base component ")]
		public void Test101()
		{
			var builder = new ComponentParameterCollectionBuilder<InhertedParams>();

			builder.Add(x => x.Param, new object());

			builder.Build().ShouldHaveSingleItem();
		}

		[Fact(DisplayName = "TryAdd returns false when parameter does not exist on component")]
		public void Test200()
		{
			var sut = new ComponentParameterCollectionBuilder<NoParams>();

			var result = sut.TryAdd("FOO", "VALUE");

			result.ShouldBeFalse();
			sut.Build().Count.ShouldBe(0);
		}

		[Fact(DisplayName = "TryAdd returns true when parameter exists on component")]
		public void Test201()
		{
			var name = nameof(Params.ValueTypeParam);
			var input = 42;

			var result = Builder.TryAdd(name, input);

			result.ShouldBeTrue();
			Builder.Build().ShouldHaveSingleItem()
				.ShouldBeParameter(name, input, isCascadingValue: false);
		}

		[Fact(DisplayName = "TryAdd returns true when unnamed cascading parameter exists on component")]
		public void Test202()
		{
			var name = nameof(Params.CC);
			var input = 42;

			var result = Builder.TryAdd(name, input);

			result.ShouldBeTrue();
			Builder.Build().ShouldHaveSingleItem()
				.ShouldBeParameter(null, input, isCascadingValue: true);
		}

		[Fact(DisplayName = "TryAdd returns true when named cascading parameter exists on component")]
		public void Test203()
		{
			var name = nameof(Params.NamedCC);
			var input = 42;

			var result = Builder.TryAdd(name, input);

			result.ShouldBeTrue();
			Builder.Build().ShouldHaveSingleItem()
				.ShouldBeParameter(name + "NAME", input, isCascadingValue: true);
		}

		[Fact(DisplayName = "Add of inherited overriden parameter works")]
		public void Test300()
		{
			var sut = new ComponentParameterCollectionBuilder<InheritedParamsWithOverride>();

			sut.Add(x => x.Value, true);

			sut.Build()
				.ShouldHaveSingleItem()
				.ShouldBeParameter("Value", true, isCascadingValue: false);
		}

		[Fact(DisplayName = "Add of inherited parameter works")]
		public void Test301()
		{
			var sut = new ComponentParameterCollectionBuilder<InheritedParamsWithoutOverride>();

			sut.Add(x => x.Value, true);

			sut.Build()
				.ShouldHaveSingleItem()
				.ShouldBeParameter("Value", true, isCascadingValue: false);
		}

		private class Params : ComponentBase
		{
			[SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private")]
			public int Field = -1;
			[Parameter(CaptureUnmatchedValues = true)] public IReadOnlyDictionary<string, object>? Attributes { get; set; }
			[Parameter] public int? NullableValueTypeParam { get; set; }
			[Parameter] public int ValueTypeParam { get; set; } = -1;
			[Parameter] public object? Param { get; set; }
			[Parameter] public EventCallback? NullableEC { get; set; }
			[Parameter] public EventCallback EC { get; set; }
			[Parameter] public EventCallback<EventArgs>? NullableECWithArgs { get; set; }
			[Parameter] public EventCallback<EventArgs> ECWithArgs { get; set; }
			[Parameter] public RenderFragment? ChildContent { get; set; }
			[Parameter] public RenderFragment? OtherFragment { get; set; }
			[Parameter] public RenderFragment<string>? Template { get; set; }
			[CascadingParameter] public string? NullableCC { get; set; }
			[CascadingParameter] public int CC { get; set; } = -1;
			[CascadingParameter(Name = nameof(NullableNamedCC) + "NAME")] public string? NullableNamedCC { get; set; }
			[CascadingParameter(Name = nameof(NamedCC) + "NAME")] public int NamedCC { get; set; } = -1;
			[CascadingParameter(Name = nameof(AnotherNamedCC) + "NAME")] public int AnotherNamedCC { get; set; } = -1;
			[CascadingParameter] public RenderFragment? RFCC { get; set; }
			public object? NonParamProp { get; set; }
			public void SomeMethod() { }
		}

		private class NoParams : ComponentBase { }
		private class NonChildContentParameter : ComponentBase { public RenderFragment? ChildContent { get; set; } }
		private class InhertedParams : Params { }
		private abstract class ParamsBase<T> : ComponentBase
		{
			public abstract T Value { get; set; }
		}

		private class InheritedParamsWithOverride : ParamsBase<bool?>
		{
			[Parameter] public override bool? Value { get; set; }
		}

		private class InheritedParamsWithoutOverride : InheritedParamsWithOverride
		{ }
	}
}
