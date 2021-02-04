using System.Reflection;
using Microsoft.AspNetCore.Components.Web;
using Xunit;

namespace Bunit
{
	public class ClipboardEventDispatchExtensionsTest : EventDispatchExtensionsTest<ClipboardEventArgs>
	{
		protected override string ElementName => "textarea";

		[Theory(DisplayName = "Clipboard events are raised correctly through helpers")]
		[MemberData(nameof(GetEventHelperMethods), typeof(ClipboardEventDispatchExtensions))]
		public void CanRaiseEvents(MethodInfo helper)
		{
			var expected = new ClipboardEventArgs()
			{
				Type = "SOME TYPE",
			};

			VerifyEventRaisesCorrectly(helper, expected);
		}
	}
}
