using System.Collections;

namespace Bunit;

public partial class TestServiceProviderTest
{
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

	[Fact(DisplayName = "No registered service returns null")]
	public void Test021()
	{
		using var sut = new TestServiceProvider();

		var result = sut.GetService(typeof(DummyService));

		Assert.Null(result);
	}

	[Fact(DisplayName = "Registered fallback service provider returns value")]
	public void Test022()
	{
		using var sut = new TestServiceProvider();
		sut.AddFallbackServiceProvider(new FallbackServiceProvider());

		var result = sut.GetService(typeof(object));

		Assert.NotNull(result);
		Assert.IsType<DummyService>(result);
	}

	[Fact(DisplayName = "Register fallback service with null value")]
	public void Test023()
	{
		using var sut = new TestServiceProvider();
		Assert.Throws<ArgumentNullException>(() => sut.AddFallbackServiceProvider(null!));
	}

	[Fact(DisplayName = "Service provider returns value before fallback service provider")]
	public void Test024()
	{
		const string exceptionStringResult = "exceptionStringResult";

		using var sut = new TestServiceProvider();
		sut.AddSingleton<string>(exceptionStringResult);
		sut.AddFallbackServiceProvider(new FallbackServiceProvider());

		var stringResult = sut.GetService(typeof(string));
		Assert.Equal(exceptionStringResult, stringResult);

		var fallbackResult = sut.GetService(typeof(DummyService));
		Assert.IsType<DummyService>(fallbackResult);
	}

	[Fact(DisplayName = "Latest fallback provider is used")]
	public void Test025()
	{
		using var sut = new TestServiceProvider();
		sut.AddFallbackServiceProvider(new FallbackServiceProvider());
		sut.AddFallbackServiceProvider(new AnotherFallbackServiceProvider());

		var result = sut.GetService(typeof(object));

		Assert.IsType<AnotherDummyService>(result);
	}

	[Fact(DisplayName = "Fallback service provider can be used to resolve services required by components")]
	public void Test030()
	{
		// Arrange
		using var ctx = new TestContext();
		var fallbackServiceProvider = new ServiceCollection()
			.AddSingleton(new DummyService())
			.BuildServiceProvider();
		ctx.Services.AddFallbackServiceProvider(fallbackServiceProvider);

		// Act and assert
		Should.NotThrow(() => ctx.RenderComponent<DummyComponentWhichRequiresDummyService>());
	}

	[Fact(DisplayName = "Can correctly resolve and dispose of scoped disposable service")]
	public void Test031()
	{
		var sut = new TestServiceProvider();
		sut.AddScoped<DisposableService>();
		var disposable = sut.GetService<DisposableService>();

		sut.Dispose();

		disposable.IsDisposed.ShouldBeTrue();
	}

	[Fact(DisplayName = "Can correctly resolve and dispose of transient disposable service")]
	public void Test032()
	{
		var sut = new TestServiceProvider();
		sut.AddTransient<DisposableService>();
		var disposable = sut.GetService<DisposableService>();

		sut.Dispose();

		disposable.IsDisposed.ShouldBeTrue();
	}

	[Fact(DisplayName = "Can correctly resolve and dispose of singleton disposable service")]
	public void Test033()
	{
		var sut = new TestServiceProvider();
		sut.AddSingleton<DisposableService>();
		var disposable = sut.GetService<DisposableService>();

		sut.Dispose();

		disposable.IsDisposed.ShouldBeTrue();
	}

	[Fact(DisplayName = "Validates that all dependencies can be created when the first service is requested, if ServiceProviderOptions.ValidateOnBuild is true")]
	public void Test035()
	{
		using var sut = new TestServiceProvider();
		sut.Options = new ServiceProviderOptions
		{
			ValidateOnBuild = true,
			ValidateScopes = true
		};
		sut.AddSingleton<DummyService>();
		sut.AddSingleton<DummyServiceWithDependencyOnAnotherDummyService>();
		var action = () => sut.GetRequiredService<DummyService>();

		action.ShouldThrow<AggregateException>("Some services are not able to be constructed (Error while validating the service descriptor");
	}

	[Fact(DisplayName = "Does not validate all dependencies can be created when the first service is requested, if ServiceProviderOptions.ValidateOnBuild is false")]
	public void Test036()
	{
		using var sut = new TestServiceProvider();
		sut.Options = new ServiceProviderOptions
		{
			ValidateOnBuild = false,
			ValidateScopes = true
		};
		sut.AddSingleton<DummyService>();
		sut.AddSingleton<DummyServiceWithDependencyOnAnotherDummyService>();

		var result = sut.GetRequiredService<DummyService>();

		result.ShouldNotBeNull();
	}

	[Fact(DisplayName = "Does not validate all dependencies can be created when the first service is requested, if no ServiceProviderOptions is provided (backwards compatibility)")]
	public void Test037()
	{
		using var sut = new TestServiceProvider();
		sut.AddSingleton<DummyService>();
		sut.AddSingleton<DummyServiceWithDependencyOnAnotherDummyService>();

		var result = sut.GetRequiredService<DummyService>();

		result.ShouldNotBeNull();
	}

	[Fact(DisplayName = "Test custom service provider factory")]
	public void Test038()
	{
		using var sut = new TestServiceProvider();
		sut.AddSingleton<DummyService>();
		var dummyServiceProviderFactory = new DummyServiceProviderFactory();
		sut.UseServiceProviderFactory(dummyServiceProviderFactory);

		var result = sut.GetRequiredService<DummyService>();

		result.ShouldNotBeNull();
		dummyServiceProviderFactory.TestContainerBuilder.ShouldNotBeNull();
		dummyServiceProviderFactory.TestContainerBuilder.TestServiceProvider.ShouldNotBeNull();
		dummyServiceProviderFactory.TestContainerBuilder.TestServiceProvider.ResolvedTestServices.ShouldContain(result);
		dummyServiceProviderFactory.TestContainerBuilder.TestServiceProvider.ResolvedTestServices.Count.ShouldBe(1);
	}

	[Fact(DisplayName = "Test custom service provider factory as delegate")]
	public void Test039()
	{
		using var sut = new TestServiceProvider();
		sut.AddSingleton<DummyService>();
		DummyServiceProvider dummyServiceProvider = null;
		sut.UseServiceProviderFactory(x => dummyServiceProvider = new DummyServiceProvider(x));

		var result = sut.GetRequiredService<DummyService>();

		result.ShouldNotBeNull();
		dummyServiceProvider.ShouldNotBeNull();
		dummyServiceProvider.ResolvedTestServices.ShouldContain(result);
		dummyServiceProvider.ResolvedTestServices.Count.ShouldBe(1);
	}
	
	[Fact(DisplayName = "Should resolve keyed service from container")]
	public void Test040()
	{
		using var ctx = new TestContext();
		ctx.Services.AddKeyedScoped<DummyService>("Key");

		var cut = ctx.RenderComponent<ComponentWithKeyedService>();

		cut.Instance.Service.ShouldNotBeNull();
	}

	[Fact(DisplayName = "Should resolved keyed service from fallback service provider")]
	public void Test041()
	{
		using var ctx = new TestContext();
		var fallbackCollection = new ServiceCollection();
		fallbackCollection.AddKeyedScoped<DummyService>("Key");
		ctx.Services.AddFallbackServiceProvider(fallbackCollection.BuildServiceProvider());
		
		var cut = ctx.RenderComponent<ComponentWithKeyedService>();

		cut.Instance.Service.ShouldNotBeNull();
	}
	
	[Fact(DisplayName = "Throw an exception if required keyed service is not found")]
	public void Test042()
	{
		using var ctx = new TestContext();

		Should.Throw<InvalidOperationException>(() => ctx.RenderComponent<ComponentWithKeyedService>());
	}

	private sealed class DummyService { }

	private sealed class AnotherDummyService { }

	private sealed class OneMoreDummyService { }

	private sealed class DummyServiceWithDependencyOnAnotherDummyService
	{
		public DummyServiceWithDependencyOnAnotherDummyService(AnotherDummyService anotherDummyService)
		{
		}
	}

	private sealed class FallbackServiceProvider : IServiceProvider
	{
		public object GetService(Type serviceType) => new DummyService();
	}

	private sealed class AnotherFallbackServiceProvider : IServiceProvider
	{
		public object GetService(Type serviceType) => new AnotherDummyService();
	}

	private sealed class DummyComponentWhichRequiresDummyService : ComponentBase
	{
		[Inject] public DummyService Service { get; set; }
	}

	private sealed class DisposableService : IDisposable
	{
		public bool IsDisposed { get; private set; }

		public void Dispose()
		{
			IsDisposed = true;
		}
	}

	private sealed class DummyServiceProvider : IServiceProvider, IServiceScopeFactory, IServiceScope
	{
		private readonly IServiceCollection serviceDescriptors;

		public readonly List<object?> ResolvedTestServices = new();

		public DummyServiceProvider(IServiceCollection serviceDescriptors)
			=> this.serviceDescriptors = serviceDescriptors;

		public object? GetService(Type serviceType)
		{
			if (serviceType == typeof(IServiceScope) || serviceType == typeof(IServiceScopeFactory))
				return this;

			var result = Activator.CreateInstance(serviceDescriptors.Single(x => x.ServiceType == serviceType).ImplementationType);
			ResolvedTestServices.Add(result);
			return result;
		}

		void IDisposable.Dispose() { }
		public IServiceScope CreateScope() => this;
		IServiceProvider IServiceScope.ServiceProvider => this;

	}

	private sealed class DummyServiceProviderFactoryContainerBuilder
	{
		private readonly IServiceCollection serviceDescriptors;

		public DummyServiceProvider? TestServiceProvider { get; private set; }

		public DummyServiceProviderFactoryContainerBuilder(IServiceCollection serviceDescriptors) => this.serviceDescriptors = serviceDescriptors;

		public IServiceProvider Build() => TestServiceProvider = new DummyServiceProvider(serviceDescriptors);
	}

	private sealed class DummyServiceProviderFactory : IServiceProviderFactory<DummyServiceProviderFactoryContainerBuilder>
	{
		public DummyServiceProviderFactoryContainerBuilder TestContainerBuilder { get; private set; }

		public DummyServiceProviderFactoryContainerBuilder CreateBuilder(IServiceCollection services)
			=> TestContainerBuilder = new DummyServiceProviderFactoryContainerBuilder(services);

		public IServiceProvider CreateServiceProvider(DummyServiceProviderFactoryContainerBuilder containerBuilder)
			=> containerBuilder.Build();
	}
	
	private sealed class ComponentWithKeyedService : ComponentBase
	{
		[Inject(Key = "Key")]
		public DummyService Service { get; set; }
	}
}
