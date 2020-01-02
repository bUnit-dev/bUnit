using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Egil.RazorComponents.Testing.EventDispatchExtensions
{
    public class GeneralEventDispatchExtensionsTest : EventDispatchExtensionsTest<EventArgs>
    {
        protected override string ElementName => "p";

        [Theory(DisplayName = "General events are raised correctly through helpers")]
        [MemberData(nameof(GetEventHelperMethods), typeof(GeneralEventDispatchExtensions))]
        public async Task CanRaiseEvents(MethodInfo helper)
        {
            if (helper is null) throw new ArgumentNullException(nameof(helper));

            if (helper.Name == nameof(GeneralEventDispatchExtensions.TriggerEventAsync)) return;

            await VerifyEventRaisesCorrectly(helper, EventArgs.Empty);
        }
    }
}
