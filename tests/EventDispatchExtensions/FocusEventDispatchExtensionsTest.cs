using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Web;
using Xunit;

namespace Egil.RazorComponents.Testing.EventDispatchExtensions
{

    public class FocusEventDispatchExtensionsTest : EventDispatchExtensionsTest<FocusEventArgs>
    {
        protected override string ElementName => "p";

        [Theory(DisplayName = "Focus events are raised correctly through helpers")]
        [MemberData(nameof(GetEventHelperMethods), typeof(FocusEventDispatchExtensions))]
        public async Task CanRaiseEvents(MethodInfo helper)
        {
            var expected = new FocusEventArgs()
            {
                Type = "SOME TYPE"
            };

            await VerifyEventRaisesCorrectly(helper, expected);
        }
    }

}

