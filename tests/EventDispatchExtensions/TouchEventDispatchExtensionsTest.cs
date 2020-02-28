﻿using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Web;
using Xunit;
using System.Diagnostics.CodeAnalysis;

namespace Bunit
{
    public class TouchEventDispatchExtensionsTest : EventDispatchExtensionsTest<TouchEventArgs>
    {
        protected override string ElementName => "p";

        [Theory(DisplayName = "Touch events are raised correctly through helpers")]
        [MemberData(nameof(GetEventHelperMethods), typeof(TouchEventDispatchExtensions))]
        [SuppressMessage("Performance", "CA1825:Avoid zero-length array allocations.", Justification = "<Pending>")]
        public async Task CanRaiseEvents(MethodInfo helper)
        {
            var expected = new TouchEventArgs
            {
                AltKey = true,
                ChangedTouches = new TouchPoint[0],
                CtrlKey = true,
                Detail = 13L,
                MetaKey = true,
                ShiftKey = true,
                TargetTouches = new TouchPoint[0],
                Touches = new TouchPoint[0],
                Type = "TOUCH"
            };

            await VerifyEventRaisesCorrectly(helper, expected);
        }
    }
}
