using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Egil.RazorComponents.Testing
{
    public class ComponentUnderTest : FragmentBase
    {
        public override Task SetParametersAsync(ParameterView parameters)
        {
            var result = base.SetParametersAsync(parameters);
            return result;
        }
    }
}
