using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using AngleSharp.Dom;
using Moq;
using Egil.RazorComponents.Testing.TestUtililities;
using Xunit;
using Shouldly;
using Egil.RazorComponents.Testing.SampleComponents;
using Egil.RazorComponents.Testing.Asserting;
using AngleSharp.Html.Dom;
using DeepEqual.Syntax;

namespace Egil.RazorComponents.Testing.Rendering.HtmlDomWrappers
{
    public class ElementWrapperTest
    {
        public static IEnumerable<MethodInfo[]> GetInterfaceMethods(Type type) => type.GetInterfaceMethods().Select(x => new[] { x });

        [Theory(DisplayName = "Forwards all method and property calls to wrapped node")]
        [MemberData(nameof(GetInterfaceMethods), typeof(IElement))]
        public void Test001(MethodInfo method)
        {
            var elmMock = new Mock<IElement>();
            var sut = new ElementWrapper(() => elmMock.Object);
            var args = method.CreateMethodArguments();

            method.Invoke(sut, args);

            var inv = elmMock.Invocations[0];
            inv.Arguments.ShouldBe(args);
            inv.Method.ShouldBe(method);
        }

        [Fact(DisplayName = "Wrapped node is internally available")]
        public void Test002()
        {
            var elmMock = Mock.Of<IElement>();

            var sut = new ElementWrapper(() => elmMock);

            sut.WrappedNode.ShouldBe(elmMock);
        }

        [Fact(DisplayName = "Wrapper refreshes wrapped node after MarkAsStale is called")]
        public void Test003()
        {
            var callCount = 0;
            var sut = new ElementWrapper(() => { callCount++; return Mock.Of<IElement>(); });
            var firstWrapped = sut.WrappedNode;

            sut.MarkAsStale();

            sut.WrappedNode.ShouldNotBe(firstWrapped);
            callCount.ShouldBe(2);
        }

        [Fact(DisplayName = "When a wrapped node is no longer available, accessing methods or properties throws ElementNoLongerAvailableException")]
        public void Test004()
        {
            var sut = new ElementWrapper(() => null);

            Should.Throw<NodeNoLongerAvailableException>(() => sut.WrappedNode);
        }

        [Fact(DisplayName = "When a method or property on an wrapped node returns an INode, it is wrapped")]
        public void Test005()
        {
            var elmMock = new Mock<IElement>();
            var elmParent = Mock.Of<IElement>();
            elmMock.SetupGet(x => x.ParentElement).Returns(elmParent);
            var sut = new ElementWrapper(() => elmMock.Object);

            var parent = sut.ParentElement;

            parent.ShouldBeOfType<ElementWrapper>().WrappedNode.ShouldBe(elmParent);
        }

        [Fact(DisplayName = "The same wrapper is used every time")]
        public void Test006()
        {
            var elmMock = new Mock<IElement>();
            elmMock.SetupGet(x => x.ParentElement).Returns(() => Mock.Of<IElement>());
            var sut = new ElementWrapper(() => elmMock.Object);

            sut.ParentElement.ShouldBeSameAs(sut.ParentElement);
        }

        [Fact(DisplayName = "Nested wrappers also gets marked as stale when the parent does")]
        public void Test007()
        {
            var elmMock = new Mock<IElement>();
            elmMock.SetupGet(x => x.ParentElement).Returns(() => new Mock<IElement>().Object);
            var sut = new ElementWrapper(() => elmMock.Object);
            var parentElementWrapper = ((ElementWrapper)sut.ParentElement);
            var initialWrappedParentNode = parentElementWrapper.WrappedNode;

            sut.MarkAsStale();

            initialWrappedParentNode.ShouldNotBeSameAs(parentElementWrapper.WrappedNode);
        }
    }
}
