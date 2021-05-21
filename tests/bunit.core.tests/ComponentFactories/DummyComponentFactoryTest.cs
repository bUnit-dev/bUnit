#if NET5_0_OR_GREATER
using System;
using Bunit.TestAssets.SampleComponents;
using Bunit.TestDoubles;
using Microsoft.AspNetCore.Components;
using Shouldly;
using Xunit;

namespace Bunit.ComponentFactories
{
	public class DummyComponentFactoryTest : TestContext
	{
		[Fact(DisplayName = "UseDummyFor throws if factories is null")]
		public void Test100()
			=> Should.Throw<ArgumentNullException>(() => ComponentFactoryCollectionExtensions.UseDummyFor(null, null));

		[Fact(DisplayName = "UseDummyFor throws if predicate is null")]
		public void Test101()
			=> Should.Throw<ArgumentNullException>(() => ComponentFactories.UseDummyFor(null));

		[Fact(DisplayName = "UseDummyFor<T> replaces T with Dummy<T>")]
		public void Test001()
		{
			ComponentFactories.UseDummyFor<CompA>();

			var cut = RenderComponent<Wrapper>(ps => ps.AddChildContent<CompA>());

			cut.HasComponent<Dummy<CompA>>().ShouldBeTrue();
		}

		[Fact(DisplayName = "UseDummyFor<T> replaces U:T with Stub<U>")]
		public void Test002()
		{
			ComponentFactories.UseDummyFor<CompA>();

			var cut = RenderComponent<Wrapper>(ps => ps.AddChildContent<CompDerivedA>());

			cut.HasComponent<Dummy<CompDerivedA>>().ShouldBeTrue();
		}

		[Fact(DisplayName = "UseDummyFor(predicate) replaces types that matches predicate")]
		public void Test003()
		{
			ComponentFactories.UseDummyFor(componentType => componentType == typeof(CompA));

			var cut = RenderComponent<Wrapper>(ps => ps.AddChildContent<CompA>());

			cut.HasComponent<Dummy<CompA>>().ShouldBeTrue();
		}

		private class CompA : ComponentBase { }
		private class CompDerivedA : CompA { }
	}
}

#endif
