#if NET5_0_OR_GREATER
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using Bunit.TestAssets.SampleComponents;
using Microsoft.AspNetCore.Components;
using Shouldly;
using Xunit;

namespace Bunit.TestDoubles.Components
{
    public class ComponentDoubleBaseTest : TestContext
    {
		private class ComponentDouble<TComponent> : ComponentDoubleBase<TComponent>
			where TComponent : IComponent
		{ }

		[Fact(DisplayName = "GetParameter(paramterSelector) throws if selector is null")]
		public void Test010()
		{
			var cut = RenderComponent<ComponentDouble<AllTypesOfParams<string>>>();

			Should.Throw<ArgumentNullException>(() => cut.Instance.GetParameter<string>(null));
		}

		[Fact(DisplayName = "GetParameter(parameterSelector) throws if method is selected")]
		public void Test011()
		{
			var cut = RenderComponent<ComponentDouble<AllTypesOfParams<string>>>();

			Should.Throw<ArgumentException>(() => cut.Instance.GetParameter(x => x.DummyMethod()));
		}

		[Fact(DisplayName = "GetParameter(parameterSelector) throws if non-parameter property is selected")]
		public void Test012()
		{
			var cut = RenderComponent<ComponentDouble<AllTypesOfParams<string>>>();

			Should.Throw<ArgumentException>(() => cut.Instance.GetParameter(x => x.JSRuntime));
		}

		[Fact(DisplayName = "GetParameter(parameterSelector) throws if selected parameter was not passed to component")]
		public void Test013()
		{
			var cut = RenderComponent<ComponentDouble<AllTypesOfParams<string>>>();

			Should.Throw<ParameterNotFoundException>(() => cut.Instance.GetParameter(x => x.RegularParam));
		}

		[Fact(DisplayName = "GetParameter(parameterSelector) throws if selected parameter is not the expected type")]
		public void Test014()
		{
			var cut = RenderComponent<ComponentDouble<AllTypesOfParams<string>>>(
				(nameof(AllTypesOfParams<string>.ChildContent), "not a render fragment"));

			Should.Throw<InvalidCastException>(() => cut.Instance.GetParameter(x => x.ChildContent));
		}

		[Theory(DisplayName = "GetParameter(paramterSelector) returns value of selected parameter")]
		[AutoData]
		public void Test020(string regularParam)
		{
			var cut = RenderComponent<ComponentDouble<AllTypesOfParams<string>>>(
				(nameof(AllTypesOfParams<string>.RegularParam), regularParam));

			cut.Instance.GetParameter(x => x.RegularParam).ShouldBe(regularParam);
		}

		[Fact(DisplayName = "GetParameter(paramterSelector) returns null when null was passed to selected parameter")]
		public void Test021()
		{
			var cut = RenderComponent<ComponentDouble<AllTypesOfParams<string>>>(
				(nameof(AllTypesOfParams<string>.UnnamedCascadingValue), null));

			cut.Instance.GetParameter(x => x.UnnamedCascadingValue).ShouldBeNull();
		}
	}
}
#endif
