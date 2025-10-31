namespace Bunit.EventDispatchExtensions;

public class FormDispatchExtensionTest : BunitContext
{
	[Fact]
	public void ClickingOnSubmitButtonTriggersOnsubmitOfForm()
	{
		var cut = Render<FormWithButton>(p => p.Add(s => s.HasSubmitType, true));

		cut.Find("#submitter").Click();

		cut.Instance.SubmitWasCalled.ShouldBeTrue();
	}

	[Fact]
	public void ButtonWithoutTypeIsConsideredSubmitAndTriggersOnsubmitOfForm()
	{
		var cut = Render<FormWithButton>(p => p.Add(s => s.HasSubmitType, false));

		cut.Find("#submitter").Click();

		cut.Instance.SubmitWasCalled.ShouldBeTrue();
	}

	[Fact]
	public void ButtonThatIsNotSubmitShouldNotTrigger()
	{
		var cut = Render<FormWithButton>(p => p.Add(s => s.HasSubmitType, false));

		cut.Find("#other").Click();

		cut.Instance.SubmitWasCalled.ShouldBeFalse();
	}

	[Fact]
	public void ClickingOnSubmitButtonOutsideTriggersOnsubmitOfForm()
	{
		var cut = Render<FormWithButtonOutside>();

		cut.Find("#submitter").Click();

		cut.Instance.SubmitWasCalled.ShouldBeTrue();
	}
}
