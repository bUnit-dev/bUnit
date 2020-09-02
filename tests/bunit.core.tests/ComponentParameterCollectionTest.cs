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
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit;

namespace Bunit
{
	public class ComponentParameterCollectionTest
	{
		private const string RegularParamName = nameof(AllTypesOfParams<string>.RegularParam);

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

			var c = RenderWithRenderFragment(rf);
			c.AllParametersShouldHaveDefaultValues();
		}

		//[Fact(DisplayName = "ToComponentRenderFragment creates RenderFragment with regular parameter passed")]
		//public void Test010()
		//{
		//	var sut = new ComponentParameterCollection();
		//	var p = ComponentParameter.CreateParameter(RegularParamName, "FOO");
		//	sut.Add(p);

		//	var rf = sut.ToComponentRenderFragment<AllTypesOfParams<string>>();

		//	var c = RenderWithRenderFragment(rf);
		//	c.RegularParam.ShouldBe("FOO");
		//}

		private Params RenderWithRenderFragment(RenderFragment renderFragment)
		{
			using var ctx = new TestContext();
			ctx.Services.AddMockJSRuntime();
			var res = (IRenderedFragment)ctx.Renderer.RenderFragment(renderFragment);
			return res.FindComponent<Params>().Instance;
		}

		private class Params : ComponentBase
		{
			[Parameter] public int? NullableValueTypeParam { get; set; }
			[Parameter] public int ValueTypeParam { get; set; } = -1;
			[Parameter] public object Param { get; set; }

			public void AllParametersShouldHaveDefaultValues()
			{
				NullableValueTypeParam.ShouldBeNull();
				ValueTypeParam.ShouldBe(-1);
				Param.ShouldBeNull();
			}

		}
	}
}
