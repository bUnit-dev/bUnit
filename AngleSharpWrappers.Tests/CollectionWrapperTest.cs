using System.Linq;
using AngleSharp.Css.Dom;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.Io.Dom;
using AngleSharp.Media.Dom;
using Moq;
using Shouldly;
using Xunit;

namespace AngleSharpWrappers
{
    public class CollectionWrapperTest
    {
        [Fact(DisplayName = "HtmlAllCollectionWrapper Items returned by Enumerators are wrapped")]
        public void Test001()
        {
            var colMock = new Mock<IHtmlAllCollection>();
            var elm1 = new Mock<IElement>().Object;
            var elm2 = new Mock<IElement>().Object;
            colMock.SetupGet(x => x[0]).Returns(elm1);
            colMock.SetupGet(x => x[1]).Returns(elm2);
            colMock.SetupGet(x => x.Length).Returns(2);

            var sut = new HtmlAllCollectionWrapper(() => colMock.Object);

            sut.ShouldAllBe(
                x => x.ShouldBeOfType<ElementWrapper>().WrappedObject.ShouldBe(elm1),
                x => x.ShouldBeOfType<ElementWrapper>().WrappedObject.ShouldBe(elm2)
            );
            ((System.Collections.IEnumerable)sut).Cast<object>().ShouldAllBe(
                x => x.ShouldBeOfType<ElementWrapper>().WrappedObject.ShouldBe(elm1),
                x => x.ShouldBeOfType<ElementWrapper>().WrappedObject.ShouldBe(elm2)
            );
        }

        [Fact(DisplayName = "HtmlFormControlsCollectionWrapper Items returned by Enumerators are wrapped")]
        public void Test002()
        {
            var colMock = new Mock<IHtmlFormControlsCollection>();
            var elm1 = new Mock<IHtmlElement>().Object;
            var elm2 = new Mock<IHtmlElement>().Object;
            colMock.SetupGet(x => x[0]).Returns(elm1);
            colMock.SetupGet(x => x[1]).Returns(elm2);
            colMock.SetupGet(x => x.Length).Returns(2);

            var sut = new HtmlFormControlsCollectionWrapper(() => colMock.Object);

            sut.ShouldAllBe(
                x => x.ShouldBeOfType<HtmlElementWrapper>().WrappedObject.ShouldBe(elm1),
                x => x.ShouldBeOfType<HtmlElementWrapper>().WrappedObject.ShouldBe(elm2)
            );
            ((System.Collections.IEnumerable)sut).Cast<object>().ShouldAllBe(
                x => x.ShouldBeOfType<HtmlElementWrapper>().WrappedObject.ShouldBe(elm1),
                x => x.ShouldBeOfType<HtmlElementWrapper>().WrappedObject.ShouldBe(elm2)
            );
        }

        [Fact(DisplayName = "HtmlOptionsCollectionWrapper Items returned by Enumerators are wrapped")]
        public void Test003()
        {
            var colMock = new Mock<IHtmlOptionsCollection>();
            var elm1 = new Mock<IHtmlOptionElement>().Object;
            var elm2 = new Mock<IHtmlOptionElement>().Object;
            colMock.SetupGet(x => x[0]).Returns(elm1);
            colMock.SetupGet(x => x[1]).Returns(elm2);
            colMock.SetupGet(x => x.Length).Returns(2);

            var sut = new HtmlOptionsCollectionWrapper(() => colMock.Object);

            sut.ShouldAllBe(
                x => x.ShouldBeOfType<HtmlOptionElementWrapper>().WrappedObject.ShouldBe(elm1),
                x => x.ShouldBeOfType<HtmlOptionElementWrapper>().WrappedObject.ShouldBe(elm2)
            );
            ((System.Collections.IEnumerable)sut).Cast<object>().ShouldAllBe(
                x => x.ShouldBeOfType<HtmlOptionElementWrapper>().WrappedObject.ShouldBe(elm1),
                x => x.ShouldBeOfType<HtmlOptionElementWrapper>().WrappedObject.ShouldBe(elm2)
            );
        }

        [Fact(DisplayName = "NamedNodeMapWrapper Items returned by Enumerators are wrapped")]
        public void Test004()
        {
            var colMock = new Mock<INamedNodeMap>();
            var elm1 = new Mock<IAttr>().Object;
            var elm2 = new Mock<IAttr>().Object;
            colMock.SetupGet(x => x[0]).Returns(elm1);
            colMock.SetupGet(x => x[1]).Returns(elm2);
            colMock.SetupGet(x => x.Length).Returns(2);

            var sut = new NamedNodeMapWrapper(() => colMock.Object);

            sut.ShouldAllBe(
                x => x.ShouldBeOfType<AttrWrapper>().WrappedObject.ShouldBe(elm1),
                x => x.ShouldBeOfType<AttrWrapper>().WrappedObject.ShouldBe(elm2)
            );

            ((System.Collections.IEnumerable)sut).Cast<object>().ShouldAllBe(
                x => x.ShouldBeOfType<AttrWrapper>().WrappedObject.ShouldBe(elm1),
                x => x.ShouldBeOfType<AttrWrapper>().WrappedObject.ShouldBe(elm2)
            );
        }

        [Fact(DisplayName = "NodeListWrapper Items returned by Enumerators are wrapped")]
        public void Test005()
        {
            var colMock = new Mock<INodeList>();
            var elm1 = new Mock<INode>().Object;
            var elm2 = new Mock<INode>().Object;
            colMock.SetupGet(x => x[0]).Returns(elm1);
            colMock.SetupGet(x => x[1]).Returns(elm2);
            colMock.SetupGet(x => x.Length).Returns(2);

            var sut = new NodeListWrapper(() => colMock.Object);

            sut.ShouldAllBe(
                x => x.ShouldBeOfType<NodeWrapper>().WrappedObject.ShouldBe(elm1),
                x => x.ShouldBeOfType<NodeWrapper>().WrappedObject.ShouldBe(elm2)
            );
            ((System.Collections.IEnumerable)sut).Cast<object>().ShouldAllBe(
                x => x.ShouldBeOfType<NodeWrapper>().WrappedObject.ShouldBe(elm1),
                x => x.ShouldBeOfType<NodeWrapper>().WrappedObject.ShouldBe(elm2)
            );
        }

        [Fact(DisplayName = "AudioTrackListWrapper GetEnumerator forwards to wrapper")]
        public void Test011()
        {
            var colMock = new Mock<IAudioTrackList>();

            var sut = new AudioTrackListWrapper(() => colMock.Object);

            sut.GetEnumerator();
            ((System.Collections.IEnumerable)sut).GetEnumerator();

            colMock.Verify(x => x.GetEnumerator(), Times.Exactly(2));
        }

        [Fact(DisplayName = "FileListWrapper GetEnumerator forwards to wrapper")]
        public void Test012()
        {
            var colMock = new Mock<IFileList>();

            var sut = new FileListWrapper(() => colMock.Object);

            sut.GetEnumerator();
            ((System.Collections.IEnumerable)sut).GetEnumerator();

            colMock.Verify(x => x.GetEnumerator(), Times.Exactly(2));
        }

        [Fact(DisplayName = "MediaListWrapper GetEnumerator forwards to wrapper")]
        public void Test013()
        {
            var colMock = new Mock<IMediaList>();

            var sut = new MediaListWrapper(() => colMock.Object);

            sut.GetEnumerator();
            ((System.Collections.IEnumerable)sut).GetEnumerator();

            colMock.Verify(x => x.GetEnumerator(), Times.Exactly(2));
        }

        [Fact(DisplayName = "SettableTokenListWrapper GetEnumerator forwards to wrapper")]
        public void Test014()
        {
            var colMock = new Mock<ISettableTokenList>();

            var sut = new SettableTokenListWrapper(() => colMock.Object);

            sut.GetEnumerator();
            ((System.Collections.IEnumerable)sut).GetEnumerator();

            colMock.Verify(x => x.GetEnumerator(), Times.Exactly(2));
        }

        [Fact(DisplayName = "StringListWrapper GetEnumerator forwards to wrapper")]
        public void Test015()
        {
            var colMock = new Mock<IStringList>();

            var sut = new StringListWrapper(() => colMock.Object);

            sut.GetEnumerator();
            ((System.Collections.IEnumerable)sut).GetEnumerator();

            colMock.Verify(x => x.GetEnumerator(), Times.Exactly(2));
        }

        [Fact(DisplayName = "StringMapWrapper GetEnumerator forwards to wrapper")]
        public void Test016()
        {
            var colMock = new Mock<IStringMap>();

            var sut = new StringMapWrapper(() => colMock.Object);

            sut.GetEnumerator();
            ((System.Collections.IEnumerable)sut).GetEnumerator();

            colMock.Verify(x => x.GetEnumerator(), Times.Exactly(2));
        }

        [Fact(DisplayName = "StyleSheetListWrapper GetEnumerator forwards to wrapper")]
        public void Test017()
        {
            var colMock = new Mock<IStyleSheetList>();

            var sut = new StyleSheetListWrapper(() => colMock.Object);

            sut.GetEnumerator();
            ((System.Collections.IEnumerable)sut).GetEnumerator();

            colMock.Verify(x => x.GetEnumerator(), Times.Exactly(2));
        }


        [Fact(DisplayName = "TextTrackCueListWrapper GetEnumerator forwards to wrapper")]
        public void Test018()
        {
            var colMock = new Mock<ITextTrackCueList>();

            var sut = new TextTrackCueListWrapper(() => colMock.Object);

            sut.GetEnumerator();
            ((System.Collections.IEnumerable)sut).GetEnumerator();

            colMock.Verify(x => x.GetEnumerator(), Times.Exactly(2));
        }

        [Fact(DisplayName = "TextTrackListWrapper GetEnumerator forwards to wrapper")]
        public void Test019()
        {
            var colMock = new Mock<ITextTrackList>();

            var sut = new TextTrackListWrapper(() => colMock.Object);

            sut.GetEnumerator();
            ((System.Collections.IEnumerable)sut).GetEnumerator();

            colMock.Verify(x => x.GetEnumerator(), Times.Exactly(2));
        }

        [Fact(DisplayName = "TokenListWrapper GetEnumerator forwards to wrapper")]
        public void Test020()
        {
            var colMock = new Mock<ITokenList>();

            var sut = new TokenListWrapper(() => colMock.Object);

            sut.GetEnumerator();
            ((System.Collections.IEnumerable)sut).GetEnumerator();

            colMock.Verify(x => x.GetEnumerator(), Times.Exactly(2));
        }

        [Fact(DisplayName = "VideoTrackListWrapper GetEnumerator forwards to wrapper")]
        public void Test021()
        {
            var colMock = new Mock<IVideoTrackList>();

            var sut = new VideoTrackListWrapper(() => colMock.Object);

            sut.GetEnumerator();
            ((System.Collections.IEnumerable)sut).GetEnumerator();

            colMock.Verify(x => x.GetEnumerator(), Times.Exactly(2));
        }

    }
}
