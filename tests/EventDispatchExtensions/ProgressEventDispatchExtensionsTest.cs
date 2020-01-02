using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Web;
using Xunit;

namespace Egil.RazorComponents.Testing.EventDispatchExtensions
{
    public class ProgressEventDispatchExtensionsTest : EventDispatchExtensionsTest<ProgressEventArgs>
    {
        protected override string ElementName => "div";

        [Theory(DisplayName = "Progress events are raised correctly through helpers")]
        [MemberData(nameof(GetEventHelperMethods), typeof(ProgressEventDispatchExtensions))]
        public async Task CanRaiseEvents(MethodInfo helper)
        {
            var expected = new ProgressEventArgs()
            {
                LengthComputable = true,
                Loaded = 42L,
                Total = 1337L,
                Type = "FILE"                
            };

            await VerifyEventRaisesCorrectly(helper, expected);
        }
    }
}
