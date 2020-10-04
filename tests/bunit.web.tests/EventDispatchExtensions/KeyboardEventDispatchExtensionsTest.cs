using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Components.Web;
using Xunit;

namespace Bunit
{
	public class KeyboardEventDispatchExtensionsTest : EventDispatchExtensionsTest<KeyboardEventArgs>
	{
		public static IEnumerable<MethodInfo[]> Helpers { get; } = GetEventHelperMethods(typeof(KeyboardEventDispatchExtensions), x => x.GetParameters().All(p => p.ParameterType != typeof(Key)));

		protected override string ElementName => "input";

		[Theory(DisplayName = "Keyboard events are raised correctly through helpers")]
		[MemberData(nameof(Helpers))]
		public void CanRaiseEvents(MethodInfo helper)
		{
			var expected = new KeyboardEventArgs()
			{
				AltKey = true,
				CtrlKey = true,
				Code = "X",
				Key = "B",
				Location = 42F,
				MetaKey = true,
				Repeat = true,
				ShiftKey = true,
				Type = "ASDF"
			};

			VerifyEventRaisesCorrectly(helper, expected);
		}
	}
}
