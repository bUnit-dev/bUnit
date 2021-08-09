#if NET5_0_OR_GREATER
using System;
using Bunit.Extensions;
using Bunit.TestAssets.SampleComponents;
using Microsoft.AspNetCore.Components;
using Moq;
using Xunit;

namespace Bunit
{
	public class TestContextBaseTest : TestContext
	{
		[Fact(DisplayName = "ComponentFactories CanCreate() method are checked during component instantiation")]
		public void Test0001()
		{
			var mock = CreateMockComponentFactory(canCreate: _ => false, create: _ => null);
			ComponentFactories.Add(mock.Object);

			RenderComponent<Simple1>();

			mock.Verify(x => x.CanCreate(typeof(Simple1)), Times.Once);
			mock.Verify(x => x.Create(It.IsAny<Type>()), Times.Never);
		}

		[Fact(DisplayName = "ComponentFactories Create() method is called when their CanCreate() method returns true")]
		public void Test0002()
		{
			var mock = CreateMockComponentFactory(canCreate: _ => true, create: _ => new Simple1());
			ComponentFactories.Add(mock.Object);

			RenderComponent<Simple1>();

			mock.Verify(x => x.CanCreate(typeof(Simple1)), Times.Once);
			mock.Verify(x => x.Create(typeof(Simple1)), Times.Once);
		}

		[Fact(DisplayName = "ComponentFactories is used in last added order")]
		public void Test0003()
		{
			var firstMock = CreateMockComponentFactory(canCreate: _ => true, create: _ => new Simple1());
			var secondMock = CreateMockComponentFactory(canCreate: _ => true, create: _ => new Simple1());
			ComponentFactories.Add(firstMock.Object);
			ComponentFactories.Add(secondMock.Object);

			RenderComponent<Simple1>();

			firstMock.Verify(x => x.CanCreate(It.IsAny<Type>()), Times.Never);
			firstMock.Verify(x => x.Create(It.IsAny<Type>()), Times.Never);
			secondMock.Verify(x => x.CanCreate(typeof(Simple1)), Times.Once);
			secondMock.Verify(x => x.Create(typeof(Simple1)), Times.Once);
		}

		private static Mock<IComponentFactory> CreateMockComponentFactory(Func<Type, bool> canCreate, Func<Type, IComponent> create)
		{
			var result = new Mock<IComponentFactory>(MockBehavior.Strict);
			result.Setup(x => x.CanCreate(It.IsAny<Type>())).Returns(canCreate);
			result.Setup(x => x.Create(It.IsAny<Type>())).Returns(create);
			return result;
		}
	}
}
#endif

