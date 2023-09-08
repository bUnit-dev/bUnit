using Xunit.Abstractions;

namespace Bunit;

public class ClipboardEventDispatchExtensionsTest : EventDispatchExtensionsTest<ClipboardEventArgs>
{
	public ClipboardEventDispatchExtensionsTest(ITestOutputHelper outputHelper) : base(outputHelper)
	{
	}

	protected override string ElementName => "textarea";

	[Theory(DisplayName = "Clipboard events are raised correctly through helpers")]
	[MemberData(nameof(GetEventHelperMethods), typeof(ClipboardEventDispatchExtensions))]
	public void CanRaiseEvents(MethodInfo helper)
	{
		var expected = new ClipboardEventArgs
		{
			Type = "SOME TYPE",
		};

		VerifyEventRaisesCorrectly(helper, expected);
	}
}
