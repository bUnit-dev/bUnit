namespace Bunit;

public class FocusEventDispatchExtensionsBunit : EventDispatchExtensionsBunit<FocusEventArgs>
{
	public FocusEventDispatchExtensionsBunit(ITestOutputHelper outputHelper) : base(outputHelper)
	{
	}

	protected override string ElementName => "p";

	[Theory(DisplayName = "Focus events are raised correctly through helpers")]
	[MemberData(nameof(GetEventHelperMethods), typeof(FocusEventDispatchExtensions))]
	public void CanRaiseEvents(MethodInfo helper)
	{
		var expected = new FocusEventArgs
		{
			Type = "SOME TYPE",
		};

		VerifyEventRaisesCorrectly(helper, expected);
	}
}
