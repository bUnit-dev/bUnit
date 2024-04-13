namespace Bunit;

public class FocusEventDispatchExtensionsTest : EventDispatchExtensionsTest<FocusEventArgs>
{
	public FocusEventDispatchExtensionsTest(ITestOutputHelper outputHelper) : base(outputHelper)
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
