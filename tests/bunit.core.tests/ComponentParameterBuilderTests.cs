using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Bunit.TestAssets.SampleComponents;
using Bunit.TestDoubles.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Shouldly;
using Xunit;

namespace Bunit
{
	public class ComponentParameterBuilderTests
	{
		private readonly NewParamBuilder<Params> Builder = new NewParamBuilder<Params>();

		private bool EventCallbackCalled { get; set; }

		private void VerifyParameter<T>(string expectedName, [AllowNull] T expectedInput)
		{
			Builder.Parameters
				.ShouldHaveSingleItem()
				.ShouldBeParameter<object?>(
					name: expectedName,
					value: expectedInput,
					isCascadingValue: false
				);
		}

		private async Task VerifyEventCallback(string expectedName)
		{
			var actual = Builder.Parameters
				.ShouldHaveSingleItem()
				.ShouldBeParameter<EventCallback>(name: expectedName, isCascadingValue: false);
			await actual.InvokeAsync(EventArgs.Empty);
			EventCallbackCalled.ShouldBeTrue();
		}

		private async Task VerifyEventCallback<T>(string expectedName) where T : new()
		{
			var actual = Builder.Parameters
				.ShouldHaveSingleItem()
				.ShouldBeParameter<EventCallback<T>>(name: expectedName, isCascadingValue: false);
			await actual.InvokeAsync(new T());
			EventCallbackCalled.ShouldBeTrue();
		}

		private static IRenderedFragment RenderWithRenderFragment(RenderFragment renderFragment)
		{
			var ctx = new TestContext();
			return (IRenderedFragment)ctx.Renderer.RenderFragment(renderFragment);
		}

		private static IRenderedComponent<TComponent> RenderWithRenderFragment<TComponent>(RenderFragment renderFragment) where TComponent : IComponent
		{
			var ctx = new TestContext();
			var res = (IRenderedFragment)ctx.Renderer.RenderFragment(renderFragment);
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
			Should.Throw<ArgumentException>(() => Builder.Add(x => x._nonParam, 42));
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
			await VerifyEventCallback("EC");
		}

		[Fact(DisplayName = "Action<object> to EventCallback with parameter selector")]
		public async Task Test013()
		{
			Builder.Add(x => x.EC, (x) => { EventCallbackCalled = true; });
			await VerifyEventCallback("EC");
		}

		[Fact(DisplayName = "Func<Task> to EventCallback with parameter selector")]
		public async Task Test014()
		{
			Builder.Add(x => x.EC, () => { EventCallbackCalled = true; return Task.CompletedTask; });
			await VerifyEventCallback("EC");
		}

		[Fact(DisplayName = "Action to EventCallback? with parameter selector")]
		public async Task Test015()
		{
			Builder.Add(x => x.NullableEC, () => { EventCallbackCalled = true; });
			await VerifyEventCallback("NullableEC");
		}

		[Fact(DisplayName = "Action<object> to EventCallback? with parameter selector")]
		public async Task Test016()
		{
			Builder.Add(x => x.NullableEC, (x) => { EventCallbackCalled = true; });
			await VerifyEventCallback("NullableEC");
		}

		[Fact(DisplayName = "Func<Task> to EventCallback? with parameter selector")]
		public async Task Test017()
		{
			Builder.Add(x => x.NullableEC, () => { EventCallbackCalled = true; return Task.CompletedTask; });
			await VerifyEventCallback("NullableEC");
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
			await VerifyEventCallback<EventArgs>("ECWithArgs");
		}

		[Fact(DisplayName = "Action<object> to EventCallback<T> with parameter selector")]
		public async Task Test020()
		{
			Builder.Add(x => x.ECWithArgs, (x) => { EventCallbackCalled = true; });
			await VerifyEventCallback<EventArgs>("ECWithArgs");
		}

		[Fact(DisplayName = "Func<Task> to EventCallback<T> with parameter selector")]
		public async Task Test021()
		{
			Builder.Add(x => x.ECWithArgs, () => { EventCallbackCalled = true; return Task.CompletedTask; });
			await VerifyEventCallback<EventArgs>("ECWithArgs");
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
			await VerifyEventCallback<EventArgs>("NullableECWithArgs");
		}

		[Fact(DisplayName = "Action<object> to EventCallback<T> with parameter selector")]
		public async Task Test024()
		{
			Builder.Add(x => x.NullableECWithArgs, (x) => { EventCallbackCalled = true; });
			await VerifyEventCallback<EventArgs>("NullableECWithArgs");
		}

		[Fact(DisplayName = "Func<Task> to EventCallback<T> with parameter selector")]
		public async Task Test025()
		{
			Builder.Add(x => x.NullableECWithArgs, () => { EventCallbackCalled = true; return Task.CompletedTask; });
			await VerifyEventCallback<EventArgs>("NullableECWithArgs");
		}

		[Fact(DisplayName = "ChildContent can be passed as RenderFragment")]
		public void Test030()
		{
			RenderFragment input = b => b.AddMarkupContent(0, "");
			Builder.AddChildContent(input);
			VerifyParameter<RenderFragment>("ChildContent", input);
		}

		[Fact(DisplayName = "Calling AddChildContent when TCompnent does not have a parameter named ChildContent throws")]
		public void Test031()
		{
			RenderFragment input = b => b.AddMarkupContent(0, "");
			Assert.Throws<ArgumentException>(() => new NewParamBuilder<NoParams>().AddChildContent(input));
			Assert.Throws<ArgumentException>(() => new NewParamBuilder<NonChildContentParameter>().AddChildContent(input));
		}

		[Fact(DisplayName = "ChildContent can be passed as a nested component parameter builder")]
		public void Test032()
		{
			Builder.AddChildContent<InhertedParams>(parameters => parameters.Add(p => p.ValueTypeParam, 42));

			var actual = Builder.Parameters.ShouldHaveSingleItem()
				.ShouldBeParameter<RenderFragment>("ChildContent", isCascadingValue: false);
			var actualComponent = RenderWithRenderFragment<InhertedParams>(actual);
			actualComponent.Instance.ValueTypeParam.ShouldBe(42);
		}

		[Fact(DisplayName = "ChildContent can be passed as a child component without parameters")]
		public void Test033()
		{
			Builder.AddChildContent<NoParams>();

			var actual = Builder.Parameters.ShouldHaveSingleItem()
				.ShouldBeParameter<RenderFragment>("ChildContent", isCascadingValue: false);
			var actualComponent = RenderWithRenderFragment<NoParams>(actual);
			actualComponent.Instance.ShouldBeOfType<NoParams>();
		}

		[Fact(DisplayName = "ChildContent can be passed as a markup string")]
		public void Test034()
		{
			var input = "<p>42</p>";
			Builder.AddChildContent(input);

			var actual = Builder.Parameters.ShouldHaveSingleItem()
				.ShouldBeParameter<RenderFragment>("ChildContent", isCascadingValue: false);
			var actualComponent = RenderWithRenderFragment(actual);
			actualComponent.Markup.ShouldBe(input);
		}

		[Fact(DisplayName = "RenderFragment can be passed as a nested component parameter builder")]
		public void Test040()
		{
			Builder.Add<InhertedParams>(x => x.OtherFragment, parameters => parameters.Add(p => p.ValueTypeParam, 42));

			var actual = Builder.Parameters.ShouldHaveSingleItem()
				.ShouldBeParameter<RenderFragment>("OtherFragment", isCascadingValue: false);
			var actualComponent = RenderWithRenderFragment<InhertedParams>(actual);
			actualComponent.Instance.ValueTypeParam.ShouldBe(42);
		}

		[Fact(DisplayName = "RenderFragment can be passed as a child component without parameters")]
		public void Test041()
		{
			Builder.Add<NoParams>(x => x.OtherFragment);

			var actual = Builder.Parameters.ShouldHaveSingleItem()
				.ShouldBeParameter<RenderFragment>("OtherFragment", isCascadingValue: false);
			var actualComponent = RenderWithRenderFragment<NoParams>(actual);
			actualComponent.Instance.ShouldBeOfType<NoParams>();
		}

		[Fact(DisplayName = "RenderFragment can be passed as a markup string")]
		public void Test042()
		{
			var input = "<p>42</p>";
			Builder.Add(x => x.OtherFragment, input);

			var actual = Builder.Parameters.ShouldHaveSingleItem()
				.ShouldBeParameter<RenderFragment>("OtherFragment", isCascadingValue: false);
			var actualComponent = RenderWithRenderFragment(actual);
			actualComponent.Markup.ShouldBe(input);
		}

		[Fact(DisplayName = "RenderFragment can be passed RenderFragment")]
		public void Test043()
		{
			RenderFragment input = b => b.AddMarkupContent(0, "");

			Builder.Add(x => x.OtherFragment, input);

			Builder.Parameters.ShouldHaveSingleItem()
				.ShouldBeParameter("OtherFragment", input, isCascadingValue: false);
		}

		[Fact(DisplayName = "RenderFragment<string>? can be passed as RenderFragment")]
		public void Test050()
		{
			RenderFragment<string> input = s => b => b.AddMarkupContent(0, s);

			Builder.Add(x => x.Template, input);

			Builder.Parameters.ShouldHaveSingleItem()
				.ShouldBeParameter("Template", input, isCascadingValue: false);
		}

		[Fact(DisplayName = "RenderFragment<string>? can be passed lambda builder")]
		public void Test051()
		{
			var input = "FOO";
			Builder.Add(x => x.Template, value => value);

			var actual = Builder.Parameters.ShouldHaveSingleItem()
				.ShouldBeParameter<RenderFragment<string>>("Template", isCascadingValue: false);
			var actualComponent = RenderWithRenderFragment(actual(input));
			actualComponent.Markup.ShouldBe(input);
		}

		// TODO: Document new feature
		[Fact(DisplayName = "RenderFragment<string>? can be passed as nested object builder")]
		public void Test052()
		{
			var input = "FOO";
			Builder.Add<InhertedParams, string>(
				x => x.Template,
				value => parameters => parameters.Add(p => p.Param, value)
			);

			var actual = Builder.Parameters.ShouldHaveSingleItem()
				.ShouldBeParameter<RenderFragment<string>>("Template", isCascadingValue: false);
			var actualComponent = RenderWithRenderFragment<InhertedParams>(actual(input));
			actualComponent.Instance.Param.ShouldBe(input);
		}

		[Fact(DisplayName = "Can select parameters inherited from base component ")]
		public void Test101()
		{
			var builder = new NewParamBuilder<InhertedParams>();

			builder.Add(x => x.Param, new object());

			builder.Parameters.ShouldHaveSingleItem();
		}

		private class Params : ComponentBase
		{
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
			[CascadingParameter(Name = nameof(NullableNamedCC))] public string? NullableNamedCC { get; set; }
			[CascadingParameter(Name = nameof(NamedCC))] public int NamedCC { get; set; } = -1;
			[CascadingParameter(Name = nameof(AnotherNamedCC))] public int AnotherNamedCC { get; set; } = -1;
			[CascadingParameter] public RenderFragment? RFCC { get; set; }
			public int _nonParam = -1;
			public object? NonParamProp { get; set; }
			public void SomeMethod() { }
		}

		private class NoParams : ComponentBase { }
		private class NonChildContentParameter : ComponentBase { public RenderFragment? ChildContent { get; set; } }
		private class InhertedParams : Params { }

		//[Fact(DisplayName = "Add with a parameterSelector for a CascadingParameter and a nullable integer as value and Build should return the correct ComponentParameters")]
		//public void Test001()
		//{
		//	// Arrange
		//	var sut = CreateSut();
		//	const int value = 42;

		//	// Arrange
		//	sut.Add(c => c.NamedCascadingValue, value);
		//	var result = sut.Build();

		//	// Assert
		//	result.Count.ShouldBe(1);

		//	var parameter = result[0];
		//	parameter.IsCascadingValue.ShouldBeTrue();
		//	parameter.Name.ShouldBe(nameof(AllTypesOfParams<string>.NamedCascadingValue));
		//	parameter.Value.ShouldBe(value);
		//}

		//[Theory(DisplayName = "Add with a parameterSelector for a Parameter and a string as value and Build should return the correct ComponentParameters")]
		//[InlineData(null)]
		//[InlineData("")]
		//[InlineData("foo")]
		//public void Test002(string? value)
		//{
		//	// Arrange
		//	var sut = CreateSut();

		//	// Act
		//	sut.Add(c => c.RegularParam, value);
		//	var result = sut.Build();

		//	// Assert
		//	result.Count.ShouldBe(1);

		//	var parameter = result[0];
		//	parameter.IsCascadingValue.ShouldBeFalse();
		//	parameter.Name.ShouldBe(nameof(AllTypesOfParams<string>.RegularParam));
		//	parameter.Value.ShouldBe(value);
		//}

		//[Fact(DisplayName = "Add with a parameterSelector for a RenderFragment and a markup string as value and Build should return the correct ComponentParameters")]
		//public void Test003()
		//{
		//	// Arrange
		//	var sut = CreateSut();
		//	string value = "test";

		//	// Act
		//	sut.Add(c => c.OtherContent, value);
		//	var result = sut.Build();

		//	// Assert
		//	result.Count.ShouldBe(1);

		//	var parameter = result[0];
		//	parameter.IsCascadingValue.ShouldBeFalse();
		//	parameter.Name.ShouldBe(nameof(AllTypesOfParams<string>.OtherContent));
		//	parameter.Value.ShouldBeOfType<RenderFragment>();
		//}

		//[Fact(DisplayName = "Add with a parameterSelector for a RenderFragment<TValue> and a markupFactory as value and Build should return the correct ComponentParameters")]
		//public void Test004()
		//{
		//	// Arrange
		//	var sut = CreateSut();

		//	// Act
		//	sut.Add(c => c.ItemTemplate, num => $"<p>{num}</p>");
		//	var result = sut.Build();

		//	// Assert
		//	result.Count.ShouldBe(1);

		//	var parameter = result[0];
		//	parameter.IsCascadingValue.ShouldBeFalse();
		//	parameter.Name.ShouldBe(nameof(AllTypesOfParams<string>.ItemTemplate));
		//	parameter.Value.ShouldBeOfType<RenderFragment<string>>();
		//}

		//[Fact(DisplayName = "Add with a parameterSelector for a template (RenderFragment<TValue>) and a template as value and Build should return the correct ComponentParameters")]
		//public void Test005()
		//{
		//	// Arrange
		//	var sut = CreateSut();

		//	// Act
		//	sut.Add(c => c.ItemTemplate, num => builder => builder.AddMarkupContent(0, $"<p>{num}</p>"));
		//	var result = sut.Build();

		//	// Assert
		//	result.Count.ShouldBe(1);

		//	var parameter = result[0];
		//	parameter.IsCascadingValue.ShouldBeFalse();
		//	parameter.Name.ShouldBe(nameof(AllTypesOfParams<string>.ItemTemplate));
		//	parameter.Value.ShouldBeOfType<RenderFragment<string>>();
		//}

		//[Fact(DisplayName = "Add with a parameterSelector for a NonGenericEventCallback and a async-callback as value and Build should return the correct ComponentParameters")]
		//public void Test006()
		//{
		//	// Arrange
		//	var sut = CreateSut();
		//	var @event = EventCallback.Empty;
		//	Func<Task> callback = () => Task.FromResult(@event);

		//	// Act
		//	sut.Add(c => c.NonGenericCallback, callback);
		//	var result = sut.Build();

		//	// Assert
		//	result.Count.ShouldBe(1);

		//	var parameter = result[0];
		//	parameter.IsCascadingValue.ShouldBeFalse();
		//	parameter.Name.ShouldBe(nameof(AllTypesOfParams<string>.NonGenericCallback));
		//	parameter.Value.ShouldNotBeNull();
		//}

		//[Fact(DisplayName = "Add with a parameterSelector for a NonGenericEventCallback and a callback as value and Build should return the correct ComponentParameters")]
		//public void Test007()
		//{
		//	// Arrange
		//	var sut = CreateSut();

		//	// Act
		//	sut.Add(c => c.NonGenericCallback, () => throw new Exception("NonGenericCallback"));
		//	var result = sut.Build();

		//	// Assert
		//	result.Count.ShouldBe(1);

		//	var parameter = result[0];
		//	parameter.IsCascadingValue.ShouldBeFalse();
		//	parameter.Name.ShouldBe(nameof(AllTypesOfParams<string>.NonGenericCallback));
		//	parameter.Value.ShouldNotBeNull();
		//}

		//[Fact(DisplayName = "Add with a parameterSelector for a GenericEventCallback and a async-callback as value and Build should return the correct ComponentParameters")]
		//public void Test008()
		//{
		//	// Arrange
		//	var sut = CreateSut();
		//	var @event = EventCallback<EventArgs>.Empty;
		//	Func<EventArgs, Task> callback = (args) => Task.FromResult(@event);

		//	// Act
		//	sut.Add(c => c.GenericCallback, callback);
		//	var result = sut.Build();

		//	// Assert
		//	result.Count.ShouldBe(1);

		//	var parameter = result[0];
		//	parameter.IsCascadingValue.ShouldBeFalse();
		//	parameter.Name.ShouldBe(nameof(AllTypesOfParams<string>.GenericCallback));
		//	parameter.Value.ShouldNotBeNull();
		//}

		//[Fact(DisplayName = "Add with a parameterSelector for a GenericEventCallback and a callback as value and Build should return the correct ComponentParameters")]
		//public void Test009()
		//{
		//	// Arrange
		//	var sut = CreateSut();

		//	// Act
		//	sut.Add(c => c.GenericCallback, args => throw new Exception("GenericCallback"));
		//	var result = sut.Build();

		//	// Assert
		//	result.Count.ShouldBe(1);

		//	var parameter = result[0];
		//	parameter.IsCascadingValue.ShouldBeFalse();
		//	parameter.Name.ShouldBe(nameof(AllTypesOfParams<string>.GenericCallback));
		//	parameter.Value.ShouldNotBeNull();
		//}

		//[Fact(DisplayName = "Add with multiple mixed parameterSelectors and valid values and Build should return the correct ComponentParameters")]
		//public void Test010()
		//{
		//	// Arrange
		//	var sut = CreateSut();

		//	// Act
		//	sut.Add(c => c.NamedCascadingValue, 42).Add(c => c.RegularParam, "bar");
		//	var result = sut.Build();

		//	// Assert
		//	result.Count.ShouldBe(2);

		//	var first = result[0];
		//	first.IsCascadingValue.ShouldBeTrue();
		//	first.Name.ShouldBe(nameof(AllTypesOfParams<string>.NamedCascadingValue));
		//	first.Value.ShouldBe(42);

		//	var second = result[1];
		//	second.IsCascadingValue.ShouldBeFalse();
		//	second.Name.ShouldBe(nameof(AllTypesOfParams<string>.RegularParam));
		//	second.Value.ShouldBe("bar");
		//}

		//[Fact(DisplayName = "Add with a parameterSelectors for multiple RenderFragments and childBuilders as values and Build should return the correct ComponentParameters")]
		//public void Test011()
		//{
		//	// Arrange
		//	var sut = CreateSut<TwoComponentWrapper>();

		//	// Act
		//	sut.Add<Simple1>(wrapper => wrapper.First, childBuilder =>
		//		{
		//			childBuilder
		//				.Add(c => c.Header, "H1")
		//				.Add(c => c.AttrValue, "A1");
		//		})
		//		.Add<AllTypesOfParams<int>>(wrapper => wrapper.Second, childBuilder =>
		//		{
		//			childBuilder
		//				.Add(c => c.RegularParam, "test");
		//		});
		//	var result = sut.Build();

		//	// Assert
		//	result.Count.ShouldBe(2);

		//	var first = result[0];
		//	first.IsCascadingValue.ShouldBeFalse();
		//	first.Name.ShouldBe(nameof(TwoComponentWrapper.First));
		//	first.Value.ShouldBeOfType<RenderFragment>();

		//	var second = result[1];
		//	second.IsCascadingValue.ShouldBeFalse();
		//	second.Name.ShouldBe(nameof(TwoComponentWrapper.Second));
		//	second.Value.ShouldBeOfType<RenderFragment>();
		//}

		//[Fact(DisplayName = "AddChildContent with a childBuilders and Build should return the correct ComponentParameters")]
		//public void Test012()
		//{
		//	// Arrange
		//	var sut = CreateSut<Wrapper>();

		//	// Act
		//	sut.AddChildContent<Simple1>(childBuilder =>
		//	{
		//		childBuilder
		//			.Add(c => c.Header, "H1")
		//			.Add(c => c.AttrValue, "A1");
		//	});
		//	var result = sut.Build();

		//	// Assert
		//	result.Count.ShouldBe(1);

		//	var first = result[0];
		//	first.IsCascadingValue.ShouldBeFalse();
		//	first.Name.ShouldBe(nameof(Wrapper.ChildContent));
		//	first.Value.ShouldBeOfType<RenderFragment>();
		//}

		//[Fact(DisplayName = "AddChildContent with a string markup and Build should return the correct ComponentParameters")]
		//public void Test013()
		//{
		//	// Arrange
		//	var sut = CreateSut<Wrapper>();

		//	// Act
		//	sut.AddChildContent("x");
		//	var result = sut.Build();

		//	// Assert
		//	result.Count.ShouldBe(1);

		//	var first = result[0];
		//	first.IsCascadingValue.ShouldBeFalse();
		//	first.Name.ShouldBe(nameof(Wrapper.ChildContent));
		//	first.Value.ShouldBeOfType<RenderFragment>();
		//}

		//[Fact(DisplayName = "Add unnamed CascadingParameter with a value and Build should return the correct ComponentParameters")]
		//public void Test014()
		//{
		//	// Arrange
		//	var sut = CreateSut();
		//	const int value = 42;

		//	// Act
		//	sut.Add(value);
		//	var result = sut.Build();

		//	// Assert
		//	result.Count.ShouldBe(1);

		//	var parameter = result[0];
		//	parameter.IsCascadingValue.ShouldBeTrue();
		//	parameter.Name.ShouldBeNull();
		//	parameter.Value.ShouldBe(value);
		//}

		//[Fact(DisplayName = "AddUnmatched with a key and value and Build should return the correct ComponentParameters")]
		//public void Test015()
		//{
		//	// Arrange
		//	var sut = CreateSut();
		//	const string key = "some-unmatched-attribute";
		//	const int value = 42;

		//	// Arrange
		//	sut.AddUnmatched(key, value);
		//	var result = sut.Build();

		//	// Assert
		//	result.Count.ShouldBe(1);

		//	var parameter = result[0];
		//	parameter.IsCascadingValue.ShouldBeFalse();
		//	parameter.Name.ShouldBe(key);
		//	parameter.Value.ShouldBe(value);
		//}

		//[Fact(DisplayName = "Add duplicate name should throw Exception")]
		//public void Test100()
		//{
		//	// Arrange
		//	var sut = CreateSut();
		//	sut.Add(c => c.NamedCascadingValue, 42);

		//	// Act and Assert
		//	Assert.Throws<ArgumentException>(() => sut.Add(c => c.NamedCascadingValue, 43));
		//}

		//[Fact(DisplayName = "Add CascadingParameter (with null value) should throw Exception")]
		//public void Test101()
		//{
		//	// Arrange
		//	var sut = CreateSut();

		//	// Act and Assert
		//	Assert.Throws<ArgumentNullException>(() => sut.Add(c => c.NamedCascadingValue, null));
		//}

		//[Fact(DisplayName = "Add with a property which does not have the [Parameter] or [CascadingParameter] attribute defined should throw Exception")]
		//public void Test102()
		//{
		//	// Arrange
		//	var sut = CreateSut();

		//	// Act and Assert
		//	Assert.Throws<ArgumentException>(() => sut.Add(c => c.NoParameterProperty, 42));
		//}

		//[Fact(DisplayName = "AddChildContent without a ChildContent property defined should throw Exception")]
		//public void Test103()
		//{
		//	// Arrange
		//	var sut = CreateSut<Simple1>();

		//	// Act and Assert
		//	Assert.Throws<ArgumentException>(() => sut.AddChildContent("html"));
		//}

		//[Fact(DisplayName = "Add with a selectorExpression which is not a property should throw Exception")]
		//public void Test104()
		//{
		//	// Arrange
		//	var sut = CreateSut();

		//	// Act and Assert
		//	Assert.Throws<ArgumentException>(() => sut.Add(c => c.DummyMethod(), 42));
		//}




		//private static ComponentParameterBuilder<AllTypesOfParams<string>> CreateSut()
		//	=> CreateSut<AllTypesOfParams<string>>();

		//private static ComponentParameterBuilder<TComponent> CreateSut<TComponent>() where TComponent : IComponent
		//	=> new ComponentParameterBuilder<TComponent>();
	}

	//public class ComponentParameterBuilderTest : TestContext
	//{
	//	string GetMarkupFromRenderFragment(RenderFragment renderFragment)
	//	{
	//		return ((IRenderedFragment)Renderer.RenderFragment(renderFragment)).Markup;
	//	}

	//	[Fact(DisplayName = "All types of parameters are correctly assigned to component on render")]
	//	public void Test005()
	//	{
	//		Services.AddMockJSRuntime();

	//		var cut = RenderComponent<AllTypesOfParams<string>>(parameters => parameters
	//			.AddUnmatched("some-unmatched-attribute", "unmatched value")
	//			.Add(p => p.RegularParam, "some value")
	//			//.Add(42)
	//			.Add(p => p.UnnamedCascadingValue, 42)
	//			.Add(p => p.NamedCascadingValue, 1337)
	//			.Add(p => p.NonGenericCallback, () => throw new Exception("NonGenericCallback"))
	//			.Add(p => p.GenericCallback, args => throw new Exception("GenericCallback"))
	//			.AddChildContent(nameof(AllTypesOfParams<string>.ChildContent))
	//			.Add(p => p.OtherContent, nameof(AllTypesOfParams<string>.OtherContent))
	//			.Add(p => p.ItemTemplate, (item) => (builder) => throw new Exception("ItemTemplate"))
	//		);

	//		// assert that all parameters have been set correctly
	//		var instance = cut.Instance;
	//		instance.Attributes["some-unmatched-attribute"].ShouldBe("unmatched value");
	//		instance.RegularParam.ShouldBe("some value");
	//		instance.UnnamedCascadingValue.ShouldBe(42);
	//		instance.NamedCascadingValue.ShouldBe(1337);
	//		Should.Throw<Exception>(async () => await instance.NonGenericCallback.InvokeAsync(EventArgs.Empty)).Message.ShouldBe("NonGenericCallback");
	//		Should.Throw<Exception>(async () => await instance.GenericCallback.InvokeAsync(EventArgs.Empty)).Message.ShouldBe("GenericCallback");

	//		GetMarkupFromRenderFragment(instance.ChildContent!).ShouldBe(nameof(AllTypesOfParams<string>.ChildContent));
	//		GetMarkupFromRenderFragment(instance.OtherContent!).ShouldBe(nameof(AllTypesOfParams<string>.OtherContent));
	//		Should.Throw<Exception>(() => instance.ItemTemplate!("")(new RenderTreeBuilder())).Message.ShouldBe("ItemTemplate");
	//	}

	//	[Fact]
	//	public void MyTestMethod()
	//	{
	//		Services.AddMockJSRuntime();

	//		var cut = RenderComponent<AllTypesOfParams<string>>(parameterBuilder => parameterBuilder
	//			.AddUnmatched("some-unmatched-attribute", "unmatched value")
	//			.Add(p => p.RegularParam, "some value")
	//			.Add(p => p.UnnamedCascadingValue, 42)
	//			.Add(p => p.NamedCascadingValue, 1337)
	//			.Add(p => p.NonGenericCallback, () => throw new Exception("NonGenericCallback"))
	//			.Add(p => p.GenericCallback, (EventArgs args) => throw new Exception("GenericCallback"))
	//			.Add(p => p.ChildContent, nameof(AllTypesOfParams<string>.ChildContent))
	//			.Add(p => p.OtherContent, nameof(AllTypesOfParams<string>.OtherContent))
	//			.Add(p => p.ItemTemplate, (item) => (builder) => throw new Exception("ItemTemplate"))
	//		);

	//		// assert that all parameters have been set correctly
	//		var instance = cut.Instance;
	//		instance.Attributes["some-unmatched-attribute"].ShouldBe("unmatched value");
	//		instance.RegularParam.ShouldBe("some value");
	//		instance.UnnamedCascadingValue.ShouldBe(42); // Currently fails.
	//		instance.NamedCascadingValue.ShouldBe(1337);
	//		Should.Throw<Exception>(async () => await instance.NonGenericCallback.InvokeAsync(EventArgs.Empty)).Message.ShouldBe("NonGenericCallback");
	//		Should.Throw<Exception>(async () => await instance.GenericCallback.InvokeAsync(EventArgs.Empty)).Message.ShouldBe("GenericCallback");
	//	}
	//}
}
