using System;
using System.Reflection;
using System.Threading.Tasks;
using AngleSharp;
using AngleSharp.Dom;
using Bunit.Rendering;
using Bunit.TestAssets.SampleComponents;
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
		public void CanRaiseEvents(MethodInfo helper)
		{
			if (helper is null)
				throw new ArgumentNullException(nameof(helper));

			if (helper.Name == nameof(TriggerEventDispatchExtensions.TriggerEventAsync))
				return;

			VerifyEventRaisesCorrectly(helper, EventArgs.Empty);
		}

		[Fact(DisplayName = "TriggerEventAsync throws element is null")]
		public void Test001()
		{
			IElement elm = default!;
			Should.Throw<ArgumentNullException>(() => elm.TriggerEventAsync(string.Empty, EventArgs.Empty))
				.ParamName.ShouldBe("element");
		}

		[Fact(DisplayName = "TriggerEventAsync throws if element does not contain an attribute with the blazor event-name")]
		public void Test002()
		{
			var cut = RenderComponent<Simple1>();

			Should.Throw<MissingEventHandlerException>(() => cut.Find("h1").Click());
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

		[Fact(DisplayName = "When clicking on an element with an event handler, " +
							"event handlers higher up the DOM tree is also triggered")]
		public void Test100()
		{
			var cut = RenderComponent<ClickEventBubbling>();

			cut.Find("span").Click();

			cut.Instance.SpanClickCount.ShouldBe(1);
			cut.Instance.HeaderClickCount.ShouldBe(1);
		}

		[Fact(DisplayName = "When clicking on an element without an event handler attached, " +
							"event handlers higher up the DOM tree is triggered")]
		public void Test101()
		{
			var cut = RenderComponent<ClickEventBubbling>();

			cut.Find("button").Click();

			cut.Instance.SpanClickCount.ShouldBe(0);
			cut.Instance.HeaderClickCount.ShouldBe(1);
		}

		[Theory(DisplayName = "When clicking element with non-bubbling events, the event does not bubble")]
		[InlineData("onabort")]
		[InlineData("onblur")]
		[InlineData("onchange")]
		[InlineData("onerror")]
		[InlineData("onfocus")]
		[InlineData("onload")]
		[InlineData("onloadend")]
		[InlineData("onloadstart")]
		[InlineData("onmouseenter")]
		[InlineData("onmouseleave")]
		[InlineData("onprogress")]
		[InlineData("onreset")]
		[InlineData("onscroll")]
		[InlineData("onsubmit")]
		[InlineData("onunload")]
		[InlineData("ontoggle")]
		[InlineData("onDOMNodeInsertedIntoDocument")]
		[InlineData("onDOMNodeRemovedFromDocument")]
		public async Task Test110(string eventName)
		{
			var cut = RenderComponent<EventBubbles>(ps => ps.Add(p => p.EventName, eventName));

			await cut.Find("#child").TriggerEventAsync(eventName, EventArgs.Empty);

			cut.Instance.ChildTriggerCount.ShouldBe(1);
			cut.Instance.ParentTriggerCount.ShouldBe(0);
			cut.Instance.GrandParentTriggerCount.ShouldBe(0);
		}

		[Fact(DisplayName = "When event has StopPropergation modifier, events does not bubble from target")]
		public async Task Test111()
		{
			var cut = RenderComponent<EventBubbles>(ps => ps
				.Add(p => p.EventName, "onclick")
				.Add(p => p.ChildStopPropergation, true));

			await cut.Find("#child").TriggerEventAsync("onclick", EventArgs.Empty);

			cut.Instance.ChildTriggerCount.ShouldBe(1);
			cut.Instance.ParentTriggerCount.ShouldBe(0);
			cut.Instance.GrandParentTriggerCount.ShouldBe(0);
		}

		[Fact(DisplayName = "When event has StopPropergation modifier, events does not bubble from parent of target")]
		public async Task Test112()
		{
			var cut = RenderComponent<EventBubbles>(ps => ps
				.Add(p => p.EventName, "onclick")
				.Add(p => p.ParentStopPropergation, true));

			await cut.Find("#child").TriggerEventAsync("onclick", EventArgs.Empty);

			cut.Instance.ChildTriggerCount.ShouldBe(1);
			cut.Instance.ParentTriggerCount.ShouldBe(1);
			cut.Instance.GrandParentTriggerCount.ShouldBe(0);
		}

		[Theory(DisplayName = "Disabled input elements does not bubble for event type"), PairwiseData]
		public async Task Test113(
			[CombinatorialValues("onclick", "ondblclick", "onmousedown", "onmousemove", "onmouseup")] string eventName,
			[CombinatorialValues("button", "input", "textarea", "select")] string elementType)
		{
			var cut = RenderComponent<EventBubbles>(ps => ps
				.Add(p => p.EventName, eventName)
				.Add(p => p.ChildElementType, elementType)
				.Add(p => p.ChildElementDisabled, true));

			await cut.Find("#child").TriggerEventAsync(eventName, EventArgs.Empty);

			cut.Instance.ChildTriggerCount.ShouldBe(1);
			cut.Instance.ParentTriggerCount.ShouldBe(0);
			cut.Instance.GrandParentTriggerCount.ShouldBe(0);
		}

		[Theory(DisplayName = "Enabled input elements does not bubble for event type"), PairwiseData]
		public async Task Test114(
			[CombinatorialValues("onclick", "ondblclick", "onmousedown", "onmousemove", "onmouseup")] string eventName,
			[CombinatorialValues("button", "input", "textarea", "select")] string elementType)
		{
			var cut = RenderComponent<EventBubbles>(ps => ps
				.Add(p => p.EventName, eventName)
				.Add(p => p.ChildElementType, elementType)
				.Add(p => p.ChildElementDisabled, false));

			await cut.Find("#child").TriggerEventAsync(eventName, EventArgs.Empty);

			cut.Instance.ChildTriggerCount.ShouldBe(1);
			cut.Instance.ParentTriggerCount.ShouldBe(1);
			cut.Instance.GrandParentTriggerCount.ShouldBe(1);
		}
	}
}
