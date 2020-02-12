using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Bunit.SampleComponents;
using Bunit.SampleComponents.Data;
using Microsoft.AspNetCore.Components.Web;
using Moq;
using Shouldly;
using Xunit;

namespace Bunit
{
    public class ClipboardEventDispatchExtensionsTest : EventDispatchExtensionsTest<ClipboardEventArgs>
    {
        protected override string ElementName => "textarea";

        [Theory(DisplayName = "Clipboard events are raised correctly through helpers")]
        [MemberData(nameof(GetEventHelperMethods), typeof(ClipboardEventDispatchExtensions))]
        public async Task CanRaiseEvents(MethodInfo helper)
        {
            var expected = new ClipboardEventArgs()
            {
                Type = "SOME TYPE"
            };

            await VerifyEventRaisesCorrectly(helper, expected);
        }
    }
}

