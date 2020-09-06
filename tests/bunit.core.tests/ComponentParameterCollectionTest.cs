using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bunit.Extensions;
using Bunit.Rendering;
using Bunit.TestAssets.SampleComponents;
using Bunit.TestDoubles.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit;

namespace Bunit
{
	public class ComponentParameterCollectionTest
	{
		private static readonly TestContext Context = new TestContext();

		static ComponentParameterCollectionTest()
		{
			Context.Services.AddMockJSRuntime();
		}

		private static IRenderedComponent<Params> RenderWithRenderFragment(RenderFragment renderFragment)
		{
			var res = (IRenderedFragment)Context.Renderer.RenderFragment(renderFragment);
			return res.FindComponent<Params>();
		}

		[Fact(DisplayName = "ComponentParameters can be added to collection")]
		public void Test001()
		{
			var sut = new ComponentParameterCollection();
			var p = ComponentParameter.CreateParameter("attr", 42);

			sut.Add(p);

			sut.Count.ShouldBe(1);
			sut.Contains(p).ShouldBeTrue();
		}

		[Fact(DisplayName = "Add() throws if invalid parameter is passed to it")]
		public void Test002()
		{
			var sut = new ComponentParameterCollection();
			var p = new ComponentParameter();

			Should.Throw<ArgumentException>(() => sut.Add(p));
		}

		[Fact(DisplayName = "ToComponentRenderFragment creates RenderFragment for component when empty")]
		public void Test010()
		{
			var sut = new ComponentParameterCollection();

			var rf = sut.ToComponentRenderFragment<Params>();

			var c = RenderWithRenderFragment(rf).Instance;
			c.VerifyParamsHaveDefaultValues();
		}

		[Fact(DisplayName = "ToComponentRenderFragment passes regular params to component in RenderFragment")]
		public void Test011()
		{
			var sut = new ComponentParameterCollection();
			sut.Add(ComponentParameter.CreateParameter(nameof(Params.Param), "FOO"));
			sut.Add(ComponentParameter.CreateParameter(nameof(Params.ValueTypeParam), 42));
			sut.Add(ComponentParameter.CreateParameter(nameof(Params.NullableValueTypeParam), 1337));

			var rf = sut.ToComponentRenderFragment<Params>();

			var c = RenderWithRenderFragment(rf).Instance;
			c.Param.ShouldBe("FOO");
			c.ValueTypeParam.ShouldBe(42);
			c.NullableValueTypeParam.ShouldBe(1337);
		}

		[Fact(DisplayName = "ToComponentRenderFragment passes single ChildContent param to component in RenderFragment")]
		public void Test012()
		{
			var sut = new ComponentParameterCollection();
			var cc = ComponentParameter.CreateParameter(nameof(Params.ChildContent), (RenderFragment)(b => b.AddMarkupContent(0, "FOO")));
			sut.Add(cc);

			var rf = sut.ToComponentRenderFragment<Params>();

			var rc = RenderWithRenderFragment(rf);
			rc.Markup.ShouldBe("FOO");
		}

		[Fact(DisplayName = "ToComponentRenderFragment passes multiple ChildContent params to component in RenderFragment")]
		public void Test013()
		{
			var sut = new ComponentParameterCollection();
			var ccFoo = ComponentParameter.CreateParameter(nameof(Params.ChildContent), (RenderFragment)(b => b.AddMarkupContent(0, "FOO")));
			var ccBar = ComponentParameter.CreateParameter(nameof(Params.ChildContent), (RenderFragment)(b => b.AddMarkupContent(0, "BAR")));
			var ccBaz = ComponentParameter.CreateParameter(nameof(Params.ChildContent), (RenderFragment)(b => b.AddMarkupContent(0, "BAZ")));
			sut.Add(ccFoo);
			sut.Add(ccBar);
			sut.Add(ccBaz);

			var rf = sut.ToComponentRenderFragment<Params>();

			var rc = RenderWithRenderFragment(rf);
			rc.Markup.ShouldBe("FOOBARBAZ");
		}

		[Fact(DisplayName = "ToComponentRenderFragment passes single RenderFragment param to component in RenderFragment")]
		public void Test014()
		{
			var sut = new ComponentParameterCollection();
			var cc = ComponentParameter.CreateParameter(nameof(Params.OtherFragment), (RenderFragment)(b => b.AddMarkupContent(0, "FOO")));
			sut.Add(cc);

			var rf = sut.ToComponentRenderFragment<Params>();

			var rc = RenderWithRenderFragment(rf);
			rc.Markup.ShouldBe("FOO");
		}

		[Fact(DisplayName = "ToComponentRenderFragment passes multiple RenderFragment params to component in RenderFragment")]
		public void Test015()
		{
			var sut = new ComponentParameterCollection();
			var ccFoo = ComponentParameter.CreateParameter(nameof(Params.OtherFragment), (RenderFragment)(b => b.AddMarkupContent(0, "FOO")));
			var ccBar = ComponentParameter.CreateParameter(nameof(Params.OtherFragment), (RenderFragment)(b => b.AddMarkupContent(0, "BAR")));
			var ccBaz = ComponentParameter.CreateParameter(nameof(Params.OtherFragment), (RenderFragment)(b => b.AddMarkupContent(0, "BAZ")));
			sut.Add(ccFoo);
			sut.Add(ccBar);
			sut.Add(ccBaz);

			var rf = sut.ToComponentRenderFragment<Params>();

			var rc = RenderWithRenderFragment(rf);
			rc.Markup.ShouldBe("FOOBARBAZ");
		}

		[Fact(DisplayName = "ToComponentRenderFragment passes unmatched attributes to component in RenderFragment")]
		public void Test016()
		{
			var sut = new ComponentParameterCollection();
			var attr = ComponentParameter.CreateParameter("attr", "");
			sut.Add(attr);

			var rf = sut.ToComponentRenderFragment<Params>();

			var c = RenderWithRenderFragment(rf).Instance;
			c.Attributes?.ContainsKey("attr").ShouldBeTrue();
		}

		[Fact(DisplayName = "ToComponentRenderFragment passes EventCallback params to component in RenderFragment")]
		public void Test017()
		{
			// arrange
			var sut = new ComponentParameterCollection();
			var ec1 = EventCallback.Factory.Create(this, () => { });
			var ec2 = EventCallback.Factory.Create(this, () => { });
			var ec3 = EventCallback.Factory.Create<EventArgs>(this, (e) => { });
			var ec4 = EventCallback.Factory.Create<EventArgs>(this, (e) => { });
			var p1 = ComponentParameter.CreateParameter(nameof(Params.NullableEC), ec1);
			var p2 = ComponentParameter.CreateParameter(nameof(Params.EC), ec2);
			var p3 = ComponentParameter.CreateParameter(nameof(Params.NullableECWithArgs), ec3);
			var p4 = ComponentParameter.CreateParameter(nameof(Params.ECWithArgs), ec4);
			sut.Add(p1);
			sut.Add(p2);
			sut.Add(p3);
			sut.Add(p4);

			// act
			var rf = sut.ToComponentRenderFragment<Params>();

			// assert
			var c = RenderWithRenderFragment(rf).Instance;
			c.NullableEC.ShouldBe(ec1);
			c.EC.ShouldBe(ec2);
			c.NullableECWithArgs.ShouldBe(ec3);
			c.ECWithArgs.ShouldBe(ec4);
		}

		[Fact(DisplayName = "ToComponentRenderFragment passes single template param to component in RenderFragment")]
		public void Test018()
		{
			var sut = new ComponentParameterCollection();
			var t = ComponentParameter.CreateParameter(nameof(Params.Template), (RenderFragment<string>)(s => b => b.AddMarkupContent(0, s)));
			sut.Add(t);

			var rf = sut.ToComponentRenderFragment<Params>();

			var rc = RenderWithRenderFragment(rf);
			rc.Markup.ShouldBe(Params.TemplateContent);
		}

		[Fact(DisplayName = "ToComponentRenderFragment passes multiple template params to component in RenderFragment")]
		public void Test019()
		{
			var sut = new ComponentParameterCollection();
			var t1 = ComponentParameter.CreateParameter(nameof(Params.Template), (RenderFragment<string>)(s => b => b.AddMarkupContent(0, $"{s}1")));
			var t2 = ComponentParameter.CreateParameter(nameof(Params.Template), (RenderFragment<string>)(s => b => b.AddMarkupContent(0, $"{s}2")));
			sut.Add(t1);
			sut.Add(t2);

			var rf = sut.ToComponentRenderFragment<Params>();

			var rc = RenderWithRenderFragment(rf);
			rc.Markup.ShouldBe($"{Params.TemplateContent}1{Params.TemplateContent}2");
		}

		[Fact(DisplayName = "ToComponentRenderFragment with different template types to same param throws")]
		public void Test020()
		{
			var sut = new ComponentParameterCollection();
			var t1 = ComponentParameter.CreateParameter(nameof(Params.Template), (RenderFragment<string>)(s => b => b.AddMarkupContent(0, $"{s}1")));
			var t2 = ComponentParameter.CreateParameter(nameof(Params.Template), (RenderFragment<int>)(s => b => b.AddMarkupContent(0, $"{s}2")));
			sut.Add(t1);
			sut.Add(t2);

			var rf = sut.ToComponentRenderFragment<Params>();

			Should.Throw<ArgumentException>(() => RenderWithRenderFragment(rf));
		}

		[Fact(DisplayName = "ToComponentRenderFragment passes skips null RenderFragment params")]
		public void Test030()
		{
			var sut = new ComponentParameterCollection();
			var ccFoo = ComponentParameter.CreateParameter(nameof(Params.OtherFragment), default(RenderFragment));
			var ccBar = ComponentParameter.CreateParameter(nameof(Params.OtherFragment), (RenderFragment)(b => b.AddMarkupContent(0, "BAR")));
			sut.Add(ccFoo);
			sut.Add(ccBar);

			var rf = sut.ToComponentRenderFragment<Params>();

			var rc = RenderWithRenderFragment(rf);
			rc.Markup.ShouldBe("BAR");
		}

		[Fact(DisplayName = "ToComponentRenderFragment throws if same regular param is added twice")]
		public void Test040()
		{
			var sut = new ComponentParameterCollection();
			sut.Add(ComponentParameter.CreateParameter(nameof(Params.Param), "FOO"));
			sut.Add(ComponentParameter.CreateParameter(nameof(Params.Param), "BAR"));

			var rf = sut.ToComponentRenderFragment<Params>();

			Should.Throw<ArgumentException>(() => RenderWithRenderFragment(rf));
		}

		[Fact(DisplayName = "ToComponentRenderFragment throws if same regular null value param is added twice")]
		public void Test041()
		{
			var sut = new ComponentParameterCollection();
			sut.Add(ComponentParameter.CreateParameter(nameof(Params.Param), null));
			sut.Add(ComponentParameter.CreateParameter(nameof(Params.Param), null));

			var rf = sut.ToComponentRenderFragment<Params>();

			Should.Throw<ArgumentException>(() => RenderWithRenderFragment(rf));
		}

		[Fact(DisplayName = "ToComponentRenderFragment wraps component in unnamed cascading values in RenderFragment")]
		public void Test050()
		{
			var sut = new ComponentParameterCollection();
			sut.Add(ComponentParameter.CreateCascadingValue(null, "FOO"));

			var rf = sut.ToComponentRenderFragment<Params>();

			var c = RenderWithRenderFragment(rf).Instance;
			c.NullableCC.ShouldBe("FOO");
		}

		[Fact(DisplayName = "ToComponentRenderFragment wraps component in multiple unnamed cascading values in RenderFragment")]
		public void Test051()
		{
			var sut = new ComponentParameterCollection();
			sut.Add(ComponentParameter.CreateCascadingValue(null, "FOO"));
			sut.Add(ComponentParameter.CreateCascadingValue(null, 42));

			var rf = sut.ToComponentRenderFragment<Params>();

			var c = RenderWithRenderFragment(rf).Instance;
			c.NullableCC.ShouldBe("FOO");
			c.CC.ShouldBe(42);
		}

		[Fact(DisplayName = "ToComponentRenderFragment throws when multiple unnamed cascading values with same type is added")]
		public void Test052()
		{
			var sut = new ComponentParameterCollection();
			sut.Add(ComponentParameter.CreateCascadingValue(null, "FOO"));
			sut.Add(ComponentParameter.CreateCascadingValue(null, 42));
			sut.Add(ComponentParameter.CreateCascadingValue(null, "BAR"));
			sut.Add(ComponentParameter.CreateCascadingValue(null, Array.Empty<string>()));

			Should.Throw<ArgumentException>(() => sut.ToComponentRenderFragment<Params>());
		}

		[Fact(DisplayName = "ToComponentRenderFragment wraps component in named cascading values in RenderFragment")]
		public void Test053()
		{
			var sut = new ComponentParameterCollection();
			sut.Add(ComponentParameter.CreateCascadingValue(nameof(Params.NullableNamedCC), "FOO"));

			var rf = sut.ToComponentRenderFragment<Params>();

			var c = RenderWithRenderFragment(rf).Instance;
			c.NullableNamedCC.ShouldBe("FOO");
		}

		[Fact(DisplayName = "ToComponentRenderFragment wraps component in multiple named cascading values in RenderFragment")]
		public void Test054()
		{
			var sut = new ComponentParameterCollection();
			sut.Add(ComponentParameter.CreateCascadingValue(nameof(Params.NullableNamedCC), "FOO"));
			sut.Add(ComponentParameter.CreateCascadingValue(nameof(Params.NamedCC), 42));
			sut.Add(ComponentParameter.CreateCascadingValue(nameof(Params.AnotherNamedCC), 1337));

			var rf = sut.ToComponentRenderFragment<Params>();

			var c = RenderWithRenderFragment(rf).Instance;
			c.NullableNamedCC.ShouldBe("FOO");
			c.NamedCC.ShouldBe(42);
			c.AnotherNamedCC.ShouldBe(1337);
		}

		[Fact(DisplayName = "ToComponentRenderFragment throws when multiple named cascading values with same name and type is added")]
		public void Test055()
		{
			var sut = new ComponentParameterCollection();
			sut.Add(ComponentParameter.CreateCascadingValue(nameof(Params.NullableNamedCC), "FOO"));
			sut.Add(ComponentParameter.CreateCascadingValue(nameof(Params.NamedCC), 42));
			sut.Add(ComponentParameter.CreateCascadingValue(nameof(Params.AnotherNamedCC), 1337));
			sut.Add(ComponentParameter.CreateCascadingValue(nameof(Params.NamedCC), 42));

			Should.Throw<ArgumentException>(() => sut.ToComponentRenderFragment<Params>());
		}

		private class Params : ComponentBase
		{
			public const string TemplateContent = "FOO";

			[Parameter(CaptureUnmatchedValues = true)]
			public IReadOnlyDictionary<string, object>? Attributes { get; set; }
			[Parameter] public int? NullableValueTypeParam { get; set; }
			[Parameter] public int ValueTypeParam { get; set; } = -1;
			[Parameter] public object? Param { get; set; }
			[Parameter] public RenderFragment? ChildContent { get; set; }
			[Parameter] public RenderFragment? OtherFragment { get; set; }
			[Parameter] public EventCallback? NullableEC { get; set; }
			[Parameter] public EventCallback EC { get; set; } = EventCallback.Empty;
			[Parameter] public EventCallback<EventArgs>? NullableECWithArgs { get; set; }
			[Parameter] public EventCallback<EventArgs> ECWithArgs { get; set; } = EventCallback<EventArgs>.Empty;
			[Parameter] public RenderFragment<string>? Template { get; set; }
			[CascadingParameter] public string? NullableCC { get; set; }
			[CascadingParameter] public int CC { get; set; } = -1;
			[CascadingParameter(Name = nameof(NullableNamedCC))] public string? NullableNamedCC { get; set; }
			[CascadingParameter(Name = nameof(NamedCC))] public int NamedCC { get; set; } = -1;
			[CascadingParameter(Name = nameof(AnotherNamedCC))] public int AnotherNamedCC { get; set; } = -1;

			public void VerifyParamsHaveDefaultValues()
			{
				Attributes.ShouldBeNull();
				NullableValueTypeParam.ShouldBeNull();
				ValueTypeParam.ShouldBe(-1);
				Param.ShouldBeNull();
				ChildContent.ShouldBeNull();
				OtherFragment.ShouldBeNull();
				NullableEC.ShouldBeNull();
				EC.ShouldBe(EventCallback.Empty);
				NullableECWithArgs.ShouldBeNull();
				ECWithArgs.ShouldBe(EventCallback<EventArgs>.Empty);
				Template.ShouldBeNull();
				NullableCC.ShouldBeNull();
				CC.ShouldBe(-1);
				NullableNamedCC.ShouldBeNull();
				NamedCC.ShouldBe(-1);
				AnotherNamedCC.ShouldBe(-1);
			}

			protected override void BuildRenderTree(RenderTreeBuilder builder)
			{
				builder.AddContent(0, ChildContent);
				builder.AddContent(1, OtherFragment);
				if (Template != null)
					builder.AddContent(2, Template(TemplateContent));
			}
		}
	}
}
