#if NET5_0_OR_GREATER
using System;
using Bunit.TestAssets.SampleComponents;
using Bunit.TestDoubles;
using Microsoft.AspNetCore.Components;
using Shouldly;
using Xunit;

namespace Bunit.ComponentFactories
{
	public class StubComponentFactoryTest : TestContext
	{
		[Fact(DisplayName = "AddStub throws if factories is null")]
		public void Test100()
			=> Should.Throw<ArgumentNullException>(() => ComponentFactoryCollectionWebExtensions.AddStub(null, null));

		[Fact(DisplayName = "AddStub throws if predicate is null")]
		public void Test101()
			=> Should.Throw<ArgumentNullException>(() => ComponentFactories.AddStub(null));

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

		private class CompA : ComponentBase { }
		private class CompDerivedA : CompA { }
	}
}

#endif
