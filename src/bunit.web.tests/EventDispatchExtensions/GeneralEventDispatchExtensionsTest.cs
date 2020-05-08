using System;
using System.Reflection;
using System.Threading.Tasks;

using AngleSharp;
using AngleSharp.Dom;

using Bunit.Rendering;

using Moq;

using Shouldly;

using Xunit;

namespace Bunit
{
	public class GeneralEventDispatchExtensionsTest : EventDispatchExtensionsTest<EventArgs>
	{
		protected override string ElementName => "p";

		[Theory(DisplayName = "General events are raised correctly through helpers")]
		[MemberData(nameof(GetEventHelperMethods), typeof(GeneralEventDispatchExtensions))]
		public async Task CanRaiseEvents(MethodInfo helper)
		{
			if (helper is null)
				throw new ArgumentNullException(nameof(helper));

			if (helper.Name == nameof(GeneralEventDispatchExtensions.TriggerEventAsync))
				return;

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
			var elmMock = new Mock<IElement>();
			elmMock.Setup(x => x.GetAttribute(It.IsAny<string>())).Returns(() => null!);

			Should.Throw<MissingEventHandlerException>(() => elmMock.Object.Click());
		}

		[Fact(DisplayName = "TriggerEventAsync throws if element was not rendered through blazor (has a TestRendere in its context)")]
		public void Test003()
		{
			var elmMock = new Mock<IElement>();
			var docMock = new Mock<IDocument>();
			var ctxMock = new Mock<IBrowsingContext>();

			elmMock.Setup(x => x.GetAttribute(It.IsAny<string>())).Returns("1");
			elmMock.SetupGet(x => x.Owner).Returns(docMock.Object);
			docMock.SetupGet(x => x.Context).Returns(ctxMock.Object);
			ctxMock.Setup(x => x.GetService<ITestRenderer>()).Returns(() => null!);

			Should.Throw<InvalidOperationException>(() => elmMock.Object.TriggerEventAsync("click", EventArgs.Empty));
		}

	}
}
