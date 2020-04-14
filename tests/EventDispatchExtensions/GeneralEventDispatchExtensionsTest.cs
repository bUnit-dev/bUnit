using System;
using System.Reflection;
using System.Threading.Tasks;
using AngleSharp;
using AngleSharp.Dom;
using Telerik.JustMock;
using Shouldly;
using Xunit;
using Telerik.JustMock.Helpers;

namespace Bunit
{
    public class GeneralEventDispatchExtensionsTest : EventDispatchExtensionsTest<EventArgs>
    {
        protected override string ElementName => "p";

        [Theory(DisplayName = "General events are raised correctly through helpers")]
        [MemberData(nameof(GetEventHelperMethods), typeof(GeneralEventDispatchExtensions))]
        public async Task CanRaiseEvents(MethodInfo helper)
        {
            if (helper is null) throw new ArgumentNullException(nameof(helper));

            if (helper.Name == nameof(GeneralEventDispatchExtensions.TriggerEventAsync)) return;

            await VerifyEventRaisesCorrectly(helper, EventArgs.Empty);
        }

        [Fact(DisplayName = "TriggerEventAsync throws element is null")]
        public void Test001()
        {
            IElement elm = default!;
            Should.Throw<ArgumentNullException>(() => elm.TriggerEventAsync("", EventArgs.Empty))
                .ParamName.ShouldBe("element");
        }

        [Fact(DisplayName = "TriggerEventAsync throws if element does not contain an attribute with the blazor event-name")]
        public void Test002()
        {
            var elmMock = Mock.Create<IElement>();
            elmMock.Arrange(x => x.GetAttribute(Arg.IsAny<string>())).Returns(() => null!);

            Should.Throw<MissingEventHandlerException>(() => elmMock.Click());
        }

        [Fact(DisplayName = "TriggerEventAsync throws if element was not rendered through blazor (has a TestRendere in its context)")]
        public void Test003()
        {
            var elmMock = Mock.Create<IElement>();
            var docMock = Mock.Create<IDocument>();
            var ctxMock = Mock.Create<IBrowsingContext>();

            elmMock.Arrange(x => x.GetAttribute(Arg.IsAny<string>())).Returns("1");
            elmMock.Arrange(x => x.Owner).Returns(docMock);
            docMock.Arrange(x => x.Context).Returns(ctxMock);
            ctxMock.Arrange(x => x.GetService<TestRenderer>()).Returns(() => null!);

            Should.Throw<InvalidOperationException>(() => elmMock.TriggerEventAsync("click", EventArgs.Empty));
        }

    }
}
