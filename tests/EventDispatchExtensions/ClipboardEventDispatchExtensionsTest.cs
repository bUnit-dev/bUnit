using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Egil.RazorComponents.Testing.SampleComponents;
using Egil.RazorComponents.Testing.SampleComponents.Data;
using Microsoft.AspNetCore.Components.Web;
using Moq;
using Shouldly;
using Xunit;

namespace Egil.RazorComponents.Testing.EventDispatchExtensions
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

