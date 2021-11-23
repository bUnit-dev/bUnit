#if NET5_0_OR_GREATER
using System;
using AutoFixture.Xunit2;
using Bunit.TestAssets.SampleComponents;
using Bunit.TestDoubles;
using Microsoft.AspNetCore.Components;
using Shouldly;
using Xunit;

namespace Bunit.ComponentFactories;

public class StubComponentFactoryTest : TestContext
{
	[Fact(DisplayName = "AddStub throws if factories is null")]
	public void Test100()
		=> Should.Throw<ArgumentNullException>(() => ComponentFactories.AddStub(componentTypePredicate: default, _ => ""));

	[Fact(DisplayName = "AddStub throws if predicate is null")]
	public void Test101()
		=> Should.Throw<ArgumentNullException>(() => ComponentFactories.AddStub(componentTypePredicate: default, _ => ""));

	[Fact(DisplayName = "AddStub<T> replaces T with Stub<T>")]
	public void Test001()
	{
		ComponentFactories.AddStub<CompA>();

		var cut = RenderComponent<Wrapper>(ps => ps.AddChildContent<CompA>());

		cut.HasComponent<Stub<CompA>>().ShouldBeTrue();
	}

	[Fact(DisplayName = "AddStub<T> replaces U:T with Stub<U>")]
	public void Test002()
	{
		ComponentFactories.AddStub<CompA>();

		var cut = RenderComponent<Wrapper>(ps => ps.AddChildContent<CompDerivedA>());

		cut.HasComponent<Stub<CompDerivedA>>().ShouldBeTrue();
	}

	[Fact(DisplayName = "AddStub(predicate) replaces types that matches predicate")]
	public void Test003()
	{
		ComponentFactories.AddStub(componentType => componentType == typeof(CompA));

		var cut = RenderComponent<Wrapper>(ps => ps.AddChildContent<CompA>());

		cut.HasComponent<Stub<CompA>>().ShouldBeTrue();
	}

	[Theory(DisplayName = "AddStub<T>(renderFragment<params>) replaces types with Stub that output from render fragment")]
	[AutoData]
	public void Test004(string regularParamValue)
	{
		ComponentFactories.AddStub<AllTypesOfParams<string>>(ps
			=> builder
			=> builder.AddMarkupContent(0, $"<div>{ps["RegularParam"]}</div>"));

		var cut = RenderComponent<Wrapper>(parameters => parameters
			.AddChildContent<AllTypesOfParams<string>>(ps => ps.Add(p => p.RegularParam, regularParamValue)));

		cut.MarkupMatches($"<div>{regularParamValue}</div>");
	}

	[Theory(DisplayName = "AddStub<T>(Func<params>) replaces types with Stub that output from render fragment")]
	[AutoData]
	public void Test005(string regularParamValue)
	{
		ComponentFactories.AddStub<AllTypesOfParams<string>>(ps => $"<div>{ps["RegularParam"]}</div>");

		var cut = RenderComponent<Wrapper>(parameters => parameters
			.AddChildContent<AllTypesOfParams<string>>(ps => ps.Add(p => p.RegularParam, regularParamValue)));

		cut.MarkupMatches($"<div>{regularParamValue}</div>");
	}

	private class CompA : ComponentBase { }
	private class CompDerivedA : CompA { }
}

#endif
