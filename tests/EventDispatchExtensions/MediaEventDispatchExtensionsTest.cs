using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;

namespace Egil.RazorComponents.Testing.EventDispatchExtensions
{
    public class MediaEventDispatchExtensionsTest : EventDispatchExtensionsTest<EventArgs>
    {
        protected override string ElementName => "audio";

        [Theory(DisplayName = "Media events are raised correctly through helpers")]
        [MemberData(nameof(GetEventHelperMethods), typeof(MediaEventDispatchExtensions))]
        public async Task CanRaiseEvents(MethodInfo helper)
        {
            await VerifyEventRaisesCorrectly(helper, EventArgs.Empty);
        }
    }
}

