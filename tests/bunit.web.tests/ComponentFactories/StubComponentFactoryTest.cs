using Bunit.TestDoubles;

namespace Bunit.ComponentFactories;

public class StubComponentFactoryTest : TestContext
{
	[UIFact(DisplayName = "AddStub throws if predicate is null")]
	public void Test101()
		=> Should.Throw<ArgumentNullException>(() => ComponentFactories.AddStub(componentTypePredicate: default, string.Empty));

	[UIFact(DisplayName = "AddStub<T> replaces T with Stub<T>")]
	public void Test001()
	{
		ComponentFactories.AddStub<CompA>();

		var cut = RenderComponent<Wrapper>(ps => ps.AddChildContent<CompA>());

		cut.HasComponent<Stub<CompA>>().ShouldBeTrue();
	}

	[UIFact(DisplayName = "AddStub<T> replaces U:T with Stub<U>")]
	public void Test002()
	{
		ComponentFactories.AddStub<CompA>();

		var cut = RenderComponent<Wrapper>(ps => ps.AddChildContent<CompDerivedA>());

		cut.HasComponent<Stub<CompDerivedA>>().ShouldBeTrue();
	}

	[UITheory(DisplayName = "AddStub(markup) replaces types with markup")]
	[AutoData]
	public void Test010(string randomText)
	{
		ComponentFactories.AddStub<CompA>($"<h1>{randomText}</h1>");

		var cut = RenderComponent<Wrapper>(ps => ps.AddChildContent<CompDerivedA>());

		cut.HasComponent<Stub<CompDerivedA>>().ShouldBeTrue();
		cut.MarkupMatches($"<h1>{randomText}</h1>");
	}

	[UITheory(DisplayName = "AddStub(renderFragment) replaces types with markup")]
	[AutoData]
	public void Test011(string randomText)
	{
		ComponentFactories.AddStub<CompA>(b => b.AddMarkupContent(0, $"<h1>{randomText}</h1>"));

		var cut = RenderComponent<Wrapper>(ps => ps.AddChildContent<CompDerivedA>());

		cut.HasComponent<Stub<CompDerivedA>>().ShouldBeTrue();
		cut.MarkupMatches($"<h1>{randomText}</h1>");
	}

	[UITheory(DisplayName = "AddStub<T>(Func<params, string>) replaces types with Stub that output from render fragment")]
	[AutoData]
	public void Test0042(string regularParamValue)
	{
		ComponentFactories.AddStub<AllTypesOfParams<string>>(ps => $"<div>{ps.Get(x => x.RegularParam)}</div>");

		var cut = RenderComponent<Wrapper>(parameters => parameters
			.AddChildContent<AllTypesOfParams<string>>(ps => ps.Add(p => p.RegularParam, regularParamValue)));

		cut.MarkupMatches($"<div>{regularParamValue}</div>");
	}

	[UITheory(DisplayName = "AddStub<T>(renderFragment<params>) replaces types with Stub that output from render fragment")]
	[AutoData]
	public void Test004(string regularParamValue)
	{
		ComponentFactories.AddStub<AllTypesOfParams<string>>(ps
			=> builder
			=> builder.AddMarkupContent(0, $"<div>{ps.Get(x => x.RegularParam)}</div>"));

		var cut = RenderComponent<Wrapper>(parameters => parameters
			.AddChildContent<AllTypesOfParams<string>>(ps => ps.Add(p => p.RegularParam, regularParamValue)));

		cut.MarkupMatches($"<div>{regularParamValue}</div>");
	}

	[UIFact(DisplayName = "AddStub(predicate) replaces types that matches predicate")]
	public void Test003()
	{
		ComponentFactories.AddStub(componentType => componentType == typeof(CompA));

		var cut = RenderComponent<Wrapper>(ps => ps.AddChildContent<CompA>());

		cut.HasComponent<Stub<CompA>>().ShouldBeTrue();
	}

	[UITheory(DisplayName = "AddStub(predicate, markup) replaces types that matches predicate with markup")]
	[AutoData]
	public void Test023(string randomText)
	{
		ComponentFactories.AddStub(componentType => componentType == typeof(CompA), $"<h1>{randomText}</h1>");

		var cut = RenderComponent<Wrapper>(ps => ps.AddChildContent<CompA>());

		cut.HasComponent<Stub<CompA>>().ShouldBeTrue();
		cut.MarkupMatches($"<h1>{randomText}</h1>");
	}

	[UITheory(DisplayName = "AddStub(predicate, markup) replaces types that matches predicate with markup")]
	[AutoData]
	public void Test024(string randomText)
	{
		ComponentFactories.AddStub(componentType => componentType == typeof(CompA), b => b.AddMarkupContent(1, $"<h1>{randomText}</h1>"));

		var cut = RenderComponent<Wrapper>(ps => ps.AddChildContent<CompA>());

		cut.HasComponent<Stub<CompA>>().ShouldBeTrue();
		cut.MarkupMatches($"<h1>{randomText}</h1>");
	}

	private class CompA : ComponentBase { }
	private sealed class CompDerivedA : CompA { }
}
