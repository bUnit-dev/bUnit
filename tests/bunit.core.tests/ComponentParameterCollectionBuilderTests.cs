using System.Linq.Expressions;

namespace Bunit;

public partial class ComponentParameterCollectionBuilderTests : TestContext
{
	private ComponentParameterCollectionBuilder<Params> Builder { get; } = new();

	private bool EventCallbackCalled { get; set; }

	private void VerifyParameter<T>(string expectedName, T? expectedInput)
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

	[UIFact(DisplayName = "Null for parameter selector throws")]
	public void Test000()
	{
		Should.Throw<ArgumentNullException>(() => Builder.Add(default!, 42));
	}

	[UIFact(DisplayName = "Selecting a non property with parameter selector throws")]
	public void Test0000()
	{
		Should.Throw<ArgumentException>(() => Builder.Add(x => x.Field, 42));
	}

	[UIFact(DisplayName = "Selecting a non parameter property with parameter selector throws")]
	public void Test00000()
	{
		Should.Throw<ArgumentException>(() => Builder.Add(x => x.NonParamProp, new object()));
	}

	[UIFact(DisplayName = "Value type with parameter selector")]
	public void Test001()
	{
		Builder.Add(x => x.ValueTypeParam, 42);
		VerifyParameter("ValueTypeParam", 42);
	}

	[UIFact(DisplayName = "Null for struct? with parameter selector")]
	public void Test002()
	{
		Builder.Add(x => x.NullableValueTypeParam, null);
		VerifyParameter<int?>("NullableValueTypeParam", null);
	}

	[UIFact(DisplayName = "Struct? with parameter selector")]
	public void Test003()
	{
		Builder.Add(x => x.NullableValueTypeParam, 1234);
		VerifyParameter("NullableValueTypeParam", 1234);
	}

	[UIFact(DisplayName = "Object with parameter selector")]
	public void Test004()
	{
		var input = new object();
		Builder.Add(x => x.Param, input);
		VerifyParameter("Param", input);
	}

	[UIFact(DisplayName = "Null for object with parameter selector")]
	public void Test005()
	{
		Builder.Add(x => x.Param, null);
		VerifyParameter<object>("Param", null);
	}

	[UIFact(DisplayName = "EventCallback with parameter selector")]
	public void Test010()
	{
		var input = EventCallback.Empty;
		Builder.Add(x => x.EC, input);
		VerifyParameter("EC", input);
	}

	[UIFact(DisplayName = "Null to EventCallback throws")]
	public void Test011()
	{
		Should.Throw<ArgumentNullException>(() => Builder.Add(x => x.EC, null!));
	}

	[UIFact(DisplayName = "Null for EventCallback? with parameter selector")]
	public void Test011_2()
	{
		Builder.Add<EventCallback?>(x => x.NullableEC, null);
		VerifyParameter<EventCallback?>("NullableEC", null);
	}

	[UIFact(DisplayName = "Action to EventCallback with parameter selector")]
	public async Task Test012()
	{
		Builder.Add(x => x.EC, () => { EventCallbackCalled = true; });
		await VerifyEventCallbackAsync("EC");
	}

	[UIFact(DisplayName = "Action<object> to EventCallback with parameter selector")]
	public async Task Test013()
	{
		Builder.Add(x => x.EC, _ => { EventCallbackCalled = true; });
		await VerifyEventCallbackAsync("EC");
	}

	[UIFact(DisplayName = "Func<Task> to EventCallback with parameter selector")]
	public async Task Test014()
	{
		Builder.Add(x => x.EC, () => { EventCallbackCalled = true; return Task.CompletedTask; });
		await VerifyEventCallbackAsync("EC");
	}

	[UIFact(DisplayName = "Action to EventCallback? with parameter selector")]
	public async Task Test015()
	{
		Builder.Add(x => x.NullableEC, () => { EventCallbackCalled = true; });
		await VerifyEventCallbackAsync("NullableEC");
	}

	[UIFact(DisplayName = "Action<object> to EventCallback? with parameter selector")]
	public async Task Test016()
	{
		Builder.Add(x => x.NullableEC, _ => { EventCallbackCalled = true; });
		await VerifyEventCallbackAsync("NullableEC");
	}

	[UIFact(DisplayName = "Func<Task> to EventCallback? with parameter selector")]
	public async Task Test017()
	{
		Builder.Add(x => x.NullableEC, () => { EventCallbackCalled = true; return Task.CompletedTask; });
		await VerifyEventCallbackAsync("NullableEC");
	}

	[UIFact(DisplayName = "EventCallback<T> with parameter selector")]
	public void Test018()
	{
		var input = EventCallback<EventArgs>.Empty;
		Builder.Add(x => x.ECWithArgs, input);
		VerifyParameter("ECWithArgs", input);
	}

	[UIFact(DisplayName = "Action to EventCallback<T> with parameter selector")]
	public async Task Test019()
	{
		Builder.Add(x => x.ECWithArgs, () => { EventCallbackCalled = true; });
		await VerifyEventCallbackAsync<EventArgs>("ECWithArgs");
	}

	[UIFact(DisplayName = "Action<object> to EventCallback<T> with parameter selector")]
	public async Task Test020()
	{
		Builder.Add(x => x.ECWithArgs, _ => { EventCallbackCalled = true; });
		await VerifyEventCallbackAsync<EventArgs>("ECWithArgs");
	}

	[UIFact(DisplayName = "Func<Task> to EventCallback<T> with parameter selector")]
	public async Task Test021()
	{
		Builder.Add(x => x.ECWithArgs, () => { EventCallbackCalled = true; return Task.CompletedTask; });
		await VerifyEventCallbackAsync<EventArgs>("ECWithArgs");
	}

	[UIFact(DisplayName = "EventCallback<T> with parameter selector")]
	public void Test022()
	{
		var input = EventCallback<EventArgs>.Empty;
		Builder.Add(x => x.NullableECWithArgs, input);
		VerifyParameter("NullableECWithArgs", input);
	}

	[UIFact(DisplayName = "Action to EventCallback<T> with parameter selector")]
	public async Task Test023()
	{
		Builder.Add(x => x.NullableECWithArgs, () => { EventCallbackCalled = true; });
		await VerifyEventCallbackAsync<EventArgs>("NullableECWithArgs");
	}

	[UIFact(DisplayName = "Action<object> to EventCallback<T> with parameter selector")]
	public async Task Test024()
	{
		Builder.Add(x => x.NullableECWithArgs, _ => { EventCallbackCalled = true; });
		await VerifyEventCallbackAsync<EventArgs>("NullableECWithArgs");
	}

	[UIFact(DisplayName = "Func<Task> to EventCallback<T> with parameter selector")]
	public async Task Test025()
	{
		Builder.Add(x => x.NullableECWithArgs, () => { EventCallbackCalled = true; return Task.CompletedTask; });
		await VerifyEventCallbackAsync<EventArgs>("NullableECWithArgs");
	}

	[UIFact(DisplayName = "ChildContent can be passed as RenderFragment")]
	public void Test030()
	{
		RenderFragment input = b => b.AddMarkupContent(0, string.Empty);
		Builder.AddChildContent(input);
		VerifyParameter<RenderFragment>("ChildContent", input);
	}

	[UIFact(DisplayName = "Calling AddChildContent when TCompnent does not have a parameter named ChildContent throws")]
	public void Test031()
	{
		RenderFragment input = b => b.AddMarkupContent(0, string.Empty);
		Assert.Throws<ArgumentException>(() => new ComponentParameterCollectionBuilder<NoParams>().AddChildContent(input));
		Assert.Throws<ArgumentException>(() => new ComponentParameterCollectionBuilder<NonChildContentParameter>().AddChildContent(input));
	}

	[UIFact(DisplayName = "ChildContent can be passed as a nested component parameter builder")]
	public void Test032()
	{
		Builder.AddChildContent<InhertedParams>(parameters => parameters.Add(p => p.ValueTypeParam, 42));

		var actual = Builder.Build().ShouldHaveSingleItem()
			.ShouldBeParameter<RenderFragment>("ChildContent", isCascadingValue: false);
		var actualComponent = RenderWithRenderFragment<InhertedParams>(actual);
		actualComponent.Instance.ValueTypeParam.ShouldBe(42);
	}

	[UIFact(DisplayName = "ChildContent can be passed as a child component without parameters")]
	public void Test033()
	{
		Builder.AddChildContent<NoParams>();

		var actual = Builder.Build().ShouldHaveSingleItem()
			.ShouldBeParameter<RenderFragment>("ChildContent", isCascadingValue: false);
		var actualComponent = RenderWithRenderFragment<NoParams>(actual);
		actualComponent.Instance.ShouldBeOfType<NoParams>();
	}

	[UIFact(DisplayName = "ChildContent can be passed as a markup string")]
	public void Test034()
	{
		var input = "<p>42</p>";
		Builder.AddChildContent(input);

		var actual = Builder.Build().ShouldHaveSingleItem()
			.ShouldBeParameter<RenderFragment>("ChildContent", isCascadingValue: false);
		var actualComponent = RenderWithRenderFragment(actual);
		actualComponent.Markup.ShouldBe(input);
	}

	[UIFact(DisplayName = "RenderFragment can be passed as a nested component parameter builder")]
	public void Test040()
	{
		Builder.Add<InhertedParams>(x => x.OtherFragment, parameters => parameters.Add(p => p.ValueTypeParam, 42));

		var actual = Builder.Build().ShouldHaveSingleItem()
			.ShouldBeParameter<RenderFragment>("OtherFragment", isCascadingValue: false);
		var actualComponent = RenderWithRenderFragment<InhertedParams>(actual);
		actualComponent.Instance.ValueTypeParam.ShouldBe(42);
	}

	[UIFact(DisplayName = "RenderFragment can be passed as a child component without parameters")]
	public void Test041()
	{
		Builder.Add<NoParams>(x => x.OtherFragment);

		var actual = Builder.Build().ShouldHaveSingleItem()
			.ShouldBeParameter<RenderFragment>("OtherFragment", isCascadingValue: false);
		var actualComponent = RenderWithRenderFragment<NoParams>(actual);
		actualComponent.Instance.ShouldBeOfType<NoParams>();
	}

	[UIFact(DisplayName = "RenderFragment can be passed as a markup string")]
	public void Test042()
	{
		var input = "<p>42</p>";
		Builder.Add(x => x.OtherFragment, input);

		var actual = Builder.Build().ShouldHaveSingleItem()
			.ShouldBeParameter<RenderFragment>("OtherFragment", isCascadingValue: false);
		var actualComponent = RenderWithRenderFragment(actual);
		actualComponent.Markup.ShouldBe(input);
	}

	[UIFact(DisplayName = "RenderFragment can be passed RenderFragment")]
	public void Test043()
	{
		RenderFragment input = b => b.AddMarkupContent(0, string.Empty);

		Builder.Add(x => x.OtherFragment, input);

		Builder.Build().ShouldHaveSingleItem()
			.ShouldBeParameter("OtherFragment", input, isCascadingValue: false);
	}

	[UIFact(DisplayName = "RenderFragment can be passed multiple times")]
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

	[UIFact(DisplayName = "RenderFragment<T>? can be passed as RenderFragment")]
	public void Test050()
	{
		RenderFragment<string> input = s => b => b.AddMarkupContent(0, s);

		Builder.Add(x => x.Template, input);

		Builder.Build().ShouldHaveSingleItem()
			.ShouldBeParameter("Template", input, isCascadingValue: false);
	}

	[UIFact(DisplayName = "RenderFragment<T>? can be passed lambda builder")]
	public void Test051()
	{
		var input = "FOO";
		Builder.Add(x => x.Template, value => value);

		var actual = Builder.Build().ShouldHaveSingleItem()
			.ShouldBeParameter<RenderFragment<string>>("Template", isCascadingValue: false);
		var actualComponent = RenderWithRenderFragment(actual(input));
		actualComponent.Markup.ShouldBe(input);
	}

	[UIFact(DisplayName = "RenderFragment<T>? can be passed as nested object builder")]
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

	[UIFact(DisplayName = "RenderFragment<T> can be passed multiple times")]
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

	[UIFact(DisplayName = "Cascading values can be passed using Add and parameter selector")]
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

	[UIFact(DisplayName = "AddCascadingValue can add unnamed cascading values")]
	public void Test061()
	{
		var input = "FOO";

		Builder.AddCascadingValue(input);

		Builder.Build().ShouldHaveSingleItem()
			.ShouldBeParameter(null, input, isCascadingValue: true);
	}

	[UIFact(DisplayName = "AddCascadingValue can add named cascading values")]
	public void Test062()
	{
		var name = "NAME";
		var input = "FOO";

		Builder.AddCascadingValue(name, input);

		Builder.Build().ShouldHaveSingleItem()
			.ShouldBeParameter(name, input, isCascadingValue: true);
	}

	[UIFact(DisplayName = "AddUnmatched can add unmatched empty value attributes as parameters")]
	public void Test070()
	{
		var name = "NAME";

		Builder.AddUnmatched(name);

		Builder.Build()
			.ShouldHaveSingleItem()
			.ShouldBeParameter<object>(name, null, isCascadingValue: false);
	}

	[UIFact(DisplayName = "AddUnmatched can add unmatched value attributes as parameters")]
	public void Test071()
	{
		var name = "NAME";
		var value = "FOO";

		Builder.AddUnmatched(name, value);

		Builder.Build()
			.ShouldHaveSingleItem()
			.ShouldBeParameter(name, value, isCascadingValue: false);
	}

	[UITheory(DisplayName = "AddUnmatched throws if name is null or whitespace")]
	[InlineData("")]
	[InlineData(" ")]
	public void Test072(string emptyName)
	{
		Should.Throw<ArgumentException>(() => Builder.AddUnmatched(emptyName));
	}

	[UIFact(DisplayName = "AddUnmatched throws if component doesnt have an Parameter(CaptureUnmatchedValues = true)")]
	public void Test073()
	{
		var sut = new ComponentParameterCollectionBuilder<NoParams>();

		Should.Throw<ArgumentException>(() => sut.AddUnmatched("foo"));
	}

	[UIFact(DisplayName = "Can select parameters inherited from base component ")]
	public void Test101()
	{
		Builder.Add(x => x.Param, new object());

		Builder.Build().ShouldHaveSingleItem();
	}

	[UIFact(DisplayName = "TryAdd returns false when parameter does not exist on component")]
	public void Test200()
	{
		var sut = new ComponentParameterCollectionBuilder<NoParams>();

		var result = sut.TryAdd("FOO", "VALUE");

		result.ShouldBeFalse();
		sut.Build().Count.ShouldBe(0);
	}

	[UIFact(DisplayName = "TryAdd returns true when parameter exists on component")]
	public void Test201()
	{
		var name = nameof(Params.ValueTypeParam);
		var input = 42;

		var result = Builder.TryAdd(name, input);

		result.ShouldBeTrue();
		Builder.Build().ShouldHaveSingleItem()
			.ShouldBeParameter(name, input, isCascadingValue: false);
	}

	[UIFact(DisplayName = "TryAdd returns true when unnamed cascading parameter exists on component")]
	public void Test202()
	{
		var name = nameof(Params.CC);
		var input = 42;

		var result = Builder.TryAdd(name, input);

		result.ShouldBeTrue();
		Builder.Build().ShouldHaveSingleItem()
			.ShouldBeParameter(null, input, isCascadingValue: true);
	}

	[UIFact(DisplayName = "TryAdd returns true when named cascading parameter exists on component")]
	public void Test203()
	{
		var name = nameof(Params.NamedCC);
		var input = 42;

		var result = Builder.TryAdd(name, input);

		result.ShouldBeTrue();
		Builder.Build().ShouldHaveSingleItem()
			.ShouldBeParameter(name + "NAME", input, isCascadingValue: true);
	}

	[UIFact(DisplayName = "Add of inherited overriden parameter works")]
	public void Test300()
	{
		var sut = new ComponentParameterCollectionBuilder<InheritedParamsWithOverride>();

		sut.Add(x => x.Value, true);

		sut.Build()
			.ShouldHaveSingleItem()
			.ShouldBeParameter("Value", true, isCascadingValue: false);
	}

	[UIFact(DisplayName = "Add of inherited parameter works")]
	public void Test301()
	{
		var sut = new ComponentParameterCollectionBuilder<InheritedParamsWithoutOverride>();

		sut.Add(x => x.Value, true);

		sut.Build()
			.ShouldHaveSingleItem()
			.ShouldBeParameter("Value", true, isCascadingValue: false);
	}

	[UIFact(DisplayName = "AddChildContent on generic child content parameter throws")]
	public void Test302()
	{
		Action addChildContent = () => new ComponentParameterCollectionBuilder<TemplatedChildContent>().AddChildContent("<p>item</p>");

		addChildContent.ShouldThrow<ArgumentException>();
	}

	[UIFact(DisplayName = "Add with generic child content works")]
	public void Test303()
	{
		var sut = new ComponentParameterCollectionBuilder<TemplatedChildContent>();

		sut.Add(p => p.ChildContent, _ => "<p>item</p>");

		sut.Build()
			.ShouldHaveSingleItem()
			.ShouldBeParameter<RenderFragment<string>?>(nameof(TemplatedChildContent.ChildContent), false);
	}

	[UIFact(DisplayName = "Bind should add Value and ValueChanged event")]
	public void Test304()
	{
		var sut = new ComponentParameterCollectionBuilder<SimpleBind>();

		sut.Bind(p => p.Value, "init", _ => { });

		sut.Build().ShouldAllBe(
			x => x.ShouldBeParameter("Value", "init", false),
			x => x.ShouldBeParameter<EventCallback<string>>("ValueChanged", false));
	}

	[UIFact(DisplayName = "Bind should add Expression event when available")]
	public void Test305()
	{
		var sut = new ComponentParameterCollectionBuilder<FullBind>();

		sut.Bind(p => p.Foo, "init", _ => { });

		sut
			.Build()
			.Where(p => string.Equals(p.Name, "FooExpression", StringComparison.Ordinal))
			.ShouldHaveSingleItem();
	}

	[UIFact(DisplayName = "Throw an exception when no Changed event available")]
	public void Test306()
	{
		var sut = new ComponentParameterCollectionBuilder<NoTwoWayBind>();

		Action action = () => sut.Bind(p => p.Value, "init", _ => { });

		action.ShouldThrow<InvalidOperationException>();
	}

	[UIFact(DisplayName = "Throw an exception when cascading parameter")]
	public void Test307()
	{
		var sut = new ComponentParameterCollectionBuilder<ComponentWithCascadingParameter>();

		Action action = () => sut.Bind(p => p.Value, "init", _ => { });

		action.ShouldThrow<ArgumentException>();
	}

	[UIFact(DisplayName = "Throw an exception when Changed event is not a public parameter")]
	public void Test308()
	{
		var sut = new ComponentParameterCollectionBuilder<InvalidTwoWayBind>();

		Action action = () => sut.Bind(p => p.Value, "init", _ => { });

		action.ShouldThrow<InvalidOperationException>();
	}

	[UIFact(DisplayName = "Throw exception when parameter selector is null")]
	public void Test309()
	{
		var sut = new ComponentParameterCollectionBuilder<SimpleBind>();

		Action action = () => sut.Bind(null, "init", _ => { });

		action.ShouldThrow<ArgumentNullException>();
	}

	[UIFact(DisplayName = "Throw exception when changed action is null")]
	public void Test310()
	{
		var sut = new ComponentParameterCollectionBuilder<SimpleBind>();

		Action action = () => sut.Bind(p => p.Value, "init", null);

		action.ShouldThrow<ArgumentNullException>();
	}

	[UIFact(DisplayName = "Throw exception when wrong parameter is changed action")]
	public void Test311()
	{
		var sut = new ComponentParameterCollectionBuilder<SimpleBind>();

		Action action = () => sut.Bind(p => p.ValueChanged, new EventCallback<string>(), _ => { });

		action.ShouldThrow<ArgumentException>();
	}

	[UIFact(DisplayName = "Throw exception when wrong parameter is expression")]
	public void Test312()
	{
		const string value = "some string";
		var sut = new ComponentParameterCollectionBuilder<FullBind>();

		Action action = () => sut.Bind(p => p.FooExpression, () => value, _ => { });

		action.ShouldThrow<ArgumentException>();
	}

	[UIFact(DisplayName = "Properties with Changed at the end, which are not of type EventCallback can be bound")]
	public void Test313()
	{
		var sut = new ComponentParameterCollectionBuilder<ValidNamesComponent>();

		Action action = () => sut.Bind(p => p.LastChanged, DateTime.Now, _ => { });

		action.ShouldNotThrow();
	}

	[UIFact(DisplayName = "Properties with Expression at the end, which are not of type expression can be bound")]
	public void Test314()
	{
		var sut = new ComponentParameterCollectionBuilder<ValidNamesComponent>();

		Action action = () => sut.Bind(p => p.FacialExpression, string.Empty, _ => { }, () => string.Empty);

		action.ShouldNotThrow();
	}

	private class Params : ComponentBase
	{
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

	private sealed class NoParams : ComponentBase { }
	private sealed class NonChildContentParameter : ComponentBase { public RenderFragment? ChildContent { get; set; } }
	private sealed class InhertedParams : Params { }
	private abstract class ParamsBase<T> : ComponentBase
	{
		public abstract T Value { get; set; }
	}

	private class InheritedParamsWithOverride : ParamsBase<bool?>
	{
		[Parameter] public override bool? Value { get; set; }
	}

	private sealed class InheritedParamsWithoutOverride : InheritedParamsWithOverride
	{ }

	private sealed class TemplatedChildContent : ComponentBase
	{
		[Parameter] public RenderFragment<string>? ChildContent { get; set; }
	}

	private sealed class NoTwoWayBind : ComponentBase
	{
		[Parameter]
		public string Value { get; set; }
	}

	private sealed class InvalidTwoWayBind : ComponentBase
	{
		[Parameter]
		public string Value { get; set; }

		public EventCallback<string> ValueChanged { get; set; }
	}

	private sealed class ComponentWithCascadingParameter : ComponentBase
	{
		[CascadingParameter] public string Value { get; set; } = string.Empty;
		[Parameter] public EventCallback<string> ValueChanged { get; set; }
	}

	private sealed class ValidNamesComponent : ComponentBase
	{
		[Parameter] public DateTime LastChanged { get; set; }
		[Parameter] public EventCallback<DateTime> LastChangedChanged { get; set; }

		[Parameter] public string FacialExpression { get; set; }
		[Parameter] public EventCallback<string> FacialExpressionChanged { get; set; }
		[Parameter] public Expression<Func<string>> FacialExpressionExpression { get; set; }
	}
}
