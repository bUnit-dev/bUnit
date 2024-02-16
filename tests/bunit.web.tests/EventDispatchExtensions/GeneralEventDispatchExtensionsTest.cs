using AngleSharp;
using AngleSharp.Dom;
using Bunit.Rendering;

namespace Bunit;

public class GeneralEventDispatchExtensionsTest : EventDispatchExtensionsTest<EventArgs>
{
	protected override string ElementName => "p";

	public GeneralEventDispatchExtensionsTest(ITestOutputHelper outputHelper)
		: base(outputHelper)
	{
		TestContext.DefaultWaitTimeout = TimeSpan.FromSeconds(30);
	}

	[Theory(DisplayName = "General events are raised correctly through helpers")]
	[MemberData(nameof(GetEventHelperMethods), typeof(GeneralEventDispatchExtensions))]
	public void CanRaiseEvents(MethodInfo helper)
	{
		if (helper is null)
		{
			throw new ArgumentNullException(nameof(helper));
		}

		if (helper.Name == nameof(TriggerEventDispatchExtensions.TriggerEventAsync))
		{
			return;
		}

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
		var elmMock = Substitute.For<IElement>();
		var docMock = Substitute.For<IDocument>();
		var ctxMock = Substitute.For<IBrowsingContext>();

		elmMock.GetAttribute(Arg.Any<string>()).Returns("1");
		elmMock.Owner.Returns(docMock);
		docMock.Context.Returns(ctxMock);

		Should.Throw<InvalidOperationException>(() => elmMock.TriggerEventAsync("click", EventArgs.Empty));
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
	[InlineData("onunload")]
	[InlineData("ontoggle")]
	[InlineData("onDOMNodeInsertedIntoDocument")]
	[InlineData("onDOMNodeRemovedFromDocument")]
	[InlineData("oninvalid")]
	[InlineData("onpointerleave")]
	[InlineData("onpointerenter")]
	[InlineData("onselectionchange")]
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
			.Add(p => p.ChildStopPropagation, true));

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
			.Add(p => p.ParentStopPropagation, true));

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

#if NET6_0_OR_GREATER
	[Fact(DisplayName = "TriggerEvent can trigger custom events")]
	public void Test201()
	{
		var cut = RenderComponent<CustomPasteSample>();

		cut.Find("input").TriggerEvent("oncustompaste", new CustomPasteEventArgs
		{
			EventTimestamp = DateTime.Now,
			PastedData = "FOO"
		});

		cut.Find("p:last-child").MarkupMatches("<p>You pasted: FOO</p>");
	}
#endif

	[Fact(DisplayName = "TriggerEventAsync throws NoEventHandlerException when invoked with an unknown event handler ID")]
	public void Test300()
	{
		var cut = RenderComponent<ClickRemovesEventHandler>();
		var buttons = cut.FindAll("button");
		buttons[0].Click();

		Should.Throw<UnknownEventHandlerIdException>(() => buttons[1].Click());
	}

	[Fact(DisplayName = "Removed bubbled event handled NoEventHandlerException are ignored")]
	public void Test301()
	{
		var cut = RenderComponent<BubbleEventsRemoveTriggers>();

		cut.Find("button").Click();

		// When middle div clicked event handlers is disposed, the
		// NoEventHandlerException is ignored and the top div clicked event
		// handler is still invoked.
		cut.Instance.BtnClicked.ShouldBeTrue();
		cut.Instance.MiddleDivClicked.ShouldBeFalse();
		cut.Instance.TopDivClicked.ShouldBeTrue();
	}

	[Theory(DisplayName = "When bubbling event throws, no other event handlers are triggered")]
	[AutoData]
	public void Test302(string exceptionMessage)
	{
		var cut = RenderComponent<BubbleEventsThrows>(ps => ps.Add(p => p.ExceptionMessage, exceptionMessage));

		Should.Throw<Exception>(() => cut.Find("button").Click())
			.Message.ShouldBe(exceptionMessage);

		cut.Instance.BtnClicked.ShouldBeTrue();
		cut.Instance.MiddleDivClicked.ShouldBeFalse();
		cut.Instance.TopDivClicked.ShouldBeFalse();
	}

	[Theory(DisplayName = "When event handler throws, the exception is passed up to test")]
	[AutoData]
	public void Test303(string exceptionMessage)
	{
		var cut = RenderComponent<EventHandlerThrows>(ps => ps.Add(p => p.ExceptionMessage, exceptionMessage));

		Should.Throw<Exception>(() => cut.Find("button").Click())
			.Message.ShouldBe(exceptionMessage);
	}

	[Fact(DisplayName = "Should handle click event first and submit form afterwards for button")]
	public void Test304()
	{
		var cut = RenderComponent<SubmitFormOnClick>();

		cut.Find("button").Click();

		cut.Instance.FormSubmitted.ShouldBeTrue();
		cut.Instance.Clicked.ShouldBeTrue();
	}

	[Fact(DisplayName = "Should handle click event first and submit form afterwards for input when type button")]
	public void Test305()
	{
		var cut = RenderComponent<SubmitFormOnClick>();

		cut.Find("#inside-form-input").Click();

		cut.Instance.FormSubmitted.ShouldBeTrue();
		cut.Instance.Clicked.ShouldBeTrue();
	}

	[Fact(DisplayName = "Should throw exception when invoking onsubmit from non form")]
	public void Test306()
	{
		var cut = RenderComponent<OnsubmitButton>();

		Should.Throw<InvalidOperationException>(() => cut.Find("button").Submit());
	}

	[Theory(DisplayName = "Should not submit a form if the button has preventDefault")]
	[InlineData("#inside-form-input")]
	[InlineData("#inside-form-button")]
	[InlineData("#span-inside-form-button")]
	public void Test307(string submitElementSelector)
	{
		var cut = RenderComponent<SubmitFormOnClick>(ps => ps
			.Add(x => x.PreventDefault, true));

		cut.Find(submitElementSelector).Click();

		cut.Instance.FormSubmitted.ShouldBeFalse();
		cut.Instance.Clicked.ShouldBeTrue();
	}

	[Theory(DisplayName = "Should submit a form when submit button clicked")]
	[InlineData("#inside-form-input")]
	[InlineData("#inside-form-button")]
	[InlineData("#span-inside-form-button")]
	[InlineData("#inside-form-input-no-handler")]
	[InlineData("#inside-form-button-no-handler")]
	[InlineData("#span-inside-form-button-no-handler")]
	public void Test308(string submitElementSelector)
	{
		var cut = RenderComponent<SubmitFormOnClick>();

		cut.Find(submitElementSelector).Click();

		cut.Instance.FormSubmitted.ShouldBeTrue();
	}

	[Theory(DisplayName = "Should trigger click handler of buttons inside form")]
	[InlineData("#inside-form-input")]
	[InlineData("#inside-form-button")]
	[InlineData("#span-inside-form-button")]
	public void Test309(string submitElementSelector)
	{
		var cut = RenderComponent<ButtonsInsideForm>();

		cut.Find(submitElementSelector).Click();

		cut.Instance.Clicked.ShouldBeTrue();
	}

	// Runs the test multiple times to trigger the race condition
	// reliably.
	[SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Needed to trigger multiple reruns of test.")]
	[Theory(DisplayName = "TriggerEventAsync avoids race condition with DOM tree updates")]
	[Repeat(10)]
	[Trait("Category", "async")]
	public async Task Test400(int repeatCount)
	{
		var cut = RenderComponent<CounterComponentDynamic>();

		await cut.WaitForAssertionAsync(() => cut.Find("[data-id=1]"));

		await cut.InvokeAsync(() => cut.Find("[data-id=1]").Click());

		await cut.WaitForAssertionAsync(() => cut.Find("[data-id=2]"));
	}

	// Runs the test multiple times to trigger the race condition
	// reliably.
	[SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Needed to trigger multiple reruns of test.")]
	[Theory(DisplayName = "TriggerEventAsync avoids race condition with DOM tree updates")]
	[Repeat(10)]
	[Trait("Category", "sync")]
	public async Task Test400_Sync(int repeatCount)
	{
		var cut = RenderComponent<CounterComponentDynamic>();

		cut.WaitForAssertion(() => cut.Find("[data-id=1]"));

		await cut.InvokeAsync(() => cut.Find("[data-id=1]").Click());

		cut.WaitForAssertion(() => cut.Find("[data-id=2]"));
	}
}
