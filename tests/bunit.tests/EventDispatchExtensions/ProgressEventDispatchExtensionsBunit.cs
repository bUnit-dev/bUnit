namespace Bunit;

public class ProgressEventDispatchExtensionsBunit : EventDispatchExtensionsBunit<ProgressEventArgs>
{
	public ProgressEventDispatchExtensionsBunit(ITestOutputHelper outputHelper) : base(outputHelper)
	{
	}

	protected override string ElementName => "div";

	[Theory(DisplayName = "Progress events are raised correctly through helpers")]
	[MemberData(nameof(GetEventHelperMethods), typeof(ProgressEventDispatchExtensions))]
	public void CanRaiseEvents(MethodInfo helper)
	{
		var expected = new ProgressEventArgs
		{
			LengthComputable = true,
			Loaded = 42L,
			Total = 1337L,
			Type = "FILE",
		};

		VerifyEventRaisesCorrectly(helper, expected);
	}
}
