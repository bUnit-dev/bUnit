using System;
using System.Collections;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit;

namespace Bunit
{
	public partial class TestServiceProviderTest
	{
		private class DummyService { }

		private class AnotherDummyService { }

		private class OneMoreDummyService { }

		[Fact(DisplayName = "Provider initialized without a service collection has zero services by default")]
		public void Test001()
		{
			using var sut = new TestServiceProvider();

			sut.Count.ShouldBe(0);
		}

		[Fact(DisplayName = "Provider initialized with a service collection has the services form the provided collection")]
		public void Test002()
		{
			var services = new ServiceCollection();
			services.AddSingleton(new DummyService());
			using var sut = new TestServiceProvider(services);

			sut.Count.ShouldBe(1);
			sut[0].ServiceType.ShouldBe(typeof(DummyService));
		}

		[Fact(DisplayName = "Services can be registered in the provider like a normal service collection")]
		public void Test010()
		{
			using var sut = new TestServiceProvider();

			sut.Add(new ServiceDescriptor(typeof(DummyService), new DummyService()));
			sut.Insert(0, new ServiceDescriptor(typeof(AnotherDummyService), new AnotherDummyService()));
			sut[1] = new ServiceDescriptor(typeof(DummyService), new DummyService());

			sut.Count.ShouldBe(2);
			sut[0].ServiceType.ShouldBe(typeof(AnotherDummyService));
			sut[1].ServiceType.ShouldBe(typeof(DummyService));
		}

		[Fact(DisplayName = "Services can be removed in the provider like a normal service collection")]
		public void Test011()
		{
			using var sut = new TestServiceProvider();
			var descriptor = new ServiceDescriptor(typeof(DummyService), new DummyService());
			var anotherDescriptor = new ServiceDescriptor(typeof(AnotherDummyService), new AnotherDummyService());
			var oneMoreDescriptor = new ServiceDescriptor(typeof(OneMoreDummyService), new OneMoreDummyService());

			sut.Add(descriptor);
			sut.Add(anotherDescriptor);
			sut.Add(oneMoreDescriptor);

			sut.Remove(descriptor);
			sut.Count.ShouldBe(2);

			sut.RemoveAt(1);
			sut.Count.ShouldBe(1);

			sut.Clear();
			sut.ShouldBeEmpty();
		}

		[Fact(DisplayName = "Misc collection methods works as expected")]
		public void Test012()
		{
			using var sut = new TestServiceProvider();
			var descriptor = new ServiceDescriptor(typeof(DummyService), new DummyService());
			var copyToTarget = new ServiceDescriptor[1];
			sut.Add(descriptor);

			sut.IndexOf(descriptor).ShouldBe(0);
			sut.Contains(descriptor).ShouldBeTrue();
			sut.CopyTo(copyToTarget, 0);
			copyToTarget[0].ShouldBe(descriptor);
			sut.IsReadOnly.ShouldBeFalse();
			((IEnumerable)sut).OfType<ServiceDescriptor>().Count().ShouldBe(1);
		}

		[Fact(DisplayName = "After the first service is requested, " +
							"the provider does not allow changes to service collection")]
		public void Test013()
		{
			var descriptor = new ServiceDescriptor(typeof(AnotherDummyService), new AnotherDummyService());

			using var sut = new TestServiceProvider();
			sut.AddSingleton(new DummyService());
			sut.GetService<DummyService>();

			// Try adding
			Should.Throw<InvalidOperationException>(() => sut.Add(descriptor));
			Should.Throw<InvalidOperationException>(() => sut.Insert(0, descriptor));
			Should.Throw<InvalidOperationException>(() => sut[0] = descriptor);

			// Try removing
			Should.Throw<InvalidOperationException>(() => sut.Remove(descriptor));
			Should.Throw<InvalidOperationException>(() => sut.RemoveAt(0));
			Should.Throw<InvalidOperationException>(() => sut.Clear());

			// Verify state
			sut.IsProviderInitialized.ShouldBeTrue();
			sut.IsReadOnly.ShouldBeTrue();
		}

		[Fact(DisplayName = "Registered services can be retrieved from the provider")]
		public void Test020()
		{
			using var sut = new TestServiceProvider();
			var expected = new DummyService();
			sut.AddSingleton(expected);

			var actual = sut.GetService<DummyService>();

			actual.ShouldBe(expected);
		}
	}
}
