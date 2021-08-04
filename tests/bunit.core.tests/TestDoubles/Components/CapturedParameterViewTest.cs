#if NET5_0_OR_GREATER
using System;
using System.Collections.Generic;
using AutoFixture.Xunit2;
using Bunit.TestAssets.SampleComponents;
using Microsoft.AspNetCore.Components;
using Shouldly;
using Xunit;

namespace Bunit.TestDoubles.Components
{
	public class CapturedParameterViewTest
	{
		[Fact(DisplayName = "Get(paramterSelector) throws if selector is null")]
		public void Test010()
		{
			var sut = CapturedParameterView<AllTypesOfParams<string>>.Empty;
			Should.Throw<ArgumentNullException>(() => sut.Get<string>(null));
		}

		[Fact(DisplayName = "Get(parameterSelector) throws if method is selected")]
		public void Test011()
		{
			var sut = CapturedParameterView<AllTypesOfParams<string>>.Empty;
			Should.Throw<ArgumentException>(() => sut.Get(x => x.DummyMethod()));
		}

		[Fact(DisplayName = "Get(parameterSelector) throws if non-parameter property is selected")]
		public void Test012()
		{
			var sut = CapturedParameterView<AllTypesOfParams<string>>.Empty;

			Should.Throw<ArgumentException>(() => sut.Get(x => x.JSRuntime));
		}

		[Fact(DisplayName = "Get(parameterSelector) throws if selected parameter was not added to parameter view")]
		public void Test013()
		{
			var sut = CapturedParameterView<AllTypesOfParams<string>>.Empty;
			Should.Throw<ParameterNotFoundException>(() => sut.Get(x => x.RegularParam));
		}

		[Fact(DisplayName = "Get(parameterSelector) throws if selected parameter is not the expected type")]
		public void Test014()
		{
			var sut = CapturedParameterView<AllTypesOfParams<string>>.From(
				ParameterView.FromDictionary(
					  new Dictionary<string, object> {
						{ nameof(AllTypesOfParams<string>.ChildContent), "not a render fragment" }}));

			Should.Throw<InvalidCastException>(() => sut.Get(x => x.ChildContent));
		}

		[Theory(DisplayName = "Get(paramterSelector) returns value of selected parameter")]
		[AutoData]
		public void Test020(string regularParam)
		{
			var sut = CapturedParameterView<AllTypesOfParams<string>>.From(
				ParameterView.FromDictionary(
					  new Dictionary<string, object> {
					{ nameof(AllTypesOfParams<string>.RegularParam), regularParam }}));

			sut.Get(x => x.RegularParam).ShouldBe(regularParam);
		}

		[Fact(DisplayName = "GetParameter(paramterSelector) returns null when null was passed to selected parameter")]
		public void Test021()
		{
			var sut = CapturedParameterView<AllTypesOfParams<string>>.From(
				ParameterView.FromDictionary(
					  new Dictionary<string, object> {
					{ nameof(AllTypesOfParams<string>.UnnamedCascadingValue), null }}));

			sut.Get(x => x.UnnamedCascadingValue).ShouldBeNull();
		}
	}
}
#endif
