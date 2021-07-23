#if NET5_0_OR_GREATER
using System;
using AutoFixture.Xunit2;
using Bunit.TestAssets.SampleComponents;
using Microsoft.AspNetCore.Components;
using Shouldly;
using Xunit;

namespace Bunit.TestDoubles.Components
{
	public class StubTest : TestContext
	{
		[Fact(DisplayName = "Stub<TComponent> renders nothing with default options")]
		public void Test001()
		{
			var cut = RenderComponent<Stub<Simple1>>();

			cut.Nodes.Length.ShouldBe(0);
		}

		[Theory(DisplayName = "Stub<TComponent> captures parameters passed to TComponent")]
		[AutoData]
		public void Test002(string header, string attrValue)
		{
			var cut = RenderComponent<Stub<Simple1>>(
				(nameof(Simple1.Header), header),
				(nameof(Simple1.AttrValue), attrValue));

			cut.Instance
				.Parameters
				.ShouldSatisfyAllConditions(
					ps => ps.ShouldContain(x => x.Key == nameof(Simple1.Header) && header.Equals(x.Value)),
					ps => ps.ShouldContain(x => x.Key == nameof(Simple1.AttrValue) && attrValue.Equals(x.Value)),
					ps => ps.Count.ShouldBe(2));
		}

		[Theory(DisplayName = "Stub<TComponent> add parameters as attribute when StubOptions.RenderPlaceholder = true and StubOptions.RenderParameters = true")]
		[AutoData]
		public void Test003(string header, string attrValue)
		{
			ComponentFactories.Add(new StubComponentFactory<Simple1>(new Stub<Simple1>(new() { RenderPlaceholder = true, RenderParameters = true })));
			var cut = RenderComponent<Stub<Simple1>>(
				(nameof(Simple1.Header), header),
				(nameof(Simple1.AttrValue), attrValue));

			var simple1 = cut.Find("Simple1");

			simple1.Attributes["header"].Value.ShouldBe(header);
			simple1.Attributes["attrvalue"].Value.ShouldBe(attrValue);
			cut.Instance.Options.RenderParameters.ShouldBeTrue();
		}

		[Theory(DisplayName = "Stub<TComponent> does not add parameters as attribute when StubOptions.RenderParameters = false")]
		[AutoData]
		public void Test004(string header)
		{
			ComponentFactories.Add(new StubComponentFactory<Simple1>(new Stub<Simple1>(new() { RenderPlaceholder = true, RenderParameters = false })));
			var cut = RenderComponent<Stub<Simple1>>((nameof(Simple1.Header), header));

			var simple1 = cut.Find("Simple1");

			simple1.HasAttribute("header").ShouldBeFalse();
			cut.Instance.Options.RenderParameters.ShouldBeFalse();
		}

		[Fact(DisplayName = "Stub<TComponent<T>> renders element with name of TComponent and T set to name of type when StubOptions.RenderPlaceholder = true ")]
		public void Test005()
		{
			ComponentFactories.Add(new StubComponentFactory<CascadingValue<string>>(new Stub<CascadingValue<string>>(new() { RenderPlaceholder = true })));
			var cut = RenderComponent<Stub<CascadingValue<string>>>();

			cut.Find("CascadingValue").Attributes["tvalue"].Value.ShouldBe(typeof(string).Name);
		}

		private class StubComponentFactory<TComponent> : IComponentFactory where TComponent : IComponent
		{
			private readonly Stub<TComponent> stub;
			public StubComponentFactory(Stub<TComponent> stub) => this.stub = stub;
			public bool CanCreate(Type componentType) => componentType == stub.GetType();
			public IComponent Create(Type componentType) => stub;
		}
	}
}
#endif
