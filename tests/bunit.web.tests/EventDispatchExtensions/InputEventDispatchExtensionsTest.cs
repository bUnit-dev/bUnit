using System.Reflection;
using Microsoft.AspNetCore.Components;
using Xunit;

namespace Bunit
{
	public class InputEventDispatchExtensionsTest : EventDispatchExtensionsTest<ChangeEventArgs>
	{
		protected override string ElementName => "input";

		[Theory(DisplayName = "Input events are raised correctly through helpers")]
		[MemberData(nameof(GetEventHelperMethods), typeof(InputEventDispatchExtensions))]
		public void CanRaiseEvents(MethodInfo helper)
		{
			var expected = new ChangeEventArgs()
			{
				Value = "SOME VALUE",
			};

			VerifyEventRaisesCorrectly(helper, expected);
		}
	}
}
