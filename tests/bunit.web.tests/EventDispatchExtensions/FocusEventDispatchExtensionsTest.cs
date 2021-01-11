using System.Reflection;
using Microsoft.AspNetCore.Components.Web;
using Xunit;

namespace Bunit
{
	public class FocusEventDispatchExtensionsTest : EventDispatchExtensionsTest<FocusEventArgs>
	{
		protected override string ElementName => "p";

		[Theory(DisplayName = "Focus events are raised correctly through helpers")]
		[MemberData(nameof(GetEventHelperMethods), typeof(FocusEventDispatchExtensions))]
		public void CanRaiseEvents(MethodInfo helper)
		{
			var expected = new FocusEventArgs()
			{
				Type = "SOME TYPE",
			};

			VerifyEventRaisesCorrectly(helper, expected);
		}
	}
}
