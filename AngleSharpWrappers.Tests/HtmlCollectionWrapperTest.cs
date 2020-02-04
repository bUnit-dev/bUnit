using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.Media.Dom;
using Moq;
using Shouldly;
using Xunit;

namespace AngleSharpWrappers
{
    public class HtmlCollectionWrapperTest
    {
        public static IEnumerable<object[]> GetInterfaceMethods(Type type) =>
            type.GetInterfaceMethods().Where(x => x.Name != "GetEnumerator").Select(x => new[] { x });

        [Theory(DisplayName = "Forwards all method and property calls to wrapped collection")]
        [MemberData(nameof(GetInterfaceMethods), typeof(IHtmlCollection<IElement>))]
        public void Test001(MethodInfo method)
        {
            var elmMock = new Mock<IHtmlCollection<IElement>>();
            var sut = new HtmlCollectionWrapper<IElement>(() => elmMock.Object);
            var args = method.CreateMethodArguments();

            method.Invoke(sut, args);

            var inv = elmMock.Invocations[0];
            inv.Arguments.ShouldBe(args);
            inv.Method.ShouldBe(method);
        }

        [Fact(DisplayName = "Wrapped node is internally available")]
        public void Test003()
        {
            var elmMock = new Mock<IHtmlCollection<IElement>>();

            var sut = new HtmlCollectionWrapper<IElement>(() => elmMock.Object);

            sut.WrappedObject.ShouldBe(elmMock.Object);
        }

        [Fact(DisplayName = "Wrapper refreshes wrapped node after MarkAsStale is called")]
        public void Test004()
        {
            var callCount = 0;
            var sut = new HtmlCollectionWrapper<IElement>(() => { callCount++; return new Mock<IHtmlCollection<IElement>>().Object; });
            var firstWrapped = sut.WrappedObject;

            sut.MarkAsStale();

            var secondWrapped = sut.WrappedObject;
            Assert.NotSame(secondWrapped, firstWrapped);
            callCount.ShouldBe(2);
        }

        [Fact(DisplayName = "When a wrapped node is no longer available, accessing methods or properties throws ElementNoLongerAvailableException")]
        public void Test005()
        {
            var sut = new HtmlCollectionWrapper<IElement>(() => null);

            Should.Throw<NodeNoLongerAvailableException>(() => sut.WrappedObject);
        }

        [Fact(DisplayName = "When indexers are used to access an element, it is wrapped")]
        public void Test006()
        {
            var colMock = new Mock<IHtmlCollection<IElement>>();
            var elm1 = new Mock<IElement>().Object;
            var elm2 = new Mock<IElement>().Object;
            colMock.SetupGet(x => x[0]).Returns(elm1);
            colMock.SetupGet(x => x["id"]).Returns(elm2);

            var sut = new HtmlCollectionWrapper<IElement>(() => colMock.Object);

            sut[0].ShouldBeOfType<ElementWrapper>().WrappedObject.ShouldBe(elm1);
            sut["id"].ShouldBeOfType<ElementWrapper>().WrappedObject.ShouldBe(elm2);
        }

        [Fact(DisplayName = "Elements returned by Enumerators are wrapped")]
        public void Test007()
        {
            var colMock = new Mock<IHtmlCollection<IElement>>();
            var elm1 = new Mock<IElement>().Object;
            var elm2 = new Mock<IElement>().Object;
            colMock.SetupGet(x => x[0]).Returns(elm1);
            colMock.SetupGet(x => x[1]).Returns(elm2);
            colMock.SetupGet(x => x.Length).Returns(2);

            var sut = new HtmlCollectionWrapper<IElement>(() => colMock.Object);

            sut.ShouldAllBe(
                x => x.ShouldBeOfType<ElementWrapper>().WrappedObject.ShouldBe(elm1),
                x => x.ShouldBeOfType<ElementWrapper>().WrappedObject.ShouldBe(elm2)
            );
        }

        [Fact(DisplayName = "Nested wrappers also gets marked as stale when the parent does")]
        public void Test008()
        {
            var colMock = new Mock<IHtmlCollection<IElement>>();
            var elm1 = new Mock<IElement>().Object;
            colMock.SetupGet(x => x[0]).Returns(() => new Mock<IElement>().Object);
            var sut = new HtmlCollectionWrapper<IElement>(() => colMock.Object);
            var colElmWrapper = (ElementWrapper)sut[0];
            var initialWrappedColElm = colElmWrapper.WrappedObject;

            sut.MarkAsStale();

            initialWrappedColElm.ShouldNotBeSameAs(colElmWrapper.WrappedObject);
        }
    }
}
