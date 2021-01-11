using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.AspNetCore.Components.Web;
using Xunit;

namespace Bunit
{
	public class WheelEventDispatchExtensionsTest : EventDispatchExtensionsTest<WheelEventArgs>
	{
		public static IEnumerable<MethodInfo[]> Helpers { get; } = GetEventHelperMethods(typeof(MouseEventDispatchExtensions), x => x.Name.Contains("Wheel", StringComparison.OrdinalIgnoreCase));

		protected override string ElementName => "button";

		[Theory(DisplayName = "Mouse wheel/wheel events are raised correctly through helpers")]
		[MemberData(nameof(Helpers))]
		public void CanRaiseEvents(MethodInfo helper)
		{
			var expected = new WheelEventArgs
			{
				DeltaMode = 1337L,
				DeltaX = 4242D,
				DeltaY = 4141D,
				DeltaZ = 4040D,
				Detail = 2,
				ScreenX = 3,
				ScreenY = 4,
				ClientX = 5,
				ClientY = 6,
				Button = 7,
				Buttons = 8,
				CtrlKey = true,
				AltKey = true,
				MetaKey = true,
				Type = "TYPE",
			};

			VerifyEventRaisesCorrectly(helper, expected);
		}
	}
}
